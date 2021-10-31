namespace TopData
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// User login form.
    /// </summary>
    public partial class FormUserLogin : Form
    {
        private dynamic JsonObjSettings { get; set; }

        private bool AccesIsTrue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUserLogin"/> class.
        /// </summary>
        public FormUserLogin()
        {
            this.InitializeComponent();
        }

        private void ButtonAgain_Click(object sender, EventArgs e)
        {
            this.TextBoxUser.Clear();
            this.TextBoxPassword.Clear();
            this.TextBoxUser.Focus();
        }

        private void TextBoxUser_Leave(object sender, EventArgs e)
        {
            TdVisual.TxtLeave(sender, e);
        }

        private void TextBoxUser_Enter(object sender, EventArgs e)
        {
            TdVisual.TxtEnter(sender, e);
        }

        #region from load
        private void FormUserLogin_Load(object sender, EventArgs e)
        {
            this.Text = ResourceText.FrmUserLoginInText;
            this.ButtonLogIn.Enabled = false;
            this.ButtonAgain.Enabled = false;
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new ();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }
        #endregion from load

        #region close form
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            TdUserData.AccesIsTrue = this.AccesIsTrue; // If an user was already logged in and change user fails then keep the parent access is true and stay logged in
            TdUserData.ChangeUser = false;
            this.Close();
        }

        #endregion close form

        private void TextBoxUser_TextChanged(object sender, EventArgs e)
        {
            this.EnbleButtons();
        }

        private void EnbleButtons()
        {
            if (!string.IsNullOrEmpty(this.TextBoxUser.Text) && !string.IsNullOrEmpty(this.TextBoxPassword.Text))
            {
                this.ButtonLogIn.Enabled = true;
            }
            else
            {
                this.ButtonLogIn.Enabled = false;
            }

            if (!string.IsNullOrEmpty(this.TextBoxUser.Text) || !string.IsNullOrEmpty(this.TextBoxPassword.Text))
            {
                this.ButtonAgain.Enabled = true;
            }
            else
            {
                this.ButtonAgain.Enabled = false;
            }
        }

        private void ButtonLogIn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            using TdSecurityExtensions securityExt = new();
            using TdUserLogIn userLogin = new ();
            if (userLogin.ValidateInlog(securityExt.ConvertToSecureString(this.TextBoxPassword.Text), this.TextBoxUser.Text))
            {
                TdUserData.AccesIsTrue = true;  // Enable munu's and buttons

                TdUserData.ChangeUser = true;
                this.AccesIsTrue = true;
                TdLogging.WriteToLogInformation(string.Format("Gebruiker '{0}' is ingelogd.", TdUserData.UserName));
                this.Close();
            }
            else
            {
                TdLogging.WriteToLogWarning(string.Format("Inloggen gebruiker '{0}' is mislukt.", TdUserData.UserName));
            }

            Cursor.Current = Cursors.Default;
        }
    }
}
