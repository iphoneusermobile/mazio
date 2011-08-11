using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes
{
    class Pencil: Shape
	{
        private readonly List<PointF> points = new List<PointF>();

        private PointF prevPoint;

        private readonly ShadowSettingsPanel sp;

        public Pencil(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel)
            : base(editor, color, thickness, panel)
        {
            points.Add(p);
            prevPoint = p;

            sp = panel as ShadowSettingsPanel;
        }

        public Pencil(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel()) {
            sp = settingsPanel as ShadowSettingsPanel;

            sp.chkUseShadow.Checked = bool.Parse(element.GetAttribute("shadow", ""));
            XPathNodeIterator it = element.Clone().Select("points/point");
            while (it.MoveNext()) {
                float x = float.Parse(it.Current.GetAttribute("x", ""));
                float y = float.Parse(it.Current.GetAttribute("y", ""));
                points.Add(new PointF(x, y));
            }
        }

        public override string Name { get { return "Free-hand Line"; } }

        public const string HINT_TEXT =
            "Drag painted line to move it\r\n"
            + "Use \"Color\" and \"Width\" tools to change line color and width\r\n"
            + "Click \"Selected Tool Settings\" for more options\r\n"
            + "Press \"Delete\" to remove painted line";

        public static string HintText {
            get { return HINT_TEXT; }
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            writer.WriteAttributeString("shadow", sp.chkUseShadow.Checked.ToString());
        }

        protected override void writeCustomXml(XmlTextWriter writer) {
            writer.WriteStartElement("points");
            foreach (PointF p in points) {
                writer.WriteStartElement("point");
                writer.WriteAttributeString("x", p.X.ToString());
                writer.WriteAttributeString("y", p.Y.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public static ShapeSettingsPanel createSettingsPanel()
        {
            return new ShadowSettingsPanel();
        }

        protected override ShapeView createView()
        {
            return new PencilView(this);
        }

		public override bool isHit(PointF p)
        {
            foreach (PointF pt in points)
                if (getLengthSquared(p, pt) <= Thickness * Thickness / 4)
                    return true;
            return false;
        }

		public override bool dragging(PointF p)
        {
            PencilView.drawSegment(this, editor.CanvasGraphics, p, prevPoint, editor.CurrentViewParameters);

            prevPoint = p;
            points.Add(p);

            return true;
        }

        public override void moveTo(PointF to)
        {
            List<PointF> tmp = new List<PointF>();

            foreach (PointF p in points)
                tmp.Add(p);

            points.Clear();

            foreach (PointF p in tmp)
                points.Add(new PointF(p.X + to.X - moveFrom.X, p.Y + to.Y - moveFrom.Y));

            base.moveTo(to);
        }

        public class PencilView : ShapeView
        {
            private readonly Pencil pencil;
            public PencilView(Pencil p) : base(p) { pencil = p; }

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                Point[] pShadow = new Point[pencil.points.Count];
                Point[] pColor = new Point[pencil.points.Count];

                int i = 0;
                foreach (PointF p in pencil.points)
                {
                    pColor[i] = parameters.toAbsolute(p);

                    int offsx = (int)(Constants.SHADOW_X * parameters.ZoomRatio);
                    int offsy = (int)(Constants.SHADOW_Y * parameters.ZoomRatio);

                    pShadow[i] = new Point(pColor[i].X + offsx, pColor[i].Y + offsy);
                    ++i;
                }

                Brush b = shape.ShapeBrush;

                if (pencil.sp.chkUseShadow.Checked)
                {
                    Brush shadow = new SolidBrush(Constants.SHADOW_COLOR);

                    draw(g, pShadow, shadow, parameters);
                    shadow.Dispose();
                }

                draw(g, pColor, b, parameters);

                b.Dispose();
            }

            private void draw(Graphics g, Point[] pts, Brush b, ViewParameters parameters)
            {
                Pen pen = new Pen(b, scaledThickness(shape, parameters))
                          {
                              LineJoin = LineJoin.Round, 
                              StartCap = LineCap.Round, 
                              EndCap = LineCap.Round
                          };

                float o = scaledThickness(shape, parameters) / 2;
                if (pts.Length > 1)
                    g.DrawLines(pen, pts);
                else
                    g.FillEllipse(b, pts[0].X - o, pts[0].Y - o, scaledThickness(shape, parameters), scaledThickness(shape, parameters));

                pen.Dispose();
            }

            public static void drawSegment(Shape s, Graphics g, PointF start, PointF end, ViewParameters parameters)
            {
                Pen pen = new Pen(Color.FromArgb(0xff, s.ShapeColor), scaledThickness(s, parameters))
                          {
                              LineJoin = LineJoin.Round, 
                              StartCap = LineCap.Round, 
                              EndCap = LineCap.Round
                          };

                Point p1 = parameters.toAbsolute(start);
                Point p2 = parameters.toAbsolute(end);
                p1.Offset(parameters.Scrolls.X, parameters.Scrolls.Y);
                p2.Offset(parameters.Scrolls.X, parameters.Scrolls.Y);
                g.DrawLine(pen, p1, p2);

                pen.Dispose();
            }
        }

    }
}
