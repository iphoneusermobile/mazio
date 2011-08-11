using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using FlickrNet;

namespace mazio.uploaders
{
    public partial class FlickrAuthorizer : Form
    {
        private readonly Flickr flickr;
        private bool authStarted;
        private string authToken;
        private string frob;

        public FlickrAuthorizer(Flickr flickr)
        {
            InitializeComponent();
            this.flickr = flickr;
            Icon = Properties.Resources.trayIcon_Icon;

            textBox.Text = "This program requires your authorization before it can upload data to Flickr\r\n\r\n";
            textBox.Text += "Authorizing is a simple process which takes place in your web browser. ";
            textBox.Text += "When you are finished, return to this window to complete authorization\r\n\r\n";
            textBox.Text += "(You must be connected to the internet in order to authorize this program)";
        }

        private void buttonAuthorize_Click(object sender, EventArgs e)
        {
            if (!authStarted)
            {
                frob = flickr.AuthGetFrob();

                Debug.WriteLine("Frob = " + frob);

                string url = flickr.AuthCalcUrl(frob, AuthLevel.Write);

                Debug.WriteLine("Url = " + url);

                Process.Start(url);

                authStarted = true;
                buttonAuthorize.Text = "Complete Authorization";

                textBox.Text = "Return to this window to complete authorization on Flickr.com\r\n\r\n";
                textBox.Text = "Once you are done, click the \"Complete Authorization\" button below\r\n\r\n";
                textBox.Text += "(You can revoke this program's authorization at any time inyour account page on Flickr.com)";
            }
            else
            {
                buttonCancel.Enabled = false;
                buttonAuthorize.Enabled = false;

                Thread thread = new Thread(runGetAuthToken);
                thread.Start();
            }
        }

        private void runGetAuthToken()
        {
            try
            {
                Auth auth = flickr.AuthGetToken(frob);
                Debug.WriteLine("User Authentcated = " + auth.User.Username);
                Debug.WriteLine("Auth Token = " + auth.Token);

                authToken = auth.Token;
            }
            catch (FlickrException ex)
            {
                Debug.WriteLine("Authentication failed : " + ex.Message);
            }

            Invoke(new MethodInvoker(delegate 
                {
                    DialogResult = DialogResult.OK;
                    Close(); 
                }));
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public string getAuthToken()
        {
            return authToken;
        }
    }
}
