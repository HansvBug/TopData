﻿namespace TopData
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Create user or maintain the User data.
    /// </summary>
    public partial class FormMaintainUsers : Form
    {
        #region Fields
        private User selectedUser;
        #endregion Fields

        #region Properties
        private string UserNameOrg { get; set; }

        private string UserAuthenticationORG { get; set; }

        private string DatabaseFileName { get; set; }

        private string UserName { get; set; }

        private string UserFullName { get; set; }

        private string UserPassword { get; set; }

        private string UserRepeatPassword { get; set; }

        private string UserRole { get; set; }

        private string UserGroup { get; set; }

        private string Authentication { get; set; }

        private string UserAuthentication { get; set; }

        private int SelectedRowIndex { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        /// <summary>
        /// Gets or sets the Errortype within the is ue unique funcion.
        /// </summary>
        private string IsUserUniqueErrorType { get; set; }

        #endregion Properties

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMaintainUsers"/> class.
        /// </summary>
        public FormMaintainUsers()
        {
            this.InitializeComponent();
            this.selectedUser = new User();
            this.selectedUser = null;
        }
        #endregion constructor

        #region load form

        private void FormMaintainUsers_Load(object sender, EventArgs e)
        {
            this.Text = ResourceText.FrmUserMaintenance;

            this.TextBoxWindowsAuthencation.Text = Environment.UserName;
            this.FillComboboxRole();
            this.FillComboBoxAuthentication();
            this.LoadSettings();
            this.LoadFormPosition();
            this.GetDatabaseFileName();
            this.LoadUserTable();
            this.ActiveControl = this.TextBoxName;

            TdVisual.DataGridHighlightStyle();  // Set the highlight style for: mouse over row
        }

        private void FillComboboxRole()
        {
            if (TdUserData.UserRole == TdRoleTypes.Owner)
            {
                this.ComboBoxRole.Items.Clear();
                this.ComboBoxRole.Items.Add(TdRoleTypes.Owner);
                this.ComboBoxRole.Items.Add(TdRoleTypes.System);
                this.ComboBoxRole.Items.Add(TdRoleTypes.Administrator);
                this.ComboBoxRole.Items.Add(TdRoleTypes.Muteren);
                this.ComboBoxRole.Items.Add(TdRoleTypes.Raadplegen);
                this.ComboBoxRole.Sorted = true;
                this.ComboBoxRole.SelectedIndex = 1;
            }
            else if (TdUserData.UserRole == TdRoleTypes.System)
            {
                this.ComboBoxRole.Items.Clear();
                this.ComboBoxRole.Items.Add(TdRoleTypes.Administrator);
                this.ComboBoxRole.Items.Add(TdRoleTypes.Muteren);
                this.ComboBoxRole.Items.Add(TdRoleTypes.Raadplegen);
                this.ComboBoxRole.Items.Add(TdRoleTypes.System);
                this.ComboBoxRole.Sorted = true;
                this.ComboBoxRole.SelectedIndex = 2;
            }
            else if (TdUserData.UserRole == TdRoleTypes.Administrator)
            {
                this.ComboBoxRole.Items.Clear();
                this.ComboBoxRole.Items.Add(TdRoleTypes.Administrator);
                this.ComboBoxRole.Items.Add(TdRoleTypes.Muteren);
                this.ComboBoxRole.Items.Add(TdRoleTypes.Raadplegen);
                this.ComboBoxRole.Sorted = true;
                this.ComboBoxRole.SelectedIndex = 2;
            }
            else if (TdUserData.UserRole == TdRoleTypes.Muteren)
            {
                this.ComboBoxRole.Items.Clear();
                this.ComboBoxRole.Items.Add(TdRoleTypes.Muteren);
                this.ComboBoxRole.Items.Add(TdRoleTypes.Raadplegen);
                this.ComboBoxRole.Sorted = true;
                this.ComboBoxRole.SelectedIndex = 1;
            }
            else if (TdUserData.UserRole == TdRoleTypes.Raadplegen)
            {
                this.ComboBoxRole.Items.Clear();
                this.ComboBoxRole.Items.Add(TdRoleTypes.Raadplegen);
                this.ComboBoxRole.Sorted = true;
                this.ComboBoxRole.SelectedIndex = 0;
            }
            else
            {
                this.ComboBoxRole.Items.Clear();
                this.ComboBoxRole.Items.Add(TdRoleTypes.Raadplegen);
                this.ComboBoxRole.Sorted = true;
                this.ComboBoxRole.SelectedIndex = 0;
            }
        }

        private void FillComboBoxAuthentication()
        {
            this.ComboBoxAuthentication.Items.Clear();
            this.ComboBoxAuthentication.Items.Add("Ja");
            this.ComboBoxAuthentication.Items.Add("Nee");
            this.ComboBoxAuthentication.SelectedIndex = 1;
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new ();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }

        private void LoadFormPosition()
        {
            using TdFormPosition frmPos = new (this);
            frmPos.LoadMaintainUserdataFormPosition();
        }

        private void GetDatabaseFileName()
        {
            string dbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            this.DatabaseFileName = Path.Combine(dbLocation, TdSettingsDefault.SqlLiteDatabaseName);
        }

        private void LoadUserTable()
        {
            this.TextBoxLoggedInUserName.Text = TdUserData.UserName;

            using TdMaintainUsers loadusertable = new ()
            {
                UserRoleActive = TdUserData.UserRole,
            };
            loadusertable.GetAllUsers();
            DataTable userDt = loadusertable.Dt;
            this.DataGridViewUsers.DataSource = userDt;

            this.DataGridViewUsers.Columns[0].Visible = false;  // ID
            this.DataGridViewUsers.Columns[1].Visible = false;  // GUID
            this.DataGridViewUsers.Columns[4].Visible = false;  // PASSWORD

            this.DataGridViewUsers.Columns[5].Visible = false;  // ROLE_ID
            this.DataGridViewUsers.Columns[6].Visible = false;  // ROLE_NAME
            this.DataGridViewUsers.Columns[7].Visible = false;  // GROUP_ID
            this.DataGridViewUsers.Columns[8].Visible = false;  // GROUP_NAME
            this.DataGridViewUsers.Columns[9].Visible = false;  // AUTHENTICATION
            this.DataGridViewUsers.Columns[10].Visible = true; // USER_AUTHENTICATION

            this.ActiveControl = this.TextBoxName;
        }

        #endregion load form

        #region Close Form
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion Close Form

        #region Color the textbox/combobox on enter/leave
        private void TextBoxName_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void TextBoxName_Leave(object sender, EventArgs e)
        {
            if (this.TextBoxName.Text != this.TextBoxPassword.Text)
            {
                if ((this.TextBoxPassword.Text == this.TextBoxRepeatPassword.Text) &&
                !string.IsNullOrEmpty(this.TextBoxPassword.Text) && !string.IsNullOrEmpty(this.TextBoxRepeatPassword.Text))
                {
                    this.TextBoxRepeatPassword.BackColor = Color.LightGreen;
                }
                else
                {
                    TdVisual.TxtLeave(sender, e);
                }
            }
            else if (!string.IsNullOrEmpty(this.TextBoxName.Text) && !string.IsNullOrEmpty(this.TextBoxPassword.Text))
            {
                MessageBox.Show(
                    "Wachtwoord mag niet gelijk zijn aan de naam.",
                    "Fout.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                this.ActiveControl = this.TextBoxPassword;
            }
            else
            {
                TdVisual.TxtLeave(sender, e);
            }
        }

        private void ComboBoxRole_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);

            this.UserRole = this.ComboBoxRole.Text;

            if (this.UserRole == TdRoleTypes.Owner || this.UserRole == TdRoleTypes.System)
            {
                this.ComboBoxAuthentication.Text = ResourceText._1005;
                this.ComboBoxAuthentication.Enabled = false;
            }
            else
            {
                this.ComboBoxAuthentication.Enabled = true;
            }
        }

        private void ComboBoxAuthentication_Leave(object sender, EventArgs e)
        {
            this.EnableButtons();
            this.Authentication = this.ComboBoxAuthentication.Text;
        }

        private void TextBoxAuthencationName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
            this.UserAuthentication = this.TextBoxAuthencationName.Text;
        }

        private void ComboBoxGroup_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
            this.UserGroup = this.ComboBoxGroup.Text;
        }

        private void TextBoxPassword_TextChanged(object sender, EventArgs e)
        {
            TdVisual.TxtLengthTolarge(sender, 100);
            this.UserPassword = this.TextBoxPassword.Text;

            if ((this.TextBoxPassword.Text == this.TextBoxRepeatPassword.Text) &&
                !string.IsNullOrEmpty(this.TextBoxPassword.Text) && !string.IsNullOrEmpty(this.TextBoxRepeatPassword.Text))
            {
                this.TextBoxRepeatPassword.BackColor = Color.LightGreen;
            }
            else
            {
                TdVisual.TxtLeave(this.TextBoxRepeatPassword, e);
            }

            this.EnableButtons();
        }

        private void TextBoxRepeatPassword_TextChanged(object sender, EventArgs e)
        {
            TdVisual.TxtLengthTolarge(sender, 100);
            this.UserRepeatPassword = this.TextBoxRepeatPassword.Text;

            if ((this.TextBoxPassword.Text == this.TextBoxRepeatPassword.Text) &&
                (!string.IsNullOrEmpty(this.TextBoxPassword.Text) && !string.IsNullOrEmpty(this.TextBoxRepeatPassword.Text)))
            {
                this.TextBoxRepeatPassword.BackColor = Color.LightGreen;
            }
            else
            {
                TdVisual.TxtEnter(sender, e);
            }

            this.EnableButtons();
        }

        private void ComboBoxRole_TextChanged(object sender, EventArgs e)
        {
            this.UserRole = this.ComboBoxRole.Text;
            this.EnableButtons();
        }

        private void TextBoxAuthencationName_TextChanged(object sender, EventArgs e)
        {
            // Disables background color action
            if (this.selectedUser == null)
            {
                TdVisual.TxtLengthTolarge(sender, 100);
            }

            this.UserAuthentication = this.TextBoxAuthencationName.Text;
            this.EnableButtons();
        }

        private void ComboBoxGroup_TextChanged(object sender, EventArgs e)
        {
            TdVisual.TxtLengthTolarge(sender, 100);
            this.EnableButtons();
        }

        private void TextBoxRepeatPassword_Leave(object sender, EventArgs e)
        {
            if ((this.TextBoxPassword.Text == this.TextBoxRepeatPassword.Text) &&
                (!string.IsNullOrEmpty(this.TextBoxPassword.Text) && !string.IsNullOrEmpty(this.TextBoxRepeatPassword.Text)))
            {
                this.TextBoxRepeatPassword.BackColor = Color.LightGreen;
            }
            else
            {
                TdVisual.TxtLeave(sender, e);
            }
        }

        private void TextBoxFullName_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        private void TextBoxFullName_TextChanged(object sender, EventArgs e)
        {
            TdVisual.TxtLengthTolarge(sender, 100);
            this.UserFullName = this.TextBoxFullName.Text;
            this.EnableButtons();
        }

        private void TextBoxFullName_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }
        #endregion Color the textbox/combobox on enter/leave

        #region Validate Input length
        private void TextBoxName_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateInputLength(sender, 100);
        }

        private void TextBoxFullName_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateInputLength(sender, 100);
        }

        private void ValidateInputLength(object sender, int length)
        {
            if (sender is TextBox tb)
            {
                if (tb.Text.Length <= length)
                {
                    this.UserPassword = tb.Text;
                }
                else
                {
                   MessageBox.Show(string.Format("Maximaal {0} tekens toegestaan.", length.ToString()), MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void TextBoxName_TextChanged(object sender, EventArgs e)
        {
            TdVisual.TxtLengthTolarge(sender, 100);
            this.UserName = this.TextBoxName.Text;
            this.EnableButtons();
        }
        #endregion Validate Input length

        private void ComboBoxAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComboBoxAuthentication.Text == "Ja")
            {
                this.TextBoxAuthencationName.Enabled = true;
                this.Authentication = "Ja";
            }
            else
            {
                this.TextBoxAuthencationName.Enabled = false;
                this.TextBoxAuthencationName.Text = string.Empty;
                this.Authentication = "Nee";
            }

            this.EnableButtons();
            this.ActiveControl = this.ComboBoxAuthentication;
        }

        #region new user
        private void ButtonCreateUser_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            TdLogging.WriteToLogInformation("Aanmaken nieuwe gebruiker.");
            this.UserNameOrg = string.Empty;
            this.UserAuthenticationORG = string.Empty;

            this.UserGroup = this.ComboBoxGroup.Text;

            if (this.IsUserUnique(MaintainUserType.CreateUser))
            {
                this.CreateUser();
                this.LoadUserTable();
            }
            else
            {
                if (this.IsUserUniqueErrorType != "Authentication is not Unique")
                {
                    MessageBox.Show("De gebruiker komt reeds voor. Kies een andere andere naam.", "Informatie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("De authenticatie komt reeds voor. Kies een andere authenticatie naam.", "Informatie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.ActiveControl = this.TextBoxName;
            }

            Cursor.Current = Cursors.Default;
        }

        private bool IsUserUnique(string type) // Check, does the name of the new user exists
        {
            bool isUnique = false;
            this.IsUserUniqueErrorType = "User is not Unique";

            if (type == "CreateUser")
            {
                if (!string.IsNullOrEmpty(this.TextBoxName.Text))
                {
                    if (this.UserName != this.UserNameOrg)
                    {
                        if (this.TextBoxPassword.Text == this.TextBoxRepeatPassword.Text)
                        {
                            using TdMaintainUsers userMaintain = new()
                            {
                                UserName = this.UserName,
                                UserAuthentication = this.UserAuthentication,
                            };

                            // Check if the username allready exists
                            if (userMaintain.CheckIfUserIsUnique())
                            {
                                if (string.IsNullOrEmpty(this.UserAuthentication) || string.IsNullOrWhiteSpace(this.UserAuthentication))
                                {
                                    isUnique = true;
                                }
                                else
                                {
                                    if (userMaintain.CheckIfAuthenticationIsUnique())
                                    {
                                        isUnique = true;
                                    }
                                    else
                                    {
                                        this.IsUserUniqueErrorType = "Authentication is not Unique";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Naam gebruiker is niet ingevuld", MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (string.IsNullOrEmpty(this.TextBoxName.Text))
                    {
                        this.ActiveControl = this.TextBoxName;
                    }
                }
            }
            else if (type == "AlterUser")
            {
                // Current username and authentication do not change
                if (this.UserName == this.UserNameOrg && this.UserAuthentication == this.UserAuthenticationORG)
                {
                    if (this.TextBoxPassword.Text == this.TextBoxRepeatPassword.Text)
                    {
                        isUnique = true;
                    }
                }
                else
                {
                    if (this.TextBoxPassword.Text == this.TextBoxRepeatPassword.Text)
                    {
                        using TdMaintainUsers user = new();
                        user.UserName = this.UserName;
                        user.UserAuthentication = this.UserAuthentication;

                        if (!user.CheckIfUserIsUnique())
                        {
                            if (string.IsNullOrEmpty(this.UserAuthentication) || string.IsNullOrWhiteSpace(this.UserAuthentication))
                            {
                                isUnique = true;
                            }
                            else
                            {
                                if (user.CheckIfAuthenticationIsUnique())
                                {
                                    isUnique = true;
                                }
                            }
                        }
                        else
                        {
                            isUnique = true;
                        }
                    }
                }
            }

            return isUnique;
        }

        private void CreateUser()
        {
            if (!string.IsNullOrEmpty(this.TextBoxName.Text) && !string.IsNullOrEmpty(this.TextBoxPassword.Text) && !string.IsNullOrEmpty(this.TextBoxRepeatPassword.Text) &&
                !string.IsNullOrEmpty(this.ComboBoxRole.Text) && !string.IsNullOrEmpty(this.ComboBoxGroup.Text) && !string.IsNullOrEmpty(this.ComboBoxAuthentication.Text))
            {
                if (this.ComboBoxAuthentication.Text == "Ja" && string.IsNullOrEmpty(this.TextBoxAuthencationName.Text))
                {
                    MessageBox.Show(MB_Text.Authentication_Name, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (this.TextBoxPassword.Text == this.TextBoxRepeatPassword.Text)
                {
                    // Create the new user
                    using TdMaintainUsers user = new(-1);   // id = -1 is new user
                    user.UserName = this.UserName;
                    user.UserFullName = this.UserFullName;
                    user.UserPassword = this.UserPassword;
                    user.UserRole = this.UserRole;
                    user.UserGroup = this.UserGroup;
                    user.Authentication = this.Authentication;
                    user.UserAuthentication = this.UserAuthentication;

                    if (user.CreateUser())
                    {
                        this.ClearEntry();
                    }
                }
                else
                {
                    MessageBox.Show(MB_Text.Password_Repeat, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(MB_Text.Not_All_Fields_Filled, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (string.IsNullOrEmpty(this.TextBoxName.Text))
                {
                    this.ActiveControl = this.TextBoxName;
                }
                else if (string.IsNullOrEmpty(this.TextBoxPassword.Text))
                {
                    this.ActiveControl = this.TextBoxPassword;
                }
                else if (string.IsNullOrEmpty(this.TextBoxRepeatPassword.Text))
                {
                    this.ActiveControl = this.TextBoxRepeatPassword;
                }
                else if (string.IsNullOrEmpty(this.ComboBoxRole.Text))
                {
                    this.ActiveControl = this.ComboBoxRole;
                }
                else if (string.IsNullOrEmpty(this.ComboBoxGroup.Text))
                {
                    this.ActiveControl = this.ComboBoxGroup;
                }
                else if (string.IsNullOrEmpty(this.ComboBoxAuthentication.Text))
                {
                    this.ActiveControl = this.ComboBoxAuthentication;
                }
                else if (string.IsNullOrEmpty(this.TextBoxAuthencationName.Text))
                {
                    this.ActiveControl = this.TextBoxAuthencationName;
                }
            }
        }
        #endregion new user

        private void ToolStripMenuItemCurrentUser_Click(object sender, EventArgs e)
        {
            using TdAppEnvironment appEnvironment = new ();
            this.TextBoxAuthencationName.Text = appEnvironment.UserName;
        }

        private void EnableButtons()
        {
            // New User
            if (this.selectedUser == null)
            {
                if (!string.IsNullOrEmpty(this.TextBoxName.Text) &&
                !string.IsNullOrEmpty(this.TextBoxPassword.Text) &&
                !string.IsNullOrEmpty(this.TextBoxRepeatPassword.Text) &&
                !string.IsNullOrEmpty(this.ComboBoxRole.Text) &&
                !string.IsNullOrEmpty(this.ComboBoxAuthentication.Text) &&
                !string.IsNullOrEmpty(this.ComboBoxGroup.Text) &&
                this.selectedUser == null)
                {
                    if (this.ComboBoxRole.Text == TdRoleTypes.Owner || this.ComboBoxRole.Text == TdRoleTypes.System ||
                        this.ComboBoxRole.Text == TdRoleTypes.Administrator || this.ComboBoxRole.Text == TdRoleTypes.Muteren ||
                        this.ComboBoxRole.Text == TdRoleTypes.Raadplegen)
                    {
                        if (this.ComboBoxAuthentication.Text == "Nee")
                        {
                            this.ButtonCreateUser.Enabled = true;
                            this.ButtonDeleteUser.Enabled = false;
                            this.ButtonAlterUser.Enabled = false;
                            this.ButtonCancel.Enabled = true;
                        }
                        else if (this.ComboBoxAuthentication.Text == "Ja" && !string.IsNullOrEmpty(this.TextBoxAuthencationName.Text))
                        {
                            this.ButtonCreateUser.Enabled = true;
                            this.ButtonDeleteUser.Enabled = false;
                            this.ButtonAlterUser.Enabled = false;
                            this.ButtonCancel.Enabled = true;
                        }
                        else
                        {
                            this.ButtonCreateUser.Enabled = false;
                            this.ButtonDeleteUser.Enabled = false;
                            this.ButtonAlterUser.Enabled = false;
                            this.ButtonCancel.Enabled = false;
                        }
                    }
                    else
                    {
                        this.ButtonCreateUser.Enabled = false;
                        this.ButtonDeleteUser.Enabled = false;
                        this.ButtonAlterUser.Enabled = false;
                        this.ButtonCancel.Enabled = true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(this.TextBoxName.Text) ||
                        string.IsNullOrEmpty(this.TextBoxPassword.Text) ||
                        string.IsNullOrEmpty(this.TextBoxRepeatPassword.Text) ||
                        string.IsNullOrEmpty(this.ComboBoxRole.Text) ||
                        string.IsNullOrEmpty(this.ComboBoxAuthentication.Text) ||
                        string.IsNullOrEmpty(this.ComboBoxGroup.Text))
                    {
                        this.ButtonCreateUser.Enabled = false;
                        this.ButtonDeleteUser.Enabled = false;
                        this.ButtonAlterUser.Enabled = false;
                        this.ButtonCancel.Enabled = false;
                    }
                }
            }

            // Alter user data
            if (this.selectedUser != null)
            {
                if (!string.IsNullOrEmpty(this.TextBoxName.Text) &&
                !string.IsNullOrEmpty(this.ComboBoxRole.Text) &&
                !string.IsNullOrEmpty(this.ComboBoxAuthentication.Text) &&
                !string.IsNullOrEmpty(this.ComboBoxGroup.Text) &&
                this.selectedUser != null)
                {
                    if (this.ComboBoxRole.Text == TdRoleTypes.Owner || this.ComboBoxRole.Text == TdRoleTypes.System ||
                        this.ComboBoxRole.Text == TdRoleTypes.Administrator || this.ComboBoxRole.Text == TdRoleTypes.Muteren ||
                        this.ComboBoxRole.Text == TdRoleTypes.Raadplegen)
                    {
                        if (this.ComboBoxAuthentication.Text == "Nee")
                        {
                            this.ButtonCreateUser.Enabled = false;
                            this.ButtonDeleteUser.Enabled = true;
                            this.ButtonAlterUser.Enabled = true;
                            this.ButtonCancel.Enabled = true;
                        }
                        else if (this.ComboBoxAuthentication.Text == "Ja" && !string.IsNullOrEmpty(this.TextBoxAuthencationName.Text))
                        {
                            this.ButtonCreateUser.Enabled = false;
                            this.ButtonDeleteUser.Enabled = true;
                            this.ButtonAlterUser.Enabled = true;
                            this.ButtonCancel.Enabled = true;
                        }
                        else
                        {
                            this.ButtonCreateUser.Enabled = false;
                            this.ButtonDeleteUser.Enabled = false;
                            this.ButtonAlterUser.Enabled = false;
                            this.ButtonCancel.Enabled = false;
                        }
                    }
                    else
                    {
                        this.ButtonCreateUser.Enabled = false;
                        this.ButtonDeleteUser.Enabled = false;
                        this.ButtonAlterUser.Enabled = false;
                        this.ButtonCancel.Enabled = true;
                    }
                }
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.ClearEntry();
            this.EnableButtons();
        }

        private void ClearEntry()
        {
            this.TextBoxName.Text = string.Empty;
            this.TextBoxFullName.Text = string.Empty;
            this.TextBoxPassword.Text = string.Empty;
            this.TextBoxRepeatPassword.Text = string.Empty;
            this.ComboBoxRole.Text = TdRoleTypes.Raadplegen;
            this.ComboBoxAuthentication.Text = "Nee";
            this.TextBoxAuthencationName.Text = string.Empty;
            this.ActiveControl = this.TextBoxName;
            this.selectedUser = null;
        }

        private void ButtonDeleteUser_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(MB_Text.User_Gets_deleted, MB_Title.Continue, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;

                string curUserName = this.TextBoxName.Text;

                // You can't delete the user which is logged on
                if (curUserName != TdUserData.UserName)
                {
                    if (TdUserData.UserRole == TdRoleTypes.Owner || TdUserData.UserRole == TdRoleTypes.System)
                    {
                        this.DeleteUser();
                    }
                    else if (TdUserData.UserRole == TdRoleTypes.Administrator)
                    {
                        if (this.UserRole == TdRoleTypes.Owner || this.UserRole == TdRoleTypes.System)
                        {
                            MessageBox.Show(MB_Text.Not_Allowed_Deleted_User, MB_Title.Attention, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            this.DeleteUser();  // Administrator can remove all users except System or Owner
                        }
                    }
                    else if (TdUserData.UserRole == TdRoleTypes.Muteren)
                    {
                        if (this.UserRole == TdRoleTypes.Owner || this.UserRole == TdRoleTypes.System || this.UserRole == TdRoleTypes.Administrator)
                        {
                            MessageBox.Show(MB_Text.Not_Allowed_Deleted_User, MB_Title.Attention, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            this.DeleteUser();
                        }
                    }
                    else if (TdUserData.UserRole == TdRoleTypes.Raadplegen)
                    {
                        MessageBox.Show(MB_Text.Not_Allowed_Deleted_Any_User, MB_Title.Attention, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(MB_Text.Not_Allowed_Deleted_Any_User, MB_Title.Attention, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Owner can delete Owner
                    if (curUserName == this.UserName && curUserName == TdRoleTypes.Owner)
                    {
                        this.DeleteUser();
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(
                            "De gebruiker '" + curUserName + "' kan niet worden verwijderd. " + Environment.NewLine +
                            "U bent in gelogd.", MB_Title.Attention,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            else if (dialogResult == DialogResult.No)
            {
                // Do nothing
            }
        }

        private void DeleteUser()
        {
            using TdMaintainUsers user = new ()
            {
                UserName = this.selectedUser.UserName,
                UserFullName = this.selectedUser.UserFullName,
                UserPassword = this.TextBoxPassword.Text,
                UserRole = this.selectedUser.RoleName,
                UserGroup = this.selectedUser.GroupName,
                NewGuid = this.selectedUser.Guid,
                Id = this.selectedUser.Id,
            };

            if (user.DeleteUser())
            {
                this.TextBoxName.Text = string.Empty;
                this.TextBoxFullName.Text = string.Empty;
                this.TextBoxPassword.Text = string.Empty;
                this.TextBoxRepeatPassword.Text = string.Empty;
                this.selectedUser = null;

                user.UserRoleActive = TdUserData.UserRole;

                MessageBox.Show(MB_Text.User_Is_Deleted, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.ButtonDeleteUser.Enabled = false;
                this.ActiveControl = this.TextBoxName;
                this.Refresh();
            }

            this.LoadUserTable();
        }

        private void ButtonAlterUser_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool loadData = false;
            this.UserGroup = this.ComboBoxGroup.Text;  // TEMP !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! remove when usergroup is used

            if (this.IsUserUnique("AlterUser"))
            {
                if (TdUserData.UserRole == TdRoleTypes.Owner || TdUserData.UserRole == TdRoleTypes.System)
                {
                    this.UpdateUserData();
                    loadData = true;
                }
                else if (TdUserData.UserRole == TdRoleTypes.Administrator)
                {
                    if (this.UserRole == "Owner" || this.UserRole == "System")
                    {
                        MessageBox.Show(MB_Text.Not_Allowd_Change_Other_User_With_Role_1, MB_Title.Change_data, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        this.UpdateUserData();  // Administrator can update all users except System or Owner
                        loadData = true;
                    }
                }
                else if (TdUserData.UserRole == TdRoleTypes.Muteren)
                {
                    if (this.UserRole == "Owner" || this.UserRole == "System" || this.UserRole == "Administrator")
                    {
                        MessageBox.Show(MB_Text.Not_Allowd_Change_Other_User_With_Role, MB_Title.Change_data, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        this.UpdateUserData();
                        loadData = true;
                    }
                }
                else if (TdUserData.UserRole == TdRoleTypes.Raadplegen)
                {
                    if (this.UserName == TdUserData.UserName)
                    {
                        this.UpdateUserData();
                        loadData = true;
                    }
                    else
                    {
                        MessageBox.Show(MB_Text.Not_Allowd_Change_Other_User, MB_Title.Change_data, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(MB_Text.Not_Allowd_Change_User, MB_Title.Change_data, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.ClearEntry();
            }

            if (loadData)
            {
                this.LoadUserTable();
            }

            if (this.SelectedRowIndex >= 0)
            {
                this.DataGridViewUsers.ClearSelection();
                this.DataGridViewUsers.Rows[this.SelectedRowIndex].Selected = false;
            }

            this.ButtonAlterUser.Enabled = false;
            this.Refresh();
            Cursor.Current = Cursors.Default;
        }

        private void UpdateUserData()
        {
            if (!string.IsNullOrEmpty(this.UserName) &&
                    !string.IsNullOrEmpty(this.UserRole) &&
                    !string.IsNullOrEmpty(this.UserGroup) &&
                    !string.IsNullOrEmpty(this.Authentication))
            {
                if (this.ComboBoxAuthentication.Text == "Ja" && string.IsNullOrEmpty(this.TextBoxAuthencationName.Text))
                {
                    MessageBox.Show(MB_Text.Enter_Authentiction_Name, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using TdMaintainUsers userMaintain = new ()
                {
                    Id = this.selectedUser.Id,
                    NewGuid = this.selectedUser.Guid,
                    UserName = this.UserName,
                    UserFullName = this.UserFullName,
                    UserPassword = this.UserPassword,
                    Authentication = this.Authentication,
                    UserAuthentication = this.UserAuthentication,
                };
                userMaintain.Salt = userMaintain.GetUserSalt();
                userMaintain.UserGroup = this.UserGroup;
                userMaintain.UserRole = this.UserRole;
                userMaintain.UserRoleActive = TdUserData.UserRole;  // The rol of the current user

                if (!string.IsNullOrEmpty(this.UserPassword) && !string.IsNullOrEmpty(this.UserRepeatPassword))
                {
                    // Update all data
                    // Password = repeat password
                    if (this.UserPassword == this.UserRepeatPassword)
                    {
                        // Check if pasword is changed
                        if (EncryptDecryptUserData.VerifyPassword(this.UserPassword, Convert.FromBase64String(userMaintain.Salt), Convert.FromBase64String(userMaintain.GetPasswordUser())))
                        {
                            // 0 = all, 1 is all without password
                            if (!userMaintain.UpdateUserData(0))
                            {
                                TdLogging.WriteToLogInformation(string.Format("De volledige gegevens van gebruiker '{0}'  zijn gewijzigd.", userMaintain.UserName));
                            }
                        }
                        else
                        {
                            DialogResult dialogResult = MessageBox.Show(
                                "Het wachtwoord komt niet overeen met het opgeslagen wachtwoord." + Environment.NewLine +
                                "Wilt u het wachtwoord wijzigen?",
                                MB_Title.Continue,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                // update the data but not the password
                                // 0 = all, 1 is all without password
                                if (!userMaintain.UpdateUserData(0))
                                {
                                    TdLogging.WriteToLogInformation(string.Format("De volledige gegevens van gebruiker '{0}'  zijn gewijzigd (m.u.v. het wachtwoord).", userMaintain.UserName));
                                }
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                TdLogging.WriteToLogInformation(string.Format("De gegevens van gebruiker '{0}'  zijn niet gewijzigd.", userMaintain.UserName));
                            }
                        }
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show(
                            @"Het ""Wachtwoord"" is ongelijk aan ""Herhaal wachtwoord""." + Environment.NewLine +
                            "Wilt u de gegevens met uitzondering van het wachtwoord wijzigen?",
                            MB_Title.Continue,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        // Update the data but not the password
                        if (dialogResult == DialogResult.Yes)
                        {
                            // 0 = all, 1 is all without password
                            if (!userMaintain.UpdateUserData(1))
                            {
                                TdLogging.WriteToLogInformation(string.Format("De volledige gegevens van gebruiker '{0}'  zijn gewijzigd (m.u.v. het wachtwoord).", userMaintain.UserName));
                            }
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            TdLogging.WriteToLogInformation(string.Format("De gegevens van gebruiker '{0}'  zijn niet gewijzigd. (Wachtwoord niet geliljk aan herhaal wachtwoord).", userMaintain.UserName));
                            this.TextBoxPassword.Focus();
                        }
                    }
                }
                else
                {
                    // Password or repeat pasword are not filled
                    DialogResult dialogResult = MessageBox.Show(
                        @"Het ""Wachtwoord"" of ""Herhaal wachtwoord"" is niet ingevuld." + Environment.NewLine +
                        "Wilt u de gegevens met uitzondering van het wachtwoord wijzigen?",
                        MB_Title.Continue,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    // Update the data but not the password
                    if (dialogResult == DialogResult.Yes)
                    {
                        // 0 = all, 1 is all without password
                        if (!userMaintain.UpdateUserData(1))
                        {
                            TdLogging.WriteToLogInformation(string.Format("De gegevens van gebruiker '{0}'  zijn gewijzigd (m.u.v. het wachtwoord).", userMaintain.UserName));
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        TdLogging.WriteToLogInformation(string.Format("De gegevens van gebruiker '{0}'  zijn niet gewijzigd.", userMaintain.UserName));
                        if (string.IsNullOrEmpty(this.UserPassword))
                        {
                            this.TextBoxPassword.Focus();
                        }
                        else if (string.IsNullOrEmpty(this.UserRepeatPassword))
                        {
                            this.TextBoxRepeatPassword.Focus();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(MB_Text.Not_All_Fields_Filled, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (string.IsNullOrEmpty(this.TextBoxName.Text))
                {
                    this.ActiveControl = this.TextBoxName;
                }
                else if (string.IsNullOrEmpty(this.ComboBoxRole.Text))
                {
                    this.ActiveControl = this.ComboBoxRole;
                }
                else if (string.IsNullOrEmpty(this.ComboBoxGroup.Text))
                {
                    this.ActiveControl = this.ComboBoxGroup;
                }
                else if (string.IsNullOrEmpty(this.ComboBoxAuthentication.Text))
                {
                    this.ActiveControl = this.ComboBoxAuthentication;
                }
                else if (string.IsNullOrEmpty(this.TextBoxAuthencationName.Text))
                {
                    this.ActiveControl = this.TextBoxAuthencationName;
                }
            }

            this.LoadUserTable();
        }

        #region Form closing
        private void FormMaintainUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveFormPosition();
            this.SaveSettings();
        }

        private void SaveFormPosition()
        {
            using TdFormPosition frmPos = new (this);
            frmPos.SaveMaintainUserdataFormPosition();
        }

        private void SaveSettings()
        {
            TdSettingsManager.SaveSettings(this.JsonObjSettings);
        }
        #endregion Form closing

        private void DataGridViewUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (e.RowIndex >= 0 && e.RowIndex <= this.DataGridViewUsers.RowCount)
                {
                    DataGridViewRow row = this.DataGridViewUsers.Rows[e.RowIndex];  // Gets a collection that contains all the rows

                    this.SelectedRowIndex = this.DataGridViewUsers.CurrentCell.RowIndex;

                    this.Authentication = row.Cells[9].Value.ToString();

                    if (this.selectedUser == null)
                    {
                        this.selectedUser = new User();
                    }

                    this.selectedUser.Id = int.Parse(row.Cells[0].Value.ToString());
                    this.selectedUser.Guid = row.Cells[1].Value.ToString();
                    this.selectedUser.UserName = row.Cells[2].Value.ToString();
                    this.selectedUser.UserFullName = row.Cells[3].Value.ToString();
                    this.selectedUser.RoleName = row.Cells[6].Value.ToString();
                    this.selectedUser.GroupName = row.Cells[8].Value.ToString();

                    this.TextBoxName.Text = this.selectedUser.UserName;
                    this.TextBoxFullName.Text = this.selectedUser.UserFullName;
                    this.ComboBoxRole.Text = this.selectedUser.RoleName;

                    if (this.Authentication == "False" || this.Authentication == ResourceText._1005)
                    {
                        this.ComboBoxAuthentication.Text = ResourceText._1005;
                    }

                    if (this.Authentication == "True" || this.Authentication == ResourceText._1006)
                    {
                        this.ComboBoxAuthentication.Text = ResourceText._1006;
                    }

                    this.UserAuthentication = row.Cells[10].Value.ToString();
                    this.UserAuthenticationORG = row.Cells[10].Value.ToString();
                    this.TextBoxAuthencationName.Text = this.UserAuthentication;

                    this.TextBoxPassword.Text = string.Empty;
                    this.TextBoxRepeatPassword.Text = string.Empty;
                    this.ActiveControl = this.TextBoxName;
                    this.EnableButtons();
                }
            }
        }

        private void ComboBoxAuthentication_TextChanged(object sender, EventArgs e)
        {
            if (this.ComboBoxAuthentication.Text == "Ja")
            {
                this.TextBoxAuthencationName.Enabled = true;
                this.Authentication = "Ja";
            }
            else
            {
                this.TextBoxAuthencationName.Enabled = false;
                this.TextBoxAuthencationName.Text = string.Empty;
                this.Authentication = "Nee";
            }

            this.EnableButtons();
        }

        #region Highlight datagrid row

        private void DataGridViewUsers_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            TdVisual.DataGridView_CellMouseEnter(sender, e);
        }

        private void DataGridViewUsers_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            TdVisual.DataGridView_CellMouseLeave(sender, e);
        }
        #endregion Highlight datagrid row
    }
}