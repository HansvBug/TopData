
namespace TopData
{
    partial class FormMaintainUsers
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
            this.components = new System.ComponentModel.Container();
            this.GroupBoxUsers = new System.Windows.Forms.GroupBox();
            this.DataGridViewUsers = new System.Windows.Forms.DataGridView();
            this.GroupBoxLoggedInAsUser = new System.Windows.Forms.GroupBox();
            this.LoggedInAuthenticationLabel = new System.Windows.Forms.Label();
            this.LoggedInRoleLabel = new System.Windows.Forms.Label();
            this.LoggedInNameLabel = new System.Windows.Forms.Label();
            this.TextBoxWindowsAuthencation = new System.Windows.Forms.TextBox();
            this.TextBoxLoggedInUserRole = new System.Windows.Forms.TextBox();
            this.TextBoxLoggedInUserName = new System.Windows.Forms.TextBox();
            this.GroupBoxUserData = new System.Windows.Forms.GroupBox();
            this.LabelFullName = new System.Windows.Forms.Label();
            this.TextBoxFullName = new System.Windows.Forms.TextBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonAlterUser = new System.Windows.Forms.Button();
            this.ButtonDeleteUser = new System.Windows.Forms.Button();
            this.ButtonCreateUser = new System.Windows.Forms.Button();
            this.LabelGroup = new System.Windows.Forms.Label();
            this.LabelAuthencationName = new System.Windows.Forms.Label();
            this.LabelAuthencation = new System.Windows.Forms.Label();
            this.LabelRole = new System.Windows.Forms.Label();
            this.LabelRepeatPassword = new System.Windows.Forms.Label();
            this.LabelPassword = new System.Windows.Forms.Label();
            this.LabelName = new System.Windows.Forms.Label();
            this.ComboBoxGroup = new System.Windows.Forms.ComboBox();
            this.TextBoxAuthencationName = new System.Windows.Forms.TextBox();
            this.ContextMenuStripAuthentication = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemCurrentUser = new System.Windows.Forms.ToolStripMenuItem();
            this.ComboBoxAuthentication = new System.Windows.Forms.ComboBox();
            this.ComboBoxRole = new System.Windows.Forms.ComboBox();
            this.TextBoxRepeatPassword = new System.Windows.Forms.TextBox();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.TextBoxName = new System.Windows.Forms.TextBox();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.GroupBoxUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewUsers)).BeginInit();
            this.GroupBoxLoggedInAsUser.SuspendLayout();
            this.GroupBoxUserData.SuspendLayout();
            this.ContextMenuStripAuthentication.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBoxUsers
            // 
            this.GroupBoxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxUsers.Controls.Add(this.DataGridViewUsers);
            this.GroupBoxUsers.Controls.Add(this.GroupBoxLoggedInAsUser);
            this.GroupBoxUsers.Controls.Add(this.GroupBoxUserData);
            this.GroupBoxUsers.Location = new System.Drawing.Point(12, 0);
            this.GroupBoxUsers.Name = "GroupBoxUsers";
            this.GroupBoxUsers.Size = new System.Drawing.Size(794, 476);
            this.GroupBoxUsers.TabIndex = 0;
            this.GroupBoxUsers.TabStop = false;
            // 
            // DataGridViewUsers
            // 
            this.DataGridViewUsers.AllowUserToAddRows = false;
            this.DataGridViewUsers.AllowUserToDeleteRows = false;
            this.DataGridViewUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridViewUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.DataGridViewUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewUsers.Location = new System.Drawing.Point(6, 22);
            this.DataGridViewUsers.MultiSelect = false;
            this.DataGridViewUsers.Name = "DataGridViewUsers";
            this.DataGridViewUsers.ReadOnly = true;
            this.DataGridViewUsers.RowTemplate.Height = 25;
            this.DataGridViewUsers.Size = new System.Drawing.Size(425, 443);
            this.DataGridViewUsers.TabIndex = 9;
            this.DataGridViewUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewUsers_CellClick);
            this.DataGridViewUsers.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewUsers_CellMouseEnter);
            this.DataGridViewUsers.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewUsers_CellMouseLeave);
            // 
            // GroupBoxLoggedInAsUser
            // 
            this.GroupBoxLoggedInAsUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxLoggedInAsUser.Controls.Add(this.LoggedInAuthenticationLabel);
            this.GroupBoxLoggedInAsUser.Controls.Add(this.LoggedInRoleLabel);
            this.GroupBoxLoggedInAsUser.Controls.Add(this.LoggedInNameLabel);
            this.GroupBoxLoggedInAsUser.Controls.Add(this.TextBoxWindowsAuthencation);
            this.GroupBoxLoggedInAsUser.Controls.Add(this.TextBoxLoggedInUserRole);
            this.GroupBoxLoggedInAsUser.Controls.Add(this.TextBoxLoggedInUserName);
            this.GroupBoxLoggedInAsUser.Location = new System.Drawing.Point(437, 351);
            this.GroupBoxLoggedInAsUser.Name = "GroupBoxLoggedInAsUser";
            this.GroupBoxLoggedInAsUser.Size = new System.Drawing.Size(350, 114);
            this.GroupBoxLoggedInAsUser.TabIndex = 2;
            this.GroupBoxLoggedInAsUser.TabStop = false;
            this.GroupBoxLoggedInAsUser.Text = "Ingelogd als";
            // 
            // LoggedInAuthenticationLabel
            // 
            this.LoggedInAuthenticationLabel.AutoSize = true;
            this.LoggedInAuthenticationLabel.Location = new System.Drawing.Point(15, 83);
            this.LoggedInAuthenticationLabel.Name = "LoggedInAuthenticationLabel";
            this.LoggedInAuthenticationLabel.Size = new System.Drawing.Size(85, 15);
            this.LoggedInAuthenticationLabel.TabIndex = 5;
            this.LoggedInAuthenticationLabel.Text = "Authencation :";
            // 
            // LoggedInRoleLabel
            // 
            this.LoggedInRoleLabel.AutoSize = true;
            this.LoggedInRoleLabel.Location = new System.Drawing.Point(15, 54);
            this.LoggedInRoleLabel.Name = "LoggedInRoleLabel";
            this.LoggedInRoleLabel.Size = new System.Drawing.Size(36, 15);
            this.LoggedInRoleLabel.TabIndex = 4;
            this.LoggedInRoleLabel.Text = "Role :";
            this.LoggedInRoleLabel.Visible = false;
            // 
            // LoggedInNameLabel
            // 
            this.LoggedInNameLabel.AutoSize = true;
            this.LoggedInNameLabel.Location = new System.Drawing.Point(15, 25);
            this.LoggedInNameLabel.Name = "LoggedInNameLabel";
            this.LoggedInNameLabel.Size = new System.Drawing.Size(45, 15);
            this.LoggedInNameLabel.TabIndex = 3;
            this.LoggedInNameLabel.Text = "Name :";
            // 
            // TextBoxWindowsAuthencation
            // 
            this.TextBoxWindowsAuthencation.Enabled = false;
            this.TextBoxWindowsAuthencation.Location = new System.Drawing.Point(143, 80);
            this.TextBoxWindowsAuthencation.Name = "TextBoxWindowsAuthencation";
            this.TextBoxWindowsAuthencation.Size = new System.Drawing.Size(190, 23);
            this.TextBoxWindowsAuthencation.TabIndex = 2;
            this.TextBoxWindowsAuthencation.LocationChanged += new System.EventHandler(this.TextBoxName_Leave);
            this.TextBoxWindowsAuthencation.Enter += new System.EventHandler(this.TextBoxName_Enter);
            // 
            // TextBoxLoggedInUserRole
            // 
            this.TextBoxLoggedInUserRole.Enabled = false;
            this.TextBoxLoggedInUserRole.Location = new System.Drawing.Point(143, 51);
            this.TextBoxLoggedInUserRole.Name = "TextBoxLoggedInUserRole";
            this.TextBoxLoggedInUserRole.Size = new System.Drawing.Size(190, 23);
            this.TextBoxLoggedInUserRole.TabIndex = 1;
            this.TextBoxLoggedInUserRole.Visible = false;
            this.TextBoxLoggedInUserRole.Enter += new System.EventHandler(this.TextBoxName_Enter);
            this.TextBoxLoggedInUserRole.Leave += new System.EventHandler(this.TextBoxName_Leave);
            // 
            // TextBoxLoggedInUserName
            // 
            this.TextBoxLoggedInUserName.Enabled = false;
            this.TextBoxLoggedInUserName.Location = new System.Drawing.Point(143, 22);
            this.TextBoxLoggedInUserName.Name = "TextBoxLoggedInUserName";
            this.TextBoxLoggedInUserName.Size = new System.Drawing.Size(190, 23);
            this.TextBoxLoggedInUserName.TabIndex = 0;
            this.TextBoxLoggedInUserName.Enter += new System.EventHandler(this.TextBoxName_Enter);
            this.TextBoxLoggedInUserName.Leave += new System.EventHandler(this.TextBoxName_Leave);
            // 
            // GroupBoxUserData
            // 
            this.GroupBoxUserData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxUserData.Controls.Add(this.LabelFullName);
            this.GroupBoxUserData.Controls.Add(this.TextBoxFullName);
            this.GroupBoxUserData.Controls.Add(this.ButtonCancel);
            this.GroupBoxUserData.Controls.Add(this.ButtonAlterUser);
            this.GroupBoxUserData.Controls.Add(this.ButtonDeleteUser);
            this.GroupBoxUserData.Controls.Add(this.ButtonCreateUser);
            this.GroupBoxUserData.Controls.Add(this.LabelGroup);
            this.GroupBoxUserData.Controls.Add(this.LabelAuthencationName);
            this.GroupBoxUserData.Controls.Add(this.LabelAuthencation);
            this.GroupBoxUserData.Controls.Add(this.LabelRole);
            this.GroupBoxUserData.Controls.Add(this.LabelRepeatPassword);
            this.GroupBoxUserData.Controls.Add(this.LabelPassword);
            this.GroupBoxUserData.Controls.Add(this.LabelName);
            this.GroupBoxUserData.Controls.Add(this.ComboBoxGroup);
            this.GroupBoxUserData.Controls.Add(this.TextBoxAuthencationName);
            this.GroupBoxUserData.Controls.Add(this.ComboBoxAuthentication);
            this.GroupBoxUserData.Controls.Add(this.ComboBoxRole);
            this.GroupBoxUserData.Controls.Add(this.TextBoxRepeatPassword);
            this.GroupBoxUserData.Controls.Add(this.TextBoxPassword);
            this.GroupBoxUserData.Controls.Add(this.TextBoxName);
            this.GroupBoxUserData.Location = new System.Drawing.Point(437, 22);
            this.GroupBoxUserData.Name = "GroupBoxUserData";
            this.GroupBoxUserData.Size = new System.Drawing.Size(350, 306);
            this.GroupBoxUserData.TabIndex = 1;
            this.GroupBoxUserData.TabStop = false;
            this.GroupBoxUserData.Text = "User data";
            // 
            // LabelFullName
            // 
            this.LabelFullName.AutoSize = true;
            this.LabelFullName.Location = new System.Drawing.Point(15, 54);
            this.LabelFullName.Name = "LabelFullName";
            this.LabelFullName.Size = new System.Drawing.Size(95, 15);
            this.LabelFullName.TabIndex = 19;
            this.LabelFullName.Text = "Volledige naam :";
            // 
            // TextBoxFullName
            // 
            this.TextBoxFullName.Location = new System.Drawing.Point(143, 51);
            this.TextBoxFullName.Name = "TextBoxFullName";
            this.TextBoxFullName.Size = new System.Drawing.Size(190, 23);
            this.TextBoxFullName.TabIndex = 1;
            this.TextBoxFullName.TextChanged += new System.EventHandler(this.TextBoxFullName_TextChanged);
            this.TextBoxFullName.Enter += new System.EventHandler(this.TextBoxFullName_Enter);
            this.TextBoxFullName.Leave += new System.EventHandler(this.TextBoxFullName_Leave);
            this.TextBoxFullName.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxFullName_Validating);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Enabled = false;
            this.ButtonCancel.Location = new System.Drawing.Point(258, 271);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 17;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonAlterUser
            // 
            this.ButtonAlterUser.Enabled = false;
            this.ButtonAlterUser.Location = new System.Drawing.Point(177, 271);
            this.ButtonAlterUser.Name = "ButtonAlterUser";
            this.ButtonAlterUser.Size = new System.Drawing.Size(75, 23);
            this.ButtonAlterUser.TabIndex = 16;
            this.ButtonAlterUser.Text = "&Update";
            this.ButtonAlterUser.UseVisualStyleBackColor = true;
            this.ButtonAlterUser.Click += new System.EventHandler(this.ButtonAlterUser_Click);
            // 
            // ButtonDeleteUser
            // 
            this.ButtonDeleteUser.Enabled = false;
            this.ButtonDeleteUser.Location = new System.Drawing.Point(96, 271);
            this.ButtonDeleteUser.Name = "ButtonDeleteUser";
            this.ButtonDeleteUser.Size = new System.Drawing.Size(75, 23);
            this.ButtonDeleteUser.TabIndex = 15;
            this.ButtonDeleteUser.Text = "&Delete";
            this.ButtonDeleteUser.UseVisualStyleBackColor = true;
            this.ButtonDeleteUser.Click += new System.EventHandler(this.ButtonDeleteUser_Click);
            // 
            // ButtonCreateUser
            // 
            this.ButtonCreateUser.Enabled = false;
            this.ButtonCreateUser.Location = new System.Drawing.Point(15, 271);
            this.ButtonCreateUser.Name = "ButtonCreateUser";
            this.ButtonCreateUser.Size = new System.Drawing.Size(75, 23);
            this.ButtonCreateUser.TabIndex = 14;
            this.ButtonCreateUser.Text = "&New";
            this.ButtonCreateUser.UseVisualStyleBackColor = true;
            this.ButtonCreateUser.Click += new System.EventHandler(this.ButtonCreateUser_Click);
            // 
            // LabelGroup
            // 
            this.LabelGroup.AutoSize = true;
            this.LabelGroup.Location = new System.Drawing.Point(15, 228);
            this.LabelGroup.Name = "LabelGroup";
            this.LabelGroup.Size = new System.Drawing.Size(46, 15);
            this.LabelGroup.TabIndex = 13;
            this.LabelGroup.Text = "Group :";
            this.LabelGroup.Visible = false;
            // 
            // LabelAuthencationName
            // 
            this.LabelAuthencationName.AutoSize = true;
            this.LabelAuthencationName.Location = new System.Drawing.Point(15, 199);
            this.LabelAuthencationName.Name = "LabelAuthencationName";
            this.LabelAuthencationName.Size = new System.Drawing.Size(118, 15);
            this.LabelAuthencationName.TabIndex = 12;
            this.LabelAuthencationName.Text = "Authencation name :";
            // 
            // LabelAuthencation
            // 
            this.LabelAuthencation.AutoSize = true;
            this.LabelAuthencation.Location = new System.Drawing.Point(15, 170);
            this.LabelAuthencation.Name = "LabelAuthencation";
            this.LabelAuthencation.Size = new System.Drawing.Size(92, 15);
            this.LabelAuthencation.TabIndex = 11;
            this.LabelAuthencation.Text = "Authentication :";
            // 
            // LabelRole
            // 
            this.LabelRole.AutoSize = true;
            this.LabelRole.Location = new System.Drawing.Point(15, 141);
            this.LabelRole.Name = "LabelRole";
            this.LabelRole.Size = new System.Drawing.Size(36, 15);
            this.LabelRole.TabIndex = 10;
            this.LabelRole.Text = "Role :";
            // 
            // LabelRepeatPassword
            // 
            this.LabelRepeatPassword.AutoSize = true;
            this.LabelRepeatPassword.Location = new System.Drawing.Point(15, 112);
            this.LabelRepeatPassword.Name = "LabelRepeatPassword";
            this.LabelRepeatPassword.Size = new System.Drawing.Size(102, 15);
            this.LabelRepeatPassword.TabIndex = 9;
            this.LabelRepeatPassword.Text = "Repeat password :";
            // 
            // LabelPassword
            // 
            this.LabelPassword.AutoSize = true;
            this.LabelPassword.Location = new System.Drawing.Point(15, 83);
            this.LabelPassword.Name = "LabelPassword";
            this.LabelPassword.Size = new System.Drawing.Size(63, 15);
            this.LabelPassword.TabIndex = 8;
            this.LabelPassword.Text = "Password :";
            // 
            // LabelName
            // 
            this.LabelName.AutoSize = true;
            this.LabelName.Location = new System.Drawing.Point(15, 25);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(45, 15);
            this.LabelName.TabIndex = 7;
            this.LabelName.Text = "Name :";
            // 
            // ComboBoxGroup
            // 
            this.ComboBoxGroup.FormattingEnabled = true;
            this.ComboBoxGroup.ItemHeight = 15;
            this.ComboBoxGroup.Location = new System.Drawing.Point(143, 225);
            this.ComboBoxGroup.Name = "ComboBoxGroup";
            this.ComboBoxGroup.Size = new System.Drawing.Size(190, 23);
            this.ComboBoxGroup.TabIndex = 6;
            this.ComboBoxGroup.Text = "<Group>";
            this.ComboBoxGroup.Visible = false;
            this.ComboBoxGroup.SelectedIndexChanged += new System.EventHandler(this.ComboBoxGroup_Leave);
            this.ComboBoxGroup.TextChanged += new System.EventHandler(this.ComboBoxGroup_TextChanged);
            this.ComboBoxGroup.Enter += new System.EventHandler(this.TextBoxName_Enter);
            this.ComboBoxGroup.Leave += new System.EventHandler(this.ComboBoxGroup_Leave);
            // 
            // TextBoxAuthencationName
            // 
            this.TextBoxAuthencationName.ContextMenuStrip = this.ContextMenuStripAuthentication;
            this.TextBoxAuthencationName.Location = new System.Drawing.Point(143, 196);
            this.TextBoxAuthencationName.Name = "TextBoxAuthencationName";
            this.TextBoxAuthencationName.Size = new System.Drawing.Size(190, 23);
            this.TextBoxAuthencationName.TabIndex = 6;
            this.TextBoxAuthencationName.TextChanged += new System.EventHandler(this.TextBoxAuthencationName_TextChanged);
            this.TextBoxAuthencationName.Enter += new System.EventHandler(this.TextBoxName_Enter);
            this.TextBoxAuthencationName.Leave += new System.EventHandler(this.TextBoxAuthencationName_Leave);
            this.TextBoxAuthencationName.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxName_Validating);
            // 
            // ContextMenuStripAuthentication
            // 
            this.ContextMenuStripAuthentication.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemCurrentUser});
            this.ContextMenuStripAuthentication.Name = "ContextMenuStripAuthentication";
            this.ContextMenuStripAuthentication.Size = new System.Drawing.Size(170, 26);
            // 
            // ToolStripMenuItemCurrentUser
            // 
            this.ToolStripMenuItemCurrentUser.Name = "ToolStripMenuItemCurrentUser";
            this.ToolStripMenuItemCurrentUser.Size = new System.Drawing.Size(169, 22);
            this.ToolStripMenuItemCurrentUser.Text = "Huidige gebruiker";
            this.ToolStripMenuItemCurrentUser.Click += new System.EventHandler(this.ToolStripMenuItemCurrentUser_Click);
            // 
            // ComboBoxAuthentication
            // 
            this.ComboBoxAuthentication.FormattingEnabled = true;
            this.ComboBoxAuthentication.Location = new System.Drawing.Point(143, 167);
            this.ComboBoxAuthentication.Name = "ComboBoxAuthentication";
            this.ComboBoxAuthentication.Size = new System.Drawing.Size(190, 23);
            this.ComboBoxAuthentication.TabIndex = 5;
            this.ComboBoxAuthentication.Text = TdResText.No;
            this.ComboBoxAuthentication.SelectedIndexChanged += new System.EventHandler(this.ComboBoxAuthentication_SelectedIndexChanged);
            this.ComboBoxAuthentication.TextChanged += new System.EventHandler(this.ComboBoxAuthentication_TextChanged);
            this.ComboBoxAuthentication.Leave += new System.EventHandler(this.ComboBoxAuthentication_Leave);
            // 
            // ComboBoxRole
            // 
            this.ComboBoxRole.FormattingEnabled = true;
            this.ComboBoxRole.Location = new System.Drawing.Point(143, 138);
            this.ComboBoxRole.Name = "ComboBoxRole";
            this.ComboBoxRole.Size = new System.Drawing.Size(190, 23);
            this.ComboBoxRole.TabIndex = 4;
            this.ComboBoxRole.TextChanged += new System.EventHandler(this.ComboBoxRole_TextChanged);
            this.ComboBoxRole.Enter += new System.EventHandler(this.TextBoxName_Enter);
            this.ComboBoxRole.Leave += new System.EventHandler(this.ComboBoxRole_Leave);
            // 
            // TextBoxRepeatPassword
            // 
            this.TextBoxRepeatPassword.Location = new System.Drawing.Point(143, 109);
            this.TextBoxRepeatPassword.Name = "TextBoxRepeatPassword";
            this.TextBoxRepeatPassword.PasswordChar = '*';
            this.TextBoxRepeatPassword.Size = new System.Drawing.Size(190, 23);
            this.TextBoxRepeatPassword.TabIndex = 3;
            this.TextBoxRepeatPassword.TextChanged += new System.EventHandler(this.TextBoxRepeatPassword_TextChanged);
            this.TextBoxRepeatPassword.Enter += new System.EventHandler(this.TextBoxName_Enter);
            this.TextBoxRepeatPassword.Leave += new System.EventHandler(this.TextBoxRepeatPassword_Leave);
            this.TextBoxRepeatPassword.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxName_Validating);
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(143, 80);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.PasswordChar = '*';
            this.TextBoxPassword.Size = new System.Drawing.Size(190, 23);
            this.TextBoxPassword.TabIndex = 2;
            this.TextBoxPassword.TextChanged += new System.EventHandler(this.TextBoxPassword_TextChanged);
            this.TextBoxPassword.Enter += new System.EventHandler(this.TextBoxName_Enter);
            this.TextBoxPassword.Leave += new System.EventHandler(this.TextBoxName_Leave);
            this.TextBoxPassword.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxName_Validating);
            // 
            // TextBoxName
            // 
            this.TextBoxName.Location = new System.Drawing.Point(143, 22);
            this.TextBoxName.Name = "TextBoxName";
            this.TextBoxName.Size = new System.Drawing.Size(190, 23);
            this.TextBoxName.TabIndex = 0;
            this.TextBoxName.TextChanged += new System.EventHandler(this.TextBoxName_TextChanged);
            this.TextBoxName.Enter += new System.EventHandler(this.TextBoxName_Enter);
            this.TextBoxName.Leave += new System.EventHandler(this.TextBoxName_Leave);
            this.TextBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxName_Validating);
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.Location = new System.Drawing.Point(731, 482);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 1;
            this.ButtonClose.Text = "C&lose";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // FormMaintainUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(818, 516);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.GroupBoxUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormMaintainUsers";
            this.Text = "FormMaintainUsers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMaintainUsers_FormClosing);
            this.Load += new System.EventHandler(this.FormMaintainUsers_Load);
            this.GroupBoxUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewUsers)).EndInit();
            this.GroupBoxLoggedInAsUser.ResumeLayout(false);
            this.GroupBoxLoggedInAsUser.PerformLayout();
            this.GroupBoxUserData.ResumeLayout(false);
            this.GroupBoxUserData.PerformLayout();
            this.ContextMenuStripAuthentication.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBoxUsers;
        private System.Windows.Forms.ComboBox ComboBoxGroup;
        private System.Windows.Forms.TextBox TextBoxAuthencationName;
        private System.Windows.Forms.ComboBox ComboBoxAuthentication;
        private System.Windows.Forms.ComboBox ComboBoxRole;
        private System.Windows.Forms.TextBox TextBoxRepeatPassword;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.TextBox TextBoxWindowsAuthencation;
        private System.Windows.Forms.TextBox TextBoxLoggedInUserRole;
        private System.Windows.Forms.TextBox TextBoxLoggedInUserName;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStripAuthentication;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCurrentUser;
        private System.Windows.Forms.DataGridView DataGridViewUsers;
        private System.Windows.Forms.TextBox TextBoxFullName;
        internal System.Windows.Forms.Label LabelName;
        internal System.Windows.Forms.GroupBox GroupBoxUserData;
        internal System.Windows.Forms.Label LabelGroup;
        internal System.Windows.Forms.Label LabelAuthencationName;
        internal System.Windows.Forms.Label LabelAuthencation;
        internal System.Windows.Forms.Label LabelRole;
        internal System.Windows.Forms.Label LabelRepeatPassword;
        internal System.Windows.Forms.Label LabelPassword;
        internal System.Windows.Forms.Button ButtonCancel;
        internal System.Windows.Forms.Button ButtonAlterUser;
        internal System.Windows.Forms.Button ButtonDeleteUser;
        internal System.Windows.Forms.Button ButtonCreateUser;
        internal System.Windows.Forms.GroupBox GroupBoxLoggedInAsUser;
        internal System.Windows.Forms.Label LoggedInAuthenticationLabel;
        internal System.Windows.Forms.Label LoggedInRoleLabel;
        internal System.Windows.Forms.Label LoggedInNameLabel;
        internal System.Windows.Forms.Label LabelFullName;
        internal System.Windows.Forms.Button ButtonClose;
    }
}