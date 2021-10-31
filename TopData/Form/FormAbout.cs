namespace TopData
{
    using System;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// About form.
    /// </summary>
    public partial class FormAbout : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormAbout"/> class.
        /// </summary>
        public FormAbout()
        {
            this.InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            TdSwitchLanguage sl = new(this, TdCulture.Cul);
            sl.SetLanguageAboutForm();

            this.Text = TdSettingsDefault.ApplicationName;
            this.DoubleBuffered = true;
            this.LabelAppName.Text = TdSettingsDefault.ApplicationName;
            this.LabelVersion.Text += TdSettingsDefault.ApplicationVersion;
            this.LabelBuilddate.Text += " " + TdSettingsDefault.ApplicationBuildDate;
        }

        private void FormAbout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
