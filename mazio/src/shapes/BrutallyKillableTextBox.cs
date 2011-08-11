using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace mazio.shapes {
    class BrutallyKillableTextBox : RichTextBox {
        private readonly OnLostFocusCb onLostFocus;

        public delegate void OnLostFocusCb();

        public BrutallyKillableTextBox(OnLostFocusCb onLostFocus) {
            this.onLostFocus = onLostFocus;
            LostFocus += brutallyKillableTextBoxLostFocus;
        }

        private void brutallyKillableTextBoxLostFocus(object sender, System.EventArgs e) {
//            Console.WriteLine(Handle + ": lost focus");
            onLostFocus();
            DestroyHandle();
        }
    }
}
