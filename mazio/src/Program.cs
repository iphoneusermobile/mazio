using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace mazio
{
    static class Program
    {
//        private static volatile bool mazioRuns = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                ServicePointManager.ServerCertificateValidationCallback =
                    delegate(Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return (true); };

                Application.Run(new Mazio());
            } catch (NullReferenceException e) {
                new ExceptionDialog(e).ShowDialog();
            }
        }
    }
}