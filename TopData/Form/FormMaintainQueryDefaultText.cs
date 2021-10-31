namespace TopData
{
    using System;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// Form for editing the default query text.
    /// </summary>
    public partial class FormMaintainQueryDefaultText : Form
    {
        // public dynamic JsonObjSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMaintainQueryDefaultText"/> class.
        /// </summary>
        public FormMaintainQueryDefaultText()
        {
            this.InitializeComponent();
            this.Text = ResourceText.FrmMaintainQueriesText;
        }

        #region load form
        private void FormMaintainQueryDefaultText_Load(object sender, EventArgs e)
        {
            // LoadSettings();
            TdLogging.WriteToLogInformation("Scherm Standaard Query tekst wordt geopend. + (Gebruiker : " + TdUserData.UserName + ").");

            using TdDefaultQueryComment loadText = new (TdUserData.UserName, int.Parse(TdUserData.UserId, CultureInfo.InvariantCulture));
            if (!string.IsNullOrEmpty(loadText.LoadDefaultQueryStartText()))
            {
                this.RichTextBoxDefaultQueryText.Text = loadText.LoadDefaultQueryStartText();
            }
        }

        /*private void LoadSettings()
        {
            using SettingsManager Set = new SettingsManager();
            Set.LoadSettings();
            JsonObjSettings = Set.JsonObjSettings;
        }*/
        #endregion load form

        #region form close
        private void FormMaintainQueryDefaultText_FormClosing(object sender, FormClosingEventArgs e)
        {
            // SaveSettings();
        }

        /*private void SaveSettings()
        {
            SettingsManager.SaveSettings(JsonObjSettings);
        }*/
        #endregion form close

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            TdLogging.WriteToLogInformation("Scherm Standaard Query tekst wordt gesloten.");
            this.Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            using TdDefaultQueryComment saveText = new (TdUserData.UserName, int.Parse(TdUserData.UserId, CultureInfo.InvariantCulture));
            saveText.DeleteDefaultQueryStartText();
            saveText.SaveDefaultQueryStartText(this.RichTextBoxDefaultQueryText.Text);
        }

        private void ButtonSaveAndClose_Click(object sender, EventArgs e)
        {
            using TdDefaultQueryComment saveText = new (TdUserData.UserName, int.Parse(TdUserData.UserId, CultureInfo.InvariantCulture));
            saveText.DeleteDefaultQueryStartText();
            saveText.SaveDefaultQueryStartText(this.RichTextBoxDefaultQueryText.Text);
            TdLogging.WriteToLogInformation("Scherm Standaard Query tekst wordt gesloten.");

            this.Close();
        }
    }
}
