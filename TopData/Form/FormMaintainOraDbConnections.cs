namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Resources;
    using System.Security;
    using System.Windows.Forms;
    using Oracle.ManagedDataAccess.Client;

    // using Oracle.DataAccess.Client;

    /// <summary>
    /// Maintain the Oracle connections.
    /// </summary>
    public partial class FormMaintainOraDbConnections : Form
    {
        #region fields
        private ResourceManager rmMb;     // Declare Resource manager to access to specific cultureinfo
        private ResourceManager rmLog;    // Declare Resource manager to access to specific cultureinfo
        private CultureInfo cul;          // Declare culture info
        #endregion fields

        #region Properties

        /// <summary>
        /// Gets or sets settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        private string ConnectionName { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        private string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        private SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the datasource.
        /// </summary>
        private string Datasource { get; set; }

        /// <summary>
        /// Gets or sets the SQLitedatabse file location.
        /// </summary>
        private string DbLocation { get; set; }

        /// <summary>
        /// Gets or sets the SQlite database file name.
        /// </summary>
        private string DbFileName { get; set; }

        /// <summary>
        /// Gets or sets the Ora connection id. Belongs to delete a database connection.
        /// </summary>
        private int OracleConnectionID { get; set; }

        /// <summary>
        /// Gets or sets the Ora database name. Belongs to delete a database connection.
        /// </summary>
        private string OracleConnectionDatabase { get; set; }

        /// <summary>
        /// Gets or sets the Ora schema name. Belongs to delete a database connection.
        /// </summary>
        private string OracleConnectionSchema { get; set; }

        /// <summary>
        /// Gets or sets the password. Belongs to delete a database connection.
        /// </summary>
        private SecureString OracleConnectionPassword { get; set; }

        /// <summary>
        /// Gets or sets the connection name. Belongs to delete a database connection.
        /// </summary>
        private string OracleConnectionName { get; set; }

        /// <summary>
        /// Gets or sets the current tabpage index.
        /// </summary>
        private int CurrentTabpageIndex { get; set; }
        #endregion Properties

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMaintainOraDbConnections"/> class.
        /// </summary>
        public FormMaintainOraDbConnections()
        {
            this.InitializeComponent();
        }
        #endregion constructor

        #region Form Load
        private void FormMaintainOraDbConnections_Load(object sender, EventArgs e)
        {
            this.Text = ResourceText.FrmmaintainDatabseConections;
            this.DoubleBuffered = true;
            this.ButtonTestConnection.Enabled = false;
            this.ButtonClear.Enabled = false;
            this.ButtonAlterTestLogin.Enabled = false;

            this.LoadSettings();
            this.SetCulture();

            this.DbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            this.DbFileName = TdSettingsDefault.SqlLiteDatabaseName;
            this.ActiveControl = this.ComboBoxSchemaName;

            this.LoadFormPosition();
            this.GetConnectionNames(this.TabControlOraConnections.SelectedIndex);
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new ();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }

        private void SetCulture()
        {
            if (this.JsonObjSettings.AppParam[0].Language == "en-US")
            {
                this.cul = CultureInfo.CreateSpecificCulture("en-US");
            }
            else if (this.JsonObjSettings.AppParam[0].Language == "nl-NL")
            {
                this.cul = CultureInfo.CreateSpecificCulture("nl-NL");
            }

            this.rmMb = new ResourceManager("TopData.Resource.Res_Messagebox", typeof(FormMain).Assembly);
            this.rmLog = new ResourceManager("TopData.Resource.Res_Log", typeof(FormMain).Assembly);
        }

        private void LoadFormPosition()
        {
            using TdFormPosition frmPos = new(this);
            frmPos.LoadMaintainOraDbConnFormPosition();
        }

        private void GetConnectionNames(int tabpageindex)
        {
            TdOracleConnectionMaintain oraConn = new ();

            TdOracleConnections conns = oraConn.GetOracleConnectionNames();

            if (tabpageindex == 0)
            {
                this.ComboBoxSchemaName.Items.Clear();
                this.ComboBoxDatabaseName.Items.Clear();

                ReloadCombboxItems(this.ComboBoxSchemaName, "SchemaName");
                ReloadCombboxItems(this.ComboBoxDatabaseName, "DatabaseName");
            }

            if (tabpageindex == 1)
            {
                this.ComboBoxAlterConnectionNames.Items.Clear();

                foreach (TdOracleConnection con in conns.Items)
                {
                    this.ComboBoxAlterConnectionNames.Items.Add(con.Name);  // Only the name is enough, on the textchange the right from the connectioname is retrieved

                    this.ComboBoxAlterConnectionNames.Sorted = true;
                    this.ComboBoxAlterConnectionNames.SelectedIndex = 0;
                }

                ReloadCombboxItems(this.ComboBoxAlterConnectionNames, "ConnectionName");
                ReloadCombboxItems(this.ComboBoxAlterSchemaName, "SchemaName");
                ReloadCombboxItems(this.ComboBoxAlterOraDatabase, "DatabaseName");
            }

            if (tabpageindex == 2)
            {
                this.ComboBoxAlterSchemaName.Items.Clear();
                this.ComboBoxAlterOraDatabase.Items.Clear();

                ReloadCombboxItems(this.ComboBoxDelSchemaName, "SchemaName");
                ReloadCombboxItems(this.ComboBoxDelDatabaseName, "DatabaseName");
            }
        }

        private static void ReloadCombboxItems(ComboBox aCombobox, string item)
        {
            aCombobox.Items.Clear();

            TdAppDbMaintain alterAppDb = new ();
            List<string> itemNames = alterAppDb.ReadDataBaseNames(item);
            foreach (string name in itemNames)
            {
                aCombobox.Items.Add(name);
            }
        }
        #endregion Form Load

        #region Form Close
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMaintainOraDbConnections_FormClosing(object sender, FormClosingEventArgs e)
        {
            TdLogging.WriteToLogInformation("Sluiten beheer Oracle connecties scherm.");
            this.SaveFormPosition();
            this.SaveSettings();
        }

        private void SaveFormPosition()
        {
            using TdFormPosition frmPos = new (this);
            frmPos.SaveMaintainOraDbConnFormPosition();
        }

        private void SaveSettings()
        {
            TdSettingsManager.SaveSettings(this.JsonObjSettings);
        }
        #endregion Form Close

        #region Clear new connections
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            this.ClearAllNewConnection();
        }

        private void ClearAllNewConnection()
        {
            this.ComboBoxSchemaName.Text = string.Empty;
            this.TextBoxPassword.Text = string.Empty;
            this.ComboBoxDatabaseName.Text = string.Empty;
            this.TextBoxConnectionName.Text = string.Empty;

            this.ButtonTestConnection.Enabled = true;
            this.ButtonSave.Enabled = false;
            this.LabelTestConnection.Text = string.Empty;

            this.ActiveControl = this.ComboBoxSchemaName;
        }
        #endregion Clear new connections

        #region New Connection
        private void TextBoxConnectionName_TextChanged(object sender, EventArgs e)
        {
            TdVisual.TxtLengthTolarge(sender, 100);

            if (this.TextBoxConnectionName.TextLength <= 100)
            {
                this.ConnectionName = this.TextBoxConnectionName.Text;
            }
            else
            {
                MessageBox.Show(string.Format("Maximaal {0} tekens toegestaan.", "100"), MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonTestConnection_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.LabelTestConnection.Text = ResourceText._2001;
            this.Refresh();
            try
            {
                using (TdOracleConnectionMaintain testNewConnection = new (this.UserName, this.Password, this.Datasource, this.ConnectionName))
                {
                    if (testNewConnection.TestNewConnection())
                    {
                        TdAppDbMaintain alterAppDb = new ();

                        SaveComboboxItems(this.ComboBoxSchemaName);      // Save the list items in a text file.
                        SaveComboboxItems(this.ComboBoxDatabaseName);    // Save the list items in a text file.

                        this.ButtonSave.Enabled = true;
                        this.ButtonTestConnection.Enabled = false;
                        this.LabelTestConnection.Text = ResourceText._2003;
                        this.ActiveControl = this.ButtonSave;
                    }
                    else
                    {
                        this.LabelTestConnection.Text = ResourceText._2002;
                        this.ButtonSave.Enabled = false;
                    }
                }

                this.Refresh();
                Cursor.Current = Cursors.Default;
            }
            catch (OracleException ex)
            {
                TdLogging.WriteToLogError("Fout opgetreden bij maken van een connectie met Oracle.");
                TdLogging.WriteToLogError("Is de schrijfwijze van de schemanaam of databasename (sid)  juist?");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                this.LabelTestConnection.Text = ResourceText._2002;
            }

            Cursor.Current = Cursors.Default;
        }

        private void ComboBoxSchemaName_TextChanged(object sender, EventArgs e)
        {
            this.UserName = this.ComboBoxSchemaName.Text;

            bool pwdIsSchemaName = this.JsonObjSettings.AppParam[0].PasswordIsSchemaName;

            if (pwdIsSchemaName)
            {
                this.TextBoxPassword.Text = this.ComboBoxSchemaName.Text;
            }

            bool constructConnectionName = this.JsonObjSettings.AppParam[0].ConstructConnectionName;
            if (constructConnectionName)
            {
                if (!string.IsNullOrEmpty(this.ComboBoxSchemaName.Text) || !string.IsNullOrEmpty(this.ComboBoxDatabaseName.Text))
                {
                    this.TextBoxConnectionName.Text = this.ComboBoxSchemaName.Text + "@" + this.ComboBoxDatabaseName.Text;
                }
                else if (string.IsNullOrEmpty(this.ComboBoxSchemaName.Text) && string.IsNullOrEmpty(this.ComboBoxDatabaseName.Text))
                {
                    this.TextBoxConnectionName.Text = string.Empty;
                }
            }

            this.EnableButtons();
        }

        private void ComboBoxSchemaName_Validated(object sender, EventArgs e)
        {
            if (!this.ComboBoxSchemaName.Items.Contains(this.ComboBoxSchemaName.Text))
            {
                this.ComboBoxSchemaName.Items.Add(this.ComboBoxSchemaName.Text);
            }
        }

        private void TextBoxPassword_TextChanged(object sender, EventArgs e)
        {
            using TdSecurityExtensions securityExt = new();
            this.Password = securityExt.ConvertToSecureString(TdEncryptDecrypt.Encrypt(this.TextBoxPassword.Text, TdSettingsDefault.StringSleutel));

            this.EnableButtons();
        }

        private void ComboBoxDatabaseName_TextChanged(object sender, EventArgs e)
        {
            this.Datasource = this.ComboBoxDatabaseName.Text;
            bool constructConnectionName = this.JsonObjSettings.AppParam[0].ConstructConnectionName;

            if (constructConnectionName)
            {
                if (!string.IsNullOrEmpty(this.ComboBoxSchemaName.Text) || !string.IsNullOrEmpty(this.ComboBoxDatabaseName.Text))
                {
                    this.TextBoxConnectionName.Text = this.ComboBoxSchemaName.Text + "@" + this.ComboBoxDatabaseName.Text;
                }
                else if (string.IsNullOrEmpty(this.ComboBoxSchemaName.Text) && string.IsNullOrEmpty(this.ComboBoxDatabaseName.Text))
                {
                    this.TextBoxConnectionName.Text = string.Empty;
                }
            }

            this.EnableButtons();
        }

        private void ComboBoxDatabaseName_Validated(object sender, EventArgs e)
        {
            if (!this.ComboBoxDatabaseName.Items.Contains(this.ComboBoxDatabaseName.Text))
            {
                this.ComboBoxDatabaseName.Items.Add(this.ComboBoxDatabaseName.Text);
            }
        }

        private void EnableButtons()
        {
            if (this.CurrentTabpageIndex == 0)
            {
                if (!string.IsNullOrEmpty(this.ComboBoxSchemaName.Text) ||
                !string.IsNullOrEmpty(this.TextBoxPassword.Text) ||
                !string.IsNullOrEmpty(this.ComboBoxDatabaseName.Text) ||
                !string.IsNullOrEmpty(this.TextBoxConnectionName.Text))
                {
                    this.ButtonClear.Enabled = true;
                }
                else
                {
                    this.ButtonClear.Enabled = false;
                }

                if (!string.IsNullOrEmpty(this.ComboBoxSchemaName.Text) &&
                !string.IsNullOrEmpty(this.TextBoxPassword.Text) &&
                !string.IsNullOrEmpty(this.ComboBoxDatabaseName.Text))
                {
                    this.ButtonTestConnection.Enabled = true;
                }
                else
                {
                    this.ButtonTestConnection.Enabled = false;
                }
            }
            else if (this.CurrentTabpageIndex == 1)
            {
                if (!string.IsNullOrEmpty(this.ComboBoxAlterConnectionNames.Text) &&
                    !string.IsNullOrEmpty(this.ComboBoxAlterSchemaName.Text) &&
                    !string.IsNullOrEmpty(this.TextBoxAlterOraPassword.Text) &&
                    !string.IsNullOrEmpty(this.ComboBoxAlterOraDatabase.Text))
                {
                    this.ButtonAlterTestLogin.Enabled = true;
                }
                else
                {
                    this.ButtonAlterTestLogin.Enabled = false;
                }
            }
            else if (this.CurrentTabpageIndex == 2)
            {
                if (!string.IsNullOrEmpty(this.ComboBoxDelSchemaName.Text))
                {
                    this.ButtonDeleteSchemaName.Enabled = true;
                }
                else
                {
                    this.ButtonDeleteSchemaName.Enabled = false;
                }

                if (!string.IsNullOrEmpty(this.ComboBoxDelDatabaseName.Text))
                {
                    this.ButtonDeleteDatabaseName.Enabled = true;
                }
                else
                {
                    this.ButtonDeleteDatabaseName.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Save all combobox items.
        /// </summary>
        /// <param name="aCombobox">The combobox of which the items will be saved.</param>
        public static void SaveComboboxItems(ComboBox aCombobox) // TODO; move to TopDataMaintainOracleConnections
        {
            TdAppDbMaintain alterAppDb = new ();

            if (aCombobox != null)
            {
                // Not the best solution using a components name
                if (aCombobox.Name.Replace("ComboBox", string.Empty) == "SchemaName" ||
                    aCombobox.Name.Replace("ComboBox", string.Empty) == "AlterSchemaName" ||
                    aCombobox.Name.Replace("ComboBox", string.Empty) == "DelSchemaName")
                {
                    // Create a list with all combobox items
                    foreach (var getitem in aCombobox.Items)
                    {
                        alterAppDb.AddToSchemaNameList(getitem.ToString());
                    }
                }

                if (aCombobox.Name.Replace("ComboBox", string.Empty) == "DatabaseName" ||
                    aCombobox.Name.Replace("ComboBox", string.Empty) == "AlterOraDatabase" ||
                    aCombobox.Name.Replace("ComboBox", string.Empty) == "DelDatabaseName")
                {
                    foreach (var getitem in aCombobox.Items)
                    {
                        alterAppDb.AddToDatabaseaNameList(getitem.ToString());
                    }
                }

                alterAppDb.WriteAllComboBoxItems(aCombobox.Name.Replace("ComboBox", string.Empty));
            }
        }
        #endregion New Connection

        #region Color the textbox/combobox on enter
        private void TextBoxConnectionName_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void TextBoxPassword_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void TextBoxConnectionName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void TextBoxPassword_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void ComboBoxSchemaName_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void ComboBoxDatabaseName_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void ComboBoxSchemaName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void ComboBoxDatabaseName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void ComboBoxAlterConnectionNames_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void TextBoxAlterOraPassword_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void ComboBoxAlterOraDatabase_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void ComboBoxAlterSchemaName_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void ComboBoxAlterConnectionNames_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void TextBoxAlterOraPassword_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void ComboBoxAlterOraDatabase_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void ComboBoxAlterSchemaName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void ComboBoxDelSchemaName_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void ComboBoxDelDatabaseName_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void ComboBoxDelSchemaName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void ComboBoxDelDatabaseName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }
        #endregion Color the textbox/combobox on enter

        #region SaveConnection
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            using (TdOracleConnectionChange saveConnection = new(this.ConnectionName, this.UserName, this.Password, this.Datasource))
            {
                if (saveConnection.SaveConnecctionToOracle())
                {
                    this.LabelTestConnection.Text = string.Empty;
                    this.ButtonSave.Enabled = false;
                    this.ClearAll();
                }
                else
                {
                    this.LabelTestConnection.Text = ResourceText._2000;
                    this.ButtonSave.Enabled = false;
                    this.ButtonTestConnection.Enabled = true;
                }

                this.Refresh();
            }

            Cursor.Current = Cursors.Default;
        }

        private void ClearAll()
        {
            this.ComboBoxSchemaName.Text = string.Empty;
            this.TextBoxPassword.Text = string.Empty;
            this.ComboBoxDatabaseName.Text = string.Empty;
            this.TextBoxConnectionName.Text = string.Empty;

            this.ComboBoxAlterSchemaName.Text = string.Empty;
            this.ComboBoxAlterOraDatabase.Text = string.Empty;

            this.ComboBoxAlterConnectionNames.Text = string.Empty;
            this.ComboBoxAlterSchemaName.Text = string.Empty;
            this.TextBoxAlterOraPassword.Text = string.Empty;
            this.ComboBoxAlterOraDatabase.Text = string.Empty;

            // ButtonAlterTestLogin.Enabled = false;
            this.LabelTestConnection.Text = string.Empty;

            if (this.TabControlOraConnections.SelectedTab.Text == "Maak een connectie")
            {
                this.ActiveControl = this.ComboBoxSchemaName;
            }
        }

        #endregion SaveConnection

        private void TabControlOraConnections_Click(object sender, EventArgs e)
        {
            this.GetConnectionNames(this.TabControlOraConnections.SelectedIndex);
        }

        #region change connection data
        private void ComboBoxAlterConnectionNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetConnectionValues(this.TabControlOraConnections.SelectedIndex);
        }

        private void GetConnectionValues(int tabpageIndex)
        {
            TdOracleConnectionMaintain oraConn = new ();

            TdOracleConnections conns = oraConn.GetOracleConnectionNames();

            foreach (TdOracleConnection con in conns.Items)
            {
                this.OracleConnectionID = con.Id;

                // Alter or delete a connection
                if (tabpageIndex == 1)
                {
                    if (this.ComboBoxAlterConnectionNames.Text == con.Name)
                    {
                        using TdSecurityExtensions securityExt = new();

                        this.ComboBoxAlterSchemaName.Text = con.Schema;
                        try
                        {
                            this.TextBoxAlterOraPassword.Text = TdEncryptDecrypt.Decrypt(securityExt.UnSecureString(con.Password), TdSettingsDefault.StringSleutel);
                        }
                        catch
                        {
                            this.TextBoxAlterOraPassword.Text = securityExt.UnSecureString(con.Password);
                        }

                        this.ComboBoxAlterOraDatabase.Text = con.Connection;

                        this.OracleConnectionDatabase = con.Connection;
                        this.OracleConnectionSchema = con.Schema;
                        try
                        {
                            this.OracleConnectionPassword = securityExt.ConvertToSecureString(TdEncryptDecrypt.Decrypt(securityExt.UnSecureString(con.Password), TdSettingsDefault.StringSleutel));
                        }
                        catch
                        {
                            this.OracleConnectionPassword = con.Password;
                        }

                        this.OracleConnectionName = con.Name;
                        break;
                    }
                    else
                    {
                        this.ComboBoxAlterSchemaName.Text = string.Empty;
                        this.TextBoxAlterOraPassword.Text = string.Empty;
                        this.ComboBoxAlterOraDatabase.Text = string.Empty;
                    }
                }
            }
        }

        #endregion change connection data

        private void ComboBoxAlterConnectionNames_TextChanged(object sender, EventArgs e)
        {
            this.ConnectionName = this.ComboBoxAlterConnectionNames.Text;
            this.EnableButtons();
        }

        private void ComboBoxAlterSchemaName_TextChanged(object sender, EventArgs e)
        {
            this.UserName = this.ComboBoxAlterSchemaName.Text;
            bool pwdIsSchemaName = this.JsonObjSettings.AppParam[0].PasswordIsSchemaName;
            if (pwdIsSchemaName)
            {
                this.TextBoxAlterOraPassword.Text = this.ComboBoxAlterSchemaName.Text;
            }

            this.EnableButtons();
        }

        private void TextBoxAlterOraPassword_TextChanged(object sender, EventArgs e)
        {
            using TdSecurityExtensions securityExt = new();
            this.Password = securityExt.ConvertToSecureString(this.TextBoxAlterOraPassword.Text);
            this.EnableButtons();
        }

        private void ComboBoxAlterOraDatabase_TextChanged(object sender, EventArgs e)
        {
            this.Datasource = this.ComboBoxAlterOraDatabase.Text;
            this.EnableButtons();
        }

        private void ComboBoxAlterOraDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Datasource = this.ComboBoxAlterOraDatabase.Text;
        }

        private void ButtonAlterTestLogin_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                this.LabelTestConnection.Text = ResourceText._2101;
                this.Refresh();

                using TdOracleConnectionChange testOraConnection = new(this.ConnectionName, this.UserName, this.Password, this.Datasource);
                if (testOraConnection.ConnectToOracleAlterConnection())
                {
                    this.LabelTestConnection.Text = ResourceText._2008;
                    this.ButtonAlter.Enabled = true;
                    this.ActiveControl = this.ButtonAlter;
                }
                else
                {
                    this.ButtonAlter.Enabled = false;
                    this.LabelTestConnection.Text = ResourceText._2007;
                }

                this.Refresh();
            }
            catch (OracleException ex)
            {
                TdLogging.WriteToLogError("Fout opgetreden bij maken van een connectie met Oracle.");
                TdLogging.WriteToLogError("Is de schrijfwijze van de schemanaam of databasename (sid)  juist?");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                this.LabelTestConnection.Text = ResourceText._2002;
            }

            Cursor.Current = Cursors.Default;
        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.LabelTestConnection.Text = string.Empty;
            try
            {
                this.LabelTestConnection.Text = ResourceText._2100;
                this.Refresh();

                using TdOracleConnectionChange alterOraConnection = new(this.ConnectionName, this.UserName, this.Password, this.Datasource);
                if (alterOraConnection.AlterConnectionToOracle(this.OracleConnectionID))
                {
                    this.ButtonAlter.Enabled = false;
                    this.LabelTestConnection.Text = ResourceText._2005;
                    this.ComboBoxAlterConnectionNames.Text = this.ConnectionName;    // Put the changed name back

                    TdAppDbMaintain àlterAppDb = new ();

                    SaveComboboxItems(this.ComboBoxAlterSchemaName);                 // Save the list items in a text file.
                    SaveComboboxItems(this.ComboBoxAlterOraDatabase);                // Save the list items in a text file.

                    this.GetConnectionNames(this.TabControlOraConnections.SelectedIndex); // Reload the combobox items
                    this.GetConnectionValues(this.TabControlOraConnections.SelectedIndex);

                    this.TextBoxAlterOraPassword.Text = string.Empty;
                }
                else
                {
                    this.LabelTestConnection.Text = ResourceText._2004;
                    this.ButtonAlter.Enabled = true;
                }

                this.Refresh();
                Cursor.Current = Cursors.Default;
            }
            catch (OracleException ex)
            {
                TdLogging.WriteToLogError("Fout opgetreden bij het wijzigen van een connectie met Oracle.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                this.LabelTestConnection.Text = ResourceText._2004;
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.LabelTestConnection.Text = string.Empty;
            int oraConId = this.OracleConnectionID;

            if (this.OracleConnectionID != TdUserData.ConnectionId)
            {
                try
                {
                    using (TdOracleConnectionChange deleteOraConn = new(this.ConnectionName, this.UserName, this.Password, this.Datasource))
                    {
                        if (TdOracleConnectionChange.DeleteConnecctionToOracle(this.OracleConnectionID, this.ComboBoxAlterConnectionNames.Text))
                        {
                            this.ComboBoxAlterSchemaName.Text = string.Empty;
                            this.TextBoxAlterOraPassword.Text = string.Empty;
                            this.ComboBoxAlterOraDatabase.Text = string.Empty;
                            this.ComboBoxAlterConnectionNames.Text = string.Empty;

                            this.LabelTestConnection.Text = ResourceText._2006;

                            // Reove the connection from de QUERY_GROUP table
                            TdMaintainQueryGroups qGroup = new();
                            qGroup.DeleteQueryGroupOraCon(oraConId);

                            TdAppDbMaintain alterAppDb = new();

                            SaveComboboxItems(this.ComboBoxAlterSchemaName);   // Save the list items
                            SaveComboboxItems(this.ComboBoxAlterOraDatabase);

                            this.GetConnectionNames(this.TabControlOraConnections.SelectedIndex);
                            this.ButtonAlter.Enabled = false;
                        }
                    }

                    Cursor.Current = Cursors.Default;
                }
                catch (OracleException ex)
                {
                    TdLogging.WriteToLogError("Fout opgetreden bij het verwijderen van een connectie met Oracle.");
                    TdLogging.WriteToLogError("Melding:");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    this.LabelTestConnection.Text = ResourceText._2105;
                }
            }
            else
            {
                MessageBox.Show(
                    "U kunt geen actieve connectie verwijderen.",
                    "Informatie.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning); // TODO; een connectie weggooien die door eenanders actief is ??? gaat nu wel. Tabel maken die actieve connecies bij houdt.
            }
        }

        private void ButtonDeleteSchemaName_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string schemaNameToDelete = this.ComboBoxDelSchemaName.Text;
            this.ComboBoxDelSchemaName.Items.Remove(schemaNameToDelete);

            TdAppDbMaintain alterAppDb = new ();
            alterAppDb.DeleteSchemaName(schemaNameToDelete);

            Cursor.Current = Cursors.Default;
        }

        private void TabControlOraConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentTabpageIndex = this.TabControlOraConnections.SelectedIndex;
        }

        private void ComboBoxDelSchemaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableButtons();
        }

        private void ComboBoxDelDatabaseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableButtons();
        }

        private void ButtonDeleteDatabaseName_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string databaseNameToDelete = this.ComboBoxDelDatabaseName.Text;
            this.ComboBoxDelDatabaseName.Items.Remove(databaseNameToDelete);

            TdAppDbMaintain alterAppDb = new();
            alterAppDb.DeleteDatabaseName(databaseNameToDelete);

            Cursor.Current = Cursors.Default;
        }
    }
}
