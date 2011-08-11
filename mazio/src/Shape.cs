using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace mazio {
    public abstract class Shape : SettingsChangeListener {

        public const string DEFAULT_HINT_TEXT = 
            "Select a shape with a mouse\r\n"
            + "Drag shape to move it\r\n"
            + "Drag a handle to edit shape\r\n"
            + "Use \"Color\" and \"Width\" tools to change color and line width\r\n"
            + "Click \"Selected Tool Settings\" button for more options\r\n"
            + "Press \"Delete\" to remove shape";


        protected ScreenshotEditor editor;

        protected PointF moveFrom;

        private int lastThickness;

        private Color hilightColor;
        private Color lastColor;
        private bool hilight;

        private Dictionary<string, PointF> handles;
        private bool doDrawHandles = true;

        private string selectedHandleName;

        protected abstract void writeCustomXmlAttributes(XmlTextWriter writer);
        protected abstract void writeCustomXml(XmlTextWriter writer);

        public void toXml(XmlTextWriter writer) {
            writer.WriteStartElement("shape");
            writer.WriteAttributeString("type", GetType().Name);
            writer.WriteAttributeString("color.a", ShapeColorNoHilight.A.ToString());
            writer.WriteAttributeString("color.r", ShapeColorNoHilight.R.ToString());
            writer.WriteAttributeString("color.g", ShapeColorNoHilight.G.ToString());
            writer.WriteAttributeString("color.b", ShapeColorNoHilight.B.ToString());
            writer.WriteAttributeString("thickness", Thickness.ToString());
            writeCustomXmlAttributes(writer);
            writer.WriteStartElement("handles");
            foreach (KeyValuePair<string, PointF> handle in handles) {
                writer.WriteStartElement("handle");
                writer.WriteAttributeString("name", handle.Key);
                writer.WriteAttributeString("x", handle.Value.X.ToString());
                writer.WriteAttributeString("y", handle.Value.Y.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writeCustomXml(writer);

            writer.WriteEndElement();
        }

        public string SelectedHandle {
            get { return selectedHandleName; }
            set {
                if (value == null)
                    selectedHandleName = null;
                else if (handles.ContainsKey(value))
                    selectedHandleName = value;
            }
        }

        public Color ShapeColor {
            get { return hilight && useHilights ? hilightColor : ShapeColorNoHilight; }
            set { setColor(value); }
        }

        protected ShapeSettingsPanel settingsPanel;

        public ShapeSettingsPanel SettingsPanel {
            get { return settingsPanel; }
        }

        protected static ShapeSettingsPanel createDefaultSettingsPanel() {
            ShapeSettingsPanel p = new ShapeSettingsPanel();

            Label l = new Label
                      {
                          Dock = DockStyle.Fill,
                          TextAlign = ContentAlignment.MiddleCenter,
                          Text = "No settings\r\nfor this tool"
                      };

            p.Controls.Add(l);
            return p;
        }

        public Brush ShapeBrush {
            get { return new HatchBrush(HatchStyle.LargeCheckerBoard, ShapeColor, ShapeColorNoHilight); }
        }

        public abstract string Name { get; }

        public Color ShapeColorNoHilight { get; private set; }

        private static bool useHilights = true;

        public static bool UseHilights {
            set { useHilights = value; }
            get { return useHilights; }
        }

        public virtual void colorChanged(Color c) {}
        public virtual void thicknessChanged(int thickness) {}
        public virtual void screenShotGrabbed(Bitmap screenshot) {}

        public virtual void screenshotZoomed(int zoomFactor) {
            editor.invalidateView();
        }

        public virtual void canvasResized(Size newSize) {}

        public int Thickness { get; set; }

        public abstract bool isHit(PointF p);
        public abstract bool dragging(PointF p);

        protected abstract ShapeView createView();

        private ShapeView view;

        public ShapeView getView() {
            if (view == null)
                view = createView();
            return view;
        }

        protected Shape(ScreenshotEditor editor, Color color, int thickness, ShapeSettingsPanel panel) {
            init(editor, panel, color, thickness);
        }

        protected Shape(ScreenshotEditor editor, XPathNavigator element, ShapeSettingsPanel panel) {
            int a = int.Parse(element.GetAttribute("color.a", ""));
            int r = int.Parse(element.GetAttribute("color.r", ""));
            int g = int.Parse(element.GetAttribute("color.g", ""));
            int b = int.Parse(element.GetAttribute("color.b", ""));
            int thickness = int.Parse(element.GetAttribute("thickness", ""));
            init(editor, panel, Color.FromArgb(a, r, g, b), thickness);
            XPathNodeIterator it = element.Clone().Select("handles/handle");
            while (it.MoveNext()) {
                string name = it.Current.GetAttribute("name", "");
                float x = float.Parse(it.Current.GetAttribute("x", ""));
                float y = float.Parse(it.Current.GetAttribute("y", ""));
                addHandle(name, new PointF(x, y));
            }
        }

        private void init(ScreenshotEditor editor, ShapeSettingsPanel panel, Color color, int thickness) {
            this.editor = editor;
            settingsPanel = panel;

            editor.addSettingsChangeListener(this);
            if (panel != null)
                settingsPanel.SettingsChanged += shapeSettingsChanged;

            setColor(color);
            lastColor = color;

            Thickness = lastThickness = thickness;

            handles = new Dictionary<string, PointF>();
        }

        private void shapeSettingsChanged(object sender, EventArgs e) {
            editor.invalidateAllViews();
        }

        ~Shape() {
            editor.removeSettingsChangeListener(this);
        }

        public virtual void onDoubleClick(object sender, MouseEventArgs e) {}

        protected virtual void shapeChanged() {}

        private void setColor(Color c) {
            ShapeColorNoHilight = c;

            double h = c.GetHue()/360;
            double s = c.GetSaturation();
            double b = c.GetBrightness();
            b = getPerceivedBrightness(c) > 128 ? b - Constants.HILIGHT_BOOST : b + Constants.HILIGHT_BOOST;
            hilightColor = hsl2Rgb(h, s, b);
        }

        public Dictionary<string, PointF> Handles {
            get { return handles; }
        }

        private bool isEditing;

        public virtual bool Editing {
            get { return isEditing; }
            set {
                doDrawHandles = value;
                isEditing = value;

                if (value) {
                    lastColor = ShapeColorNoHilight;
                    lastThickness = Thickness;
                } else {
                    selectedHandleName = null;

                    if (!lastColor.Equals(ShapeColorNoHilight)) {
                        Command c = new ColorChangeCommand(this, ShapeColorNoHilight, lastColor);
                        editor.addCommandToUndo(c);
                    }
                    if (lastThickness != Thickness) {
                        Command c = new ThicknessChangeCommand(this, Thickness, lastThickness);
                        editor.addCommandToUndo(c);
                    }
                }
            }
        }

        private class ColorChangeCommand : Command {
            private readonly Shape s;
            private readonly Color newColor;
            private readonly Color oldColor;

            public ColorChangeCommand(Shape s, Color newColor, Color oldColor) {
                this.s = s;
                this.newColor = newColor;
                this.oldColor = oldColor;
            }

            public void execute() {
                s.ShapeColorNoHilight = newColor;
                s.colorChanged(newColor);
                s.editor.invalidateView();
            }

            public void unexecute() {
                s.ShapeColorNoHilight = oldColor;
                s.colorChanged(oldColor);
                s.editor.invalidateView();
            }

            public string Name {
                get { return "Change Color of " + s.Name; }
            }
        }

        private class ThicknessChangeCommand : Command {
            private readonly Shape s;
            private readonly int newThickness;
            private readonly int oldThickness;

            public ThicknessChangeCommand(Shape s, int newThickness, int oldThickness) {
                this.s = s;
                this.newThickness = newThickness;
                this.oldThickness = oldThickness;
            }

            public void execute() {
                s.Thickness = newThickness;
                s.thicknessChanged(newThickness);
                s.editor.invalidateView();
            }

            public void unexecute() {
                s.Thickness = oldThickness;
                s.thicknessChanged(oldThickness);
                s.editor.invalidateView();
            }

            public string Name {
                get { return "Change Width of " + s.Name; }
            }
        }

        public string getHandleAtPoint(PointF p) {
            foreach (string handle in handles.Keys) {
                PointF pt = Handles[handle];
                if (Math.Abs(pt.X - p.X) < 10 && Math.Abs(pt.Y - p.Y) < 10)
                    return handle;
            }
            return null;
        }

        public void addHandle(string name, PointF p) {
            handles.Add(name, p);
        }

        public virtual void moveHandle(string name, PointF to) {
            handles[name] = to;
            shapeChanged();
        }

        public virtual void startMoving(PointF from) {
            moveFrom = from;
        }

        public virtual void moveTo(PointF to) {
            Dictionary<string, PointF> tmp = new Dictionary<string, PointF>();

            foreach (string name in handles.Keys)
                tmp[name] = handles[name];

            foreach (string name in tmp.Keys) {
                PointF p = handles[name];
                handles[name] = new PointF(p.X + to.X - moveFrom.X, p.Y + to.Y - moveFrom.Y);
            }
            moveFrom = to;

            shapeChanged();
        }

        public bool Highlight {
            get { return hilight; }
            set { hilight = value; }
        }

        public virtual void finished() {
            doDrawHandles = false;
        }

        public virtual void deactivate() {}

        public virtual bool isValid() {
            return true;
        }

        protected static Point rotate(PointF start, double arc, PointF p) {
            int x = (int) (p.X - start.X);
            int y = (int) (p.Y - start.Y);

            int xp = (int) (Math.Cos(arc)*x - Math.Sin(arc)*y);
            int yp = (int) (Math.Sin(arc)*x + Math.Cos(arc)*y);
            return new Point(xp + (int) start.X, yp + (int) start.Y);
        }

        // taken from http://www.gamedev.pl/articles.php?x=view&id=192
        protected static bool isPointInPolygon(PointF p, PointF[] polygon) {
            PointF[] pfs = new PointF[polygon.Length];
            int idx = 0;
            foreach (PointF pt in polygon)
                pfs[idx++] = new PointF(pt.X, pt.Y);

            int i, j = 0;
            bool oddNodes = false;

            for (i = 0; i < polygon.Length; ++i) {
                j++;
                if (j == polygon.Length)
                    j = 0;
                if (((pfs[i].Y >= p.Y) || (pfs[j].Y < p.Y)) && ((pfs[j].Y >= p.Y) || (pfs[i].Y < p.Y))) {
                    continue;
                }
                if (pfs[i].X + (p.Y - pfs[i].Y)/(pfs[j].Y - pfs[i].Y)*(pfs[j].X - pfs[i].X) < p.X) {
                    oddNodes = !oddNodes;
                }
            }

            return oddNodes;
        }

        public static RectangleF getShapeRectangleF(PointF start, PointF end) {
            float x1 = Math.Min(start.X, end.X);
            float x2 = Math.Max(start.X, end.X);
            float y1 = Math.Min(start.Y, end.Y);
            float y2 = Math.Max(start.Y, end.Y);

            return new RectangleF(x1, y1, x2 - x1, y2 - y1);
        }

        // see http://en.wikipedia.org/wiki/Ellipse#Equations
        protected static bool isPointInEllipse(PointF center, double w, double h, PointF p) {
            double dx = center.X - p.X;
            double dy = center.Y - p.Y;

            double l = ((dx*dx)/(w*w)) + ((dy*dy)/(h*h));

            return l <= 1.0;
        }

        protected static int getLength(PointF pStart, PointF pEnd) {
            return (int) (Math.Sqrt(getLengthSquared(pStart, pEnd)));
        }

        protected static int getLengthSquared(PointF pStart, PointF pEnd) {
            int lenx = Math.Abs((int) (pStart.X - pEnd.X));
            int leny = Math.Abs((int) (pStart.Y - pEnd.Y));

            return (int) (Math.Pow(lenx, 2) + Math.Pow(leny, 2));
        }

        protected static double getArc(PointF pStart, PointF pEnd) {
            return Math.Atan2((int) (pEnd.Y - pStart.Y), (int) (pEnd.X - pStart.X));
        }

        // taken from http://www.geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
        protected static Color hsl2Rgb(double h, double sl, double l) {
            double r = l;
            double g = l;
            double b = l;
            double v = (l <= 0.5) ? (l*(1.0 + sl)) : (l + sl - l*sl);

            if (v > 0) {
                double m = l + l - v;
                double sv = (v - m)/v;
                h *= 6.0;
                int sextant = (int) h;
                double fract = h - sextant;
                double vsf = v*sv*fract;
                double mid1 = m + vsf;
                double mid2 = v - vsf;

                switch (sextant) {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            Color rgb = Color.FromArgb(Convert.ToByte(r*255.0f), Convert.ToByte(g*255.0f), Convert.ToByte(b*255.0f));
            return rgb;
        }

        // taken from http://www.nbdtech.com/blog/archive/2008/04/27/Calculating-the-Perceived-Brightness-of-a-Color.aspx
        protected static int getPerceivedBrightness(Color c) {
            return (int) Math.Sqrt(
                c.R*c.R*.241 +
                c.G*c.G*.691 +
                c.B*c.B*.068);
        }

        public abstract class ShapeView : View {
            protected Shape shape;

            protected ShapeView(Shape s) {
                shape = s;
            }

            public virtual void paint(Graphics g, bool drawHandles, ViewParameters parameters) {
                if (!drawHandles || !shape.doDrawHandles) {
                    return;
                }
                foreach (string h in shape.handles.Keys)
                    drawHandle(g, h, parameters);
            }

            public static Rectangle getShapeRectangle(PointF start, PointF end, ViewParameters parameters) {
                Point ps = parameters.toAbsolute(start);
                Point pe = parameters.toAbsolute(end);

                int x1 = Math.Min(ps.X, pe.X);
                int x2 = Math.Max(ps.X, pe.X);
                int y1 = Math.Min(ps.Y, pe.Y);
                int y2 = Math.Max(ps.Y, pe.Y);

                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }

            public static float scaledThickness(Shape s, ViewParameters p) {
                return s.Thickness*p.ZoomRatio > 2 ? s.Thickness*p.ZoomRatio : 2;
            }

            protected void drawHandle(Graphics g, string handle, ViewParameters p) {
                Point pt = p.toAbsolute(shape.handles[handle]);
                Bitmap b;
                if (shape.SelectedHandle != null && handle.Equals(shape.SelectedHandle))
                    b = Properties.Resources.handle_selected;
                else
                    b = Properties.Resources.handle;

                Point point = new Point(pt.X - 5, pt.Y - 5);
                g.DrawImage(b, point);
                b.Dispose();
            }
        }
    }
}