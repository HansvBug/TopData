namespace TopData
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.MenuStripMainForm = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDatabaseConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDatabaseDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemChangeUser = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemClose = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemMaintain = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemMaintainQueries = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemMaintainDbConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemmaintainUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemMaintainQueryGroups = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemConfigure = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLanguageDutch = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLanguageEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.PanelHeaderMainForm = new System.Windows.Forms.Panel();
            this.CheckBoxShowGeometryField = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ComboBoxQueryGroup = new System.Windows.Forms.ComboBox();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPageQueries = new System.Windows.Forms.TabPage();
            this.SplitContainer1FormMain = new System.Windows.Forms.SplitContainer();
            this.SplitContainerQueryTree = new System.Windows.Forms.SplitContainer();
            this.ButtonSearchQueryName = new System.Windows.Forms.Button();
            this.LabelTrvSearchFound = new System.Windows.Forms.Label();
            this.TreeViewExecuteQueries = new System.Windows.Forms.TreeView();
            this.ButtonRunQuery = new System.Windows.Forms.Button();
            this.TextBoxSearchInQueryTreeView = new System.Windows.Forms.TextBox();
            this.RichTextBoxQueryDescription = new System.Windows.Forms.RichTextBox();
            this.ButtonRemoveSelection = new System.Windows.Forms.Button();
            this.ButtonRunQueryAndExport = new System.Windows.Forms.Button();
            this.ButtonExport = new System.Windows.Forms.Button();
            this.DataGridViewQueries = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ButtonExportDomeinwaardeCTRL = new System.Windows.Forms.Button();
            this.ButtonGetHelpTable = new System.Windows.Forms.Button();
            this.GroupBoxDomainValues = new System.Windows.Forms.GroupBox();
            this.GroupBoxPassports = new System.Windows.Forms.GroupBox();
            this.GroupBoxPrepare = new System.Windows.Forms.GroupBox();
            this.ComboBoxGBIbasisConnName = new System.Windows.Forms.ComboBox();
            this.ComboBoxGBIsystemConnName = new System.Windows.Forms.ComboBox();
            this.ProgressBarExport = new System.Windows.Forms.ProgressBar();
            this.MenuStripMainForm.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.PanelHeaderMainForm.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.TabPageQueries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1FormMain)).BeginInit();
            this.SplitContainer1FormMain.Panel1.SuspendLayout();
            this.SplitContainer1FormMain.Panel2.SuspendLayout();
            this.SplitContainer1FormMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerQueryTree)).BeginInit();
            this.SplitContainerQueryTree.Panel1.SuspendLayout();
            this.SplitContainerQueryTree.Panel2.SuspendLayout();
            this.SplitContainerQueryTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewQueries)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.GroupBoxPrepare.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStripMainForm
            // 
            this.MenuStripMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemProgram,
            this.ToolStripMenuItemMaintain,
            this.ToolStripMenuItemOptions});
            this.MenuStripMainForm.Location = new System.Drawing.Point(0, 0);
            this.MenuStripMainForm.Name = "MenuStripMainForm";
            this.MenuStripMainForm.Size = new System.Drawing.Size(889, 24);
            this.MenuStripMainForm.TabIndex = 0;
            this.MenuStripMainForm.Text = "menuStrip1";
            // 
            // ToolStripMenuItemProgram
            // 
            this.ToolStripMenuItemProgram.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemDatabaseConnection,
            this.ToolStripMenuItemDatabaseDisconnect,
            this.toolStripSeparator1,
            this.ToolStripMenuItemChangeUser,
            this.toolStripSeparator2,
            this.ToolStripMenuItemClose});
            this.ToolStripMenuItemProgram.Name = "ToolStripMenuItemProgram";
            this.ToolStripMenuItemProgram.Size = new System.Drawing.Size(65, 20);
            this.ToolStripMenuItemProgram.Text = "&Program";
            // 
            // ToolStripMenuItemDatabaseConnection
            // 
            this.ToolStripMenuItemDatabaseConnection.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemDatabaseConnection.Image")));
            this.ToolStripMenuItemDatabaseConnection.Name = "ToolStripMenuItemDatabaseConnection";
            this.ToolStripMenuItemDatabaseConnection.Size = new System.Drawing.Size(288, 22);
            this.ToolStripMenuItemDatabaseConnection.Text = "Database connection(s)";
            // 
            // ToolStripMenuItemDatabaseDisconnect
            // 
            this.ToolStripMenuItemDatabaseDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemDatabaseDisconnect.Image")));
            this.ToolStripMenuItemDatabaseDisconnect.Name = "ToolStripMenuItemDatabaseDisconnect";
            this.ToolStripMenuItemDatabaseDisconnect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.ToolStripMenuItemDatabaseDisconnect.Size = new System.Drawing.Size(288, 22);
            this.ToolStripMenuItemDatabaseDisconnect.Text = "Disconnect database connection";
            this.ToolStripMenuItemDatabaseDisconnect.Click += new System.EventHandler(this.ToolStripMenuItemDatabaseDisconnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(285, 6);
            // 
            // ToolStripMenuItemChangeUser
            // 
            this.ToolStripMenuItemChangeUser.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemChangeUser.Image")));
            this.ToolStripMenuItemChangeUser.Name = "ToolStripMenuItemChangeUser";
            this.ToolStripMenuItemChangeUser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.ToolStripMenuItemChangeUser.Size = new System.Drawing.Size(288, 22);
            this.ToolStripMenuItemChangeUser.Text = "Switch &user";
            this.ToolStripMenuItemChangeUser.Click += new System.EventHandler(this.ToolStripMenuItemChangeUser_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(285, 6);
            // 
            // ToolStripMenuItemClose
            // 
            this.ToolStripMenuItemClose.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemClose.Image")));
            this.ToolStripMenuItemClose.Name = "ToolStripMenuItemClose";
            this.ToolStripMenuItemClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ToolStripMenuItemClose.Size = new System.Drawing.Size(288, 22);
            this.ToolStripMenuItemClose.Text = "Exit";
            this.ToolStripMenuItemClose.Click += new System.EventHandler(this.ToolStripMenuItemClose_Click);
            // 
            // ToolStripMenuItemMaintain
            // 
            this.ToolStripMenuItemMaintain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemMaintainQueries,
            this.ToolStripMenuItemMaintainDbConnections,
            this.ToolStripMenuItemmaintainUsers,
            this.ToolStripMenuItemMaintainQueryGroups});
            this.ToolStripMenuItemMaintain.Name = "ToolStripMenuItemMaintain";
            this.ToolStripMenuItemMaintain.Size = new System.Drawing.Size(62, 20);
            this.ToolStripMenuItemMaintain.Text = "&Manage";
            // 
            // ToolStripMenuItemMaintainQueries
            // 
            this.ToolStripMenuItemMaintainQueries.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemMaintainQueries.Image")));
            this.ToolStripMenuItemMaintainQueries.Name = "ToolStripMenuItemMaintainQueries";
            this.ToolStripMenuItemMaintainQueries.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemMaintainQueries.Text = "&Queries ...";
            this.ToolStripMenuItemMaintainQueries.Click += new System.EventHandler(this.ToolStripMenuItemMaintainQueries_Click);
            // 
            // ToolStripMenuItemMaintainDbConnections
            // 
            this.ToolStripMenuItemMaintainDbConnections.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemMaintainDbConnections.Image")));
            this.ToolStripMenuItemMaintainDbConnections.Name = "ToolStripMenuItemMaintainDbConnections";
            this.ToolStripMenuItemMaintainDbConnections.Size = new System.Drawing.Size(155, 22);
            this.ToolStripMenuItemMaintainDbConnections.Text = "C&onnections";
            this.ToolStripMenuItemMaintainDbConnections.Click += new System.EventHandler(this.ToolStripMenuItemMaintainDbConnections_Click);
            // 
            // ToolStripMenuItemmaintainUsers
            // 
            this.ToolStripMenuItemmaintainUsers.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemmaintainUsers.Image")));
            this.ToolStripMenuItemmaintainUsers.Name = "ToolStripMenuItemmaintainUsers";
            this.ToolStripMenuItemmaintainUsers.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemmaintainUsers.Text = "&Users...";
            this.ToolStripMenuItemmaintainUsers.Click += new System.EventHandler(this.ToolStripMenuItemmaintainUsers_Click);
            // 
            // ToolStripMenuItemMaintainQueryGroups
            // 
            this.ToolStripMenuItemMaintainQueryGroups.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemMaintainQueryGroups.Image")));
            this.ToolStripMenuItemMaintainQueryGroups.Name = "ToolStripMenuItemMaintainQueryGroups";
            this.ToolStripMenuItemMaintainQueryGroups.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemMaintainQueryGroups.Text = "Query &groups...";
            this.ToolStripMenuItemMaintainQueryGroups.Click += new System.EventHandler(this.ToolStripMenuItemMaintainQueryGroups_Click);
            // 
            // ToolStripMenuItemOptions
            // 
            this.ToolStripMenuItemOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemConfigure,
            this.ToolStripMenuItemLanguage,
            this.ToolStripMenuItemAbout});
            this.ToolStripMenuItemOptions.Name = "ToolStripMenuItemOptions";
            this.ToolStripMenuItemOptions.Size = new System.Drawing.Size(61, 20);
            this.ToolStripMenuItemOptions.Text = "&Options";
            // 
            // ToolStripMenuItemConfigure
            // 
            this.ToolStripMenuItemConfigure.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemConfigure.Image")));
            this.ToolStripMenuItemConfigure.Name = "ToolStripMenuItemConfigure";
            this.ToolStripMenuItemConfigure.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemConfigure.Text = "&Settings...";
            this.ToolStripMenuItemConfigure.Click += new System.EventHandler(this.ToolStripMenuItemConfigure_Click);
            // 
            // ToolStripMenuItemLanguage
            // 
            this.ToolStripMenuItemLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemLanguageDutch,
            this.ToolStripMenuItemLanguageEnglish});
            this.ToolStripMenuItemLanguage.Name = "ToolStripMenuItemLanguage";
            this.ToolStripMenuItemLanguage.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemLanguage.Text = "&Language";
            // 
            // ToolStripMenuItemLanguageDutch
            // 
            this.ToolStripMenuItemLanguageDutch.Name = "ToolStripMenuItemLanguageDutch";
            this.ToolStripMenuItemLanguageDutch.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemLanguageDutch.Text = "&Dutch";
            this.ToolStripMenuItemLanguageDutch.Click += new System.EventHandler(this.ToolStripMenuItemLanguageDutch_Click);
            // 
            // ToolStripMenuItemLanguageEnglish
            // 
            this.ToolStripMenuItemLanguageEnglish.Name = "ToolStripMenuItemLanguageEnglish";
            this.ToolStripMenuItemLanguageEnglish.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemLanguageEnglish.Text = "&English";
            this.ToolStripMenuItemLanguageEnglish.Click += new System.EventHandler(this.ToolStripMenuItemLanguageEnglish_Click);
            // 
            // ToolStripMenuItemAbout
            // 
            this.ToolStripMenuItemAbout.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItemAbout.Image")));
            this.ToolStripMenuItemAbout.Name = "ToolStripMenuItemAbout";
            this.ToolStripMenuItemAbout.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemAbout.Text = "&Information";
            this.ToolStripMenuItemAbout.Click += new System.EventHandler(this.ToolStripMenuItemAbout_Click_1);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1,
            this.ToolStripStatusLabel2,
            this.ToolStripStatusLabel3,
            this.ToolStripStatusLabel4});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 493);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(889, 22);
            this.StatusStrip1.TabIndex = 1;
            this.StatusStrip1.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            this.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToolStripStatusLabel2
            // 
            this.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2";
            this.ToolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // ToolStripStatusLabel3
            // 
            this.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3";
            this.ToolStripStatusLabel3.Size = new System.Drawing.Size(874, 17);
            this.ToolStripStatusLabel3.Spring = true;
            this.ToolStripStatusLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ToolStripStatusLabel4
            // 
            this.ToolStripStatusLabel4.Name = "ToolStripStatusLabel4";
            this.ToolStripStatusLabel4.Size = new System.Drawing.Size(0, 17);
            this.ToolStripStatusLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PanelHeaderMainForm
            // 
            this.PanelHeaderMainForm.Controls.Add(this.CheckBoxShowGeometryField);
            this.PanelHeaderMainForm.Controls.Add(this.button3);
            this.PanelHeaderMainForm.Controls.Add(this.button2);
            this.PanelHeaderMainForm.Controls.Add(this.button1);
            this.PanelHeaderMainForm.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelHeaderMainForm.Location = new System.Drawing.Point(0, 24);
            this.PanelHeaderMainForm.Name = "PanelHeaderMainForm";
            this.PanelHeaderMainForm.Size = new System.Drawing.Size(889, 50);
            this.PanelHeaderMainForm.TabIndex = 2;
            // 
            // CheckBoxShowGeometryField
            // 
            this.CheckBoxShowGeometryField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckBoxShowGeometryField.AutoSize = true;
            this.CheckBoxShowGeometryField.Location = new System.Drawing.Point(740, 12);
            this.CheckBoxShowGeometryField.Name = "CheckBoxShowGeometryField";
            this.CheckBoxShowGeometryField.Size = new System.Drawing.Size(137, 19);
            this.CheckBoxShowGeometryField.TabIndex = 3;
            this.CheckBoxShowGeometryField.Text = "Toon Geometrie veld";
            this.CheckBoxShowGeometryField.UseVisualStyleBackColor = true;
            this.CheckBoxShowGeometryField.Click += new System.EventHandler(this.CheckBoxShowGeometryField_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(468, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(218, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "save XML to machine key container";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(303, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "save keycontiner to file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(222, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "maak keycontainer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ComboBoxQueryGroup
            // 
            this.ComboBoxQueryGroup.FormattingEnabled = true;
            this.ComboBoxQueryGroup.Location = new System.Drawing.Point(165, 4);
            this.ComboBoxQueryGroup.Name = "ComboBoxQueryGroup";
            this.ComboBoxQueryGroup.Size = new System.Drawing.Size(121, 23);
            this.ComboBoxQueryGroup.TabIndex = 5;
            this.ComboBoxQueryGroup.Text = "<Query groep>";
            this.ComboBoxQueryGroup.DropDown += new System.EventHandler(this.ComboBoxQueryGroup_DropDown);
            this.ComboBoxQueryGroup.SelectedIndexChanged += new System.EventHandler(this.ComboBoxQueryGroup_SelectedIndexChanged);
            this.ComboBoxQueryGroup.DropDownClosed += new System.EventHandler(this.ComboBoxQueryGroup_DropDownClosed);
            this.ComboBoxQueryGroup.TextChanged += new System.EventHandler(this.ComboBoxQueryGroup_TextChanged);
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPageQueries);
            this.TabControl1.Controls.Add(this.tabPage2);
            this.TabControl1.Controls.Add(this.tabPage1);
            this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl1.Location = new System.Drawing.Point(0, 74);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(889, 419);
            this.TabControl1.TabIndex = 3;
            // 
            // TabPageQueries
            // 
            this.TabPageQueries.Controls.Add(this.SplitContainer1FormMain);
            this.TabPageQueries.Location = new System.Drawing.Point(4, 24);
            this.TabPageQueries.Name = "TabPageQueries";
            this.TabPageQueries.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageQueries.Size = new System.Drawing.Size(881, 391);
            this.TabPageQueries.TabIndex = 0;
            this.TabPageQueries.Text = "Query\'s";
            this.TabPageQueries.UseVisualStyleBackColor = true;
            // 
            // SplitContainer1FormMain
            // 
            this.SplitContainer1FormMain.BackColor = System.Drawing.SystemColors.Window;
            this.SplitContainer1FormMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainer1FormMain.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.SplitContainer1FormMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1FormMain.Location = new System.Drawing.Point(3, 3);
            this.SplitContainer1FormMain.Name = "SplitContainer1FormMain";
            // 
            // SplitContainer1FormMain.Panel1
            // 
            this.SplitContainer1FormMain.Panel1.Controls.Add(this.SplitContainerQueryTree);
            // 
            // SplitContainer1FormMain.Panel2
            // 
            this.SplitContainer1FormMain.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.SplitContainer1FormMain.Panel2.Controls.Add(this.ButtonRemoveSelection);
            this.SplitContainer1FormMain.Panel2.Controls.Add(this.ComboBoxQueryGroup);
            this.SplitContainer1FormMain.Panel2.Controls.Add(this.ButtonRunQueryAndExport);
            this.SplitContainer1FormMain.Panel2.Controls.Add(this.ButtonExport);
            this.SplitContainer1FormMain.Panel2.Controls.Add(this.DataGridViewQueries);
            this.SplitContainer1FormMain.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.SplitContainer1FormMain.Size = new System.Drawing.Size(875, 385);
            this.SplitContainer1FormMain.SplitterDistance = 234;
            this.SplitContainer1FormMain.SplitterWidth = 10;
            this.SplitContainer1FormMain.TabIndex = 4;
            this.SplitContainer1FormMain.Paint += new System.Windows.Forms.PaintEventHandler(this.SplitContainer1FormMain_Paint);
            // 
            // SplitContainerQueryTree
            // 
            this.SplitContainerQueryTree.BackColor = System.Drawing.Color.Transparent;
            this.SplitContainerQueryTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainerQueryTree.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.SplitContainerQueryTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainerQueryTree.Location = new System.Drawing.Point(0, 0);
            this.SplitContainerQueryTree.Name = "SplitContainerQueryTree";
            this.SplitContainerQueryTree.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainerQueryTree.Panel1
            // 
            this.SplitContainerQueryTree.Panel1.BackColor = System.Drawing.SystemColors.Window;
            this.SplitContainerQueryTree.Panel1.Controls.Add(this.ButtonSearchQueryName);
            this.SplitContainerQueryTree.Panel1.Controls.Add(this.LabelTrvSearchFound);
            this.SplitContainerQueryTree.Panel1.Controls.Add(this.TreeViewExecuteQueries);
            this.SplitContainerQueryTree.Panel1.Controls.Add(this.ButtonRunQuery);
            this.SplitContainerQueryTree.Panel1.Controls.Add(this.TextBoxSearchInQueryTreeView);
            this.SplitContainerQueryTree.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // SplitContainerQueryTree.Panel2
            // 
            this.SplitContainerQueryTree.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.SplitContainerQueryTree.Panel2.Controls.Add(this.RichTextBoxQueryDescription);
            this.SplitContainerQueryTree.Size = new System.Drawing.Size(234, 385);
            this.SplitContainerQueryTree.SplitterDistance = 298;
            this.SplitContainerQueryTree.SplitterWidth = 10;
            this.SplitContainerQueryTree.TabIndex = 0;
            this.SplitContainerQueryTree.Paint += new System.Windows.Forms.PaintEventHandler(this.SplitContainerQueryTree_Paint);
            // 
            // ButtonSearchQueryName
            // 
            this.ButtonSearchQueryName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSearchQueryName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonSearchQueryName.Location = new System.Drawing.Point(142, 269);
            this.ButtonSearchQueryName.Name = "ButtonSearchQueryName";
            this.ButtonSearchQueryName.Size = new System.Drawing.Size(75, 23);
            this.ButtonSearchQueryName.TabIndex = 4;
            this.ButtonSearchQueryName.Text = "Zoek";
            this.ButtonSearchQueryName.UseVisualStyleBackColor = true;
            this.ButtonSearchQueryName.Click += new System.EventHandler(this.ButtonSearchQueryName_Click);
            // 
            // LabelTrvSearchFound
            // 
            this.LabelTrvSearchFound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelTrvSearchFound.AutoSize = true;
            this.LabelTrvSearchFound.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.LabelTrvSearchFound.Location = new System.Drawing.Point(111, 273);
            this.LabelTrvSearchFound.Name = "LabelTrvSearchFound";
            this.LabelTrvSearchFound.Size = new System.Drawing.Size(25, 15);
            this.LabelTrvSearchFound.TabIndex = 3;
            this.LabelTrvSearchFound.Text = "0 st";
            // 
            // TreeViewExecuteQueries
            // 
            this.TreeViewExecuteQueries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeViewExecuteQueries.CheckBoxes = true;
            this.TreeViewExecuteQueries.Cursor = System.Windows.Forms.Cursors.Default;
            this.TreeViewExecuteQueries.HotTracking = true;
            this.TreeViewExecuteQueries.Location = new System.Drawing.Point(12, 32);
            this.TreeViewExecuteQueries.Name = "TreeViewExecuteQueries";
            this.TreeViewExecuteQueries.Size = new System.Drawing.Size(205, 232);
            this.TreeViewExecuteQueries.TabIndex = 2;
            this.TreeViewExecuteQueries.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewExecuteQueries_AfterCheck);
            this.TreeViewExecuteQueries.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewExecuteQueries_AfterSelect);
            this.TreeViewExecuteQueries.DoubleClick += new System.EventHandler(this.TreeViewExecuteQueries_DoubleClick);
            this.TreeViewExecuteQueries.Validating += new System.ComponentModel.CancelEventHandler(this.TreeViewExecuteQueries_Validating);
            // 
            // ButtonRunQuery
            // 
            this.ButtonRunQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonRunQuery.Enabled = false;
            this.ButtonRunQuery.Location = new System.Drawing.Point(12, 3);
            this.ButtonRunQuery.Name = "ButtonRunQuery";
            this.ButtonRunQuery.Size = new System.Drawing.Size(205, 23);
            this.ButtonRunQuery.TabIndex = 1;
            this.ButtonRunQuery.Text = "&Uitvoeren";
            this.ButtonRunQuery.UseVisualStyleBackColor = true;
            this.ButtonRunQuery.Click += new System.EventHandler(this.ButtonRunQuery_Click);
            // 
            // TextBoxSearchInQueryTreeView
            // 
            this.TextBoxSearchInQueryTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSearchInQueryTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.TextBoxSearchInQueryTreeView.Location = new System.Drawing.Point(12, 270);
            this.TextBoxSearchInQueryTreeView.Name = "TextBoxSearchInQueryTreeView";
            this.TextBoxSearchInQueryTreeView.Size = new System.Drawing.Size(100, 23);
            this.TextBoxSearchInQueryTreeView.TabIndex = 0;
            this.TextBoxSearchInQueryTreeView.TextChanged += new System.EventHandler(this.TextBoxSearchInQueryTreeView_TextChanged);
            this.TextBoxSearchInQueryTreeView.Enter += new System.EventHandler(this.TextBoxSearchInQueryTreeView_Enter_1);
            this.TextBoxSearchInQueryTreeView.Leave += new System.EventHandler(this.TextBoxSearchInQueryTreeView_Leave_1);
            // 
            // RichTextBoxQueryDescription
            // 
            this.RichTextBoxQueryDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxQueryDescription.Location = new System.Drawing.Point(0, 0);
            this.RichTextBoxQueryDescription.Name = "RichTextBoxQueryDescription";
            this.RichTextBoxQueryDescription.Size = new System.Drawing.Size(232, 75);
            this.RichTextBoxQueryDescription.TabIndex = 0;
            this.RichTextBoxQueryDescription.Text = "";
            // 
            // ButtonRemoveSelection
            // 
            this.ButtonRemoveSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonRemoveSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonRemoveSelection.Location = new System.Drawing.Point(343, 357);
            this.ButtonRemoveSelection.Name = "ButtonRemoveSelection";
            this.ButtonRemoveSelection.Size = new System.Drawing.Size(114, 23);
            this.ButtonRemoveSelection.TabIndex = 6;
            this.ButtonRemoveSelection.Text = "&Verwijder selectie";
            this.ButtonRemoveSelection.UseVisualStyleBackColor = true;
            this.ButtonRemoveSelection.Visible = false;
            this.ButtonRemoveSelection.Click += new System.EventHandler(this.ButtonRemoveSelection_Click);
            // 
            // ButtonRunQueryAndExport
            // 
            this.ButtonRunQueryAndExport.Cursor = System.Windows.Forms.Cursors.Default;
            this.ButtonRunQueryAndExport.Enabled = false;
            this.ButtonRunQueryAndExport.Location = new System.Drawing.Point(84, 3);
            this.ButtonRunQueryAndExport.Name = "ButtonRunQueryAndExport";
            this.ButtonRunQueryAndExport.Size = new System.Drawing.Size(75, 23);
            this.ButtonRunQueryAndExport.TabIndex = 2;
            this.ButtonRunQueryAndExport.Text = "&Uitvoeren";
            this.ButtonRunQueryAndExport.UseVisualStyleBackColor = true;
            this.ButtonRunQueryAndExport.Click += new System.EventHandler(this.ButtonRunQueryAndExport_Click);
            // 
            // ButtonExport
            // 
            this.ButtonExport.Enabled = false;
            this.ButtonExport.Location = new System.Drawing.Point(3, 3);
            this.ButtonExport.Name = "ButtonExport";
            this.ButtonExport.Size = new System.Drawing.Size(75, 23);
            this.ButtonExport.TabIndex = 1;
            this.ButtonExport.Text = "&Exporteren";
            this.ButtonExport.UseVisualStyleBackColor = true;
            this.ButtonExport.Click += new System.EventHandler(this.ButtonExport_Click);
            // 
            // DataGridViewQueries
            // 
            this.DataGridViewQueries.AllowUserToAddRows = false;
            this.DataGridViewQueries.AllowUserToDeleteRows = false;
            this.DataGridViewQueries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridViewQueries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewQueries.Location = new System.Drawing.Point(3, 34);
            this.DataGridViewQueries.Name = "DataGridViewQueries";
            this.DataGridViewQueries.ReadOnly = true;
            this.DataGridViewQueries.Size = new System.Drawing.Size(454, 317);
            this.DataGridViewQueries.TabIndex = 0;
            this.DataGridViewQueries.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridViewQueries_CellFormatting);
            this.DataGridViewQueries.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewQueries_CellMouseDown);
            this.DataGridViewQueries.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewQueries_CellMouseEnter);
            this.DataGridViewQueries.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewQueries_CellMouseLeave);
            this.DataGridViewQueries.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridViewQueries_MouseDown);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(881, 391);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ButtonExportDomeinwaardeCTRL);
            this.tabPage1.Controls.Add(this.ButtonGetHelpTable);
            this.tabPage1.Controls.Add(this.GroupBoxDomainValues);
            this.tabPage1.Controls.Add(this.GroupBoxPassports);
            this.tabPage1.Controls.Add(this.GroupBoxPrepare);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(881, 391);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ButtonExportDomeinwaardeCTRL
            // 
            this.ButtonExportDomeinwaardeCTRL.Location = new System.Drawing.Point(14, 359);
            this.ButtonExportDomeinwaardeCTRL.Name = "ButtonExportDomeinwaardeCTRL";
            this.ButtonExportDomeinwaardeCTRL.Size = new System.Drawing.Size(141, 23);
            this.ButtonExportDomeinwaardeCTRL.TabIndex = 4;
            this.ButtonExportDomeinwaardeCTRL.Text = "&Exporteren";
            this.ButtonExportDomeinwaardeCTRL.UseVisualStyleBackColor = true;
            // 
            // ButtonGetHelpTable
            // 
            this.ButtonGetHelpTable.Location = new System.Drawing.Point(14, 330);
            this.ButtonGetHelpTable.Name = "ButtonGetHelpTable";
            this.ButtonGetHelpTable.Size = new System.Drawing.Size(141, 23);
            this.ButtonGetHelpTable.TabIndex = 3;
            this.ButtonGetHelpTable.Text = "Complete &hulp tabel";
            this.ButtonGetHelpTable.UseVisualStyleBackColor = true;
            // 
            // GroupBoxDomainValues
            // 
            this.GroupBoxDomainValues.Location = new System.Drawing.Point(8, 224);
            this.GroupBoxDomainValues.Name = "GroupBoxDomainValues";
            this.GroupBoxDomainValues.Size = new System.Drawing.Size(200, 100);
            this.GroupBoxDomainValues.TabIndex = 2;
            this.GroupBoxDomainValues.TabStop = false;
            this.GroupBoxDomainValues.Text = "Stamlijst(en)";
            // 
            // GroupBoxPassports
            // 
            this.GroupBoxPassports.Location = new System.Drawing.Point(8, 118);
            this.GroupBoxPassports.Name = "GroupBoxPassports";
            this.GroupBoxPassports.Size = new System.Drawing.Size(200, 100);
            this.GroupBoxPassports.TabIndex = 1;
            this.GroupBoxPassports.TabStop = false;
            this.GroupBoxPassports.Text = "Paspoorten (let op, hulptabellen moeten aanwezig zijn)";
            // 
            // GroupBoxPrepare
            // 
            this.GroupBoxPrepare.Controls.Add(this.ComboBoxGBIbasisConnName);
            this.GroupBoxPrepare.Controls.Add(this.ComboBoxGBIsystemConnName);
            this.GroupBoxPrepare.Location = new System.Drawing.Point(8, 12);
            this.GroupBoxPrepare.Name = "GroupBoxPrepare";
            this.GroupBoxPrepare.Size = new System.Drawing.Size(200, 100);
            this.GroupBoxPrepare.TabIndex = 0;
            this.GroupBoxPrepare.TabStop = false;
            this.GroupBoxPrepare.Text = "Voorbereiden hulptabellen";
            // 
            // ComboBoxGBIbasisConnName
            // 
            this.ComboBoxGBIbasisConnName.FormattingEnabled = true;
            this.ComboBoxGBIbasisConnName.Location = new System.Drawing.Point(6, 51);
            this.ComboBoxGBIbasisConnName.Name = "ComboBoxGBIbasisConnName";
            this.ComboBoxGBIbasisConnName.Size = new System.Drawing.Size(121, 23);
            this.ComboBoxGBIbasisConnName.TabIndex = 1;
            // 
            // ComboBoxGBIsystemConnName
            // 
            this.ComboBoxGBIsystemConnName.FormattingEnabled = true;
            this.ComboBoxGBIsystemConnName.Location = new System.Drawing.Point(6, 22);
            this.ComboBoxGBIsystemConnName.Name = "ComboBoxGBIsystemConnName";
            this.ComboBoxGBIsystemConnName.Size = new System.Drawing.Size(121, 23);
            this.ComboBoxGBIsystemConnName.TabIndex = 0;
            // 
            // ProgressBarExport
            // 
            this.ProgressBarExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBarExport.Location = new System.Drawing.Point(426, 495);
            this.ProgressBarExport.Name = "ProgressBarExport";
            this.ProgressBarExport.Size = new System.Drawing.Size(184, 15);
            this.ProgressBarExport.TabIndex = 4;
            this.ProgressBarExport.Visible = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 515);
            this.Controls.Add(this.ProgressBarExport);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.PanelHeaderMainForm);
            this.Controls.Add(this.StatusStrip1);
            this.Controls.Add(this.MenuStripMainForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStripMainForm;
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.MenuStripMainForm.ResumeLayout(false);
            this.MenuStripMainForm.PerformLayout();
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.PanelHeaderMainForm.ResumeLayout(false);
            this.PanelHeaderMainForm.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.TabPageQueries.ResumeLayout(false);
            this.SplitContainer1FormMain.Panel1.ResumeLayout(false);
            this.SplitContainer1FormMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1FormMain)).EndInit();
            this.SplitContainer1FormMain.ResumeLayout(false);
            this.SplitContainerQueryTree.Panel1.ResumeLayout(false);
            this.SplitContainerQueryTree.Panel1.PerformLayout();
            this.SplitContainerQueryTree.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerQueryTree)).EndInit();
            this.SplitContainerQueryTree.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewQueries)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.GroupBoxPrepare.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel PanelHeaderMainForm;
        private System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage TabPageQueries;
        public System.Windows.Forms.SplitContainer SplitContainer1FormMain;
        public System.Windows.Forms.SplitContainer SplitContainerQueryTree;
        private System.Windows.Forms.Button ButtonRunQuery;
        private System.Windows.Forms.TextBox TextBoxSearchInQueryTreeView;
        private System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel4;
        public System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox GroupBoxDomainValues;
        private System.Windows.Forms.GroupBox GroupBoxPassports;
        private System.Windows.Forms.GroupBox GroupBoxPrepare;
        public System.Windows.Forms.TreeView TreeViewExecuteQueries;
        public System.Windows.Forms.MenuStrip MenuStripMainForm;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemProgram;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemClose;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemChangeUser;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemMaintain;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemMaintainDbConnections;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemmaintainUsers;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemMaintainQueries;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemMaintainQueryGroups;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOptions;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemConfigure;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAbout;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLanguage;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLanguageDutch;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLanguageEnglish;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox ComboBoxGBIbasisConnName;
        private System.Windows.Forms.ComboBox ComboBoxGBIsystemConnName;
        private System.Windows.Forms.RichTextBox RichTextBoxQueryDescription;
        private System.Windows.Forms.Button ButtonGetHelpTable;
        private System.Windows.Forms.Button ButtonExportDomeinwaardeCTRL;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.DataGridView DataGridViewQueries;
        private System.Windows.Forms.StatusStrip StatusStrip1;
        private System.Windows.Forms.CheckBox CheckBoxShowGeometryField;
        private System.Windows.Forms.Button ButtonRunQueryAndExport;
        private System.Windows.Forms.Button ButtonExport;
        public System.Windows.Forms.ProgressBar ProgressBarExport;
        private System.Windows.Forms.ComboBox ComboBoxQueryGroup;
        public System.Windows.Forms.Button ButtonRemoveSelection;
        private System.Windows.Forms.Label LabelTrvSearchFound;
        private System.Windows.Forms.Button ButtonSearchQueryName;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDatabaseDisconnect;
        internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDatabaseConnection;
    }
}

