using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes {
    public class Oval : Shape {
        private readonly bool isOpaque;
        private readonly ShadowSettingsPanel sp;

        public Oval(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel)
            : this(editor, color, thickness, p, false, panel) {}

        public Oval(ScreenshotEditor editor, Color color, int thickness, PointF p, bool opaque, ShapeSettingsPanel panel)
            : base(editor, color, thickness, panel) {
            isOpaque = opaque;
            addHandle("start", p);
            addHandle("end", p);

            sp = (ShadowSettingsPanel) panel;
        }

        public Oval(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel()) {
            sp = settingsPanel as ShadowSettingsPanel;
            init(element);
        }

        internal Oval(ScreenshotEditor editor, XPathNavigator element, ShapeSettingsPanel sp, bool opaque) : base(editor, element, sp) {
            this.sp = settingsPanel as ShadowSettingsPanel;
            isOpaque = opaque;
            init(element);
        }

        private void init(XPathNavigator element) {
            if (sp is ShadowAndFillingSettingsPanel) {
                ((ShadowAndFillingSettingsPanel)sp).chkFill.Checked = bool.Parse(element.GetAttribute("filled", ""));
            }
            sp.chkUseShadow.Checked = bool.Parse(element.GetAttribute("shadow", ""));
        }

        public override string Name {
            get { return "Ellipse"; }
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            writer.WriteAttributeString("filled", ((ShadowAndFillingSettingsPanel) sp).chkFill.Checked.ToString());
            writer.WriteAttributeString("shadow", sp.chkUseShadow.Checked.ToString());
        }

        protected override void writeCustomXml(XmlTextWriter writer) {}

        public const string HINT_TEXT =
            "Select a oval with a mouse\r\n"
            + "Drag oval to move it\r\n"
            + "Drag a handle to edit oval\r\n"
            + "Use \"Color\" and \"Width\" tools to change color and line width\r\n"
            + "Click \"Selected Tool Settings\" button for more options\r\n"
            + "Press \"Delete\" to remove oval";

        public static string HintText {
            get { return HINT_TEXT; }
        }

        public static ShapeSettingsPanel createSettingsPanel() {
            return new ShadowAndFillingSettingsPanel();
        }

        protected override ShapeView createView() {
            return new OvalView(this);
        }

        public override bool isHit(PointF p) {
            PointF start = Handles["start"];
            PointF end = Handles["end"];

            RectangleF r = getShapeRectangleF(start, end);
// ReSharper disable PossibleLossOfFraction
            r.Inflate(Thickness/2, Thickness/2);
// ReSharper restore PossibleLossOfFraction

            bool filled = sp is ShadowAndFillingSettingsPanel && ((ShadowAndFillingSettingsPanel) sp).chkFill.Checked;
            return isPointInEllipse(new PointF(r.X + r.Width/2, r.Y + r.Height/2), r.Width/2, r.Height/2, p)
                   && (filled || isOpaque ||
                       !isPointInEllipse(new PointF(r.X + r.Width/2, r.Y + r.Height/2), (r.Width - 2*Thickness)/2,
                                         (r.Height - 2*Thickness)/2, p));
        }

        public override bool isValid() {
            PointF start = Handles["start"];
            PointF end = Handles["end"];

            return Math.Abs(start.X - end.X) > Thickness && Math.Abs(start.Y - end.Y) > Thickness;
        }

        public override bool dragging(PointF p) {
            moveHandle("end", p);
            return false;
        }

        public class OvalView : ShapeView {
            private readonly Oval o;

            public OvalView(Oval o) : base(o) {
                this.o = o;
            }

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters) {
                g.SmoothingMode = SmoothingMode.HighQuality;

                Brush b = shape.ShapeBrush;
                Pen pen = new Pen(b, scaledThickness(shape, parameters));

                Rectangle r = getShapeRectangle(shape.Handles["start"], shape.Handles["end"], parameters);

                bool filled = o.sp is ShadowAndFillingSettingsPanel && ((ShadowAndFillingSettingsPanel)o.sp).chkFill.Checked;

                if (o.sp.chkUseShadow.Checked) {

                    int offsx = (int)(Constants.SHADOW_X * parameters.ZoomRatio);
                    int offsy = (int)(Constants.SHADOW_Y * parameters.ZoomRatio);

                    if (filled) {
                        Rectangle r2 = new Rectangle(r.Location, r.Size);
                        r2.Inflate((int)(scaledThickness(shape, parameters) / 2), (int)(scaledThickness(shape, parameters) / 2));
                        Brush shadowBrush = new SolidBrush(Constants.SHADOW_COLOR);
                        g.FillEllipse(shadowBrush, new Rectangle(r2.X + offsx, r2.Y + offsy, r2.Width, r2.Height));
                        shadowBrush.Dispose();
                    } else {
                        Pen shadowPen = new Pen(Constants.SHADOW_COLOR, scaledThickness(shape, parameters));

                        g.DrawEllipse(shadowPen, new Rectangle(r.X + offsx, r.Y + offsy, r.Width, r.Height));
                        shadowPen.Dispose();
                    }
                }

                if (filled) {
                    r.Inflate((int) (scaledThickness(shape, parameters)/2), (int) (scaledThickness(shape, parameters)/2));
                    g.FillEllipse(b, r);
                } else {
                    g.DrawEllipse(pen, r);
                }

                base.paint(g, drawHandles, parameters);

                b.Dispose();
                pen.Dispose();
            }
        }
    }
}