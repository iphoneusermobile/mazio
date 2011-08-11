using System;
using System.Windows.Forms;

namespace mazio {
    public partial class BackgroundTransformatorSettings : Form {

        private readonly OnClose onClose;
        public delegate void OnClose();

        public BackgroundTransformatorSettings(OnClose onClose) {
            this.onClose = onClose;
            InitializeComponent();
        }

        private void backgroundTransformatorSettingsFormClosed(object sender, FormClosedEventArgs e) {
            if (onClose != null) onClose();
        }

        private void buttonResetRotation_Click(object sender, EventArgs e) {
            trackVerticalRotationOffset.Value = 0;
            trackHorizontalRotationOffset.Value = 0;
            trackRotationAngle.Value = 0;
        }

        public event EventHandler<EventArgs> SettingsChanged;

        private void invokeSettingsChanged(EventArgs e) {
            EventHandler<EventArgs> handler = SettingsChanged;
            if (handler != null) handler(this, e);
        }

        private void trackHorizontalRotationOffset_ValueChanged(object sender, EventArgs e) {
            BackgroundTransformator.Instance.RotationXOffset = trackHorizontalRotationOffset.Value * 1f / 100;
            invokeSettingsChanged(e);
        }

        private void trackVerticalRotationOffset_ValueChanged(object sender, EventArgs e) {
            BackgroundTransformator.Instance.RotationYOffset = trackVerticalRotationOffset.Value * 1f / 100;
            invokeSettingsChanged(e);
        }

        private void trackRotationAngle_ValueChanged(object sender, EventArgs e) {
            BackgroundTransformator.Instance.RotationAngle = trackRotationAngle.Value;
            invokeSettingsChanged(e);
        }

        private void trackPerspective_ValueChanged(object sender, EventArgs e) {
            BackgroundTransformator.Instance.PerspectiveRatio = trackPerspective.Value * 1f / 100;

            BackgroundTransformator.Instance.PerspectiveFarSide = 
                1 - Math.Abs(BackgroundTransformator.Instance.PerspectiveRatio) * 1f * trackPerspective.Maximum / 100;

            invokeSettingsChanged(e);
        }

        private void buttonResetPerspective_Click(object sender, EventArgs e) {
            trackPerspective.Value = 0;
        }

        private void checkDrawPivotPoint_CheckedChanged(object sender, EventArgs e) {
            BackgroundTransformator.Instance.DrawPivotPoint = checkDrawPivotPoint.Checked;
            invokeSettingsChanged(e);
        }
    }
}
