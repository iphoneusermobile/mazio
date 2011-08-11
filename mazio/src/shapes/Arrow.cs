using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace mazio.shapes {
    public class Arrow : Shape {
        private readonly ArrowSettingsPanel sp;

        private class ArrowSettingsPanel : ShadowSettingsPanel {
            private readonly RadioButton radioTwo;
            private readonly RadioButton radioOne;

            private readonly List<RadioButton> buttons = new List<RadioButton>();
            public const string SHAPE_ARROW_TYPE = "ShapeArrowType";

            public ArrowSettingsPanel() {
                radioOne = new RadioButton {Image = Properties.Resources.arrow_1};
                buttons.Add(radioOne);

                radioTwo = new RadioButton {Image = Properties.Resources.arrow_2};
                buttons.Add(radioTwo);

                GroupBox group = new GroupBox();

                group.SuspendLayout();
                SuspendLayout();

                int i = 0;

                int arrowType = getSettingValue(SHAPE_ARROW_TYPE, 0);

                foreach (RadioButton b in buttons) {
                    if (i == arrowType) {
                        b.Checked = true;
                    }
                    b.AutoSize = true;
                    b.Appearance = Appearance.Button;
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.BorderSize = 0;
                    b.FlatAppearance.CheckedBackColor = SystemColors.ControlDark;
                    b.Location = new Point(32, 24 + 60*i++);
                    b.Padding = new Padding(6);
                    b.TabStop = false;
                    b.Click += switchArrow;

                    group.Controls.Add(b);
                }

                group.Dock = DockStyle.Top;
                group.TabStop = false;
                group.Text = "Arrow Type";
                group.AutoSize = true;

                Padding = new Padding(12);
                Controls.Add(group);

                group.ResumeLayout(false);
                group.PerformLayout();
                ResumeLayout(false);
            }

            public int getArrowType() {
                int i = 0;
                foreach (RadioButton b in buttons) {
                    if (b.Checked) {
                        break;
                    }
                    ++i;
                }
                return i;
            }

            public void setArrowType(int type) {
                int i = 0;
                foreach (RadioButton b in buttons) {
                    if (i == type) {
                        b.Checked = true;
                        break;
                    }
                    ++i;
                }
            }

            private void switchArrow(object sender, EventArgs e) {
                int i = 0;
                foreach (RadioButton b in buttons) {
                    if (b.Checked) {
                        saveSettingValue(SHAPE_ARROW_TYPE, i);
                        break;
                    }
                    ++i;
                }
                fireSettingsChangeNotification();
            }

            public PointF[] getArrowPoints(PointF start, int length, double arc, int thickness) {
                float x = start.X;
                float y = start.Y;

                int s = Math.Max(thickness, 5);

                PointF[] points;

                if (radioTwo.Checked) {
                    points = new PointF[6];
                    points[0] = new PointF(x, y);
                    points[1] = rotate(start, arc, new PointF(x + 0.8f*length, y - thickness));
                    points[2] = rotate(start, arc, new PointF(x + 0.8f*length, y - 2*s));
                    points[3] = rotate(start, arc, new PointF(x + length, y));
                    points[4] = rotate(start, arc, new PointF(x + 0.8f*length, y + 2*s));
                    points[5] = rotate(start, arc, new PointF(x + 0.8f*length, y + thickness));
                } else {
                    points = new PointF[8];
                    points[0] = new PointF(x, y);
                    points[1] = rotate(start, arc, new PointF(x, y - thickness));
                    points[2] = rotate(start, arc, new PointF(x + 0.80f*length, y - thickness));
                    points[3] = rotate(start, arc, new PointF(x + 0.75f*length, y - 2*s));
                    points[4] = rotate(start, arc, new PointF(x + length, y));
                    points[5] = rotate(start, arc, new PointF(x + 0.75f*length, y + 2*s));
                    points[6] = rotate(start, arc, new PointF(x + 0.80f*length, y + thickness));
                    points[7] = rotate(start, arc, new PointF(x, y + thickness));
                }

                return points;
            }
        }

        public Arrow(ScreenshotEditor editor, Color color, int thickness, PointF p, ShapeSettingsPanel panel)
            : base(editor, color, thickness, panel) {
            addHandle("start", p);
            addHandle("end", p);

            sp = (ArrowSettingsPanel) panel;
        }

        public Arrow(ScreenshotEditor editor, XPathNavigator element) : base(editor, element, createSettingsPanel()) {
            sp = (ArrowSettingsPanel) settingsPanel;

            sp.chkUseShadow.Checked = bool.Parse(element.GetAttribute("shadow", ""));
            sp.setArrowType(int.Parse(element.GetAttribute("ShapeArrowType", "")));
        }

        public override string Name {
            get { return "Arrow"; }
        }

        public const string HINT_TEXT =
            "Drag arrow to move it\r\n"
            + "Drag a handle to edit arrow\r\n"
            + "Use \"Color\" and \"Width\" tools to change arrow color and width\r\n"
            + "Click \"Selected Tool Settings\" to change arrow type\r\n"
            + "Press \"Delete\" to remove arrow";

        protected override void writeCustomXmlAttributes(XmlTextWriter writer) {
            writer.WriteAttributeString("shadow", sp.chkUseShadow.Checked.ToString());
            writer.WriteAttributeString(ArrowSettingsPanel.SHAPE_ARROW_TYPE, sp.getArrowType().ToString());
        }

        protected override void writeCustomXml(XmlTextWriter writer) {}

        public static string HintText {
            get { return HINT_TEXT; }
        }

        public static ShapeSettingsPanel createSettingsPanel() {
            return new ArrowSettingsPanel();
        }

        protected override ShapeView createView() {
            return new ArrowView(this);
        }

        public override bool isHit(PointF p) {
            PointF ps = Handles["start"];
            PointF pe = Handles["end"];

            return isPointInPolygon(p, calculatePoints(ps, getLength(ps, pe), getArc(ps, pe)));
        }

        private PointF[] calculatePoints(PointF start, int length, double arc) {
            return sp.getArrowPoints(start, length, arc, Thickness);
        }

        public override bool dragging(PointF p) {
            moveHandle("end", p);
            return false;
        }

        public class ArrowView : ShapeView {
            private readonly Arrow a;

            public ArrowView(Arrow a) : base(a) {
                this.a = a;
            }

            public override void paint(Graphics g, bool drawHandles, ViewParameters parameters) {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                Brush arrowBrush = shape.ShapeBrush;

                PointF ps = shape.Handles["start"];
                PointF pe = shape.Handles["end"];

                PointF[] points = a.calculatePoints(ps, getLength(ps, pe), getArc(ps, pe));

                Point[] ptsAbs = new Point[points.Length];
                Point[] pointsOffs = new Point[points.Length];
                for (int i = 0; i < points.Length; ++i) {
                    ptsAbs[i] = pointsOffs[i] = parameters.toAbsolute(points[i]);
                    pointsOffs[i].Offset((int) (Constants.SHADOW_X*parameters.ZoomRatio),
                                         (int) (Constants.SHADOW_Y*parameters.ZoomRatio));
                }

                if (a.sp.chkUseShadow.Checked) {
                    Brush shadowBrush = new SolidBrush(Constants.SHADOW_COLOR);
                    g.FillPolygon(shadowBrush, pointsOffs);
                    shadowBrush.Dispose();
                }
                g.FillPolygon(arrowBrush, ptsAbs);

                base.paint(g, drawHandles, parameters);

                arrowBrush.Dispose();
            }
        }
    }
}