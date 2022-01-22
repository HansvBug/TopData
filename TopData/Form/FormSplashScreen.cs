namespace TopData
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Splashscreen form.
    /// </summary>
    public partial class FormSplashScreen : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormSplashScreen"/> class.
        /// </summary>
        public FormSplashScreen()
        {
            this.InitializeComponent();
        }

        private void FormSplashScreen_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.Text = TdSettingsDefault.ApplicationName;
            this.LabelAppName.Text = TdSettingsDefault.ApplicationName;
            this.LabelVersion.Text += TdSettingsDefault.ApplicationVersion;
            this.LabelBuilddate.Text += " " + TdSettingsDefault.ApplicationBuildDate;
        }

        private void Panel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
