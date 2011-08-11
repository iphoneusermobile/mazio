using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace mazio.shapes {
    internal class ScreenShot : Shape, IDisposable {
        private Bitmap shotUnwarped;
        private Bitmap shot;

        private float perspectiveRatio;
        private float perspectivefarSide = 1f;

        private bool needRedraw;

        private volatile int transformationGeneration;

        public ScreenShot(ScreenshotEditor editor, Bitmap shot)
            : base(editor, Color.Black, 0, null) {
            this.shot = shot;
            shotUnwarped = new Bitmap(shot);
            BackgroundTransformator.Instance.SettingsChanged += transformationSettingsChanged;
        }

        private void transformationSettingsChanged(object sender, EventArgs e) {
            needRedraw = true;

            if (BackgroundTransformator.Instance.PerspectiveRatio == perspectiveRatio
                && BackgroundTransformator.Instance.PerspectiveFarSide == perspectivefarSide) {
                return;
            }

            perspectiveRatio = -1 * BackgroundTransformator.Instance.PerspectiveRatio;
            perspectivefarSide = BackgroundTransformator.Instance.PerspectiveFarSide;

            if (perspectiveRatio == 0) {
                shot = new Bitmap(shotUnwarped);
                return;
            }

            int farHeightDiff = (int) (shotUnwarped.Height*perspectiveRatio/2);
            int farDistance = Math.Min(shotUnwarped.Width - 1, (int) (shotUnwarped.Width*perspectivefarSide));

            Point topleft = new Point(perspectiveRatio >= 0 ? 0 : shotUnwarped.Width - farDistance,
                                      perspectiveRatio > 0 ? 0 : -farHeightDiff);
            Point topright = new Point(perspectiveRatio >= 0 ? farDistance : shotUnwarped.Width,
                                       perspectiveRatio > 0 ? farHeightDiff : 0);
            Point bottomleft = new Point(perspectiveRatio >= 0 ? 0 : shotUnwarped.Width - farDistance,
                                         perspectiveRatio > 0
                                             ? shotUnwarped.Height
                                             : shotUnwarped.Height + farHeightDiff);
            Point bottomright = new Point(perspectiveRatio >= 0 ? farDistance : shotUnwarped.Width,
                                          perspectiveRatio > 0
                                              ? shotUnwarped.Height - farHeightDiff
                                              : shotUnwarped.Height);
            Bitmap tmp = new Bitmap(shotUnwarped);
            int gen = ++transformationGeneration;
            Thread t = new Thread(new ThreadStart(delegate {
                                                      Bitmap distorted = QuadDistort.Distort(
                                                          tmp, topleft, topright, bottomleft, bottomright);

                                                      if (gen != transformationGeneration) return;

                                                      ((Control) editor).Invoke(new MethodInvoker(
                                                          delegate {
                                                              Graphics graphics = Graphics.FromImage(shot);
                                                              graphics.Clear(Color.Transparent);
                                                              graphics.DrawImageUnscaled(distorted, 0, 0);
                                                              distorted.Dispose();
                                                              tmp.Dispose();
                                                              needRedraw = true;
                                                              editor.invalidateView();
                                                          }));
                                                  }));
            t.Start();
        }

        public void Dispose() {
            shot.Dispose();
            shotUnwarped = shot;
            BackgroundTransformator.Instance.SettingsChanged -= transformationSettingsChanged;
        }

        public Bitmap Shot {
            get { return shot; }
        }

        public override string Name {
            get { return "Screenshot"; }
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            throw new NotImplementedException();
        }

        protected override void writeCustomXml(XmlTextWriter writer) {
            throw new NotImplementedException();
        }

        protected override ShapeView createView() {
            return new ScreenshotView(this);
        }

        public override bool isHit(PointF p) {
            return false;
        }

        public override bool dragging(PointF p) {
            return false;
        }

        public class ScreenshotView : ShapeView {
            private readonly ScreenShot s;
            private ViewParameters parameters;
            private Bitmap scaledShot;

            public ScreenshotView(ScreenShot s) : base(s) {
                this.s = s;
            }

            public override void paint(Graphics g, bool drawHandles, ViewParameters pars) {
                if (pars != parameters || s.needRedraw) {
                    parameters = pars;
                    s.needRedraw = false;

                    scaledShot = new Bitmap(pars.Viewport.Width, pars.Viewport.Height);

                    Graphics gg = Graphics.FromImage(scaledShot);

                    gg.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    gg.ScaleTransform(pars.ZoomRatio, pars.ZoomRatio);

                    PointF[] dst = new PointF[3];
                    dst[0] = new PointF(0, 0);
                    dst[1] = new PointF(scaledShot.Width/pars.ZoomRatio, 0);
                    dst[2] = new PointF(0, scaledShot.Height/pars.ZoomRatio);

                    Bitmap warpedShot = new Bitmap(s.shot.Width, s.shot.Height);
                    Graphics ggg = Graphics.FromImage(warpedShot);
                    Matrix transform = BackgroundTransformator.Instance.getTransform(warpedShot.Size);

                    ggg.Transform = transform;
                    ggg.DrawImage(s.shot, 0, 0);

                    if (BackgroundTransformator.Instance.DrawPivotPoint && !pars.ForSave) {
                        Brush b = new SolidBrush(Color.Black);
                        Point pivotPoint = BackgroundTransformator.Instance.getPivotPoint(warpedShot.Size);
                        pivotPoint.Offset(-10, -10);
                        ggg.FillEllipse(b, pivotPoint.X, pivotPoint.Y, 20, 20);
                        b.Dispose();
                    }

                    if (pars.ZoomRatio != 1) {
                        RectangleF src = new RectangleF(
                            (pars.Scrolls.X) / pars.ZoomRatio - (pars.CanvasSize.Width / pars.ZoomRatio - s.shot.Width) / 2 - 0.5f,
                            (pars.Scrolls.Y) / pars.ZoomRatio - (pars.CanvasSize.Height / pars.ZoomRatio - s.shot.Height) / 2 - 0.5f,
                            (pars.Viewport.Width) / pars.ZoomRatio,
                            (pars.Viewport.Height) / pars.ZoomRatio);
                        gg.DrawImage(warpedShot, dst, src, GraphicsUnit.Pixel);
                    } else {
                        gg.DrawImageUnscaled(
                            warpedShot, 
                            (pars.CanvasSize.Width - s.shot.Width) / 2 - pars.Scrolls.X,
                            (pars.CanvasSize.Height - s.shot.Height) / 2 - pars.Scrolls.Y);
                    }

                    warpedShot.Dispose();
                }

                g.DrawImageUnscaled(scaledShot, 0, 0);
            }
        }
    }
}