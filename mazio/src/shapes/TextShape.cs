using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes {
    internal class TextShape : Shape {
        private string text;
        private Font textFont;
        private Color editorColor;

        private bool fromXml;

        private readonly TextSettingsPanel sp;

        private const int REL_SIZE = 4;

        public TextShape(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel)
            : base(editor, color, thickness, panel) {
            setColorAndThickness();

            addHandle("start", p);
            p.X += 100;
            p.Y += TextHeight;
            addHandle("end", p);

            sp = panel as TextSettingsPanel;

            sp.SettingsChanged += sp_SettingsChanged;
        }

        public TextShape(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel()) {
            sp = (TextSettingsPanel) settingsPanel;
            sp.SettingsChanged += sp_SettingsChanged;
            setColorAndThickness();
            sp.trckOutlineWidth.Value = int.Parse(element.GetAttribute("outlineWidth", ""));
            sp.chkUseShadow.Checked = bool.Parse(element.GetAttribute("shadow", ""));
            sp.chkOpaqueOutline.Checked = bool.Parse(element.GetAttribute("opaqueOutline", ""));
            string fontStyle = element.GetAttribute("fontStyle", "");
            string fontFamily = element.GetAttribute("fontFamily", "");

            XPathNodeIterator it = element.Clone().Select("text");
            it.MoveNext();
            text = it.Current.Value;

            FontStyle style = (FontStyle)Enum.Parse(typeof(FontStyle), fontStyle);
            sp.buttonFont.Font = new Font(fontFamily, 14, style, GraphicsUnit.Pixel, 0);

            fromXml = true;
        }

        ~TextShape() {
            textFont.Dispose();
        }

        private void sp_SettingsChanged(object sender, EventArgs e) {
            TextView v = (TextView) getView();

            editor.invalidateView();
            v.setFont(editor.CurrentViewParameters);
            v.resizeEditor();
        }

        private class TextSettingsPanel : ShadowSettingsPanel {
            public readonly CheckBox chkOpaqueOutline;
            public readonly TrackBar trckOutlineWidth;

            public readonly Button buttonFont;

            private static bool useOpaqueOutline = true;
            private const string SHAPE_USE_OPAQUE_OUTLINE = "ShapeUseOpaqueOutline";
            private const string SHAPE_OUTLINE_WIDTH = "ShapeTextOutlineWidth";
            private const string SHAPE_TEXT_FONT_NAME = "ShapeTextFontName";
            private const string SHAPE_TEXT_FONT_STYLE = "ShapeTextFontStyle";

            public TextSettingsPanel() {
                SuspendLayout();

                Padding = new Padding(12);

                int value = getSettingValue(SHAPE_OUTLINE_WIDTH, 2);

                trckOutlineWidth = new TrackBar
                                   {
                                       Minimum = 1,
                                       Maximum = 4,
                                       Value = value,
                                       TickStyle = TickStyle.None,
                                       Dock = DockStyle.Top,
                                       TabStop = false
                                   };

                trckOutlineWidth.ValueChanged += trckOutlineWidth_ValueChanged;

                Controls.Add(trckOutlineWidth);

                Label l = new Label
                          {
                              Text = "Text Outline Width",
                              Dock = DockStyle.Top,
                              TextAlign = ContentAlignment.MiddleCenter
                          };

                Controls.Add(l);

                chkOpaqueOutline = new CheckBox
                                   {
                                       Text = "Opaque Text Outline",
                                       Dock = DockStyle.Top,
                                       TabStop = false,
                                       Checked = useOpaqueOutline
                                   };

                chkOpaqueOutline.Click += opaqueClicked;
                Controls.Add(chkOpaqueOutline);

                value = getSettingValue(SHAPE_USE_OPAQUE_OUTLINE, 1);

                chkUseShadow.Checked = value != 0;

                buttonFont = new Button
                             {
                                 Text = "Font",
                                 Font = getFont(),
                                 Dock = DockStyle.Top
                             };
                buttonFont.Click += buttonFont_Click;

                Controls.Add(buttonFont);

                ResumeLayout(false);
            }

            private Font getFont() {
                string settingValue = getSettingValue(SHAPE_TEXT_FONT_STYLE,
                                                      Enum.GetName(typeof (FontStyle), FontStyle.Bold));
                FontStyle style;
                if (Enum.IsDefined(typeof (FontStyle), settingValue)) {
                    style = (FontStyle) Enum.Parse(typeof (FontStyle), settingValue);
                } else {
                    style = FontStyle.Bold;
                }
                return new Font(getSettingValue(SHAPE_TEXT_FONT_NAME, "Tahoma"), 14, style, GraphicsUnit.Pixel, 0);
            }

            private void buttonFont_Click(object sender, EventArgs e) {
                FontDialog fd = new FontDialog
                                {
                                    ShowEffects = false,
                                    ShowColor = false,
                                    MinSize = 14,
                                    MaxSize = 14,
                                    Font = buttonFont.Font
                                };

                if (DialogResult.OK != fd.ShowDialog()) return;
                saveSettingValue(SHAPE_TEXT_FONT_NAME, fd.Font.Name);
                saveSettingValue(SHAPE_TEXT_FONT_STYLE, fd.Font.Style);
                buttonFont.Font = getFont();

                fireSettingsChangeNotification();
            }

            private void trckOutlineWidth_ValueChanged(object sender, EventArgs e) {
                saveSettingValue(SHAPE_OUTLINE_WIDTH, trckOutlineWidth.Value);
                fireSettingsChangeNotification();
            }

            private void opaqueClicked(object sender, EventArgs e) {
                useOpaqueOutline = chkOpaqueOutline.Checked;
                saveSettingValue(SHAPE_USE_OPAQUE_OUTLINE, useOpaqueOutline ? 1 : 0);
                fireSettingsChangeNotification();
            }
        }

        public static ShapeSettingsPanel createSettingsPanel() {
            return new TextSettingsPanel();
        }

        protected override ShapeView createView() {
            return new TextView(this, editor);
        }

        public override string Name {
            get { return "Text"; }
        }

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            writer.WriteAttributeString("fontFamily", sp.buttonFont.Font.FontFamily.Name);
            writer.WriteAttributeString("fontStyle", sp.buttonFont.Font.Style.ToString());
            writer.WriteAttributeString("opaqueOutline", sp.chkOpaqueOutline.Checked.ToString());
            writer.WriteAttributeString("shadow", sp.chkUseShadow.Checked.ToString());
            writer.WriteAttributeString("outlineWidth", sp.trckOutlineWidth.Value.ToString());
        }

        protected override void writeCustomXml(XmlTextWriter writer) {
            writer.WriteStartElement("text");
            writer.WriteCData(text ?? "");
            writer.WriteEndElement();
        }

        private RichTextBox Editor { get; set; }

        private void setColorAndThickness() {
            editorColor = ShapeColorNoHilight;
            if (textFont != null)
                textFont.Dispose();
            textFont = new Font("Tahoma", REL_SIZE*Thickness, FontStyle.Bold, GraphicsUnit.Pixel, 0);
        }

        private int TextHeight {
            get {
                SizeF result = editor.CanvasGraphics.MeasureString("Yy", textFont);
                return (int) result.Height;
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

            addEditor();
            base.finished();
            Editor.Focus();
            editor.CanAddNewShape = false;
            editor.invalidateView();
        }

        private void addEditor() {
            Editor = new BrutallyKillableTextBox(delegate { deactivate(); editor.invalidateView(); });
//            Editor = new RichTextBox();
            ViewParameters p = editor.CurrentViewParameters;
            Rectangle r = ShapeView.getShapeRectangle(Handles["start"], Handles["end"], p);
            Editor.Location = new Point(r.Location.X + p.Scrolls.X, r.Location.Y + p.Scrolls.Y);
            Editor.Size = r.Size;
            Editor.BorderStyle = BorderStyle.None;
            Editor.ScrollBars = RichTextBoxScrollBars.None;
            Editor.DetectUrls = false;
            Editor.Font = new Font(sp.buttonFont.Font.FontFamily, REL_SIZE*ShapeView.scaledThickness(this, p),
                                   sp.buttonFont.Font.Style, GraphicsUnit.Pixel, 0);
            Editor.ForeColor = editorColor;
            Editor.BackColor = getPerceivedBrightness(editorColor) > 0x80 ? Color.DarkGray : Color.LightGray;
            Editor.HideSelection = false;
            // bug #52 - max length of the text input no longer makes sense for multi-line editors
//            Editor.MaxLength = 100;

            Editor.Multiline = true;
            Editor.AcceptsTab = true;
            Editor.WordWrap = false;

            Editor.Name = "editor";
            Editor.TabIndex = 0;
            Editor.TabStop = false;
            Editor.Text = Text ?? "";
            Editor.Anchor = AnchorStyles.None;
            editor.addControl(Editor);

            TextView v = (TextView) getView();
            v.setTextChangeHandler(Editor);
        }

        private void resize() {
            Graphics g = editor.CanvasGraphics;
            string txt = Editor != null && Editor.Text != null ? Editor.Text : (Text ?? "");
            g.MeasureString(txt, textFont, 10000, StringFormat.GenericTypographic);
            SizeF result = g.MeasureString(txt, textFont);

            PointF p = Handles["start"];
            p.X += Math.Max(result.Width, 100);

            string[] lines = txt.Split(new[] {'\n'});
            p.Y += TextHeight*lines.Length;
            Handles["end"] = p;
        }

        public override void screenshotZoomed(int zoomFactor) {
            base.screenshotZoomed(zoomFactor);

            setColorAndThickness();
            resize();
        }

        public override void thicknessChanged(int thickness) {
            base.thicknessChanged(thickness);
            setColorAndThickness();
            resize();
        }

        public override void onDoubleClick(object sender, MouseEventArgs e) {
            setColorAndThickness();
            addEditor();
        }

        private readonly Object deactivateLock = new object();

        public override void deactivate() {
            lock(deactivateLock) {
                RichTextBox e = Editor;
                if (e == null) {
                    return;
                }
                editor.removeControl(e);
                string t = e.Text.TrimEnd();
                text = string.IsNullOrEmpty(t) ? text : t;
                e.Dispose();
                Editor = null;
            }
        }

        public override bool isValid() {
            if (Editor != null)
                return true;
            return !string.IsNullOrEmpty(text);
        }

        public string Text {
            get { return text != null ? text.TrimEnd() : null; }
        }

        public const string HINT_TEXT =
            "Select text with a mouse\r\n"
            + "Drag text to move it\r\n"
            + "Double-click text to edit it\r\n"
            + "Use \"Color\" and \"Width\" tools to change color and font size\r\n"
            + "Click \"Selected Tool Settings\" button for more options\r\n"
            + "Press \"Delete\" to remove text";

        public static string HintText {
            get { return HINT_TEXT; }
        }

        public class TextView : ShapeView, SettingsChangeListener {
            private readonly TextShape t;
            private readonly ScreenshotEditor editor;

            private Bitmap cache;

            public TextView(TextShape t, ScreenshotEditor editor) : base(t) {
                this.t = t;
                this.editor = editor;

                editor.addSettingsChangeListener(this);
                setFont(editor.CurrentViewParameters);
            }

            ~TextView() {
                editorFont.Dispose();
                editor.removeSettingsChangeListener(this);
            }

            private void invalidateCache() {
                if (cache != null) {
                    cache.Dispose();
                    cache = null;
                }
            }

            private void textChanged(object sender, EventArgs e) {
                t.resize();
                if (t.Editor == null) return;

                ViewParameters p = editor.CurrentViewParameters;
                Rectangle r = getShapeRectangle(shape.Handles["start"], shape.Handles["end"], p);

                t.Editor.Location = new Point(r.Location.X + p.Scrolls.X, r.Location.Y + p.Scrolls.Y);
                t.Editor.Size = r.Size;
                t.Editor.Font = editorFont;
                invalidateCache();
            }

            public void colorChanged(Color color) {
                if (t.Editor == null) {
                    return;
                }
                t.Editor.ForeColor = color;
                t.Editor.BackColor = getPerceivedBrightness(color) > 0x80 ? Color.Black : Color.White;
                invalidateCache();
            }

            public void thicknessChanged(int thickness) {
                setFont(editor.CurrentViewParameters);
                resizeEditor();
                invalidateCache();
            }

            public void screenShotGrabbed(Bitmap screenshot) {
                invalidateCache();
            }

            public void canvasResized(Size newSize) {
                moveEditor(editor.CurrentViewParameters);
                invalidateCache();
            }

            public void screenshotZoomed(int zoomFactor) {
                resizeEditor();
                setFont(editor.CurrentViewParameters);
                moveEditor(editor.CurrentViewParameters);
                invalidateCache();
            }

            public void resizeEditor() {
                if (t.Editor == null) {
                    return;
                }
                Graphics g = editor.CanvasGraphics;
                Size result = g.MeasureString(t.Editor.Text + " ", editorFont).ToSize();
                t.Editor.Size = new Size(Math.Max(result.Width, 100), result.Height);
                t.Editor.Font = editorFont;
                t.Editor.Invalidate();
            }

            private Font editorFont;

            public void setFont(ViewParameters p) {
                if (editorFont != null)
                    editorFont.Dispose();
                editorFont = new Font(t.sp.buttonFont.Font.Name, REL_SIZE*scaledThickness(shape, p),
                                      t.sp.buttonFont.Font.Style,
                                      GraphicsUnit.Pixel, 0);
                invalidateCache();
            }

            private void moveEditor(ViewParameters p) {
                if (t.Editor == null) {
                    return;
                }
                Point pt = p.toAbsolute(shape.Handles["start"]);
                t.Editor.Location = new Point(pt.X + p.Scrolls.X, pt.Y + p.Scrolls.Y);
                t.editor.invalidateView();
                invalidateCache();
            }

            private ViewParameters prevParams;
            private Color prevColor;

            public void setTextChangeHandler(RichTextBox e) {
                e.TextChanged += textChanged;
            }

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters) {
                if (prevParams != parameters) {
                    prevParams = parameters;
                    setFont(parameters);
                    invalidateCache();
                }
                if (shape.ShapeColor != prevColor) {
                    invalidateCache();
                    prevColor = shape.ShapeColor;
                }

                Rectangle r = getShapeRectangle(shape.Handles["start"], shape.Handles["end"], parameters);

                int alpha = t.sp.chkOpaqueOutline.Checked ? 0xff : shape.ShapeColorNoHilight.A;

                Color backdropColor = getPerceivedBrightness(shape.ShapeColorNoHilight) > 128
                                          ? Color.FromArgb(alpha, Color.Black)
                                          : Color.FromArgb(alpha, Color.White);

                Color shadowColor = Color.FromArgb(0x1, Color.Black);

                Brush brush = shape.ShapeColor == shape.ShapeColorNoHilight ? new SolidBrush(Color.FromArgb(255, shape.ShapeColor)) : shape.ShapeBrush;

                if ((t.Editor == null) && !string.IsNullOrEmpty(t.Text)) {
                    if (cache == null) {
                        cache = new Bitmap(r.Width + (int) (40 * parameters.ZoomRatio), r.Height + (int) (40 * parameters.ZoomRatio));
                        GraphicsPath pth = new GraphicsPath();

                        Graphics gg = Graphics.FromImage(cache);

                        gg.SmoothingMode = SmoothingMode.HighQuality;
                        gg.TextRenderingHint = TextRenderingHint.AntiAlias;

                        RectangleF rf = new RectangleF(15.5f * parameters.ZoomRatio, 15.5f * parameters.ZoomRatio, r.Width, r.Height);

                        if (t.sp.chkUseShadow.Checked) {
                            drawShadow(gg, r, parameters, shadowColor);
                        }

                        float width = Math.Max(t.sp.trckOutlineWidth.Value * 8.0f * parameters.ZoomRatio, 2f);
                        Pen p = new Pen(backdropColor, width / 2) {
                            LineJoin = LineJoin.Round,
                            EndCap = LineCap.Round,
                            StartCap = LineCap.Round
                        };

                        pth.AddString(t.Text, editorFont.FontFamily, (int)t.sp.buttonFont.Font.Style,
                                      editorFont.Size, rf.Location, StringFormat.GenericTypographic);

                        gg.DrawPath(p, pth);

                        gg.DrawString(t.Text, editorFont, brush, rf, StringFormat.GenericTypographic);

                        p.Dispose();
                        pth.Dispose();
                    }
                    g.DrawImageUnscaled(cache, r.X - (int)(15 * parameters.ZoomRatio), r.Y - (int)(15 * parameters.ZoomRatio));

                    if (shape.ShapeColor != shape.ShapeColorNoHilight) {
                        Pen rectPen = new Pen(Color.FromArgb(255, shape.ShapeColorNoHilight), 3 * parameters.ZoomRatio);
                        g.DrawRectangle(rectPen, r.X - 20f * parameters.ZoomRatio, r.Y - 10f * parameters.ZoomRatio, r.Width + 10f * parameters.ZoomRatio, r.Height + 10f * parameters.ZoomRatio);
                        rectPen.Dispose();
                    }
                }

                brush.Dispose();

                base.paint(g, false, parameters);
            }

            private void drawShadow(Graphics g, Rectangle r, ViewParameters parameters, Color shadowColor) {
                Bitmap b = new Bitmap(r.Width + (int) (40 * parameters.ZoomRatio), r.Height + (int) (40 * parameters.ZoomRatio));
                Graphics gr = Graphics.FromImage(b);

                GraphicsPath pth = new GraphicsPath();

                Pen p = new Pen(shadowColor, Math.Max(12f*parameters.ZoomRatio, 2))
                        {
                            LineJoin = LineJoin.Round,
                            EndCap = LineCap.Round,
                            StartCap = LineCap.Round
                        };


                pth.AddString(t.Text, editorFont.FontFamily, (int) t.sp.buttonFont.Font.Style,
                              editorFont.Size, new Point((int) (10 * parameters.ZoomRatio), (int) (10 * parameters.ZoomRatio)), StringFormat.GenericTypographic);

                gr.DrawPath(p, pth);

                // lame blurring :)
                for (int i = 0; i < 20; i = i + 2) {
                    for (int j = 0; j < 20; j = j + 2) {
                        g.DrawImageUnscaled(b, new Point((int) ((-2.5 + i) * parameters.ZoomRatio), (int) ((-2.5 + j) * parameters.ZoomRatio)));
                    }
                }

                b.Dispose();
                p.Dispose();
                pth.Dispose();
            }
        }
    }
}