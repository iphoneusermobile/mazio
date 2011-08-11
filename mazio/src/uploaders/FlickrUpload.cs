using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;

using FlickrNet;

namespace mazio.uploaders
{
    public partial class FlickrUpload : Form
    {
        private const string REG_VAL = "FlickrAuthToken";

        private const string API_KEY = "3860f1b06c43bdcd38bf808ac007897e";
        private const string API_SHARED_SECRET = "7c0918f6d980c73a";

        private static string authToken;

        private Flickr flickr;

        private readonly string fileName;
        private readonly string name;
        private string description;

        private Thread uploadThread;

        public FlickrUpload(string name, string fileName)
        {
            InitializeComponent();

            Icon = Properties.Resources.trayIcon_Icon;

            RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.REG_KEY);
            if (key != null) {
                authToken = (string)key.GetValue(REG_VAL);
            }

            this.fileName = fileName;
            this.name = name;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            if (authToken == null)
            {
                flickr = new Flickr(API_KEY, API_SHARED_SECRET);

                FlickrAuthorizer authorizer = new FlickrAuthorizer(flickr);
                DialogResult res = authorizer.ShowDialog();
                if (res != DialogResult.OK)
                    return;

                authToken = authorizer.getAuthToken();

                if (authToken == null)
                {
                    MessageBox.Show("Authorization failed");
                    return;
                }

                RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
                if (key != null) {
                    key.SetValue(REG_VAL, authToken);
                }
            }


            buttonUpload.Enabled = false;
            buttonCancel.Enabled = false;
            descriptionBox.Enabled = false;

            description = descriptionBox.Text;

            uploadThread = new Thread(runThread);

            uploadLog.Text += "uploading file...\r\n";

            uploadThread.Start();
        }

        public void runThread()
        {
            try
            {
                flickr = new Flickr(API_KEY, API_SHARED_SECRET, authToken);
                upload();
            }
            catch (FlickrException ex)
            {
                uploadLog.Text += "File sending failed : " + ex.Message + "\r\n";

                RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.REG_KEY, true);
                if (key != null) {
                    key.DeleteValue(REG_VAL);
                }
            }

            Invoke(new MethodInvoker(delegate 
                {
                    buttonCancel.Text = "Close";
                    buttonCancel.Enabled = true;
                }));
        }

        private void upload()
        {
            if (flickr == null)
                return;
            string photoId = flickr.UploadPicture(fileName, name, description, "", true, false, false);
            PhotoInfo info = flickr.PhotosGetInfo(photoId);

            uploadLog.Invoke(new MethodInvoker(delegate 
                { 
                    uploadLog.Text += "uploaded photo: " + info.WebUrl + "\r\n";
                }));
        }

        private void uploadLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch(Exception)
            {
                // ignore
            }
        }
    }

    public class FlickrUploadBuilder : UploadType
    {
        public override string getName() { return "Flickr"; }
        public override Form getDialog(string fileName, List<string> texts, NameValueCollection cmdline)
        {
            string name = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            string nameNoExt = name.Substring(0, name.LastIndexOf('.'));
            return new FlickrUpload(nameNoExt, fileName);
        }
    }
}
