using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace mazio.uploaders
{
    public partial class MeetingRoomUpload : Form
    {
        private readonly string filePath;

        private string room;
        private string url;
        private string jsessionId;

// ReSharper disable UnusedParameter.Local
        public MeetingRoomUpload(string filePath, string fileName, NameValueCollection cmdline)
// ReSharper restore UnusedParameter.Local
        {
            InitializeComponent();

            Icon = Properties.Resources.trayIcon_Icon;

            this.filePath = filePath;

            setupFromCommandLine(cmdline);

            StartPosition = FormStartPosition.CenterParent;
        }

        private void setupFromCommandLine(NameValueCollection cmdline)
        {
            if (cmdline == null)
                return;

            string type = cmdline["type"];
            if (type == null || !type.Equals("meetingroom"))
                return;

            room = cmdline["roomId"];
            jsessionId = cmdline["JSESSIONID"] ?? cmdline["jsessionid"];
            url = cmdline["url"] + "?room=" + room + "&JSESSIONID=" + jsessionId;
        }

        private void meetingRoomUpload()
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Dictionary<string, object> postParameters = new Dictionary<string, object>
                                                {
                                                    {"room", room},
                                                    {"file", new FormUpload.FileParameter(data, filePath)},
                                                    {"jsessionId", jsessionId}
                                                };

                const string userAgent = "Mazio";

                Cookie cookie = new Cookie("JSESSIONID", jsessionId, "/", url.Substring(0, url.LastIndexOf("/")));

                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(url, userAgent, postParameters, cookie);

                // Process response
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                responseReader.ReadToEnd();
                webResponse.Close();
                Invoke(new MethodInvoker(delegate 
                    {
                        uploadLog.Text += "Done. Result is " + webResponse.StatusCode + "\r\n";
                    }));
            }
            catch (Exception e)
            {
                Debug.Write(e.StackTrace);
                Invoke(new MethodInvoker(delegate
                    {
                        uploadLog.Text += "Failed: " + e.Message + "\r\n";
                    }));
            }

            Invoke(new MethodInvoker(delegate
                {
                    buttonCancel.Enabled = true;
                    buttonCancel.Text = "Close";
                }));
        }

        private void MeetingRoomUpload_Load(object sender, EventArgs e) {
            buttonCancel.Enabled = false;
            uploadLog.Text = "uploading to Meeting Room server...\r\n";
            Thread thr = new Thread(meetingRoomUpload);
            thr.Start();
        }
    }

    public class MeetingRoomUploadBuilder : UploadType
    {
        public override string getName() { return "Meeting Room"; }
        public override Form getDialog(string fileName, List<string> texts, NameValueCollection cmdline)
        {
            string name = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            return new MeetingRoomUpload(fileName, name, cmdline);
        }
    }

    public class MeetingRoomCommandLineParser : ServerTypeCommandLineParser
    {
        private static readonly Regex userAndRest = new Regex(@"meetingroom:(\d+)@(.+)");
        private static readonly Regex hostAndParams = new Regex(@"(.+)\?(.+)");

        public NameValueCollection getParams(string cmdline) {
            NameValueCollection nvc = new NameValueCollection { { "type", "meetingroom" } };

            if (userAndRest.IsMatch(cmdline)) {
                Match m = userAndRest.Match(cmdline);
                if (m.Groups[1].Success) {
                    nvc.Add("roomId", m.Groups[1].Value);
                }
                string rest = m.Groups[2].Value;

                if (hostAndParams.IsMatch(rest)) {
                    m = hostAndParams.Match(rest);
                    string url = m.Groups[1].Value;
                    nvc.Add("url", url);
                    if (m.Groups[2].Success) {
                        string[] pairs = m.Groups[2].Value.Split(new[] { '&' }, 2);
                        foreach (var pair in pairs) {
                            string[] keyVal = pair.Split(new[] { '=' }, 2);
                            if (keyVal.Length == 2) nvc.Add(keyVal[0], keyVal[1]);
                        }
                    }
                }
            }
            return nvc;
        }
    }
}
