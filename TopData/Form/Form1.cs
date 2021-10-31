namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;
    using Oracle.ManagedDataAccess.Client;

    // using Oracle.DataAccess.Client;

    /// <summary>
    /// The main form.
    /// </summary>
    public partial class FormMain : Form
    {
        #region Properties

        private readonly TdTreeViewSearch tvSearch = new();    // Outside a function otherwise find next doesn't work. (Treeview search).

        /// <summary>
        /// Gets or sets the name of the database file location.
        /// </summary>
        public static string DbLocation { get; set; }

        private TreeNode? previousSelectedNodeTrvQuery = null;    // Keep the node selected when the treeview loses focus

        /// <summary>
        /// Gets or sets the application settings. Holds the user and application setttings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application lock is active.
        /// If the application database is missing. most of the app will be locked because it wont work.
        /// </summary>
        private bool LockProgram { get; set; }

        /// <summary>
        /// Gets or sets the Oracle connection. Holds the Oracle connection "GBIbasis schema".
        /// </summary>
        public OracleConnection? OraConn { get; set; }

        /// <summary>
        /// Gets or sets the second Oracle connection. Holds the Oracle connection "GBIsystem schema".
        /// </summary>
        public OracleConnection OraConnSystem { get; set; }

        /// <summary>
        /// Gets or sets the Oracle schema name.
        /// </summary>
        public string OraSchema { get; set; }

        /// <summary>
        /// Gets or sets the Oracle connection name.
        /// </summary>
        public string OraConnectionName { get; set; }

        /// <summary>
        /// Gets or sets the decimal seperator needed for filter floats.
        /// </summary>
        public string DecimalSeperator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the SDO geometry column is vissble when 1 query is executed.
        /// </summary>
        public bool SdoGeometryColumnVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the SDO geometry column is vissble when mutiple queries are executed.
        /// </summary>
        public bool SdoGeometryColumnVisibleMultipleQueries { get; set; }

        /// <summary>
        /// Gets or sets the index if the SDO geometry column in the datatabe/dataset.
        /// </summary>
        public int IndexSdoGeometryColumn { get; set; }

        private readonly SQLLiteTopData sqliteApp = new ();

        /// <summary>
        /// Gets reference to SQLLiteApp. Holds the folders and queries.
        /// </summary>
        public SQLLiteTopData SQLLiteApp
        {
            get { return this.sqliteApp; }
        }

        /// <summary>
        /// Gets or sets the Export datatable.
        /// Make the datatable connection to the datagridview available outsite the form.
        /// </summary>
        public DataTable DatatabelExport { get; set; }

        private List<string> savedExpansionState = new ();  // Save the treeview state

        /// <summary>
        /// Gets or sets the active datagridview.
        /// </summary>
        private DataGridView Dgv { get; set; }

        // Query is active
        private int QueryId { get; set; }

        private string QueryGuid { get; set; }

        private int QueryGroupId { get; set; }

        private bool Uitvoeren { get; set; } // Used when multiple query's will be executed. Needed for the excel export. The excel export must be rewritten!!!! It is not good.

        /// <summary>
        /// Gets or sets a value indicating whether there is a active Querygroup.
        /// </summary>
        public bool QueryGroupIsActive { get; set; }

        // Execute multiple queries
        private string OverRuleExcelFilePath { get; set; } // Multiple query execution change filename if it all ready exists

        private bool OverWriteFile { get; set; }

        private readonly List<string> pathFileName = new ();

        /// <summary>
        /// Gets or sets a value indicating whether multiple query's are with ore without export file name.
        /// </summary>
        public bool MultipleQueriesWithoutFile { get; set; } // Execute multiple query's when there is no exportfile.

        private bool DoNotSaveFile { get; set; }

        // end multiple queries.
        private TdExecuteQueries queryExec = new ();

        /// <summary>
        /// Gets or sets a TdExecuteQueries reference.
        /// </summary>
        public TdExecuteQueries QueryExec
        {
            get { return this.queryExec; }
            set { this.queryExec = value; }
        }

        // Filter
        private TdDatabaseFilters filters = new ();

        /// <summary>
        /// Gets or sets a DatabaseFilters reference.
        /// </summary>
        public TdDatabaseFilters Filters
        {
            get { return this.filters; }
            set { this.filters = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the filter form was opened.
        /// </summary>
        public bool FilterFormIsOpenend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the query result (and data table) is filterd.
        /// </summary>
        public bool DataTableIsFilterd { get; set; }

        private int filterFormHasBeenOpenend = 0;

        /// <summary>
        /// Gets or sets if the filter form has been opened.
        /// </summary>
        public int FilterFormHasBeenOpenend
        {
            get
            {
                return this.filterFormHasBeenOpenend;
            }

            set
            {
                this.filterFormHasBeenOpenend = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether there is a filter active.
        /// </summary>
        public bool FilterIsActive { get; set; }

        private DataGridView dataGridViewFilterd;

        /// <summary>
        /// Gets or sets the filterd datagridview.
        /// </summary>
        public DataGridView DataGridViewFilterd
        {
            get { return this.dataGridViewFilterd; }
            set { this.dataGridViewFilterd = value; }
        }

        // Datagridview
        private DataGridViewCellStyle highlightStyle; // The style to use when the mouse is over a row.

        private int highlightedRowIndex = -1;

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain"/> class.
        /// The main form of Topdata.
        /// </summary>
        public FormMain()
        {
            this.InitializeComponent();

            CheckFolders();              // Create in appdata a new folder Settings and/or Database if needed
            CreateSettingsFile();        // Create the settings file if needed
            this.GetSettings();          // Get the settings a user saved
            this.SetCulture();
            GetDebugMode();
            this.StartLogging();         // Start the logging

            this.AddItemToSystemMenu();  // Add menu item tot system Menu
            this.BackColor = SystemColors.Window;
            this.Text = TdSettingsDefault.ApplicationName;
            this.SplitContainer1FormMain.SplitterWidth = 10;
            this.SplitContainerQueryTree.SplitterWidth = 10;
            this.LoadFormPosition();     // Load the last saved form position

            // Check if the application dabase file does exists. If not then create it when the Install argument is found.
            if (this.CheckAppDatabaseFile())
            {
                TdUserData.FirstLogIn = true;

                if (!this.LockProgram)
                {
                    this.CreateApplicationDatabaseTables();  // Create the tables.
                    this.CheckForUpdates(); // Check for updates;
                    this.CheckForArguments();
                }
            }
            else
            {
                MessageBox.Show(
                    "Het applicatie database bestand is niet gevonden. De applicatie wordt afgesloten." + Environment.NewLine +
                    Environment.NewLine +
                    "Start de applicatie met de parameter 'Install'.",
                    MB_Title.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                Application.Exit(); // Ends program major error.
            }

            this.CreateAutoCompleteForSearchBox();   // Create a autocomplete list for the search query textbox
            this.Enablefunctions();
        }
        #endregion Constructor

        private static void GetDebugMode()
        {
            using TdProcessArguments getArg = new ();
            foreach (string arg in getArg.CmdLineArg)
            {
                string argument = Convert.ToString(arg, CultureInfo.InvariantCulture);

                if (argument == getArg.ArgDebug)
                {
                    TdDebugMode.DebugMode = true;
                }
            }
        }

        #region Load form
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Initialize();
            this.DoubleBuffered = true;
        }

        // called from Form1_Load
        private void Initialize()
        {
            string dbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            string appDb = Path.Combine(dbLocation, TdSettingsDefault.SqlLiteDatabaseName);

            this.LoadSplitterPosition();

            // Check to see if the application database exists
            if (!File.Exists(appDb))
            {
                /*splashForm.Close();
                MessageBox.Show(ResourceMB._1039 + Environment.NewLine +
                               Environment.NewLine +
                               "Het programma wordt afgesloten."

                                    , ResourceMB._0101, MessageBoxButtons.OK, MessageBoxIcon.Warning); */
                Application.Exit(); // Ends program major error.
            }
            else
            {
                using TdAppDbMaintain appdbUserVersion = new();
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug("De applicatie database user versie = " + appdbUserVersion.GetPramaUserVersion());
                    TdLogging.WriteToLogDebug("De SQLite versie is : " + appdbUserVersion.SQLiteVersion());
                }

                this.CopyAppDatabase();       // Every xx times the application starts it makes a copy of the application database
                this.LoadOracleConnections(); // Loads the Oracle connections into the menu

                this.CreBindingNavigator(); // Create a binding navigator for the datagridview;
                TdExtensionMethods.DoubleBuffered(this.DataGridViewQueries, true); // Avoid flickering datagridview with large amount of data.
                TdExtensionMethods.DoubleBuffered(this.TreeViewExecuteQueries, true);
                this.LoadTreeviewState();
            }
        }
        #endregion Load form

        #region Load form helpers

        private static void CheckFolders()
        {
            TdAppEnvironment checkPath = new ();
            checkPath.CreateFolder(TdSettingsDefault.ApplicationName, true);
            checkPath.CreateFolder(TdSettingsDefault.ApplicationName + @"\" + TdSettingsDefault.SettingsFolder, true);
            checkPath.CreateFolder(TdSettingsDefault.DatabaseFolder, false);
            checkPath.CreateFolder(TdSettingsDefault.DatabaseFolder + @"\" + TdSettingsDefault.BackUpFolder, false);
            checkPath.Dispose();
        }

        private static void CreateSettingsFile()
        {
            // Create a settings file with default values. (if the file does not exists)
            using TdSettingsManager set = new ();  // Contructor gets the path of the settings file. So this line is needed.
            TdSettingsManager.CreateSettingsFile();
        }

        private void GetSettings()
        {
            try
            {
                using TdSettingsManager set = new ();
                set.LoadSettings();

                if (set.JsonObjSettings != null)
                {
                    this.JsonObjSettings = set.JsonObjSettings;
                }
                else
                {
                    MessageBox.Show(MB_Text.Settings_File_Not_Found, MB_Title.Attention, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (AccessViolationException)
            {
                // Logging is not available here
                MessageBox.Show(
                    "Fout bij het laden van de instellingen. " + Environment.NewLine +
                    "De default instellingen worden ingeladen.", MB_Title.Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void SetCulture()
        {
            if (this.JsonObjSettings.AppParam[0].Language == "en-US")
            {
                TdCulture.Cul = CultureInfo.CreateSpecificCulture("en-US");

                this.ToolStripMenuItemLanguageDutch.Checked = false;
                this.ToolStripMenuItemLanguageEnglish.Checked = true;
            }
            else if (this.JsonObjSettings.AppParam[0].Language == "nl-NL")
            {
                TdCulture.Cul = CultureInfo.CreateSpecificCulture("nl-NL");

                this.ToolStripMenuItemLanguageDutch.Checked = true;
                this.ToolStripMenuItemLanguageEnglish.Checked = false;
            }

            MB_Title.Cul = TdCulture.Cul;
            MB_Text.Cul = TdCulture.Cul;
            TdLogging_Resources.Cul = TdCulture.Cul;

            MB_Title.RmMb = new ResourceManager("TopData.Resource.Res_Messagebox", typeof(FormMain).Assembly);
            MB_Text.RmMb = new ResourceManager("TopData.Resource.Res_Messagebox", typeof(FormMain).Assembly);
            TdLogging_Resources.RmLog = new ResourceManager("TopData.Resource.Res_Log", typeof(FormMain).Assembly);

            TdSwitchLanguage sl = new(this, TdCulture.Cul);
            sl.SetLanguageMainForm();
        }

        private void StartLogging()
        {
            TdLogging.NameLogFile = TdSettingsDefault.LogFileName;
            TdLogging.LogFolder = this.JsonObjSettings.AppParam[0].LogFileLocation;
            TdLogging.AppendLogFile = this.JsonObjSettings.AppParam[0].AppendLogFile;
            TdLogging.ActivateLogging = this.JsonObjSettings.AppParam[0].ActivateLogging;

            TdLogging.ApplicationName = TdSettingsDefault.ApplicationName;
            TdLogging.ApplicationVersion = TdSettingsDefault.ApplicationVersion;
            TdLogging.ApplicationBuildDate = TdSettingsDefault.ApplicationBuildDate;
            TdLogging.Parent = this;

            if (TdDebugMode.DebugMode)
            {
                TdLogging.DebugMode = true;
            }

            if (!TdLogging.StartLogging())
            {
                TdLogging.WriteToFile = false;  // Stop the logging
                TdLogging.ActivateLogging = false;
                this.JsonObjSettings.AppParam[0].ActivateLogging = false;
                this.JsonObjSettings.AppParam[0].AppendLogFile = false;
            }
            else
            {
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(string.Empty);
                    TdLogging.WriteToLogDebug(TdLogging_Resources.Debug_Logging_On);
                    TdLogging.WriteToLogDebug(string.Empty);
                }
                else
                {
                    TdLogging.WriteToLogDebug("DebugMode = OFF.");
                }
            }
        }

        private void LoadFormPosition()
        {
            using TdFormPosition frmPos = new (this);
            frmPos.LoadMainFormPosition();
        }

        private void LoadSplitterPosition()
        {
            using TdFormPosition frmPos = new(this);
            frmPos.LoadMainFormSplitterPosition();
        }

        private void AddItemToSystemMenu()
        {
            using NativeMethods addItem = new ()
            {
                Parent = this,
            };
            addItem.SetupSystemMenu();
        }

        private bool CheckAppDatabaseFile()
        {
            Cursor.Current = Cursors.WaitCursor;

            string dbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            this.LockProgram = true;
            bool result = false;

            if (!File.Exists(Path.Combine(dbLocation, TdSettingsDefault.SqlLiteDatabaseName)))
            {
                using TdProcessArguments getArg = new();
                string argument = string.Empty;

                using TdAppDbCreate appDb = new();

                foreach (string arg in getArg.CmdLineArg)
                {
                    argument = Convert.ToString(arg, CultureInfo.InvariantCulture);
                    if (argument == getArg.ArgIntall)
                    {
                        TdLogging.WriteToLogWarning(TdLogging_Resources.TheAppDbWillBeCreated);
                        if (appDb.CreateNewDatabase())
                        {
                            MessageBox.Show(MB_Text.App_Database_Created, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.LockProgram = false;
                            result = true;
                        }
                        else
                        {
                            this.LockProgram = true;
                        }
                    }
                }
            }
            else
            {
                this.LockProgram = false;
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug("Het applicatie database bestand is aanwezig.");
                }

                result = true;
            }

            Cursor.Current = Cursors.Default;
            return result;
        }

        private void CreateApplicationDatabaseTables()
        {
            Cursor.Current = Cursors.WaitCursor;

            string dbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            string dBFilePath = Path.Combine(dbLocation, TdSettingsDefault.SqlLiteDatabaseName);

            if (File.Exists(dBFilePath))
            {
                using TdProcessArguments getArg = new();
                using TdAppDbCreate appDb = new();

                if (appDb.SelectMeta() < TdSettingsDefault.UpdateVersion)
                {
                    foreach (string arg in getArg.CmdLineArg)
                    {
                        if (arg == getArg.ArgIntall)
                        {
                            if (appDb.CreateTables())
                            {
                                this.LockProgram = false;
                            }
                            else
                            {
                                this.LockProgram = true;
                            }
                        }
                    }
                }
            }
            else
            {
                TdLogging.WriteToLogInformation("De applicatie database is niet aanwezig op locatie: " + dBFilePath);
            }

            Cursor.Current = Cursors.Default;
        }

        private void CheckForUpdates()
        {
            Cursor.Current = Cursors.WaitCursor;

            string dbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            string dBFilePath = Path.Combine(dbLocation, TdSettingsDefault.SqlLiteDatabaseName);

            if (File.Exists(dBFilePath))
            {
                using TdAppDbUpdate appDbUpdate = new();

                if (appDbUpdate.SelectMeta() < TdSettingsDefault.UpdateVersion)
                {
                    if (!appDbUpdate.Updatetables())
                    {
                        TdLogging.WriteToLogError("Het update van de applicatie database is mislukt.");
                        MessageBox.Show("De update van de applicatie database is mislukt.", MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.LockProgram = true;
                    }

                    MessageBox.Show(MB_Text.App_Database_Changed, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                TdLogging.WriteToLogError("Database bestand niet aangetroffen tijdens de versie controle.");
                MessageBox.Show("De update van de applicatie database is mislukt. Het bestand is niet aangetroffen.", MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.LockProgram = true;
            }

            Cursor.Current = Cursors.Default;
        }

        private void CheckForArguments()
        {
            Cursor.Current = Cursors.WaitCursor;

            string dbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            string dBFilePath = Path.Combine(dbLocation, TdSettingsDefault.SqlLiteDatabaseName);

            if (File.Exists(dBFilePath))
            {
                using TdProcessArguments getArg = new();
                using TdAppDbCreate appDb = new();

                // Allways check for other parameters
                foreach (string arg in getArg.CmdLineArg)
                {
                    // arg = Convert.ToString(arg, CultureInfo.InvariantCulture);
                    if (arg == getArg.ArgInstallOwner)
                    {
                        TdLogging.WriteToLogInformation("De gebruiker Owner opnieuw toevoegen.");
                        appDb.AddInstallUserOwner("x");
                    }
                    else if (arg == getArg.ArgInstallSystem)
                    {
                        TdLogging.WriteToLogInformation("De gebruiker System opnieuw toevoegen.");
                        appDb.AddInstallUserSystem("x");
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        #endregion Load form helpers

        #region Close form
        private void ToolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveFormPosition();
            this.SaveSettings();
            this.SaveTreeviewState();
            TdLogging.StopLogging();
        }

        private void SaveFormPosition()
        {
            using TdFormPosition frmPos = new(this);
            frmPos.SaveMainFormPosition();
            frmPos.SaveMainFormSplitterPosition();
        }

        private void SaveSettings()
        {
            TdSettingsManager.SaveSettings(this.JsonObjSettings);
        }
        #endregion Close form

        #region Open sub Forms
        private void ToolStripMenuItemConfigure_Click(object sender, EventArgs e)
        {
            TdLogging.WriteToLogInformation("Openen configuratie scherm.");

            TdSettingsManager.SaveSettings(this.JsonObjSettings);

            FormConfigure frm = new ();
            frm.ShowDialog();
            frm.Dispose();

            this.GetSettings();
            this.SetDataGridRowColor();
        }

        private void ToolStripMenuItemAbout_Click_1(object sender, EventArgs e)
        {
            TdLogging.WriteToLogInformation("Openen about scherm.");

            FormAbout frm = new ();
            frm.ShowDialog();
            frm.Dispose();
        }

        private void ToolStripMenuItemMaintainDbConnections_Click(object sender, EventArgs e)
        {
            if (TdUserData.UserRole == TdRoleTypes.Owner || TdUserData.UserRole == TdRoleTypes.System || TdUserData.UserRole == TdRoleTypes.Administrator)
            {
                TdLogging.WriteToLogInformation("Openen beheren Oracle databse connecties scherm.");
                TdSettingsManager.SaveSettings(this.JsonObjSettings);

                FormMaintainOraDbConnections frm = new ();

                frm.ShowDialog();
                frm.Dispose();

                this.GetSettings();
            }
            else
            {
                MessageBox.Show(
                    "U heeft onvoldoende rechten om databaseconnecties te maken of te bewerken.",
                    "Informatie.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            this.LoadOracleConnections();   // Add a new connection to the connection menu.

            if (this.ToolStripMenuItemDatabaseConnection.DropDownItems.Count > 0)
            {
                this.ToolStripMenuItemDatabaseConnection.Enabled = true;
            }
            else
            {
                this.ToolStripMenuItemDatabaseConnection.Enabled = false;
            }
        }

        private void ToolStripMenuItemmaintainUsers_Click(object sender, EventArgs e)
        {
            TdLogging.WriteToLogInformation("Openen beheren gebruikers scherm.");

            TdSettingsManager.SaveSettings(this.JsonObjSettings);

            FormMaintainUsers frm = new ();

            frm.ShowDialog();
            frm.Dispose();

            this.GetSettings();
        }

        private void ToolStripMenuItemChangeUser_Click(object sender, EventArgs e)
        {
            TdLogging.WriteToLogInformation("Openen wisselen van gebruiker scherm.");

            TdSettingsManager.SaveSettings(this.JsonObjSettings);
            this.SaveTreeviewState();  // Always save the current treeview state. Even if the will not be changed later on

            this.OpenFormUserLogin();
            this.GetSettings();
        }

        private void OpenFormUserLogin()
        {
            string dBaseName = this.ToolStripStatusLabel3.Text;

            FormUserLogin frm = new ()
            {
                TopMost = true,
            };
            frm.ShowDialog(this);
            frm.Dispose();

            if (TdUserData.ChangeUser)
            {
                if (this.OraConn != null)
                {
                    this.OraConn.Close();
                    this.OraConn = null;
                    TdUserData.ConnectionName = string.Empty;
                    TdUserData.ConnectionId = -1;
                }

                this.Enablefunctions();
                this.RemoveVisibleDataWhenConnectionIsLost();

                dBaseName = string.Empty;
            }

            if (!string.IsNullOrEmpty(dBaseName))
            {
                this.ToolStripStatusLabel3.Text = dBaseName;
            }

            this.Refresh();
        }

        private void ToolStripMenuItemMaintainQueries_Click(object sender, EventArgs e)
        {
            TdLogging.WriteToLogInformation("Openen Beheren query's scherm.");
            this.ToolStripStatusLabel1.Text = ResourceText._2009;
            this.Refresh();

            TdSettingsManager.SaveSettings(this.JsonObjSettings);

            if (this.OraConn != null)
            {
                this.SaveTreeviewState();
            }

            FormMaintainQueries frm = new(this);
            frm.ShowDialog();
            frm.Dispose();

            this.GetSettings();

            if (this.OraConn != null)
            {
                this.ReloadTreeView();
                this.LoadQueryGroupNames();
            }

            this.ButtonRunQueryAndExport.Enabled = false;
        }

        private void ToolStripMenuItemMaintainQueryGroups_Click(object sender, EventArgs e)
        {
            if (TdUserData.UserRole == TdRoleTypes.Owner || TdUserData.UserRole == TdRoleTypes.System || TdUserData.UserRole == TdRoleTypes.Administrator)
            {
                TdLogging.WriteToLogInformation("Openen Beheren querygroep scherm.");
                this.Refresh();

                TdSettingsManager.SaveSettings(this.JsonObjSettings);

                FormMaintainQueryGroups frm = new ();
                frm.ShowDialog();
                frm.Dispose();

                this.GetSettings();

                if (this.OraConn != null)
                {
                    this.LoadQueryGroupNames();
                }
            }
            else
            {
                MessageBox.Show(
                    "U heeft onvoldoende rechten om Query groepen aan te maken of te bewerken.",
                    "Informatie.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        #endregion Open sub Forms

        private void ReloadTreeView()
        {
            this.ToolStripStatusLabel1.Text = string.Empty;
            this.LoadTreeViewQueries();
            this.LoadTreeviewState();
        }

        private void Enablefunctions()
        {
            if (this.OraConn != null)
            {
                this.ToolStripMenuItemDatabaseDisconnect.Enabled = true;
                this.ToolStripMenuItemMaintainQueryGroups.Enabled = true;

                if (this.TreeViewExecuteQueries.Nodes.Count > 0)
                {
                    this.ButtonRunQuery.Enabled = true;
                    this.ComboBoxQueryGroup.Enabled = true;
                }
                else
                {
                    this.ButtonRunQuery.Enabled = false;
                    this.ComboBoxQueryGroup.Enabled = false;
                }
            }
            else
            {
                this.ToolStripMenuItemDatabaseDisconnect.Enabled = false;
                this.ToolStripMenuItemMaintainQueryGroups.Enabled = false;
                this.ButtonRunQuery.Enabled = false;
                this.ButtonRunQueryAndExport.Enabled = false;
            }

            this.ToolStripStatusLabel1.Text = string.Empty;
            this.ToolStripStatusLabel2.Text = string.Empty;
            this.ToolStripStatusLabel2.Text = string.Empty;
            this.ToolStripStatusLabel3.Text = string.Format("Database = {0}", TdUserData.ConnectionName) + "          ";
            this.ToolStripStatusLabel4.Text = string.Format("Gebruiker : {0}", TdUserData.UserName) + "  ";

            // when there is no application database LockProgram will be true.
            if (this.LockProgram)
            {
                this.ToolStripMenuItemMaintain.Enabled = false;
                this.ToolStripMenuItemConfigure.Enabled = false;
                TdUserData.AccesIsTrue = false;

                this.ToolStripMenuItemDatabaseConnection.Enabled = false;
                this.ToolStripMenuItemChangeUser.Enabled = false;
                this.ToolStripMenuItemClose.Enabled = true;

                this.ToolStripMenuItemMaintain.Enabled = false;

                this.ToolStripMenuItemConfigure.Enabled = false;
                this.ToolStripMenuItemLanguage.Enabled = true;
                this.ToolStripMenuItemAbout.Enabled = true;

                this.ComboBoxQueryGroup.Enabled = false;
            }
            else
            {
                if (string.IsNullOrEmpty(TdUserData.UserName))
                {
                    this.ToolStripMenuItemDatabaseConnection.Enabled = false;
                    this.ToolStripMenuItemChangeUser.Enabled = true;
                    this.ToolStripMenuItemClose.Enabled = true;

                    this.ToolStripMenuItemMaintain.Enabled = false;

                    this.ToolStripMenuItemConfigure.Enabled = false;
                    this.ToolStripMenuItemLanguage.Enabled = true;
                    this.ToolStripMenuItemAbout.Enabled = true;

                    TdUserData.AccesIsTrue = false;
                }
                else
                {
                    if (this.ToolStripMenuItemDatabaseConnection.DropDownItems.Count > 0)
                    {
                        this.ToolStripMenuItemDatabaseConnection.Enabled = true;
                    }
                    else
                    {
                        this.ToolStripMenuItemDatabaseConnection.Enabled = false;
                    }

                    this.ToolStripMenuItemChangeUser.Enabled = true;
                    this.ToolStripMenuItemClose.Enabled = true;

                    this.ToolStripMenuItemMaintain.Enabled = true;

                    this.ToolStripMenuItemConfigure.Enabled = true;
                    this.ToolStripMenuItemLanguage.Enabled = true;
                    this.ToolStripMenuItemAbout.Enabled = true;
                    TdUserData.AccesIsTrue = true;
                }
            }
        }

        #region Splitcontainer place dots
        private void SplitContainer1FormMain_Paint(object sender, PaintEventArgs e)
        {
            TdVisual.Splitcontainerhandle(this.SplitContainer1FormMain, e);
        }

        private void SplitContainerQueryTree_Paint(object sender, PaintEventArgs e)
        {
            TdVisual.Splitcontainerhandle(this.SplitContainerQueryTree, e);
        }
        #endregion Splitcontainer place dots

        private void TextBoxSearchInQueryTreeView_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void TextBoxSearchInQueryTreeView_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void CopyAppDatabase()
        {
            Cursor.Current = Cursors.WaitCursor;
            int counter = this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartupsCounter;
            counter++;  // Add a new start to the counter

            int copyAppDataBaseAfterEveryXStartups = this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartups;
            if (TdDebugMode.DebugMode)
            {
                TdLogging.WriteToLogDebug("De teller voor het aanmaken van een kopie van de applicatie database staat op : " + Convert.ToString(counter, CultureInfo.InvariantCulture));
                TdLogging.WriteToLogDebug("De database wordt gekopieerd bij elke xx keer opstarten : " + Convert.ToString(copyAppDataBaseAfterEveryXStartups, CultureInfo.InvariantCulture));
            }

            // If counter equals the option setting then make a copy
            if (counter >= copyAppDataBaseAfterEveryXStartups)
            {
                using TdAppDbMaintain appDbMaintain = new ();

                // Make a copy at the startup of the application
                if (!appDbMaintain.CopyDatabaseFile("StartUp"))
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(MB_Text.App_Database_Copy_Failed, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    TdLogging.WriteToLogInformation("De applicatie database is gekopieerd na " + Convert.ToString(counter, CultureInfo.InvariantCulture) + " keer opstarten.");
                }

                this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartupsCounter = 0;
            }
            else
            {
                this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartupsCounter = counter;
            }

            Cursor.Current = Cursors.Default;
        }

        private void RemoveVisibleDataWhenConnectionIsLost()
        {
            // Clear the export datatable
            this.ToolStripStatusLabel1.Text = string.Empty;
            this.ToolStripStatusLabel3.Text = "Database = Geen verbinding";
            TdUserData.ConnectionName = string.Empty;

            /*
            if (this.DatatabelExport != null)
            {
                this.DatatabelExport.Clear();
            }
            */

            // DisconnectDatabaseConnectionToolStripMenuItem.Enabled = false;
            this.ButtonRunQuery.Enabled = false;

            // ButtonExport.Enabled = false;
            // ButtonRunQueryAndExport.Enabled = false;
            this.TreeViewExecuteQueries.Nodes.Clear();

            this.DataGridViewQueries.DataSource = null;        // Clear the datagrid when starting the queries

            // DataGridViewVulling.DataSource = null;  //has no datasource (yet)
            // DataGridViewVulling.Rows.Clear();
            // DataGridViewGBI.DataSource = null;
            // DataGridViewGBI.Rows.Clear();

            // DataGridView2.DataSource = null;
            // DataGridView2.Rows.Clear();

            // TreeViewSchemaObjects.Nodes.Clear();
            // TreeViewFilling.Nodes.Clear();

            // LabelTableName.Text = ResourceText._2012;
            // LabelRecordCount.Text = ResourceText._2014;
            // GetTableNamesIsExecuted = false;
            this.ComboBoxQueryGroup.Enabled = false;
            this.ComboBoxQueryGroup.Text = string.Empty;
            this.ComboBoxQueryGroup.Items.Clear();

            // BindingNavigator1.BindingSource = null;
            // bindingNavigator3.BindingSource = null;
            // bindingNavigator2.BindingSource = null;
            // RichTextBoxQueryDescription.Clear();
            this.GroupBoxPrepare.Enabled = false;
            this.GroupBoxPassports.Enabled = false;
            this.GroupBoxDomainValues.Enabled = false;

            // ButtonGetHelpTable.Enabled = false;
            // ButtonExportDomeinwaardeCTRL.Enabled = false;
            this.RichTextBoxQueryDescription.Text = string.Empty;
            this.Refresh();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            // Check if user has user authentication. If not hen open the log in form.
            if (!UserAutentication())
            {
                // splashForm.Close();
                this.OpenFormUserLogin();
                this.GetSettings();
            }

            this.SetDataGridRowColor();
            this.Enablefunctions();
            this.ToolStripStatusLabel1.Text = ResourceText._1004;
            this.Refresh();
        }
        #region AutoInlog

        private static bool UserAutentication()
        {
            bool autoLogIn = false;

            using (TdMaintainUsers userAutentication = new ())
            {
                if (!string.IsNullOrEmpty(userAutentication.GetUserAuthentication()))
                {
                    autoLogIn = true;
                    TdUserData.AccesIsTrue = true;
                    TdUserData.FirstLogIn = false;
                }
            }

            return autoLogIn;
        }

        #endregion AutoInlog

        #region Switch language
        private void SetLanguageMenuItem()
        {
            TdCulture.Cul = CultureInfo.CreateSpecificCulture("en-US");

            if (this.JsonObjSettings.AppParam[0].Language == "en-US")
            {
                this.ToolStripMenuItemLanguageEnglish.Checked = true;
                this.ToolStripMenuItemLanguageDutch.Checked = false;

                TdCulture.Cul = CultureInfo.CreateSpecificCulture("en-US");

                MB_Title.Cul = TdCulture.Cul;
                MB_Text.Cul = TdCulture.Cul;
                TdLogging_Resources.Cul = TdCulture.Cul;
            }
            else if (this.JsonObjSettings.AppParam[0].Language == "nl-NL")
            {
                this.ToolStripMenuItemLanguageEnglish.Checked = false;
                this.ToolStripMenuItemLanguageDutch.Checked = true;

                TdCulture.Cul = CultureInfo.CreateSpecificCulture("nl-NL");

                MB_Title.Cul = TdCulture.Cul;
                MB_Text.Cul = TdCulture.Cul;
                TdLogging_Resources.Cul = TdCulture.Cul;
            }

            TdSwitchLanguage sl = new (this, TdCulture.Cul);
            sl.SetLanguageMainForm();
        }

        private void ToolStripMenuItemLanguageEnglish_Click(object sender, EventArgs e)
        {
            if (this.ToolStripMenuItemLanguageEnglish.Checked == true)
            {
                this.ToolStripMenuItemLanguageDutch.Checked = true;
                this.ToolStripMenuItemLanguageEnglish.Checked = false;
                this.JsonObjSettings.AppParam[0].Language = "nl-NL";

                TdCulture.Cul = CultureInfo.CreateSpecificCulture("nl-NL");
                MB_Title.Cul = TdCulture.Cul;
                MB_Text.Cul = TdCulture.Cul;
            }
            else
            {
                this.ToolStripMenuItemLanguageDutch.Checked = false;
                this.ToolStripMenuItemLanguageEnglish.Checked = true;
                this.JsonObjSettings.AppParam[0].Language = "en-US";

                TdCulture.Cul = CultureInfo.CreateSpecificCulture("en-US");

                MB_Title.Cul = TdCulture.Cul;
                MB_Text.Cul = TdCulture.Cul;
            }

            TdSwitchLanguage sl = new (this, TdCulture.Cul);
            sl.SetLanguageMainForm();
        }

        private void ToolStripMenuItemLanguageDutch_Click(object sender, EventArgs e)
        {
            if (this.ToolStripMenuItemLanguageDutch.Checked == true)
            {
                this.ToolStripMenuItemLanguageDutch.Checked = false;
                this.ToolStripMenuItemLanguageEnglish.Checked = true;
                this.JsonObjSettings.AppParam[0].Language = "en-US";

                TdCulture.Cul = CultureInfo.CreateSpecificCulture("en-US");

                MB_Title.Cul = TdCulture.Cul;
                MB_Text.Cul = TdCulture.Cul;
            }
            else
            {
                this.ToolStripMenuItemLanguageDutch.Checked = true;
                this.ToolStripMenuItemLanguageEnglish.Checked = false;
                this.JsonObjSettings.AppParam[0].Language = "nl-NL";

                TdCulture.Cul = CultureInfo.CreateSpecificCulture("nl-NL");

                MB_Title.Cul = TdCulture.Cul;
                MB_Text.Cul = TdCulture.Cul;
            }

            TdSwitchLanguage sl = new (this, TdCulture.Cul);
            sl.SetLanguageMainForm();
        }

        #endregion Switch language

        #region Get Oracle connections
        private bool LoadOracleConnections()
        {
            this.ToolStripStatusLabel1.Text = "Ophalen database connecties...";
            this.ToolStripMenuItemDatabaseConnection.DropDownItems.Clear();  // First clear the menu
            this.Refresh();

            TdOracleConnectionMaintain maintainOraConns = new ();

            TdOracleConnections oraConns = maintainOraConns.GetOracleConnectionNames();  // Get all the oracle conncetions

            string connName = string.Empty;
            this.ComboBoxGBIsystemConnName.Items.Clear();
            this.ComboBoxGBIbasisConnName.Items.Clear();

            // Loop through the Oracle connections and add them to the menu
            foreach (TdOracleConnection oraCon in oraConns.Items)
            {
                ToolStripItem menuItem = this.ToolStripMenuItemDatabaseConnection.DropDownItems.Add(oraCon.Name);
                menuItem.Click += new System.EventHandler(this.ToolStripMenuItemDatabaseConnection_Click);
                menuItem.Tag = oraCon;
                menuItem.Name = "OracleMenuItem_" + oraCon.Id;
                connName = menuItem.Name;

                // load the combobox with the schema's. Needed for copying system tables to gbibasis
                this.ComboBoxGBIsystemConnName.Items.Add(oraCon.Name);
                this.ComboBoxGBIbasisConnName.Items.Add(oraCon.Name);
            }

            this.ToolStripStatusLabel1.Text = string.Empty;
            this.Refresh();

            if (!string.IsNullOrEmpty(connName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CloseOracleConnection()
        {
            if (this.OraConn != null)
            {
                this.SaveTreeviewState();  // Save the current treeview state needed when changing the connection.

                if (this.OraConn.State == ConnectionState.Open)
                {
                    this.OraConn.Close();
                    this.OraConn = null;
                    this.OraConnectionName = string.Empty;

                    TdUserData.ConnectionId = -1;
                    this.Enablefunctions();

                    // Moet naar ---> Enablefunctions---------------------------------------------------------------------------------------------------<<<AANPASSEN
                    this.GroupBoxPrepare.Enabled = false;
                    this.GroupBoxPassports.Enabled = false;
                    this.GroupBoxDomainValues.Enabled = false;
                    this.ButtonGetHelpTable.Enabled = false;
                    this.ButtonExportDomeinwaardeCTRL.Enabled = false;

                    this.Refresh();
                }
            }
        }

        private void ToolStripMenuItemDatabaseConnection_Click(object sender, EventArgs e) // Make a database connection
        {
            Cursor.Current = Cursors.WaitCursor;
            this.ToolStripStatusLabel1.Text = string.Empty;
            this.ToolStripStatusLabel3.Text = "Verbinding maken met de database...";
            this.RichTextBoxQueryDescription.Text = string.Empty;
            this.Refresh();

            this.TreeViewExecuteQueries.AfterSelect -= new TreeViewEventHandler(this.TreeViewExecuteQueries_AfterSelect); // Is triggerd when the querys are loaded into the treeview.

            // Close an open Oracle connection.
            if (this.OraConn != null)
            {
                this.CloseOracleConnection();
                this.RemoveVisibleDataWhenConnectionIsLost();   // Remove old values from the last connection
            }

            ToolStripItem tsi = (ToolStripItem)sender;      // Cast sender to ToolStripItem
            this.ConnectToSelectedOraConnmenuItem(tsi);     // Connect the Orqacle connection a tag to the selected menu item.

            this.TreeViewExecuteQueries.AfterSelect += new TreeViewEventHandler(this.TreeViewExecuteQueries_AfterSelect); // Is triggerd when the querys are loaded into the treeview.
        }

        private void ConnectToSelectedOraConnmenuItem(ToolStripItem tsi)
        {
            // connect the Oracle connection a tag to the selected menu item.
            TdOracleConnection tdOraCon = (TdOracleConnection)tsi.Tag;

            foreach (ToolStripItem tsiTagIsOraConn in this.ToolStripMenuItemDatabaseConnection.DropDownItems)
            {
                // Check for RunTimeAdded MenuItem
                if (!(tsiTagIsOraConn.Tag == null))
                {
                    if (tsiTagIsOraConn.Tag.GetType() == typeof(TdOracleConnection))
                    {
                        // Set Menuitem Text on original name
                        TdOracleConnection oConn = (TdOracleConnection)tsiTagIsOraConn.Tag;
                        tsiTagIsOraConn.Text = oConn.Name;
                    }
                }
            }

            this.TreeViewExecuteQueries.Tag = tdOraCon; // connect the oracle connection to the Treeview. This connection is used in LoadTreeViewQueries().
            this.OraConnectionName = tdOraCon.Name;

            // Database log in
            TdOraConnection oraConnSelected = new ();

            bool hasConnection = false;

            try
            {
                if (!hasConnection)
                {
                    using TdSecurityExtensions securityExt = new();
                    string unsecurePwd = securityExt.UnSecureString(tdOraCon.Password);

                    if (!string.IsNullOrEmpty(tdOraCon.Schema) &&
                        !string.IsNullOrEmpty(unsecurePwd) &&
                        !string.IsNullOrEmpty(tdOraCon.Connection))
                    {
                        unsecurePwd = string.Empty;
                        this.ToolStripStatusLabel3.Text = "Verbinding maken met de database...";
                        this.Refresh();

                        if (oraConnSelected.OraConnect(tdOraCon.Schema, tdOraCon.Password, tdOraCon.Connection))
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            TdUserData.ConnectionId = tdOraCon.Id;
                            this.OraConn = oraConnSelected.OraConn;
                            this.OraSchema = tdOraCon.Schema;

                            if (this.OraConn.State == ConnectionState.Closed)
                            {
                                this.ToolStripStatusLabel3.Text = "Database = Geen verbinding";
                                this.ToolStripStatusLabel1.Text = "Kan niet inloggen op database";
                                this.TreeViewExecuteQueries.Nodes.Clear();
                                TdUserData.ConnectionId = -1;
                            }
                            else
                            {
                                this.LoadTreeViewQueries();
                                this.LoadTreeviewState();

                                this.LoadQueryGroupNames();

                                this.ToolStripStatusLabel3.Text = "Database = " + tdOraCon.Schema + "@" + tdOraCon.Connection + "          ";
                                this.ToolStripStatusLabel1.Text = "Inloggen op database succesvol";
                                this.ToolStripMenuItemDatabaseConnection.Enabled = true;

                                TdUserData.ConnectionName = tdOraCon.Name;
                                TdUserData.ConnectionId = tdOraCon.Id;

                                if (this.TreeViewExecuteQueries.GetNodeCount(true) >= 1)
                                {
                                    this.ButtonRunQuery.Enabled = true;  // Only enabled when there are querys to execute
                                }
                                else
                                {
                                    this.ButtonRunQuery.Enabled = false;
                                }

                                this.ToolStripStatusLabel1.Text = string.Empty;

                                this.GetDecimalSettingsOraSchema(); // Get the decimal settings. Needed to filter on floats
                                this.ClearAllFilters();
                            }

                            hasConnection = true;
                        }
                        else
                        {
                            this.ToolStripStatusLabel1.Text = string.Empty;
                            this.ToolStripStatusLabel3.Text = "Database = Geen verbinding";
                        }
                    }
                    else
                    {
                        this.ToolStripStatusLabel3.Text = "Database = Geen verbinding";
                    }
                }
                else
                {
                    this.ToolStripStatusLabel3.Text = "Database = Geen verbinding";
                }
            }
            catch (OracleException ex)
            {
                TdLogging.WriteToLogError("Het maken van een connectie met Oracle is mislukt.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                this.ToolStripStatusLabel3.Text = "Database = Geen verbinding";
                this.ToolStripStatusLabel1.Text = "Het maken van een connectie met Oracle is mislukt.";
            }
            finally
            {
                this.Enablefunctions();
                this.MarkOracleConnectionInMenu();
                this.Refresh();
                Cursor.Current = Cursors.Default;
            }
        }

        private void MarkOracleConnectionInMenu()
        {
            if (this.OraConn != null)
            {
                if (this.ToolStripMenuItemDatabaseConnection.DropDownItems != null)
                {
                    foreach (ToolStripMenuItem dropDownItem in this.ToolStripMenuItemDatabaseConnection.DropDownItems)
                    {
                        if (dropDownItem.Text == this.OraConnectionName)
                        {
                            dropDownItem.Text = "<< " + this.OraConnectionName + " >>"; // TODO; moet een check vinkje worden, en iers andere kleur
                        }
                    }
                }
            }
            else
            {
                if (this.ToolStripMenuItemDatabaseConnection.DropDownItems != null)
                {
                    foreach (ToolStripMenuItem dropDownItem in this.ToolStripMenuItemDatabaseConnection.DropDownItems)
                    {
                        dropDownItem.Text = dropDownItem.Text.Replace("<< ", string.Empty);
                        dropDownItem.Text = dropDownItem.Text.Replace(" >>", string.Empty);
                    }
                }
            }
        }

        private void GetDecimalSettingsOraSchema()
        {
            // Get the decimal settings. Needid to filter on floats
            try
            {
                if (this.OraConn.State == ConnectionState.Open)
                {
                    OracleCommand cmd = new ()
                    {
                        Connection = this.OraConn,
                        CommandText = "select value from nls_session_parameters where parameter = 'NLS_NUMERIC_CHARACTERS'",
                        CommandType = CommandType.Text,
                    };

                    OracleDataReader dr = cmd.ExecuteReader();

                    dr.Read();
                    this.DecimalSeperator = dr.GetString(0);
                    this.DecimalSeperator = this.DecimalSeperator.Substring(1, 1);

                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(string.Empty);
                        TdLogging.WriteToLogDebug("DecimalSeperator Oracle  : " + this.DecimalSeperator);
                        TdLogging.WriteToLogDebug("DecimalSeperator Windows : " + Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, CultureInfo.InvariantCulture));
                        TdLogging.WriteToLogDebug(string.Empty);
                    }

                    dr.Dispose();
                    cmd.Dispose();
                }
                else
                {
                    TdLogging.WriteToLogWarning("De database connectie is gesloten waardoor het ophalen van de decimaal notatie niet is uitgevoerd.");
                }
            }
            catch (AccessViolationException ex)
            {
                TdLogging.WriteToLogError("Het bepalen van het Windows decimaal teken is mislukt. (AccessViolationException)");
                TdLogging.WriteToLogError("Melding : ");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                MessageBox.Show(
                    "Onverwacte fout opgetreden bij het ophalen van Oracle data." + Environment.NewLine +
                    "Controleer het log bestand.", "Let op.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (OracleException ex)
            {
                TdLogging.WriteToLogError("Het bepalen van het Windows decimaal teken is mislukt. (OracleException)");
                TdLogging.WriteToLogError("Melding : ");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                MessageBox.Show(
                    "Onverwacte fout opgetreden bij het ophalen van Oracle data." + Environment.NewLine +
                    "Controleer het log bestand.", "Let op.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion Get Oracle connections

        #region Filter

        /// <summary>
        /// If i filter is active, remove it.
        /// </summary>
        public void ClearAllFilters()
        {
            if (this.DatatabelExport != null)
            {
                this.DatatabelExport.DefaultView.RowFilter = string.Empty;
            }

            this.ButtonRemoveSelection.Visible = false;
            this.DataTableIsFilterd = false;
            this.FilterIsActive = false;
            this.Filters.CheckedFilterItem.Clear();  // Clear the filters

            this.filterFormHasBeenOpenend = 0;
        }
        #endregion Filter

        #region Load the queries into the treeview

        private void LoadTreeViewQueries()
        {
            try
            {
                Stopwatch stopWatch = new ();
                stopWatch.Start();

                this.ToolStripStatusLabel1.Text = "Ophalen query's...";
                this.Refresh();

                TdLogging.WriteToLogInformation("Ophalen query's.");

                this.TreeViewExecuteQueries.BeginUpdate();
                this.TreeViewExecuteQueries.Nodes.Clear();

                // First get all query's connect to the selected database connection.
                if (this.SQLLiteApp != null)
                {
                    TdOracleConnection tdOraConn = (TdOracleConnection)this.TreeViewExecuteQueries.Tag;
                    if (tdOraConn != null)
                    {
                        TdQueries tdQueries = this.SQLLiteApp.SelectRelatedQueries(tdOraConn.Id);
                        List<string> folderGuids = new ();

                        foreach (TdQuery appQ in tdQueries.Items)
                        {
                            // Get the highest parent
                            if (!string.IsNullOrEmpty(appQ.Folder))
                            {
                                string mainParent = this.GetMainParentFolder(appQ.Folder);
                                folderGuids.Add(mainParent);
                            }
                        }

                        /*
                        Parallel.ForEach(_queries.Items, (TopDataQuery _AppQ) =>
                        {
                            if (!string.IsNullOrEmpty(_AppQ.Folder))
                            {
                                string mainParent = GetMainParentFolder(_AppQ.Folder);
                                folderGuids.Add(mainParent);
                            }
                        });*/

                        string whereClause = string.Empty;
                        if (folderGuids.Count > 0)
                        {
                            foreach (string guid in folderGuids)
                            {
                                if (string.IsNullOrEmpty(whereClause))
                                {
                                    whereClause = "WHERE GUID IN ('" + guid + "'";
                                }
                                else
                                {
                                    whereClause += ",'" + guid + "'";
                                }
                            }

                            /*
                            Parallel.ForEach(folderGuids, (string guid) =>
                            {
                                if (string.IsNullOrEmpty(WhereClause))
                                {
                                    WhereClause = "WHERE GUID IN ('" + guid + "'";
                                }
                                else
                                {
                                    WhereClause += ",'" + guid + "'";
                                }
                            });*/

                            whereClause += ")";
                            TreeNode rootNode = this.TreeViewExecuteQueries.Nodes.Add(TdSettingsDefault.ApplicationName, TdSettingsDefault.ApplicationName, 0, 0);

                            TdFolders tdFolders = this.SQLLiteApp.GetFolders(whereClause);
                            foreach (TdFolder appQ in tdFolders.Items)
                            {
                                if (appQ.FolderType == FolderTypes.Folder)
                                {
                                    TreeNode node = rootNode.Nodes.Add(appQ.Name, appQ.Name, 1, 2);

                                    // node.BackColor = Color.Wheat;// .WhiteSmoke; // TODO; could be optional
                                    node.Tag = appQ;

                                    // Read child folders
                                    this.LoadTreeViewFoldersChilds(node, tdOraConn.Id);
                                }
                                else if (appQ.FolderType == FolderTypes.Query)
                                {
                                    TreeNode node = rootNode.Nodes.Add(appQ.Name, appQ.Name, 3, 3);
                                    node.Tag = appQ;
                                }
                                else
                                {
                                    // never gets here
                                    TreeNode node = rootNode.Nodes.Add(appQ.Name, appQ.Name, 3, 3);
                                    node.Tag = appQ;
                                }
                            }
                        }

                        this.TreeViewExecuteQueries.SelectedNode = this.TreeViewExecuteQueries.TopNode;
                    }
                }

                // Add autocomplete functionality to the search textboxes
                this.TreeViewExecuteQueries.EndUpdate();

                using (TdAutoComplete aCompleteSource = new ())
                {
                    AutoCompleteStringCollection dataCollection;
                    dataCollection = aCompleteSource.CreAutoCompleteListFromTrv(this.TreeViewExecuteQueries);  // Create the autocomplete list for the search box

                    this.TextBoxSearchInQueryTreeView.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.TextBoxSearchInQueryTreeView.AutoCompleteCustomSource = dataCollection;
                }

                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0:00}:{1:00}:{2:00}.{3:00} (uur:min:sec.msec).",
                    ts.Hours,
                    ts.Minutes,
                    ts.Seconds,
                    ts.Milliseconds / 10);

                TdLogging.WriteToLogInformation(@"Het ophalen van de query's is gereed.");
                TdLogging.WriteToLogInformation(@"Het ophalen van de query's duurde : " + elapsedTime);
                TdLogging.WriteToLogInformation(string.Empty);

                this.ToolStripStatusLabel1.Text = "Ophalen query's is gereed.";
                this.Refresh();
            }
            catch (Exception ex)
            {
                TdLogging.WriteToLogError("Onverwachte fout bij het laden van de query's.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogError(ex.ToString());
                }
            }
        }

        private void LoadQueryGroupNames()
        {
            this.ComboBoxQueryGroup.Items.Clear();
            this.ComboBoxQueryGroup.Text = string.Empty;

            TdMaintainQueryGroups qGroup = new ();
            qGroup.GetAllQueryGroupNames();

            foreach (KeyValuePair<int, string> entry in qGroup.Querygroups)
            {
                this.ComboBoxQueryGroup.Items.Add(entry.Value); // Do something with entry.Value or entry.Key
            }

            this.ComboBoxQueryGroup.Items.Add(string.Empty); // Add one emtpy line.
        }

        private void LoadTreeViewFoldersChilds(TreeNode parentNode, int connId) // Connection id is needed to filter the query's with the right connection id
        {
            TdFolder parentFolder = (TdFolder)parentNode.Tag;

            TdFolders tdFolders = this.SQLLiteApp.GetFolders("WHERE PARENT = '" + parentFolder.FolderGuid + "'");
            foreach (TdFolder appQ in tdFolders.Items)
            {
                if (appQ.FolderType == FolderTypes.Folder)
                {
                    TreeNode node = parentNode.Nodes.Add(appQ.Name, appQ.Name, 1, 2);
                    node.Tag = appQ;

                    // Read child folders
                    this.LoadTreeViewFoldersChilds(node, connId);
                }
                else if (appQ.FolderType == FolderTypes.Query)
                {
                    TreeNode node = parentNode.Nodes.Add(appQ.Name, appQ.Name, 3, 3);
                    node.Tag = appQ;
                }
                else
                {
                    // nerver gets here
                    TreeNode node = parentNode.Nodes.Add(appQ.Name, appQ.Name, 3, 3);
                    node.Tag = appQ;
                }
            }

            TdQueries tdQueries = this.SQLLiteApp.GetQueries(string.Format("AND FOLDER = '" + parentFolder.FolderGuid + "' AND ID IN (SELECT QUERY_ID FROM {0} WHERE CONNECTION_ID = " + connId + ")", TdTableName.CONNECTION_QUERY));

            foreach (TdQuery tdQ in tdQueries.Items)
            {
                // TreeNode _node = parentNode.Nodes.Add(AppQ.Name, tdQ.Name, 3, 3);      //sql icon
                TreeNode node = parentNode.Nodes.Add(tdQ.Name, tdQ.Name, 10, 10);   // if the index number is higher then available indexes then no icon will show in the treeview
                node.Tag = tdQ;
            }
        }

        private string GetMainParentFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return string.Empty;
            }

            string lastParent = folder;
            string parent;
            do
            {
                try
                {
                    DataRow dRow = this.sqliteApp.SelectedRowFromQuery(string.Format("SELECT PARENT FROM {0} WHERE GUID = '" + lastParent + "'", TdTableName.FOLDER_LIST));
                    parent = dRow["PARENT"].ToString();
                }
                catch (NullReferenceException ex)
                {
                    parent = string.Empty;
                    TdLogging.WriteToLogError("Fout bij het ophalen van de parent folder.");
                    TdLogging.WriteToLogError(string.Format("Query:  SELECT PARENT FROM {0} WHERE GUID = '" + lastParent + "'", TdTableName.FOLDER_LIST));
                    TdLogging.WriteToLogError("Melding:");
                    TdLogging.WriteToLogError(ex.Message);
                }

                if (!string.IsNullOrEmpty(parent))
                {
                    lastParent = parent;
                }
            }
            while (!string.IsNullOrEmpty(parent));

            return lastParent;
        }
        #endregion Load the queries into the treeview

        private void button1_Click(object sender, EventArgs e)
        {
            // eenmalig nodig. in menu zetten met alleen owner/system rechten. hoeft maar 1 keer een xml te maken. dit bestand moet mee geleverd worden
            KeyContainer kc = new ();
            kc.GetRSAContainer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KeyContainer kc = new ();
            kc.GetKeyContainerSaveToFile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            KeyContainer kc = new ();
            kc.GetXmlCreateKeyContainer();
        }

        #region Treeview state
        private void SaveTreeviewState()
        {
            if (this.OraConn != null)
            {
                if (!string.IsNullOrEmpty(TdUserData.UserName) &&
                !string.IsNullOrEmpty(TdUserData.UserId) &&
                TdUserData.ConnectionId > -1)
                {
                    this.savedExpansionState = this.TreeViewExecuteQueries.Nodes.GetExpansionState();
                    TdTreeViewExtensions.SaveTreeviewState(this.savedExpansionState, TdUserData.UserName, int.Parse(TdUserData.UserId, CultureInfo.InvariantCulture), this.TreeViewExecuteQueries.Name, TdUserData.ConnectionId);
                }
                else
                {
                    TdLogging.WriteToLogWarning("Fout opgetreden bij het opslaan van de treeview status. Één van de variabelen is leeg.");

                    TdLogging.WriteToLogWarning("UserName = " + TdUserData.UserName);
                    TdLogging.WriteToLogWarning("UserId = " + TdUserData.UserId);
                    TdLogging.WriteToLogWarning("ConnectionId = " + TdUserData.ConnectionId.ToString());
                }
            }
        }

        private void LoadTreeviewState()
        {
            if (this.OraConn != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(TdUserData.UserName) ||
                            !string.IsNullOrEmpty(TdUserData.UserId) ||
                            !string.IsNullOrEmpty(Convert.ToString(TdUserData.ConnectionId, CultureInfo.InvariantCulture)))
                    {
                        TdTreeViewExtensions.ReadtreeviewState(this.TreeViewExecuteQueries, TdUserData.UserName, int.Parse(TdUserData.UserId, CultureInfo.InvariantCulture), TdUserData.ConnectionId);

                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug(string.Empty);
                            TdLogging.WriteToLogDebug("UserName : " + TdUserData.UserName);
                            TdLogging.WriteToLogDebug("UserId : " + TdUserData.UserId);
                            TdLogging.WriteToLogDebug("SqlLiteDatabaseName : " + TdSettingsDefault.SqlLiteDatabaseName);
                            TdLogging.WriteToLogDebug("ConnectionId : " + TdUserData.ConnectionId.ToString(CultureInfo.InvariantCulture));
                            TdLogging.WriteToLogDebug(string.Empty);
                        }
                    }
                    else
                    {
                        TdLogging.WriteToLogWarning("Fout opgetreden bij het laden van de treeview state. Één van de variabelen is leeg.");

                        TdLogging.WriteToLogWarning("UserName = " + TdUserData.UserName);
                        TdLogging.WriteToLogWarning("UserId = " + TdUserData.UserId);
                        TdLogging.WriteToLogWarning("SqlLiteDatabaseName = " + TdSettingsDefault.SqlLiteDatabaseName);
                    }
                }
                catch (Exception ex)
                {
                    TdLogging.WriteToLogError("Onverwachte fout bij het laden van de treeview status.");
                    TdLogging.WriteToLogError("Melding : ");
                    TdLogging.WriteToLogError(ex.Message);
                }
            }
        }
        #endregion Treeview state

        private void ToolStripMenuItemDatabaseDisconnect_Click(object sender, EventArgs e)
        {
            if (this.OraConn.State == ConnectionState.Open)
            {
                this.SaveTreeviewState();
                this.OraConn.Close();
                this.OraConn = null;
                this.OraConnectionName = string.Empty;
                TdUserData.ConnectionId = -1;
                TdUserData.ConnectionName = string.Empty;

                this.RemoveVisibleDataWhenConnectionIsLost();
                this.MarkOracleConnectionInMenu();

                this.Enablefunctions();
            }
        }

        // not used yet.
        private BindingNavigator bn;

        /// <summary>
        /// Gets or sets binding bnavigator. Not used yet. (Is not available in .net 5 and has to be build programmatically).
        /// </summary>
        public BindingNavigator Bn
        {
            get { return this.bn; }
            set { this.bn = value; }
        }

        private void CreBindingNavigator()
        {
            // BindingNavigator is not available in .net toolbox (yet). 09-04-2021
            this.Bn = new ();
            this.Bn.Name = "BindingNavigatorQueryResultDgv";
            this.Bn.Text = "BindingNavigatorQueryResultDgv";
            this.Bn.AutoSize = true;
            this.Bn.Location = new Point(this.DataGridViewQueries.Location.X, this.DataGridViewQueries.Location.Y);
            this.Bn.Size = new Size(286, 25);
            this.Bn.Visible = true;

            // knoppen ????
            this.PlaceControlOnSpltcontainerTwoPanel(this.Bn, 0);
        }

        private void PlaceControlOnSpltcontainerTwoPanel(Control newCntrl, int tabCount)
        {
            TabPage tp = this.TabControl1.TabPages[tabCount];
            foreach (Control c in tp.Controls)
            {
                if (c.GetType() == typeof(SplitContainer))
                {
                    SplitContainer splt1 = (SplitContainer)c;
                    splt1.Panel2.Controls.Add(newCntrl);
                }
            }
        }

        private void DataGridViewQueries_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            this.DgvCelFormatting(this.DataGridViewQueries);
        }

        private void DgvCelFormatting(DataGridView dgv)
        {
            if (dgv == null)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            dgv.SuspendLayout();

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                // TODO; test if switch is faster with a large query result.
                string valueType = column.ValueType.ToString();
                string header = column.Name;
                if (valueType == "System.Decimal" || valueType == "System.Double")
                {
                    dgv.Columns[header].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (valueType == "System.Double")
                {
                    dgv.Columns[header].DefaultCellStyle.Format = "0.00##";  // Round to 2 decimals
                    dgv.Columns[header].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (valueType == "System.Int16")
                {
                    dgv.Columns[header].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (valueType == "System.Int64")
                {
                    dgv.Columns[header].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            dgv.ResumeLayout();
            Cursor.Current = Cursors.Default;
        }

        #region Execute queries
        private void ButtonRunQuery_Click(object sender, EventArgs e)
        {
            this.ToolStripStatusLabel1.Text = string.Empty;
            this.Refresh();
            this.SaveSettings();  // Settings parameter form position can change during the query execution. first save the settings.
            this.ExecuteSingleQuery();
            this.Dgv = this.DataGridViewQueries;  // this.Dgv holds the active datagridview. (Needed when there are more datagridviews on the form).
            this.GetSettings();  // Settings parameter form position can change during the query execution. load the settings again.
        }

        private void ExecuteSingleQuery()
        {
            if (this.QueryId > -1 && this.QueryGroupId > -1 && !string.IsNullOrEmpty(this.QueryGuid))
            {
                using TdExecuteQueries queryExec = new (this.OraConn);
                if (!TdExecuteQueries.IsQueryActive(this.QueryId, this.QueryGroupId, this.QueryGuid))
                {
                    this.ClearAllFilters();

                    this.SdoGeometryColumnVisible = this.CheckBoxShowGeometryField.Checked;  // Show or hide the geometry column.
                    this.IndexSdoGeometryColumn = -1;

                    queryExec.Parent = this;
                    queryExec.SdoGeometryColumnVisible = this.SdoGeometryColumnVisible;
                    queryExec.IndexSdoGeometryColumn = this.IndexSdoGeometryColumn;
                    queryExec.Dgv = this.DataGridViewQueries;

                    queryExec.TrvNode = this.TreeViewExecuteQueries.SelectedNode as TreeNode;

                    // De-activate cellformating
                    this.DataGridViewQueries.CellFormatting -= new DataGridViewCellFormattingEventHandler(this.DataGridViewQueries_CellFormatting);

                    queryExec.StartExecuteQuery();
                    this.CheckShowGeomCheckBox();

                    // Remove the query from the table query_is_active
                    queryExec.RemoveQueryIsActive(this.QueryId, this.QueryGroupId, this.QueryGuid);

                    // Activate cellformating
                    this.DataGridViewQueries.CellFormatting += new DataGridViewCellFormattingEventHandler(this.DataGridViewQueries_CellFormatting);
                }
                else
                {
                    TdLogging.WriteToLogInformation("De query wordt al door iemand anders uitgevoerd.");
                    MessageBox.Show("De query wordt al uitgevoerd door iemand anders." + Environment.NewLine + "Probeer het over enkele minuten opnieuw.", "Waarschuwing.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void CheckShowGeomCheckBox()
        {
            // If checked than the sdo_geometry field will be shown in the datagridview and will be exported.
            try
            {
                if (this.SdoGeometryColumnVisible)
                {
                    if (this.DataGridViewQueries.Columns.Count > 0)
                    {
                        if (this.IndexSdoGeometryColumn != -1)
                        {
                            this.DataGridViewQueries.Columns[this.IndexSdoGeometryColumn].Visible = true;
                        }
                    }
                }
                else
                {
                    if (this.DataGridViewQueries.Columns.Count > 0)
                    {
                        if (this.IndexSdoGeometryColumn != -1)
                        {
                            this.DataGridViewQueries.Columns[this.IndexSdoGeometryColumn].Visible = false;
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                TdLogging.WriteToLogError("Fout bij het bepalen of de geometrie kolom moet worden getoond. (Index out of range).");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
            }
            catch (Exception ex)
            {
                TdLogging.WriteToLogError("Fout bij het bepalen of de geometrie kolom moet worden getoond.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                TdLogging.WriteToLogError(string.Empty);
                TdLogging.WriteToLogError("DataGridViewQueries.Columns.Count : " + this.DataGridViewQueries.Columns.Count.ToString(CultureInfo.InvariantCulture));
                TdLogging.WriteToLogError("IndexSdoGeometryColumn : " + this.IndexSdoGeometryColumn.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void TreeViewExecuteQueries_DoubleClick(object sender, EventArgs e)
        {
            // Avoid accidental double click when checking tho node
            var localPosition = this.TreeViewExecuteQueries.PointToClient(Cursor.Position);
            var hitTestInfo = this.TreeViewExecuteQueries.HitTest(localPosition);
            if (hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
            {
                return;
            }

            if (this.OraConn != null)
            {
                // Get the checked nodes.
                List<TreeNode> checked_nodes = this.CheckedNodesList(this.TreeViewExecuteQueries);

                // Check if list is filled.
                bool isEmpty = !checked_nodes.Any();
                if (isEmpty)
                {
                    this.ButtonRunQueryAndExport.Enabled = false;
                    this.ButtonRunQuery.Enabled = true;
                    this.ButtonExport.Enabled = true;

                    this.ExecuteSingleQuery();
                    this.Dgv = this.DataGridViewQueries;
                }
                else
                {
                    this.ButtonRunQueryAndExport.Enabled = true;
                    this.ButtonRunQuery.Enabled = false;
                    this.ButtonExport.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Er is geen database connectie.", "Fout.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonRunQueryAndExport_Click(object sender, EventArgs e)
        {
            this.Uitvoeren = false;
            this.DataGridViewQueries.CellFormatting -= new DataGridViewCellFormattingEventHandler(this.DataGridViewQueries_CellFormatting); // de-activate cellformating

            this.ToolStripStatusLabel1.Text = string.Empty;

            bool checkState = this.CheckBoxShowGeometryField.Checked;    // Get the current checkbox state
            bool enabledState = this.CheckBoxShowGeometryField.Enabled;  // Get the current checkbox state

            this.ClearAllFilters();

            try
            {
                this.DataGridViewQueries.DataSource = null;  // Clear the datagrid when starting the queries
                this.Refresh();

                TreeNode topNode = this.TreeViewExecuteQueries.SelectedNode;
                while (topNode.Parent != null)
                {
                    topNode = topNode.Parent;  // Get the top node
                }

                using (this.QueryExec)
                {
                    this.QueryExec.Parent = this;
                    this.QueryExec.SdoGeometryColumnVisible = this.SdoGeometryColumnVisible;
                    this.QueryExec.SdoGeometryColumnVisibleMultipleQueries = this.SdoGeometryColumnVisible;
                    this.QueryExec.IndexSdoGeometryColumn = this.IndexSdoGeometryColumn;
                    this.QueryExec.Dgv = this.DataGridViewQueries;

                    try
                    {
                        this.pathFileName.Clear();

                        this.ExecuteSelectedQueries(topNode);
                        this.Dgv = this.DataGridViewQueries;

                        this.CheckShowGeomCheckBox();

                        this.ToolStripStatusLabel1.Text = string.Empty;
                        this.Refresh();

                        MessageBox.Show("Het uitvoeren van meerdere query's is gereed.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        TdLogging.WriteToLogError("Onverwachte fout bij het uitvoeren van meerdere query's achter elkaar.");
                        TdLogging.WriteToLogError("Melding:");
                        TdLogging.WriteToLogError(ex.Message);

                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug(ex.ToString());
                        }

                        this.DataGridViewQueries.CellFormatting += new DataGridViewCellFormattingEventHandler(this.DataGridViewQueries_CellFormatting); // Activate cellformating
                    }
                }

                this.OverWriteFile = false;
            }
            catch (Exception ex)
            {
                TdLogging.WriteToLogError("Het uitvoeren van meerdere query's achter elkaar is mislukt.");
                TdLogging.WriteToLogError("Melding :");
                TdLogging.WriteToLogError(ex.Message);
                this.CheckBoxShowGeometryField.Checked = checkState;     // Set the org. checkbox state back
                this.CheckBoxShowGeometryField.Enabled = enabledState;   // Set the org. checkbox state back

                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }
            }
            finally
            {
                this.CheckBoxShowGeometryField.Checked = checkState;     // Set the org. checkbox state back
                this.CheckBoxShowGeometryField.Enabled = enabledState;   // Set the org. checkbox state back

                this.OverRuleExcelFilePath = string.Empty;
                this.DataGridViewQueries.CellFormatting += new DataGridViewCellFormattingEventHandler(this.DataGridViewQueries_CellFormatting);
            }
        }

        /// <summary>
        /// Execute all the selected queries.
        /// TODO; this method must be reviewd. it is to large.
        /// </summary>
        /// <param name="node">Selected tree node(s).</param>
        public void ExecuteSelectedQueries(TreeNode node)
        {
            if (node == null)
            {
                return;
            }

            foreach (TreeNode tn in node.Nodes)
            {
                if (tn.Checked == true)
                {
                    if (tn.Tag is TdQuery tdQ)
                    {
                        this.DoNotSaveFile = false;

                        // If the query has an export file name then it will be saved
                        if (this.QueryGroupIsActive)
                        {
                            // A querygroup can have it's own filename.
                            tdQ.FileNameOutput = this.GetQueryGroupPathFileName();
                            TdLogging.WriteToLogDebug("FileName output : " + tdQ.FileNameOutput);
                            this.pathFileName.Add(this.GetQueryGroupPathFileName());  // Get the filename of te query group
                            TdLogging.WriteToLogDebug("GetQueryGroupPathFileName : " + this.GetQueryGroupPathFileName());
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.OverRuleExcelFilePath))
                            {
                                tdQ.FileNameOutput = this.OverRuleExcelFilePath;
                            }

                            if (!string.IsNullOrEmpty(tdQ.FileNameOutput))
                            {
                                if (string.IsNullOrEmpty(this.OverRuleExcelFilePath))
                                {
                                    if (!this.OverWriteFile && !this.Uitvoeren)
                                    {
                                        // Check if filename exists...
                                        if (File.Exists(tdQ.FileNameOutput))
                                        {
                                            DialogResult dialogResult = MessageBox.Show(
                                                @"Het bestand """ + tdQ.FileNameOutput + @""" bestaat al." + Environment.NewLine +
                                                Environment.NewLine +
                                                "Wilt u het overschijven?", "Opslaan bestand.",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

                                            if (dialogResult == DialogResult.Yes)
                                            {
                                                this.pathFileName.Add(tdQ.FileNameOutput);  // Get the path and filename of the Excel file name of the query
                                                this.OverRuleExcelFilePath = string.Empty;
                                                this.OverWriteFile = true;
                                            }
                                            else if (dialogResult == DialogResult.No)
                                            {
                                                SaveFileDialog saveFileDialog1 = new()
                                                {
                                                    Filter = "Excel|*.xlsx",
                                                    Title = "Opslaan als...",
                                                };
                                                saveFileDialog1.ShowDialog();
                                                if (!string.IsNullOrEmpty(saveFileDialog1.FileName))
                                                {
                                                    this.OverRuleExcelFilePath = saveFileDialog1.FileName;
                                                    this.pathFileName.Add(this.OverRuleExcelFilePath);
                                                    this.OverWriteFile = false;
                                                    this.DoNotSaveFile = false;
                                                    tdQ.FileNameOutput = saveFileDialog1.FileName;
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Het bestand is niet opgeslagen.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    this.OverWriteFile = false;
                                                    this.DoNotSaveFile = true;
                                                }

                                                saveFileDialog1.Dispose();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(this.OverRuleExcelFilePath))
                                        {
                                            this.pathFileName.Add(tdQ.FileNameOutput);
                                        }
                                        else
                                        {
                                            this.pathFileName.Add(this.OverRuleExcelFilePath);
                                        }
                                    }
                                }
                                else
                                {
                                    this.pathFileName.Add(this.OverRuleExcelFilePath);
                                }
                            }
                        }

                        bool sdoGeometryColumnVisibleMultipleQueriesOldState = this.SdoGeometryColumnVisibleMultipleQueries;   // TODO can this be improved? necessary because it will be set to false when executing a query which result hasn't a geometry field.

                        this.QueryExec.TrvNode = tn;

                        // de-activate cellformating
                        this.DataGridViewQueries.CellFormatting -= new DataGridViewCellFormattingEventHandler(this.DataGridViewQueries_CellFormatting);

                        this.QueryExec.StartExecuteQuery();

                        this.SdoGeometryColumnVisibleMultipleQueries = sdoGeometryColumnVisibleMultipleQueriesOldState; // Restore the old state after executing a query which has a geometry field
                        this.CheckShowGeomCheckBox();

                        // Activate cellformating
                        this.DataGridViewQueries.CellFormatting += new DataGridViewCellFormattingEventHandler(this.DataGridViewQueries_CellFormatting);

                        if (!string.IsNullOrEmpty(this.OverRuleExcelFilePath))
                        {
                            this.ToolStripStatusLabel1.Text = @"De gegevens van query """ + tn.Text + @""" worden opgeslagen...";
                            this.Refresh();

                            TdLogging.WriteToLogDebug("OverRuleExcelFilePath + " + this.OverRuleExcelFilePath);

                            if (Directory.Exists(Path.GetDirectoryName(this.OverRuleExcelFilePath)))
                            {
                                using (TdDataExport exporteer = new(this.DatatabelExport, false, true, this, this.SdoGeometryColumnVisibleMultipleQueries))
                                {
                                    // Count > distinct count    then there is a least 1 pathname double
                                    if (this.pathFileName.Count != this.pathFileName.Distinct().Count())
                                    {
                                        // Duplicates exist  check if folder+worksheetname exitst
                                        exporteer.ExportData(this.OverRuleExcelFilePath, tdQ.WorksheetName, true);
                                    }
                                    else
                                    {
                                        exporteer.ExportData(this.OverRuleExcelFilePath, tdQ.WorksheetName, false);
                                    }

                                    // this.OverRuleExcelFilePath = string.Empty;
                                }

                                this.ToolStripStatusLabel1.Text = string.Empty;
                                this.Refresh();
                            }
                            else
                            {
                                this.ToolStripStatusLabel1.Text = string.Empty;
                                MessageBox.Show(
                                    "Het pad waarin het bestand wordt opgeslagen bestaat niet." + Environment.NewLine
                                    + Environment.NewLine +
                                    "Pas het pad in aan in menu Beheren query's.",
                                    "Fout.",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                TdLogging.WriteToLogWarning("Het pad waarin het bestand wordt opgeslagen bestaat niet. (multiple Query uitvoer). ");
                                TdLogging.WriteToLogWarning("Het pad waarin de export wordt opgeslagen : " + Path.GetDirectoryName(tdQ.FileNameOutput));
                            }
                        }

                        if (!this.DoNotSaveFile)
                        {
                            if (!string.IsNullOrEmpty(tdQ.FileNameOutput))
                            {
                                this.ToolStripStatusLabel1.Text = @"De gegevens van query """ + tn.Text + @""" worden opgeslagen...";
                                this.Refresh();

                                if (!this.Uitvoeren)
                                {
                                    this.Uitvoeren = true;
                                    if (Directory.Exists(Path.GetDirectoryName(tdQ.FileNameOutput)))
                                    {
                                        using (TdDataExport exporteer = new(this.DatatabelExport, false, true, this, this.SdoGeometryColumnVisibleMultipleQueries)) // DataGridView1,
                                        {
                                            if (this.pathFileName.Count != this.pathFileName.Distinct().Count())
                                            {
                                                // Count > distinct count    then there is a least 1 pathname double
                                                // Duplicates exist  check if folder+worksheetname exitst
                                                exporteer.ExportData(tdQ.FileNameOutput, tdQ.WorksheetName, true);
                                            }
                                            else
                                            {
                                                exporteer.ExportData(tdQ.FileNameOutput, tdQ.WorksheetName, false);
                                            }
                                        }

                                        this.ToolStripStatusLabel1.Text = string.Empty;
                                        this.Refresh();
                                    }
                                    else
                                    {
                                        this.ToolStripStatusLabel1.Text = string.Empty;
                                        MessageBox.Show(
                                            "Het pad waarin het bestand wordt opgeslagen bestaat niet." + Environment.NewLine
                                            + Environment.NewLine +
                                            "Pas het pad in aan in menu Beheren query's.",
                                            "Fout.",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                                        TdLogging.WriteToLogWarning("Het pad waarin het bestand wordt opgeslagen bestaart niet. (multiple Query uitvoer). ");
                                        TdLogging.WriteToLogWarning("Het pad waarnaar de export wil schrijven is: : " + Path.GetDirectoryName(tdQ.FileNameOutput));
                                    }
                                }
                                else
                                {
                                    using TdDataExport exporteer = new(this.DatatabelExport, false, true, this, this.SdoGeometryColumnVisibleMultipleQueries);

                                    if (this.pathFileName.Count != this.pathFileName.Distinct().Count())
                                    {
                                        exporteer.ExportData(tdQ.FileNameOutput, tdQ.WorksheetName, true);
                                    }
                                    else
                                    {
                                        exporteer.ExportData(tdQ.FileNameOutput, tdQ.WorksheetName, false);
                                    }
                                }
                            }
                            else
                            {
                                // export without file
                            }
                        }

                        this.ToolStripStatusLabel1.Text = string.Empty;
                        this.Refresh();
                    }
                }

                this.OverWriteFile = false;
                this.Uitvoeren = false;
                this.ExecuteSelectedQueries(tn);
            }
        }

        private string GetQueryGroupPathFileName()
        {
            TdMaintainQueryGroups qGroup = new ();
            string filename = qGroup.GetQueryGroupFileName(this.ComboBoxQueryGroup.Text);

            if (!string.IsNullOrEmpty(filename))
            {
                return filename;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion Execute queries

        #region Export data
        private void ButtonExport_Click(object sender, EventArgs e)
        {
            this.ExportData();
        }

        private void ExportData()
        {
            Cursor.Current = Cursors.WaitCursor;

            this.ToolStripStatusLabel1.Text = string.Empty;
            this.Refresh();

            if (this.DatatabelExport != null)
            {
                using (TdDataExport exporteer = new (this.DatatabelExport, this.DataTableIsFilterd, false, this, this.SdoGeometryColumnVisible))
                {
                    exporteer.ExportData();
                }

                this.ToolStripStatusLabel1.Text = string.Empty;
                this.Refresh();

                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show(
                    "Voer eerst een Query uit.",
                    "Fout.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        #endregion Export data

        private List<TreeNode> CheckedNodesList(TreeView trv)
        {
            List<TreeNode> checked_nodes = new ();
            this.FindCheckedNodes(checked_nodes, trv.Nodes);

            return checked_nodes;   // Return a list of the checked TreeView nodes.
        }

        private void FindCheckedNodes(List<TreeNode> checked_nodes, TreeNodeCollection nodes)
        {
            if (checked_nodes == null || nodes == null)
            {
                return;
            }

            foreach (TreeNode node in nodes)
            {
                // Add this node.
                if (node.Checked)
                {
                    checked_nodes.Add(node);
                }

                // Check the node's descendants.
                this.FindCheckedNodes(checked_nodes, node.Nodes);
            }
        }

        private void TreeViewExecuteQueries_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.TreeViewExecuteQueries.SelectedNode != null)
            {
                this.TreeViewExecuteQueries.SelectedNode.BackColor = SystemColors.Highlight;
                this.TreeViewExecuteQueries.SelectedNode.ForeColor = Color.White;
                this.previousSelectedNodeTrvQuery = this.TreeViewExecuteQueries.SelectedNode;
            }
        }

        /// <summary>
        /// After selecting a query the query discription is loaded and shown in the form.
        /// The afeter select is also used in Feom maintain queries.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public void TreeViewExecuteQueries_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Show the query description
            if (this.TreeViewExecuteQueries.Nodes.Count > 0)
            {
                // Keep the node selected when the treeview loses focus
                this.TreeViewExecuteQueries.HideSelection = true;
                if (this.previousSelectedNodeTrvQuery != null)
                {
                    this.previousSelectedNodeTrvQuery.BackColor = this.TreeViewExecuteQueries.BackColor;
                    this.previousSelectedNodeTrvQuery.ForeColor = this.TreeViewExecuteQueries.ForeColor;
                }

                this.ShowQueryDiscription(sender, e);
            }
        }

        private void ShowQueryDiscription(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode tn = this.TreeViewExecuteQueries.SelectedNode as TreeNode;
                TdQuery tdQ = (TdQuery)tn.Tag;
                if (tdQ != null)
                {
                    this.RichTextBoxQueryDescription.Text = tdQ.Description;
                    this.RichTextBoxQueryDescription.Text += Environment.NewLine + Environment.NewLine;
                    this.RichTextBoxQueryDescription.Text += "Query afzonderlijk uitvoeren, dan export naar:" + Environment.NewLine;
                    this.RichTextBoxQueryDescription.Text += tdQ.FileNameOutput + Environment.NewLine;
                    this.RichTextBoxQueryDescription.Text += "Werkblad:" + tdQ.WorksheetName + Environment.NewLine;
                    this.RichTextBoxQueryDescription.Text += Environment.NewLine;
                    this.RichTextBoxQueryDescription.Text += "Query groep: " + tdQ.QueryGroup;

                    // RichTextBoxQueryDescription.Text += Environment.NewLine;
                    // RichTextBoxQueryDescription.Text += "Qgroep export: " + AppQ.QueryGroupExportFile;
                    this.QueryId = tdQ.Id;
                    this.QueryGuid = tdQ.QueryGuid;
                    this.QueryGroupId = tdQ.QueryGroupId;
                }
            }
            catch (InvalidCastException)
            {
                // When a folders get selected there will be an error but is not important
                // TODO; check if it a folder or query.
            }
            catch (NullReferenceException)
            {
                // Happens whith "uncheck all nodes
            }
        }

        private void TreeViewExecuteQueries_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Get the checked nodes.
            List<TreeNode> checkedNodes = this.CheckedNodesList(this.TreeViewExecuteQueries);
            this.TreeViewExecuteQueries.SelectedNode = e.Node;

            if (e.Node.Checked)
            {
                e.Node.BackColor = Color.AliceBlue;
            }
            else
            {
                e.Node.BackColor = Color.White;
            }

            // Check if list is filled. If true then enable buttons else diable buttons.
            bool isEmpty = !checkedNodes.Any();
            if (isEmpty)
            {
                this.ButtonRunQueryAndExport.Enabled = false;
                this.ButtonRunQuery.Enabled = true;
                this.ButtonExport.Enabled = true;
            }
            else
            {
                this.ButtonRunQueryAndExport.Enabled = true;
                this.ButtonRunQuery.Enabled = false;
                this.ButtonExport.Enabled = false;
            }

            // (un)Select the child nodes when the parent is (un)checked
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */

                    this.CheckAllChildNodesFromParent(e.Node, e.Node.Checked);
                }

                this.TreeViewExecuteQueries.SelectedNode = e.Node;  // Select the clicked node after the child nodes are checked/unchecked
            }

            Cursor.Current = Cursors.Default;
        }

        private void CheckAllChildNodesFromParent(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodesFromParent(node, nodeChecked);
                }
            }
        }

        #region Query group combobox
        private void ComboBoxQueryGroup_DropDown(object sender, EventArgs e)
        {
            if (this.ComboBoxQueryGroup.Text == "<Query groep>")
            {
                this.ComboBoxQueryGroup.TextChanged -= this.ComboBoxQueryGroup_TextChanged;
                this.ComboBoxQueryGroup.Text = string.Empty;
            }

            this.ComboBoxQueryGroup.TextChanged += this.ComboBoxQueryGroup_TextChanged;
            this.ComboBoxQueryGroup.ForeColor = Color.Black;
        }

        private void ComboBoxQueryGroup_DropDownClosed(object sender, EventArgs e)
        {
            this.ComboBoxQueryGroup.TextChanged -= this.ComboBoxQueryGroup_TextChanged;  // Ff there is an event handler then unregister. if there isn't one then this line does nothing. it is needed because ComboBoxQueryGroup_DropDown add every!! time the handler.
            this.ComboBoxQueryGroup.TextChanged += this.ComboBoxQueryGroup_TextChanged;
        }

        private void ComboBoxQueryGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.TreeViewExecuteQueries.BeginUpdate();

            // Save the cuttent treeview state
            this.SaveTreeviewState();

            this.TreeViewExecuteQueries.AfterCheck -= new TreeViewEventHandler(this.TreeViewExecuteQueries_AfterCheck);
            this.TreeViewExecuteQueries.AfterSelect -= new TreeViewEventHandler(this.TreeViewExecuteQueries_AfterSelect);

            TreeNodeCollection nodes = this.TreeViewExecuteQueries.Nodes;
            foreach (TreeNode n in nodes)
            {
                this.GetQueryGroup(n);
            }

            bool executeallQueries = false;

            foreach (TreeNode tn in nodes)
            {
                if (tn.Checked)
                {
                    executeallQueries = true;
                }
            }

            if (executeallQueries)
            {
                this.ButtonRunQueryAndExport.Enabled = false;
                this.ButtonRunQuery.Enabled = true;
                this.ButtonExport.Enabled = true;
            }
            else
            {
                this.ButtonRunQueryAndExport.Enabled = true;
                this.ButtonRunQuery.Enabled = false;
                this.ButtonExport.Enabled = false;
            }

            TreeNodeCollection treeNodes = this.TreeViewExecuteQueries.Nodes;
            this.ExpandTreeNodes(treeNodes);

            this.TreeViewExecuteQueries.AfterCheck += new TreeViewEventHandler(this.TreeViewExecuteQueries_AfterCheck);
            this.TreeViewExecuteQueries.AfterSelect += new TreeViewEventHandler(this.TreeViewExecuteQueries_AfterSelect);

            this.TreeViewExecuteQueries.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        private void ExpandTreeNodes(TreeNodeCollection treeNodes)
        {
            foreach (TreeNode child in treeNodes)
            {
                this.ExpandParent(child);
                this.CheckExpand(child);
            }
        }

        private void ExpandParent(TreeNode treeNode)
        {
            foreach (TreeNode item in treeNode.Nodes)
            {
                if (item.Checked == true)
                {
                    item.Parent.Expand();
                }

                this.ExpandParent(item);
            }
        }

        private void CheckExpand(TreeNode treeNode)
        {
            if (treeNode.Nodes.Count == 0)
            {
                return;
            }
            else
            {
                foreach (TreeNode item in treeNode.Nodes)
                {
                    if (item.IsExpanded == true)
                    {
                        item.Parent.Expand();
                    }

                    this.CheckExpand(item);
                }
            }
        }

        private void GetQueryGroup(TreeNode treeNode)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (treeNode == null)
            {
                return;
            }

            // Remove all checks
            foreach (TreeNode tn in treeNode.Nodes)
            {
                tn.Checked = false;
                tn.BackColor = Color.White;
                tn.Collapse();
            }

            foreach (TreeNode tn in treeNode.Nodes)
            {
                try
                {
                    if (tn.Tag != null)
                    {
                        try
                        {
                            TdQuery tdQ = (TdQuery)tn.Tag;

                            if (tdQ.QueryGroupNames.Count > 0)
                            {
                                foreach (string qrgroupname in tdQ.QueryGroupNames)
                                {
                                    if (qrgroupname == this.ComboBoxQueryGroup.Text)
                                    {
                                        tn.Checked = true;
                                        tn.BackColor = Color.DeepSkyBlue;  // TODO; make color a option/choice
                                        tn.Expand();  // Expand the checked item
                                    }
                                }
                            }
                        }
                        catch (InvalidCastException)
                        {
                            // When a folders get selected there will be an error but its not important
                        }
                    }

                    this.GetQueryGroup(tn);
                }
                catch (Exception ex)
                {
                    TdLogging.WriteToLogError("Onverwachte fout bij het ophalen van de query groepen.");
                    TdLogging.WriteToLogError("Melding:");
                    TdLogging.WriteToLogError(ex.Message);

                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogError(ex.ToString());
                    }
                }
            }
        }

        private void ComboBoxQueryGroup_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ComboBoxQueryGroup.Text) && this.ComboBoxQueryGroup.Text != "<Query groep>")
            {
                this.QueryGroupIsActive = true;
                this.ComboBoxQueryGroup.ForeColor = Color.Black;
            }
            else
            {
                this.QueryGroupIsActive = false;
            }

            if (string.IsNullOrEmpty(this.ComboBoxQueryGroup.Text))
            {
                this.ComboBoxQueryGroup.Text = "<Query groep>";
                this.ComboBoxQueryGroup.ForeColor = Color.Gray;
            }
        }

        #endregion Query group combobox

        private void CheckBoxShowGeometryField_Click(object sender, EventArgs e)
        {
            if (this.IndexSdoGeometryColumn > 0)
            {
                if (this.CheckBoxShowGeometryField.Checked)
                {
                    this.SdoGeometryColumnVisible = true;
                    this.DataGridViewQueries.Columns[this.IndexSdoGeometryColumn].Visible = true;
                }
                else
                {
                    this.SdoGeometryColumnVisible = false;
                    this.DataGridViewQueries.Columns[this.IndexSdoGeometryColumn].Visible = false;
                }
            }

            this.JsonObjSettings.AppParam[0].ShowGeometryField = this.SdoGeometryColumnVisible;
        }

        #region Filter
        private void DataGridViewQueries_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.DataGridViewStartFilter(e, this.DataGridViewQueries);  // Filter the data
                this.Dgv = this.DataGridViewQueries;
            }
        }

        private void DataGridViewStartFilter(DataGridViewCellMouseEventArgs e, DataGridView dgv)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (!this.FilterFormIsOpenend)
            {
                this.DataGridViewFilterd = dgv;

                if (e.Button >= MouseButtons.Right)
                {
                    // Check row id. Only Header may respond to the mouse click
                    if (e.ColumnIndex >= 0 && e.RowIndex == -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        string columnHeaderText = dgv.Columns[e.ColumnIndex].HeaderText;  // Get the Columnname

                        if (this.DatatabelExport.Columns[e.ColumnIndex].DataType.ToString().Contains("SdoGeometry"))
                        {
                            MessageBox.Show("Onbekend datatype aangetroffen.", "Waarschuwing.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            FormFilter frm = new ()
                            {
                                Parent = this,
                                ColumnName = columnHeaderText,
                                DataTypeSelectedColumn = this.DatatabelExport.Columns[e.ColumnIndex].DataType.ToString(),  // Get the datatype of the selected column. (later needed for filtering)
                                FilterFormHasBeenOpenend = this.FilterFormHasBeenOpenend++,
                                StartPosition = FormStartPosition.Manual,
                            };

                            // Resize the form if needed.
                            if ((frm.ButtonInvertSelection.Width + frm.ButtonSelectAll.Width + 25) <= 225)
                            {
                                frm.Size = new Size(frm.ButtonInvertSelection.Width + frm.ButtonSelectAll.Width + 25, 402);
                            }
                            else
                            {
                                frm.Size = new Size(225, 402);
                            }

                            Point c = dgv.PointToScreen(dgv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location);

                            frm.Location = new Point(c.X, c.Y + dgv.GetCellDisplayRectangle(0, -1, false).Height);
                            frm.BringToFront();
                            frm.TopMost = true;  // !
                            frm.XDif = c.X - this.Location.X;
                            frm.YDif = c.Y - this.Location.Y + dgv.GetCellDisplayRectangle(0, -1, false).Height;

                            EnsureVisible(frm);  // Place the filter form always complete on a monitor

                            this.FilterFormIsOpenend = true;

                            this.SaveSettings();  // Needed because when the specific filter form is used the formposition parameters of that form can change en need to be reloaded

                            Cursor.Current = Cursors.Default;
                            frm.ShowDialog(this);
                            frm.Dispose();
                            this.GetSettings();
                        }
                    }
                }
            }
        }

        private static void EnsureVisible(Control ctrl)
        {
            Rectangle screenRect = Screen.GetWorkingArea(ctrl); // The Working Area fo the screen showing most of the Control

            // Now tweak the control Top and Left until it's fully visible.
            ctrl.Left += Math.Min(0, screenRect.Left + screenRect.Width - ctrl.Left - ctrl.Width);
            ctrl.Left -= Math.Min(0, ctrl.Left - screenRect.Left);
            ctrl.Top += Math.Min(0, screenRect.Top + screenRect.Height - ctrl.Top - ctrl.Height);
            ctrl.Top -= Math.Min(0, ctrl.Top - screenRect.Top);
        }

        #endregion Filter

        private void ButtonRemoveSelection_Click(object sender, EventArgs e)
        {
            this.ClearAllFilters();
        }

        #region Treeview search

        private void TextBoxSearchInQueryTreeView_TextChanged(object sender, EventArgs e)
        {
            this.tvSearch.FoundWithTrvSearch = 0;
            TreeNodeCollection nodes = this.TreeViewExecuteQueries.Nodes;
            foreach (TreeNode n in nodes)
            {
                this.tvSearch.ColorTrvSearchNode(n, this.TextBoxSearchInQueryTreeView);
            }

            this.LabelTrvSearchFound.Text = this.tvSearch.FoundWithTrvSearch.ToString(CultureInfo.InvariantCulture) + " st";
        }

        private void TextBoxSearchInQueryTreeView_Leave_1(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void TextBoxSearchInQueryTreeView_Enter_1(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void ButtonSearchQueryName_Click(object sender, EventArgs e)
        {
            this.tvSearch.SearchInTreeViewNodes(this.TreeViewExecuteQueries, this.TextBoxSearchInQueryTreeView.Text);
        }

        private void CreateAutoCompleteForSearchBox()
        {
            using TdAutoComplete aCompleteSource = new();
            AutoCompleteStringCollection dataCollection;
            dataCollection = aCompleteSource.CreAutoCompleteListFromTrv(this.TreeViewExecuteQueries);  // Create the autocomplete list for the search box
            this.TextBoxSearchInQueryTreeView.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.TextBoxSearchInQueryTreeView.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.TextBoxSearchInQueryTreeView.AutoCompleteCustomSource = dataCollection;
        }

        #endregion Treeview search

        #region datagridview color, highlight
        private void DataGridViewQueries_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.JsonObjSettings.AppParam[0].DataGridAlternateRowColor)
            {
                this.DataGridView_cellMouseEnter(e, this.DataGridViewQueries);
            }
        }

        private void DataGridView_cellMouseEnter(DataGridViewCellEventArgs e, DataGridView dgv)
        {
            try
            {
                if (e.RowIndex == this.highlightedRowIndex)
                {
                    return;
                }

                // Unhighlight the previously highlighted row.
                if (this.highlightedRowIndex >= 0)
                {
                    SetRowStyle(dgv.Rows[this.highlightedRowIndex], null);
                }

                // Highlight the row holding the mouse.
                this.highlightedRowIndex = e.RowIndex;
                if (this.highlightedRowIndex >= 0)
                {
                    SetRowStyle(dgv.Rows[this.highlightedRowIndex], this.highlightStyle);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                TdLogging.WriteToLogError("Argument Out Of Range Exception bij datagridview cell mouse enter.");
            }
            catch (IndexOutOfRangeException)
            {
                TdLogging.WriteToLogError("Index Out Of Range Exception bij datagridview cell mouse enter.");
            }
            catch (Exception)
            {
                TdLogging.WriteToLogError("Exception bij datagridview cell mouse enter.");
            }
        }

        private static void SetRowStyle(DataGridViewRow row, DataGridViewCellStyle style)
        {
            // Set the cell Styles in the given row.
            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Style = style;
            }
        }

        private void SetDataGridRowColor()
        {
            this.SetHighLightDatagridView();
            this.SetOddRowColor();
        }

        private void SetHighLightDatagridView()
        {
            // Highlight datagrid row under the mouse cursor.
            if (this.JsonObjSettings.AppParam[0].HighLightDataGridOnMouseOver)
            {
                this.DatagridMouseOverStyle(true);   // True = highlight = on
            }
            else
            {
                this.DatagridMouseOverStyle(false);  // False = highlight = of
            }
        }

        private void DatagridMouseOverStyle(bool highligtRowOn)
        {
            try
            {
                if (highligtRowOn)
                {
                    /*
                     DataGridViewCellStyle HighlightStyle = new DataGridViewCellStyle();
                     HighlightStyle.ForeColor = Color.DarkBlue;
                     HighlightStyle.BackColor = this.HighLightDataGridColor;   //Color.AliceBlue

                     HighlightStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                     */

                    this.highlightStyle = new DataGridViewCellStyle
                    {
                        // HighlightStyle.ForeColor = Color.DarkBlue;
                        // BackColor = Color.AliceBlue
                        BackColor = Color.FromArgb(this.JsonObjSettings.AppParam[0].HighLightDataGridColor),
                    };

                    // HighlightStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
                else
                {
                    if (this.highlightStyle != null)
                    {
                        this.highlightStyle.BackColor = Color.White;
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                TdLogging.WriteToLogError("Argument Out Of Range Exception bij DatagridMouseOverStyle.");
            }
            catch (IndexOutOfRangeException)
            {
                TdLogging.WriteToLogError("Index Out Of Range Exception bij DatagridMouseOverStyle.");
            }
            catch (Exception)
            {
                TdLogging.WriteToLogError("Exception bij DatagridMouseOverStyle.");
            }
        }

        private void SetOddRowColor()
        {
            if (this.JsonObjSettings.AppParam[0].DataGridAlternateRowColor)
            {
                this.DataGridViewQueries.RowsDefaultCellStyle.BackColor = Color.LightGray;
                this.DataGridViewQueries.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            }
            else
            {
                this.DataGridViewQueries.RowsDefaultCellStyle.BackColor = Color.White;
                this.DataGridViewQueries.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            }
        }

        private void DataGridViewQueries_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.DataGridView_CellMouseLeave(this.DataGridViewQueries);
        }

        private void DataGridView_CellMouseLeave(DataGridView dgv)
        {
            try
            {
                if (this.highlightedRowIndex >= 0)
                {
                    SetRowStyle(dgv.Rows[this.highlightedRowIndex], null);
                    this.highlightedRowIndex = -1;
                }
            }
            catch (IndexOutOfRangeException)
            {
                TdLogging.WriteToLogError("Index Out Of Range Exception bij DataGridView_CellMouseLeave.");
            }
            catch (ArgumentOutOfRangeException)
            {
                TdLogging.WriteToLogError("Argument Out Of Range Exception bij DataGridView_CellMouseLeave.");
            }
            catch (Exception)
            {
                TdLogging.WriteToLogError("Exception bij DataGridView_CellMouseLeave.");
            }
        }

        #endregion datagridview color, highlight

        private void DataGridViewQueries_MouseDown(object sender, MouseEventArgs e)
        {
            /* TODO
            if (this.DataGridViewQueries.DataSource != null)
            {
                this.DataGridViewQueries.ContextMenuStrip = ContextMenuStripQuery;
            }
            else
            {
                this.DataGridViewQueries.ContextMenuStrip = null;
            }
            */
        }
    }

    #region Add item(s) to system menu

    /// <summary>
    /// Expand the system menu of the form.
    /// </summary>
    internal class NativeMethods : IDisposable
    {
        /* Clasname must be: NativeMethods. See: https://docs.microsoft.com/nl-nl/visualstudio/code-quality/ca1060-move-p-invokes-to-nativemethods-class?view=vs-2015
           Bron: https://www.developerfusion.com/code/4655/modify-a-windows-system-menu/ */

        /// <summary>
        /// Gets or sets a reference to the form of whicht the system menu will be expanded.
        /// </summary>
        public FormMain Parent { get; set; }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetSystemMenu(int hwnd, int bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern uint AppendMenu(
          IntPtr hMenu, int Flagsw, UIntPtr IDNewItem, string lpNewItem);

        /// <summary>
        /// Create a new row in the system menu.
        /// </summary>
        public void SetupSystemMenu()
        {
            // get handle to system menu
            IntPtr menu = (IntPtr)GetSystemMenu(this.Parent.Handle.ToInt32(), 0);

            // Add a separator
            uint appMenu1 = AppendMenu(menu, 0xA00, (UIntPtr)0, null);

            // Add an item with a unique ID
            uint appMenu2 = AppendMenu(menu, 0, (UIntPtr)1234, TdSettingsDefault.ApplicationName + " " + TdSettingsDefault.ApplicationVersion);  // this adds the 2 new lines to the system menu.

            IntPtr menu2 = (IntPtr)GetSystemMenu(this.Parent.Handle.ToInt32(), 0);

            // Add an item with a unique ID
            uint appMenu3 = AppendMenu(menu2, 0, (UIntPtr)1234, "HvB " + TdSettingsDefault.SystemMenu);

            if (appMenu1 < 1 || appMenu2 < 1 || appMenu3 < 1)
            {
                TdLogging.WriteToLogError("Fout opgetreden bij het uitbreiden van het system_menu.");
            }
        }

        #region Dispose
        private readonly SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);
        private bool disposed;

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Has Dispose already been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.safeHandle?.Dispose();

                    // Free other state (managed objects).
                    this.Parent = null;
                }

                // Free your own state (unmanaged objects). Set large fields to null.
                this.disposed = true;
            }
        }
        #endregion Dispose
    }
    #endregion Add item(s) to system menu
}