using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace mazio {
    class BackgroundTransformator {

        private static readonly BackgroundTransformator instance = new BackgroundTransformator();
        
        public static BackgroundTransformator Instance { get { return instance; } }

        public float RotationAngle { get; set; }
        public float RotationXOffset { get; set; }
        public float RotationYOffset { get; set; }
        public float PerspectiveRatio { get; set; }
        public float PerspectiveFarSide { get; set; }
        public bool DrawPivotPoint { get; set; }

        public Matrix getTransform(Size s) {
            Matrix m = new Matrix();
            m.RotateAt(RotationAngle, new PointF((s.Width + RotationXOffset * s.Width) / 2, (s.Height + RotationYOffset * s.Width) / 2));
            return m;
        }

        public Point getPivotPoint(Size s) {
            return new Point((int) ((s.Width + RotationXOffset * s.Width) / 2), (int) ((s.Height + RotationYOffset * s.Width) / 2));
        }

        private BackgroundTransformatorSettings settingsDlg;

        public event EventHandler<EventArgs> SettingsChanged;

        private void invokeSettingsChanged(EventArgs e) {
            EventHandler<EventArgs> handler = SettingsChanged;
            if (handler != null) handler(this, e);
        }

        private BackgroundTransformator() {
            PerspectiveFarSide = 1f;   
        }

        public void openSettings() {
            if (settingsDlg != null) {
                settingsDlg.BringToFront();
                return;
            }
            settingsDlg = new BackgroundTransformatorSettings(settingsClosed);
            settingsDlg.SettingsChanged += instance.settingsDlg_SettingsChanged;
            settingsDlg.Show();
        }

        private void settingsDlg_SettingsChanged(object sender, EventArgs e) {
            invokeSettingsChanged(e);
        }

        public void closeSettings() {
            if (settingsDlg != null) {
                settingsDlg.Close();
            }
        }

        private void settingsClosed() {
            settingsDlg = null;
        }

        public void resetAll() {
            RotationAngle = 0;
            RotationXOffset = 0;
            RotationYOffset = 0;
            PerspectiveRatio = 0;
            PerspectiveFarSide = 1;
            DrawPivotPoint = false;
            closeSettings();
        }
    }
}
