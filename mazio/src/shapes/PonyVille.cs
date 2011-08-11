using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes {
    public class PonyVille : Shape {
        public class Element {
            public RectangleF r;
            public Bitmap b;

            public Element(RectangleF r, Bitmap b) {
                this.r = r;
                this.b = b;
            }
        }

        private readonly List<Element> elements = new List<Element>();

        private PointF prevPoint;

        private static readonly Random random = new Random();

        private const int MIN_THICKNESS = 8;

        private int PonyThickness {
            get { return Math.Max(2*Thickness, MIN_THICKNESS); }
        }

        private Bitmap getBitmap() {
            if (psp.radioHeart.Checked) {
                return Properties.Resources.emblem_favorite;
            }
            return psp.radioStar.Checked ? Properties.Resources.yellow_star : null;
        }

        private Size RandomSize {
            get {
                int w = Math.Max(4, random.Next(PonyThickness/2, PonyThickness*2));
                return new Size(w, w);
            }
        }

        public override string Name {
            get { return "Stream of Images"; }
        }

        public const string HINT_TEXT =
            "Drag ponyville to move it\r\n"
            + "Click \"Selected Tool Settings\" to change ponyville type\r\n"
            + "Press \"Delete\" to remove ponyville";


        public static string HintText {
            get { return HINT_TEXT; }
        }

        private int prevThickness;

        private bool isBeingAdded = true;

        private PonySettingsPanel psp;

        public PonyVille(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel)
            : base(editor, color, thickness, panel) {

            psp = (PonySettingsPanel)panel;
            psp.SettingsChanged += settingsChanged;

            RectangleF rect = new RectangleF(p, RandomSize);
            prevPoint = p;

            prevThickness = thickness;

            Element e = new Element(rect, PonyVilleView.generateElementBitmap(this, rect.Size, editor.CurrentViewParameters));
            elements.Add(e);
            PonyVilleView.drawElement(editor.CanvasGraphics, e, editor.CurrentViewParameters);
        }

        public PonyVille(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel()) {
            psp = (PonySettingsPanel) settingsPanel;
            psp.SettingsChanged += settingsChanged;
            bool heart = element.GetAttribute("ponytype", "").Equals("heart");
            psp.radioHeart.Checked = heart;
            psp.radioStar.Checked = !heart;

            prevThickness = Thickness;

            XPathNodeIterator it = element.Clone().Select("ponies/pony");
            while (it.MoveNext()) {
                float x = float.Parse(it.Current.GetAttribute("x", ""));
                float y = float.Parse(it.Current.GetAttribute("y", ""));
                int w = int.Parse(it.Current.GetAttribute("w", ""));
                int h = int.Parse(it.Current.GetAttribute("h", ""));
                RectangleF rect = new RectangleF(x, y, w, h);
                elements.Add(new Element(rect, PonyVilleView.generateElementBitmap(this, rect.Size, editor.CurrentViewParameters)));
            }
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            writer.WriteAttributeString("ponytype", psp.radioHeart.Checked ? "heart" : "star");
        }

        protected override void writeCustomXml(XmlTextWriter writer) {
            writer.WriteStartElement("ponies");
            foreach (Element e in elements) {
                writer.WriteStartElement("pony");
                writer.WriteAttributeString("x", e.r.X.ToString());
                writer.WriteAttributeString("y", e.r.Y.ToString());
                writer.WriteAttributeString("w", e.r.Width.ToString());
                writer.WriteAttributeString("h", e.r.Height.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private void settingsChanged(object sender, EventArgs e) {
            regenerateBitmaps();
            editor.invalidateView();
        }

        private class PonySettingsPanel : ShapeSettingsPanel {
            public readonly RadioButton radioHeart;
            public readonly RadioButton radioStar;
            private const string SHAPE_PONY_TYPE = "ShapePonyType";

            public PonySettingsPanel() {
                radioHeart = new RadioButton();
                radioStar = new RadioButton();

                radioStar.Click += switchBitmap;
                radioHeart.Click += switchBitmap;

                bool isHeart = getSettingValue(SHAPE_PONY_TYPE, 1) == 1;

                if (isHeart) {
                    radioHeart.Checked = true;
                } else {
                    radioStar.Checked = true;
                }

                GroupBox group = new GroupBox();

                group.SuspendLayout();
                SuspendLayout();

                radioHeart.AutoSize = true;
                radioHeart.Location = new Point(50, 32);
                radioHeart.TabStop = false;
                radioHeart.Appearance = Appearance.Button;
                radioHeart.FlatStyle = FlatStyle.Flat;
                radioHeart.FlatAppearance.BorderSize = 0;
                radioHeart.FlatAppearance.CheckedBackColor = SystemColors.ControlDark;
                radioHeart.Padding = new Padding(6);
                radioHeart.Image = Properties.Resources.emblem_favorite_32;

                radioStar.AutoSize = true;
                radioStar.Location = new Point(50, 92);
                radioStar.TabStop = false;
                radioStar.Appearance = Appearance.Button;
                radioStar.FlatStyle = FlatStyle.Flat;
                radioStar.FlatAppearance.BorderSize = 0;
                radioStar.FlatAppearance.CheckedBackColor = SystemColors.ControlDark;
                radioStar.Padding = new Padding(6);
                radioStar.Image = Properties.Resources.yellow_star_32;

                group.Controls.Add(radioStar);
                group.Controls.Add(radioHeart);
                group.Dock = DockStyle.Top;
                group.AutoSize = true;
                group.Location = new Point(0, 0);
                group.TabStop = false;
                group.Text = "Image to Draw";

                Padding = new Padding(12);
                Controls.Add(group);

                group.ResumeLayout(false);
                group.PerformLayout();
                ResumeLayout(false);
            }

            private void switchBitmap(object sender, EventArgs e) {
                saveSettingValue(SHAPE_PONY_TYPE, radioHeart.Checked ? 1 : 0);
                fireSettingsChangeNotification();
            }
        }

        public static ShapeSettingsPanel createSettingsPanel() {
            return new PonySettingsPanel();
        }

        public override void deactivate() {
            isBeingAdded = false;
            base.deactivate();
        }

        private void regenerateBitmaps() {
            foreach (Element e in elements) {
                RectangleF r = e.r;
                r.Width = e.r.Width*Thickness/prevThickness;
                r.Height = e.r.Height*Thickness/prevThickness;
                e.r = r;
                e.b = PonyVilleView.generateElementBitmap(this, r.Size, editor.CurrentViewParameters);
            }
        }

        public override void thicknessChanged(int thickness) {
            if (Editing || isBeingAdded) {
                regenerateBitmaps();
                prevThickness = thickness;
            }

            base.thicknessChanged(thickness);
        }

        public override bool isHit(PointF p) {
            foreach (Element e in elements) {
                PointF pMiddle = new PointF(p.X - e.r.Width/2, p.Y - e.r.Height/2);
                if (getLengthSquared(pMiddle, e.r.Location) <= e.r.Width*e.r.Height/4) {
                    return true;
                }
            }
            return false;
        }

        public override bool dragging(PointF p) {
            double len = getLength(prevPoint, p);

            int n = (int) (len/PonyThickness);

            if (n > 0) {
                double arc = getArc(prevPoint, p);

                PointF lastPoint = prevPoint;
                Bitmap bitmap = getBitmap();

                for (int i = 1; i <= n; ++i) {
                    PointF pt = new PointF(prevPoint.X + (float) Math.Cos(arc)*(PonyThickness*i + random.Next(-8, 8)),
                                           prevPoint.Y + (float) Math.Sin(arc)*(PonyThickness*i + random.Next(-8, 8)));
                    RectangleF rect = new RectangleF(pt, RandomSize);
                    Element e = new Element(rect, PonyVilleView.generateElementBitmap(this, rect.Size, editor.CurrentViewParameters));
                    elements.Add(e);
                    PonyVilleView.drawElement(editor.CanvasGraphics, e, editor.CurrentViewParameters);
                    lastPoint = pt;
                }
                bitmap.Dispose();

                prevPoint = lastPoint;
            }

            return true;
        }

        public override void moveTo(PointF to) {
            List<Element> tmp = new List<Element>();

            foreach (Element e in elements) {
                tmp.Add(e);
            }

            elements.Clear();

            foreach (Element e in tmp) {
                elements.Add(new Element(
                                 new RectangleF(
                                     new PointF(e.r.Location.X + to.X - moveFrom.X, e.r.Location.Y + to.Y - moveFrom.Y), e.r.Size), e.b));
            }

            base.moveTo(to);
        }

        protected override ShapeView createView() {
            return new PonyVilleView(this);
        }

        public class PonyVilleView : ShapeView {
            private readonly PonyVille p;

            public PonyVilleView(PonyVille p) : base(p) {
                this.p = p;
            }

            private ViewParameters prevParams;

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters) {
                if (parameters != prevParams) {
                    resizeBitmaps(parameters);
                    prevParams = parameters;
                }

                Bitmap bitmap = p.getBitmap();
                Brush brush = new SolidBrush(Color.FromArgb(0x80, shape.ShapeColor));
                foreach (Element e in p.elements) {
                    Point ptAbs = parameters.toAbsolute(e.r.Location);
                    if (!shape.ShapeColor.Equals(shape.ShapeColorNoHilight)) {
                        g.FillEllipse(brush, ptAbs.X, ptAbs.Y, e.r.Width*parameters.ZoomRatio, e.r.Height*parameters.ZoomRatio);
                    }
                    g.DrawImageUnscaled(e.b, ptAbs);
                }
                brush.Dispose();
                bitmap.Dispose();
            }

            private void resizeBitmaps(ViewParameters parameters) {
                foreach (Element e in p.elements) {
                    e.b = generateElementBitmap(p, e.r.Size, parameters);
                }
            }

            public static void drawElement(Graphics g, Element e, ViewParameters p) {
                Point ptAbs = p.toAbsolute(e.r.Location);
                ptAbs.Offset(p.Scrolls.X, p.Scrolls.Y);
                g.DrawImageUnscaled(e.b, ptAbs);
            }

            public static Bitmap generateElementBitmap(PonyVille ponyVille, SizeF s, ViewParameters p) {
                int w = (int) (s.Width*p.ZoomRatio);
                int h = (int) (s.Height*p.ZoomRatio);
                if (w == 0) {
                    w = 1;
                }
                if (h == 0) {
                    h = 1;
                }

                Bitmap b = new Bitmap(w, h);
                Bitmap pony = ponyVille.getBitmap();
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                Point[] pts = new Point[3];
                pts[0] = new Point(0, 0);
                pts[1] = new Point(w, 0);
                pts[2] = new Point(0, h);

                g.DrawImage(pony, pts);

                pony.Dispose();

                return b;
            }
        }
    }
}