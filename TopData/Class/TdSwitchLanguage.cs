namespace TopData
{
    using System.Globalization;
    using System.Resources;

    /// <summary>
    /// Switch between 2 languages.
    /// </summary>
    public class TdSwitchLanguage
    {
        #region Fields
        private readonly ResourceManager resMan;    // Declare Resource manager to access to specific cultureinfo
        private readonly CultureInfo cul;           // Declare culture info

        /// <summary>
        /// Gets or sets an instance of form main.
        /// </summary>
        public FormMain ParentFormMain { get; set; }

        public FormAbout ParentFormAbout { get; set; }
        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchLanguage"/> class.
        /// Switch between languages.
        /// </summary>
        /// <param name="parent">Instance of of form main.</param>
        /// <param name="cul">holds the culture info.</param>
        public TdSwitchLanguage(FormMain parent, CultureInfo cul)
        {
            this.resMan = new ResourceManager("TopData.Resource.Res_Controls", typeof(FormMain).Assembly);
            this.cul = cul;
            this.ParentFormMain = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdSwitchLanguage"/> class.
        /// </summary>
        /// <param name="parent">Instance of of form about.</param>
        /// <param name="cul">holds the culture info.</param>
        public TdSwitchLanguage(FormAbout parent, CultureInfo cul)
        {
            this.resMan = new ResourceManager("TopData.Resource.Res_Controls", typeof(FormAbout).Assembly);
            this.cul = cul;
            this.ParentFormAbout = parent;
        }

        /// <summary>
        /// Set the language for component texts or labels.
        /// </summary>
        public void SetLanguage()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            // MainForm menu
            // ToolStripMenuItemLanguage.Text = res_man.GetString("Language", Cul);
            this.ParentFormMain.ToolStripMenuItemProgram.Text = this.resMan.GetString("MainForm_Menu_Program", this.cul);
            this.ParentFormMain.ToolStripMenuItemDatabaseConnection.Text = this.resMan.GetString("MainForm_Menu_Program_DatabaseConnections", this.cul);
            this.ParentFormMain.ToolStripMenuItemDatabaseDisconnect.Text = this.resMan.GetString("MainForm_Menu_Program_Disconnect_Database", this.cul);
            this.ParentFormMain.ToolStripMenuItemChangeUser.Text = this.resMan.GetString("MainForm_Menu_Program_Switch_user", this.cul);
            this.ParentFormMain.ToolStripMenuItemClose.Text = this.resMan.GetString("MainForm_Menu_Program_Exit", this.cul);

            this.ParentFormMain.ToolStripMenuItemMaintain.Text = this.resMan.GetString("MainForm_Menu_Manage", this.cul);
            this.ParentFormMain.ToolStripMenuItemMaintainQueries.Text = this.resMan.GetString("MainForm_Menu_Manage_Queries", this.cul);
            this.ParentFormMain.ToolStripMenuItemMaintainDbConnections.Text = this.resMan.GetString("MainForm_Menu_Manage_Connections", this.cul);
            this.ParentFormMain.ToolStripMenuItemmaintainUsers.Text = this.resMan.GetString("MainForm_Menu_Manage_Users", this.cul);
            this.ParentFormMain.ToolStripMenuItemMaintainQueryGroups.Text = this.resMan.GetString("MainForm_Menu_Manage_QueryGroups", this.cul);

            this.ParentFormMain.ToolStripMenuItemOptions.Text = this.resMan.GetString("MainForm_Menu_Options", this.cul);
            this.ParentFormMain.ToolStripMenuItemConfigure.Text = this.resMan.GetString("MainForm_Menu_Options_Settings", this.cul);
            this.ParentFormMain.ToolStripMenuItemLanguage.Text = this.resMan.GetString("MainForm_Menu_Options_Language", this.cul);
            this.ParentFormMain.ToolStripMenuItemLanguageDutch.Text = this.resMan.GetString("MainForm_Menu_Options_Language_NL", this.cul);
            this.ParentFormMain.ToolStripMenuItemLanguageEnglish.Text = this.resMan.GetString("MainForm_Menu_Options_Language_EN", this.cul);
            this.ParentFormMain.ToolStripMenuItemAbout.Text = this.resMan.GetString("MainForm_Menu_Options_Info", this.cul);

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        public void SetLanguage_AboutForm()
        {
            this.ParentFormAbout.LabelVersion.Text = this.resMan.GetString("AboutForm_VersionLabel", this.cul);
        }
    }

    // https://docs.microsoft.com/en-us/answers/questions/281501/how-to-avoid-setting-all-control-modifiers-to-inte.html
}
