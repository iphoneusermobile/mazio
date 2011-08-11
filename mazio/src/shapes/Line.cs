using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes
{
	public class Line : Shape
	{
        private readonly ShadowSettingsPanel sp;

        public Line(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel)
            : base(editor, color, thickness, panel)
        {
            addHandle("start", p);
            addHandle("end", p);

            sp = (ShadowSettingsPanel) panel;
        }

	    public Line(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel()) {
	        sp = settingsPanel as ShadowSettingsPanel;
            sp.chkUseShadow.Checked = bool.Parse(element.GetAttribute("shadow", ""));
	    }

	    public override string Name { get { return "Straight Line"; } }

        public const string HINT_TEXT =
            "Select a line with a mouse\r\n"
            + "Drag line to move it\r\n"
            + "Drag a handle to edit line\r\n"
            + "Use \"Color\" and \"Width\" tools to change color and line width\r\n"
            + "Click \"Selected Tool Settings\" button for more options\r\n"
            + "Press \"Delete\" to remove line";

        public static string HintText {
            get { return HINT_TEXT; }
        }

	    protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
	        writer.WriteAttributeString("shadow", sp.chkUseShadow.Checked.ToString());
	    }

	    protected override void writeCustomXml(XmlTextWriter writer) {}

	    public static ShapeSettingsPanel createSettingsPanel()
        {
            return new ShadowSettingsPanel();
        }

        protected override ShapeView createView()
        {
            return new LineView(this);
        }

		public override bool isHit(PointF p)
        {
            PointF ps = Handles["start"];
            PointF pe = Handles["end"];

            int length = getLength(ps, pe);
            double arc = getArc(ps, pe);

            PointF[] points = new PointF[4];
// ReSharper disable PossibleLossOfFraction
            points[0] = rotate(ps, arc, new PointF(ps.X, ps.Y - (Thickness / 2)));
            points[1] = rotate(ps, arc, new PointF(ps.X + length, ps.Y - (Thickness / 2)));
            points[2] = rotate(ps, arc, new PointF(ps.X + length, ps.Y + (Thickness / 2)));
            points[3] = rotate(ps, arc, new PointF(ps.X, ps.Y + (Thickness / 2)));
// ReSharper restore PossibleLossOfFraction

            return isPointInPolygon(p, points);
        }

		public override bool dragging(PointF p)
        {
            moveHandle("end", p);
            Debug.WriteLine("dragging line to " + p);
            return false;
        }

        public class LineView : ShapeView
        {
            private readonly Line l;

            public LineView(Line l) : base(l) { this.l = l; }

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                Brush brush = shape.ShapeBrush;
                Pen pen = new Pen(brush, scaledThickness(shape, parameters))
                          {
                              StartCap = LineCap.Round, 
                              EndCap = LineCap.Round
                          };

                Point start = parameters.toAbsolute(shape.Handles["start"]);
                Point end = parameters.toAbsolute(shape.Handles["end"]);

                if (l.sp.chkUseShadow.Checked)
                {
                    Pen shadowPen = new Pen(Constants.SHADOW_COLOR, scaledThickness(shape, parameters))
                                    {
                                        StartCap = LineCap.Round, 
                                        EndCap = LineCap.Round
                                    };

                    int offsx = (int)(Constants.SHADOW_X * parameters.ZoomRatio);
                    int offsy = (int)(Constants.SHADOW_Y * parameters.ZoomRatio);
                    g.DrawLine(shadowPen, start.X + offsx, start.Y + offsy,
                        end.X + offsx, end.Y + offsy);
                    shadowPen.Dispose();
                }
                g.DrawLine(pen, start, end);

                pen.Dispose();
                brush.Dispose();

                base.paint(g, drawHandles, parameters);
            }
        }
	}
}
