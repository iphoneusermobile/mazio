using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

using mazio.jira;
using mazio.util;

namespace mazio.uploaders
{
    public partial class JiraUpload : Form
    {
        private readonly string name;
        private readonly string path;

        private const string REG_JIRA_ACCOUNTS = "JiraAccounts";
        private const string REG_LAST_JIRA_ACCOUNT = "LastJiraAccount";
        private const string REG_USER = "JiraUser";
        private const string REG_PASSWORD = "JiraPassword";
        private const string REG_SERVER = "JiraServer";
        private const string REG_LAST_ISSUE = "JiraIssue";

        private string issueKey;
        private string serverUrl;
        private string userLogin;
        private string userPassword;
        private string comment;

        private string issueId;
        private string jSessionId;
        private string token;

        private class JiraAccount
        {
            public JiraAccount(string url, string login, string password)
            {
                this.url = url;
                this.login = login;
                this.password = password;
            }

            public readonly string url;
            public readonly string login;
            public readonly string password;
        }

        private readonly Dictionary<string, JiraAccount> accounts = new Dictionary<string, JiraAccount>();

        private bool done;

        public JiraUpload(string fileName, string filePath, IEnumerable<string> texts, NameValueCollection cmdline)
        {
            InitializeComponent();

            Icon = Properties.Resources.trayIcon_Icon;

            name = fileName;
            path = filePath;

            buttonUpload.Enabled = false;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (key != null) {
                RegistryKey keyAccounts = key.CreateSubKey(REG_JIRA_ACCOUNTS);
                if (keyAccounts != null)
                    foreach (string k in keyAccounts.GetSubKeyNames())
                    {
                        RegistryKey subkey = keyAccounts.OpenSubKey(k);

                        if (subkey != null) {
                            string url = (string)subkey.GetValue(REG_SERVER);
                            string user = (string)subkey.GetValue(REG_USER);
                            string passwd = Rot13.Rotate((string)subkey.GetValue(REG_PASSWORD, ""));

                            accounts.Add(k, new JiraAccount(url, user, passwd));
                        }
                        comboEntryName.Items.Add(k);
                    }
            }
            if (key != null) {
                string lastAccount = (string)key.GetValue(REG_LAST_JIRA_ACCOUNT, "");
                textIssueNumber.Text = (string)key.GetValue(REG_LAST_ISSUE, "");

                if (accounts.ContainsKey(lastAccount))
                    comboEntryName.SelectedItem = lastAccount;
            }

            setupFromCommandLine(cmdline);

            if (texts != null)
                foreach (string text in texts)
                    commentBox.Text += text + "\r\n";

            updateButtons();
        }

        private const string FROM_WEB = "!FROM WEB!";

        private void setupFromCommandLine(NameValueCollection cmdline)
        {
            if (cmdline == null)
                return;

            string type = cmdline["type"];
            if (type == null || !type.Equals("jira"))
                return;

            string user = cmdline["user"];
            string host = cmdline["host"];
            if (host == null)
                return;

            issueKey = cmdline["issueKey"];

            issueId = cmdline["id"];
            jSessionId = cmdline["JSESSIONID"] ?? cmdline["jsessionid"];
            token = cmdline["soaptoken"] ?? cmdline["soaptoken"];

            bool found = false;
            foreach (string a in accounts.Keys)
            {
                if (!accounts[a].url.Equals(host)) continue;

                found = true;
                comboEntryName.SelectedItem = a;
                break;
            }

            if (!found)
            {
                accounts[FROM_WEB] = new JiraAccount(host, user, null);
                comboEntryName.Items.Add(FROM_WEB);
                comboEntryName.SelectedItem = FROM_WEB;
                textPassword.Focus();
            }

            textIssueNumber.Text = issueKey;

            StartPosition = FormStartPosition.CenterParent;

            if ((issueId != null && jSessionId != null) || (token != null && issueKey != null)) {
                textIssueNumber.Enabled = false;
                textLogin.Enabled = false;
                textPassword.Enabled = false;
                textServerUrl.Enabled = false;
                comboEntryName.Enabled = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                checkStorePassword.Enabled = false;
                checkStorePassword.Checked = false;
                commentBox.Enabled = false;

                ClientSize = new Size(ClientSize.Width / 2, ClientSize.Height - commentBox.Height - groupBoxAccounts.Height - 60);

                commentBox.Visible = false;
                groupBoxComment.Visible = false;
                groupBoxAccounts.Visible = false;
                comboEntryName.Visible = false;
                textServerUrl.Visible = false;
                btnDelete.Visible = false;
                btnSave.Visible = false;
                labelUserName.Visible = false;
                labelPassword.Visible = false;
                labelEntryName.Visible = false;
                labelIssueNumber.Visible = false;
                labelServerUrl.Visible = false;
                checkStorePassword.Visible = false;
                textIssueNumber.Visible = false;
                textLogin.Visible = false;
                textPassword.Visible = false;
                buttonUpload.Visible = false;
            }
        }

        private void saveLastUsed()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);

            if (accounts.ContainsKey(comboEntryName.Text))
                if (key != null) key.SetValue(REG_LAST_JIRA_ACCOUNT, comboEntryName.Text);

            if (key != null) key.SetValue(REG_LAST_ISSUE, textIssueNumber.Text);
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            upload();
        }

        private void upload() {
            buttonUpload.Enabled = false;
            buttonCancel.Enabled = false;
            comboEntryName.Enabled = false;
            textIssueNumber.Enabled = false;
            textServerUrl.Enabled = false;
            textLogin.Enabled = false;
            textPassword.Enabled = false;
            commentBox.Enabled = false;
            checkStorePassword.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;

            uploadLog.Text = "uploading to JIRA...\r\n";

            issueKey = textIssueNumber.Text;
            serverUrl = textServerUrl.Text;
            userLogin = textLogin.Text;
            userPassword = textPassword.Text;
            comment = commentBox.Text;

            Thread thr = token != null ? new Thread(jiraUploadBySoapWithToken) : new Thread(jiraUploadByPost);
            thr.Start();
        }

        private void jiraUploadBySoapWithToken()
        {
            try {
                JiraSoapServiceService service = new JiraSoapServiceService { Url = serverUrl + "/rpc/soap/jirasoapservice-v2" };

                string[] fileNames = new[] { name };
                byte[] bytes = File.ReadAllBytes(path);

                service.addBase64EncodedAttachmentsToIssue(token, issueKey, fileNames, new[] { Convert.ToBase64String(bytes) });
                if (!string.IsNullOrEmpty(comment) && comment.Trim().Length > 0) {
                    RemoteComment c = new RemoteComment {body = comment};
                    service.addComment(token, issueKey, c);
                }

                Invoke(new MethodInvoker(delegate 
                    {
                        uploadLog.Text += "Done. See " + serverUrl + "/browse/" + issueKey + "\r\n";
                    }));
            } catch (Exception e) {
                Debug.Write(e.StackTrace);
                Invoke(new MethodInvoker(delegate
                    {
                        uploadLog.Text += "Failed: " + e.Message + "\r\n";
                    }));
            }

            done = true;

            Invoke(new MethodInvoker(delegate
                {
                    saveLastUsed();
                    buttonCancel.Enabled = true;
                    buttonCancel.Text = "Close";
                }));
        }

        private void jiraUploadByPost() {
            try {
                JiraSoapServiceService service = new JiraSoapServiceService { Url = serverUrl + "/rpc/soap/jirasoapservice-v2" };
                string tok = null;

                if (issueId == null) {
                    tok = service.login(userLogin, userPassword);

                    RemoteIssue issue = service.getIssue(tok, issueKey);
                    issueId = issue.id;
                }

                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Dictionary<string, object> postParameters = new Dictionary<string, object>
                                                {
                                                    {"filename.1", new FormUpload.FileParameter(data, name, getMimeType(name))},
                                                };

                const string userAgent = "Mazio";

                string postUrl = 
                    serverUrl + "/secure/AttachFile.jspa?id=" + issueId 
                    + (jSessionId == null 
                        ? ("&os_username=" + HttpUtility.UrlEncode(userLogin) + "&os_password=" + HttpUtility.UrlEncode(userPassword))
                        : "");

                HttpWebResponse webResponse;
                if (jSessionId != null) {
                    Cookie cookie = new Cookie("JSESSIONID", jSessionId, "/jira", serverUrl.Substring(0, serverUrl.LastIndexOf("/")));
                    webResponse = FormUpload.MultipartFormDataPost(postUrl, userAgent, postParameters, cookie);
                } else {
                    webResponse = FormUpload.MultipartFormDataPost(postUrl, userAgent, postParameters);
                }

                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
#pragma warning disable 168
                string result = responseReader.ReadToEnd();
#pragma warning restore 168
                webResponse.Close();

                if (token != null && !string.IsNullOrEmpty(comment)) {
                    service.addComment(tok, issueKey, new RemoteComment { body = comment });
                }

                Invoke(new MethodInvoker(delegate { uploadLog.Text += "Done. See " + serverUrl + "/browse/" + issueKey + "\r\n"; }));

            } catch (Exception e) {
                Debug.Write(e.StackTrace);
                Invoke(new MethodInvoker(delegate { uploadLog.Text += "Failed: " + e.Message + "\r\n"; }));
            }
            done = true;

            Invoke(new MethodInvoker(delegate {
                    saveLastUsed();
                    buttonCancel.Enabled = true;
                    buttonCancel.Text = "Close";
                }));
        }

        private static string getMimeType(string fileName) {
            string mimeType;
            switch (fileName.Substring(Math.Max(fileName.IndexOf("."), 0))) {
                case ".png":
                    mimeType = "image/png";
                    break;
                case "jpg":
                    mimeType = "image/jpeg";
                    break;
                default:
                    mimeType = "application/octet-stream";
                    break;
            }
            return mimeType;
        }

        private void comboEntryName_TextChanged(object sender, EventArgs e)
        {
            if (accounts.ContainsKey(comboEntryName.Text))
            {
                JiraAccount a = accounts[comboEntryName.Text];
                textServerUrl.Text = a.url;
                textLogin.Text = a.login;
                textPassword.Text = a.password;

                saveLastUsed();
            }

            updateButtons();
        }

        private void updateButtons()
        {
            buttonUpload.Enabled = !done 
                && (textServerUrl.Text.Length > 0)
                && (textIssueNumber.Text.Length > 0) 
                && (textLogin.Text.Length > 0);

            btnDelete.Enabled = !done && comboEntryName.Text.Length > 0 && (issueId == null || (jSessionId == null && token == null));
            btnSave.Enabled = !done 
                && comboEntryName.Text.Length > 0 
                && textServerUrl.Text.Length > 0
                && textLogin.Text.Length > 0 && (issueId == null || (jSessionId == null && token == null));
        }

        private void userName_TextChanged(object sender, EventArgs e)
        {
            updateButtons();
        }

        private void uploadLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JiraUpload.uploadLog_LinkClicked() - exception: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (key != null) {
                RegistryKey keyAccounts= key.CreateSubKey(REG_JIRA_ACCOUNTS);
                if (keyAccounts != null) {
                    RegistryKey sub = keyAccounts.CreateSubKey(comboEntryName.Text);
                    if (sub != null) sub.SetValue(REG_USER, textLogin.Text);
                    string pwd = textPassword.Text;
                    if (pwd.Length > 0)
                        pwd = Rot13.Rotate(pwd);
                    if (checkStorePassword.Checked)
                        if (sub != null) sub.SetValue(REG_PASSWORD, pwd);
                    if (sub != null) {
                        sub.SetValue(REG_SERVER, textServerUrl.Text);
                        sub.SetValue(REG_LAST_ISSUE, textIssueNumber.Text);
                    }
                }
            }
            if (key != null) key.SetValue(REG_LAST_JIRA_ACCOUNT, comboEntryName.Text);

            accounts[comboEntryName.Text] = new JiraAccount(textServerUrl.Text, textLogin.Text, textPassword.Text);
            if (!comboEntryName.Items.Contains(comboEntryName.Text))
                comboEntryName.Items.Add(comboEntryName.Text);
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY + "\\" + REG_JIRA_ACCOUNTS);
            if (key == null) return;
            foreach (string subName in key.GetSubKeyNames()) {
                if (!subName.Equals(comboEntryName.Text)) continue;
                key.DeleteSubKey(subName);
                accounts.Remove(comboEntryName.Text);
                comboEntryName.Items.Remove(comboEntryName.Text);
                break;
            }
        }

        private void textServerUrl_TextChanged(object sender, EventArgs e)
        {
            updateButtons();
        }

        private void textIssueNumber_TextChanged(object sender, EventArgs e)
        {
            updateButtons();
        }

        private void jiraUploadLoad(object sender, EventArgs e) {
            if ((issueId != null && jSessionId != null) || (issueKey != null && token != null)) {
                upload();
            }
        }
    }

    public class JiraUploadBuilder : UploadType
    {
        public override string getName() { return "Jira Server"; }
        public override Form getDialog(string fileName, List<string> texts, NameValueCollection cmdline)
        {
            string name = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            return new JiraUpload(name, fileName, texts, cmdline);
        }
    }

    public class JiraCommandLineParser : ServerTypeCommandLineParser
    {
        private static readonly Regex userAndRest = new Regex(@"jira:((\w+)@)?(.+)"); 
        private static readonly Regex keyAndParams = new Regex(@"(\w+-\d+)(\?(.+))?");

        public NameValueCollection getParams(string cmdline) {
            NameValueCollection nvc = new NameValueCollection {{"type", "jira"}};

            if (userAndRest.IsMatch(cmdline)) {
                Match m = userAndRest.Match(cmdline);
                if (m.Groups[2].Success) {
                    nvc.Add("user", m.Groups[2].Value);    
                }
                string rest = m.Groups[3].Value;

                int sepIdx = rest.LastIndexOf(":");
                nvc.Add("host", rest.Substring(0, sepIdx));

                if (keyAndParams.IsMatch(rest)) {
                    m = keyAndParams.Match(rest);
                    string key = m.Groups[1].Value;
                    nvc.Add("issueKey", key);
                    if (m.Groups[3].Success) {
                        string[] pairs = m.Groups[3].Value.Split(new[] { '&' }, 2);
                        foreach (var pair in pairs) {
                            string[] keyVal = pair.Split(new[] {'='}, 2);
                            if (keyVal.Length == 2) nvc.Add(keyVal[0], keyVal[1]);
                        }
                    }
                }
            }
            return nvc;
        }
    }
}
