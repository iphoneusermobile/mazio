using System;
using System.Diagnostics;
using System.Net;
using System.Xml.XPath;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace mazio
{
    public partial class NewVersionInfo : Form
    {
        private static string major;
        private static string minor;
        private static string revision;
        private static XPathDocument versionDocument;

        private static string downloadUrl;
        private static string notes;

        private const string REPLACEMENT_NOTES =
            "<html><body><p style=\"font-size:large;font-family:sans-serif;\" align=\"center\">"
            + "New <strong>Mazio.NET</strong> version available</p></body></html>";

        public NewVersionInfo()
        {
            InitializeComponent();

            if (notes == null)
                notes = REPLACEMENT_NOTES;
            web.DocumentText = notes;
            buttonGetIt.Text = "Get Mazio.NET, version " + major + "." + minor + "." + revision;
        }

// ReSharper disable InconsistentNaming
#pragma warning disable 414
        private static string TEST_1 =
#pragma warning restore 414
// ReSharper restore InconsistentNaming
            "<versions>"
            + "<latest-version major=\"0\" minor=\"98\" revision=\"1\" />"
            + "<download-url>http://mazio.googlecode.com/files/mazio-setup-0.98.0.exe</download-url>"
		    + "<release-notes><![CDATA["
            + "<html><body>"
            + "<p align=\"center\">With pleasure, Kalamon presents Mazio.NET 0.98.0</p>"
            + "<p>changes:</p>"
            + "<ul>"
            + "<li>multi-monitor screen grabbing - courtesy <a target=\"_new\" href=\"http://code.google.com/u/olyen2007/\">Olyen2007</a></li>"
            + "<li>vastly improved cropping - <a target=\"_new\" href=\"http://code.google.com/p/mazio/issues/detail?id=34\">bug #34</a></li>"
            + "<li>centering of cropped image</li>"
            + "</ul>"
            + "</body></html>"
            + "]]></release-notes>"
            + "</versions>";

// ReSharper disable InconsistentNaming
#pragma warning disable 414
        private static string TEST_2 =
#pragma warning restore 414
// ReSharper restore InconsistentNaming
            "<versions>"
            + "<latest-version major=\"0\" minor=\"97\" revision=\"1\" />"
            + "</versions>";


        public static bool haveNewVersion()
        {
            bool isNew = false;

            try
            {
#if !TEST_RUN
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://mazio.googlecode.com/svn/trunk/mazio/latest_version.xml");
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream s = resp.GetResponseStream();
                versionDocument = new XPathDocument(s);
#else
                versionDocument= new XPathDocument(new StringReader(TEST_1));
#endif
                XPathNavigator nav = versionDocument.CreateNavigator();
                XPathNodeIterator it = nav.Select("/versions/latest-version");

                if (it.MoveNext())
                {
                    major = it.Current.GetAttribute("major", "");
                    minor = it.Current.GetAttribute("minor", "");
                    revision = it.Current.GetAttribute("revision", "");
                }
                if (major == null || minor == null || revision == null)
                    throw new InvalidDataException();

                int majorVer = int.Parse(major);
                int minorVer = int.Parse(minor);
                int buildNr = int.Parse(revision);

                Version v = Assembly.GetExecutingAssembly().GetName().Version;

                if (majorVer > v.Major)
                    isNew = true;
                if ((majorVer == v.Major) && (minorVer > v.Minor))
                    isNew = true;
                if (majorVer == v.Major && minorVer == v.Minor && buildNr > v.Build)
                    isNew = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return isNew;

        }

        internal static void showNewVersionInfo()
        {
            XPathNavigator nav = versionDocument.CreateNavigator();
            XPathNodeIterator url = nav.Select("/versions/download-url");
            XPathNodeIterator releaseNotes = nav.Select("/versions/release-notes");

            if (url.MoveNext())
                downloadUrl = url.Current.Value;
            else if (major != null && minor != null && revision != null)
                downloadUrl = "http://mazio.googlecode.com/files/mazio-setup-" + major + "." + minor + "." + revision + ".exe";
            else 
                downloadUrl = "http://code.google.com/p/mazio/downloads/list";

            if (releaseNotes.MoveNext())
                notes = releaseNotes.Current.Value;

            if (notes != null)
                Debug.WriteLine(notes);

            NewVersionInfo info = new NewVersionInfo();
            info.ShowDialog();
        }

        private void buttonGetIt_Click(object sender, EventArgs e)
        {
            if (downloadUrl != null)
                web.Navigate(downloadUrl);
        }
    }
}
