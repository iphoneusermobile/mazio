using System;
using System.Drawing;
using System.Windows.Forms;

namespace mazio {
    public partial class CropSettingsPanel : UserControl {

        private const string CONSTRAIN_PROPORTIONS = "CropConstrainProportions";
        private const string CONSTRAIN_TYPE = "CropConstrainType";
        private const string PROP_WIDTH = "CropCustomProportionWidth";
        private const string PROP_HEIGHT = "CropCustomProportionHeight";

        public bool ConstrainProportions { get { return checkConstrainProportions.Checked; } }
        
        public enum ConstraintType {
            CONSTRAIN_NONE,
            CONSTRAIN_SQUARE,
// ReSharper disable InconsistentNaming
            CONSTRAIN_4x3,
            CONSTRAIN_16x9,
// ReSharper restore InconsistentNaming
            CONSTRAIN_CUSTOM
        }

        public ConstraintType Constraint {
            get {
                if (!checkConstrainProportions.Checked) return ConstraintType.CONSTRAIN_NONE;
                if (radioSquare.Checked) return ConstraintType.CONSTRAIN_SQUARE;
                if (radio4x3.Checked) return ConstraintType.CONSTRAIN_4x3;
                if (radio16x9.Checked) return ConstraintType.CONSTRAIN_16x9;
                return ConstraintType.CONSTRAIN_CUSTOM;
            }
        }

        public Size CustomProportions { get { return new Size((int) numericW.Value, (int) numericH.Value); } }

        public event EventHandler ConstraintsChanged;

        public void invokeConstraintsChanged(EventArgs e) {
            EventHandler handler = ConstraintsChanged;
            if (handler != null) handler(this, e);
        }

        public CropSettingsPanel() {
            InitializeComponent();

            loadValues();

            groupProportions.Enabled = checkConstrainProportions.Checked;
            updateCustomProportionsBoxes();
        }

        private void radioCustom_CheckedChanged(object sender, EventArgs e) {
            updateCustomProportionsBoxes();

            saveValues();

            invokeConstraintsChanged(null);
        }

        private void updateCustomProportionsBoxes() {
            bool enabled = radioCustom.Checked;
            numericH.Enabled = enabled;
            numericW.Enabled = enabled;
        }

        private void checkConstrainProportions_CheckedChanged(object sender, EventArgs e) {
            groupProportions.Enabled = checkConstrainProportions.Checked;

            saveValues();

            invokeConstraintsChanged(null);
        }

        private void numericW_ValueChanged(object sender, EventArgs e) {

            saveValues();

            invokeConstraintsChanged(null);
        }

        private void numericH_ValueChanged(object sender, EventArgs e) {

            saveValues();

            invokeConstraintsChanged(null);
        }

        private void radio16X9CheckedChanged(object sender, EventArgs e) {

            saveValues();

            invokeConstraintsChanged(null);
        }

        private void radio4X3CheckedChanged(object sender, EventArgs e) {

            saveValues();

            invokeConstraintsChanged(null);
        }

        private void radioSquare_CheckedChanged(object sender, EventArgs e) {

            saveValues();

            invokeConstraintsChanged(null);
        }

        private void saveValues() {
            SettingsWrapper.saveSettingValue(CONSTRAIN_PROPORTIONS, checkConstrainProportions.Checked ? 1 : 0);
            ConstraintType t = ConstraintType.CONSTRAIN_NONE;
            if (radio4x3.Checked) {
                t = ConstraintType.CONSTRAIN_4x3;
            } else if (radio16x9.Checked) {
                t = ConstraintType.CONSTRAIN_16x9;
            } else if (radioSquare.Checked) {
                t = ConstraintType.CONSTRAIN_SQUARE;
            } else if (radioCustom.Checked) {
                t = ConstraintType.CONSTRAIN_CUSTOM;
            }
            SettingsWrapper.saveSettingValue(CONSTRAIN_TYPE, t);
            SettingsWrapper.saveSettingValue(PROP_WIDTH, numericW.Value);
            SettingsWrapper.saveSettingValue(PROP_HEIGHT, numericH.Value);
        }

        private void loadValues() {
            checkConstrainProportions.Checked = SettingsWrapper.getSettingValue(CONSTRAIN_PROPORTIONS, 0) > 0;
            string val = SettingsWrapper.getSettingValue(CONSTRAIN_TYPE, "");
            try {
                ConstraintType type = (ConstraintType) Enum.Parse(typeof (ConstraintType), val);
                switch (type) {
                    case ConstraintType.CONSTRAIN_4x3:
                        radio4x3.Checked = true;
                        break;
                    case ConstraintType.CONSTRAIN_16x9:
                        radio16x9.Checked = true;
                        break;
                    case ConstraintType.CONSTRAIN_CUSTOM:
                        radioCustom.Checked = true;
                        break;
                    default:
                        radioSquare.Checked = true;
                        break;
                }
            } catch (Exception) {
                radioSquare.Checked = true;
            }
            try {
                numericW.Value = SettingsWrapper.getSettingValue(PROP_WIDTH, 1);
                numericH.Value = SettingsWrapper.getSettingValue(PROP_HEIGHT, 1);
            } catch (Exception) {
                numericH.Value = 1;
                numericW.Value = 1;
            }
        }
    }
}
