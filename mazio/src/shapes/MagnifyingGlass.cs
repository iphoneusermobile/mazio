using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes
{
    public class MagnifyingGlass : Oval
    {
        private Bitmap view;
        private Bitmap previousScreenshot;

        private class MagnifierSettingsPanel : ShadowSettingsPanel
        {
            public readonly TrackBar trckZoom;

            private static int zoom = ConfigDialog.Instance.MagnifierZoomFactor;

            static MagnifierSettingsPanel() {
                ConfigDialog.MagnifierZoomFactorChanged += configDialogMagnifierZoomFactorChanged;
            }

            public MagnifierSettingsPanel() {

                trckZoom = new TrackBar
                           {
                               Minimum = Constants.MIN_MAGNIFIER_ZOOM, 
                               Maximum = Constants.MAX_MAGNIFIER_ZOOM, 
                               Value = zoom, TickStyle = TickStyle.None
                           };

                SuspendLayout();

                // todo: this looks like shit. Have to reposition some time in the future :) 
                trckZoom.Dock = DockStyle.Top;
                trckZoom.TabStop = false;

                trckZoom.ValueChanged += trckZoom_ValueChanged;
                Padding = new Padding(12);
                Controls.Add(trckZoom);

                Label l = new Label
                          {
                              Text = "Zoom Factor", 
                              Dock = DockStyle.Top, 
                              TextAlign = ContentAlignment.MiddleCenter
                          };

                Controls.Add(l);

                ResumeLayout(false);
            }

            private static void configDialogMagnifierZoomFactorChanged(object sender, ConfigDialog.ValueEventArgs<int> e) {
                zoom = e.Value;
            }

            void trckZoom_ValueChanged(object sender, EventArgs e)
            {
                zoom = trckZoom.Value;
                fireSettingsChangeNotification();
            }
        }

        public static new ShapeSettingsPanel createSettingsPanel()
        {
            return new MagnifierSettingsPanel();
        }

        protected override ShapeView createView()
        {
            return new MagnifyingGlassView(this);
        }

        private readonly MagnifierSettingsPanel sp;

        public MagnifyingGlass(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel) 
            : base(editor, color, thickness, p, true, panel)
        {
            view = null;

            sp = (MagnifierSettingsPanel)panel;

            sp.SettingsChanged += settingsChanged;
        }

        public MagnifyingGlass(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel(), true) {
            sp = (MagnifierSettingsPanel) settingsPanel;
            sp.trckZoom.Value = int.Parse(element.GetAttribute("zoom", ""));
            sp.chkUseShadow.Checked = bool.Parse(element.GetAttribute("shadow", ""));
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            writer.WriteAttributeString("shadow", sp.chkUseShadow.Checked.ToString());
            writer.WriteAttributeString("zoom", sp.trckZoom.Value.ToString());
        }

        void settingsChanged(object sender, EventArgs e)
        {
            shapeChanged();
            editor.invalidateView();
        }

        private Bitmap createMagnificationBitmap(RectangleF r, Image screenshot)
        {
            int w = (int)(r.Width + 0.5);
            int h = (int)(r.Height + 0.5);

            if (w < 1) w = 1;
            if (h < 1) h = 1;

            Bitmap bitmap = new Bitmap(w, h);

            Bitmap reflection = Properties.Resources.reflection;

            Graphics graphics = Graphics.FromImage(bitmap);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, r.Width, r.Height);
            graphics.SetClip(path);
            graphics.InterpolationMode = InterpolationMode.Bilinear;

            PointF[] pts = new PointF[3];
            pts[0] = new PointF(0, 0);
            pts[1] = new PointF(r.Width, 0);
            pts[2] = new PointF(0, r.Height);

            double zoomFactor = sp.trckZoom.Value / 100.0;

            Brush bgBrush = new SolidBrush(editor.BackgroundColor);
            graphics.FillEllipse(bgBrush, 0, 0, r.Width, r.Height);
            bgBrush.Dispose();

            if (screenshot != null)
            {
                PointF origin = r.Location;
// ReSharper disable PossibleLossOfFraction
                origin.X += r.Width / 2 + screenshot.Width / 2;
                origin.Y += r.Height / 2 + screenshot.Height / 2;
// ReSharper restore PossibleLossOfFraction
                RectangleF srcRect = new RectangleF(
                    origin.X - (float)(r.Width / (2 * zoomFactor)),
                    origin.Y - (float)(r.Height / (2 * zoomFactor)),
                    (int)(r.Width / zoomFactor),
                    (int)(r.Height / zoomFactor));
                graphics.DrawImage(screenshot, pts, srcRect, GraphicsUnit.Pixel);
            }

            graphics.DrawImage(reflection, pts);

            path.Dispose();
            reflection.Dispose();

            return bitmap;
        }

        public override string Name { get { return "Magnifying Glass"; } }

        public new const string HINT_TEXT =
            "Drag magnifier to move it\r\n"
            + "Drag a handle to change magnifier size\r\n"
            + "Use \"Color\" and \"Width\" tools to change line color and width\r\n"
            + "Click \"Selected Tool Settings\" to change magnification\r\n"
            + "Press \"Delete\" to remove magnifier";

        public new static string HintText {
            get { return HINT_TEXT; }
        }

        protected override void shapeChanged()
        {
            if (view == null) {
                return;
            }
            view.Dispose();
            view = null;
        }

        public class MagnifyingGlassView : OvalView
        {
            private readonly MagnifyingGlass m;
            public MagnifyingGlassView(MagnifyingGlass m) : base(m) { this.m = m; }

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters)
            {
                RectangleF r = getShapeRectangleF(m.Handles["start"], m.Handles["end"]);

                if (r.Width > 0 && r.Height > 0)
                {
                    Bitmap screenshot = m.editor.getAnnotatedScreenshot(m);
                    if (m.view == null || screenshot != m.previousScreenshot)
                    {
                        if (m.view != null)
                            m.view.Dispose();
                        m.view = m.createMagnificationBitmap(r, screenshot);
                        m.previousScreenshot = screenshot;
                    }

                    RectangleF rAbs = getShapeRectangle(m.Handles["start"], m.Handles["end"], parameters);
                    g.DrawImage(m.view, rAbs);
                }

                base.paint(g, drawHandles, parameters);
            }
        }
    }
}
