using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;
using System.IO;

using Google.GData.Photos;
using Google.GData.Client;
using mazio.util;

namespace mazio.uploaders
{
    public partial class PicasawebUpload : Form
    {
        private readonly Dictionary<string, string> accounts = new Dictionary<string, string>();
        private readonly PicasaService service;
        private readonly string fileName;
        private readonly string name;
        private readonly Dictionary<string, AlbumAccessor> albumMap = new Dictionary<string, AlbumAccessor>();

        private const string DROPBOX_URL = "http://picasaweb.google.com/data/feed/api/user/default/albumid/default";

        private const string REG_ACCOUNTS = "PicasawebAccounts";
        private const string REG_LAST_ACCOUNT = "LastPicasawebAccount";

        private bool done;

        public PicasawebUpload(string name, string fileName)
        {
            InitializeComponent();

            Icon = Properties.Resources.trayIcon_Icon;

            comboLogin.TextChanged += comboLogin_TextChanged;
            service = new PicasaService("Kalamon-Mazio");
            this.fileName = fileName;
            this.name = name;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            RegistryKey keyAccounts = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY + "\\" + REG_ACCOUNTS);
            if (keyAccounts != null) {
                foreach (string login in keyAccounts.GetValueNames())
                {
                    string password = keyAccounts.GetValue(login).ToString();
                    if (password.Length > 0)
                        password = Rot13.Rotate(password);
                    accounts.Add(login, password);
                    comboLogin.Items.Add(login);
                }
            }
            if (key != null) {
                string lastAccount = (string) key.GetValue(REG_LAST_ACCOUNT, "");
                if (accounts.ContainsKey(lastAccount))
                    comboLogin.SelectedItem = lastAccount;
            }

            updateItems();
        }

        void comboLogin_TextChanged(object sender, EventArgs e)
        {
            if (accounts.ContainsKey(comboLogin.Text))
                textPassword.Text = accounts[comboLogin.Text];

            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (key != null) {
                key.SetValue(REG_LAST_ACCOUNT, comboLogin.Text);
            }

            updateItems();
        }

        private void buttonGetAlbumList_Click(object sender, EventArgs e)
        {
            authenticate();

            string userName = comboLogin.Text;

            disableItemsForRun();

            Thread thr = new Thread(new ThreadStart(delegate
                {
                    try
                    {
                        AlbumQuery query = new AlbumQuery(PicasaQuery.CreatePicasaUri(userName));

                        PicasaFeed feed = service.Query(query);

                        Invoke(new MethodInvoker(delegate
                        {
                            listAlbums.Items.Clear();
                            albumMap.Clear();

                            foreach (PicasaEntry entry in feed.Entries)
                            {
                                AlbumAccessor ac = new AlbumAccessor(entry);
                                listAlbums.Items.Add(ac.AlbumTitle);
                                albumMap[ac.AlbumTitle] = ac;
                            }

                            if (feed.Entries.Count > 0)
                                listAlbums.SelectedIndex = 0;

                            buttonGetAlbumList.Enabled = true;

                            updateItems();
                        }));
                    }
                    catch (Exception ex)
                    {
                        Invoke(new MethodInvoker(delegate 
                            { 
                                uploadLog.Text = "Album list upload failed: " + ex.Message + "\r\n";

                                listAlbums.Items.Clear();
                                albumMap.Clear();

                                updateItems(); 
                            }));
                    }
                }));
            thr.Start();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            authenticate();

            string userName = comboLogin.Text;
            string selectedAlbum = checkUploadToDropbox.Checked ? null : listAlbums.SelectedItem.ToString();

            disableItemsForRun();

            Thread thr = new Thread(new ThreadStart(delegate
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    FileStream fileStream = fileInfo.OpenRead();

                    PicasaEntry entry = null;

                    try
                    {
                        if (checkUploadToDropbox.Checked)
                            entry = (PicasaEntry)service.Insert(new Uri(DROPBOX_URL), fileStream, "image/jpeg", fileName);
                        else
                        {
                            if (selectedAlbum != null) {
                                Uri postUri = new Uri(PicasaQuery.CreatePicasaUri(
                                                          userName, albumMap[selectedAlbum].Name));

                                entry = (PicasaEntry)service.Insert(postUri, fileStream, "image/jpeg", fileName);
                            }
                        }

                        if (entry != null) {
                            entry.Title.Text = name;
                            entry.Summary.Text = textComment.Text;
                            entry.Media.Keywords.Value = "Mazio";
                        }

                        entry = (PicasaEntry)service.Update(entry);

                        string contentUrl = entry.Media.Content.Attributes["url"] as string;

                        Invoke(new MethodInvoker(delegate
                            {
                                uploadLog.Text = "Uploaded photo, URL is " + contentUrl + "\r\n";
                            }));
                    }
                    catch (GDataRequestException ex)
                    {
                        Invoke(new MethodInvoker(delegate
                           {
                               uploadLog.Text = "Upload failed: " + ex.Message +
                                   "\r\nResponse: " + ex.ResponseString + "\r\n";
                           }));
                    }
                    catch (IOException ex)
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            uploadLog.Text = "Upload failed: " + ex.Message +
                                "\r\nResponse: " + ex.InnerException.Message + "\r\n";
                        }));
                    }

                    fileStream.Close();
                    Invoke(new MethodInvoker(delegate
                       {
                           done = true;
                           updateItems();
                           buttonOk.Enabled = false;
                           buttonCancel.Text = "Close";
                       }));
                }));
            thr.Start();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void authenticate()
        {
            service.setUserCredentials(comboLogin.Text, textPassword.Text);
        }

        private void updateItems()
        {
            buttonOk.Enabled = 
                (comboLogin.Text.Length > 0) 
                && ((listAlbums.Items.Count > 0) || checkUploadToDropbox.Checked) && !done;
            buttonGetAlbumList.Enabled = comboLogin.Text.Length > 0 && !done; 
            textComment.Enabled =  !done;
            listAlbums.Enabled = !done;
            btnSave.Enabled = comboLogin.Text.Length > 0 && !done;
            btnDelete.Enabled = comboLogin.Text.Length > 0 && !done;
            comboLogin.Enabled = !done;
            checkStorePassword.Enabled = !done;
            checkUploadToDropbox.Enabled = !done;
            textPassword.Enabled = !done;
            buttonCancel.Enabled = true;
        }

        private void disableItemsForRun()
        {
            buttonGetAlbumList.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            textComment.Enabled = false;
            listAlbums.Enabled = false;
            comboLogin.Enabled = false;
            textPassword.Enabled = false;
            buttonOk.Enabled = false;
            buttonCancel.Enabled = false;
            checkStorePassword.Enabled = false;
            checkUploadToDropbox.Enabled = false;

        }

        private void checkUploadToDropbox_CheckedChanged(object sender, EventArgs e)
        {
            updateItems();
        }

        private void uploadLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception)
            {
                // ignore
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY + "\\" + REG_ACCOUNTS);
            string passwd = checkStorePassword.Checked? Rot13.Rotate(textPassword.Text) : "";
            if (key != null) {
                key.SetValue(comboLogin.Text, passwd);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY + "\\" + REG_ACCOUNTS);
            if (key != null) {
                key.DeleteValue(comboLogin.Text);
            }
            accounts.Remove(comboLogin.Text);
            comboLogin.Text = "";
            textPassword.Text = "";
        }
    }

    public class PicasawebUploadBuilder : UploadType
    {
        public override string getName() { return "Picasaweb"; }
        public override Form getDialog(string fileName, List<string> texts, NameValueCollection cmdline)
        {
            string name = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            return new PicasawebUpload(name, fileName);
        }
    }

}
