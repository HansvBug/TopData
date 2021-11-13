namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Load and save the treeview nodes state. (Collapsed or uncollapsed).
    /// </summary>
    public class TdMaintainTreeviewState : TdSQliteDatabaseConnection, IDisposable
    {
        #region Properties etc...

        /// <summary>
        /// Gets or sets Username. The User which is currently logged in the TopData.
        /// </summary>
        private string UserName { get; set; }

        /// <summary>
        /// Gets or sets the id of the current user.
        /// </summary>
        private int UserId { get; set; }

        /// <summary>
        /// Gets or sets the pc log in name.
        /// </summary>
        private string EnvironmentUserName { get; set; }

        /// <summary>
        /// Gets or sets the Orace database connection id.
        /// </summary>
        private int ConnectionId { get; set; }

        private List<string> treeviewNodes = new ();

        /// <summary>
        /// Gets or sets list with treeview nodes.
        /// </summary>
        public List<string> TreeviewNodes
        {
            get { return this.treeviewNodes; }
            set { this.treeviewNodes = value; }
        }

        #endregion Properties etc...

        #region Constructor
        public TdMaintainTreeviewState(string userName, int userId,  int connectionId)
        {
            this.UserName = userName;
            this.EnvironmentUserName = Environment.UserName;
            this.UserId = userId;
            this.ConnectionId = connectionId;
        }

        #endregion Constructor

        /// <summary>
        /// Delete the old treeview state data.
        /// </summary>
        /// <param name="treeviewName">The treeview name.</param>
        public void DeleteTreeViewState(string treeviewName)
        {
            if (string.IsNullOrEmpty(treeviewName))
            {
                return;
            }

            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    using SQLiteCommand command = new (string.Format("delete from {0} ", TdTableName.SETTINGS_APP) +
                                                                     "where  USER_ID = @USER_ID " +
                                                                     "and USER_NAME = @USER_NAME " +
                                                                     "and LOGGED_IN_USER = @LOGGED_IN_USER " +
                                                                     "and CONNECTION_ID = @CONNECTION_ID " +
                                                                     "and ITEM = @ITEM", this.DbConnection);
                    command.Prepare();

                    command.Parameters.Add(new SQLiteParameter("@USER_ID", this.UserId));
                    command.Parameters.Add(new SQLiteParameter("@USER_NAME", this.UserName));
                    command.Parameters.Add(new SQLiteParameter("@LOGGED_IN_USER", this.EnvironmentUserName));

                    command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", this.ConnectionId));
                    command.Parameters.Add(new SQLiteParameter("@ITEM", treeviewName));

                    command.ExecuteNonQuery();
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogInformation("Alle treeview status data is verwijderd voor de gebruker : " + TdUserData.UserName + ". (Treeview: " + treeviewName + ").");
                    }
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het verwijderen van de vorige Treeview Status is mislukt.");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();

                    MessageBox.Show(
                        "Het verwijderen van de vorige Treeview Status is mislukt." + Environment.NewLine +
                        Environment.NewLine +
                        MB_Text.TextCheckLogFile,
                        MB_Title.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                tr.Commit();
            }

            this.DbConnection.Close();
        }

        /// <summary>
        /// Save the new treeview state data.
        /// </summary>
        /// <param name="treeviewName">The selected treeview.</param>
        /// <param name="treeViewNode">The selected treeview node.</param>
        public void SaveTreeViewState(string treeviewName, string treeViewNode)
        {
            if (string.IsNullOrEmpty(treeviewName) || string.IsNullOrEmpty(treeViewNode))
            {
                return;
            }

            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    using SQLiteCommand command = new (string.Format("insert into {0} (GUID, USER_ID, USER_NAME, ITEM, ITEM_DATA, LOGGED_IN_USER, CONNECTION_ID ) ", TdTableName.SETTINGS_APP) +
                                                                     "values (@GUID, @USER_ID, @USER_NAME, @ITEM, @ITEM_DATA, @LOGGED_IN_USER, @CONNECTION_ID )",
                                                                     this.DbConnection);
                    command.Prepare();

                    command.Parameters.Add(new SQLiteParameter("@GUID", System.Guid.NewGuid().ToString()));
                    command.Parameters.Add(new SQLiteParameter("@USER_ID", this.UserId));
                    command.Parameters.Add(new SQLiteParameter("@USER_NAME", this.UserName));
                    command.Parameters.Add(new SQLiteParameter("@LOGGED_IN_USER", this.EnvironmentUserName));
                    command.Parameters.Add(new SQLiteParameter("@ITEM", treeviewName));
                    command.Parameters.Add(new SQLiteParameter("@ITEM_DATA", treeViewNode));

                    command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", this.ConnectionId));

                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Fout bij  het opslaan van de Treeview Status. (Betreft treeview : " + treeviewName + ")");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();
                    MessageBox.Show(
                        "Het inlezen van de Treeview Status is mislukt." + Environment.NewLine +
                        Environment.NewLine + MB_Text.TextCheckLogFile,
                        MB_Title.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                tr.Commit();
            }

            this.DbConnection.Close();
        }

        /// <summary>
        /// Get the treeview state from the settings table and put it i a list.
        /// </summary>
        /// <param name="trv">The selected treeview.</param>
        public void ReadTreeViewState(TreeView trv)
        {
            if (trv == null)
            {
                return;
            }

            try
            {
                this.DbConnection.Open();

                using (SQLiteCommand command = new (string.Format("select ITEM_DATA from {0} where USER_ID = @USER_ID and USER_NAME = @USER_NAME ", TdTableName.SETTINGS_APP) +
                                                            "and CONNECTION_ID = @CONNECTION_ID and ITEM = @ITEM", this.DbConnection))
                {
                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@USER_ID", this.UserId));
                    command.Parameters.Add(new SQLiteParameter("@USER_NAME", this.UserName));
                    command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", this.ConnectionId));
                    command.Parameters.Add(new SQLiteParameter("@ITEM", trv.Name));

                    using SQLiteDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        if (!dr.IsDBNull(0))
                        {
                            this.TreeviewNodes.Add(dr.GetValue(0).ToString());
                        }
                        else
                        {
                            TdLogging.WriteToLogError("Ophalen Treeviewnodes mislukt.");
                        }
                    }
                }

                this.DbConnection.Close();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Het ophalen van de treeview nodes is mislukt.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }
            }
        }

        #region Dispose
        private bool disposed;

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
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                this.UserName = string.Empty;
                this.UserId = -1;
                this.EnvironmentUserName = string.Empty;
                this.DatabaseFileName = string.Empty;
                this.ConnectionId = -1;

                this.disposed = true;

                if (disposing)
                {
                    this.safeHandle?.Dispose();

                    // Free other state (managed objects).
                }
            }
        }
        #endregion Dispose
    }
}
