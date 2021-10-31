namespace TopData
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Form where settings can be changed.
    /// </summary>
    public partial class FormConfigure : Form
    {
        #region Properties

        /// <summary>
        /// Gets or sets the application settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        /// <summary>
        /// Gets or sets the number of startups after which the app. database gets copied.
        /// </summary>
        private int CopyAppDataBaseAfterEveryXStartups { get; set; }

        /// <summary>
        /// Gets or sets the current location off the application database.
        /// </summary>
        private string CurrentAppLocation { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormConfigure"/> class.
        /// </summary>
        public FormConfigure()
        {
            this.InitializeComponent();
            this.CurrentAppLocation = string.Empty;
            this.GetVersions();
        }
        #endregion Constructor

        #region Logging
        private void CheckBoxActivateLogging_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxActivateLogging.Checked)
            {
                this.CheckBoxAppenLogFile.Enabled = true;

                this.JsonObjSettings.AppParam[0].ActivateLogging = true;

                TdLogging.ActivateLogging = true;
                TdLogging.StartLogging();
                TdLogging.WriteToLogInformation("Logging aangezet.");
            }
            else
            {
                this.CheckBoxAppenLogFile.Checked = false;
                this.CheckBoxAppenLogFile.Enabled = false;

                this.JsonObjSettings.AppParam[0].ActivateLogging = false;
                this.JsonObjSettings.AppParam[0].AppendLogFile = false;

                TdLogging.WriteToLogInformation("Logging uitgezet.");
                TdLogging.StopLogging();
                TdLogging.ActivateLogging = false;
            }
        }
        #endregion Logging

        #region Form load
        private void FormConfigure_Load(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.Window;
            this.Text = "Opties";

            this.LoadSettings();
            this.ApplySettings();
            this.LoadFormPosition();
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;

            if (string.IsNullOrEmpty(this.CurrentAppLocation))
            {
                this.CurrentAppLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            }
        }

        private void ApplySettings()
        {
            if (this.JsonObjSettings.AppParam[0].AppendLogFile)
            {
                this.CheckBoxAppenLogFile.Checked = true;
            }
            else
            {
                this.CheckBoxAppenLogFile.Checked = false;
            }

            if (this.JsonObjSettings.AppParam[0].ActivateLogging)
            {
                this.CheckBoxActivateLogging.Checked = true;
            }
            else
            {
                this.CheckBoxActivateLogging.Checked = false;
            }

            this.TextBoxLocationSettingsFile.Text = this.JsonObjSettings.AppParam[0].SettingsFileLocation;
            this.TextBoxLocationLogFile.Text = this.JsonObjSettings.AppParam[0].LogFileLocation + TdSettingsDefault.LogFileName;
            this.TextBoxLocationDatabaseFile.Text = this.JsonObjSettings.AppParam[0].DatabaseLocation;

            this.LabelVersion.Text = "Versie : " + this.JsonObjSettings.AppParam[0].ApplicationVersion;
            this.LabelBuildDate.Text = "Build datum : " + this.JsonObjSettings.AppParam[0].ApplicationBuildDate;

            if (this.JsonObjSettings.AppParam[0].PasswordIsSchemaName)
            {
                this.CheckBoxPasswordIsSchemaName.Checked = true;
            }
            else
            {
                this.CheckBoxPasswordIsSchemaName.Checked = false;
            }

            if (this.JsonObjSettings.AppParam[0].ConstructConnectionName)
            {
                this.CheckBoxConstructConnectionName.Checked = true;
            }
            else
            {
                this.CheckBoxConstructConnectionName.Checked = false;
            }

            if (this.JsonObjSettings.AppParam[0].HighlightTextAndComboBox)
            {
                this.CheckBoxHiglightEntryComponents.Checked = true;
            }
            else
            {
                this.CheckBoxHiglightEntryComponents.Checked = false;
            }

            int copyAppDataBaseAfterEveryXStartups = this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartups;
            this.CopyDatabaseIntervalTextBox.Text = copyAppDataBaseAfterEveryXStartups.ToString();

            if (this.JsonObjSettings.AppParam[0].WarningOnDeleteQuery)
            {
                this.CheckBoxWarnOnDeleteQuery.Checked = true;
            }
            else
            {
                this.CheckBoxWarnOnDeleteQuery.Checked = false;
            }

            if (this.JsonObjSettings.AppParam[0].ShowQueryGuideLines)
            {
                this.CheckBoxShowQueryGuideLines.Checked = true;
            }
            else
            {
                this.CheckBoxShowQueryGuideLines.Checked = false;
            }

            if (this.JsonObjSettings.AppParam[0].HighLightDataGridOnMouseOver)
            {
                this.RadioButton1HighLightDatagrid.Checked = true;
            }
            else
            {
                this.RadioButton1HighLightDatagrid.Checked = false;
            }

            if (this.JsonObjSettings.AppParam[0].DataGridAlternateRowColor)
            {
                this.RadioButtonAlternateRowColor.Checked = true;
            }
            else
            {
                this.RadioButtonAlternateRowColor.Checked = false;
            }

            if (this.JsonObjSettings.AppParam[0].DataGridNoRowColor)
            {
                this.RadioButtonNoColor.Checked = true;
            }
            else
            {
                this.RadioButtonNoColor.Checked = false;
            }
        }

        private void LoadFormPosition()
        {
            using TdFormPosition formPosition = new(this);
            formPosition.LoadConfigureFormPosition();
        }

        private void GetVersions()
        {
            TdAppDbMaintain sqlVersion = new();
            this.TextBoxSQliteVersion.Text = sqlVersion.SQLiteVersion();

            using TdAppDbCreate appDb = new();
            this.TextBoxApplicationdatabaseMetaVersion.Text = appDb.SelectMeta().ToString();

            using TdAppDbMaintain appdbUserVersion = new();
            this.TextBoxSqliteDbFileUser_version.Text = appdbUserVersion.GetPramaUserVersion();
        }

        #endregion Form load

        #region Form close
        private void FormConfigure_FormClosing(object sender, FormClosingEventArgs e)
        {
            TdLogging.WriteToLogInformation("Sluiten configuratie scherm.");
            this.SaveFormPosition();
            this.SaveSettings();
        }

        private void SaveFormPosition()
        {
            using TdFormPosition formPosition = new(this);

            formPosition.SaveConfigureFormPosition();
        }

        private void SaveSettings()
        {
            TdSettingsManager.SaveSettings(this.JsonObjSettings);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion Form close

        #region Oracle database
        private void CheckBoxPasswordIsSchemaName_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxPasswordIsSchemaName.Checked)
            {
                this.JsonObjSettings.AppParam[0].PasswordIsSchemaName = true;
                TdLogging.WriteToLogInformation("Wachtwoord = schemanaam aangezet.");
            }
            else
            {
                this.JsonObjSettings.AppParam[0].PasswordIsSchemaName = false;
                TdLogging.WriteToLogInformation("Wachtwoord = schemanaam uitgezet.");
            }
        }

        private void CheckBoxConstructConnectionName_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxConstructConnectionName.Checked)
            {
                this.JsonObjSettings.AppParam[0].ConstructConnectionName = true;
                TdLogging.WriteToLogInformation("Stel oracle connectie naam samen aangezet.");
            }
            else
            {
                this.JsonObjSettings.AppParam[0].ConstructConnectionName = false;
                TdLogging.WriteToLogInformation("Stel oracle connectie naam samen uitgezet.");
            }
        }

        #endregion Oracle database

        private void CheckBoxHiglightEntryComponents_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxHiglightEntryComponents.Checked)
            {
                this.JsonObjSettings.AppParam[0].HighlightTextAndComboBox = true;
            }
            else
            {
                this.JsonObjSettings.AppParam[0].HighlightTextAndComboBox = false;
            }
        }

        #region Color active textbox
        private void TextBoxLocationSettingsFile_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void TextBoxLocationSettingsFile_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }
        #endregion Color active textbox

        private void ButtonCompressAppDatabase_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            TdLogging.WriteToLogInformation("Het bestand " + TdSettingsDefault.SqlLiteDatabaseName + " wordt gecomprimeerd...");
            try
            {
                string argument = string.Empty;

                using TdAppDbMaintain appDbMaintain = new();

                // Copy the database file before compress takes place
                if (appDbMaintain.CopyDatabaseFile(string.Empty))
                {
                    appDbMaintain.CompressDatabase();
                    appDbMaintain.ResetAllAutoIncrementFields();
                    TdLogging.WriteToLogInformation("Het bestand " + TdSettingsDefault.SqlLiteDatabaseName + " is succesvol gecomprimeerd.");

                    MessageBox.Show(MB_Text.App_Database_Compressed, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                TdLogging.WriteToLogInformation("Onverwachte fout opgetreden bij het comprimeren van '" + TdSettingsDefault.SqlLiteDatabaseName + "'.");
                TdLogging.WriteToLogInformation("Melding:");
                TdLogging.WriteToLogInformation(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                Cursor.Current = Cursors.Default;
                MessageBox.Show(
                    "Fout opgetreden bij het comprimeren van " + TdSettingsDefault.SqlLiteDatabaseName + "." + Environment.NewLine +
                    Environment.NewLine + "Raadpleeg het log bestand.",
                    MB_Title.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CopyDatabaseIntervalTextBox_TextChanged(object sender, EventArgs e)
        {
            TdVisual.TxtLengthTolarge(sender, 200);
            try
            {
                this.CopyAppDataBaseAfterEveryXStartups = int.Parse(this.CopyDatabaseIntervalTextBox.Text, CultureInfo.InvariantCulture);
                if (this.CopyAppDataBaseAfterEveryXStartups > 200)
                {
                    this.CopyAppDataBaseAfterEveryXStartups = 200;
                    this.CopyDatabaseIntervalTextBox.Text = ResourceText._2075;
                    MessageBox.Show(MB_Text.Max_200, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartups = this.CopyAppDataBaseAfterEveryXStartups;
            }
            catch (FormatException ex)
            {
                TdLogging.WriteToLogError("Fout opgetreden bij het omzetten van een string naar een integer.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                this.CopyDatabaseIntervalTextBox.Text = ResourceText._2074;
            }
        }

        private void CopyDatabaseIntervalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void CopyDatabaseIntervalTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.CopyDatabaseIntervalTextBox.Text) || string.IsNullOrWhiteSpace(this.CopyDatabaseIntervalTextBox.Text))
            {
                this.CopyDatabaseIntervalTextBox.Text = "0";
                this.CopyAppDataBaseAfterEveryXStartups = 0;
                TdLogging.WriteToLogInformation("Kopieer de app database frequentie is aangepast naar : " + this.CopyDatabaseIntervalTextBox.Text);
            }

            TdVisual.TxtLeave(sender, e);
        }

        private void CopyDatabaseIntervalTextBox_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void CheckBoxWarnOnDeleteQuery_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxWarnOnDeleteQuery.Checked)
            {
                this.JsonObjSettings.AppParam[0].WarningOnDeleteQuery = true;
                TdLogging.WriteToLogInformation("Waarschuwing voor verwijderen query is aangezet.");
            }
            else
            {
                this.JsonObjSettings.AppParam[0].WarningOnDeleteQuery = false;
                TdLogging.WriteToLogInformation("Waarschuwing voor verwijderen query is uitgezet.");
            }
        }

        private void CheckBoxShowQueryGuideLines_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxShowQueryGuideLines.Checked)
            {
                this.JsonObjSettings.AppParam[0].ShowQueryGuideLines = true;
                TdLogging.WriteToLogInformation("Toon Query richtlijnen is aangezet.");
            }
            else
            {
                this.JsonObjSettings.AppParam[0].ShowQueryGuideLines = false;
                TdLogging.WriteToLogInformation("Toon Query richtlijnen is uitgezet.");
            }
        }

        private void CheckBoxAppenLogFile_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxAppenLogFile.Checked)
            {
                this.JsonObjSettings.AppParam[0].AppendLogFile = true;
                TdLogging.WriteToLogInformation("Logging aanvullen is aangezet.");
            }
            else
            {
                this.JsonObjSettings.AppParam[0].AppendLogFile = false;
                TdLogging.WriteToLogInformation("Logging aanvullen is uitgezet.");
            }
        }

        private void TextBoxLocationDatabaseFile_Leave(object sender, EventArgs e)
        {
            string dbFileName = TdSettingsDefault.ApplicationName + ".db";
            this.TextBoxLocationDatabaseFile.Text.Replace(dbFileName, string.Empty);
            this.JsonObjSettings.AppParam[0].DatabaseLocation = this.TextBoxLocationDatabaseFile.Text;

            if (this.CurrentAppLocation != this.TextBoxLocationDatabaseFile.Text)
            {
                MessageBox.Show("Start de applicatie opnieuw op", MB_Title.Attention, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonLocationAppDatabaseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new())
            {
                openFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TdSettingsDefault.DatabaseFolder);
                openFileDialog.Filter = "db files (*.db)|*.db";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string dbFileName = TdSettingsDefault.ApplicationName + ".db";
                    this.TextBoxLocationDatabaseFile.Text = openFileDialog.FileName.Replace(dbFileName, string.Empty);
                }
            }

            this.ActiveControl = this.TextBoxLocationDatabaseFile;
        }

        #region Datagrid options
        private void RadioButtonAlternateRowColor_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButtonAlternateRowColor.Checked)
            {
                this.JsonObjSettings.AppParam[0].DataGridAlternateRowColor = true;
            }
            else
            {
                this.JsonObjSettings.AppParam[0].DataGridAlternateRowColor = false;
            }
        }

        private void RadioButtonAlternateRowColor_Click(object sender, EventArgs e)
        {
            if (this.RadioButtonAlternateRowColor.Checked)
            {
                TdLogging.WriteToLogInformation("Kleur oneven datagridview rij is aangezet.");
            }
            else
            {
                TdLogging.WriteToLogInformation("Kleur oneven datagridview rij is uitgezet.");
            }
        }

        private void RadioButton1HighLightDatagrid_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButton1HighLightDatagrid.Checked)
            {
                this.JsonObjSettings.AppParam[0].HighLightDataGridOnMouseOver = true;
            }
            else
            {
                this.JsonObjSettings.AppParam[0].HighLightDataGridOnMouseOver = false;
            }
        }

        private void RadioButton1HighLightDatagrid_Click(object sender, EventArgs e)
        {
            if (this.RadioButton1HighLightDatagrid.Checked)
            {
                TdLogging.WriteToLogInformation("Highlight datagridview rij is aangezet.");
            }
            else
            {
                TdLogging.WriteToLogInformation("Highlight datagridview rij is uitgezet.");
            }
        }

        private void RadioButtonNoColor_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButtonNoColor.Checked)
            {
                this.JsonObjSettings.AppParam[0].DataGridNoRowColor = true;
            }
            else
            {
                this.JsonObjSettings.AppParam[0].DataGridNoRowColor = false;
            }
        }

        private void RadioButtonNoColor_Click(object sender, EventArgs e)
        {
            if (this.RadioButtonNoColor.Checked)
            {
                TdLogging.WriteToLogInformation("Datagridview rij geen kleur is aangezet.");
            }
            else
            {
                TdLogging.WriteToLogInformation("Datagridview rij geen kleur is uitgezet.");
            }
        }
        #endregion Datagrid options
    }
}
