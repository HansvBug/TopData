namespace TopData
{
    using System;
    using System.Data;
    using System.Security;
    using System.Windows.Forms;
    using Oracle.ManagedDataAccess.Client;

    // using Oracle.DataAccess.Client;

    /// <summary>
    ///  Make an Oracle connection.
    /// </summary>
    public class TdOraConnection
    {
        #region Fields

        /// <summary>
        /// Hold the Oracle connection.
        /// </summary>
        private OracleConnection oraConn = new ();
        #endregion Fields

        #region properties

        /// <summary>
        /// Gets or sets the Oracle connection.
        /// </summary>
        public OracleConnection OraConn
        {
            get { return this.oraConn; }
            set { this.oraConn = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether there is logged in to Oracle.
        /// </summary>
        public bool OraLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets the schema name.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string ConnectionName { get; set; }

        #endregion properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdOraConnection"/> class.
        /// </summary>
        public TdOraConnection()
        {
            // default (is used).
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdOraConnection"/> class.
        /// </summary>
        /// <param name="schemaName">The schema name.</param>
        /// <param name="password">Password of the schema.</param>
        /// <param name="database">Database name.</param>
        /// <param name="connectionName">Connection name.</param>
        public TdOraConnection(string schemaName, SecureString password, string database, string connectionName)
        {
            this.SchemaName = schemaName;
            this.Password = password;
            this.Database = database;
            this.ConnectionName = connectionName;
        }
        #endregion Constructor

        /// <summary>
        /// Create an active Orcle connection with a schema.
        /// </summary>
        /// <param name="userName">Schema name.</param>
        /// <param name="password">Schema password.</param>
        /// <param name="datasource">Schema datasource.</param>
        /// <returns>True if connection is valid, false if the connection fails.</returns>
        public bool OraConnect(string userName, SecureString password, string datasource)
        {
            Cursor.Current = Cursors.WaitCursor;
            using TdSecurityExtensions securityExt = new();

            if (string.IsNullOrEmpty(userName) ||
                string.IsNullOrEmpty(securityExt.UnSecureString(password)) ||
                string.IsNullOrEmpty(datasource))
            {
                return false;
            }
            else
            {
                string passwordDecrypt;

                // Assume Password is encrypted. When password is not encrypted read as plain text. (Connection maintenance, the password is not encrypted when the connection is tested)
                try
                {
                    passwordDecrypt = TdEncryptDecrypt.Decrypt(securityExt.UnSecureString(password), TdSettingsDefault.StringSleutel);
                }
                catch
                {
                    passwordDecrypt = securityExt.UnSecureString(password);  // Password is not encrypted
                }

                string constr = "User Id = " + userName + "; Password = " + passwordDecrypt + "; Data Source = " + datasource + ";";

                try
                {
                    if (this.OraConn != null)
                    {
                        if (this.OraConn.State == ConnectionState.Open)
                        {
                            this.OraConn.Close();
                        }

                        this.OraConn.ConnectionString = constr;
                        this.OraConn.Open();

                        TdLogging.WriteToLogInformation("Oracle Connectie gemaakt met: " + userName + "@" + datasource);
                        TdLogging.WriteToLogInformation("Oracle Versie : " + this.OraConn.ServerVersion);  // Displays the Oracle version

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (OracleException ex)
                {
                    TdLogging.WriteToLogError("Connectie met: " + userName + "@" + datasource + "  is mislukt.");
                    TdLogging.WriteToLogError("Melding : ");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    this.SchemaName = null;

                    IsPasswordExpired(ex.Message);

                    Cursor.Current = Cursors.Default;
                    return false;
                }
                catch (Exception ex)
                {
                    // Different client installed, then an exception occurs.
                    TdLogging.WriteToLogError("Fout opgetreden bij het maken van de Oracle connectie.");
                    TdLogging.WriteToLogError("melding:");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(
                        "Aanmaken Oracle connectie is mislukt" + Environment.NewLine +
                        "Controleer het logbestand." + Environment.NewLine + Environment.NewLine +
                        "Is de juiste oracle client beschikbaar?", MB_Title.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Close an open Oracle connection.
        /// </summary>
        public void OraCloseConnection()
        {
            try
            {
                if (this.OraConn != null)
                {
                    this.OraConn.Close();
                    this.OraLoggedIn = false;
                }
            }
            catch (OracleException ex)
            {
                TdLogging.WriteToLogError("Sluiten Oracle connectie is mislukt.");
                TdLogging.WriteToLogError("Melding : ");
                TdLogging.WriteToLogError(ex.Message);

                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }
            }
        }

        private static void IsPasswordExpired(string exMessage)
        {
            if (exMessage == "ORA-28001: the password has expired" ||
                 exMessage == "ORA-28001: Wachtwoord is verlopen")
            {
                MessageBox.Show(MB_Text.Ora_Account_Expired, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(MB_Text.Error_Ora_Conn, MB_Title.Information, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
