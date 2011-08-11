using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes {
    public class Censor : Shape {
        private Bitmap view;
        private Bitmap previousScreenshot;

        private readonly CensorSettingsPanel sp;

        public Censor(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel)
            : base(editor, color, thickness, panel) {
            addHandle("start", p);
            addHandle("end", p);

            sp = (CensorSettingsPanel) panel;
            sp.SettingsChanged += shapeSettingsChanged;
        }

        public Censor(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel()) {
            sp = settingsPanel as CensorSettingsPanel;
            sp.SettingsChanged += shapeSettingsChanged;
        }

        private void shapeSettingsChanged(object sender, EventArgs e) {
            shapeChanged();
            editor.invalidateView();
        }

        public override string Name {
            get { return "Pixelized Rectangle"; }
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            writer.WriteAttributeString("blockSize", sp.trckSize.Value.ToString());
        }

        protected override void writeCustomXml(XmlTextWriter writer) {}

        public const string HINT_TEXT =
            "Drag censor rectangle to move it\r\n"
            + "Drag a handle to change censor size\r\n"
            + "Click \"Selected Tool Settings\" to change censor block size\r\n"
            + "Press \"Delete\" to remove censor block";


        public static string HintText {
            get { return HINT_TEXT; }
        }

        private class CensorSettingsPanel : ShapeSettingsPanel {
            public readonly TrackBar trckSize;
            private const string SHAPE_CENSOR_BLOCK_SIZE = "ShapeCensorBlockSize";

            public CensorSettingsPanel() {
                int val = getSettingValue(SHAPE_CENSOR_BLOCK_SIZE, 10);

                trckSize = new TrackBar
                           {
                               Minimum = 4,
                               Maximum = 20,
                               Value = val,
                               TickStyle = TickStyle.None
                           };

                SuspendLayout();

                // todo: this looks like shit. Have to reposition some time in the future :) 
                trckSize.Dock = DockStyle.Top;
                trckSize.TabStop = false;

                trckSize.ValueChanged += trckSize_ValueChanged;
                Padding = new Padding(12);
                Controls.Add(trckSize);

                Label l = new Label
                          {
                              Text = "Censor Block Size",
                              Dock = DockStyle.Top,
                              TextAlign = ContentAlignment.MiddleCenter
                          };

                Controls.Add(l);

                ResumeLayout(false);
            }

            private void trckSize_ValueChanged(object sender, EventArgs e) {
                saveSettingValue(SHAPE_CENSOR_BLOCK_SIZE, trckSize.Value);
                fireSettingsChangeNotification();
            }
        }

        public static ShapeSettingsPanel createSettingsPanel() {
            return new CensorSettingsPanel();
        }

        protected override ShapeView createView() {
            return new CensorView(this);
        }

        private Bitmap createCensorBitmap(RectangleF r, Image screenshot) {
            int w = (int) (r.Width + 0.5);
            int h = (int) (r.Height + 0.5);

            if (w < 1) w = 1;
            if (h < 1) h = 1;

            Bitmap bitmap = new Bitmap(w, h);

            Graphics graphics = Graphics.FromImage(bitmap);

            PointF[] pts = new PointF[3];
            pts[0] = new PointF(0, 0);
            pts[1] = new PointF(r.Width, 0);
            pts[2] = new PointF(0, r.Height);

            if (screenshot != null) {
                PointF origin = r.Location;
// ReSharper disable PossibleLossOfFraction
                origin.X += r.Width/2 + screenshot.Width/2;
                origin.Y += r.Height/2 + screenshot.Height/2;
// ReSharper restore PossibleLossOfFraction
                RectangleF srcRect = new RectangleF(origin.X - r.Width/2, origin.Y - r.Height/2, r.Width, r.Height);
                graphics.DrawImage(screenshot, pts, srcRect, GraphicsUnit.Pixel);

                ImageProcessor.Pixelate(bitmap, (short) sp.trckSize.Value, false);
            }

            return bitmap;
        }

        protected override void shapeChanged() {
            if (view == null) {
                return;
            }
            view.Dispose();
            view = null;
        }

        public override void colorChanged(Color color) {
            shapeChanged();
            base.colorChanged(color);
        }

        public override void thicknessChanged(int thickness) {
            shapeChanged();
            base.thicknessChanged(thickness);
        }

        public override void screenShotGrabbed(Bitmap screenshot) {
            shapeChanged();
            base.screenShotGrabbed(screenshot);
        }

        public override void screenshotZoomed(int zoomFactor) {
            shapeChanged();
            base.screenshotZoomed(zoomFactor);
        }

        public class CensorView : ShapeView {
            private readonly Censor c;

            public CensorView(Censor c) : base(c) {
                this.c = c;
            }

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters) {
                RectangleF r = getShapeRectangleF(c.Handles["start"], c.Handles["end"]);

                if (r.Width <= 0 || r.Height <= 0) {
                    return;
                }
                Brush b = new SolidBrush(Color.FromArgb(0x80, c.ShapeColor));

                Bitmap screenshot = c.editor.getAnnotatedScreenshot(c);
                if (c.view == null || screenshot != c.previousScreenshot) {
                    if (c.view != null)
                        c.view.Dispose();
                    c.view = c.createCensorBitmap(r, screenshot);
                    c.previousScreenshot = screenshot;
                }

                RectangleF rAbs = getShapeRectangle(c.Handles["start"], c.Handles["end"], parameters);
                g.DrawImage(c.view, rAbs);

                if (!c.editor.HaveScreenshot || !c.ShapeColor.Equals(c.ShapeColorNoHilight))
                    g.FillRectangle(b, rAbs);

                base.paint(g, drawHandles, parameters);
            }
        }

        public override bool isHit(PointF p) {
            RectangleF r = getShapeRectangleF(Handles["start"], Handles["end"]);

            PointF[] pt = new PointF[4];
            pt[0] = r.Location;
            pt[1] = new PointF(r.X + r.Width, r.Y);
            pt[2] = new PointF(r.X + r.Width, r.Y + r.Height);
            pt[3] = new PointF(r.X, r.Y + r.Height);

            return (isPointInPolygon(p, pt));
        }

        public override bool dragging(PointF p) {
            moveHandle("end", p);
            return false;
        }
    }
}