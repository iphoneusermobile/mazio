using System.Windows.Forms;

namespace mazio {
    public partial class ShapeSettings : Form {
        private static ShapeSettings instance = new ShapeSettings();

        public static ShapeSettings Instance {
            get { return instance; }
        }

        public bool Showing { get; private set; }

        private ShapeSettings() {
            InitializeComponent();
        }

        private Panel contents;

        public Panel Contents {
            set {
                basePanel.Controls.Clear();
                contents = value;
                if (value != null) {
                    basePanel.Controls.Add(value);
                } else {
                    basePanel.Controls.Add(labelEmpty);
                }
            }
            get {
                return contents;
            }
        }

        private void shapeSettingsLoad(object sender, System.EventArgs e) {
            Showing = true;
        }

        private void shapeSettingsFormClosed(object sender, FormClosedEventArgs e) {
            Showing = false;
            instance = new ShapeSettings {Contents = contents};
        }
    }
}