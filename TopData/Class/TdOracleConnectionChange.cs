namespace TopData
{
    using System;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Functions used for changing the Oracle connection parameters.
    /// </summary>
    public class TdOracleConnectionChange : IDisposable
    {
        #region fields
        private ResourceManager rmMb;     // Declare Resource manager to access to specific cultureinfo
        private ResourceManager rmLog;    // Declare Resource manager to access to specific cultureinfo
        private CultureInfo cul;          // Declare culture info
        #endregion fields

        #region properties

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        private string ConnectionName { get; set; }

        /// <summary>
        /// Gets or sets the schema name.
        /// </summary>
        private string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        private SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        private string Database { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }
        #endregion Properties

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdOracleConnectionChange"/> class.
        /// </summary>
        /// <param name="connectionName">The oracle connection name.</param>
        /// <param name="schemaName">The schema name.</param>
        /// <param name="password">The password.</param>
        /// <param name="database">The datbase name.</param>
        public TdOracleConnectionChange(string connectionName, string schemaName, SecureString password, string database)
        {
            this.ConnectionName = connectionName;
            this.SchemaName = schemaName;
            this.Password = password;
            this.Database = database;
            this.GetSettings();
            this.SetCulture();
        }
        #endregion constructor

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
                // Logging is not available here
                MessageBox.Show("Fout bij het laden van de instellingen. " + Environment.NewLine + "De default instellingen worden ingeladen.", MB_Title.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetCulture()
        {
            if (this.JsonObjSettings.AppParam[0].Language == "en-US")
            {
                this.cul = CultureInfo.CreateSpecificCulture("en-US");
            }
            else if (this.JsonObjSettings.AppParam[0].Language == "nl-NL")
            {
                this.cul = CultureInfo.CreateSpecificCulture("nl-NL");
            }

            this.rmMb = new ResourceManager("TopData.Resource.Res_Messagebox", typeof(FormMain).Assembly);
            this.rmLog = new ResourceManager("TopData.Resource.Res_Log", typeof(FormMain).Assembly);
        }

        /// <summary>
        /// Save the new Oracle connection data.
        /// </summary>
        /// <returns>true if success.</returns>
        public bool SaveConnecctionToOracle()
        {
            using TdSecurityExtensions securityExt = new();

            if (!string.IsNullOrWhiteSpace(this.ConnectionName) &&
               !string.IsNullOrWhiteSpace(this.SchemaName) &&
               !string.IsNullOrWhiteSpace(securityExt.UnSecureString(this.Password)) &&
               !string.IsNullOrWhiteSpace(this.Database))
            {
                string connectionParams = this.ConnectionName + this.SchemaName + this.Database;

                // First check if the connection parameters are unique
                if (!CheckConnectionNamesForDoubleNames(connectionParams))
                {
                    TdOracleConnectionMaintain oraConn = new (this.SchemaName, this.Password, this.Database, this.ConnectionName);
                    oraConn.SaveOracleConnection();
                    return true;
                }
                else
                {
                    MessageBox.Show(MB_Text.Duplicate_Databae_Conn, MB_Title.Cre_New_DB_Connection, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            else
            {
                MessageBox.Show(MB_Text.Not_All_Fields_Filled, MB_Text.Cre_new_Ora_Conn, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        private static bool CheckConnectionNamesForDoubleNames(string connectionPrarams) // Check to see if there are duplicate connection names
        {
            TdOracleConnectionMaintain oraConn = new ();

            TdOracleConnections conn = oraConn.GetOracleConnectionNames();

            string concatenateData;

            foreach (TdOracleConnection con in conn.Items)
            {
                concatenateData = con.Name + con.Schema + con.Connection;
                if (concatenateData == connectionPrarams)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Close the current oracle connection and connect to an other Oracle connection.
        /// </summary>
        /// <returns>True if success.</returns>
        public bool ConnectToOracleAlterConnection()
        {
            using TdSecurityExtensions securityExt = new();

            if (!string.IsNullOrWhiteSpace(this.ConnectionName) &&
               !string.IsNullOrWhiteSpace(this.SchemaName) &&
               !string.IsNullOrWhiteSpace(securityExt.UnSecureString(this.Password)) &&
               !string.IsNullOrWhiteSpace(this.Database))
            {
                TdOraConnection conn = new (this.ConnectionName, this.Password, this.Database, this.ConnectionName);
                try
                {
                    conn.OraCloseConnection();  // If exist then close the existing Oracle connection

                    if (conn.OraConnect(this.SchemaName, this.Password, this.Database))
                    {
                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug("OraConnection.OraIngelogd : " + Convert.ToString(conn.OraLoggedIn, CultureInfo.InvariantCulture));
                        }

                        conn.OraCloseConnection();
                        Cursor.Current = Cursors.Default;
                        return true;
                    }
                    else
                    {
                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug(" Username   : " + this.SchemaName);
                            TdLogging.WriteToLogDebug(" Password   : " + this.Password);
                            TdLogging.WriteToLogDebug(" Datasource : " + this.Database);
                        }

                        Cursor.Current = Cursors.Default;

                        MessageBox.Show(
                            "Inloggen op het schema '" + this.SchemaName + "@" + this.Database + "' is mislukt.",
                            this.rmMb.GetString("Title_Information", this.cul),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    TdLogging.WriteToLogError("Onverwachte fout opgetreden bij het controleren van de connectie gegevens.");
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
                MessageBox.Show(MB_Text.Not_All_Fields_Filled, MB_Title.Cre_New_DB_Connection, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// Change the Oracle connection data.
        /// </summary>
        /// <param name="connectionId">The Id of the connection.</param>
        /// <returns>True id data is changed.</returns>
        public bool AlterConnectionToOracle(int connectionId)
        {
            using TdSecurityExtensions securityExt = new();

            if (!string.IsNullOrWhiteSpace(this.ConnectionName) &&
               !string.IsNullOrWhiteSpace(this.SchemaName) &&
               !string.IsNullOrWhiteSpace(securityExt.UnSecureString(this.Password)) &&
               !string.IsNullOrWhiteSpace(this.Database))
            {
                TdOracleConnectionMaintain oraConn = new(this.SchemaName, this.Password, this.Database, this.ConnectionName);

                if (oraConn.AlterConnection(connectionId))
                {
                    TdLogging.WriteToLogInformation("De connectie '" + this.ConnectionName + "' is succesvol gewijzigd.");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show(MB_Text.Not_All_Fields_Filled, MB_Title.Attention, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// Delete the Oracle connection. First warn if there are queries connected to the connection.
        /// </summary>
        /// <param name="connectionId">The id of the selected connection.</param>
        /// <param name="connectionName">The name of the selected connection.</param>
        /// <returns>True if success.</returns>
        public static bool DeleteConnecctionToOracle(int connectionId, string connectionName)
        {
            bool connectionIsDeleted = false;
            TdOracleConnectionMaintain oraConn = new ();

            // First check if de connection is used by queries. If not then delete the oracle connection without asking
            if (oraConn.CheckConnectionQuery(connectionId))
            {
                DialogResult dialogResult = MessageBox.Show(
                    "De connectie heeft relaties naar Query's. " + Environment.NewLine +
                    "Doorgaan met verwijderen?", MB_Title.Continue,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    if (oraConn.DeleteConnection(connectionId))
                    {
                        TdLogging.WriteToLogInformation("De connectie '" + connectionName + "' is succesvol verwijderd.");

                        // Update the table CONNECTION_QUERY, set the connection_id to null for the deleted connection
                        if (oraConn.DeleteConnnectionID(connectionId))
                        {
                            TdLogging.WriteToLogInformation("De relatie connectie_id - query_id is succesvol verwijderd.");
                            connectionIsDeleted = true;
                        }
                    }
                }
            }
            else
            {
                if (oraConn.DeleteConnection(connectionId))
                {
                    TdLogging.WriteToLogInformation("De connectie '" + connectionName + "' is succesvol verwijderd.");

                    // Update the table CONNECTION_QUERY, set the connection_id to null for the deleted connection
                    if (oraConn.DeleteConnnectionID(connectionId))
                    {
                        TdLogging.WriteToLogInformation("De relatie connectie_id - query_id is succesvol verwijderd.");
                        connectionIsDeleted = true;
                    }
                }
            }

            return connectionIsDeleted;
        }

        #region Dispose
        private bool disposed = false;

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
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.safeHandle?.Dispose();

                    // Free other state (managed objects).
                }

                // Free your own state (unmanaged objects).
                // Set large fields to null.
                this.disposed = true;
            }
        }
        #endregion Dispose
    }
}
