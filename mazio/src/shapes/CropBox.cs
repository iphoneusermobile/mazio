using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.Xml.XPath;
using mazio.util;

namespace mazio.shapes {
    public class CropBox : Shape {
        public const string HINT_TXT = "Drag red lines to crop\r\n  or select crop area\r\nClick \"Selected Tool Settings\" to set crop area proportions";

        //        private static Color getOppositeColor(Color c)
//        {
//            double h = c.GetHue();
//            double s = c.GetSaturation();
//            double b = c.GetBrightness();
//
//            h = h > 180 ? h - 180 : h + 180;
//            b = b > 0.5 ? b - Constants.HILIGHT_BOOST : b + Constants.HILIGHT_BOOST;
//            h /= 360;
//
//            return hsl2Rgb(h, s, b);
//        }

//        private static Color getEditShadowColor(Color c)
//        {
//            double h = c.GetHue();
//            double s = c.GetSaturation();
//            double b = c.GetBrightness();
//
//            b = b > 0.5 ? b - Constants.HILIGHT_BOOST : b + Constants.HILIGHT_BOOST;
//            h /= 360;
//
//            return hsl2Rgb(h, s, b);
//        }

        public class CropShapeSettingsPanel : ShapeSettingsPanel {
            internal readonly CropSettingsPanel panel;

            public CropShapeSettingsPanel() {
                SuspendLayout();
                panel = new CropSettingsPanel {Dock = DockStyle.Fill};
                Controls.Add(panel);
                ResumeLayout();
            }
        }

        private CropShapeSettingsPanel sp;

        public CropBox(ScreenshotEditor editor, bool editing, ShapeSettingsPanel panel)
            : base(editor, Color.Black, 20, panel)
            //: base(editor, getOppositeColor(editor.BackgroundColor), 20, null)
        {
            rightHit = false;

            Size s = editor.Screenshot.Size;
            //s.Width += ConfigDialog.Instance.GrabMargin;
            //s.Height += ConfigDialog.Instance.GrabMargin;
            int w = s.Width;
            int h = s.Height;

// ReSharper disable PossibleLossOfFraction
            addHandle("start", new PointF(-w/2, -h/2));
            addHandle("end", new PointF(w/2, h/2));
// ReSharper restore PossibleLossOfFraction

            Editing = editing;

            sp = (CropShapeSettingsPanel)panel;
        }

        public CropBox(ScreenshotEditor editor, XPathNavigator element, bool editing) : base(editor, element, createSettingsPanel()) {
            sp = settingsPanel as CropShapeSettingsPanel;
            Editing = editing;
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {}
        protected override void writeCustomXml(XmlTextWriter writer) {}

        public override bool Editing {
            get { return base.Editing; }
            set {
                base.Editing = value;

                sp = ShapeSettings.Instance.Contents as CropShapeSettingsPanel;
                if (sp == null) return;
                if (value) {
                    sp.panel.ConstraintsChanged += panel_ConstraintsChanged;
                } else {
                    sp.panel.ConstraintsChanged -= panel_ConstraintsChanged;
                }
            }
        }

        private PointF getHandlePositionFromProportions(string handleName, PointF to, bool? vertical) {
            PointF basePoint = handleName.Equals("end") ? Handles["start"] : Handles["end"];
            float ratio = 1;
            if (sp == null) return to;
            switch (sp.panel.Constraint) {
                case CropSettingsPanel.ConstraintType.CONSTRAIN_NONE:
                    return to;
                case CropSettingsPanel.ConstraintType.CONSTRAIN_CUSTOM:
                    Size s = sp.panel.CustomProportions;
                    ratio = (float) s.Width/s.Height;
                    break;
                case CropSettingsPanel.ConstraintType.CONSTRAIN_4x3:
                    ratio = 4f/3;
                    break;
                case CropSettingsPanel.ConstraintType.CONSTRAIN_16x9:
                    ratio = 16f/9;
                    break;
            }
            float moveRatio = Math.Abs(to.X - basePoint.X) / (Math.Abs(to.Y - basePoint.Y) + 0.00000001f);
//            Debug.WriteLine("move ratio: " + moveRatio);
            float newX = to.X;
            float newY = to.Y;
            if (vertical.HasValue) {
                if (!vertical.Value) {
                    newY = basePoint.Y + Math.Sign(to.Y - basePoint.Y) * Math.Abs(newX - basePoint.X) / ratio;
                } else {
                    newX = basePoint.X + Math.Sign(to.X - basePoint.X) * Math.Abs(newY - basePoint.Y) * ratio;
                }   
            } else {
                 if (ratio < moveRatio) {
                    newY = basePoint.Y + Math.Sign(to.Y - basePoint.Y) * Math.Abs(newX - basePoint.X) / ratio;
                } else {
                    newX = basePoint.X + Math.Sign(to.X - basePoint.X) * Math.Abs(newY - basePoint.Y) * ratio;
                }   
            }
//            Debug.WriteLine("new point: " + new PointF(newX, newY));
            return new PointF(newX, newY);
        }

        private void panel_ConstraintsChanged(object sender, EventArgs e) {
            PointF newEnd = getHandlePositionFromProportions("end", Handles["end"], null);
            moveHandle("end", newEnd);
            editor.invalidateView();
        }

        protected override ShapeView createView() {
            return new CropBoxView(this);
        }

        public override string Name {
            get { return "Cropping Frame"; }
        }

//        private bool wasHit;
        private bool leftHit;
        private bool rightHit;
        private bool topHit;
        private bool bottomHit;

        public override bool isHit(PointF p) {
// ReSharper disable PossibleLossOfFraction
            leftHit = editor.CurrentViewParameters.ZoomRatio*Math.Abs(p.X - Handles["start"].X) <= (Thickness/2);
            topHit = editor.CurrentViewParameters.ZoomRatio*Math.Abs(p.Y - Handles["start"].Y) <= (Thickness/2);
            rightHit = editor.CurrentViewParameters.ZoomRatio*Math.Abs(p.X - Handles["end"].X) <= (Thickness/2);
            bottomHit = editor.CurrentViewParameters.ZoomRatio*Math.Abs(p.Y - Handles["end"].Y) <= (Thickness/2);
// ReSharper restore PossibleLossOfFraction
//            wasHit = leftHit || topHit || rightHit || bottomHit;
            Cursor c = Cursors.Default;
            if (leftHit || rightHit)
                c = Cursors.SizeWE;
            if (topHit || bottomHit)
                c = Cursors.SizeNS;
            editor.setCursor(c);
            return true;
        }

        private bool selectArea;

        public override bool dragging(PointF point) {
            if (Editing) {
                PointF ps = Handles["start"];
                PointF pe = Handles["end"];

                if (selectArea) {
                    moveHandle("end", getHandlePositionFromProportions("end", point, null));
                } else {
                    PointF pResult;
                    if (leftHit) {
                        pResult = new PointF(Math.Min(pe.X, point.X), ps.Y);
                        moveHandle("start", getHandlePositionFromProportions("start", pResult, false));
                    } else if (rightHit) {
                        pResult = new PointF(Math.Max(ps.X, point.X), pe.Y);
                        moveHandle("end", getHandlePositionFromProportions("end", pResult, false));
                    } else if (topHit) {
                        pResult = new PointF(ps.X, Math.Min(pe.Y, point.Y));
                        moveHandle("start", getHandlePositionFromProportions("start", pResult, true));
                    } else if (bottomHit) {
                        pResult = new PointF(pe.X, Math.Max(ps.Y, point.Y));
                        moveHandle("end", getHandlePositionFromProportions("end", pResult, true));
                    } else {
                        moveHandle("start", point);
                        moveHandle("end", point);
                        selectArea = true;
                    }
                }
            }
            return false;
        }

        public override void finished() {
            selectArea = false;
            if (Handles["start"].X > Handles["end"].X) {
                float x = Handles["start"].X;
                moveHandle("start", new PointF(Handles["end"].X, Handles["start"].Y));
                moveHandle("end", new PointF(x, Handles["end"].Y));
            }
            if (Handles["start"].Y > Handles["end"].Y) {
                float y = Handles["start"].Y;
                moveHandle("start", new PointF(Handles["start"].X, Handles["end"].Y));
                moveHandle("end", new PointF(Handles["end"].X, y));
            }
        }

        public void cropTight() {
            Size bitmapSize = editor.Screenshot.Size;
            int x = bitmapSize.Width/2;
            int y = bitmapSize.Height/2;
            moveHandle("start", new PointF(-x, -y));
            moveHandle("end", new PointF(x, y));
        }

        public RectangleF CropRectangle {
            get { return getShapeRectangleF(Handles["start"], Handles["end"]); }
        }

        public PointF MaxOffset {
            get {
                float xs = Math.Abs(Handles["start"].X);
                float xe = Math.Abs(Handles["end"].X);
                float ys = Math.Abs(Handles["start"].Y);
                float ye = Math.Abs(Handles["end"].Y);
                return new PointF(Math.Max(xs, xe), Math.Max(ys, ye));
            }
        }

        public static string HintText {
            get { return HINT_TXT; }
        }

        public class CropBoxView : ShapeView {

            private readonly CropBox crop;

            public CropBoxView(CropBox c) : base(c) {
                crop = c;
            }

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters) {
                Brush shadow =
                    new SolidBrush(crop.Editing ? Color.FromArgb(0xb0, crop.ShapeColor) : SystemColors.Control);

                Point p1 = parameters.toAbsolute(crop.Handles["start"]);
                Point p2 = parameters.toAbsolute(crop.Handles["end"]);
                if (p1.X > p2.X) {
                    int x = p2.X;
                    p2.X = p1.X;
                    p1.X = x;
                }
                if (p1.Y > p2.Y) {
                    int y = p2.Y;
                    p2.Y = p1.Y;
                    p1.Y = y;
                }

                Rectangle[] rects = new Rectangle[4];
                rects[0] = new Rectangle(0, 0, parameters.CanvasSize.Width, p1.Y);
                rects[1] = new Rectangle(0, crop.Editing ? p1.Y : 0, p1.X,
                                         crop.Editing ? (p2.Y - p1.Y) : parameters.CanvasSize.Height);
                rects[2] = new Rectangle(0, p2.Y, parameters.CanvasSize.Width, parameters.CanvasSize.Height - p2.Y);
                rects[3] = new Rectangle(p2.X, crop.Editing ? p1.Y : 0,
                                         parameters.CanvasSize.Width - p2.X,
                                         crop.Editing ? (p2.Y - p1.Y) : parameters.CanvasSize.Height);
                g.FillRectangles(shadow, rects);

                if (crop.Editing) {
                    Pen penActive = new Pen(Color.Red, 1);

                    g.DrawLine(penActive, 0, p1.Y, parameters.CanvasSize.Width, p1.Y);
                    g.DrawLine(penActive, p1.X, 0, p1.X, parameters.CanvasSize.Height);
                    g.DrawLine(penActive, 0, p2.Y, parameters.CanvasSize.Width, p2.Y);
                    g.DrawLine(penActive, p2.X, 0, p2.X, parameters.CanvasSize.Height);

#if false
                    if (ConfigDialog.Instance.ShowUsageHints) {
                        Font f = new Font("Arial", 10, FontStyle.Bold);
                        Size s = g.MeasureString(HINT_TXT, f).ToSize();
                        Brush b1 = new SolidBrush(Color.FromArgb(0x80, crop.ShapeColor));
                        Brush b2 = new SolidBrush(Color.Red);
                        int x1 = ((parameters.Viewport.Width - s.Width)/2) - 5;
                        const int y1 = 0;
                        g.FillRectangle(b1, x1, y1, s.Width + 10, s.Height + 10);
                        g.DrawString(HINT_TXT, f, b2, x1 + 5, y1 + 5);
                        b1.Dispose();
                        b2.Dispose();
                        f.Dispose();
                    }
#endif

                    Font f = new Font("Arial", 12, GraphicsUnit.Pixel);
                    Brush fgb = new SolidBrush(Color.White);
                    Brush bgb = new SolidBrush(Color.Red);
                    RectangleF rectangle = crop.CropRectangle;
                    int width = (int) (rectangle.Width);
                    int height = (int) (rectangle.Height);
                    string text = string.Format("{0}x{1}", width, height);
                    SizeF s = g.MeasureString(text, f);
                    int x = (int) (p1.X + (p2.X - p1.X - s.Width)/2);
                    int y = (int)(p1.Y + (p2.Y - p1.Y - s.Height) / 2);
                    GraphicsUtils.drawText(g, f, fgb, bgb, text, x, y);
                    f.Dispose();
                    fgb.Dispose();
                    bgb.Dispose();
                    penActive.Dispose();
                } else {
                    Pen outline = new Pen(Color.Black, 1);
                    g.DrawRectangle(outline, p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
                    outline.Dispose();
                }

                shadow.Dispose();
            }
        }

        public static ShapeSettingsPanel createSettingsPanel() {
            return new CropShapeSettingsPanel();
        }
    }
}