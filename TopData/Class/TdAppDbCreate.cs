namespace TopData
{
    using System;
    using System.Data.SQLite;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Create the application database.
    /// </summary>
    public class TdAppDbCreate : TdAppDb, IDisposable
    {
        #region Query strings
        private readonly string creMetaTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.SETTINGS_META + "(" +
                                "KEY                VARCHAR(50)  UNIQUE  ," +
                                "VALUE              VARCHAR(255))";

        private readonly string creUserTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.USER_LIST + "(" +
                                "ID                  INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "GUID                VARCHAR(50)  UNIQUE                                      ," +
                                "USERNAME            VARCHAR(100)                                             ," +
                                "PASSWORD            VARCHAR(100)                                             ," +
                                "SALT                VARCHAR(255)                                             ," +
                                "ROLE_ID             INTEGER                                                  ," +
                                "ROLE_NAME           VARCHAR(100)                                             ," +
                                "GROUP_ID            INTEGER                                                  ," +
                                "GROUP_NAME          VARCHAR(100)                                             ," +
                                "USER_VALIDATE       VARCHAR(500)                                             ," + // Concat USERNAME+PASSWORD+ROLE_NAME+GUID
                                "AUTHENTICATION      BOOL                                                     ," +
                                "USER_AUTHENTICATION VARCHAR(100)                                             ," +
                                "DATE_CREATED        DATE                                                     ," +
                                "DATE_ALTERED        DATE                                                     ," +
                                "CREATED_BY          VARCHAR(100)                                             ," +
                                "ALTERED_BY          VARCHAR(100))";

        private readonly string creAppSettingsTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.SETTINGS_APP + "(" +
                                "ID                 INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE ," +
                                "GUID               VARCHAR(50)  UNIQUE                                       ," +
                                "USER_ID            INTEGER                                                   ," +
                                "USER_NAME          VARCHAR(225)                                              ," +
                                "LOGGED_IN_USER     VARCHAR(255)                                              ," +
                                "CONNECTION_ID      INTEGER                                                   ," +
                                "ITEM_DATA          VARCHAR(10000)                                            ," +
                                "ITEM               VARCHAR(225))";

        private readonly string creDatabaseConnectionTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.CONN_ORACLE + "(" +
                                "ID	              INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE   ," +
                                "CONNECTNAME       VARCHAR(255)                                               ," +
                                "SCHEMANAME        VARCHAR(50)     NOT NULL                                   ," +
                                "SCHEMAPASS        VARCHAR(500)    NOT NULL                                   ," +
                                "CONNECT           VARCHAR(255)                                               ," +
                                "DATE_CREATED      DATE                                                       ," +
                                "DATE_ALTERED      DATE                                                       ," +
                                "CREATED_BY        VARCHAR(100)                                               ," +
                                "ALTERED_BY        VARCHAR(100))";

        private readonly string creConnectionQueryTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.CONNECTION_QUERY + "(" +
                                "ID                INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE  ," +
                                "CONNECTION_ID     INTEGER                                                    ," +
                                "QUERY_ID          INTEGER                                                    ," +
                                "CREATED_BY   VARCHAR(100)                                                    ," +
                                "ALTERED_BY    VARCHAR(100))";

        private readonly string creQueryListTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.QUERY_LIST + "(" +
                                "ID	            INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE     ," +
                                "GUID            VARCHAR(50)  UNIQUE                                          ," +
                                "QUERYNAME       VARCHAR(255)                                                 ," +
                                "DESCRIPTION     VARCHAR(255)                                                 ," +
                                "FILENAME_EXP   	VARCHAR(10000)                                            ," +
                                "QUERY	        VARCHAR(900000)                                               ," + // Max = 1,000,000,000
                                "DATE_CREATED    DATE                                                         ," +
                                "DATE_ALTERED    DATE                                                         ," +
                                "CREATED_BY      VARCHAR(100)                                                 ," +
                                "FOLDER          VARCHAR(50)                                                  ," +
                                "WORKSHEET       VARCHAR(31)                                                  ," +
                                "QUERY_GROUP     VARCHAR(255)                                                 ," +
                                "LOCKED          VARCHAR(255)                                                 ," + // If encryption is stronger then the field must b larger. (was varchar(50))
                                "ALTERED_BY      VARCHAR(100))";

        private readonly string creFolderListTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.FOLDER_LIST + "(" +
                                "ID                 INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE ," +
                                "GUID               VARCHAR(50)  UNIQUE                                       ," +
                                "NAME               VARCHAR(255)                                              ," +
                                "FOLDERTYPE         INTEGER                                                   ," +
                                "PARENT             VARCHAR(50)                                               ," +
                                "DATE_CREATED       DATE                                                      ," +
                                "DATE_ALTERED       DATE                                                      ," +
                                "CREATED_BY        VARCHAR(100)                                               ," +
                                "ALTERED_BY        VARCHAR(100))";

        private readonly string creOraSchemaDatabseNamesTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.ORA_SCHEMA_DATABASE_NAME + "(" +
                                "GUID              VARCHAR(50)  UNIQUE                                        ," +
                                "SCHEMA_NAME       VARCHAR(255)                                               ," +
                                "DATABASE_NAME     VARCHAR(255))";

        private readonly string creQueryGroupTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.QUERY_GROUP + "(" +
                                "ID                  INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "GUID                VARCHAR(50)  UNIQUE                                      ," +
                                "NAME                VARCHAR(100)                                             ," +
                                "QUERY_GROUP_LIST_ID INTEGER                                                  ," +
                                "QUERY_ID            INTEGER                                                  ," +
                                "CONNECTION_ID       INTEGER                                                  ," +
                                "DATE_CREATED        DATE                                                     ," +
                                "DATE_ALTERED        DATE                                                     ," +
                                "CREATED_BY          VARCHAR(100)                                             ," +
                                "ALTERED_BY          VARCHAR(100))";

        private readonly string creQueryGroupListTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.QUERY_GROUP_LIST + "(" +
                                "ID                  INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "GUID                VARCHAR(50)  UNIQUE                                      ," +
                                "NAME                VARCHAR(100)                                             ," +
                                "CONNECTION_ID       INTEGER                                                  ," +
                                "EXPORTFILE_PATH     VARCHAR(1000)                                            ," +
                                "EXPORTFILE_NAME     VARCHAR(50)                                              ," +
                                "DATE_CREATED        DATE                                                     ," +
                                "DATE_ALTERED        DATE                                                     ," +
                                "CREATED_BY          VARCHAR(100)                                             ," +
                                "ALTERED_BY          VARCHAR(100))";

        private readonly string creQueryActiveTbl = "CREATE TABLE IF NOT EXISTS " + TdTableName.QUERY_IS_ACTIVE + "(" +
                                "ID                  INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "GUID                VARCHAR(50)  UNIQUE                                      ," +
                                "QUERY_ID            INTEGER                                                  ," +
                                "Query_GUID          VARCHAR(50)                                              ," +
                                "Query_GROUP_ID     INTEGER                                                   ," +
                                "Query_GROUP_GUID    VARCHAR(50)                                              ," +
                                "DATE_CREATED        DATE                                                     ," +
                                "CREATED_BY          VARCHAR(100))";

        #endregion Query strings

        #region Properties

        /// <summary>
        /// Gets or sets the appliction database file name.
        /// </summary>
        private string DbFileName { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdAppDbCreate"/> class.
        /// </summary>
        public TdAppDbCreate()
        {
            this.DbFileName = TdSettingsDefault.SqlLiteDatabaseName;
        }
        #endregion Constructor

        #region Create SQLite Database file

        /// <summary>
        /// Create a new database file and the tables.
        /// </summary>
        /// <returns>True if file is created.</returns>
        public bool CreateNewDatabase()
        {
            this.CreateDbFile();
            if (!this.Error)
            {
                this.Createtable(this.creMetaTbl, TdTableName.SETTINGS_META, "0");
                this.InsertMeta("0");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CreateDbFile()
        {
            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                try
                {
                    // Only with a first install. (Unless a user removed the database file)
                    if (!File.Exists(this.DatabaseFileName))
                    {
                        SQLiteConnection.CreateFile(this.DatabaseFileName); // The creation of a new empty database file
                        TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.TheAppDbIsCreated, this.DatabaseFileName));
                    }
                    else
                    {
                        TdLogging.WriteToLogInformation(TdLogging_Resources.NoNewAppDbCreated);
                    }
                }
                catch (IOException ex)
                {
                    this.Error = true;
                    TdLogging.WriteToLogError(string.Format(TdLogging_Resources.ErrorCreatingAppDbase, this.DbFileName));
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(MB_Text.App_Database_Create_Failed, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    this.Error = true;
                    TdLogging.WriteToLogError(string.Format(TdLogging_Resources.ErrorCreatingAppDbase, this.DbFileName));
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(MB_Text.App_Database_Create_Failed, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                TdLogging.WriteToLogError(TdLogging_Resources.ErrorCreatingAppDbaseNoName);
                Cursor.Current = Cursors.Default;
                MessageBox.Show(MB_Text.App_Database_Changed_No_Location, MB_Title.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Create SQLite Database file

        /// <summary>
        /// Create new tables or alter existing tables.
        /// </summary>
        /// <returns>True when create or altes has succeeded.</returns>
        public bool CreateTables()
        {
            bool succes = false;
            string version;
            if (this.LatestDbVersion >= 1 && this.SelectMeta() == 0)
            {
                version = "1";
                this.Createtable(this.creUserTbl, TdTableName.USER_LIST, version);
                this.AddInstallUserSystem(version);
                this.AddInstallUserOwner(version);

                this.Createtable(this.creAppSettingsTbl, TdTableName.SETTINGS_APP, version);
                this.Createtable(this.creDatabaseConnectionTbl, TdTableName.CONN_ORACLE, version);
                this.Createtable(this.creConnectionQueryTbl, TdTableName.CONNECTION_QUERY, version);
                this.Createtable(this.creQueryListTbl, TdTableName.QUERY_LIST, version);
                this.Createtable(this.creFolderListTbl, TdTableName.FOLDER_LIST, version);
                this.Createtable(this.creOraSchemaDatabseNamesTbl, TdTableName.ORA_SCHEMA_DATABASE_NAME, version);
                this.Createtable(this.creQueryActiveTbl, TdTableName.QUERY_IS_ACTIVE, version);

                this.SetDatabaseUserVersion(version);

                this.UpdateMeta(version);  // Set the version 1
            }

            if (this.LatestDbVersion > this.SelectMeta())
            {
                if (this.SelectMeta() < 3)
                {
                    version = "3";
                    this.Createtable(this.creQueryGroupTbl, TdTableName.QUERY_GROUP, version);
                    this.Createtable(this.creQueryGroupListTbl, TdTableName.QUERY_GROUP_LIST, version);

                    this.SetDatabaseUserVersion(version);
                    this.UpdateMeta(version);  // Set the version 3
                }

                if (this.SelectMeta() < 5)
                {
                    // version = "4";
                    // new create....
                    // this.SetDatabaseUserVersion(version);
                    // this.UpdateMeta(version);  // Set the version 4
                }
            }

            if (!this.Error)
            {
                succes = true;
            }

            return succes;
        }

        private void Createtable(string sqlCreateString, string tableName, string version)
        {
            if (!this.Error)
            {
                this.DbConnection.Open();

                SQLiteCommand command = new (sqlCreateString, this.DbConnection);
                try
                {
                    command.ExecuteNonQuery();
                    TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.TableIsCreated, tableName, version));
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError(string.Format(TdLogging_Resources.FailedToCreateTable, tableName, version));
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    this.Error = true;
                }
                finally
                {
                    command.Dispose();
                    this.DbConnection.Close();
                }
            }
            else
            {
                TdLogging.WriteToLogError(string.Format(TdLogging_Resources.TableIsNotCreated, tableName));
            }
        }

        /// <summary>
        /// Add the user System when the Install parameter is given.
        /// </summary>
        /// <param name="version">The current version of the application database.</param>
        public void AddInstallUserSystem(string version)
        {
            // Add a user with all the rights to start with.
            if (this.IsSystemUnique())
            {
                string insertSql = "INSERT INTO " + TdTableName.USER_LIST + " (GUID, USERNAME, PASSWORD, ROLE_NAME, SALT, ROLE_ID, USER_VALIDATE, ";
                insertSql += "GROUP_NAME, DATE_CREATED, CREATED_BY, USER_VALIDATE, AUTHENTICATION, USER_AUTHENTICATION) ";
                insertSql += "values(@GUID, @USERNAME, @PASSWORD, @ROLE_NAME, @SALT, @ROLE_ID, @USER_VALIDATE, ";
                insertSql += "@GROUP_NAME, @DATE_CREATED, @CREATED_BY, @USER_VALIDATE, @AUTHENTICATION, @USER_AUTHENTICATION) ";

                this.DbConnection.Open();
                SQLiteCommand command = new (insertSql, this.DbConnection);
                command.Prepare();

                command.Parameters.Add(new SQLiteParameter("@GUID", System.Guid.NewGuid().ToString()));
                command.Parameters.Add(new SQLiteParameter("@USERNAME", TdRoleTypes.System));
                command.Parameters.Add(new SQLiteParameter("@PASSWORD", "yWgm185gqjzyMPUXq9I07JgcJryEz0Lv/zgHRqepN1B84cTWINNk4EWX5/Icp1X6"));  // Welkom
                command.Parameters.Add(new SQLiteParameter("@ROLE_NAME", TdRoleTypes.System));
                command.Parameters.Add(new SQLiteParameter("@SALT", "bG0dNN7qxrX0bnEGjc6Hdms7GaESUl8jRfSh16HRFgGEBlItF/fMerhlABoXIcDd"));
                command.Parameters.Add(new SQLiteParameter("@ROLE_ID", 1));
                command.Parameters.Add(new SQLiteParameter("@GROUP_NAME", "<Groep>"));
                command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));
                command.Parameters.Add(new SQLiteParameter("@USER_VALIDATE", "YmmzetKI0+6PXUBReyOAvo5M+Vceo5D4xQGsPnfhprTDwpxlw95wYfGJRNThoIqd"));
                command.Parameters.Add(new SQLiteParameter("@AUTHENTICATION", false));
                command.Parameters.Add(new SQLiteParameter("@USER_AUTHENTICATION", string.Empty));
                try
                {
                    command.ExecuteNonQuery();
                    TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.TableIsUpdated, TdTableName.USER_LIST, version));
                }
                catch (SQLiteException ex)
                {
                    this.Error = true;
                    TdLogging.WriteToLogError(string.Format(TdLogging_Resources.ErrorEnteringUserInTable, TdTableName.USER_LIST, version));
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
        }

        private bool IsSystemUnique()
        {
            try
            {
                string selectSql = "select USERNAME from " + TdTableName.USER_LIST + " where USERNAME = @USERNAME AND PASSWORD = @PASSWORD AND ROLE_NAME = @ROLE_NAME AND SALT = SALT";

                this.DbConnection.Open();

                SQLiteCommand command = new (selectSql, this.DbConnection);
                command.Prepare();
                command.Parameters.Add(new SQLiteParameter("@USERNAME", TdRoleTypes.System));
                command.Parameters.Add(new SQLiteParameter("@PASSWORD", "yWgm185gqjzyMPUXq9I07JgcJryEz0Lv/zgHRqepN1B84cTWINNk4EWX5/Icp1X6"));  // Welkom
                command.Parameters.Add(new SQLiteParameter("@ROLE_NAME", TdRoleTypes.System));
                command.Parameters.Add(new SQLiteParameter("@SALT", "bG0dNN7qxrX0bnEGjc6Hdms7GaESUl8jRfSh16HRFgGEBlItF/fMerhlABoXIcDd"));

                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    dr.Close();
                    command.Dispose();
                    return false;
                }
                else
                {
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.DefaultUserNotFound, TdRoleTypes.System));
                    }

                    command.Dispose();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(string.Format(TdLogging_Resources.FailedToSeeIfUserIsUnique, TdRoleTypes.System));
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
                this.DbConnection.Close();
            }
        }

        /// <summary>
        /// Add the user Owner when the Install Owner parameter is given.
        /// </summary>
        /// <param name="version">the version of the application database.</param>
        public void AddInstallUserOwner(string version)
        {
            if (this.IsOwnerUnique())
            {
                string insertSql = "INSERT INTO " + TdTableName.USER_LIST + " (GUID, USERNAME, PASSWORD, ROLE_NAME, SALT, ROLE_ID, USER_VALIDATE, ";
                insertSql += "GROUP_NAME, DATE_CREATED, CREATED_BY, USER_VALIDATE, AUTHENTICATION, USER_AUTHENTICATION) ";
                insertSql += "values(@GUID, @USERNAME, @PASSWORD, @ROLE_NAME, @SALT, @ROLE_ID, @USER_VALIDATE, ";
                insertSql += "@GROUP_NAME, @DATE_CREATED, @CREATED_BY, @USER_VALIDATE, @AUTHENTICATION, @USER_AUTHENTICATION) ";

                this.DbConnection.Open();
                SQLiteCommand command = new (insertSql, this.DbConnection);
                command.Prepare();

                command.Parameters.Add(new SQLiteParameter("@GUID", System.Guid.NewGuid().ToString()));
                command.Parameters.Add(new SQLiteParameter("@USERNAME", TdRoleTypes.Owner));
                command.Parameters.Add(new SQLiteParameter("@PASSWORD", "Y2HMZa3aEpTpioqB3nC7BhB5jJThysFNwDSp7Wh24gXxh3tInfT7RCWZsy83KwGM"));
                command.Parameters.Add(new SQLiteParameter("@ROLE_NAME", TdRoleTypes.Owner));
                command.Parameters.Add(new SQLiteParameter("@SALT", "xAdIp1YaeYT3tz+psZPtrxpV/asxZNycK7nXIdXaxzR1dZsl8OHT4IfWzy8wx0c7"));
                command.Parameters.Add(new SQLiteParameter("@ROLE_ID", 0));
                command.Parameters.Add(new SQLiteParameter("@GROUP_NAME", "<Groep>"));
                command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));
                command.Parameters.Add(new SQLiteParameter("@USER_VALIDATE", "7Dpe4OLgN3HI4bI6iOofN1UQt8vDTWDOGTuMcmi0dX/YZLTbd6VsPpzmWUNBLCYF"));
                command.Parameters.Add(new SQLiteParameter("@AUTHENTICATION", false));
                command.Parameters.Add(new SQLiteParameter("@USER_AUTHENTICATION", string.Empty));
                try
                {
                    command.ExecuteNonQuery();
                    TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.TableIsUpdated, TdTableName.USER_LIST, version));
                }
                catch (SQLiteException ex)
                {
                    this.Error = true;
                    TdLogging.WriteToLogError(string.Format(TdLogging_Resources.ErrorEnteringUserInTable, TdTableName.USER_LIST, version));
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
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogInformation("The Default user Exists.");
                }
            }
        }

        private bool IsOwnerUnique()
        {
            try
            {
                string selectSql = "select distinct USERNAME from " + TdTableName.USER_LIST + " where USERNAME = @USERNAME";

                this.DbConnection.Open();

                SQLiteCommand command = new (selectSql, this.DbConnection);
                command.Prepare();
                command.Parameters.Add(new SQLiteParameter("@USERNAME", TdRoleTypes.Owner));

                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    dr.Close();
                    command.Dispose();
                    return false;
                }
                else
                {
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.DefaultUserNotFound, TdRoleTypes.Owner));
                    }

                    command.Dispose();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(string.Format(TdLogging_Resources.FailedToSeeIfUserIsUnique, TdRoleTypes.Owner));
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
                this.DbConnection.Close();
            }
        }

        #region Dispose

        // Flag: Has Dispose already been called?
        private bool disposed;

        // Instantiate a SafeHandle instance.
        private SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);

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
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.safeHandle?.Dispose();

                // Free any other managed objects here.
            }

            this.disposed = true;
        }
        #endregion Dispose
    }
}