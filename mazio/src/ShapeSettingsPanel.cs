using System;
using System.Windows.Forms;
using System.Drawing;

namespace mazio {
    public class ShapeSettingsPanel : Panel {
        public ShapeSettingsPanel() {
            BackColor = SystemColors.Control;
            Dock = DockStyle.Fill;
            TabStop = false;
        }

        public event EventHandler<EventArgs> SettingsChanged;

        protected void fireSettingsChangeNotification() {
            if (SettingsChanged != null) {
                SettingsChanged(this, new EventArgs());
            }
        }

        protected T getSettingValue<T>(string key, T defaultValue) {
            return SettingsWrapper.getSettingValue(key, defaultValue);
        }

        protected void saveSettingValue<T>(string key, T value) {
            SettingsWrapper.saveSettingValue(key, value);
        }
    }
}