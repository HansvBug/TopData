namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// User data.
    /// </summary>
    public class TdMaintainUsers : TdSQliteDatabaseConnection, IDisposable
    {
        #region fields

        private readonly Dictionary<string, int> userParam = new();

        private DataTable dt;

        private Users allUsers;

        private User aUser;

        #endregion fields

        #region properties

        /// <summary>
        /// Gets or sets a datatable.
        /// </summary>
        public DataTable Dt
        {
            get { return this.dt; }
            set { this.dt = value; }
        }

        /// <summary>
        /// Gets or sets a reference of User. Holds data of a user.
        /// </summary>
        public User AUser
        {
            get { return this.aUser; }
            set { this.aUser = value; }
        }

        /// <summary>
        /// Gets or sets a reference allusers.
        /// </summary>
        public Users AllUsers
        {
            get { return this.allUsers; }
            set { this.allUsers = value; }
        }

        /// <summary>
        /// Gets or sets a new guid.
        /// </summary>
        public string NewGuid { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user (First name and last name.
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Gets or sets the uesers password.
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        public string UserRole { get; set; }

        /// <summary>
        /// Gets or sets the user group.
        /// </summary>
        public string UserGroup { get; set; }

        /// <summary>
        /// Gets or sets the authentication.
        /// </summary>
        public string Authentication { get; set; }

        /// <summary>
        /// Gets or sets active user role.
        /// </summary>
        public string UserRoleActive { get; set; }

        /// <summary>
        /// Gets or sets the users authentication.
        /// </summary>
        public string UserAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the encryption salt.
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Gets or sets the id of the user.
        /// </summary>
        public int Id { get; set; }

        #endregion properties

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainUsers"/> class.
        /// </summary>
        public TdMaintainUsers()
        {
            this.FillUserRolesIds();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintainUsers"/> class.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        public TdMaintainUsers(int id)
        {
            this.Id = id;
            this.NewGuid = System.Guid.NewGuid().ToString();
            this.FillUserRolesIds();
        }

        private void FillUserRolesIds()
        {
            this.userParam.Add(TdRoleTypes.Owner, 0);
            this.userParam.Add(TdRoleTypes.System, 1);
            this.userParam.Add(TdRoleTypes.Administrator, 2);
            this.userParam.Add(TdRoleTypes.Muteren, 3);
            this.userParam.Add(TdRoleTypes.Raadplegen, 4);
        }
        #endregion constructor

        /// <summary>
        /// Check if the user data is unique.
        /// </summary>
        /// <returns>True if the user data is unique.</returns>
        public bool CheckIfUserIsUnique() // Check if the username exists
        {
            this.DbConnection.Open();
            SQLiteCommand command = new ("select USERNAME from " + TdTableName.USER_LIST + " where USERNAME = @USERNAME", this.DbConnection);
            command.Prepare();
            command.Parameters.Add(new SQLiteParameter("@USERNAME", this.UserName));
            try
            {
                using SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    string userName = dr.GetString(0);

                    if (userName == this.UserName)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Fout bij het bepalen of een gebruikernaam uniek is.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                return false;
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }
        }

        /// <summary>
        /// Check if the user authentication is unique in the table USER_LIST.
        /// </summary>
        /// <returns>True if data is unique.</returns>
        public bool CheckIfAuthenticationIsUnique()
        {
            this.DbConnection.Open();
            SQLiteCommand command = new ("select USER_AUTHENTICATION, USERNAME from " + TdTableName.USER_LIST + " where USER_AUTHENTICATION = @USER_AUTHENTICATION", this.DbConnection);
            command.Prepare();
            command.Parameters.Add(new SQLiteParameter("@USER_AUTHENTICATION", this.UserAuthentication));
            try
            {
                using SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    string authentication = dr.GetString(0);
                    string username = dr.GetString(1);

                    if (authentication.ToUpperInvariant() == this.UserAuthentication.ToUpperInvariant() && username == this.UserName)
                    {
                        return true;
                    }
                    else if (authentication.ToUpperInvariant() == this.UserAuthentication.ToUpperInvariant())
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Fout bij het ophalen van de authenticatie naamn.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                return false;
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <returns>True if user is created.</returns>
        public bool CreateUser()
        {
            bool succes = false;
            string insertSql;

            var passwordSalt = EncryptDecryptUserData.GenerateSalt();
            var passwordHash = EncryptDecryptUserData.ComputeHash(this.UserPassword, passwordSalt);
            var usereroleHash = EncryptDecryptUserData.ComputeHash(this.UserName + this.UserRole, passwordSalt);  // To avoid copy role to an other user the combi username+rolename are saved

            // Id=-1 => New User
            if (this.Id == -1)
            {
                insertSql = "insert into " + TdTableName.USER_LIST + "(GUID, USERNAME, PASSWORD, SALT, ROLE_NAME, GROUP_NAME, DATE_CREATED, CREATED_BY, USER_VALIDATE, ROLE_ID, AUTHENTICATION, USER_AUTHENTICATION, USERNAME_FULL) ";
                insertSql += "values (@GUID, @USERNAME, @PASSWORD, @SALT ,@ROLE_NAME,  @GROUP_NAME, @DATE_CREATED, @CREATED_BY, @USER_VALIDATE, @ROLE_ID, @AUTHENTICATION, @USER_AUTHENTICATION, @USERNAME_FULL)";

                this.DbConnection.Open();
                SQLiteCommand command = new (insertSql, this.DbConnection);
                command.Prepare();
                command.Parameters.Add(new SQLiteParameter("@USERNAME", this.UserName));
                command.Parameters.Add(new SQLiteParameter("@PASSWORD", Convert.ToBase64String(passwordHash)));
                command.Parameters.Add(new SQLiteParameter("@SALT", Convert.ToBase64String(passwordSalt)));
                command.Parameters.Add(new SQLiteParameter("@ROLE_NAME", this.UserRole));
                command.Parameters.Add(new SQLiteParameter("@GROUP_NAME", this.UserGroup));
                command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));
                command.Parameters.Add(new SQLiteParameter("@GUID", this.NewGuid));
                command.Parameters.Add(new SQLiteParameter("@USER_VALIDATE", Convert.ToBase64String(usereroleHash)));
                command.Parameters.Add(new SQLiteParameter("@ROLE_ID", this.userParam[this.UserRole]));
                command.Parameters.Add(new SQLiteParameter("@USERNAME_FULL", this.UserFullName));

                if (this.Authentication.ToUpperInvariant() == "NEE" || this.Authentication.ToUpperInvariant() == "FALSE")
                {
                    command.Parameters.Add(new SQLiteParameter("@AUTHENTICATION", false));
                }

                if (this.Authentication.ToUpperInvariant() == "JA" || this.Authentication.ToUpperInvariant() == "TRUE")
                {
                    command.Parameters.Add(new SQLiteParameter("@AUTHENTICATION", true));
                }

                command.Parameters.Add(new SQLiteParameter("@USER_AUTHENTICATION", this.UserAuthentication));

                try
                {
                    command.ExecuteNonQuery();
                    TdLogging.WriteToLogInformation("De gegevens van een nieuwe gebruiker zijn opgeslagen.");

                    MessageBox.Show(MB_Text.New_User, MB_Title.Create_New_User, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    succes = true;
                }
                catch (SQLiteException ex)
                {
                    succes = false;

                    TdLogging.WriteToLogError("De gegevens van de nieuwe gebruiker zijn niet opgeslagen.");
                    MessageBox.Show(
                        "Aanmaken nieuwe gebruiker is mislukt." + Environment.NewLine +
                        Environment.NewLine +
                        "Controleer het logbestand.", MB_Title.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
                finally
                {
                    command.Dispose();
                    this.DbConnection.Close();
                }
            }
            else
            {
                TdLogging.WriteToLogError("Muteren gebruikergegevens is mislukt.");
                TdLogging.WriteToLogError(string.Format("ID is onjuist: {0}.", this.Id.ToString()));

                MessageBox.Show(
                    "Het muteren van de gegevens is mislukt." + Environment.NewLine +
                    Environment.NewLine +
                    "Controleer het logbestand.", MB_Title.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                succes = false;
            }

            return succes;
        }

        /// <summary>
        /// Delete the data of a user.
        /// </summary>
        /// <returns>True if data is deleted.</returns>
        public bool DeleteUser()
        {
            bool succes = false;

            this.DbConnection.Open();
            SQLiteCommand command = new ("DELETE FROM " + TdTableName.USER_LIST + " WHERE ID = @ID and GUID = @GUID", this.DbConnection);
            command.Prepare();
            command.Parameters.Add(new SQLiteParameter("@ID", this.Id));
            command.Parameters.Add(new SQLiteParameter("@GUID", this.NewGuid));

            try
            {
                command.ExecuteNonQuery();
                TdLogging.WriteToLogInformation("De gebruiker is verwijderd. (ID was : " + this.Id + ")");
                this.DeleteUserSettings(); // Delete the settings from the deleted user
                succes = true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(MB_Text.Selected__User_Not_Deleted, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                TdLogging.WriteToLogError("Het verwijderen van de gebruiker is mislukt. (ID : " + this.Id + ").");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);

                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                succes = false;
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }

            return succes;
        }

        private void DeleteUserSettings()
        {
            SQLiteCommand command = new("DELETE FROM " + TdTableName.SETTINGS_APP + " WHERE USER_ID = @ID", this.DbConnection);
            command.Prepare();
            command.Parameters.Add(new SQLiteParameter("@ID", this.Id));

            try
            {
                command.ExecuteNonQuery();
                TdLogging.WriteToLogInformation("De instellingen van de gebruiker zijn verwijderd. (Was ID : " + this.Id + ").");
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Het verwijderen van de instellingen van de verwijderde gebruiker is mislukt. (ID gebruiker: " + this.Id + ").");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }
            }
            finally
            {
                command.Dispose();
            }
        }

        /// <summary>
        /// Get all the users from table USER_LIST.
        /// </summary>
        public void GetAllUsers()
        {
            TdLogging.WriteToLogInformation("Ophalen gegevens alle gebruikers.");

            this.AllUsers = new Users();
            string selectSql = string.Empty;

            if (this.UserRoleActive == TdRoleTypes.Owner)
            {
                selectSql = string.Format("SELECT ID, GUID, USERNAME, USERNAME_FULL, PASSWORD, ROLE_ID, ROLE_NAME, GROUP_ID, GROUP_NAME, AUTHENTICATION, USER_AUTHENTICATION FROM {0}", TdTableName.USER_LIST); // Get all the users
            }

            if (this.UserRoleActive == TdRoleTypes.System)
            {
                selectSql = string.Format("SELECT ID, GUID, USERNAME, USERNAME_FULL, PASSWORD, ROLE_ID, ROLE_NAME, GROUP_ID, GROUP_NAME, AUTHENTICATION, USER_AUTHENTICATION FROM {0} WHERE ROLE_NAME in ('{1}', '{2}', '{3}', '{4}')", TdTableName.USER_LIST, TdRoleTypes.System, TdRoleTypes.Administrator, TdRoleTypes.Muteren, TdRoleTypes.Raadplegen); // Get all the users  (as Administrator)
            }

            if (this.UserRoleActive == TdRoleTypes.Administrator)
            {
                selectSql = string.Format("SELECT ID, GUID, USERNAME, USERNAME_FULL, PASSWORD, ROLE_ID, ROLE_NAME, GROUP_ID, GROUP_NAME, AUTHENTICATION, USER_AUTHENTICATION FROM {0} WHERE ROLE_NAME in ('{1}', '{2}', '{3}')", TdTableName.USER_LIST, TdRoleTypes.Administrator, TdRoleTypes.Muteren, TdRoleTypes.Raadplegen);
            }

            if (this.UserRoleActive == TdRoleTypes.Muteren)
            {
                selectSql = string.Format("SELECT ID, GUID, USERNAME, USERNAME_FULL, PASSWORD, ROLE_ID, ROLE_NAME, GROUP_ID, GROUP_NAME, AUTHENTICATION, USER_AUTHENTICATION FROM {0} WHERE ROLE_NAME in ('{1}', '{2}')", TdTableName.USER_LIST, TdRoleTypes.Muteren, TdRoleTypes.Raadplegen);
            }

            if (this.UserRoleActive == TdRoleTypes.Raadplegen)
            {
                selectSql = string.Format("SELECT ID, GUID, USERNAME, USERNAME_FULL, PASSWORD, ROLE_ID, ROLE_NAME, GROUP_ID, GROUP_NAME, AUTHENTICATION, USER_AUTHENTICATION FROM {0} WHERE ROLE_NAME = '{1}'", TdTableName.USER_LIST, TdRoleTypes.Raadplegen);
            }

            this.DbConnection.Open();

            try
            {
                SQLiteCommand command = new (selectSql, this.DbConnection);

                SQLiteDataAdapter da = new (command);
                DataSet ds = new ();

                try
                {
                    if (da != null)
                    {
                        da.Fill(ds);
                        this.Dt = ds.Tables[0];
                    }
                    else
                    {
                        TdLogging.WriteToLogError("Fout bij het ophalen van alle gebruikers.");
                    }
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError(string.Format("Fout bij het ophalen van alle gegevens uit de tabel {0}.", TdTableName.USER_LIST));
                    TdLogging.WriteToLogError("Melding:");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
                finally
                {
                    command.Dispose();
                    this.DbConnection.Close();
                }
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Het ophalen van alle gebruikersnamen is mislukt.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                MessageBox.Show(
                    "Het ophalen van alle gebruikersnamen is mislukt." + Environment.NewLine +
                    Environment.NewLine +
                    "Controleer het logbestand.", MB_Title.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Get the salt value.
        /// </summary>
        /// <returns>The salt value.</returns>
        public string GetUserSalt()
        {
            string salt = string.Empty;

            this.DbConnection.Open();
            SQLiteCommand command = new (string.Format("SELECT SALT FROM {0} WHERE ID = @ID and GUID = @GUID", TdTableName.USER_LIST), this.DbConnection);
            command.Prepare();
            command.Parameters.Add(new SQLiteParameter("@ID", this.Id));
            command.Parameters.Add(new SQLiteParameter("@GUID", this.NewGuid));

            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    salt = dr.GetString(0);
                }

                dr.Close();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Onverwachte fout bij het ophalen van gebruiker gegevens.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }

            return salt;
        }

        /// <summary>
        /// Get the password of the requested user from table USER_LIST.
        /// </summary>
        /// <returns>The users password.</returns>
        public string GetPasswordUser()
        {
            string pwd = string.Empty;

            this.DbConnection.Open();
            SQLiteCommand command = new (string.Format("SELECT PASSWORD FROM {0} WHERE ID = @ID and GUID = @GUID", TdTableName.USER_LIST), this.DbConnection);
            command.Prepare();
            command.Parameters.Add(new SQLiteParameter("@ID", this.Id));
            command.Parameters.Add(new SQLiteParameter("@GUID", this.NewGuid));

            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    pwd = dr.GetString(0);
                }

                dr.Close();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(string.Format("Onverwachte fout bij het ophalen van gebruiker gegevens. (Betreft ID = {0}).", this.Id));
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }

            return pwd;
        }

        /// <summary>
        /// Update the user data.
        /// </summary>
        /// <param name="updateType">Type = 0 means update all data, 1 means update all except the password.</param>
        /// <returns>true if success.</returns>
        public bool UpdateUserData(int updateType)
        {
            bool faulted = true;
            string updateSql = string.Empty;

            this.Salt = this.GetUserSalt();
            var passwordSalt = Convert.FromBase64String(this.Salt);  // Get the excisting salt
            var passwordHash = EncryptDecryptUserData.ComputeHash(this.UserPassword, passwordSalt);
            var usereroleHash = EncryptDecryptUserData.ComputeHash(this.UserName + this.UserRole, passwordSalt);  // To avoid copy role to an other user the combi username+rolenam are saved

            if (updateType == 0)
            {
                updateSql = string.Format("UPDATE {0} SET USERNAME = @USERNAME, USERNAME_FULL = @USERNAME_FULL, PASSWORD = @PASSWORD, ROLE_NAME = @ROLE_NAME, GROUP_NAME = @GROUP_NAME, ", TdTableName.USER_LIST);
                updateSql += " DATE_ALTERED = @DATE_ALTERED, CREATED_BY = @CREATED_BY, USER_VALIDATE = @USER_VALIDATE, ROLE_ID=@ROLE_ID, ";
                updateSql += " AUTHENTICATION =@AUTHENTICATION, USER_AUTHENTICATION=@USER_AUTHENTICATION";
                updateSql += " WHERE ID = @ID and GUID = @GUID";
            }
            else if (updateType == 1)
            {
                updateSql = string.Format("UPDATE {0} SET USERNAME = @USERNAME, USERNAME_FULL = @USERNAME_FULL, ROLE_NAME = @ROLE_NAME, GROUP_NAME = @GROUP_NAME, ", TdTableName.USER_LIST);
                updateSql += " DATE_ALTERED = @DATE_ALTERED, CREATED_BY = @CREATED_BY, USER_VALIDATE = @USER_VALIDATE, ROLE_ID=@ROLE_ID, ";
                updateSql += " AUTHENTICATION =@AUTHENTICATION, USER_AUTHENTICATION=@USER_AUTHENTICATION";
                updateSql += " WHERE ID = @ID and GUID = @GUID";
            }

            this.DbConnection.Open();
            SQLiteCommand command = new (updateSql, this.DbConnection);
            command.Prepare();
            command.Parameters.Add(new SQLiteParameter("@ID", this.Id));
            command.Parameters.Add(new SQLiteParameter("@GUID", this.NewGuid));
            command.Parameters.Add(new SQLiteParameter("@USERNAME", this.UserName));
            command.Parameters.Add(new SQLiteParameter("@USERNAME_FULL", this.UserFullName));
            command.Parameters.Add(new SQLiteParameter("@ROLE_NAME", this.UserRole));
            command.Parameters.Add(new SQLiteParameter("@GROUP_NAME", this.UserGroup));
            command.Parameters.Add(new SQLiteParameter("@DATE_ALTERED", DateTime.Now));
            command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));
            command.Parameters.Add(new SQLiteParameter("@USER_VALIDATE", Convert.ToBase64String(usereroleHash)));
            command.Parameters.Add(new SQLiteParameter("@ROLE_ID", this.userParam[this.UserRole]));

            if (this.Authentication == "Ja" || this.Authentication == "True")
            {
                command.Parameters.Add(new SQLiteParameter("@AUTHENTICATION", true));
            }

            if (this.Authentication == "Nee" || this.Authentication == "False")
            {
                command.Parameters.Add(new SQLiteParameter("@AUTHENTICATION", false));
            }

            command.Parameters.Add(new SQLiteParameter("@USER_AUTHENTICATION", this.UserAuthentication));

            if (updateType == 0)
            {
                command.Parameters.Add(new SQLiteParameter("@PASSWORD", Convert.ToBase64String(passwordHash)));
            }

            try
            {
                command.ExecuteNonQuery();
                TdLogging.WriteToLogInformation("De gegevens van de gebruiker zijn gewijzigd. (ID : " + this.Id + ").");
                MessageBox.Show(MB_Text.User_Data_Changed, MB_Title.Change_data, MessageBoxButtons.OK, MessageBoxIcon.Information);
                faulted = false;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(MB_Text.Data_User_Not_Changed, MB_Title.Change_data, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                TdLogging.WriteToLogError("Het wijzigen van de gegevens van de gebruiker is mislukt. (ID : " + this.Id + ").");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                faulted = true;
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }

            return faulted;
        }

        /// <summary>
        /// Get the user authentication from table USER_LIST.
        /// </summary>
        /// <returns>The user authentication data.</returns>
        public string GetUserAuthentication()
        {
            string userAutentication = string.Empty;

            this.DbConnection.Open();
            SQLiteCommand command = new ("SELECT USERNAME, ID, PASSWORD, ROLE_NAME, SALT, AUTHENTICATION, USER_AUTHENTICATION " +
                                          string.Format("FROM {0} order by AUTHENTICATION DESC", TdTableName.USER_LIST),
                                         this.DbConnection);
            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        try
                        {
                            if (dr.GetBoolean(5) == true)
                            {
                                if (dr.GetString(6).ToUpperInvariant() == Environment.UserName.ToUpperInvariant())
                                {
                                    if (string.IsNullOrEmpty(userAutentication))
                                    {
                                        using TdSecurityExtensions securityExt = new();

                                        userAutentication = Environment.UserName;
                                        TdUserData.EnvironmentUserName = userAutentication;
                                        TdUserData.UserName = dr.GetString(0);
                                        TdUserData.UserId = Convert.ToString(dr.GetInt16(1), CultureInfo.InvariantCulture);
                                        TdUserData.UserPassword = securityExt.ConvertToSecureString(dr.GetString(2));
                                        TdUserData.UserRole = dr.GetString(3);
                                        TdUserData.InlogSalt = dr.GetString(4);
                                    }
                                }
                            }
                        }
                        catch (SQLiteException ex)
                        {
                            TdLogging.WriteToLogError("User_authentication is niet goed gevuld in de applicatie database.");
                            TdLogging.WriteToLogError("Melding:");
                            TdLogging.WriteToLogError(ex.Message);
                            if (TdDebugMode.DebugMode)
                            {
                                TdLogging.WriteToLogDebug(ex.ToString());
                            }
                        }
                    }
                }

                dr.Close();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Onverwachte fout bij het ophalen van de user authentication.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }

            return userAutentication;
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
        #endregion IDisposable
    }

    /// <summary>
    /// Create or Alter user.
    /// </summary>
    public static class MaintainUserType
    {
        /// <summary>
        /// Gets the string "CreateUser".
        /// </summary>
        public static string CreateUser
        {
            get { return "CreateUser"; }
        }

        /// <summary>
        /// Gets the string "AlterUser".
        /// </summary>
        public static string AlterUser
        {
            get { return "AlterUser"; }
        }
    }

    /// <summary>
    /// Hold a list of users.
    /// </summary>
    public class Users
    {
        public List<User> Items { get; } = new List<User>();
    }

    /// <summary>
    /// User type.
    /// </summary>
    public class User
    {
        #region properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user full name. (Firtname and last name).
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the authentication yes/no.
        /// </summary>
        public string Authentication { get; set; }

        /// <summary>
        /// Gets or sets authentication.
        /// </summary>
        public string UserAuthentication { get; set; }
        #endregion properties
    }
}
