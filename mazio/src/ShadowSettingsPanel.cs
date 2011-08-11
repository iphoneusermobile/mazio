using System;
using System.Windows.Forms;

namespace mazio {
    internal class ShadowSettingsPanel : ShapeSettingsPanel {
        public CheckBox chkUseShadow;

        private static bool useShadow = true;
        private const string SHAPE_USE_SHADOW = "ShapeUseShadow";

        public ShadowSettingsPanel() {
            SuspendLayout();

            chkUseShadow = new CheckBox
                           {Text = "Draw Shadow", Dock = DockStyle.Top, TabStop = false, Checked = useShadow};

            chkUseShadow.Click += checkClicked;
            Padding = new Padding(12);
            Controls.Add(chkUseShadow);

            ResumeLayout(false);

            int value = getSettingValue(SHAPE_USE_SHADOW, 1);
            chkUseShadow.Checked = value != 0;
        }

        private void checkClicked(object sender, EventArgs e) {
            useShadow = chkUseShadow.Checked;
            saveSettingValue(SHAPE_USE_SHADOW, useShadow ? 1 : 0);
            fireSettingsChangeNotification();
        }
    }

    internal class ShadowAndFillingSettingsPanel : ShadowSettingsPanel {
        public CheckBox chkFill;

        private static bool useFill = true;
        private const string SHAPE_USE_FILL = "ShapeUseFill";

        public ShadowAndFillingSettingsPanel() {
            SuspendLayout();

            chkFill = new CheckBox { Text = "Filled", Dock = DockStyle.Top, TabStop = false, Checked = useFill };

            chkFill.Click += checkClicked;
            Padding = new Padding(12);
            Controls.Add(chkFill);

            ResumeLayout(false);

            int value = getSettingValue(SHAPE_USE_FILL, 0);
            chkFill.Checked = value != 0;
        }

        private void checkClicked(object sender, EventArgs e) {
            useFill = chkFill.Checked;
            saveSettingValue(SHAPE_USE_FILL, useFill ? 1 : 0);
            fireSettingsChangeNotification();
        }
    }
}