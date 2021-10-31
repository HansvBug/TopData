namespace TopData
{
    partial class FormConfigure
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.LabelVersion = new System.Windows.Forms.Label();
            this.LabelBuildDate = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.LabelLocationAppDatabaseFile = new System.Windows.Forms.Label();
            this.TextBoxLocationDatabaseFile = new System.Windows.Forms.TextBox();
            this.GroupBoxOraDbConnections = new System.Windows.Forms.GroupBox();
            this.CheckBoxConstructConnectionName = new System.Windows.Forms.CheckBox();
            this.CheckBoxPasswordIsSchemaName = new System.Windows.Forms.CheckBox();
            this.GroupBoxDbmaintenance = new System.Windows.Forms.GroupBox();
            this.LabelCopyAppDb = new System.Windows.Forms.Label();
            this.ButtonCompressAppDatabase = new System.Windows.Forms.Button();
            this.CopyDatabaseIntervalTextBox = new System.Windows.Forms.TextBox();
            this.CheckBoxWarnOnDeleteQuery = new System.Windows.Forms.CheckBox();
            this.CheckBoxShowQueryGuideLines = new System.Windows.Forms.CheckBox();
            this.ButtonLocationAppDatabaseFile = new System.Windows.Forms.Button();
            this.GroupBoxDivers = new System.Windows.Forms.GroupBox();
            this.RadioButtonNoColor = new System.Windows.Forms.RadioButton();
            this.RadioButton1HighLightDatagrid = new System.Windows.Forms.RadioButton();
            this.RadioButtonAlternateRowColor = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabPageGeneralOptions = new System.Windows.Forms.TabPage();
            this.GroupBoxVisual = new System.Windows.Forms.GroupBox();
            this.CheckBoxHiglightEntryComponents = new System.Windows.Forms.CheckBox();
            this.GroupBoxLogFile = new System.Windows.Forms.GroupBox();
            this.TextBoxLocationLogFile = new System.Windows.Forms.TextBox();
            this.TextBoxLocationSettingsFile = new System.Windows.Forms.TextBox();
            this.LabelLocationLogFile = new System.Windows.Forms.Label();
            this.LabelLocationSettingsFile = new System.Windows.Forms.Label();
            this.CheckBoxAppenLogFile = new System.Windows.Forms.CheckBox();
            this.CheckBoxActivateLogging = new System.Windows.Forms.CheckBox();
            this.TabPageApplication = new System.Windows.Forms.TabPage();
            this.TextBoxSqliteDbFileUser_version = new System.Windows.Forms.TextBox();
            this.TextBoxApplicationdatabaseMetaVersion = new System.Windows.Forms.TextBox();
            this.LabelSqliteDbFileUser_version = new System.Windows.Forms.Label();
            this.LabelApplicationdatabaseMetaVersion = new System.Windows.Forms.Label();
            this.TextBoxSQliteVersion = new System.Windows.Forms.TextBox();
            this.LabelSQliteVersion = new System.Windows.Forms.Label();
            this.GroupBoxOraDbConnections.SuspendLayout();
            this.GroupBoxDbmaintenance.SuspendLayout();
            this.GroupBoxDivers.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.TabPageGeneralOptions.SuspendLayout();
            this.GroupBoxVisual.SuspendLayout();
            this.GroupBoxLogFile.SuspendLayout();
            this.TabPageApplication.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-111, -34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // LabelVersion
            // 
            this.LabelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelVersion.AutoSize = true;
            this.LabelVersion.Location = new System.Drawing.Point(19, 377);
            this.LabelVersion.Name = "LabelVersion";
            this.LabelVersion.Size = new System.Drawing.Size(52, 15);
            this.LabelVersion.TabIndex = 8;
            this.LabelVersion.Text = "Versie : -";
            // 
            // LabelBuildDate
            // 
            this.LabelBuildDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelBuildDate.AutoSize = true;
            this.LabelBuildDate.Location = new System.Drawing.Point(123, 377);
            this.LabelBuildDate.Name = "LabelBuildDate";
            this.LabelBuildDate.Size = new System.Drawing.Size(86, 15);
            this.LabelBuildDate.TabIndex = 9;
            this.LabelBuildDate.Text = "Build datum : -";
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.Location = new System.Drawing.Point(582, 373);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 12;
            this.ButtonClose.Text = "&Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // LabelLocationAppDatabaseFile
            // 
            this.LabelLocationAppDatabaseFile.AutoSize = true;
            this.LabelLocationAppDatabaseFile.Location = new System.Drawing.Point(7, 122);
            this.LabelLocationAppDatabaseFile.Name = "LabelLocationAppDatabaseFile";
            this.LabelLocationAppDatabaseFile.Size = new System.Drawing.Size(155, 15);
            this.LabelLocationAppDatabaseFile.TabIndex = 13;
            this.LabelLocationAppDatabaseFile.Text = "Locatie applicatie database :";
            // 
            // TextBoxLocationDatabaseFile
            // 
            this.TextBoxLocationDatabaseFile.Location = new System.Drawing.Point(170, 119);
            this.TextBoxLocationDatabaseFile.Name = "TextBoxLocationDatabaseFile";
            this.TextBoxLocationDatabaseFile.Size = new System.Drawing.Size(439, 23);
            this.TextBoxLocationDatabaseFile.TabIndex = 14;
            this.TextBoxLocationDatabaseFile.Enter += new System.EventHandler(this.TextBoxLocationSettingsFile_Enter);
            this.TextBoxLocationDatabaseFile.Leave += new System.EventHandler(this.TextBoxLocationDatabaseFile_Leave);
            // 
            // GroupBoxOraDbConnections
            // 
            this.GroupBoxOraDbConnections.Controls.Add(this.CheckBoxConstructConnectionName);
            this.GroupBoxOraDbConnections.Controls.Add(this.CheckBoxPasswordIsSchemaName);
            this.GroupBoxOraDbConnections.Location = new System.Drawing.Point(338, 162);
            this.GroupBoxOraDbConnections.Name = "GroupBoxOraDbConnections";
            this.GroupBoxOraDbConnections.Size = new System.Drawing.Size(297, 71);
            this.GroupBoxOraDbConnections.TabIndex = 15;
            this.GroupBoxOraDbConnections.TabStop = false;
            this.GroupBoxOraDbConnections.Text = "Oracle Database connecties";
            // 
            // CheckBoxConstructConnectionName
            // 
            this.CheckBoxConstructConnectionName.AutoSize = true;
            this.CheckBoxConstructConnectionName.Location = new System.Drawing.Point(6, 44);
            this.CheckBoxConstructConnectionName.Name = "CheckBoxConstructConnectionName";
            this.CheckBoxConstructConnectionName.Size = new System.Drawing.Size(184, 19);
            this.CheckBoxConstructConnectionName.TabIndex = 12;
            this.CheckBoxConstructConnectionName.Text = "Stel de connectienaam samen";
            this.CheckBoxConstructConnectionName.UseVisualStyleBackColor = true;
            this.CheckBoxConstructConnectionName.Click += new System.EventHandler(this.CheckBoxConstructConnectionName_Click);
            // 
            // CheckBoxPasswordIsSchemaName
            // 
            this.CheckBoxPasswordIsSchemaName.AutoSize = true;
            this.CheckBoxPasswordIsSchemaName.Location = new System.Drawing.Point(6, 19);
            this.CheckBoxPasswordIsSchemaName.Name = "CheckBoxPasswordIsSchemaName";
            this.CheckBoxPasswordIsSchemaName.Size = new System.Drawing.Size(179, 19);
            this.CheckBoxPasswordIsSchemaName.TabIndex = 0;
            this.CheckBoxPasswordIsSchemaName.Text = "Wachtwoord = schemanaam";
            this.CheckBoxPasswordIsSchemaName.UseVisualStyleBackColor = true;
            this.CheckBoxPasswordIsSchemaName.Click += new System.EventHandler(this.CheckBoxPasswordIsSchemaName_Click);
            // 
            // GroupBoxDbmaintenance
            // 
            this.GroupBoxDbmaintenance.Controls.Add(this.LabelCopyAppDb);
            this.GroupBoxDbmaintenance.Controls.Add(this.ButtonCompressAppDatabase);
            this.GroupBoxDbmaintenance.Controls.Add(this.CopyDatabaseIntervalTextBox);
            this.GroupBoxDbmaintenance.Location = new System.Drawing.Point(6, 6);
            this.GroupBoxDbmaintenance.Name = "GroupBoxDbmaintenance";
            this.GroupBoxDbmaintenance.Size = new System.Drawing.Size(282, 92);
            this.GroupBoxDbmaintenance.TabIndex = 17;
            this.GroupBoxDbmaintenance.TabStop = false;
            this.GroupBoxDbmaintenance.Text = "Database onderhoud";
            // 
            // LabelCopyAppDb
            // 
            this.LabelCopyAppDb.AutoSize = true;
            this.LabelCopyAppDb.Location = new System.Drawing.Point(3, 55);
            this.LabelCopyAppDb.Name = "LabelCopyAppDb";
            this.LabelCopyAppDb.Size = new System.Drawing.Size(224, 15);
            this.LabelCopyAppDb.TabIndex = 1;
            this.LabelCopyAppDb.Text = "Kopieer de app database na x keer starten";
            // 
            // ButtonCompressAppDatabase
            // 
            this.ButtonCompressAppDatabase.Location = new System.Drawing.Point(3, 19);
            this.ButtonCompressAppDatabase.Name = "ButtonCompressAppDatabase";
            this.ButtonCompressAppDatabase.Size = new System.Drawing.Size(191, 23);
            this.ButtonCompressAppDatabase.TabIndex = 0;
            this.ButtonCompressAppDatabase.Text = "Comprimeer de app. database";
            this.ButtonCompressAppDatabase.UseVisualStyleBackColor = true;
            this.ButtonCompressAppDatabase.Click += new System.EventHandler(this.ButtonCompressAppDatabase_Click);
            // 
            // CopyDatabaseIntervalTextBox
            // 
            this.CopyDatabaseIntervalTextBox.Location = new System.Drawing.Point(233, 52);
            this.CopyDatabaseIntervalTextBox.Name = "CopyDatabaseIntervalTextBox";
            this.CopyDatabaseIntervalTextBox.Size = new System.Drawing.Size(43, 23);
            this.CopyDatabaseIntervalTextBox.TabIndex = 2;
            this.CopyDatabaseIntervalTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CopyDatabaseIntervalTextBox.TextChanged += new System.EventHandler(this.CopyDatabaseIntervalTextBox_TextChanged);
            this.CopyDatabaseIntervalTextBox.Enter += new System.EventHandler(this.CopyDatabaseIntervalTextBox_Enter);
            this.CopyDatabaseIntervalTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CopyDatabaseIntervalTextBox_KeyPress);
            this.CopyDatabaseIntervalTextBox.Leave += new System.EventHandler(this.CopyDatabaseIntervalTextBox_Leave);
            // 
            // CheckBoxWarnOnDeleteQuery
            // 
            this.CheckBoxWarnOnDeleteQuery.AutoSize = true;
            this.CheckBoxWarnOnDeleteQuery.Location = new System.Drawing.Point(6, 120);
            this.CheckBoxWarnOnDeleteQuery.Name = "CheckBoxWarnOnDeleteQuery";
            this.CheckBoxWarnOnDeleteQuery.Size = new System.Drawing.Size(293, 19);
            this.CheckBoxWarnOnDeleteQuery.TabIndex = 18;
            this.CheckBoxWarnOnDeleteQuery.Text = "Waarschuwen voordat een Query wordt verwijderd";
            this.CheckBoxWarnOnDeleteQuery.UseVisualStyleBackColor = true;
            this.CheckBoxWarnOnDeleteQuery.Click += new System.EventHandler(this.CheckBoxWarnOnDeleteQuery_Click);
            // 
            // CheckBoxShowQueryGuideLines
            // 
            this.CheckBoxShowQueryGuideLines.AutoSize = true;
            this.CheckBoxShowQueryGuideLines.Location = new System.Drawing.Point(6, 97);
            this.CheckBoxShowQueryGuideLines.Name = "CheckBoxShowQueryGuideLines";
            this.CheckBoxShowQueryGuideLines.Size = new System.Drawing.Size(145, 19);
            this.CheckBoxShowQueryGuideLines.TabIndex = 19;
            this.CheckBoxShowQueryGuideLines.Text = "Toon Query richtlijnen";
            this.CheckBoxShowQueryGuideLines.UseVisualStyleBackColor = true;
            this.CheckBoxShowQueryGuideLines.Click += new System.EventHandler(this.CheckBoxShowQueryGuideLines_Click);
            // 
            // ButtonLocationAppDatabaseFile
            // 
            this.ButtonLocationAppDatabaseFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonLocationAppDatabaseFile.Location = new System.Drawing.Point(615, 118);
            this.ButtonLocationAppDatabaseFile.Name = "ButtonLocationAppDatabaseFile";
            this.ButtonLocationAppDatabaseFile.Size = new System.Drawing.Size(25, 23);
            this.ButtonLocationAppDatabaseFile.TabIndex = 20;
            this.ButtonLocationAppDatabaseFile.Text = "...";
            this.ButtonLocationAppDatabaseFile.UseVisualStyleBackColor = true;
            this.ButtonLocationAppDatabaseFile.Click += new System.EventHandler(this.ButtonLocationAppDatabaseFile_Click);
            // 
            // GroupBoxDivers
            // 
            this.GroupBoxDivers.Controls.Add(this.RadioButtonNoColor);
            this.GroupBoxDivers.Controls.Add(this.CheckBoxShowQueryGuideLines);
            this.GroupBoxDivers.Controls.Add(this.CheckBoxWarnOnDeleteQuery);
            this.GroupBoxDivers.Controls.Add(this.RadioButton1HighLightDatagrid);
            this.GroupBoxDivers.Controls.Add(this.RadioButtonAlternateRowColor);
            this.GroupBoxDivers.Location = new System.Drawing.Point(6, 162);
            this.GroupBoxDivers.Name = "GroupBoxDivers";
            this.GroupBoxDivers.Size = new System.Drawing.Size(326, 149);
            this.GroupBoxDivers.TabIndex = 21;
            this.GroupBoxDivers.TabStop = false;
            this.GroupBoxDivers.Text = "Divers";
            // 
            // RadioButtonNoColor
            // 
            this.RadioButtonNoColor.AutoSize = true;
            this.RadioButtonNoColor.Location = new System.Drawing.Point(6, 72);
            this.RadioButtonNoColor.Name = "RadioButtonNoColor";
            this.RadioButtonNoColor.Size = new System.Drawing.Size(81, 19);
            this.RadioButtonNoColor.TabIndex = 2;
            this.RadioButtonNoColor.TabStop = true;
            this.RadioButtonNoColor.Text = "Geen kleur";
            this.RadioButtonNoColor.UseVisualStyleBackColor = true;
            this.RadioButtonNoColor.CheckedChanged += new System.EventHandler(this.RadioButtonNoColor_CheckedChanged);
            this.RadioButtonNoColor.Click += new System.EventHandler(this.RadioButtonNoColor_Click);
            // 
            // RadioButton1HighLightDatagrid
            // 
            this.RadioButton1HighLightDatagrid.AutoSize = true;
            this.RadioButton1HighLightDatagrid.Location = new System.Drawing.Point(6, 22);
            this.RadioButton1HighLightDatagrid.Name = "RadioButton1HighLightDatagrid";
            this.RadioButton1HighLightDatagrid.Size = new System.Drawing.Size(126, 19);
            this.RadioButton1HighLightDatagrid.TabIndex = 1;
            this.RadioButton1HighLightDatagrid.TabStop = true;
            this.RadioButton1HighLightDatagrid.Text = "HighLight Datagrid";
            this.RadioButton1HighLightDatagrid.UseVisualStyleBackColor = true;
            this.RadioButton1HighLightDatagrid.CheckedChanged += new System.EventHandler(this.RadioButton1HighLightDatagrid_CheckedChanged);
            this.RadioButton1HighLightDatagrid.Click += new System.EventHandler(this.RadioButton1HighLightDatagrid_Click);
            // 
            // RadioButtonAlternateRowColor
            // 
            this.RadioButtonAlternateRowColor.AutoSize = true;
            this.RadioButtonAlternateRowColor.Location = new System.Drawing.Point(6, 47);
            this.RadioButtonAlternateRowColor.Name = "RadioButtonAlternateRowColor";
            this.RadioButtonAlternateRowColor.Size = new System.Drawing.Size(190, 19);
            this.RadioButtonAlternateRowColor.TabIndex = 0;
            this.RadioButtonAlternateRowColor.TabStop = true;
            this.RadioButtonAlternateRowColor.Text = "Datagrid rij kleur even / oneven";
            this.RadioButtonAlternateRowColor.UseVisualStyleBackColor = true;
            this.RadioButtonAlternateRowColor.CheckedChanged += new System.EventHandler(this.RadioButtonAlternateRowColor_CheckedChanged);
            this.RadioButtonAlternateRowColor.Click += new System.EventHandler(this.RadioButtonAlternateRowColor_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TabPageGeneralOptions);
            this.tabControl1.Controls.Add(this.TabPageApplication);
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(659, 355);
            this.tabControl1.TabIndex = 22;
            // 
            // TabPageGeneralOptions
            // 
            this.TabPageGeneralOptions.Controls.Add(this.GroupBoxVisual);
            this.TabPageGeneralOptions.Controls.Add(this.GroupBoxOraDbConnections);
            this.TabPageGeneralOptions.Controls.Add(this.GroupBoxDivers);
            this.TabPageGeneralOptions.Controls.Add(this.GroupBoxLogFile);
            this.TabPageGeneralOptions.Location = new System.Drawing.Point(4, 24);
            this.TabPageGeneralOptions.Name = "TabPageGeneralOptions";
            this.TabPageGeneralOptions.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageGeneralOptions.Size = new System.Drawing.Size(651, 327);
            this.TabPageGeneralOptions.TabIndex = 0;
            this.TabPageGeneralOptions.Text = "Instellingen";
            this.TabPageGeneralOptions.UseVisualStyleBackColor = true;
            // 
            // GroupBoxVisual
            // 
            this.GroupBoxVisual.Controls.Add(this.CheckBoxHiglightEntryComponents);
            this.GroupBoxVisual.Location = new System.Drawing.Point(338, 259);
            this.GroupBoxVisual.Name = "GroupBoxVisual";
            this.GroupBoxVisual.Size = new System.Drawing.Size(297, 51);
            this.GroupBoxVisual.TabIndex = 1;
            this.GroupBoxVisual.TabStop = false;
            this.GroupBoxVisual.Text = "Visueel";
            // 
            // CheckBoxHiglightEntryComponents
            // 
            this.CheckBoxHiglightEntryComponents.AutoSize = true;
            this.CheckBoxHiglightEntryComponents.Location = new System.Drawing.Point(6, 23);
            this.CheckBoxHiglightEntryComponents.Name = "CheckBoxHiglightEntryComponents";
            this.CheckBoxHiglightEntryComponents.Size = new System.Drawing.Size(146, 19);
            this.CheckBoxHiglightEntryComponents.TabIndex = 1;
            this.CheckBoxHiglightEntryComponents.Text = "Kleur actief invoer veld";
            this.CheckBoxHiglightEntryComponents.UseVisualStyleBackColor = true;
            this.CheckBoxHiglightEntryComponents.Click += new System.EventHandler(this.CheckBoxHiglightEntryComponents_Click);
            // 
            // GroupBoxLogFile
            // 
            this.GroupBoxLogFile.Controls.Add(this.TextBoxLocationLogFile);
            this.GroupBoxLogFile.Controls.Add(this.TextBoxLocationSettingsFile);
            this.GroupBoxLogFile.Controls.Add(this.LabelLocationLogFile);
            this.GroupBoxLogFile.Controls.Add(this.LabelLocationSettingsFile);
            this.GroupBoxLogFile.Controls.Add(this.CheckBoxAppenLogFile);
            this.GroupBoxLogFile.Controls.Add(this.CheckBoxActivateLogging);
            this.GroupBoxLogFile.Location = new System.Drawing.Point(6, 6);
            this.GroupBoxLogFile.Name = "GroupBoxLogFile";
            this.GroupBoxLogFile.Size = new System.Drawing.Size(629, 150);
            this.GroupBoxLogFile.TabIndex = 0;
            this.GroupBoxLogFile.TabStop = false;
            this.GroupBoxLogFile.Text = "Log bestand";
            // 
            // TextBoxLocationLogFile
            // 
            this.TextBoxLocationLogFile.Enabled = false;
            this.TextBoxLocationLogFile.Location = new System.Drawing.Point(161, 111);
            this.TextBoxLocationLogFile.Name = "TextBoxLocationLogFile";
            this.TextBoxLocationLogFile.Size = new System.Drawing.Size(452, 23);
            this.TextBoxLocationLogFile.TabIndex = 13;
            // 
            // TextBoxLocationSettingsFile
            // 
            this.TextBoxLocationSettingsFile.Enabled = false;
            this.TextBoxLocationSettingsFile.Location = new System.Drawing.Point(161, 81);
            this.TextBoxLocationSettingsFile.Name = "TextBoxLocationSettingsFile";
            this.TextBoxLocationSettingsFile.Size = new System.Drawing.Size(452, 23);
            this.TextBoxLocationSettingsFile.TabIndex = 13;
            this.TextBoxLocationSettingsFile.Enter += new System.EventHandler(this.TextBoxLocationSettingsFile_Enter);
            this.TextBoxLocationSettingsFile.Leave += new System.EventHandler(this.TextBoxLocationSettingsFile_Leave);
            // 
            // LabelLocationLogFile
            // 
            this.LabelLocationLogFile.AutoSize = true;
            this.LabelLocationLogFile.Location = new System.Drawing.Point(6, 114);
            this.LabelLocationLogFile.Name = "LabelLocationLogFile";
            this.LabelLocationLogFile.Size = new System.Drawing.Size(119, 15);
            this.LabelLocationLogFile.TabIndex = 11;
            this.LabelLocationLogFile.Text = "Locatie Log bestand :";
            // 
            // LabelLocationSettingsFile
            // 
            this.LabelLocationSettingsFile.AutoSize = true;
            this.LabelLocationSettingsFile.Location = new System.Drawing.Point(6, 84);
            this.LabelLocationSettingsFile.Name = "LabelLocationSettingsFile";
            this.LabelLocationSettingsFile.Size = new System.Drawing.Size(140, 15);
            this.LabelLocationSettingsFile.TabIndex = 12;
            this.LabelLocationSettingsFile.Text = "Locatie settings bestand :";
            // 
            // CheckBoxAppenLogFile
            // 
            this.CheckBoxAppenLogFile.AutoSize = true;
            this.CheckBoxAppenLogFile.Location = new System.Drawing.Point(6, 47);
            this.CheckBoxAppenLogFile.Name = "CheckBoxAppenLogFile";
            this.CheckBoxAppenLogFile.Size = new System.Drawing.Size(150, 19);
            this.CheckBoxAppenLogFile.TabIndex = 8;
            this.CheckBoxAppenLogFile.Text = "Vul het log bestand aan";
            this.CheckBoxAppenLogFile.UseVisualStyleBackColor = true;
            this.CheckBoxAppenLogFile.Click += new System.EventHandler(this.CheckBoxAppenLogFile_Click);
            // 
            // CheckBoxActivateLogging
            // 
            this.CheckBoxActivateLogging.AutoSize = true;
            this.CheckBoxActivateLogging.Location = new System.Drawing.Point(6, 22);
            this.CheckBoxActivateLogging.Name = "CheckBoxActivateLogging";
            this.CheckBoxActivateLogging.Size = new System.Drawing.Size(113, 19);
            this.CheckBoxActivateLogging.TabIndex = 7;
            this.CheckBoxActivateLogging.Text = "Activeer logging";
            this.CheckBoxActivateLogging.UseVisualStyleBackColor = true;
            this.CheckBoxActivateLogging.Click += new System.EventHandler(this.CheckBoxActivateLogging_Click);
            // 
            // TabPageApplication
            // 
            this.TabPageApplication.Controls.Add(this.TextBoxSqliteDbFileUser_version);
            this.TabPageApplication.Controls.Add(this.TextBoxApplicationdatabaseMetaVersion);
            this.TabPageApplication.Controls.Add(this.LabelSqliteDbFileUser_version);
            this.TabPageApplication.Controls.Add(this.LabelApplicationdatabaseMetaVersion);
            this.TabPageApplication.Controls.Add(this.TextBoxSQliteVersion);
            this.TabPageApplication.Controls.Add(this.LabelSQliteVersion);
            this.TabPageApplication.Controls.Add(this.GroupBoxDbmaintenance);
            this.TabPageApplication.Controls.Add(this.ButtonLocationAppDatabaseFile);
            this.TabPageApplication.Controls.Add(this.TextBoxLocationDatabaseFile);
            this.TabPageApplication.Controls.Add(this.LabelLocationAppDatabaseFile);
            this.TabPageApplication.Location = new System.Drawing.Point(4, 24);
            this.TabPageApplication.Name = "TabPageApplication";
            this.TabPageApplication.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageApplication.Size = new System.Drawing.Size(651, 327);
            this.TabPageApplication.TabIndex = 1;
            this.TabPageApplication.Text = "Applicatie database";
            this.TabPageApplication.UseVisualStyleBackColor = true;
            // 
            // TextBoxSqliteDbFileUser_version
            // 
            this.TextBoxSqliteDbFileUser_version.Enabled = false;
            this.TextBoxSqliteDbFileUser_version.Location = new System.Drawing.Point(170, 189);
            this.TextBoxSqliteDbFileUser_version.Name = "TextBoxSqliteDbFileUser_version";
            this.TextBoxSqliteDbFileUser_version.Size = new System.Drawing.Size(60, 23);
            this.TextBoxSqliteDbFileUser_version.TabIndex = 26;
            this.TextBoxSqliteDbFileUser_version.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TextBoxApplicationdatabaseMetaVersion
            // 
            this.TextBoxApplicationdatabaseMetaVersion.Enabled = false;
            this.TextBoxApplicationdatabaseMetaVersion.Location = new System.Drawing.Point(170, 160);
            this.TextBoxApplicationdatabaseMetaVersion.Name = "TextBoxApplicationdatabaseMetaVersion";
            this.TextBoxApplicationdatabaseMetaVersion.Size = new System.Drawing.Size(60, 23);
            this.TextBoxApplicationdatabaseMetaVersion.TabIndex = 25;
            this.TextBoxApplicationdatabaseMetaVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LabelSqliteDbFileUser_version
            // 
            this.LabelSqliteDbFileUser_version.AutoSize = true;
            this.LabelSqliteDbFileUser_version.Location = new System.Drawing.Point(12, 192);
            this.LabelSqliteDbFileUser_version.Name = "LabelSqliteDbFileUser_version";
            this.LabelSqliteDbFileUser_version.Size = new System.Drawing.Size(139, 15);
            this.LabelSqliteDbFileUser_version.TabIndex = 24;
            this.LabelSqliteDbFileUser_version.Text = "SQlite db bestand versie :";
            // 
            // LabelApplicationdatabaseMetaVersion
            // 
            this.LabelApplicationdatabaseMetaVersion.AutoSize = true;
            this.LabelApplicationdatabaseMetaVersion.Location = new System.Drawing.Point(9, 163);
            this.LabelApplicationdatabaseMetaVersion.Name = "LabelApplicationdatabaseMetaVersion";
            this.LabelApplicationdatabaseMetaVersion.Size = new System.Drawing.Size(94, 15);
            this.LabelApplicationdatabaseMetaVersion.TabIndex = 23;
            this.LabelApplicationdatabaseMetaVersion.Text = "Database versie :";
            // 
            // TextBoxSQliteVersion
            // 
            this.TextBoxSQliteVersion.Enabled = false;
            this.TextBoxSQliteVersion.Location = new System.Drawing.Point(170, 218);
            this.TextBoxSQliteVersion.Name = "TextBoxSQliteVersion";
            this.TextBoxSQliteVersion.Size = new System.Drawing.Size(60, 23);
            this.TextBoxSQliteVersion.TabIndex = 22;
            this.TextBoxSQliteVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LabelSQliteVersion
            // 
            this.LabelSQliteVersion.AutoSize = true;
            this.LabelSQliteVersion.Location = new System.Drawing.Point(12, 221);
            this.LabelSQliteVersion.Name = "LabelSQliteVersion";
            this.LabelSQliteVersion.Size = new System.Drawing.Size(80, 15);
            this.LabelSQliteVersion.TabIndex = 21;
            this.LabelSQliteVersion.Text = "SQLite versie :";
            // 
            // FormConfigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 407);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.LabelBuildDate);
            this.Controls.Add(this.LabelVersion);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormConfigure";
            this.Text = "FormConfigure";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConfigure_FormClosing);
            this.Load += new System.EventHandler(this.FormConfigure_Load);
            this.GroupBoxOraDbConnections.ResumeLayout(false);
            this.GroupBoxOraDbConnections.PerformLayout();
            this.GroupBoxDbmaintenance.ResumeLayout(false);
            this.GroupBoxDbmaintenance.PerformLayout();
            this.GroupBoxDivers.ResumeLayout(false);
            this.GroupBoxDivers.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.TabPageGeneralOptions.ResumeLayout(false);
            this.GroupBoxVisual.ResumeLayout(false);
            this.GroupBoxVisual.PerformLayout();
            this.GroupBoxLogFile.ResumeLayout(false);
            this.GroupBoxLogFile.PerformLayout();
            this.TabPageApplication.ResumeLayout(false);
            this.TabPageApplication.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelVersion;
        private System.Windows.Forms.Label LabelBuildDate;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Label LabelLocationAppDatabaseFile;
        private System.Windows.Forms.TextBox TextBoxLocationDatabaseFile;
        private System.Windows.Forms.GroupBox GroupBoxOraDbConnections;
        private System.Windows.Forms.CheckBox CheckBoxPasswordIsSchemaName;
        private System.Windows.Forms.CheckBox CheckBoxConstructConnectionName;
        private System.Windows.Forms.GroupBox GroupBoxDbmaintenance;
        private System.Windows.Forms.Button ButtonCompressAppDatabase;
        private System.Windows.Forms.Label LabelCopyAppDb;
        private System.Windows.Forms.TextBox CopyDatabaseIntervalTextBox;
        private System.Windows.Forms.CheckBox CheckBoxWarnOnDeleteQuery;
        private System.Windows.Forms.CheckBox CheckBoxShowQueryGuideLines;
        private System.Windows.Forms.Button ButtonLocationAppDatabaseFile;
        private System.Windows.Forms.GroupBox GroupBoxDivers;
        private System.Windows.Forms.RadioButton RadioButtonAlternateRowColor;
        private System.Windows.Forms.RadioButton RadioButtonNoColor;
        private System.Windows.Forms.RadioButton RadioButton1HighLightDatagrid;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabPageGeneralOptions;
        private System.Windows.Forms.GroupBox GroupBoxLogFile;
        private System.Windows.Forms.Label LabelLocationLogFile;
        private System.Windows.Forms.Label LabelLocationSettingsFile;
        private System.Windows.Forms.CheckBox CheckBoxAppenLogFile;
        private System.Windows.Forms.CheckBox CheckBoxActivateLogging;
        private System.Windows.Forms.TabPage TabPageApplication;
        private System.Windows.Forms.TextBox TextBoxLocationSettingsFile;
        private System.Windows.Forms.TextBox TextBoxLocationLogFile;
        private System.Windows.Forms.GroupBox GroupBoxVisual;
        private System.Windows.Forms.CheckBox CheckBoxHiglightEntryComponents;
        private System.Windows.Forms.Label LabelSQliteVersion;
        private System.Windows.Forms.TextBox TextBoxSQliteVersion;
        private System.Windows.Forms.TextBox TextBoxSqliteDbFileUser_version;
        private System.Windows.Forms.TextBox TextBoxApplicationdatabaseMetaVersion;
        private System.Windows.Forms.Label LabelSqliteDbFileUser_version;
        private System.Windows.Forms.Label LabelApplicationdatabaseMetaVersion;
    }
}