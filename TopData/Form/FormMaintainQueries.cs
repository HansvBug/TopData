namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using FastColoredTextBoxNS;

    /// <summary>
    /// Form for maintaining queries.
    /// </summary>
    public partial class FormMaintainQueries : Form
    {
        #region properies

        private readonly List<TreeNode> nodesNotSaved = new ();  // List with not saved queries

        private readonly SQLLiteTopData sqlLiteApp;
        private UndoQueryGroups undoQrps = new ();

        /// <summary>
        /// Gets or sets the application settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        /// <summary>
        /// Gets or sets a Reference to the MainForm.
        /// </summary>
        public new FormMain Parent { get; set; }

        private readonly TdTreeViewSearch tvSearch = new ();    // Outside a function otherwise find next doesn't work.

        private bool notSavedQueryOrFolder;

        private SQLLiteTopData SQLLiteApp
        {
            get { return this.sqlLiteApp; }
        }

        // Query data, used to see if there is a change in the data. (If changed then it will be saved)
        private string QueryName { get; set; }

        private string QueryDescription { get; set; }

        private string QueryFileName { get; set; }

        private string QueryWorksheetName { get; set; }

        private string QueryBody { get; set; }

        private bool QueryIsLocked { get; set; }

        private bool QueryIsLockedOldstate { get; set; }

        private string QueryAutorisationGroup { get; set; }

        private string QueryGroupName { get; set; }

        private readonly Dictionary<int, string> querygroupsDict = new ();    // QueryGroup

        private readonly List<string> queryGroupNamesOrgList = new ();        // QueryGroup

        private TdFolder selectedFolder;

        private TdQuery selectedQuery;

        private string SelectedTrvNodeText { get; set; }

        private TreeNode SelectedNode { get; set; }

        /// <summary>
        /// Keep the node selected when the treeview loses focus.
        /// </summary>
        private TreeNode previousSelectedNode = null;

        private List<string> savedExpansionState = new ();    // Save the treeview state

        #endregion properies

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMaintainQueries"/> class.
        /// </summary>
        /// <param name="parent">Reference to the main form.</param>
        public FormMaintainQueries(FormMain parent)
        {
            this.InitializeComponent();
            this.sqlLiteApp = new SQLLiteTopData();
            this.selectedQuery = null;
            this.SelectedNode = null;
            this.Parent = parent;

            this.DoubleBuffered = true;

            this.LoadSettings();
            this.LoadFormPosition();

            this.Text = ResourceText.FrmmaintainQueries;
            if (!string.IsNullOrEmpty(TdUserData.ConnectionName))
            {
                this.Text += "       -> Databaconnectie : " + TdUserData.ConnectionName;
            }

            this.PrepareTreeViewQueries();
            this.TextControls_removeEventHandlers(); // Disable the event handlers from the textboxes. When they fire on loading the fomr there is a false positive on changing
            this.LoadTreeViewFoldersAndQueries();    // Get the folders and queries and put them in a treeview
            this.LoadTreeViewOracleConnections();    // Get the Oracle connection names and put them in a treeview
            this.LoadQueryGroupNames();              // load the querygroup names
            this.LoadTreeviewState();                // Get the expand state of the treeview folders
            this.TextControls_addEventHandlers();    // Enable the event handlers from the textboxes
            this.CreateAutoCompleteForSearchBox();   // Create a autocomplete list for the search query textbox
            this.EmptyInputFields();                 // Disable all input fields
            this.EnableDisableInfo();                // Make de create/edit date and created by textboxes vissible or invissible.
            this.FillComboboxQueryAutorisation();    // Load the autoristion names
            this.SplitContainer1FormQuery.SplitterWidth = 10;
            this.SplitContainer2FormQuery.SplitterWidth = 10;
        }
        #endregion constructor

        #region load form
        private void FormMaintainQueries_Load(object sender, EventArgs e)
        {
            this.ButtonSave.Enabled = false;
            if (TdUserData.UserRole != TdRoleTypes.Viewer)
            {
                this.ToolStripMenuItemCopyDatabase.Enabled = true;
                if (TdUserData.ConnectionId > 0)
                {
                    this.ToolStripMenuItemQueryGroup.Enabled = true;
                }
                else
                {
                    this.ToolStripMenuItemQueryGroup.Enabled = false;
                }
            }
            else
            {
                this.ToolStripMenuItemCopyDatabase.Enabled = false;
            }

            if (TdUserData.UserRole == TdRoleTypes.Owner || TdUserData.UserRole == TdRoleTypes.System)
            {
                this.CheckBoxLockQuery.Visible = true;                  // Lock a query
                this.ToolStripMenuItemExportAllQueries.Visible = true;  // Export all queries
            }
            else
            {
                this.CheckBoxLockQuery.Visible = false;
                this.ToolStripMenuItemExportAllQueries.Visible = false;
            }
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new ();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }

        private void LoadFormPosition()
        {
            using TdFormPosition frmPos = new(this);
            frmPos.LoadMaintainQueriesFormPosition();
            frmPos.LoadMaintainQueriesFormSplitterPosition();
        }

        private void EnableDisableInfo()
        {
            if (TdUserData.UserRole == TdRoleTypes.Owner || TdUserData.UserRole == TdRoleTypes.System || TdUserData.UserRole == TdRoleTypes.Administrator)
            {
                this.LabelCreatedBy.Visible = true;
                this.TextBoxCreatedBy.Visible = true;
                this.TextBoxDateCreated.Visible = true;
                this.LabelDateAltered.Visible = true;
                this.TextBoxDateAltered.Visible = true;
                this.TextBoxAlteredBy.Visible = true;
            }
            else
            {
                this.LabelCreatedBy.Visible = false;
                this.TextBoxCreatedBy.Visible = false;
                this.TextBoxDateCreated.Visible = false;
                this.LabelDateAltered.Visible = false;
                this.TextBoxDateAltered.Visible = false;
                this.TextBoxAlteredBy.Visible = false;
            }
        }

        private void LoadTreeviewState()
        {
            if (this.TreeViewCreateQueries == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(TdUserData.UserName) &&
                    !string.IsNullOrEmpty(TdUserData.UserId) &&
                    !string.IsNullOrEmpty(TdSettingsDefault.SqlLiteDatabaseName))
            {
                TdTreeViewExtensions.ReadtreeviewState(this.TreeViewCreateQueries, TdUserData.UserName, int.Parse(TdUserData.UserId, CultureInfo.InvariantCulture), TdUserData.ConnectionId);
                this.savedExpansionState = this.TreeViewCreateQueries.Nodes.GetExpansionState();
            }
            else
            {
                this.TreeViewCreateQueries.ExpandAll();
            }
        }

        private void FillComboboxQueryAutorisation()
        {
            if (TdUserData.UserRole == TdRoleTypes.Owner)
            {
                this.ComboBoxQueryAutorisation.Items.Clear();
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Owner);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.System);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Administrator);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Editor);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Viewer);
            }
            else if (TdUserData.UserRole == TdRoleTypes.System)
            {
                this.ComboBoxQueryAutorisation.Items.Clear();
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.System);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Administrator);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Editor);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Viewer);
            }
            else if (TdUserData.UserRole == TdRoleTypes.Administrator)
            {
                this.ComboBoxQueryAutorisation.Items.Clear();
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Administrator);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Editor);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Viewer);
            }
            else if (TdUserData.UserRole == TdRoleTypes.Editor)
            {
                this.ComboBoxQueryAutorisation.Items.Clear();
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Editor);
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Viewer);
            }
            else if (TdUserData.UserRole == TdRoleTypes.Viewer)
            {
                this.ComboBoxQueryAutorisation.Items.Clear();
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Viewer);
            }
            else
            {
                this.ComboBoxQueryAutorisation.Items.Clear();
                this.ComboBoxQueryAutorisation.Items.Add(TdRoleTypes.Viewer);
            }

            this.ComboBoxQueryAutorisation.Sorted = true;
        }

        private void ShowExportAllQueries()
        {
            if (TdUserData.UserRole == TdRoleTypes.Owner)
            {
                this.ToolStripMenuItemExportAllQueries.Visible = true;
            }
            else
            {
                this.ToolStripMenuItemExportAllQueries.Visible = false;
            }
        }
        #endregion load form

        #region close form
        private void ToolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMaintainQueries_FormClosing(object sender, FormClosingEventArgs e)
        {
            TdLogging.WriteToLogInformation("Sluiten beheer query's scherm.");
            this.SaveFormPosition();
            this.SaveSettings();

            this.Parent.TreeViewExecuteQueries.AfterSelect -= new TreeViewEventHandler(this.Parent.TreeViewExecuteQueries_AfterSelect);
            this.SaveTreeviewState();   // Save the current treeview expansion state
            this.CheckUnSavedData(e);
            DeleteExport_tmp();         // Delete the Export_Temp folder
            this.Parent.TreeViewExecuteQueries.AfterSelect += new TreeViewEventHandler(this.Parent.TreeViewExecuteQueries_AfterSelect);
        }

        private void SaveTreeviewState()
        {
            this.savedExpansionState = this.TreeViewCreateQueries.Nodes.GetExpansionState();

            if (this.TreeViewCreateQueries.Nodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(TdUserData.UserName) &&
                    !string.IsNullOrEmpty(TdUserData.UserId) &&
                    !string.IsNullOrEmpty(TdSettingsDefault.SqlLiteDatabaseName))
                {
                    TdTreeViewExtensions.SaveTreeviewState(this.savedExpansionState, TdUserData.UserName, int.Parse(TdUserData.UserId, CultureInfo.InvariantCulture), this.TreeViewCreateQueries.Name, TdUserData.ConnectionId);
                }
            }
        }

        private void SaveFormPosition()
        {
            using TdFormPosition frmPos = new (this);
            frmPos.SaveMaintainQueriesFormPosition();
            frmPos.SaveMaintainQueriesFormSplitterPosition();
        }

        private void SaveSettings()
        {
            TdSettingsManager.SaveSettings(this.JsonObjSettings);
        }

        private void CheckUnSavedData(FormClosingEventArgs e)
        {
            if (this.notSavedQueryOrFolder)
            {
                DialogResult dlgsave = MessageBox.Show(
                    "De wijzigingen zijn (nog) niet opgeslagen." + Environment.NewLine +
                    MB_Text.Continue_Without_Saving, MB_Title.Continue,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (dlgsave == DialogResult.No)
                {
                    e.Cancel = true;
                }

                if (dlgsave == DialogResult.Yes)
                {
                    if (TdUserData.ConnectionId > -1)
                    {
                        foreach (UndoQueryGroup item in this.undoQrps.Items)
                        {
                            TdQuery tdQ = new (item.QueryId);
                            tdQ.DeleteQuerGroupIDs(string.Empty);

                            foreach (string qgrp in item.QgrpNames)
                            {
                                tdQ.QueryGroupNames.Add(qgrp);

                                tdQ.SaveQueryGroup();
                            }
                        }
                    }

                    this.notSavedQueryOrFolder = false;
                }
            }
        }
        #endregion close form

        private void SplitContainer1FormQuery_Paint(object sender, PaintEventArgs e)
        {
            TdVisual.Splitcontainerhandle(this.SplitContainer1FormQuery, e);
        }

        private void SplitContainer2FormQuery_Paint(object sender, PaintEventArgs e)
        {
            TdVisual.Splitcontainerhandle(this.SplitContainer2FormQuery, e);
        }

        #region Treeview Qyeries
        private void PrepareTreeViewQueries()
        {
            this.TreeViewCreateQueries.AllowDrop = true;
            this.TreeViewCreateQueries.HideSelection = false;    // TreeView Show selected node when not active
            this.TreeViewCreateQueries.LabelEdit = true;         // Enable edit node labels
        }

        private void LoadTreeViewFoldersAndQueries()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (this.SQLLiteApp != null)
            {
                this.TreeViewCreateQueries.BeginUpdate();

                this.TreeViewCreateQueries.Nodes.Clear();
                TreeNode rootNode = this.TreeViewCreateQueries.Nodes.Add(TdSettingsDefault.ApplicationName, TdSettingsDefault.ApplicationName);   // Set the rootnode
                TdFolders tdFolders = this.SQLLiteApp.GetFolders("WHERE PARENT IS NULL");                                                            // Get the folders stored in the sqlite database file, the parent folders

                foreach (TdFolder tdF in tdFolders.Items)
                {
                    if (tdF.FolderType == FolderTypes.Folder)
                    {
                        TreeNode node = rootNode.Nodes.Add(tdF.Name, tdF.Name);
                        node.Tag = tdF;
                        this.LoadTreeViewFoldersChilds(node);   // Read child folders
                    }
                    else
                    {
                        TreeNode node = rootNode.Nodes.Add(tdF.Name, tdF.Name);
                        node.Tag = tdF;
                    }
                }

                this.TreeViewCreateQueries.EndUpdate();
            }
            else
            {
                MessageBox.Show("De applicatiedatabase is niet gevonden." + Environment.NewLine + "De query's kunnen niet worden ingeladen.", MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TdLogging.WriteToLogError(string.Format("De applicatiedatabase of de tabel {0} is niet gevonden.", TdTableName.QUERY_LIST));
            }

            Cursor.Current = Cursors.Default;
        }

        private void LoadTreeViewFoldersChilds(TreeNode parentNode)
        {
            TdFolder parentFolder = (TdFolder)parentNode.Tag;
            TdFolders tdFolders = this.SQLLiteApp.GetFolders("WHERE PARENT = '" + parentFolder.FolderGuid + "'");

            foreach (TdFolder tdF in tdFolders.Items)
            {
                if (tdF.FolderType == FolderTypes.Folder)
                {
                    TreeNode node = parentNode.Nodes.Add(tdF.Name, tdF.Name);
                    node.Tag = tdF;
                    this.LoadTreeViewFoldersChilds(node);   // Read child folders
                }
                else
                {
                    TreeNode node = parentNode.Nodes.Add(tdF.Name, tdF.Name);
                    node.Tag = tdF;
                }
            }

            TdQueries tdQueries = this.SQLLiteApp.GetQueries("AND FOLDER = '" + parentFolder.FolderGuid + "'");

            foreach (TdQuery tdQ in tdQueries.Items)
            {
                TreeNode node = parentNode.Nodes.Add(tdQ.Name, tdQ.Name);
                node.Tag = tdQ;   //--> !!!
            }
        }
        #endregion Treeview Queries

        #region TreeView connections
        private void LoadTreeViewOracleConnections()
        {
            TdLogging.WriteToLogInformation("Laden connecties.");
            this.TreeViewConnections.HideSelection = false;
            this.TreeViewConnections.CheckBoxes = true;

            if (this.SQLLiteApp != null)
            {
                this.TreeViewConnections.BeginUpdate();
                this.TreeViewConnections.Nodes.Clear();

                TdOracleConnections tdOraCons = this.SQLLiteApp.GetOracleConnectionNames();
                foreach (TdOracleConnection tdoc in tdOraCons.Items)
                {
                    TreeNode node = this.TreeViewConnections.Nodes.Add(tdoc.Name);
                    node.Tag = tdoc;
                }

                this.TreeViewConnections.EndUpdate();
            }

            TdLogging.WriteToLogInformation("Laden connecties gereed.");
            this.Parent.ToolStripStatusLabel1.Text = string.Empty;
            this.Parent.Refresh();
        }

        private void TreeViewConnections_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.TreeViewConnections.SelectedNode = e.Node;

            if (this.TreeViewConnections.SelectedNode.Tag is TdOracleConnection tdOraCon)
            {
                if (tdOraCon != null)
                {
                    try
                    {
                        if (this.TreeViewCreateQueries.SelectedNode != null)
                        {
                            // Check for unsaved queries
                            if (this.TreeViewCreateQueries.SelectedNode.Tag is TdQuery tdQ)
                            {
                                if (tdQ != null)
                                {
                                    if (tdQ.Id == -1)
                                    {
                                        MessageBox.Show(MB_Text.Query_Not_Saved_No_Ora_Conn, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }

                                    if (this.TreeViewConnections.SelectedNode.Checked == true)
                                    {
                                        // The relation Query-connection is saved directly
                                        this.SQLLiteApp.RelateConnectionToQuery(tdOraCon.Id, tdQ.Id);
                                    }
                                    else
                                    {
                                        // The relation Query-connection is deleted directly
                                        this.SQLLiteApp.DeleteRelateConnectionFromQuery(tdOraCon.Id, tdQ.Id);
                                    }
                                }

                                if (!e.Node.Checked)
                                {
                                    this.TreeViewQueryGroup.Enabled = false;
                                    foreach (TreeNode n in this.TreeViewQueryGroup.Nodes)
                                    {
                                        n.Checked = false;
                                    }
                                }
                                else
                                {
                                    if (tdOraCon.Id == TdUserData.ConnectionId)
                                    {
                                        this.TreeViewQueryGroup.Enabled = true;
                                    }
                                }
                            }
                            else
                            {
                                // It is a folder, do nothing
                            }
                        }
                        else
                        {
                            MessageBox.Show("selecteer eerst een query", MB_Title.Attention, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.TreeViewConnections.AfterCheck -= new TreeViewEventHandler(this.TreeViewConnections_AfterCheck);
                            this.TreeViewConnections.SelectedNode.Checked = false;
                            this.TreeViewConnections.AfterCheck += new TreeViewEventHandler(this.TreeViewConnections_AfterCheck);
                        }

                        Cursor.Current = Cursors.Default;
                    }
                    catch (NullReferenceException ex)
                    {
                        TdLogging.WriteToLogError("Onverwachte fout bij het aanvinken van een Oracle connectie.");
                        TdLogging.WriteToLogError("Melding: ");
                        TdLogging.WriteToLogError(ex.Message);
                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug(ex.ToString());
                        }

                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void TreeViewConnections_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            this.TreeViewConnections.SelectedNode = e.Node;  // Select a node. This is used in Afterchecked
        }
        #endregion TreeView connections

        #region CheckChanges data entry
        private void TextBoxName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
            this.Checkchanges();
        }

        private void TextBoxDescription_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
            this.Checkchanges();
        }

        private void TextBoxOutputFileName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
            this.Checkchanges();
        }

        private void TextBoxWorksheetName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
            this.Checkchanges();
        }

        private void ComboBoxQueryAutorisation_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
            this.Checkchanges();
        }

        private void CheckBoxLockQuery_Leave(object sender, EventArgs e)
        {
            this.Checkchanges();
            this.Checkchanges();
        }

        private void FastColoredSQLTextBox_Leave(object sender, EventArgs e)
        {
            // Needed for saving an empty query body
            if (string.IsNullOrEmpty(this.FastColoredSQLTextBox.Text))
            {
                this.selectedQuery.Query = this.FastColoredSQLTextBox.Text;
            }

            this.Checkchanges();
        }

        private void Checkchanges()
        {
            if (TdDebugMode.DebugMode)
            {
                TdLogging.WriteToLogDebug("Checkchanges()  --> this.QueryName : " + this.QueryName);
                TdLogging.WriteToLogDebug("Checkchanges()  --> this.QueryDescription : " + this.QueryDescription);
                TdLogging.WriteToLogDebug("Checkchanges()  --> this.QueryFileName : " + this.QueryFileName);
                TdLogging.WriteToLogDebug("Checkchanges()  --> TextBoxWorksheetName.Text : " + this.TextBoxWorksheetName.Text);
                TdLogging.WriteToLogDebug("Checkchanges()  --> this.QueryIsLocked : " + Convert.ToString(this.QueryIsLocked, CultureInfo.InvariantCulture));
                TdLogging.WriteToLogDebug("Checkchanges()  --> QueryIsLockedOldstate : " + Convert.ToString(this.QueryIsLockedOldstate, CultureInfo.InvariantCulture));

                // TdLogging.WriteToLogDebug("Checkchanges()  --> this.QueryBody : " + this.QueryBody);  //>>>>
                // TdLogging.WriteToLogDebug("Checkchanges()  --> FastColoredSQLTextBox.Text : " + FastColoredSQLTextBox.Text);
                TdLogging.WriteToLogDebug("Checkchanges()  --> this.QueryAutorisationGroup : " + this.QueryAutorisationGroup);

                foreach (KeyValuePair<int, string> entry in this.querygroupsDict)
                {
                    TdLogging.WriteToLogDebug("Query groep (id - value) : " + entry.Key.ToString(CultureInfo.InvariantCulture) + " - " + entry.Value);
                }

                TdLogging.WriteToLogDebug("Checkchanges()  --> this.QueryGroup : " + this.QueryGroupName);
            }

            // Check if there is a difference between the original data and textbox.text data
            if (this.selectedQuery != null)
            {
                var curQueryGroupNames = this.selectedQuery.QueryGroupNames.Except(this.queryGroupNamesOrgList).ToList();
                var orgQueryGroupNames = this.queryGroupNamesOrgList.Except(this.selectedQuery.QueryGroupNames).ToList();

                if (this.QueryName != this.TextBoxName.Text ||
                    this.QueryDescription != this.TextBoxDescription.Text ||
                    this.QueryFileName != this.TextBoxOutputFileName.Text ||
                    this.QueryWorksheetName != this.TextBoxWorksheetName.Text ||
                    this.QueryIsLocked != this.QueryIsLockedOldstate ||
                    this.QueryBody != this.FastColoredSQLTextBox.Text ||
                    this.QueryAutorisationGroup != this.ComboBoxQueryAutorisation.Text ||
                    curQueryGroupNames.Count != orgQueryGroupNames.Count)
                {
                    this.notSavedQueryOrFolder = true;
                    this.AddNotSavedNode(this.SelectedNode);
                    this.ButtonSave.Enabled = true;
                }
                else
                {
                    this.ButtonSave.Enabled = false;
                }
            }
            else
            {
                if (this.QueryName != this.TextBoxName.Text ||
                    this.QueryDescription != this.TextBoxDescription.Text ||
                    this.QueryFileName != this.TextBoxOutputFileName.Text ||
                    this.QueryWorksheetName != this.TextBoxWorksheetName.Text ||
                    this.QueryIsLocked != this.QueryIsLockedOldstate ||
                    this.QueryBody != this.FastColoredSQLTextBox.Text ||
                    this.QueryAutorisationGroup != this.ComboBoxQueryAutorisation.Text)
                {
                    this.notSavedQueryOrFolder = true;
                    this.AddNotSavedNode(this.SelectedNode);
                    this.ButtonSave.Enabled = true;
                }
                else
                {
                    this.ButtonSave.Enabled = false;
                }
            }
        }
        #endregion CheckChanges data entry

        private void AddNotSavedNode(TreeNode node)
        {
            // Check if node exists in the not saved list
            foreach (TreeNode tn in this.nodesNotSaved)
            {
                if (tn == node)
                {
                    return;
                }
            }

            this.nodesNotSaved.Add(node);
            this.notSavedQueryOrFolder = true;
        }

        #region handle eventhandles
        private void TextControls_removeEventHandlers()
        {
            this.TextBoxName.TextChanged -= new EventHandler(this.TextBoxName_TextChanged);
            this.TextBoxName.Leave -= new EventHandler(this.TextBoxName_Leave);

            this.TextBoxDescription.TextChanged -= new EventHandler(this.TextBoxDescription_TextChanged);
            this.TextBoxDescription.Leave -= new EventHandler(this.TextBoxDescription_Leave);

            this.TextBoxOutputFileName.TextChanged -= new EventHandler(this.TextBoxOutputFileName_TextChanged);
            this.TextBoxOutputFileName.Leave -= new EventHandler(this.TextBoxOutputFileName_Leave);

            this.TextBoxWorksheetName.TextChanged -= new EventHandler(this.TextBoxWorksheetName_TextChanged);
            this.TextBoxWorksheetName.Leave -= new EventHandler(this.TextBoxWorksheetName_Leave);

            this.ComboBoxQueryAutorisation.TextChanged -= new EventHandler(this.ComboBoxQueryAutorisation_TextChanged);
            this.ComboBoxQueryAutorisation.Leave -= new EventHandler(this.ComboBoxQueryAutorisation_Leave);

            this.CheckBoxLockQuery.CheckedChanged -= new EventHandler(this.CheckBoxLockQuery_CheckedChanged);
            this.CheckBoxLockQuery.Leave -= new EventHandler(this.CheckBoxLockQuery_Leave);

            this.FastColoredSQLTextBox.Leave -= new EventHandler(this.FastColoredSQLTextBox_Leave);

            this.TreeViewQueryGroup.AfterCheck -= new TreeViewEventHandler(this.TreeViewQueryGroup_AfterCheck);
        }

        private void TextControls_addEventHandlers()
        {
            this.TextBoxName.TextChanged += new EventHandler(this.TextBoxName_TextChanged);
            this.TextBoxName.Leave += new EventHandler(this.TextBoxName_Leave);

            this.TextBoxDescription.TextChanged += new EventHandler(this.TextBoxDescription_TextChanged);
            this.TextBoxDescription.Leave += new EventHandler(this.TextBoxDescription_Leave);

            this.TextBoxOutputFileName.TextChanged += new EventHandler(this.TextBoxOutputFileName_TextChanged);
            this.TextBoxOutputFileName.Leave += new EventHandler(this.TextBoxOutputFileName_Leave);

            this.TextBoxWorksheetName.TextChanged += new EventHandler(this.TextBoxWorksheetName_TextChanged);
            this.TextBoxWorksheetName.Leave += new EventHandler(this.TextBoxWorksheetName_Leave);

            this.ComboBoxQueryAutorisation.TextChanged += new EventHandler(this.ComboBoxQueryAutorisation_TextChanged);
            this.ComboBoxQueryAutorisation.Leave += new EventHandler(this.ComboBoxQueryAutorisation_Leave);

            this.CheckBoxLockQuery.CheckedChanged += new EventHandler(this.CheckBoxLockQuery_CheckedChanged);
            this.CheckBoxLockQuery.Leave += new EventHandler(this.CheckBoxLockQuery_Leave);

            this.FastColoredSQLTextBox.Leave += new EventHandler(this.FastColoredSQLTextBox_Leave);

            this.TreeViewQueryGroup.AfterCheck += new TreeViewEventHandler(this.TreeViewQueryGroup_AfterCheck); // ToDo hoort hier niet (dubbel?)
        }
        #endregion handle eventhandles

        #region Entry is changed
        private void TextBoxName_TextChanged(object sender, EventArgs e)
        {
            // QUERYNAME  VARCHAR(255)
            if (this.TextBoxName.Text.Length <= 255)
            {
                this.selectedQuery.Name = this.TextBoxName.Text;
            }
            else
            {
                MessageBox.Show(MB_Text.Max_255, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            // DESCRIPTION VARCHAR(255)
            if (this.TextBoxDescription.Text.Length <= 255)
            {
                this.selectedQuery.Description = this.TextBoxDescription.Text;
            }
            else
            {
                MessageBox.Show(MB_Text.Max_255, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TextBoxOutputFileName_TextChanged(object sender, EventArgs e)
        {
            if (this.TextBoxOutputFileName.Text.Length > 0)
            {
                this.TextBoxWorksheetName.Enabled = true;

                // FILENAME_EXP VARCHAR(10000)
                if (this.TextBoxOutputFileName.Text.Length <= 10000)
                {
                    this.selectedQuery.FileNameOutput = this.TextBoxOutputFileName.Text;
                }
                else
                {
                    MessageBox.Show(MB_Text.Max_10000, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                this.TextBoxWorksheetName.Enabled = false;
                this.selectedQuery.FileNameOutput = string.Empty;
            }
        }

        private void TextBoxWorksheetName_TextChanged(object sender, EventArgs e)
        {
            // WORKSHEET VARCHAR(31)
            if (this.TextBoxWorksheetName.Text.Length <= 31)
            {
                this.selectedQuery.WorksheetName = this.TextBoxWorksheetName.Text;
            }
            else
            {
                MessageBox.Show(MB_Text.Max_31_Chars_Excel, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ComboBoxQueryAutorisation_TextChanged(object sender, EventArgs e)
        {
            // VARCHAR(50)
            if (this.ComboBoxQueryAutorisation.Text.Length <= 50)
            {
                this.selectedQuery.QueryAutorisation = this.ComboBoxQueryAutorisation.Text;
            }
            else
            {
                MessageBox.Show(MB_Text.Max_50, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CheckBoxLockQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBoxLockQuery.Checked)
            {
                this.QueryIsLocked = true;
            }
            else
            {
                this.QueryIsLocked = false;
            }

            this.selectedQuery.QueryIsLocked = this.QueryIsLocked;
        }

        private void FastColoredSQLTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // QUERY  VARCHAR(900000)
            if (this.FastColoredSQLTextBox.Text.Length > 0)
            {
                if (this.FastColoredSQLTextBox.Text.Length <= 900000)
                {
                    this.selectedQuery.Query = this.FastColoredSQLTextBox.Text;
                }
                else
                {
                    MessageBox.Show(MB_Text.Max_900000, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // Do not add else when there is no query body the query body wil not be viewed until you start again (Empty fastcolored textbox)
        }
        #endregion Entry is changed

        #region Search in Treeview

        private void TextBoxSearchInQueryTreeView_TextChanged(object sender, EventArgs e)
        {
            this.tvSearch.FoundWithTrvSearch = 0;
            TreeNodeCollection nodes = this.TreeViewCreateQueries.Nodes;
            foreach (TreeNode n in nodes)
            {
                this.tvSearch.ColorTrvSearchNode(n, this.TextBoxSearchInQueryTreeView);
            }

            this.LabelTrvSearchFoundGBIschema.Text = this.tvSearch.FoundWithTrvSearch.ToString(CultureInfo.InvariantCulture) + " st";
        }

        private void ButtonSearchQueryName_Click(object sender, EventArgs e)
        {
            this.tvSearch.SearchInTreeViewNodes(this.TreeViewCreateQueries, this.TextBoxSearchInQueryTreeView.Text);
        }
        #endregion Search in Treeview

        private void TextBoxSearchInQueryTreeView_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void TextBoxSearchInQueryTreeView_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        #region TreeViewCreateQueries
        private void TreeViewCreateQueries_Click(object sender, EventArgs e)
        {
            this.LabelQueryIsExecuted.Text = string.Empty;  // Empty the label text after a query test is done
        }

        private void TreeViewCreateQueries_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.TreeViewCreateQueries.SelectedNode == this.TreeViewCreateQueries.TopNode)
            {
                return;
            }

            if (this.TreeViewCreateQueries.SelectedNode == null)
            {
                return;
            }

            if (e.KeyData == Keys.F2)
            {
                this.TreeViewCreateQueries.LabelEdit = true;
                this.TreeViewCreateQueries.SelectedNode.BeginEdit();
                this.SelectedTrvNodeText = this.TreeViewCreateQueries.SelectedNode.Text;
            }

            if (e.KeyData == Keys.Escape)
            {
                this.TreeViewCreateQueries.SelectedNode.EndEdit(false);
            }
        }

        private void TreeViewCreateQueries_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // Get the map name. Even when the TAB key is pressed
            if (e.Label != null)
            {
                if (this.TreeViewCreateQueries.SelectedNode == this.TreeViewCreateQueries.TopNode)
                {
                    return;
                }

                if (this.TreeViewCreateQueries.SelectedNode == null)
                {
                    return;
                }

                try
                {
                    if (this.TreeViewCreateQueries.SelectedNode.Tag != null)
                    {
                        if (this.TreeViewCreateQueries.SelectedNode.Tag is TdFolder tdF)
                        {
                            tdF.Name = e.Label;
                            this.AddNotSavedNode(this.TreeViewCreateQueries.SelectedNode);
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    TdLogging.WriteToLogError("Onverwachte fout bij het veranderen van een map- of query naam.");
                    TdLogging.WriteToLogError("Melding:");
                    TdLogging.WriteToLogError(ex.Message);
                }
                finally
                {
                    this.TreeViewCreateQueries.SelectedNode.EndEdit(true);
                }
            }
        }

        private void TreeViewCreateQueries_Validated(object sender, EventArgs e)
        {
            if (this.TreeViewCreateQueries.SelectedNode != null)
            {
                this.TreeViewCreateQueries.SelectedNode.BackColor = SystemColors.Highlight;
                this.TreeViewCreateQueries.SelectedNode.ForeColor = Color.White;
                this.previousSelectedNode = this.TreeViewCreateQueries.SelectedNode;
            }
        }

        private void TreeViewCreateQueries_MouseClick(object sender, MouseEventArgs e)
        {
            // Used for the renaming of the treeview nodes
            if (e.Button == MouseButtons.Right)
            {
                // Point where mouse is clicked
                Point p = new (e.X, e.Y);
                this.TreeViewCreateQueries.SelectedNode = this.TreeViewCreateQueries.GetNodeAt(p);
                this.TreeViewCreateQueries_AfterSelect(sender, null);
            }
        }

        private void TreeViewCreateQueries_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.SelectedNode = this.TreeViewCreateQueries.SelectedNode;  // Needed when a query is changed and then another is deleted. The selected node.tag is then empty.

            this.KeepTrvNodeselected();              // Keep the node selected when the treeview loses focus
            this.TextControls_removeEventHandlers(); // Avoid triggering textchange eventhandlers when clicking on a treeview node
            this.EmptyInputFields();                 // Clear the entry fields

            this.selectedQuery = null;
            this.selectedFolder = null;

            this.UnCheckAllTrvQueryNodes();          // Uncheck all Querygroups

            try
            {
                if (this.SelectedNode.Tag is TdQuery query)
                {
                    this.selectedQuery = query;

                    // Get the org. data of the selected node //needed to see if there will be changes
                    this.TextBoxName.Text = this.selectedQuery.Name;
                    this.TextBoxDescription.Text = this.selectedQuery.Description;
                    this.FastColoredSQLTextBox.Text = this.selectedQuery.Query;
                    this.TextBoxOutputFileName.Text = this.selectedQuery.FileNameOutput;
                    this.TextBoxWorksheetName.Text = this.selectedQuery.WorksheetName;
                    this.ComboBoxQueryAutorisation.Text = this.selectedQuery.QueryAutorisation;

                    if (!this.selectedQuery.QueryIsLocked)
                    {
                        this.QueryIsLocked = false;
                    }
                    else
                    {
                        this.QueryIsLocked = true;
                    }

                    this.TextControls_addEventHandlers();
                    this.TreeViewQueryGroup.AfterCheck -= new TreeViewEventHandler(this.TreeViewQueryGroup_AfterCheck);

                    // Ad the query info into the entry fields
                    if (!string.IsNullOrEmpty(this.selectedQuery.CreatedBy) &&
                        !string.IsNullOrEmpty(this.selectedQuery.AlteredBy) &&
                        !string.IsNullOrEmpty(this.selectedQuery.QueryGuid))
                    {
                        this.TextBoxCreatedBy.Text = this.selectedQuery.CreatedBy;
                        this.TextBoxAlteredBy.Text = this.selectedQuery.AlteredBy;
                        this.TextBoxDateCreated.Text = this.selectedQuery.Created.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        this.TextBoxDateAltered.Text = this.selectedQuery.Modified.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        this.TextBoxQueryGuid.Text = this.selectedQuery.QueryGuid;
                    }

                    // Set properties with original data. This is used to see if there is a change in data
                    this.QueryName = this.TextBoxName.Text;
                    this.QueryDescription = this.TextBoxDescription.Text;
                    this.QueryFileName = this.TextBoxOutputFileName.Text;
                    this.QueryBody = this.FastColoredSQLTextBox.Text;
                    this.QueryWorksheetName = this.TextBoxWorksheetName.Text;
                    this.QueryAutorisationGroup = this.ComboBoxQueryAutorisation.Text;

                    this.TreeViewConnecties_removeEventHandlers();

                    // Get the query-dataschema connections
                    TdOracleConnections tdCons = this.SQLLiteApp.SelectRelatedConnections(this.selectedQuery.Id);

                    bool enableQuerygroups = false;

                    foreach (TreeNode trvCon in this.TreeViewConnections.Nodes)
                    {
                        trvCon.Checked = false;
                        TdOracleConnection tnTdcon = (TdOracleConnection)trvCon.Tag;
                        foreach (TdOracleConnection tdCon in tdCons.Items)
                        {
                            if (tnTdcon.Id == tdCon.Id)
                            {
                                trvCon.Checked = true;

                                // Only activate the querygroups when there is a connection
                                if (tnTdcon.Id == TdUserData.ConnectionId)
                                {
                                    enableQuerygroups = true;
                                }
                            }
                        }
                    }

                    this.CheckComponentStatus(enableQuerygroups);

                    this.TreeViewConnecties_setEventHandlers();

                    this.EnbleTrvDbConnections(this.selectedQuery);   // Only enable related databaseconnections when query is saved.
                    this.PrepareQueryGroups(this.selectedQuery);      // Get the query groups.

                    this.PrepareComponentsAfterselect();
               }
                else
                {
                    this.UnCheckAllTrvConnectionNodes(); // Uncheck the connections
                    this.CheckComponentStatus(false);
                    this.EmptyInputFields();
                }
            }
            catch (NullReferenceException ex)
            {
                TdLogging.WriteToLogError("Onverwachte fout bij TreeViewCreateQueries_AfterSelect");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }
            }

            // Select a folder
            try
            {
                if (this.SelectedNode.Tag is TdFolder folder)
                {
                    // Get the data of the selected node
                    this.selectedFolder = folder;
                    this.TreeViewCreateQueries.ContextMenuStrip = this.ContextMenuStripFolderBeheer;
                }
            }
            catch (NullReferenceException ex)
            {
                TdLogging.WriteToLogError("Onverwachte fout bij TreeViewCreateQueries_AfterSelect");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
            }

            // Delete contect menu when topnode is selected.
            if (this.TreeViewCreateQueries.SelectedNode == this.TreeViewCreateQueries.TopNode)
            {
                this.TreeViewCreateQueries.ContextMenuStrip = this.ContextMenuStripFolderBeheer;
            }
        }

        private void KeepTrvNodeselected() // Keep the node selected when the treeview loses focus
        {
            this.TreeViewCreateQueries.HideSelection = true;
            if (this.previousSelectedNode != null)
            {
                this.previousSelectedNode.BackColor = this.TreeViewCreateQueries.BackColor;
                this.previousSelectedNode.ForeColor = this.TreeViewCreateQueries.ForeColor;
            }
        }

        private void EmptyInputFields()
        {
            this.TextControls_removeEventHandlers();         // Add when new objects are added to the form
            this.TextBoxName.Text = string.Empty;
            this.TextBoxDescription.Text = string.Empty;
            this.TextBoxWorksheetName.Text = string.Empty;
            this.FastColoredSQLTextBox.Text = string.Empty;
            this.TextBoxOutputFileName.Text = string.Empty;
            this.ComboBoxQueryAutorisation.Text = string.Empty;

            this.TextBoxCreatedBy.Text = string.Empty;
            this.TextBoxDateCreated.Text = string.Empty;
            this.TextBoxDateAltered.Text = string.Empty;
            this.TextBoxAlteredBy.Text = string.Empty;
            this.TextBoxQueryGuid.Text = string.Empty;

            this.TextBoxName.Enabled = false;
            this.TextBoxDescription.Enabled = false;
            this.TextBoxWorksheetName.Enabled = false;
            this.FastColoredSQLTextBox.Enabled = false;
            this.TextBoxOutputFileName.Enabled = false;
            this.CheckBoxLockQuery.Enabled = false;
            this.ComboBoxQueryAutorisation.Enabled = false;

            this.ButtonSelectedFileOutput.Enabled = false;
            this.TreeViewConnections.Enabled = false;
            this.TreeViewQueryGroup.Enabled = false;

            this.TextBoxName.Focus();
            this.TextControls_addEventHandlers();    // Add when new objects are added to the form
        }

        private void UnCheckAllTrvQueryNodes() // Uncheck all Querygroups
        {
            foreach (TreeNode tn in this.TreeViewQueryGroup.Nodes)
            {
                tn.Checked = false;
            }
        }

        private void CheckComponentStatus(bool enableQgroup)
        {
            if (this.Parent.OraConn != null)
            {
                this.ButtonTestQuery.Enabled = true;
                this.ToolStripMenuItemTestQuery.Enabled = true;
                if (enableQgroup)
                {
                    this.TreeViewQueryGroup.Enabled = true;
                }
            }
            else
            {
                this.ButtonTestQuery.Enabled = false;
                this.ToolStripMenuItemTestQuery.Enabled = false;
                this.TreeViewQueryGroup.Enabled = false;
            }
        }

        private void EnbleTrvDbConnections(TdQuery selectedQuery)
        {
            if (selectedQuery.Id != -1)
            {
                this.TreeViewConnections.Enabled = true;
                this.TreeViewCreateQueries.ContextMenuStrip = this.ContextMenuStripQueryBeheer;
            }
            else
            {
                this.TreeViewConnections.Enabled = false;
                this.TreeViewCreateQueries.ContextMenuStrip = null;
            }
        }

        private void PrepareQueryGroups(TdQuery selectedQuery)
        {
            this.TreeViewQueryGroup.AfterCheck -= new TreeViewEventHandler(this.TreeViewQueryGroup_AfterCheck);  // Do not remove

            if (selectedQuery.QueryGroupNames.Count > 0)
            {
                foreach (string qGroup in selectedQuery.QueryGroupNames)
                {
                    this.CheckQueryGroups(this.TreeViewQueryGroup.Nodes, qGroup);
                }
            }
            else
            {
                foreach (TreeNode node in this.TreeViewQueryGroup.Nodes)
                {
                    this.CheckQueryGroups(this.TreeViewQueryGroup.Nodes, string.Empty);  // = --> node.Checked = false;
                }
            }

            this.TreeViewQueryGroup.AfterCheck += new TreeViewEventHandler(this.TreeViewQueryGroup_AfterCheck);

            // Needed for checkChanges();
            // Prepare a class with uno info for the querygroups
            UndoQueryGroup undoQgrp = new ()
            {
                QueryId = selectedQuery.Id,
                SelectedQuery = selectedQuery,
            };

            this.queryGroupNamesOrgList.Clear();
            foreach (TreeNode node in this.TreeViewQueryGroup.Nodes)
            {
                if (node.Checked)
                {
                    this.queryGroupNamesOrgList.Add(node.Text);
                    undoQgrp.QgrpNames.Add(node.Text);  // Needed for the uno action when the forms closes without saving
                }
            }

            this.undoQrps.Items.Add(undoQgrp);
        }

        private void CheckQueryGroups(TreeNodeCollection nodes, string qGroup)
        {
            foreach (TreeNode child in nodes)
            {
                if (child.Text == qGroup)
                {
                    child.Checked = true;
                }
                else if (string.IsNullOrEmpty(qGroup))
                {
                    child.Checked = false;
                }

                this.CheckQueryGroups(child.Nodes, qGroup);
            }
        }

        private void PrepareComponentsAfterselect()
        {
            if (TdUserData.UserRole == TdRoleTypes.Owner || TdUserData.UserRole == TdRoleTypes.System)
            {
                this.TextBoxName.Enabled = true;
                this.TextBoxDescription.Enabled = true;
                this.FastColoredSQLTextBox.Enabled = true;
                this.TextBoxOutputFileName.Enabled = true;
                this.ButtonSelectedFileOutput.Enabled = true;
                this.ComboBoxQueryAutorisation.Enabled = true;

                if (!this.selectedQuery.QueryIsLocked)
                {
                    this.CheckBoxLockQuery.Checked = false;
                    this.QueryIsLockedOldstate = false;
                }
                else
                {
                    this.CheckBoxLockQuery.Checked = true;
                    this.QueryIsLockedOldstate = true;
                }

                this.CheckBoxLockQuery.Enabled = true;

                if (!string.IsNullOrEmpty(this.TextBoxOutputFileName.Text))
                {
                    this.TextBoxWorksheetName.Enabled = true;
                }
            }
            else
            {
                if (this.QueryIsLocked)
                {
                    this.TextBoxName.Enabled = false;
                    this.TextBoxDescription.Enabled = false;
                    this.FastColoredSQLTextBox.Enabled = false;
                    this.TextBoxOutputFileName.Enabled = false;
                    this.ButtonSelectedFileOutput.Enabled = false;
                    this.ComboBoxQueryAutorisation.Enabled = false;
                    this.TreeViewQueryGroup.Enabled = false;

                    this.QueryIsLockedOldstate = false;

                    this.CheckBoxLockQuery.Enabled = false;
                    this.TextBoxWorksheetName.Enabled = false;
                }
                else
                {
                    this.TextBoxName.Enabled = true;
                    this.TextBoxDescription.Enabled = true;
                    this.FastColoredSQLTextBox.Enabled = true;
                    this.TextBoxOutputFileName.Enabled = true;
                    this.ButtonSelectedFileOutput.Enabled = true;
                    this.ComboBoxQueryAutorisation.Enabled = true;

                    if (!this.selectedQuery.QueryIsLocked)
                    {
                        this.CheckBoxLockQuery.Checked = false;
                        this.QueryIsLockedOldstate = false;
                    }
                    else
                    {
                        this.CheckBoxLockQuery.Checked = true;
                        this.QueryIsLockedOldstate = true;
                    }

                    this.CheckBoxLockQuery.Enabled = false;

                    if (!string.IsNullOrEmpty(this.TextBoxOutputFileName.Text))
                    {
                        this.TextBoxWorksheetName.Enabled = true;
                    }
                }
            }
        }

        private void UnCheckAllTrvConnectionNodes()
        {
            this.TreeViewConnecties_removeEventHandlers();
            foreach (TreeNode tn in this.TreeViewConnections.Nodes)
            {
                tn.Checked = false;
            }

            this.TreeViewConnecties_setEventHandlers();
        }

        #region Undo Querygroup checks
        private class UndoQueryGroup
        {
            public int QueryId { get; set; }

            public TdQuery SelectedQuery { get; set; }

            private List<string> qgrpNames = new ();

            /// <summary>
            /// Gets or sets a list with query group names.
            /// </summary>
            public List<string> QgrpNames
            {
                get { return this.qgrpNames; }
                set { this.qgrpNames = value; }
            }

            private int ClassId { get; set; }

            public UndoQueryGroup()
            {
                this.ClassId++;
            }
        }

        private class UndoQueryGroups
        {
            private int ClassId { get; set; }

            private readonly List<UndoQueryGroup> items = new ();

            public List<UndoQueryGroup> Items
            {
                get { return this.items; }
            }

            public UndoQueryGroups()
            {
                this.ClassId++;
            }
        }
        #endregion Undo Querygroup checks
        #endregion TreeViewCreateQueries

        #region Drag drop
        private void TreeViewCreateQueries_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = this.TreeViewCreateQueries.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = this.TreeViewCreateQueries.GetNodeAt(targetPoint);  // Wrong when dropped on the topnode?

            var dropppedNode = targetNode.Tag;  // Droppped Node holds the complete object from the targed treenode tag

            // The TopNode "Topdata"has no tag and is null
            if (dropppedNode != null)
            {
                System.Reflection.PropertyInfo droppedPi = dropppedNode.GetType().GetProperty("FolderGuid");  // FolderGuid = propertyname of "public class TopDataFolder"

                if (droppedPi != null)
                {
                    string queryDestFolderGuid = (string)droppedPi.GetValue(dropppedNode, null);

                    // Retrieve the node that was dragged.
                    TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                    var queryObject = draggedNode.Tag;  // QueryObject holds the complete object from the dragged treenode tag. Get the Id of the selected query:

                    // You can't drag the topnode of the treeview. 'Topdata"
                    if (queryObject != null)
                    {
                        /*try
                        {
                            TopDataFolder tmp = (TopDataFolder)queryObject;
                            if (tmp.FolderType == FolderTypes.Folder)
                            {
                                MessageBox.Show("Mappen kunnen niet met drag en drop worden verplaatst.", MB_Title.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        finally
                        {
                        TODO; how to make this work?
                        }*/

                        System.Reflection.PropertyInfo draggedPiId = queryObject.GetType().GetProperty("Id");
                        int queryFromId = (int)draggedPiId.GetValue(queryObject, null);  // Query id

                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug("TreeViewQueries_DragDrop  --> QueryDestFolderGuid : " + queryDestFolderGuid);
                            TdLogging.WriteToLogDebug("TreeViewQueries_DragDrop  --> QueryObject : " + queryObject.ToString());
                            TdLogging.WriteToLogDebug("TreeViewQueries_DragDrop  --> QueryFromId : " + queryFromId.ToString(CultureInfo.InvariantCulture));
                        }

                        // Confirm that the node at the drop location is not
                        // The dragged node and that target node isn't null (for example if you drag outside the control)
                        // Allow only drop when the target is a folder.
                        if (this.GetFolderGuid(targetNode))
                        {
                            if (!draggedNode.Equals(targetNode) && targetNode != null)
                            {
                                // Remove the node from its current
                                // location and add it to the node at the drop location.
                                draggedNode.Remove();

                                targetNode.Nodes.Add(draggedNode);

                                this.TreeViewCreateQueries.Sort();

                                // Expand the node at the location
                                // to show the dropped node.
                                targetNode.Expand();

                                // Update the QB_QUERY_LIST
                                TdQuery moveQuery = new (queryFromId);
                                moveQuery.UpdateQueryFolderWhenDragged(queryFromId, queryDestFolderGuid);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(MB_Text.Drop_Query_On_Folder, MB_Title.Warning, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool GetFolderGuid(TreeNode targetNode)
        {
            return this.GetFolderFromNode(targetNode, folderGuid: out _);
        }

        private bool GetFolderFromNode(TreeNode aTreenode, out string folderGuid)
        {
            string[] splitnodes = aTreenode.FullPath.Split('\\');
            string guid = string.Empty;
            bool isFolder = false;

            TdFolders folders = this.SQLLiteApp.GetFolders(" where NAME = '" + splitnodes.Last() + "' ");
            foreach (TdFolder tdQ in folders.Items)
            {
                if (tdQ.Name == splitnodes.Last())
                {
                    isFolder = true;
                    guid = tdQ.FolderGuid;
                }
            }

            folderGuid = guid;
            return isFolder;
        }

        private void TreeViewCreateQueries_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;    // Handle user dragging node
        }

        private void TreeViewCreateQueries_DragOver(object sender, DragEventArgs e)
        {
            // Scoll automatic during drag drop
            try
            {
                TreeView tv = sender as TreeView;
                Point pt = tv.PointToClient(new Point(e.X, e.Y));

                int deltay = tv.Height - pt.Y;
                if ((deltay < tv.Height / 2) && (deltay > 0))
                {
                    TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
                    if (tn.NextVisibleNode != null)
                    {
                        tn.NextVisibleNode.EnsureVisible();
                    }
                }

                if ((deltay > tv.Height / 2) && (deltay < tv.Height))
                {
                    TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
                    if (tn.PrevVisibleNode != null)
                    {
                        tn.PrevVisibleNode.EnsureVisible();
                    }
                }
            }
            catch (NullReferenceException)
            {
                TdLogging.WriteToLogError("Fout bij het automatisch scrollen tijdens drag-drop query.");
            }
        }

        private void TreeViewCreateQueries_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Drag and drop is only active when any change in folders or Queries is saved. So no pending changes are allowd
            if (this.notSavedQueryOrFolder)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Wijzigingen in mappen of query's zijn nog niet opgeslagen." + Environment.NewLine +
                    "Voordat u een query kunt verslepen moeten alle wijzigingen zijn opgeslagen." + Environment.NewLine
                    + Environment.NewLine +
                    "Opslaan?",
                    MB_Title.Drag_Query,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);

                if (dialogResult == DialogResult.Yes)
                {
                    this.SaveQuery();
                }
                else if (dialogResult == DialogResult.No)
                {
                    // Return without saving and without drag and drop
                }
            }
            else
            {
                this.DoDragDrop(e.Item, DragDropEffects.Move);   // Handle user dragging nodes in treeview

                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug("TreeViewQueries_DragDrop  --> e.Item : " + e.Item.ToString());
                }
            }
        }
        #endregion Drag drop

        #region Save the folders and queries
        private void SaveQuery()
        {
            Cursor.Current = Cursors.WaitCursor;

            // Save new/changed folders and queries
            if (this.notSavedQueryOrFolder)
            {
                this.SaveUpdateQueryOrFolder();
                this.notSavedQueryOrFolder = false;
            }

            Cursor.Current = Cursors.Default;
        }

        private void SaveUpdateQueryOrFolder()
        {
            Cursor.Current = Cursors.WaitCursor;

            Stopwatch stopWatch = new ();
            stopWatch.Start();
            TdLogging.WriteToLogInformation("Opslaan mappen en query's.");

            // TODO; create HERE a dbconnection, so that only 1 connection is needed. Now there is a connection for every query that will be saved
            foreach (TreeNode node in this.nodesNotSaved)
            {
                if (node.Tag is TdFolder tdF)
                {
                    tdF.SaveFolder();                  // Save the folder
                }

                if (node.Tag is TdQuery tdQ)
                {
                    tdQ.SaveQuery();                   // Save the query
                }
            }

            this.nodesNotSaved.Clear();                // Empty SaveBuffer
            this.notSavedQueryOrFolder = false;

            this.LoadTreeViewFoldersAndQueries();      // Load the changes folders and query's

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format(
                CultureInfo.InvariantCulture,
                "{0:00}:{1:00}:{2:00}.{3:00} (uur:min:sec.msec).",
                ts.Hours,
                ts.Minutes,
                ts.Seconds,
                ts.Milliseconds / 10);

            TdLogging.WriteToLogInformation(@"Het opslaan van de query('s) is gereed.");
            TdLogging.WriteToLogInformation(@"Het opslaan van de query('s) duurde : " + elapsedTime);
            TdLogging.WriteToLogInformation(string.Empty);

            Cursor.Current = Cursors.Default;
        }
        #endregion Save the folders and queries

        #region reset the treeview expansion state
        private void ToolStripMenuItemResetTreeviewBoom_Click(object sender, EventArgs e)
        {
            this.ResetTrvState();
        }

        private void ContextMenuStripMenuItemResetTreeviewTree_Click(object sender, EventArgs e)
        {
            this.ResetTrvState();
        }

        private void ResetTrvState()
        {
            this.TreeViewCreateQueries.BeginUpdate();
            this.DeleteTreeViewState();
            this.TreeViewCreateQueries.EndUpdate();
        }

        private void DeleteTreeViewState()
        {
            // Delete all treeview state data en collapse the tree to the rootnode
            this.SaveTreeviewState();  // First save the current treeview expansion state

            this.EmptyInputFields();

            if (
                    !string.IsNullOrEmpty(TdUserData.UserName) &&
                    !string.IsNullOrEmpty(TdUserData.UserId) &&
                    !string.IsNullOrEmpty(TdSettingsDefault.SqlLiteDatabaseName))
            {
                TdTreeViewExtensions.DeleteTreeViewStateReset(TdUserData.UserName, int.Parse(TdUserData.UserId, CultureInfo.InvariantCulture), this.TreeViewCreateQueries.Name, TdUserData.ConnectionId);
                this.TreeViewCreateQueries.CollapseAll();

                // Allways expand 1 level. TODOO make optional
                foreach (TreeNode tn in this.TreeViewCreateQueries.Nodes)
                {
                    tn.Expand();
                }
            }
        }
        #endregion reset the treeview expansion state

        private void FormMaintainQueries_FormClosed(object sender, FormClosedEventArgs e)
        {
            /*if (Parent.OraConn != null)
            {
                Parent.ReloadTreeView();
                Parent.ComboBoxQueryGroup.Text = string.Empty;
                TdLogging.WriteToLogInformation("Query beheer is afgesloten.");
            }*/
        }

        #region Delete query
        private void ToolStripMenuItemDeleteQuery_Click(object sender, EventArgs e)
        {
            bool warningOnDeleteQuery = this.JsonObjSettings.AppParam[0].WarningOnDeleteQuery;

            // Ask before delete query...
            if (warningOnDeleteQuery)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "De geselecteerde query wordt verwijderd." + Environment.NewLine +
                    Environment.NewLine +
                    "Weet u het zeker?",
                    MB_Title.Continue,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    TdLogging.WriteToLogInformation("De volgende query wordt verwijderd:");
                    TdLogging.WriteToLogInformation(this.selectedQuery.Name + " -> Id: " + this.selectedQuery.Id);

                    this.ButtonTestQuery.Enabled = false;
                    this.DeleteQuery();
                    Cursor.Current = Cursors.Default;
                }
                else if (dialogResult == DialogResult.No)
                {
                    TdLogging.WriteToLogInformation("Verwijderen Query is afgebroken.");
                }
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                TdLogging.WriteToLogInformation("Er wordt een Query verwijderd.");
                this.DeleteQuery();
                Cursor.Current = Cursors.Default;
            }
        }

        private void DeleteQuery()
        {
            this.TreeViewCreateQueries.BeginUpdate();
            this.SaveTreeviewState();

            // Unsaved data...
            if (this.nodesNotSaved.Count > 0)
            {
                DialogResult dlgsave = MessageBox.Show(
                    "De wijzigingen zijn (nog) niet opgeslagen." + Environment.NewLine +
                    "Wijzigingen opslaan en doorgaan met verwijderen?",
                    MB_Title.Continue,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (dlgsave == DialogResult.Yes)
                {
                    // First check if there are unsaved queries if yes then save.
                    while (this.nodesNotSaved.Count > 0)
                    {
                        this.SaveUpdateQueryOrFolder();
                    }

                    this.notSavedQueryOrFolder = false;

                    if (this.TreeViewCreateQueries.SelectedNode != null)
                    {
                        if (this.TreeViewCreateQueries.SelectedNode.Tag is TdQuery tdQ)
                        {
                            tdQ.DeleteQueryComplete();
                            this.LoadTreeViewFoldersAndQueries();
                            this.nodesNotSaved.Clear();
                        }
                    }
                    else if (this.SelectedNode != null)
                    {
                        if (this.SelectedNode.Tag is TdQuery tdQ)
                        {
                            tdQ.DeleteQueryComplete();
                            this.LoadTreeViewFoldersAndQueries();
                            this.nodesNotSaved.Clear();
                        }
                    }
                }
            }
            else
            {
                // If all query's are saved then delete the selected query
                if (this.TreeViewCreateQueries.SelectedNode.Tag is TdQuery tdQ)
                {
                    tdQ.DeleteQueryComplete();
                    this.LoadTreeViewFoldersAndQueries();
                    this.nodesNotSaved.Clear();
                }
            }

            this.LoadTreeviewState();
            this.TreeViewCreateQueries.EndUpdate();
        }
        #endregion delete query

        #region add query
        private void ContextMenuStripFolderBeheerAddQuery_Click(object sender, EventArgs e)
        {
            this.AddQuery();
            this.ButtonTestQuery.Enabled = false;
        }

        private void AddQuery()
        {
            TreeNode selectedNode = this.TreeViewCreateQueries.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode == this.TreeViewCreateQueries.TopNode || selectedNode.Tag is TdQuery)
                {
                    MessageBox.Show(ResourceText._2081, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                this.SelectedNode = selectedNode.Nodes.Add(null, ResourceText._2080);

                // Add new QueryItem to the node
                this.selectedQuery = new TdQuery(-1)
                {
                    QueryGuid = Guid.NewGuid().ToString(),
                    Name = this.SelectedNode.Text,
                };

                this.SelectedNode.Tag = this.selectedQuery;

                // Check for Parent node
                if (this.SelectedNode.Parent != this.TreeViewCreateQueries.TopNode)
                {
                    if (this.SelectedNode.Parent.Tag != null)
                    {
                        TdFolder tdF = (TdFolder)this.SelectedNode.Parent.Tag;
                        this.selectedQuery.Folder = tdF.FolderGuid;

                        // Show query data in "aangemaakt door" en "gewijzigd door" textboxes
                        this.selectedQuery.Created = DateTime.Now;
                        this.selectedQuery.CreatedBy = Environment.UserName;
                        this.selectedQuery.Modified = DateTime.Now;
                        this.selectedQuery.AlteredBy = Environment.UserName;
                    }
                }

                // Add Node to savelist and notify the savelist
                this.AddNotSavedNode(this.SelectedNode);

                // Uncheck all connections
                this.TreeViewConnecties_removeEventHandlers();   // Remove eventhandlers from the connection treeview to suppress warnings when checkbox are changed runtime
                for (int i = 0; i < this.TreeViewConnections.Nodes.Count; i++)
                {
                    this.TreeViewConnections.Nodes[i].Checked = false;
                }

                // Restore eventhandlers
                this.TreeViewConnecties_setEventHandlers();

                this.TextBoxName.Enabled = true;
                this.TextBoxDescription.Enabled = true;
                this.FastColoredSQLTextBox.Enabled = true;
                this.TextBoxOutputFileName.Enabled = true;
                this.TextBoxWorksheetName.Enabled = true;
                this.ComboBoxQueryAutorisation.Enabled = true;

                if (this.Parent.OraConn != null)
                {
                    this.TreeViewQueryGroup.Enabled = true;
                }

                if (TdUserData.UserRole == TdRoleTypes.System || TdUserData.UserRole == TdRoleTypes.Owner)
                {
                    this.CheckBoxLockQuery.Enabled = true;
                }
                else
                {
                    this.CheckBoxLockQuery.Enabled = false;
                }

                this.GetQueryDefaultText();  // Default query text which is comment text  for the query like data, name creator etc.

                bool showQueryGuideLines = this.JsonObjSettings.AppParam[0].ShowQueryGuideLines;
                if (showQueryGuideLines)
                {
                    this.GetQueryGuideLines();
                }

                this.ComboBoxQueryAutorisation.Text = TdResText.Viewer;
                this.Checkchanges();
                this.TreeViewCreateQueries.SelectedNode = this.SelectedNode;
            }
        }
        #endregion add query

        #region Add Folder
        private void ContextMenuStripFolderBeheerAddFolder_Click(object sender, EventArgs e)
        {
            this.AddFolder();    // Add a new folder
        }

        private void AddFolder()
        {
            TreeNode selectedNode = this.TreeViewCreateQueries.SelectedNode;

            if (selectedNode == this.TreeViewCreateQueries.TopNode || selectedNode.Tag is TdFolder)
            {
                this.SelectedNode = selectedNode.Nodes.Add(null, ResourceText._2079);

                // Add new QueryItem to the node
                this.selectedFolder = new TdFolder(-1) // TODO create a new constructor without loading the settings.
                {
                    FolderGuid = Guid.NewGuid().ToString(),
                    Name = this.SelectedNode.Text,
                    FolderType = FolderTypes.Folder,
                };
                this.SelectedNode.Tag = this.selectedFolder;

                // Check for Parent node
                if (this.SelectedNode.Parent != this.TreeViewCreateQueries.TopNode)
                {
                    if (this.SelectedNode.Parent.Tag != null)
                    {
                        TdFolder tdF = (TdFolder)this.SelectedNode.Parent.Tag;
                        this.selectedFolder.ParentFolder = tdF.FolderGuid;
                    }
                }

                // Add Node to savelist and notify savelist
                this.AddNotSavedNode(this.SelectedNode);

                this.TreeViewCreateQueries.SelectedNode = this.SelectedNode;

                // Uncheck all connections, a folder can not have a connection
                for (int i = 0; i < this.TreeViewConnections.Nodes.Count; i++)
                {
                    this.TreeViewConnections.Nodes[i].Checked = false;
                }

                this.EmptyInputFields();
                this.TextBoxName.Enabled = false;
                this.TextBoxDescription.Enabled = false;
                this.FastColoredSQLTextBox.Enabled = false;
                this.TextBoxOutputFileName.Enabled = false;
                this.TextBoxWorksheetName.Enabled = false;
                this.CheckBoxLockQuery.Enabled = false;
                this.ButtonSelectedFileOutput.Enabled = false;
                this.ComboBoxQueryAutorisation.Enabled = false;
                this.SelectedNode.BeginEdit();
                this.Checkchanges();
            }
        }

        #endregion Add Folder

        private void TreeViewConnecties_removeEventHandlers()
        {
            this.TreeViewConnections.BeforeCheck -= new TreeViewCancelEventHandler(this.TreeViewConnections_BeforeCheck);
            this.TreeViewConnections.AfterCheck -= new TreeViewEventHandler(this.TreeViewConnections_AfterCheck);
        }

        private void TreeViewConnecties_setEventHandlers()
        {
            this.TreeViewConnections.BeforeCheck += new TreeViewCancelEventHandler(this.TreeViewConnections_BeforeCheck);
            this.TreeViewConnections.AfterCheck += new TreeViewEventHandler(this.TreeViewConnections_AfterCheck);
        }

        #region Save
        private void ToolStripMenuItemSaveQuery_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.LabelQueryIsExecuted.Text = string.Empty;

            this.SaveTreeviewState();

            this.SaveQuery();
            this.ButtonSave.Enabled = false;
            if (this.selectedQuery != null)
            {
                this.selectedQuery.Id = 0;
            }

            Cursor.Current = Cursors.WaitCursor;
            this.LoadTreeviewState();            // Get the expand state of the treeview folders

            this.CreateAutoCompleteForSearchBox();

            this.LabelQueryIsExecuted.Text = "Opslaan is gereed.";

            Cursor.Current = Cursors.Default;
        }

        #endregion Save

        private void CreateAutoCompleteForSearchBox()
        {
            using TdAutoComplete aCompleteSource = new ();
            AutoCompleteStringCollection dataCollection;
            dataCollection = aCompleteSource.CreAutoCompleteListFromTrv(this.TreeViewCreateQueries);  // Create the autocomplete list for the search box
            this.TextBoxSearchInQueryTreeView.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.TextBoxSearchInQueryTreeView.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.TextBoxSearchInQueryTreeView.AutoCompleteCustomSource = dataCollection;
        }

        #region Delete a folder with its contents
        private void ContextMenuStripFolderBeheerDelete_Click(object sender, EventArgs e)
        {
            if (this.SelectedNode.Text != TdSettingsDefault.ApplicationName)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "De geselecteerde map met alle onderliggende query's wordt verwijderd." + Environment.NewLine +
                    Environment.NewLine +
                    "Weet u het zeker?",
                    MB_Title.Continue,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    TdLogging.WriteToLogInformation("Er wordt een map verwijderd.");
                    this.DeleteFolder();
                    Cursor.Current = Cursors.Default;
                }
                else if (dialogResult == DialogResult.No)
                {
                    TdLogging.WriteToLogInformation("Verwijderen map is afgebroken.");
                }
            }
            else
            {
                MessageBox.Show(string.Format("{0} kan niet worden verwijderd.", TdSettingsDefault.ApplicationName), MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DeleteFolder()
        {
            this.TreeViewCreateQueries.BeginUpdate();
            this.SaveTreeviewState();

            // Unsaved data...
            if (this.nodesNotSaved.Count > 0)
            {
                DialogResult dlgsave = MessageBox.Show(
                    "De wijzigingen zijn (nog) niet opgeslagen." + Environment.NewLine +
                    "Wijzigingen opslaan en doorgaan met verwijderen?",
                    MB_Title.Continue,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (dlgsave == DialogResult.Yes)
                {
                    // First check if there are unsaved query',s if yes then save.
                    while (this.nodesNotSaved.Count > 0)
                    {
                        this.SaveUpdateQuery();
                    }

                    // Then save the folders
                    if (this.TreeViewCreateQueries.SelectedNode.Tag is TdFolder tdF)
                    {
                        tdF.DeleteFolderComplete(TdSettingsDefault.SqlLiteDatabaseName);
                        this.nodesNotSaved.Clear();
                    }

                    this.LoadTreeViewFoldersAndQueries();
                }
            }
            else
            {
                // There is no unsaved data, delete the folder without the need of saving queries
                if (this.TreeViewCreateQueries.SelectedNode.Tag is TdFolder tdF)
                {
                    tdF.DeleteFolderComplete(TdSettingsDefault.SqlLiteDatabaseName);
                    this.LoadTreeViewFoldersAndQueries();
                    this.nodesNotSaved.Clear();
                }
            }

            this.LoadTreeviewState();
            this.TreeViewCreateQueries.EndUpdate();
        }

        private void SaveUpdateQuery()
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (TreeNode node in this.nodesNotSaved)
            {
                if (node.Tag is TdFolder tdF)
                {
                    tdF.SaveFolder();  // Save the folder
                }
                else if (node.Tag is TdQuery tdQ)
                {
                    tdQ.SaveQuery(); // Save the query
                }
            }

            this.nodesNotSaved.Clear();
            this.notSavedQueryOrFolder = false;

            foreach (TreeNode node in this.TreeViewCreateQueries.Nodes)
            {
                if (node.Tag is TdFolder folder)
                {
                    if (this.selectedFolder != null)
                    {
                        TdFolder tdF = folder;
                        if (tdF.FolderGuid == this.selectedFolder.FolderGuid)
                        {
                            this.TreeViewCreateQueries.SelectedNode = node;
                        }
                    }
                }

                if (node.Tag is TdQuery query)
                {
                    if (this.selectedQuery != null)
                    {
                        TdQuery tdQ = query;
                        if (tdQ.QueryGuid == this.selectedQuery.QueryGuid)
                        {
                            this.TreeViewCreateQueries.SelectedNode = node;
                        }
                    }
                }

                this.SelectNodeRecursive(node);
            }

            Cursor.Current = Cursors.Default;
        }

        private bool SelectNodeRecursive(TreeNode iterNode)
        {
            bool returnValue = false;

            if (iterNode == null)
            {
                return returnValue;
            }

            foreach (TreeNode tn in iterNode.Nodes)
            {
                if (tn.Tag is TdFolder tdF && this.selectedFolder != null)
                {
                    if (tdF.FolderGuid == this.selectedFolder.FolderGuid)
                    {
                        this.TreeViewCreateQueries.SelectedNode = tn;
                        returnValue = true;
                    }
                }
                else if (tn.Tag is TdQuery tdQ && this.selectedQuery != null)
                {
                    if (tdQ.QueryGuid == this.selectedQuery.QueryGuid)
                    {
                        this.TreeViewCreateQueries.SelectedNode = tn;
                        returnValue = true;
                    }
                }

                this.SelectNodeRecursive(tn);
            }

            return returnValue;
        }
        #endregion Delete a folder with its contents

        private void ContextMenuStripMenuItemExpandAll_Click(object sender, EventArgs e)
        {
            this.TreeViewCreateQueries.ExpandAll();
        }

        private void ContextMenuStripMenuItemCollapseAll_Click(object sender, EventArgs e)
        {
            this.TreeViewCreateQueries.CollapseAll();
        }

        private void ToolStripMenuItemCollapseAll_Click(object sender, EventArgs e)
        {
            this.TreeViewCreateQueries.CollapseAll();
        }

        private void ToolStripMenuItemTrvExpandAll_Click(object sender, EventArgs e)
        {
            this.TreeViewCreateQueries.ExpandAll();
        }

        #region Export all Query's
        private void ToolStripMenuItemExportAllQueries_Click(object sender, EventArgs e)
        {
            // Export all Queries to textfiles
            if (TdUserData.UserRole == TdRoleTypes.Owner || TdUserData.UserRole == TdRoleTypes.System)
            {
                this.TreeViewCreateQueries.BeginUpdate();
                this.SaveTreeviewState();
                this.ExportAllQueries();
                this.TextControls_removeEventHandlers(); // Disable the event handlers from the textboxes.
                this.LoadTreeViewFoldersAndQueries();    // Get the folders and queries and show them in the treeview
                this.LoadTreeviewState();                // Get the expand state of the treeview folders
                this.TextControls_addEventHandlers();    // Enable the event handlers from the textboxes
                this.TreeViewCreateQueries.EndUpdate();
            }
        }

        private void ExportAllQueries()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool error = false;

            CreateExportFolder();

            // Save Queries in a temp folder
            if (this.SQLLiteApp != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                TdQueries queries = this.SQLLiteApp.GetQueries();
                try
                {
                    foreach (TdQuery qtd in queries.Items)
                    {
                        TreeNode node = this.TreeViewCreateQueries.Nodes.Add(qtd.Name);
                        node.Tag = qtd;

                        try
                        {
                            if (!string.IsNullOrEmpty(qtd.Name))
                            {
                                string file = qtd.Name;
                                file = RemoveSpecialCharacters(file);

                                string folderFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName, TdSettingsDefault.ExportTempFolder) + file + ".txt";

                                if (!File.Exists(folderFile))
                                {
                                    using var writer = new StreamWriter(folderFile);
                                    writer.Write(qtd.Query);
                                }
                                else
                                {
                                    string folder = Path.GetDirectoryName(folderFile);
                                    string fileName = Path.GetFileName(folderFile);

                                    SaveFileDialog saveFileDialog1 = new ();
                                    saveFileDialog1.Filter = "Text File|*.txt";
                                    saveFileDialog1.Title = "Geeft het bestand een nieuwe naam. " + fileName + " bestaat al.";
                                    saveFileDialog1.InitialDirectory = folder;

                                    saveFileDialog1.FileName = folderFile;
                                    saveFileDialog1.ShowDialog();
                                    using var writer = new StreamWriter(saveFileDialog1.FileName);
                                    writer.Write(qtd.Query);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            TdLogging.WriteToLogError("De volgende query is niet geëxporteerd.");
                            TdLogging.WriteToLogError(qtd.Name);
                            TdLogging.WriteToLogError("Melding:");
                            TdLogging.WriteToLogError(ex.Message);
                            if (TdDebugMode.DebugMode)
                            {
                                TdLogging.WriteToLogDebug(ex.ToString());
                            }

                            error = true;
                        }
                    }

                    TdLogging.WriteToLogInformation("Alle Queries zijn geëxporteerd. (Gebruiker : " + TdUserData.UserName + ")");

                    MessageBox.Show(
                        "Alle Queries zijn geëxporteerd." + Environment.NewLine +
                        Environment.NewLine +
                        "Let op, de bestanden worden verwijderd als het query scherm wordt gesloten.",
                        MB_Title.Ready,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
                catch (Exception ex)
                {
                    TdLogging.WriteToLogError(ex.Message);
                }
            }

            Cursor.Current = Cursors.Default;
            if (error)
            {
                MessageBox.Show(MB_Text.Query_Not_Exported, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Remove secial characters from a query name before saveing the query. Queryname = filename.
        /// </summary>
        /// <param name="str">The query string.</param>
        /// <returns>String without special chars.</returns>
        public static string RemoveSpecialCharacters(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            StringBuilder sb = new ();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9')
                    || (str[i] >= 'A' && str[i] <= 'z')
                        || str[i] == '.' || str[i] == '_')
                {
                    sb.Append(str[i]);
                }
            }

            return sb.ToString();
        }

        private static void CreateExportFolder()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName, TdSettingsDefault.ExportTempFolder);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private static void DeleteExport_tmp()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName, TdSettingsDefault.ExportTempFolder);
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }

        private void ToolStripMenuItemExportMapQueryNames_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new ()
            {
                Filter = "Tekst bestand|*.txt",
                Title = "Opslaan alle map- en query namen.",
            };
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != string.Empty)
            {
                Cursor.Current = Cursors.WaitCursor;
                TdLogging.WriteToLogInformation("Alle map- en querynamen exporteren naar een tekst bestand.");
                this.TreeViewCreateQueries.BeginUpdate();
                this.SaveTreeviewState();
                this.TreeViewCreateQueries.ExpandAll();

                StringBuilder buffer = new (); // Create buffer for storing string data

                // Loop through each of the treeview's root nodes
                foreach (TreeNode rootNode in this.TreeViewCreateQueries.Nodes)
                {
                    this.BuildTreeString(rootNode, buffer);  // Call recursive function
                }

                File.WriteAllText(saveFileDialog1.FileName, buffer.ToString());

                this.TreeViewCreateQueries.CollapseAll();
                this.LoadTreeviewState();
                this.TreeViewCreateQueries.EndUpdate();
                MessageBox.Show("Opslaan alle map- en querynamen is gereed.", MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cursor.Current = Cursors.Default;
            }
        }

        private void BuildTreeString(TreeNode rootNode, StringBuilder buffer, int level = 0)
        {
            buffer.Append(new string(' ', level * 3));
            buffer.Append(rootNode.Text);
            buffer.Append(Environment.NewLine);
            foreach (TreeNode childNode in rootNode.Nodes)
            {
                this.BuildTreeString(childNode, buffer, level + 1);
            }
        }

        #endregion Export all Query's

        #region Copy Database
        private void ToolStripMenuItemCopyDatabase_Click(object sender, EventArgs e)
        {
            string initDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName);
            SaveFileDialog saveFileDialog1 = new ()
            {
                Filter = ResourceText._3002,
                DefaultExt = "db",
                InitialDirectory = initDir,
                Title = ResourceText._3001,
            };
            saveFileDialog1.ShowDialog();

            if (!string.IsNullOrEmpty(saveFileDialog1.FileName))
            {
                using TdAppDbMaintain appDbMaintain = new ();
                if (!appDbMaintain.CopyDatabaseFileWithoutCompress(saveFileDialog1.FileName, true))
                {
                    MessageBox.Show(MB_Text.Database_Copy_Failed, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(MB_Text.Copy_DatabaseFile_Completed, MB_Title.Ready, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            saveFileDialog1.Dispose();
        }
        #endregion Copy Database

        #region Default Query text
        private void GetQueryDefaultText()
        {
            using TdDefaultQueryComment loadText = new (TdUserData.UserName, int.Parse(TdUserData.UserId));
            this.FastColoredSQLTextBox.Text = loadText.LoadDefaultQueryStartText();
            loadText.Dispose();
        }

        private void GetQueryGuideLines()
        {
            // Query guide lines
            this.FastColoredSQLTextBox.Text += Environment.NewLine + Environment.NewLine +
                 "TopData (Versie : " + TdSettingsDefault.ApplicationVersion + ")" + Environment.NewLine +
                 "BAR-Organisatie" + Environment.NewLine + Environment.NewLine +
                 DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + Environment.NewLine + Environment.NewLine +
                "Richtlijnen queries:" + Environment.NewLine + Environment.NewLine +
                @"Commentaar m.b.v. ""--""  wordt ondersteund." + Environment.NewLine +
                @"Commentaar m.b.v. /*  .. */ wordt ondersteund." + Environment.NewLine + Environment.NewLine +

                "Beëindig een query met ^ (i.p.v. ; )" + Environment.NewLine + Environment.NewLine +
                "Een parameter in een query moet beginnen en eindigen met een #" + Environment.NewLine +
                "Bijvoorbeeld : #Tabel_naam:_#" + Environment.NewLine +
                @"De tekst tussen de ""#"" wordt gebruikt in het parameter scherm waarbij de _ worden vervangen door spaties." + Environment.NewLine +
                Environment.NewLine + Environment.NewLine +
                @"Queries die een foutcode kunnen genereren zoals een ""Drop table"" moeten in een korte PL/SQL procedure worden gezet zodat de eventuele exception wordt afgevangen." + Environment.NewLine +
                Environment.NewLine + Environment.NewLine;
        }

        private void ToolStripMenuItemDefaultQueryText_Click(object sender, EventArgs e)
        {
            FormMaintainQueryDefaultText frm = new ();
            frm.ShowDialog(this);
            frm.Dispose();
        }
        #endregion Default Query text

        private void ButtonSelectedFileOutput_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogExportFile = new ()
            {
                FileName = this.TextBoxOutputFileName.Text,
                Filter = ResourceText._3000,
                AddExtension = true,
                CheckFileExists = true,
                InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName),
            };
            Cursor.Current = Cursors.Default;

            if (openFileDialogExportFile.ShowDialog() == DialogResult.OK)
            {
                this.TextBoxOutputFileName.Text = openFileDialogExportFile.FileName;
                this.Checkchanges();
            }

            openFileDialogExportFile.Dispose();
        }

        #region QuerygroupNames
        private void TreeViewQueryGroup_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (this.selectedQuery != null)
            {
                TdQuery tdQ = new(this.selectedQuery.Id);
                foreach (TreeNode node in this.TreeViewQueryGroup.Nodes)
                {
                    if (node.Checked)
                    {
                        // List.Contains
                        if (!this.selectedQuery.QueryGroupNames.Contains(node.Text))
                        {
                            this.selectedQuery.QueryGroupNames.Add(node.Text);  // Add the querygroup to the list
                            this.selectedQuery.QueryGroup = node.Text;          // Needed to set querygroup name in the query_list table
                        }
                    }
                    else
                    {
                        if (this.selectedQuery.QueryGroupNames.Contains(node.Text))
                        {
                            this.selectedQuery.QueryGroupNames.Remove(node.Text);
                        }

                        this.selectedQuery.QueryGroup = string.Empty;           // Needed to set querygroup name in the query_list table
                    }
                }

                // Save the querygroup relation
                if (this.selectedQuery.QueryGroupNames.Count == 0)
                {
                    tdQ.DeleteQuerGroupIDs(string.Empty);
                }
                else
                {
                    foreach (string qgrp in this.selectedQuery.QueryGroupNames)
                    {
                        tdQ.QueryGroupNames.Add(qgrp);

                        tdQ.SaveQueryGroup();
                    }
                }

                this.Checkchanges();    /* Needed because the querygroup name will be stored in query-list. so a change in checked/unchecked must set _notSavedQuerYOrFolder and fillnodesNotSaved
                                           There is no need to chech change. it is saved immediately!*/
            }

            Cursor.Current = Cursors.Default;
        }

        private void LoadQueryGroupNames()
        {
            this.TreeViewQueryGroup.BeginUpdate();
            this.TreeViewQueryGroup.Nodes.Clear();

            TdMaintainQueryGroups qGroup = new ();
            qGroup.GetAllQueryGroupNames();

            foreach (KeyValuePair<int, string> entry in qGroup.Querygroups)
            {
                this.TreeViewQueryGroup.Nodes.Add(entry.Value);
            }

            this.TreeViewQueryGroup.EndUpdate();
        }
        #endregion QuerygroupNames

        private void ToolStripMenuItemQueryGroup_Click(object sender, EventArgs e)
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
                this.LoadQueryGroupNames();
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
                MessageBox.Show(
                    "Fout bij het laden van de instellingen. " + Environment.NewLine +
                    "De default instellingen worden ingeladen.", MB_Title.Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        #region test query
        private void ToolStripMenuItemTestQuery_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.SaveSettings();  // when the parameter form is used then the position of this form can change. first save the current settings

            this.TestQuery();

            this.LoadSettings();  // when the parameter form is used then the position of this form can change. load the settings
            Cursor.Current = Cursors.Default;
        }

        private void TestQuery()
        {
            if (TdDebugMode.DebugMode)
            {
                TdLogging.WriteToLogDebug("Bezig met het testen van query " + this.TextBoxName.Text + "...");
            }

            this.Parent.ToolStripStatusLabel1.Text = "Bezig met het testen van de query " + this.TextBoxName.Text + "...";
            this.Parent.Refresh();
            this.LabelQueryIsExecuted.Text = "Bezig met het testen van de query " + this.TextBoxName.Text + "...";
            this.TextBoxOraWarning.Visible = false;
            this.TextBoxOraWarning.Clear();
            this.Refresh();

            using TdExecuteQueries queryExec = new(this.Parent.OraConn);
            queryExec.TrvNode = this.TreeViewCreateQueries.SelectedNode as TreeNode;
            queryExec.Dgv = this.Parent.DataGridViewQueries;
            queryExec.Parent = this.Parent;
            queryExec.TestaQuery();  // Run the query to see if it works

            if (queryExec.QueryIsExecuted == "Succes")
            {
                this.LabelQueryIsExecuted.Text = "Query is succesvol uitgevoerd.";
                this.Parent.ToolStripStatusLabel1.Text = string.Empty;
            }
            else
            {
                this.TextBoxOraWarning.Visible = true;
                this.LabelQueryIsExecuted.Text = "Query is niet succesvol uitgevoerd.";
                this.TextBoxOraWarning.Text = queryExec.QueryIsExecuted;
                this.Parent.ToolStripStatusLabel1.Text = string.Empty;
            }

            this.Refresh();
        }
        #endregion test query

        private void TreeViewConnections_Leave(object sender, EventArgs e)
        {
            this.Checkchanges();
        }

        private void TreeViewQueryGroup_Leave(object sender, EventArgs e)
        {
            this.Checkchanges();
        }
    }
}