using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using Microsoft.Win32;

namespace mazio.uploaders
{
    public partial class SkitchUpload : Form
    {
        private readonly List<string> maildrops = new List<string>();
        private readonly string fileName;
        private readonly string name;

        private const string REG_MAILDROPS = "SkitchMaildrops";
        private const string REG_LAST_MAILDROP = "LastSkitchMaildropUsed";

        public SkitchUpload(string name, string fileName)
        {
            InitializeComponent();

            Icon = Properties.Resources.trayIcon_Icon;

            this.fileName = fileName;
            this.name = name;

            comboMaildrops.TextChanged += comboMaildrops_TextChanged;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY + "\\" + REG_MAILDROPS);
            if (key != null) {
                foreach (string vname in key.GetValueNames())
                {
                    maildrops.Add(vname); 
                    comboMaildrops.Items.Add(vname);
                }
            }
            key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (key == null) {
                return;
            }
            string last = key.GetValue(REG_LAST_MAILDROP, "").ToString();
            if (last.Length > 0 && maildrops.Contains(last))
                comboMaildrops.SelectedItem = last;
        }

        void configDialogSmtpUserNameChanged(object sender, ConfigDialog.ValueEventArgs<string> e) {
            checkMailSettings();
        }

        void configDialogSmtpServerChanged(object sender, ConfigDialog.ValueEventArgs<string> e) {
            checkMailSettings();
        }

        void comboMaildrops_TextChanged(object sender, EventArgs e)
        {
            buttonUpload.Enabled = comboMaildrops.Text.Length > 0;
        }

        protected override void OnShown(EventArgs e)
        {
            ConfigDialog.SmtpServerChanged += configDialogSmtpServerChanged;
            ConfigDialog.SmtpUserNameChanged += configDialogSmtpUserNameChanged;

            base.OnShown(e);
            checkMailSettings();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ConfigDialog.SmtpServerChanged -= configDialogSmtpServerChanged;
            ConfigDialog.SmtpUserNameChanged -= configDialogSmtpUserNameChanged;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (comboMaildrops.Text.Length > 0)
            {
                if (key != null) {
                    key.SetValue(REG_LAST_MAILDROP, comboMaildrops.Text);
                }
                bool found = false;
                foreach (string maildrop in maildrops)
                {
                    if (!comboMaildrops.Text.Equals(maildrop)) {
                        continue;
                    }
                    found = true;
                    break;
                }
                if (!found) {
                    key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY + "\\" + REG_MAILDROPS);
                    if (key != null) {
                        key.SetValue(comboMaildrops.Text, comboMaildrops.Text);
                    }
                }
            }

            base.OnClosing(e);
        }

        private bool settingsDialogShown;
        private void checkMailSettings()
        {
            string userName = ConfigDialog.Instance.SMTPUserName;
            string smtpServer = ConfigDialog.Instance.SMTPServer;

            if (userName.Length == 0 || smtpServer.Length == 0)
            {
                uploadLog.Text = "Unable to send mail, please enter your mail settings...\r\n";
                buttonCancel.Text = "Close";
                buttonUpload.Enabled = false;
                textDescription.Enabled = false;
                comboMaildrops.Enabled = false;

                ConfigDialog config = ConfigDialog.Instance;
                if (!config.Visible && ! settingsDialogShown)
                {
                    settingsDialogShown = true;
                    config.setMailTabActive();
                    config.ShowDialog();
                }
            }
            else
            {
                uploadLog.Text = "";
                buttonCancel.Text = "Cancel";
                textDescription.Enabled = true;
                comboMaildrops.Enabled = true;
                buttonUpload.Enabled = comboMaildrops.Text.Length > 0;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            string userName = ConfigDialog.Instance.SMTPUserName;
            string password = ConfigDialog.Instance.SMTPPassword;
            string smtpServer = ConfigDialog.Instance.SMTPServer;
            int smtpPort = ConfigDialog.Instance.SMTPPort;
            bool useSsl = ConfigDialog.Instance.SMTPUseSSL;

            SmtpClient client = new SmtpClient
                                {
                                    Host = smtpServer, 
                                    Port = smtpPort, 
                                    EnableSsl = useSsl, 
                                    Credentials = new NetworkCredential(userName, password)
                                };
            client.SendCompleted += sendCompletedCallback;

            string toAddr = comboMaildrops.Text;
            if (checkTwitter.Checked)
                toAddr += "+twitter";
            if (checkPublic.Checked)
                toAddr += "+public";
            toAddr += "@skitch.com";

            try {
                Attachment a = new Attachment(fileName);
                MailAddress from = new MailAddress(userName);
                MailAddress to = new MailAddress(toAddr);
                MailMessage message = new MailMessage(from, to) { Subject = name, Body = textDescription.Text };
                message.Attachments.Add(a);

                uploadLog.Text += "Sending picture to Skitch MailDrop...\r\n";
                buttonUpload.Enabled = false;
                buttonCancel.Enabled = false;
                comboMaildrops.Enabled = false;
                textDescription.Enabled = false;

                client.SendAsync(message, a);
            } catch (Exception ex) {
                uploadLog.Text += ex.Message + "\r\n";
            }
        }

        private bool uploaded;

        private void sendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            Attachment a = (Attachment)e.UserState;
            a.Dispose();

            if (e.Cancelled)
            {
                uploadLog.Text += "Send canceled.\r\n";
            }
            if (e.Error != null)
            {
                uploadLog.Text += e.Error + "\r\n";
            }
            else
            {
                uploadLog.Text += "Success. Go to http://skitch.com to verify";
            }

            buttonUpload.Enabled = false;
            buttonCancel.Enabled = true;
            buttonCancel.Text = "Close";

            uploaded = true;
        }

        private void uploadLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception)
            {
                // let's ignore...
            }
        }

        private void SkitchUpload_Activated(object sender, EventArgs e)
        {
            if (!uploaded)
                checkMailSettings();
        }
    }

    public class SkitchUploadBuilder : UploadType
    {
        public override string getName() { return "Skitch"; }
        public override Form getDialog(string fileName, List<string> texts, NameValueCollection cmdline)
        {
            string name = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            string nameNoExt = name.Substring(0, name.LastIndexOf('.'));
            return new SkitchUpload(nameNoExt, fileName);
        }
    }

}
