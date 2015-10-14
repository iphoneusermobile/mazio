using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using mazio.util;
using System.Drawing.Drawing2D;

namespace XorLine {
    public class Line : IDisposable {
        private const int R2_XORPEN = 7;
        private const int PS_SOLID = 0;

        private readonly IntPtr invertingPen;

        public Line() {
            invertingPen = Win32.CreatePen(PS_SOLID, 1, 0x808080);
        }

        public struct Coords {
            public Coords(int x1, int y1, int x2, int y2) {
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
            }

            public int x1, y1, x2, y2;
        }

        private readonly Stack<Coords> linesStack = new Stack<Coords>();

        public void Restore(Graphics grp) {
            while (linesStack.Count > 0) {
                doDrawXORLine(grp, linesStack.Pop());
            }
        }

        public void DrawXORLine(Graphics grp, Coords lineCoords) {
            linesStack.Push(lineCoords);
            doDrawXORLine(grp, lineCoords);
        }

        private void doDrawXORLine(Graphics grp, Coords lineCoords) {
            IntPtr hdc = grp.GetHdc();
            Win32.SetROP2(hdc, R2_XORPEN);

            IntPtr oldPen = Win32.SelectObject(hdc, invertingPen);
            IntPtr point = new IntPtr();
            Win32.MoveToEx(hdc, lineCoords.x1, lineCoords.y1, point);
            Win32.LineTo(hdc, lineCoords.x2, lineCoords.y2);
            Win32.SelectObject(hdc, oldPen);
            grp.ReleaseHdc(hdc);
        }

        #region IDisposable Members

        public void Dispose() {
            Win32.DeleteObject(invertingPen);
        }

        #endregion
    }
}

namespace mazio {
    public sealed class Grabber : Form {
        private const int MagnifierMargin = 50;

        private const int INVALID_COORD = -10000;
        private int xBegin = INVALID_COORD;
        private int yBegin = INVALID_COORD;
        private int xCursor = INVALID_COORD;
        private int yCursor = INVALID_COORD;

        private Bitmap bmpScreenshot;
        private readonly BufferedGraphicsContext context;
        private readonly BufferedGraphics grafx;
        private bool suppressCursor;

        private Rectangle allScreensBounds;

        private const string TEXT =
            "Press and drag to grab region\r\n"
            + "Click to grab window\r\n"
            + "SHIFT-click to grab a full screen\r\n"
            + "CTRL-click to grab all screens\r\n"
            + "ESC to cancel";

        private bool shiftHeld;
        private bool ctrlHeld;

        private Point currentDisplayLocation = Point.Empty;

        private bool haveFullScreen;

        public bool HaveFullScreen {
            get { return haveFullScreen; }
        }

        public Grabber(Screen screen) {
            allScreensBounds = new Rectangle(0, 0, 0, 0);
            foreach (Screen s in Screen.AllScreens) {
                allScreensBounds = Rectangle.Union(allScreensBounds, s.Bounds);
            }
            Rectangle activeScreenBounds = screen == null ? allScreensBounds : screen.Bounds;

            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            DoubleBuffered = true;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            Size = activeScreenBounds.Size;
            Location = activeScreenBounds.Location;
            Cursor.Clip = activeScreenBounds;
            AutoScaleMode = AutoScaleMode.None;

            Paint += grabberPaint;
            MouseClick += onMouseClick;
            MouseDown += onMouseDown;
            KeyPress += onKeyPress;
            KeyDown += onKeyDown;
            KeyUp += onKeyUp;
            MouseMove += onMouseMove;
            Deactivate += grabberDeactivate;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            // Retrieves the BufferedGraphicsContext for the 
            // current application domain.
            context = BufferedGraphicsManager.Current;

            // Sets the maximum size for the primary graphics buffer
            // of the buffered graphics context for the application
            // domain.  Any allocation requests for a buffer larger 
            // than this will create a temporary buffered graphics 
            // context to host the graphics buffer.
            context.MaximumBuffer = new Size(Size.Width + 1, Size.Height + 1);

            // Allocates a graphics buffer the size of this form
            // using the pixel format of the Graphics created by 
            // the Form.CreateGraphics() method, which returns a 
            // Graphics object that matches the pixel format of the form.
            Graphics graphics = CreateGraphics();
            grafx = context.Allocate(graphics, new Rectangle(0, 0, Size.Width, Size.Height));


            Bitmap b = createCursorBitmap();
            Cursor = createCursor(b, 0, 0);
            b.Dispose();

            Point topLeftLocation = new Point(MagnifierMargin, MagnifierMargin);

            currentDisplayLocation = topLeftLocation;
        }

        private void onMouseClick(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) {
                return;
            }

            // re-paint the bitmap to erase cropping lines 
            // otherwise sometimes they show up in the screenshot
            suppressCursor = true;
            CreateGraphics().DrawImageUnscaled(bmpScreenshot, 0, 0);

            if (shiftHeld) {
                Screen activeScreen = Screen.FromPoint(Cursor.Position);
                xBegin = activeScreen.Bounds.Left;
                yBegin = activeScreen.Bounds.Top;
                xCursor = activeScreen.Bounds.Right;
                yCursor = activeScreen.Bounds.Bottom;
                haveFullScreen = true;
            } else if (ctrlHeld) {
                xBegin = allScreensBounds.Left;
                yBegin = allScreensBounds.Top;
                xCursor = allScreensBounds.Right;
                yCursor = allScreensBounds.Bottom;
                haveFullScreen = true;
            }

            DialogResult = DialogResult.OK;

            Close();
        }

        protected override void OnClosed(EventArgs e) {
            bmpScreenshot.Dispose();
            Cursor.Dispose();
            xorline.Dispose();

            Win32.ReleaseCapture();

            base.OnClosed(e);
        }

        public bool CheckForWindow() {
            if ((GrabWidth == 0) || (GrabHeight == 0)) {
                Win32.WINDOWINFO window = new Win32.WINDOWINFO();
                Win32.POINT pt = new Win32.POINT(GrabX, GrabY);
                IntPtr hwnd = Win32.WindowFromPoint(pt);
                do {
                    Win32.GetWindowInfo(hwnd, ref window);
                    if ((window.dwStyle & Win32.WS_CAPTION) == 0) {
                        hwnd = Win32.GetParent(hwnd);
                    }
                } while ((hwnd != IntPtr.Zero) && (window.dwStyle & Win32.WS_CAPTION) == 0);
                if (hwnd == IntPtr.Zero) {
                    return false;
                }

                Win32.SetForegroundWindow(hwnd);
                Thread.Sleep(1000);

                xBegin = window.rcWindow.left;
                xCursor = window.rcWindow.right;
                yBegin = window.rcWindow.top;
                yCursor = window.rcWindow.bottom;

                // bug #49
                int borderX = SystemInformation.FixedFrameBorderSize.Width + 2;
                int borderY = SystemInformation.FixedFrameBorderSize.Height + 2;
                if (((window.dwExStyle & Win32.WS_EX_DLGMODALFRAME) == Win32.WS_EX_DLGMODALFRAME)
                    || ((window.dwExStyle & Win32.WS_EX_TOOLWINDOW) == Win32.WS_EX_TOOLWINDOW)
                    || ((window.dwExStyle & Win32.WS_EX_PALETTEWINDOW) == Win32.WS_EX_PALETTEWINDOW)) {
                    xBegin = Math.Max(0, xBegin - borderX);
                    xCursor += borderX;
                    yBegin = Math.Max(0, yBegin - borderY);
                    yCursor += borderY;
                }
            }

            return true;
        }

        private void onMouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) {
                return;
            }

            Win32.SetCapture(Handle);

            xBegin = Location.X + e.X;
            yBegin = Location.Y + e.Y;
            xCursor = Location.X + e.X;
            yCursor = Location.Y + e.Y;
        }

        public int GrabX {
            get { return Math.Min(xBegin, xCursor); }
        }

        public int GrabY {
            get { return Math.Min(yBegin, yCursor); }
        }

        public int GrabWidth {
            get { return Math.Abs(xCursor - xBegin); }
        }

        public int GrabHeight {
            get { return Math.Abs(yCursor - yBegin); }
        }

        private void grabberPaint(object sender, PaintEventArgs e) {
            grafx.Render(e.Graphics);

            paintCoords(e.Graphics);
        }

        protected override void OnPaintBackground(PaintEventArgs e) {}

        protected override void OnShown(EventArgs e) {
            int w = Size.Width;
            int h = Size.Height;
            if (w < 1) {
                w = 1;
            }
            if (h < 1) {
                h = 1;
            }
            bmpScreenshot = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            Graphics.FromImage(bmpScreenshot).CopyFromScreen(
                Location.X,
                Location.Y,
                0, 0, new Size(w, h), CopyPixelOperation.SourceCopy);
            grafx.Graphics.DrawImageUnscaled(bmpScreenshot, 0, 0);
        }

        private readonly XorLine.Line xorline = new XorLine.Line();

        private void restoreScreenArea(Rectangle restore) {
            Invalidate(restore, false);
        }

        private void onMouseMove(object sender, MouseEventArgs e) {
            if (suppressCursor) {
                return;
            }

            int x = Location.X + e.X;
            int y = Location.Y + e.Y;

            int xCursorOld = xCursor;
            xCursor = x;
            int yCursorOld = yCursor;
            yCursor = y;

            if (xCursorOld != INVALID_COORD) {
                restoreScreenArea(new Rectangle(xCursorOld - Location.X, 0, 1, Size.Height));
            }
            restoreScreenArea(new Rectangle(e.X, 0, 1, Size.Height));

            if (yCursorOld != INVALID_COORD) {
                restoreScreenArea(new Rectangle(0, yCursorOld - Location.Y, Size.Width, 1));
            }
            restoreScreenArea(new Rectangle(0, e.Y, Size.Width, 1));

            if ((xBegin != INVALID_COORD) && (yBegin != INVALID_COORD)) {
                restoreScreenArea(new Rectangle(xBegin - Location.X, 0, 1, Size.Height));
                restoreScreenArea(new Rectangle(0, yBegin - Location.Y, Size.Width, 1));
            }

            xorline.Restore(grafx.Graphics);
            if ((xBegin != INVALID_COORD) && (yBegin != INVALID_COORD)) {
                xorline.DrawXORLine(grafx.Graphics,
                                    new XorLine.Line.Coords(xBegin - Location.X, 0, xBegin - Location.X, Size.Height));
                xorline.DrawXORLine(grafx.Graphics,
                                    new XorLine.Line.Coords(0, yBegin - Location.Y, Size.Width, yBegin - Location.Y));
            }
            if (xCursor != xBegin) {
                xorline.DrawXORLine(grafx.Graphics,
                                    new XorLine.Line.Coords(xCursor - Location.X, 0, xCursor - Location.X, Size.Height));
            }
            if (yCursor != yBegin) {
                xorline.DrawXORLine(grafx.Graphics,
                                    new XorLine.Line.Coords(0, yCursor - Location.Y, Size.Width, yCursor - Location.Y));
            }

            paintMagnifier();
            Invalidate();
        }

        private void paintCoords(Graphics g) {

            string cursorCoord = string.Format("{0}x{1}", xCursor, yCursor);
            Font f = new Font("Arial", 12, GraphicsUnit.Pixel);
            SizeF s = g.MeasureString(cursorCoord, f);
            Brush b = new SolidBrush(Color.White);
            Brush bb = new SolidBrush(Color.Black);

            GraphicsUtils.drawText(g, f, b, bb, cursorCoord, xCursor - s.Width, yCursor - s.Height);   
            if (xBegin != INVALID_COORD && yBegin != INVALID_COORD) {
                string beginCoord = string.Format("{0}x{1}", xBegin, yBegin);
                GraphicsUtils.drawText(g, f, b, bb, beginCoord, xBegin + 2, yBegin + 2);

                int w = Math.Abs(xCursor - xBegin) - 1;
                int h = Math.Abs(yCursor - yBegin) - 1;
                string grabSize = string.Format("size: {0}x{1}", w, h);
                s = g.MeasureString(grabSize, f);
                GraphicsUtils.drawText(
                    g, f, b, bb, grabSize, 
                    Math.Min(xCursor, xBegin) + (w - s.Width) / 2, 
                    Math.Min(yCursor, yBegin) + (h - s.Height) / 2);
            }
            b.Dispose();
            f.Dispose();
        }

        private void paintMagnifier() {
            if (!ConfigDialog.Instance.ShowGrabMagnifier) {
                return;
            }

            // save old InterpolationMode
            InterpolationMode mode = grafx.Graphics.InterpolationMode;
            // Set the InterpolationMode to NearestNeighbor to see the pixels clearly
            grafx.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            // settings
            const int pixelSize = 8;
            const int range = 10;

            // some shortcut variables
            const int shotSize = range*2;
            const int displaySize = shotSize*pixelSize;
            Point zeroPoint = new Point(0, 0);
            Point cursorPos = Cursor.Position;

            Rectangle primaryScreenSize = Screen.PrimaryScreen.Bounds;
            int locX = primaryScreenSize.Width - displaySize - MagnifierMargin;
            int locY = primaryScreenSize.Height - displaySize - MagnifierMargin;
            Point topLeftLocation = new Point(MagnifierMargin, MagnifierMargin);
            Point topRightLocation = new Point(locX, MagnifierMargin);
            Point bottomLeftLocation = new Point(MagnifierMargin, locY);
            Point bottomRightLocation = new Point(locX, locY);

            // The rectangle within the display to show the screenshot
            Rectangle display = new Rectangle(zeroPoint, new Size(displaySize, displaySize)) {Location = currentDisplayLocation};

            Rectangle displayWithMargin = new Rectangle(display.X - 50, display.Y - 50, display.Width + 100, display.Height + 100);
            // switch corners if xor-lines will get near display area
            if ((cursorPos.X > displayWithMargin.X && cursorPos.X < displayWithMargin.Right) ||
                (cursorPos.Y > displayWithMargin.Y && cursorPos.Y < displayWithMargin.Bottom)) {
                // clear old image on old location
                grafx.Graphics.DrawImage(bmpScreenshot, display, display, GraphicsUnit.Pixel);
                restoreScreenArea(display);

                if (currentDisplayLocation.Equals(topLeftLocation)) {
                    currentDisplayLocation = topRightLocation;
                } else if (currentDisplayLocation.Equals(topRightLocation)) {
                    currentDisplayLocation = bottomRightLocation;
                } else if (currentDisplayLocation.Equals(bottomRightLocation)) {
                    currentDisplayLocation = bottomLeftLocation;
                } else {
                    currentDisplayLocation = topLeftLocation;
                }
                display.Location = currentDisplayLocation;
            }

            // clear old image
            restoreScreenArea(display);

            // The final screenshot size and position
            Rectangle shot = new Rectangle(zeroPoint, new Size(shotSize, shotSize))
                             {
                                 Location = new Point(cursorPos.X - range, cursorPos.Y - range)
                             };

            // cut the screenshot and paint it scaled to the display
            grafx.Graphics.DrawImage(bmpScreenshot, display, shot, GraphicsUnit.Pixel);

            // paint xor-lines
            for (int lineY = (2*range - 1)*pixelSize/2; lineY < (2*range + 1)*pixelSize/2; lineY++) {
                xorline.DrawXORLine(grafx.Graphics,
                                    new XorLine.Line.Coords(
                                        display.Location.X,
                                        display.Location.Y + lineY,
                                        display.Location.X + displaySize,
                                        display.Location.Y + lineY));
            }

            for (int lineX = (2*range - 1)*pixelSize/2; lineX < (2*range + 1)*pixelSize/2; lineX++) {
                xorline.DrawXORLine(grafx.Graphics,
                                    new XorLine.Line.Coords(
                                        display.Location.X + lineX,
                                        display.Location.Y,
                                        display.Location.X + lineX,
                                        display.Location.Y + displaySize - pixelSize/2));
            }

            // paint border
            Pen border = new Pen(Brushes.Red) {Width = pixelSize, Alignment = PenAlignment.Inset};
            grafx.Graphics.DrawRectangle(border, display);
            border.Dispose();

            // restore old interpolation mode
            grafx.Graphics.InterpolationMode = mode;
        }

        private void onKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char) Keys.Escape) {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.ShiftKey) {
                shiftHeld = true;
            }
            if (e.KeyCode == Keys.ControlKey) {
                ctrlHeld = true;
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.ShiftKey) {
                shiftHeld = false;
            }
            if (e.KeyCode == Keys.ControlKey) {
                ctrlHeld = true;
            }
        }

        private void grabberDeactivate(object sender, EventArgs e) {
            // handle focus loss
            if (DialogResult == DialogResult.None /* not programmatically closing */
                && Visible /* not before shown*/) {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private Bitmap createCursorBitmap() {
            Brush brush = new SolidBrush(Color.Black);
            Brush yellowBrush = new SolidBrush(Constants.TEXT_BG_COLOR);
            Font font = new Font("Arial", 10, FontStyle.Bold);

            Size s = CreateGraphics().MeasureString(TEXT, font).ToSize();

            Bitmap b = new Bitmap(s.Width + 10, s.Height + 10);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(yellowBrush, 2, 2, s.Width + 5, s.Height + 5);
            g.DrawString(TEXT, font, brush, 5, 5);

            brush.Dispose();
            yellowBrush.Dispose();
            font.Dispose();

            return b;
        }

        // cursor ops from http://blog.paranoidferret.com/index.php/2008/01/30/csharp-tutorial-how-to-use-custom-cursors/
        public static Cursor createCursor(Bitmap bmp, int xHotSpot, int yHotSpot) {
            IntPtr ptr = bmp.GetHicon();
            Win32.IconInfo tmp = new Win32.IconInfo();
            Win32.GetIconInfo(ptr, ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            ptr = Win32.CreateIconIndirect(ref tmp);
            return new Cursor(ptr);
        }
    }
}