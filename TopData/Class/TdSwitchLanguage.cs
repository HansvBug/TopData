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

        /// <summary>
        /// Gets or sets an instance of form about.
        /// </summary>
        public FormAbout ParentFormAbout { get; set; }

        /// <summary>
        /// Gets or sets an instance of form splashscreen.
        /// </summary>
        public FormSplashScreen ParentFormSplashScreen { get; set; }

        /// <summary>
        /// Gets or sets an instance of form maintain users.
        /// </summary>
        public FormMaintainUsers ParentFormMaintainUsers { get; set; }

        /// <summary>
        /// Gets or sets an instance of form maintain query groups.
        /// </summary>
        public FormMaintainQueryGroups ParentFormMaintainQueryGroups { get; set; }

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdSwitchLanguage"/> class.
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
        /// Initializes a new instance of the <see cref="TdSwitchLanguage"/> class.
        /// </summary>
        /// <param name="parent">Instance of the form maintain users.</param>
        /// <param name="cul">holds the culture info.</param>
        public TdSwitchLanguage(FormMaintainUsers parent, CultureInfo cul)
        {
            this.resMan = new ResourceManager("TopData.Resource.Res_Controls", typeof(FormMaintainUsers).Assembly);
            this.cul = cul;
            this.ParentFormMaintainUsers = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdSwitchLanguage"/> class.
        /// </summary>
        /// <param name="parent">Instance of of form maintain query groups.</param>
        /// <param name="cul">holds the culture info.</param>
        public TdSwitchLanguage(FormMaintainQueryGroups parent, CultureInfo cul)
        {
            this.resMan = new ResourceManager("TopData.Resource.Res_Controls", typeof(FormMaintainQueryGroups).Assembly);
            this.cul = cul;
            this.ParentFormMaintainQueryGroups = parent;
        }

        #endregion Constructor

        #region Form main

        /// <summary>
        /// Set the language for component texts or labels.
        /// </summary>
        public void SetLanguageMainForm()
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
        #endregion Form main

        #region Form About

        /// <summary>
        /// Set the language for component texts or labels.
        /// </summary>
        public void SetLanguageAboutForm()
        {
            this.ParentFormAbout.LabelVersion.Text = this.resMan.GetString("AboutForm_VersionLabel", this.cul);
            this.ParentFormAbout.LabelBuilddate.Text = this.resMan.GetString("AboutForm_BuildDateLabel", this.cul);
        }
        #endregion Form About

        #region Splash screen

        /// <summary>
        /// Set the language for component texts or labels.
        /// </summary>
        public void SetLanguageSplashScreenForm()
        {
            this.ParentFormSplashScreen.LabelVersion.Text = this.resMan.GetString("AboutForm_VersionLabel", this.cul);
            this.ParentFormSplashScreen.LabelBuilddate.Text = this.resMan.GetString("AboutForm_BuildDateLabel", this.cul);
        }

        #endregion Splash screen

        /// <summary>
        /// Set the language for component texts or labels.
        /// </summary>
        public void SetLanguageMaintainUsers()
        {
            this.ParentFormMaintainUsers.LabelName.Text = this.resMan.GetString("MantainUserForm_NameLabel", this.cul);
            this.ParentFormMaintainUsers.LabelFullName.Text = this.resMan.GetString("MantainUserForm_FullNameLabel", this.cul);
            this.ParentFormMaintainUsers.LabelPassword.Text = this.resMan.GetString("MantainUserForm_PasswordLabel", this.cul);
            this.ParentFormMaintainUsers.LabelRepeatPassword.Text = this.resMan.GetString("MantainUserForm_RepeatPasswordLabel", this.cul);
            this.ParentFormMaintainUsers.LabelRole.Text = this.resMan.GetString("MantainUserForm_RoleLabel", this.cul);
            this.ParentFormMaintainUsers.LabelAuthencation.Text = this.resMan.GetString("MantainUserForm_AuthenticationLabel", this.cul);
            this.ParentFormMaintainUsers.LabelAuthencationName.Text = this.resMan.GetString("MantainUserForm_AuthenticationNameLabel", this.cul);
            this.ParentFormMaintainUsers.LabelGroup.Text = this.resMan.GetString("MantainUserForm_GroupLabel", this.cul);

            this.ParentFormMaintainUsers.LoggedInNameLabel.Text = this.resMan.GetString("MantainUserForm_NameLabel", this.cul);
            this.ParentFormMaintainUsers.LoggedInRoleLabel.Text = this.resMan.GetString("MantainUserForm_RoleLabel", this.cul);
            this.ParentFormMaintainUsers.LoggedInAuthenticationLabel.Text = this.resMan.GetString("MantainUserForm_AuthenticationLabel", this.cul);

            this.ParentFormMaintainUsers.GroupBoxUserData.Text = this.resMan.GetString("MantainUserForm_GroupBoxUserData", this.cul);
            this.ParentFormMaintainUsers.GroupBoxLoggedInAsUser.Text = this.resMan.GetString("MantainUserForm_GroupBoxLoggedInAsUser", this.cul);

            this.ParentFormMaintainUsers.ButtonCreateUser.Text = this.resMan.GetString("MantainUserForm_NewUser", this.cul);
            this.ParentFormMaintainUsers.ButtonDeleteUser.Text = this.resMan.GetString("MantainUserForm_DeleteUser", this.cul);
            this.ParentFormMaintainUsers.ButtonAlterUser.Text = this.resMan.GetString("MantainUserForm_UpdateUser", this.cul);
            this.ParentFormMaintainUsers.ButtonCancel.Text = this.resMan.GetString("MantainUserForm_CancelUser", this.cul);
            this.ParentFormMaintainUsers.ButtonClose.Text = this.resMan.GetString("MantainUserForm_Close", this.cul);

            this.ParentFormMaintainUsers.Text = this.resMan.GetString("MantainUserForm_Text", this.cul);
        }

        /// <summary>
        /// Set the language for component texts or labels of the form maintain query groups.
        /// </summary>
        public void SetLanguageMaintainQueryGroups()
        {
            this.ParentFormMaintainQueryGroups.ButtonNewQueryGroup.Text = this.resMan.GetString("MaintainQueryForm_New", this.cul);
            this.ParentFormMaintainQueryGroups.ButtonSave.Text = this.resMan.GetString("MaintainQueryForm_Save", this.cul);
            this.ParentFormMaintainQueryGroups.ButtonClose.Text = this.resMan.GetString("MaintainQueryForm_Close", this.cul);
        }
    }

    // https://docs.microsoft.com/en-us/answers/questions/281501/how-to-avoid-setting-all-control-modifiers-to-inte.html
}
