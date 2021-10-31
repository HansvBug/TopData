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
    /// Maintain Oracle connections.
    /// </summary>
    public class TdOracleConnectionMaintain : TdSQliteDatabaseConnection, IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the schema name.
        /// </summary>
        private string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        private SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the database name / datasource.
        /// </summary>
        private string Database { get; set; }

        /// <summary>
        /// Gets or sets the connectionname.
        /// </summary>
        private string ConnectionName { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdOracleConnectionMaintain"/> class.
        /// </summary>
        /// <param name="schemaName">Schema name.</param>
        /// <param name="password">Schema password.</param>
        /// <param name="database">Schema database.</param>
        /// <param name="connectionName">Connection name.</param>
        public TdOracleConnectionMaintain(string schemaName, SecureString password, string database, string connectionName)
        {
            this.SchemaName = schemaName;
            this.Password = password;
            this.Database = database;
            this.ConnectionName = connectionName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdOracleConnectionMaintain"/> class.
        /// </summary>
        public TdOracleConnectionMaintain()
        {
            // Default
        }

        #endregion Constructor

        /// <summary>
        /// Test te new Orcale connection before saving it.
        /// </summary>
        /// <returns>True if connection is made. </returns>
        public bool TestNewConnection()
        {
            using TdSecurityExtensions securityExt = new();

            if (!string.IsNullOrWhiteSpace(this.ConnectionName) &&
               !string.IsNullOrWhiteSpace(this.SchemaName) &&
               !string.IsNullOrWhiteSpace(securityExt.UnSecureString(this.Password)) &&
               !string.IsNullOrWhiteSpace(this.Database))
            {
                TdOraConnection oraConn = new (this.SchemaName, this.Password, this.Database, this.ConnectionName);
                try
                {
                    oraConn.OraCloseConnection();  // If exist then close the existing Oracle connection

                    if (oraConn.OraConnect(this.SchemaName, this.Password, this.Database))
                    {
                        oraConn.OraCloseConnection(); // Close the connection

                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug("Test nieuwe Oracle connectie gegevens is geslaagd.");
                        }

                        return true;
                    }
                    else
                    {
                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug(" Username   : " + this.SchemaName);
                            TdLogging.WriteToLogDebug(" Password   : " + this.Password);
                            TdLogging.WriteToLogDebug(" Datasource : " + this.Database);
                            TdLogging.WriteToLogDebug(" Connectie naam : " + this.ConnectionName);
                        }

                        Cursor.Current = Cursors.Default;

                        MessageBox.Show(
                            "Inloggen op het schema '" + this.SchemaName + "@" + this.Database + "' is mislukt.",
                            MB_Title.Information,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    TdLogging.WriteToLogError("Onverwachte fout opgetreden bij inloggen op het gekozen schema.");
                    TdLogging.WriteToLogError("Melding: ");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    return false;
                }
            }
            else
            {
                MessageBox.Show(MB_Text.Not_All_Fields_Filled, MB_Text.Cre_new_Ora_Conn, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        #region save connection

        /// <summary>
        /// Save the Orcle connection parameters.
        /// </summary>
        /// <param name="name">Connction name.</param>
        /// <param name="schema">Schema name.</param>
        /// <param name="pass">Password.</param>
        /// <param name="connection">Datasource.</param>
        /// <returns>True when data is saved.</returns>
        public bool SaveOracleConnection()
        {
            bool result;
            this.DbConnection.Open();
            using TdSecurityExtensions securityExt = new();

            SQLiteCommand command = new("insert into CONN_ORACLE " +
                                                      "(CONNECTNAME, SCHEMANAME, SCHEMAPASS, CONNECT, DATE_CREATED, CREATED_BY)" +
                                                      "values " +
                                                      "( @CONNECTNAME, @SCHEMANAME, @SCHEMAPASS, @CONNECT, @DATE_CREATED, @CREATED_BY)",
                                                      this.DbConnection);

            command.Parameters.Add(new SQLiteParameter("@CONNECTNAME", this.ConnectionName));
            command.Parameters.Add(new SQLiteParameter("@SCHEMANAME", this.SchemaName));
            command.Parameters.Add(new SQLiteParameter("@SCHEMAPASS", securityExt.UnSecureString(this.Password)));
            command.Parameters.Add(new SQLiteParameter("@CONNECT", this.Database));
            command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));
            command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));
            try
            {
                command.Prepare();
                command.ExecuteNonQuery();
                result = true;
                TdLogging.WriteToLogInformation("De Oracle connectie gegevens zijn opgeslagen.");
                TdLogging.WriteToLogInformation("Connectie naam: " + this.ConnectionName);
                TdLogging.WriteToLogInformation("Connectie schema: " + this.SchemaName);
                TdLogging.WriteToLogInformation("Connectie database: " + this.Database);

                MessageBox.Show(MB_Text.Ready_Saving_Connection, MB_Title.Save, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Opslaan van de Oracle connectie is misukt.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                MessageBox.Show(
                    "Er is een onverwachte fout opgetreden bij het opslaan van de connectie gegevens." + Environment.NewLine +
                    Environment.NewLine +
                    "Controleer het log bestand.", MB_Title.Save,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                result = false;
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }

            return result;
        }
        #endregion save connection

        /// <summary>
        /// Get all Oracle connection names.
        /// </summary>
        /// <returns>All connections.</returns>
        public TdOracleConnections GetOracleConnectionNames()
        {
            TdOracleConnections connections = new ();
            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                try
                {
                    this.DbConnection.Open();
                    string selectSQL = string.Format("SELECT ID, CONNECTNAME, SCHEMANAME, SCHEMAPASS, CONNECT, CREATED_BY, DATE_CREATED, DATE_ALTERED, ALTERED_BY FROM {0} ORDER BY CONNECTNAME", TdTableName.CONN_ORACLE);

                    using (SQLiteCommand command = new(selectSQL, this.DbConnection))
                    {
                        SQLiteDataReader reader = command.ExecuteReader();

                        DataTable dt = new ();
                        dt.Load(reader);

                        using TdSecurityExtensions securityExt = new();

                        foreach (DataRow row in dt.Rows)
                        {
                            if (int.TryParse(row["ID"].ToString(), out int id))
                            {
                                TdOracleConnection item = new(id)
                                {
                                    Name = row["CONNECTNAME"].ToString(),
                                    Schema = row["SCHEMANAME"].ToString(),
                                    Password = securityExt.ConvertToSecureString(row["SCHEMAPASS"].ToString()),
                                    Connection = row["CONNECT"].ToString(),  // TODO change name to Datasource
                                    CreatedBy = row["CREATED_BY"].ToString(),
                                    Alteredby = row["ALTERED_BY"].ToString(),
                                };

                                if (DateTime.TryParse(row["DATE_CREATED"].ToString(), out DateTime dtCreated))
                                {
                                    item.Created = dtCreated;
                                }

                                if (DateTime.TryParse(row["DATE_ALTERED"].ToString(), out DateTime dtAltered))
                                {
                                    item.Altered = dtAltered;
                                }

                                connections.Items.Add(item);
                            }
                        }

                        reader.Close();
                        dt.Dispose();
                    }

                    TdLogging.WriteToLogInformation("De Oracle database connectie gegevens zijn opgehaald.");
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Onverwachte fout opgetreden bij het ophalen van de Oracle connectie gegevens.");
                    TdLogging.WriteToLogError("Melding: ");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
                catch (Exception e)
                {
                    TdLogging.WriteToLogError("Onverwachte fout opgetreden bij het ophalen van de Oracle connectie gegevens.");
                    TdLogging.WriteToLogError("Melding: ");
                    TdLogging.WriteToLogError(e.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(e.ToString());
                    }
                }
                finally
                {
                    this.DbConnection.Close();
                }
            }

            return connections;
        }

        /// <summary>
        /// Alter the connection data.
        /// </summary>
        /// <param name="connId">The connection id. Used for identifying the connection.</param>
        /// <returns>True when record is saved.</returns>
        public bool AlterConnection(int connId)
        {
            this.DbConnection.Open();
            using TdSecurityExtensions securityExt = new();

            SQLiteCommand command = new(string.Format("UPDATE {0} SET ", TdTableName.CONN_ORACLE) +
                                                      "CONNECTNAME = @CONNECTNAME, SCHEMANAME = @SCHEMANAME, SCHEMAPASS = @SCHEMAPASS, " +
                                                      "CONNECT = @CONNECT, DATE_ALTERED = @DATE_ALTERED, ALTERED_BY= @ALTERED_BY" +
                                                      " WHERE ID = @CONNECTION_ID",
                                                      this.DbConnection);

            command.Parameters.Add(new SQLiteParameter("@CONNECTNAME", this.ConnectionName));
            command.Parameters.Add(new SQLiteParameter("@SCHEMANAME", this.SchemaName));
            command.Parameters.Add(new SQLiteParameter("@SCHEMAPASS", TdEncryptDecrypt.Encrypt(securityExt.UnSecureString(this.Password), TdSettingsDefault.StringSleutel)));
            command.Parameters.Add(new SQLiteParameter("@CONNECT", this.Database));
            command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", connId));
            command.Parameters.Add(new SQLiteParameter("@DATE_ALTERED", DateTime.Now));
            command.Parameters.Add(new SQLiteParameter("@ALTERED_BY", Environment.UserName));
            command.Prepare();
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(
                    "De connectiegegevens zijn niet gewijzigd." + Environment.NewLine +
                    Environment.NewLine +
                    "Kijk in het log bestand voor informatie.",
                    MB_Title.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                TdLogging.WriteToLogError("Het wijzigen van de connectiegegevens is misukt. (Connectie ID = " + connId);
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
        /// Check if the connection_id is connected to query's.
        /// </summary>
        /// <param name="connId">Connectuin id.</param>
        /// <returns>True if connection-query combi is found.</returns>
        public bool CheckConnectionQuery(int connId)
        {
            this.DbConnection.Open();
            SQLiteCommand command = new(string.Format("SELECT COUNT(*) from {0} WHERE CONNECTION_ID = @CONNECTION_ID", TdTableName.CONNECTION_QUERY), this.DbConnection);

            command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", connId));
            command.Prepare();

            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    int aantal = dr.GetInt32(0);
                    if (aantal > 0)
                    {
                        dr.Close();
                        return true;
                    }
                    else
                    {
                        dr.Close();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(
                    "De controle Connection_id - Query_id is mislukt." + Environment.NewLine +
                    Environment.NewLine +
                    "Kijk in het log bestand voor informatie.",
                    MB_Title.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                TdLogging.WriteToLogError("De controle Connection_id - Query_id is mislukt.");
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

        #region delete connection

        /// <summary>
        /// Delete connection.
        /// </summary>
        /// <param name="connId">The connection id which will be deleted.</param>
        /// <returns>True if deleted succeeded.</returns>
        public bool DeleteConnection(int connId)
        {
            this.DbConnection.Open();
            SQLiteCommand command = new(string.Format("DELETE FROM {0} WHERE ID = @CONNECTION_ID", TdTableName.CONN_ORACLE), this.DbConnection);

            command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", connId));
            command.Prepare();
            using TdAppDbMaintain appDbMaintain = new();

            try
            {
                command.ExecuteNonQuery();
                this.DeleteConnectionSettings(connId);

                appDbMaintain.ResetAutoIncrementFields(TdTableName.CONN_ORACLE);

                TdLogging.WriteToLogInformation("De connectie met id '" + connId + "' is verwijderd.");
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(
                    "De connectie is niet verwijderd." + Environment.NewLine +
                    Environment.NewLine +
                    "Kijk in het log bestand voor informatie.",
                    MB_Title.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                TdLogging.WriteToLogError("Verwijderen van de connectie is misukt. (Connectie ID = " + connId);
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

        private void DeleteConnectionSettings(int connId)
        {
            using SQLiteConnection dbConnection = new("Data Source=" + this.DatabaseFileName);
            dbConnection.Open();
            SQLiteCommand command = new(string.Format("DELETE FROM {0} WHERE CONNECTION_ID = @CONNECTION_ID", TdTableName.SETTINGS_APP), dbConnection);

            command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", connId));
            command.Prepare();

            try
            {
                command.ExecuteNonQuery();
                TdLogging.WriteToLogInformation("De settings van de connectie zijn verwijderd.");
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Verwijderen van de connectie setting gegevens zijn misukt. (Connectie ID = " + connId);
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
                dbConnection.Close();
            }
        }

        /// <summary>
        /// Delete the oracle connection from table CONNECTION_QUERY.
        /// </summary>
        /// <param name="connId">The id of the connection.</param>
        /// <returns>True id record is deleted.</returns>
        public bool DeleteConnnectionID(int connId)
        {
            this.DbConnection.Open();
            SQLiteCommand command = new(string.Format("DELETE FROM {0} WHERE CONNECTION_ID = @CONNECTION_ID", TdTableName.CONNECTION_QUERY), this.DbConnection);
            command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", connId));
            command.Prepare();

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(
                    "Het connectie ID is niet verwijderd uit de tabel CONNECTION_QUERY." + Environment.NewLine +
                    Environment.NewLine +
                    "Kijk in het log bestand voor informatie.", MB_Title.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                TdLogging.WriteToLogError("Het verwijderen van de records met het connectie id " + connId + "in de tabel CONNECTION_QUERY is misukt.");
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

        #endregion delete connection

        #region Dispose
        private bool disposed;  // Flag: Has Dispose already been called?

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
