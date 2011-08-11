using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

using mazio.util;

namespace mazio
{
    public partial class ConfigDialog : Form
    {
        private const string REG_JPEG_QUALITY = "JpegQuality";
        private const string REG_GRAB_MARGIN = "GrabMargin";
        
        private const string REG_SMTP_HOST = "SmtpHost";
        private const string REG_SMTP_PORT = "SmtpPort";
        private const string REG_SMTP_USE_SSL = "SmtpUseSsl";
        private const string REG_SMTP_USER = "SmtpUser";
        private const string REG_SMTP_PASSWORD = "SmtpPassword";

        private const string REG_SHOW_HINTS = "ShowUsageHints";
        private const string REG_LIMIT_GRAB = "LimitGrabByActiveScreen";
        private const string REG_SHOW_GRAB_MAGNIFIER = "ShowGrabMagnifier";

        private const string REG_MAGNIFIER_ZOOM = "MagnifierZoom";

        private int jpegQuality = Constants.DEFAULT_JPEG_QUALITY;
        private int grabMargin = Constants.DEFAULT_GRAB_MARGIN;

        private string smtpServer;
        private int smtpPort;
        private string smtpUserName;
        private string smtpPassword;
        private bool smtpUseSsl;

        private bool showUsageHints;
        private bool limitGrabByActiveScreen;
        private bool showGrabMagnifier;

        private int magnifierZoomFactor;

        private static ConfigDialog instance = new ConfigDialog();

        public static ConfigDialog Instance { get { return instance; } }

        public int GrabMargin { get {
            return 50;// return grabMargin; 
            } 
        }

        public int JpegQuality { get { return jpegQuality; } }

// ReSharper disable InconsistentNaming
        public string SMTPServer { get { return smtpServer; } }
        public int SMTPPort { get { return smtpPort; } }
        public string SMTPUserName { get { return smtpUserName; } }
        public string SMTPPassword { get { return smtpPassword; } }
        public bool SMTPUseSSL { get { return smtpUseSsl; } }
// ReSharper restore InconsistentNaming

        public bool ShowUsageHints { get { return showUsageHints; } }
        public bool LimitGrabByActiveScreen { get { return limitGrabByActiveScreen; } }

        public int MagnifierZoomFactor { get { return magnifierZoomFactor; } }

        public bool ShowGrabMagnifier { get { return showGrabMagnifier; } }

        public void setMailTabActive()
        {
            tabbedPanelSettings.SelectedTab = tabMail;
        }

        private void loadRegistrySettings()
        {
            try
            {
                
                jpegQuality = Math.Max(25, Math.Min(100, SettingsWrapper.getSettingValue(REG_JPEG_QUALITY, Constants.DEFAULT_JPEG_QUALITY)));
                grabMargin = Math.Max(0, Math.Min(2 * Constants.DEFAULT_GRAB_MARGIN,
                                                    SettingsWrapper.getSettingValue(REG_GRAB_MARGIN, Constants.DEFAULT_GRAB_MARGIN)));

                smtpServer = SettingsWrapper.getSettingValue(REG_SMTP_HOST, "");
                smtpPort = SettingsWrapper.getSettingValue(REG_SMTP_PORT, 25);
                smtpUseSsl = SettingsWrapper.getSettingValue(REG_SMTP_USE_SSL, 0) != 0;
                smtpUserName = SettingsWrapper.getSettingValue(REG_SMTP_USER, "");
                smtpPassword = Rot13.Rotate(SettingsWrapper.getSettingValue(REG_SMTP_PASSWORD, ""));

                showUsageHints = SettingsWrapper.getSettingValue(REG_SHOW_HINTS, 1) != 0;
                limitGrabByActiveScreen = SettingsWrapper.getSettingValue(REG_LIMIT_GRAB, 0) != 0;

                magnifierZoomFactor = Math.Min(Constants.MAX_MAGNIFIER_ZOOM,
                                                Math.Max(Constants.MIN_MAGNIFIER_ZOOM,
                                                        SettingsWrapper.getSettingValue(REG_MAGNIFIER_ZOOM, Constants.DEFAULT_MAGNIFIER_ZOOM)));
                showGrabMagnifier = SettingsWrapper.getSettingValue(REG_SHOW_GRAB_MAGNIFIER, 1) != 0;
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace); 
            }
        }

        private ConfigDialog()
        {
            loadRegistrySettings();

            InitializeComponent();

            Icon = Properties.Resources.trayIcon_Icon;

            trackGrabMargin.Minimum = 0;
            trackGrabMargin.Maximum = Constants.DEFAULT_GRAB_MARGIN * 2;
            trackGrabMargin.TickFrequency = 10;
            trackGrabMargin.Value = grabMargin;
            labelGrabMargin.Text = trackGrabMargin.Value.ToString();

            trackJpegQuality.Minimum = 25;
            trackJpegQuality.Maximum = 100;
            trackJpegQuality.TickFrequency = 5;
            trackJpegQuality.Value = jpegQuality;
            labelJpegQuality.Text = trackJpegQuality.Value.ToString();

            textSMTPServer.Text = smtpServer;
            textSMTPPort.Text = smtpPort.ToString();
            checkSSL.Checked = smtpUseSsl;
            textUserName.Text = smtpUserName;
            textPassword.Text = smtpPassword;

            checkShowUsageHints.Checked = showUsageHints;
            checkLimitGrabByActiveScreen.Checked = limitGrabByActiveScreen;
            checkShowMagnifier.Checked = showGrabMagnifier;

            numericZoom.Value = magnifierZoomFactor;
        }

        private void trackJpegQuality_ValueChanged(object sender, EventArgs e)
        {
            jpegQuality = trackJpegQuality.Value;
            labelJpegQuality.Text = trackJpegQuality.Value.ToString();
            if (JpegQualityChanged != null) JpegQualityChanged(this, new ValueEventArgs<int>(jpegQuality));
            /*
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (key != null) {
                key.SetValue(REG_JPEG_QUALITY, jpegQuality);
            }*/
            SettingsWrapper.saveSettingValue(REG_JPEG_QUALITY, jpegQuality);

        }

        private void trackGrabMargin_ValueChanged(object sender, EventArgs e)
        {
            grabMargin = trackGrabMargin.Value;
            labelGrabMargin.Text = trackGrabMargin.Value.ToString();

            if (GrabMarginChanged != null) GrabMarginChanged(this, new ValueEventArgs<int>(grabMargin));
            /*
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (key != null) {
                key.SetValue(REG_GRAB_MARGIN, grabMargin);
            }
            */
            SettingsWrapper.saveSettingValue(REG_GRAB_MARGIN, grabMargin);
        }

        private void configDialogFormClosing(object sender, FormClosingEventArgs e)
        {
            instance = new ConfigDialog();
        }

        private void checkSslCheckedChanged(object sender, EventArgs e)
        {
            smtpUseSsl = checkSSL.Checked;
            textSMTPPort.Text = checkSSL.Checked ? "587" : "25";
            if (SmtpUseSslChanged != null) SmtpUseSslChanged(this, new ValueEventArgs<bool>(smtpUseSsl));

            /*
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (key != null) {
                key.SetValue(REG_SMTP_USE_SSL, smtpUseSsl ? 1 : 0);
            }
            */
            SettingsWrapper.saveSettingValue(REG_SMTP_USE_SSL, smtpUseSsl ? 1 : 0);
        }

        private void textUserName_TextChanged(object sender, EventArgs e)
        {
            smtpUserName = textUserName.Text;
            if (SmtpUserNameChanged != null) SmtpUserNameChanged(this, new ValueEventArgs<string>(smtpUserName));
            /*
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (key != null) {
                key.SetValue(REG_SMTP_USER, smtpUserName);
            }*/
            SettingsWrapper.saveSettingValue(REG_SMTP_USER, smtpUserName);
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            smtpPassword = textPassword.Text;
            if (SmtpPasswordChanged != null) SmtpPasswordChanged(this, new ValueEventArgs<string>(smtpPassword));

            if (!checkStorePassword.Checked) {
                return;
            }
            SettingsWrapper.saveSettingValue(REG_SMTP_PASSWORD, Rot13.Rotate(smtpPassword));
        }

        private void textSmtpServerTextChanged(object sender, EventArgs e)
        {
            smtpServer = textSMTPServer.Text;
            if (SmtpServerChanged != null) SmtpServerChanged(this, new ValueEventArgs<string>(smtpServer));

            SettingsWrapper.saveSettingValue(REG_SMTP_HOST, smtpServer);
        }

        private void textSmtpPortValidating(object sender, CancelEventArgs e)
        {
            try 
            {
                Int32.Parse(textSMTPPort.Text);
                textSMTPPort.ForeColor = Color.Black;
            } 
            catch (Exception)
            {
                textSMTPPort.ForeColor = Color.Red;
                e.Cancel = true;
            }
        }

        private void textSmtpPortTextChanged(object sender, EventArgs e)
        {
            smtpPort = Int32.Parse(textSMTPPort.Text);
            if (SmtpPortChanged != null) SmtpPortChanged(this, new ValueEventArgs<int>(smtpPort));
            SettingsWrapper.saveSettingValue(REG_SMTP_PORT, smtpPort);
        }

        private void checkShowUsageHints_CheckedChanged(object sender, EventArgs e)
        {
            showUsageHints = checkShowUsageHints.Checked;
            if (ShowUsageHintsChanged != null) ShowUsageHintsChanged(this, new ValueEventArgs<bool>(showUsageHints));
            SettingsWrapper.saveSettingValue(REG_SHOW_HINTS, showUsageHints ? 1 : 0);
        }

        private void numericZoom_ValueChanged(object sender, EventArgs e)
        {
            magnifierZoomFactor = (int) numericZoom.Value;
            if (MagnifierZoomFactorChanged != null) MagnifierZoomFactorChanged(this, new ValueEventArgs<int>(magnifierZoomFactor));
            SettingsWrapper.saveSettingValue(REG_MAGNIFIER_ZOOM, magnifierZoomFactor);
        }

        public class ValueEventArgs<T> : EventArgs {
            public ValueEventArgs(T val) { Value = val; }
            public T Value { get; private set; }
        }

#pragma warning disable 67
        public static event EventHandler<ValueEventArgs<bool>> ShowUsageHintsChanged;
        public static event EventHandler<ValueEventArgs<bool>> LimitGrabByActiveScreenChanged;
        public static event EventHandler<ValueEventArgs<int>> SmtpPortChanged;
        public static event EventHandler<ValueEventArgs<string>> SmtpServerChanged;
        public static event EventHandler<ValueEventArgs<string>> SmtpPasswordChanged;
        public static event EventHandler<ValueEventArgs<string>> SmtpUserNameChanged;
        public static event EventHandler<ValueEventArgs<bool>> SmtpUseSslChanged;
        public static event EventHandler<ValueEventArgs<int>> GrabMarginChanged;
        public static event EventHandler<ValueEventArgs<int>> JpegQualityChanged;
        public static event EventHandler<ValueEventArgs<int>> MagnifierZoomFactorChanged;
#pragma warning restore 67

        private void checkLimitGrabByActiveScreen_CheckedChanged(object sender, EventArgs e)
        {
            limitGrabByActiveScreen = checkLimitGrabByActiveScreen.Checked;
            if (LimitGrabByActiveScreenChanged != null) LimitGrabByActiveScreenChanged(this, new ValueEventArgs<bool>(limitGrabByActiveScreen));
            SettingsWrapper.saveSettingValue(REG_LIMIT_GRAB, limitGrabByActiveScreen ? 1 : 0);
        }

        private void checkShowMagnifier_CheckedChanged(object sender, EventArgs e) {
            showGrabMagnifier = checkShowMagnifier.Checked;
            SettingsWrapper.saveSettingValue(REG_SHOW_GRAB_MAGNIFIER, showGrabMagnifier ? 1 : 0);
        }

        private void configDialogKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (int) Keys.Escape) Close();
        }
    }
}
