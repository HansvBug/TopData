namespace TopData
{
    using System;
    using System.Data;
    using System.Data.SQLite;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Manage the user login when the app. starts.
    /// </summary>
    public class TdUserLogIn : TdSQliteDatabaseConnection, IDisposable
    {
        #region Properies

        private string ErrorType { get; set; }
        #endregion Properies

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdUserLogIn"/> class.
        /// </summary>
        public TdUserLogIn()
        {
            // Default
        }
        #endregion constructor

        /// <summary>
        /// Validate the ueer name and password.
        /// </summary>
        /// <param name="userPassword">User passsword.</param>
        /// <param name="userName">User name.</param>
        /// <returns>True if name and password are correct.</returns>
        public bool ValidateInlog(SecureString userPassword, string userName)
        {
            this.ErrorType = ErrorTypes.UnknownError;

            using TdSecurityExtensions securityExt = new();
            string unsecurePwd = securityExt.UnSecureString(userPassword);

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(unsecurePwd))
            {
                bool loginIsValide = false;
                string selectSQL = string.Format("SELECT USERNAME, ID, PASSWORD, SALT, ROLE_NAME, USER_VALIDATE FROM {0} WHERE USERNAME = @USERNAME", TdTableName.USER_LIST);

                this.DbConnection.Open();

                SQLiteCommand command = new(selectSQL, this.DbConnection);

                command.Prepare();
                command.Parameters.Add(new SQLiteParameter("@USERNAME", userName));

                try
                {
                    SQLiteDataReader reader = command.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(reader);
                    if (dt.Rows.Count > 0)
                    {
                        // Loop through the passwords because it is possible that users have the same name but different passwords
                        foreach (DataRow row in dt.Rows)
                        {
                            // If user names are equal then check password;
                            if (userName == row["USERNAME"].ToString())
                            {
                                if (!string.IsNullOrEmpty(row["SALT"].ToString()))
                                {
                                    if (EncryptDecryptUserData.VerifyPassword(unsecurePwd, Convert.FromBase64String(row["SALT"].ToString()), Convert.FromBase64String(row["PASSWORD"].ToString())))
                                    { // User and password are corrrect
                                      // Check if user and user_role are the same. (Avoid copy user_name with a db tool)
                                        unsecurePwd = string.Empty;
                                        string userValidate = row["ROLE_NAME"].ToString();

                                        var usereroleHash = EncryptDecryptUserData.ComputeHash(userName + row["USERNAME"].ToString(), Convert.FromBase64String(row["SALT"].ToString()));  // To avoid copy role to an other user the combi username+rolename ara saved
                                        if (EncryptDecryptUserData.VerifyPassword(userName + userValidate, Convert.FromBase64String(row["SALT"].ToString()), Convert.FromBase64String(row["USER_VALIDATE"].ToString())))
                                        {
                                            TdUserData.UserName = row["USERNAME"].ToString();
                                            TdUserData.InlogSalt = row["SALT"].ToString();
                                            TdUserData.UserPassword = securityExt.ConvertToSecureString(row["PASSWORD"].ToString());
                                            TdUserData.UserRole = row["ROLE_NAME"].ToString();
                                            TdUserData.UserId = row["ID"].ToString();
                                            loginIsValide = true;

                                            // Set error type to empty. (empty then login is correct)
                                            if (!string.IsNullOrEmpty(this.ErrorType))
                                            {
                                                this.ErrorType = string.Empty;
                                            }
                                        }
                                        else
                                        {
                                            TdLogging.WriteToLogInformation("De gebruiker en de rol zijn niet meer met elkaar in overeenstemming.");
                                            TdLogging.WriteToLogInformation("ROL_NAME is waarschijnlijk direct in de database ingevoerd.");

                                            loginIsValide = false;

                                            // If emtpy there is allready a good login else give the errortype a new value
                                            if (!string.IsNullOrEmpty(this.ErrorType))
                                            {
                                                this.ErrorType = ErrorTypes.CombiRoleUser;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!loginIsValide)
                                        {
                                            if (!string.IsNullOrEmpty(this.ErrorType))
                                            {
                                                this.ErrorType = ErrorTypes.InvalidUserOrPassword;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!loginIsValide)
                                    {
                                        if (!string.IsNullOrEmpty(this.ErrorType))
                                        {
                                            this.ErrorType = ErrorTypes.EmptyColumn;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!loginIsValide)
                                {
                                    if (!string.IsNullOrEmpty(this.ErrorType))
                                    {
                                        this.ErrorType = ErrorTypes.InvalidUserOrPassword;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!loginIsValide)
                        {
                            if (!string.IsNullOrEmpty(this.ErrorType))
                            {
                                this.ErrorType = ErrorTypes.UnkownUser;
                            }
                        }
                    }

                    dt.Dispose();
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Opvragen naam en wachtwoord is mislukt.");
                    TdLogging.WriteToLogError("Melding : ");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    return loginIsValide;
                }
                finally
                {
                    command.Dispose();
                }

                if (!loginIsValide)
                {
                    this.ShowErrorMessage();
                }

                return loginIsValide;
            }
            else
            {
                this.ShowErrorMessage();
                return false;
            }
        }

        private void ShowErrorMessage()
        {
            if (this.ErrorType == ErrorTypes.CombiRoleUser)
            {
                MessageBox.Show(MB_Text.User_Role_Not_In_Sync, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TdLogging.WriteToLogError("Gebruiker en rol van de gebruiker komen niet meer met elkaar overeen.");
            }
            else if (this.ErrorType == ErrorTypes.InvalidUserOrPassword)
            {
                MessageBox.Show(
                    "Gebruiker of Wachtwoord is onjuist." + Environment.NewLine +
                    Environment.NewLine +
                    "Probeer het opnieuw.", MB_Title.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                TdLogging.WriteToLogError("Gebruiker of wachtwoord is onjuist.");
            }
            else if (this.ErrorType == ErrorTypes.EmptyColumn)
            {
                MessageBox.Show(MB_Text.Column_Is_Empty, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TdLogging.WriteToLogError("Een verplichte kolom in de tabel is leeg.");
            }
            else if (this.ErrorType == ErrorTypes.UnkownUser)
            {
                MessageBox.Show(MB_Text.User_Data_Does_not_Exist, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Information);
                TdLogging.WriteToLogInformation("De gebruikergegevens komen niet voor.");
            }
            else if (this.ErrorType == ErrorTypes.EmptyUserNameOrPassword)
            {
                MessageBox.Show("Naam of wachtwoord is niet ingevuld.", MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TdLogging.WriteToLogInformation("Naam of wachtwoord vergeten in te vullen.");
            }
            else
            {
                MessageBox.Show("Onbekende fout opgetreden tijdens het inloggen.", MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                TdLogging.WriteToLogInformation("Onbekende fout opgetreden tijdens het inloggen.");
            }
        }

        #region IDisposable

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
                }
            }

            // Free your own state (unmanaged objects).
            // Set large fields to null.
            this.disposed = true;
        }
        #endregion

        /// <summary>
        /// Returns error types.
        /// </summary>
        internal class ErrorTypes
        {
            /// <summary>
            /// Gets error type Unknown.
            /// </summary>
            internal static string UnknownError
            {
                get { return "Onbekend"; }
            }

            /// <summary>
            /// Gets the Combi Role-user.
            /// </summary>
            internal static string CombiRoleUser
            {
                get { return "De combinatie van gebruiker-rol is onjuist."; }
            }

            /// <summary>
            /// Gets Invalid user password warning.
            /// </summary>
            internal static string InvalidUserOrPassword
            {
                get { return "Gebruiker of wachtwoord onjuist."; }
            }

            /// <summary>
            /// Gets Empty column warning.
            /// </summary>
            internal static string EmptyColumn
            {
                get { return "Kolom is leeg."; }
            }

            /// <summary>
            /// Gets the user data is not correct warning.
            /// </summary>
            internal static string UnkownUser
            {
                get { return "De gebruikergegevens komen niet voor."; }
            }

            /// <summary>
            /// Gets the user or password is empry warning.
            /// </summary>
            internal static string EmptyUserNameOrPassword
            {
                get { return "Naam of wachtwoord is leeg."; }
            }
        }
    }
}
