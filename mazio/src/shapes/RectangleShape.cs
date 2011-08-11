using System;
using System.Drawing;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes {
    public class RectangleShape : Shape {
        private readonly ShadowAndFillingSettingsPanel sp;

        public RectangleShape(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel)
            : base(editor, color, thickness, panel) {
            addHandle("start", p);
            addHandle("end", p);

            sp = (ShadowAndFillingSettingsPanel) panel;
        }

        public RectangleShape(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel()) {
            sp = (ShadowAndFillingSettingsPanel) settingsPanel;
            sp.chkUseShadow.Checked = bool.Parse(element.GetAttribute("shadow", ""));
            sp.chkFill.Checked = bool.Parse(element.GetAttribute("filled", ""));
        }

        public override string Name {
            get { return "Rectangle"; }
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            writer.WriteAttributeString("filled", sp.chkFill.Checked.ToString());
            writer.WriteAttributeString("shadow", sp.chkUseShadow.Checked.ToString());
        }

        protected override void writeCustomXml(XmlTextWriter writer) {}

        public const string HINT_TEXT =
            "Select a rectangle with a mouse\r\n"
            + "Drag rectangle to move it\r\n"
            + "Drag a handle to edit rectangle\r\n"
            + "Use \"Color\" and \"Width\" tools to change color and line width\r\n"
            + "Click \"Selected Tool Settings\" button for more options\r\n"
            + "Press \"Delete\" to remove rectangle";

        public static string HintText {
            get { return HINT_TEXT; }
        }

        public static ShapeSettingsPanel createSettingsPanel() {
            return new ShadowAndFillingSettingsPanel();
        }

        protected override ShapeView createView() {
            return new RectangleView(this);
        }

        public override bool isHit(PointF p) {
            PointF p1 = Handles["start"];
            PointF p2 = Handles["end"];

// ReSharper disable PossibleLossOfFraction
            float x11 = Math.Min(p1.X, p2.X) - (Thickness/2);
            float y11 = Math.Min(p1.Y, p2.Y) - (Thickness/2);
            float x12 = Math.Max(p1.X, p2.X) + (Thickness/2);
            float y12 = Math.Max(p1.Y, p2.Y) + (Thickness/2);

            float x21 = Math.Min(p1.X, p2.X) + (Thickness/2);
            float y21 = Math.Min(p1.Y, p2.Y) + (Thickness/2);
            float x22 = Math.Max(p1.X, p2.X) - (Thickness/2);
            float y22 = Math.Max(p1.Y, p2.Y) - (Thickness/2);
// ReSharper restore PossibleLossOfFraction

            PointF[] pOuter = new PointF[4];
            PointF[] pInner = new PointF[4];

            pOuter[0] = new PointF(x11, y11);
            pOuter[1] = new PointF(x11, y12);
            pOuter[2] = new PointF(x12, y12);
            pOuter[3] = new PointF(x12, y11);

            pInner[0] = new PointF(x21, y21);
            pInner[1] = new PointF(x21, y22);
            pInner[2] = new PointF(x22, y22);
            pInner[3] = new PointF(x22, y21);

            return (isPointInPolygon(p, pOuter) && (!isPointInPolygon(p, pInner)));
        }

        public override bool dragging(PointF p) {
            moveHandle("end", p);
            return false;
        }

        public class RectangleView : ShapeView {
            private readonly RectangleShape rs;

            public RectangleView(RectangleShape rs) : base(rs) {
                this.rs = rs;
            }

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters) {
                Brush b = shape.ShapeBrush;
                Pen pen = new Pen(b, scaledThickness(shape, parameters));

                Rectangle r = getShapeRectangle(shape.Handles["start"], shape.Handles["end"], parameters);

                if (rs.sp.chkUseShadow.Checked) {
                    if (rs.sp.chkFill.Checked) {
                        Rectangle r2 = new Rectangle(r.Location, r.Size);
                        r2.Inflate((int)(scaledThickness(shape, parameters) / 2), (int)(scaledThickness(shape, parameters) / 2));
                        Brush shadowBrush = new SolidBrush(Constants.SHADOW_COLOR);
                        g.FillRectangle(shadowBrush,
                                        new Rectangle(r2.X + (int)(Constants.SHADOW_X * parameters.ZoomRatio),
                                                      r2.Y + (int)(Constants.SHADOW_Y * parameters.ZoomRatio), r2.Width,
                                                      r2.Height));
                        shadowBrush.Dispose();
                    } else {
                        Pen shadowPen = new Pen(Constants.SHADOW_COLOR, scaledThickness(shape, parameters));
                        g.DrawRectangle(shadowPen,
                                        new Rectangle(r.X + (int)(Constants.SHADOW_X * parameters.ZoomRatio),
                                                      r.Y + (int)(Constants.SHADOW_Y * parameters.ZoomRatio), r.Width,
                                                      r.Height));
                        shadowPen.Dispose();
                    }
                }

                if (rs.sp.chkFill.Checked) {
                    r.Inflate((int)(scaledThickness(shape, parameters) / 2), (int)(scaledThickness(shape, parameters) / 2));
                    g.FillRectangle(b, r);
                } else {
                    g.DrawRectangle(pen, r);
                }

                base.paint(g, drawHandles, parameters);

                b.Dispose();
                pen.Dispose();
            }
        }
    }
}