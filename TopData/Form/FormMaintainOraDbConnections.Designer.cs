
namespace TopData
{
    partial class FormMaintainOraDbConnections
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.TabControlOraConnections = new System.Windows.Forms.TabControl();
            this.TabPageMakeConnection = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ButtonClear = new System.Windows.Forms.Button();
            this.ButtonSave = new System.Windows.Forms.Button();
            this.ButtonTestConnection = new System.Windows.Forms.Button();
            this.ComboBoxDatabaseName = new System.Windows.Forms.ComboBox();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.ComboBoxSchemaName = new System.Windows.Forms.ComboBox();
            this.TextBoxConnectionName = new System.Windows.Forms.TextBox();
            this.LabelDatabaseName = new System.Windows.Forms.Label();
            this.LabelPassword = new System.Windows.Forms.Label();
            this.LabelSchemaName = new System.Windows.Forms.Label();
            this.LabelConnectionName = new System.Windows.Forms.Label();
            this.TabPageModifyConnection = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ComboBoxAlterSchemaName = new System.Windows.Forms.ComboBox();
            this.ButtonDelete = new System.Windows.Forms.Button();
            this.ButtonAlter = new System.Windows.Forms.Button();
            this.ButtonAlterTestLogin = new System.Windows.Forms.Button();
            this.ComboBoxAlterOraDatabase = new System.Windows.Forms.ComboBox();
            this.TextBoxAlterOraPassword = new System.Windows.Forms.TextBox();
            this.ComboBoxAlterConnectionNames = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TabPageDatabaseSchemaName = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ButtonDeleteDatabaseName = new System.Windows.Forms.Button();
            this.ButtonDeleteSchemaName = new System.Windows.Forms.Button();
            this.ComboBoxDelDatabaseName = new System.Windows.Forms.ComboBox();
            this.ComboBoxDelSchemaName = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.LabelTestConnection = new System.Windows.Forms.Label();
            this.TabControlOraConnections.SuspendLayout();
            this.TabPageMakeConnection.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.TabPageModifyConnection.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.TabPageDatabaseSchemaName.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControlOraConnections
            // 
            this.TabControlOraConnections.Controls.Add(this.TabPageMakeConnection);
            this.TabControlOraConnections.Controls.Add(this.TabPageModifyConnection);
            this.TabControlOraConnections.Controls.Add(this.TabPageDatabaseSchemaName);
            this.TabControlOraConnections.Dock = System.Windows.Forms.DockStyle.Top;
            this.TabControlOraConnections.HotTrack = true;
            this.TabControlOraConnections.Location = new System.Drawing.Point(0, 0);
            this.TabControlOraConnections.Name = "TabControlOraConnections";
            this.TabControlOraConnections.SelectedIndex = 0;
            this.TabControlOraConnections.Size = new System.Drawing.Size(419, 206);
            this.TabControlOraConnections.TabIndex = 0;
            this.TabControlOraConnections.SelectedIndexChanged += new System.EventHandler(this.TabControlOraConnections_SelectedIndexChanged);
            this.TabControlOraConnections.Click += new System.EventHandler(this.TabControlOraConnections_Click);
            // 
            // TabPageMakeConnection
            // 
            this.TabPageMakeConnection.Controls.Add(this.groupBox1);
            this.TabPageMakeConnection.Location = new System.Drawing.Point(4, 24);
            this.TabPageMakeConnection.Name = "TabPageMakeConnection";
            this.TabPageMakeConnection.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageMakeConnection.Size = new System.Drawing.Size(411, 178);
            this.TabPageMakeConnection.TabIndex = 0;
            this.TabPageMakeConnection.Text = "Nieuw";
            this.TabPageMakeConnection.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ButtonClear);
            this.groupBox1.Controls.Add(this.ButtonSave);
            this.groupBox1.Controls.Add(this.ButtonTestConnection);
            this.groupBox1.Controls.Add(this.ComboBoxDatabaseName);
            this.groupBox1.Controls.Add(this.TextBoxPassword);
            this.groupBox1.Controls.Add(this.ComboBoxSchemaName);
            this.groupBox1.Controls.Add(this.TextBoxConnectionName);
            this.groupBox1.Controls.Add(this.LabelDatabaseName);
            this.groupBox1.Controls.Add(this.LabelPassword);
            this.groupBox1.Controls.Add(this.LabelSchemaName);
            this.groupBox1.Controls.Add(this.LabelConnectionName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(405, 172);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // ButtonClear
            // 
            this.ButtonClear.Location = new System.Drawing.Point(314, 143);
            this.ButtonClear.Name = "ButtonClear";
            this.ButtonClear.Size = new System.Drawing.Size(75, 23);
            this.ButtonClear.TabIndex = 10;
            this.ButtonClear.Text = "&Opnieuw";
            this.ButtonClear.UseVisualStyleBackColor = true;
            this.ButtonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // ButtonSave
            // 
            this.ButtonSave.Enabled = false;
            this.ButtonSave.Location = new System.Drawing.Point(233, 143);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(75, 23);
            this.ButtonSave.TabIndex = 9;
            this.ButtonSave.Text = "Op&slaan";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ButtonTestConnection
            // 
            this.ButtonTestConnection.Location = new System.Drawing.Point(152, 143);
            this.ButtonTestConnection.Name = "ButtonTestConnection";
            this.ButtonTestConnection.Size = new System.Drawing.Size(75, 23);
            this.ButtonTestConnection.TabIndex = 8;
            this.ButtonTestConnection.Text = "&Test";
            this.ButtonTestConnection.UseVisualStyleBackColor = true;
            this.ButtonTestConnection.Click += new System.EventHandler(this.ButtonTestConnection_Click);
            // 
            // ComboBoxDatabaseName
            // 
            this.ComboBoxDatabaseName.FormattingEnabled = true;
            this.ComboBoxDatabaseName.Location = new System.Drawing.Point(111, 109);
            this.ComboBoxDatabaseName.Name = "ComboBoxDatabaseName";
            this.ComboBoxDatabaseName.Size = new System.Drawing.Size(278, 23);
            this.ComboBoxDatabaseName.TabIndex = 7;
            this.ComboBoxDatabaseName.TextChanged += new System.EventHandler(this.ComboBoxDatabaseName_TextChanged);
            this.ComboBoxDatabaseName.Enter += new System.EventHandler(this.ComboBoxDatabaseName_Enter);
            this.ComboBoxDatabaseName.Leave += new System.EventHandler(this.ComboBoxDatabaseName_Leave);
            this.ComboBoxDatabaseName.Validated += new System.EventHandler(this.ComboBoxDatabaseName_Validated);
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(111, 80);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.PasswordChar = '*';
            this.TextBoxPassword.Size = new System.Drawing.Size(278, 23);
            this.TextBoxPassword.TabIndex = 6;
            this.TextBoxPassword.TextChanged += new System.EventHandler(this.TextBoxPassword_TextChanged);
            this.TextBoxPassword.Enter += new System.EventHandler(this.TextBoxPassword_Enter);
            this.TextBoxPassword.Leave += new System.EventHandler(this.TextBoxPassword_Leave);
            // 
            // ComboBoxSchemaName
            // 
            this.ComboBoxSchemaName.FormattingEnabled = true;
            this.ComboBoxSchemaName.Location = new System.Drawing.Point(111, 51);
            this.ComboBoxSchemaName.Name = "ComboBoxSchemaName";
            this.ComboBoxSchemaName.Size = new System.Drawing.Size(278, 23);
            this.ComboBoxSchemaName.TabIndex = 5;
            this.ComboBoxSchemaName.TextChanged += new System.EventHandler(this.ComboBoxSchemaName_TextChanged);
            this.ComboBoxSchemaName.Enter += new System.EventHandler(this.ComboBoxSchemaName_Enter);
            this.ComboBoxSchemaName.Leave += new System.EventHandler(this.ComboBoxSchemaName_Leave);
            this.ComboBoxSchemaName.Validated += new System.EventHandler(this.ComboBoxSchemaName_Validated);
            // 
            // TextBoxConnectionName
            // 
            this.TextBoxConnectionName.Location = new System.Drawing.Point(111, 22);
            this.TextBoxConnectionName.Name = "TextBoxConnectionName";
            this.TextBoxConnectionName.Size = new System.Drawing.Size(278, 23);
            this.TextBoxConnectionName.TabIndex = 4;
            this.TextBoxConnectionName.TextChanged += new System.EventHandler(this.TextBoxConnectionName_TextChanged);
            this.TextBoxConnectionName.Enter += new System.EventHandler(this.TextBoxConnectionName_Enter);
            this.TextBoxConnectionName.Leave += new System.EventHandler(this.TextBoxConnectionName_Leave);
            // 
            // LabelDatabaseName
            // 
            this.LabelDatabaseName.AutoSize = true;
            this.LabelDatabaseName.Location = new System.Drawing.Point(6, 112);
            this.LabelDatabaseName.Name = "LabelDatabaseName";
            this.LabelDatabaseName.Size = new System.Drawing.Size(61, 15);
            this.LabelDatabaseName.TabIndex = 3;
            this.LabelDatabaseName.Text = "Database :";
            // 
            // LabelPassword
            // 
            this.LabelPassword.AutoSize = true;
            this.LabelPassword.Location = new System.Drawing.Point(6, 83);
            this.LabelPassword.Name = "LabelPassword";
            this.LabelPassword.Size = new System.Drawing.Size(81, 15);
            this.LabelPassword.TabIndex = 2;
            this.LabelPassword.Text = "Wachtwoord :";
            // 
            // LabelSchemaName
            // 
            this.LabelSchemaName.AutoSize = true;
            this.LabelSchemaName.Location = new System.Drawing.Point(6, 54);
            this.LabelSchemaName.Name = "LabelSchemaName";
            this.LabelSchemaName.Size = new System.Drawing.Size(55, 15);
            this.LabelSchemaName.TabIndex = 1;
            this.LabelSchemaName.Text = "Schema :";
            // 
            // LabelConnectionName
            // 
            this.LabelConnectionName.AutoSize = true;
            this.LabelConnectionName.Location = new System.Drawing.Point(6, 25);
            this.LabelConnectionName.Name = "LabelConnectionName";
            this.LabelConnectionName.Size = new System.Drawing.Size(100, 15);
            this.LabelConnectionName.TabIndex = 0;
            this.LabelConnectionName.Text = "Naam connectie :";
            // 
            // TabPageModifyConnection
            // 
            this.TabPageModifyConnection.Controls.Add(this.groupBox2);
            this.TabPageModifyConnection.Location = new System.Drawing.Point(4, 24);
            this.TabPageModifyConnection.Name = "TabPageModifyConnection";
            this.TabPageModifyConnection.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageModifyConnection.Size = new System.Drawing.Size(411, 178);
            this.TabPageModifyConnection.TabIndex = 1;
            this.TabPageModifyConnection.Text = "Verander / verwijder";
            this.TabPageModifyConnection.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ComboBoxAlterSchemaName);
            this.groupBox2.Controls.Add(this.ButtonDelete);
            this.groupBox2.Controls.Add(this.ButtonAlter);
            this.groupBox2.Controls.Add(this.ButtonAlterTestLogin);
            this.groupBox2.Controls.Add(this.ComboBoxAlterOraDatabase);
            this.groupBox2.Controls.Add(this.TextBoxAlterOraPassword);
            this.groupBox2.Controls.Add(this.ComboBoxAlterConnectionNames);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(405, 172);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // ComboBoxAlterSchemaName
            // 
            this.ComboBoxAlterSchemaName.FormattingEnabled = true;
            this.ComboBoxAlterSchemaName.Location = new System.Drawing.Point(111, 51);
            this.ComboBoxAlterSchemaName.Name = "ComboBoxAlterSchemaName";
            this.ComboBoxAlterSchemaName.Size = new System.Drawing.Size(278, 23);
            this.ComboBoxAlterSchemaName.TabIndex = 11;
            this.ComboBoxAlterSchemaName.TextChanged += new System.EventHandler(this.ComboBoxAlterSchemaName_TextChanged);
            this.ComboBoxAlterSchemaName.Enter += new System.EventHandler(this.ComboBoxAlterSchemaName_Enter);
            this.ComboBoxAlterSchemaName.Leave += new System.EventHandler(this.ComboBoxAlterSchemaName_Leave);
            // 
            // ButtonDelete
            // 
            this.ButtonDelete.Location = new System.Drawing.Point(314, 143);
            this.ButtonDelete.Name = "ButtonDelete";
            this.ButtonDelete.Size = new System.Drawing.Size(75, 23);
            this.ButtonDelete.TabIndex = 10;
            this.ButtonDelete.Text = "&Verwijder";
            this.ButtonDelete.UseVisualStyleBackColor = true;
            this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // ButtonAlter
            // 
            this.ButtonAlter.Enabled = false;
            this.ButtonAlter.Location = new System.Drawing.Point(233, 143);
            this.ButtonAlter.Name = "ButtonAlter";
            this.ButtonAlter.Size = new System.Drawing.Size(75, 23);
            this.ButtonAlter.TabIndex = 9;
            this.ButtonAlter.Text = "&Muteer";
            this.ButtonAlter.UseVisualStyleBackColor = true;
            this.ButtonAlter.Click += new System.EventHandler(this.ButtonAlter_Click);
            // 
            // ButtonAlterTestLogin
            // 
            this.ButtonAlterTestLogin.Location = new System.Drawing.Point(152, 143);
            this.ButtonAlterTestLogin.Name = "ButtonAlterTestLogin";
            this.ButtonAlterTestLogin.Size = new System.Drawing.Size(75, 23);
            this.ButtonAlterTestLogin.TabIndex = 8;
            this.ButtonAlterTestLogin.Text = "&Test";
            this.ButtonAlterTestLogin.UseVisualStyleBackColor = true;
            this.ButtonAlterTestLogin.Click += new System.EventHandler(this.ButtonAlterTestLogin_Click);
            // 
            // ComboBoxAlterOraDatabase
            // 
            this.ComboBoxAlterOraDatabase.FormattingEnabled = true;
            this.ComboBoxAlterOraDatabase.Location = new System.Drawing.Point(111, 109);
            this.ComboBoxAlterOraDatabase.Name = "ComboBoxAlterOraDatabase";
            this.ComboBoxAlterOraDatabase.Size = new System.Drawing.Size(278, 23);
            this.ComboBoxAlterOraDatabase.TabIndex = 7;
            this.ComboBoxAlterOraDatabase.SelectedIndexChanged += new System.EventHandler(this.ComboBoxAlterOraDatabase_SelectedIndexChanged);
            this.ComboBoxAlterOraDatabase.TextChanged += new System.EventHandler(this.ComboBoxAlterOraDatabase_TextChanged);
            this.ComboBoxAlterOraDatabase.Enter += new System.EventHandler(this.ComboBoxAlterOraDatabase_Enter);
            this.ComboBoxAlterOraDatabase.Leave += new System.EventHandler(this.ComboBoxAlterOraDatabase_Leave);
            // 
            // TextBoxAlterOraPassword
            // 
            this.TextBoxAlterOraPassword.Location = new System.Drawing.Point(111, 80);
            this.TextBoxAlterOraPassword.Name = "TextBoxAlterOraPassword";
            this.TextBoxAlterOraPassword.PasswordChar = '*';
            this.TextBoxAlterOraPassword.Size = new System.Drawing.Size(278, 23);
            this.TextBoxAlterOraPassword.TabIndex = 6;
            this.TextBoxAlterOraPassword.TextChanged += new System.EventHandler(this.TextBoxAlterOraPassword_TextChanged);
            this.TextBoxAlterOraPassword.Enter += new System.EventHandler(this.TextBoxAlterOraPassword_Enter);
            this.TextBoxAlterOraPassword.Leave += new System.EventHandler(this.TextBoxAlterOraPassword_Leave);
            // 
            // ComboBoxAlterConnectionNames
            // 
            this.ComboBoxAlterConnectionNames.FormattingEnabled = true;
            this.ComboBoxAlterConnectionNames.Location = new System.Drawing.Point(111, 22);
            this.ComboBoxAlterConnectionNames.Name = "ComboBoxAlterConnectionNames";
            this.ComboBoxAlterConnectionNames.Size = new System.Drawing.Size(278, 23);
            this.ComboBoxAlterConnectionNames.TabIndex = 4;
            this.ComboBoxAlterConnectionNames.SelectedIndexChanged += new System.EventHandler(this.ComboBoxAlterConnectionNames_SelectedIndexChanged);
            this.ComboBoxAlterConnectionNames.TextChanged += new System.EventHandler(this.ComboBoxAlterConnectionNames_TextChanged);
            this.ComboBoxAlterConnectionNames.Enter += new System.EventHandler(this.ComboBoxAlterConnectionNames_Enter);
            this.ComboBoxAlterConnectionNames.Leave += new System.EventHandler(this.ComboBoxAlterConnectionNames_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Database :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Wachtwoord :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Schema :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Naam connectie :";
            // 
            // TabPageDatabaseSchemaName
            // 
            this.TabPageDatabaseSchemaName.Controls.Add(this.groupBox3);
            this.TabPageDatabaseSchemaName.Location = new System.Drawing.Point(4, 24);
            this.TabPageDatabaseSchemaName.Name = "TabPageDatabaseSchemaName";
            this.TabPageDatabaseSchemaName.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageDatabaseSchemaName.Size = new System.Drawing.Size(411, 178);
            this.TabPageDatabaseSchemaName.TabIndex = 2;
            this.TabPageDatabaseSchemaName.Text = "Verwijder een schema- of database naam";
            this.TabPageDatabaseSchemaName.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ButtonDeleteDatabaseName);
            this.groupBox3.Controls.Add(this.ButtonDeleteSchemaName);
            this.groupBox3.Controls.Add(this.ComboBoxDelDatabaseName);
            this.groupBox3.Controls.Add(this.ComboBoxDelSchemaName);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(405, 172);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // ButtonDeleteDatabaseName
            // 
            this.ButtonDeleteDatabaseName.Enabled = false;
            this.ButtonDeleteDatabaseName.Location = new System.Drawing.Point(324, 51);
            this.ButtonDeleteDatabaseName.Name = "ButtonDeleteDatabaseName";
            this.ButtonDeleteDatabaseName.Size = new System.Drawing.Size(75, 23);
            this.ButtonDeleteDatabaseName.TabIndex = 11;
            this.ButtonDeleteDatabaseName.Text = "Verwijder";
            this.ButtonDeleteDatabaseName.UseVisualStyleBackColor = true;
            this.ButtonDeleteDatabaseName.Click += new System.EventHandler(this.ButtonDeleteDatabaseName_Click);
            // 
            // ButtonDeleteSchemaName
            // 
            this.ButtonDeleteSchemaName.Enabled = false;
            this.ButtonDeleteSchemaName.Location = new System.Drawing.Point(324, 22);
            this.ButtonDeleteSchemaName.Name = "ButtonDeleteSchemaName";
            this.ButtonDeleteSchemaName.Size = new System.Drawing.Size(75, 23);
            this.ButtonDeleteSchemaName.TabIndex = 10;
            this.ButtonDeleteSchemaName.Text = "Verwijder";
            this.ButtonDeleteSchemaName.UseVisualStyleBackColor = true;
            this.ButtonDeleteSchemaName.Click += new System.EventHandler(this.ButtonDeleteSchemaName_Click);
            // 
            // ComboBoxDelDatabaseName
            // 
            this.ComboBoxDelDatabaseName.FormattingEnabled = true;
            this.ComboBoxDelDatabaseName.Location = new System.Drawing.Point(112, 51);
            this.ComboBoxDelDatabaseName.Name = "ComboBoxDelDatabaseName";
            this.ComboBoxDelDatabaseName.Size = new System.Drawing.Size(202, 23);
            this.ComboBoxDelDatabaseName.TabIndex = 7;
            this.ComboBoxDelDatabaseName.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDelDatabaseName_SelectedIndexChanged);
            this.ComboBoxDelDatabaseName.TextChanged += new System.EventHandler(this.ComboBoxDelDatabaseName_SelectedIndexChanged);
            this.ComboBoxDelDatabaseName.Enter += new System.EventHandler(this.ComboBoxDelDatabaseName_Enter);
            this.ComboBoxDelDatabaseName.Leave += new System.EventHandler(this.ComboBoxDelDatabaseName_Leave);
            // 
            // ComboBoxDelSchemaName
            // 
            this.ComboBoxDelSchemaName.FormattingEnabled = true;
            this.ComboBoxDelSchemaName.Location = new System.Drawing.Point(111, 22);
            this.ComboBoxDelSchemaName.Name = "ComboBoxDelSchemaName";
            this.ComboBoxDelSchemaName.Size = new System.Drawing.Size(203, 23);
            this.ComboBoxDelSchemaName.TabIndex = 4;
            this.ComboBoxDelSchemaName.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDelSchemaName_SelectedIndexChanged);
            this.ComboBoxDelSchemaName.TextChanged += new System.EventHandler(this.ComboBoxDelSchemaName_SelectedIndexChanged);
            this.ComboBoxDelSchemaName.Enter += new System.EventHandler(this.ComboBoxDelSchemaName_Enter);
            this.ComboBoxDelSchemaName.Leave += new System.EventHandler(this.ComboBoxDelSchemaName_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Database :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "Schema :";
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.Location = new System.Drawing.Point(331, 209);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.Text = "Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // LabelTestConnection
            // 
            this.LabelTestConnection.AutoSize = true;
            this.LabelTestConnection.Location = new System.Drawing.Point(13, 213);
            this.LabelTestConnection.Name = "LabelTestConnection";
            this.LabelTestConnection.Size = new System.Drawing.Size(49, 15);
            this.LabelTestConnection.TabIndex = 11;
            this.LabelTestConnection.Text = "              ";
            // 
            // FormMaintainOraDbConnections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(419, 236);
            this.Controls.Add(this.LabelTestConnection);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.TabControlOraConnections);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormMaintainOraDbConnections";
            this.Text = "FormMaintainOraDbConnections";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMaintainOraDbConnections_FormClosing);
            this.Load += new System.EventHandler(this.FormMaintainOraDbConnections_Load);
            this.TabControlOraConnections.ResumeLayout(false);
            this.TabPageMakeConnection.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.TabPageModifyConnection.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.TabPageDatabaseSchemaName.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl TabControlOraConnections;
        private System.Windows.Forms.TabPage TabPageMakeConnection;
        private System.Windows.Forms.TabPage TabPageModifyConnection;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.TabPage TabPageDatabaseSchemaName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox ComboBoxDatabaseName;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.ComboBox ComboBoxSchemaName;
        private System.Windows.Forms.TextBox TextBoxConnectionName;
        private System.Windows.Forms.Label LabelDatabaseName;
        private System.Windows.Forms.Label LabelPassword;
        private System.Windows.Forms.Label LabelSchemaName;
        private System.Windows.Forms.Label LabelConnectionName;
        private System.Windows.Forms.Button ButtonClear;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Button ButtonTestConnection;
        private System.Windows.Forms.Label LabelTestConnection;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button ButtonDelete;
        private System.Windows.Forms.Button ButtonAlter;
        private System.Windows.Forms.Button ButtonAlterTestLogin;
        private System.Windows.Forms.ComboBox ComboBoxAlterOraDatabase;
        private System.Windows.Forms.TextBox TextBoxAlterOraPassword;
        private System.Windows.Forms.ComboBox ComboBoxAlterConnectionNames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button ButtonDeleteSchemaName;
        private System.Windows.Forms.ComboBox ComboBoxDelDatabaseName;
        private System.Windows.Forms.ComboBox ComboBoxDelSchemaName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ComboBoxAlterSchemaName;
        private System.Windows.Forms.Button ButtonDeleteDatabaseName;
    }
}