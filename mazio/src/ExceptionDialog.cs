using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace mazio {
    public partial class ExceptionDialog : Form {
        public ExceptionDialog(Exception e) {
            InitializeComponent();

            textStackTrace.Text = e.ToString();
        }

        private void buttonClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void linkBugReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try {
                Process.Start("http://code.google.com/p/mazio/issues/entry");
            } catch (Exception) {}
        }

        private void exceptionDialogKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (int) Keys.Escape) Close();
        }
    }
}