using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes {
    internal class Picture : Shape {

        private bool fromXml;

        public Image Img { get; private set; }

        public Picture(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel) 
            : base(editor, color, thickness, panel) {
            init(panel, p);
        }

        public Picture(Image img, ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel) 
            : base(editor, color, thickness, panel) {

            init(panel, p);
            Img = img;
            moveEndHandleToImageSize();
        }

        public Picture(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel()) {
            sp = (PictureSettingsPanel) settingsPanel;
            sp.ProportionsTypeChanged += sp_ProportionsTypeChanged;
            sp.chkUseShadow.Checked = bool.Parse(element.GetAttribute("shadow", ""));
            sp.chkUseBorder.Checked = bool.Parse(element.GetAttribute("border", ""));
            sp.radioOriginal.Checked = bool.Parse(element.GetAttribute("useOriginalSize", ""));
            if (!sp.radioOriginal.Checked) {
                sp.radioConstrain.Checked = true;
            }
            XPathNodeIterator it = element.Clone().Select("image");
            it.MoveNext();
            string b64 = it.Current.Value;
            byte[] array = Convert.FromBase64String(b64);
            MemoryStream ms = new MemoryStream(array);
            Img = new Bitmap(ms);
            fromXml = true;
        }

        private void init(ShapeSettingsPanel panel, PointF p) {
            addHandle("start", p);
            addHandle("end", p);
            sp = panel as PictureSettingsPanel;
            sp.ProportionsTypeChanged += sp_ProportionsTypeChanged;
        }

        private class PictureSettingsPanel : ShadowSettingsPanel {
            public readonly CheckBox chkUseBorder;
            public readonly RadioButton radioOriginal;
            public readonly RadioButton radioConstrain;
            public readonly RadioButton radioFree;

            private static bool useBorder = true;
            private static int proportionsType;
            private const string USE_BORDER = "PictureShapeUseBorder";
            private const string PROPORTIONS = "PictureShapeProportions";

            public event EventHandler<EventArgs> ProportionsTypeChanged;

            public PictureSettingsPanel() {
                SuspendLayout();

                Padding = new Padding(12);

                proportionsType = getSettingValue(PROPORTIONS, 0);
                useBorder = getSettingValue(USE_BORDER, 0) > 0;

                chkUseBorder = new CheckBox {
                    Text = "Picture Has Border",
                    Dock = DockStyle.Top,
                    TabStop = false,
                    Checked = useBorder
                };

                chkUseBorder.Click += hasBorderClicked;
                Controls.Add(chkUseBorder);

                radioOriginal = new RadioButton {
                    Checked = proportionsType == 0,
                    Text = "Original Size",
                    Dock = DockStyle.Top,
                    TabStop = false
                };
                radioConstrain = new RadioButton {
                    Checked = proportionsType == 1,
                    Text = "Retain Proportions",
                    Dock = DockStyle.Top,
                    TabStop = false
                };
                radioFree = new RadioButton {
                    Checked = proportionsType == 2,
                    Text = "Free Resize",
                    Dock = DockStyle.Top,
                    TabStop = false
                };

                Controls.Add(radioFree);
                Controls.Add(radioConstrain);
                Controls.Add(radioOriginal);

                radioOriginal.CheckedChanged += proportionsConstraintsChanged;
                radioConstrain.CheckedChanged += proportionsConstraintsChanged;
                radioFree.CheckedChanged += proportionsConstraintsChanged;

                chkUseShadow.Enabled = useBorder;

                ResumeLayout(false);
            }

            private void proportionsConstraintsChanged(object sender, EventArgs e) {
                int val = radioOriginal.Checked
                              ? 0
                              : (radioConstrain.Checked
                                     ? 1
                                     : 2);
                proportionsType = val;
                saveSettingValue(PROPORTIONS, proportionsType);
                fireSettingsChangeNotification();
                if (ProportionsTypeChanged != null) {
                    ProportionsTypeChanged(this, null);
                }
            }

            private void hasBorderClicked(object sender, EventArgs e) {
                useBorder = chkUseBorder.Checked;
                chkUseShadow.Enabled = useBorder;
                saveSettingValue(USE_BORDER, useBorder ? 1 : 0);
                fireSettingsChangeNotification();
            }
        }

        private PictureSettingsPanel sp;

        public static ShapeSettingsPanel createSettingsPanel() {
            return new PictureSettingsPanel();
        }

        public override string Name {
            get { return "Picture Box"; }
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            writer.WriteAttributeString("border", sp.chkUseBorder.Checked.ToString());
            writer.WriteAttributeString("shadow", sp.chkUseShadow.Checked.ToString());
            writer.WriteAttributeString("useOriginalSize", sp.radioOriginal.Checked.ToString());
        }

        protected override void writeCustomXml(XmlTextWriter writer) {
            writer.WriteStartElement("image");
            string fileName = Path.GetTempPath() + "mazioPictureShape.png";
            PngFormat format = new PngFormat();
            Img.Save(fileName, format.getCodecInfo(), format.getParameters());
            byte[] bytes = File.ReadAllBytes(fileName);
            writer.WriteBase64(bytes, 0, bytes.Length);
            writer.WriteEndElement();
        }

        public const string HINT_TEXT =
            "Select image with a mouse\r\n"
            + "Use \"Color\" and \"Width\" tools to change border color and width\r\n"
            + "Click \"Selected Tool Settings\" to select image proportions and border style options\r\n"
            + "Press \"Delete\" to remove image";

        public static string HintText {
            get { return HINT_TEXT; }
        }

        private void sp_ProportionsTypeChanged(object sender, EventArgs e) {
            if (sp.radioOriginal.Checked) {
                moveEndHandleToImageSize();
            } else if (sp.radioConstrain.Checked) {
                moveHandle("end", Handles["end"]);
            }
        }

        public override void moveHandle(string name, PointF to) {
            if (sp.radioOriginal.Checked) return;
            if (sp.radioFree.Checked) {
                base.moveHandle(name, to);
                return;
            }
            PointF basePoint = name.Equals("end") ? Handles["start"] : Handles["end"];
            float ratio = ((float) Img.Width)/Img.Height;
            float moveRatio = Math.Abs(to.X - basePoint.X)/Math.Max(Math.Abs(to.Y - basePoint.Y), 0.0001f);
            if (ratio < moveRatio) {
                float newX = to.X;
                float newY = basePoint.Y + Math.Sign(to.Y - basePoint.Y)*Math.Abs(newX - basePoint.X) / ratio;
                base.moveHandle(name, new PointF(newX, newY));
            } else {
                float newY = to.Y;
                float newX = basePoint.X + Math.Sign(to.X - basePoint.X) * Math.Abs(newY - basePoint.Y) * ratio;
                base.moveHandle(name, new PointF(newX, newY));
            }
        }

        public override bool isHit(PointF p) {
            RectangleF rectRel = getShapeRectangleF(Handles["start"], Handles["end"]);

            PointF[] points = new PointF[4];

            points[0] = rectRel.Location;
            points[1] = new PointF(rectRel.Right, rectRel.Y);
            points[2] = new PointF(rectRel.Right, rectRel.Bottom);
            points[3] = new PointF(rectRel.X, rectRel.Bottom);

            bool hit = isPointInPolygon(p, points);
            return hit;
        }

        public override bool dragging(PointF p) {
            return false;
        }

        public override void finished() {
            if (fromXml) return;

            OpenFileDialog dlg = new OpenFileDialog {CheckFileExists = true, Filter = "Image Files|*.png;*.jpg;*.bmp"};
            if (DialogResult.OK != dlg.ShowDialog()) return;
            try {
                Img = Image.FromStream(dlg.OpenFile());

                moveEndHandleToImageSize();

            } catch (Exception e) {
                Debug.WriteLine("Picture.finished() - exception: " + e); 
            }
        }

        private void moveEndHandleToImageSize() {
            PointF p = Handles["start"];
            p.X += editor.CurrentViewParameters.ZoomRatio * Img.Width;
            p.Y += editor.CurrentViewParameters.ZoomRatio * Img.Height;
                
            base.moveHandle("end", p);
        }

        public override bool isValid() {
            return Img != null;
        }

        protected override ShapeView createView() {
            return new PictureView(this);
        }

        public class PictureView : ShapeView, SettingsChangeListener {
            private readonly Picture pic;

            public PictureView(Picture p) : base(p) {
                pic = p;
            }

            public void colorChanged(Color color) {}

            public void thicknessChanged(int thickness) {}

            public void screenShotGrabbed(Bitmap screenshot) {}

            public void screenshotZoomed(int zoomFactor) {}

            public void canvasResized(Size newSize) {}

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters) {
                if (pic.sp.chkUseShadow.Checked || pic.sp.chkUseBorder.Checked) {
                    Brush b = shape.ShapeBrush;
                    float thickness = scaledThickness(shape, parameters);
                    Pen pen = new Pen(b, thickness);

                    RectangleF r = getShapeRectangle(shape.Handles["start"], shape.Handles["end"], parameters);
                    r.Inflate(thickness / 2, thickness / 2);
                    if (pic.sp.chkUseShadow.Checked && pic.sp.chkUseBorder.Checked) {
                        Pen shadowPen = new Pen(Constants.SHADOW_COLOR, thickness);
                        g.DrawRectangle(
                            shadowPen,
                            r.X + Constants.SHADOW_X * parameters.ZoomRatio,
                            r.Y + Constants.SHADOW_Y * parameters.ZoomRatio,
                            r.Width, r.Height);
                        shadowPen.Dispose();
                    }
                    if (pic.sp.chkUseBorder.Checked) {
                        g.DrawRectangle(pen, r.Left, r.Top, r.Width, r.Height);
                    }
                }
                if (pic == null || pic.Img == null) return;

                Point ps = parameters.toAbsolute(pic.Handles["start"]);
                Point pe = parameters.toAbsolute(pic.Handles["end"]);
                RectangleF rect = getShapeRectangleF(ps, pe);
                float x = Math.Min(ps.X, pe.X);
                float y = Math.Min(ps.Y, pe.Y);
                PointF[] dest = new PointF[3];
                dest[0] = new PointF(x, y);
                dest[1] = new PointF(x + rect.Width, y);
                dest[2] = new PointF(x, y + rect.Height);
                if (pic.Highlight && UseHilights) {
                    Bitmap copy = new Bitmap(pic.Img.Width, pic.Img.Height);
                    Graphics gg = Graphics.FromImage(copy);
                    gg.DrawImageUnscaled(pic.Img, 0, 0);
                    ImageProcessor.Invert(copy);
                    g.DrawImage(copy, dest);
                    copy.Dispose();
                } else {
                    if (pic.sp.radioOriginal.Checked && parameters.ZoomRatio == 1) {
                        g.DrawImageUnscaled(pic.Img, (int) x, (int) y);
                    } else {
                        g.DrawImage(pic.Img, dest);
                    }
                }
                base.paint(g, drawHandles && !pic.sp.radioOriginal.Checked, parameters);
            }
        }
    }
}
