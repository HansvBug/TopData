namespace TopData
{
    using System;
    using System.Windows.Forms;

    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            this.InitializeComponent();

            l

                TdSwitchLanguage sl = new(this, cul);
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            this.Text = TdSettingsDefault.ApplicationName;
            this.DoubleBuffered = true;
            this.LabelAppName.Text = TdSettingsDefault.ApplicationName;
            this.LabelVersion.Text = "Versie : " + TdSettingsDefault.ApplicationVersion;
            this.LabelBuilddate.Text += " " + TdSettingsDefault.ApplicationBuildDate;
        }

        private void FormAbout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
