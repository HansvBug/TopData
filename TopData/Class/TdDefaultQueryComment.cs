namespace TopData
{
    using System;
    using System.Data.SQLite;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Handle the default Query comment text.
    /// </summary>
    public class TdDefaultQueryComment : TdSQliteDatabaseConnection, IDisposable
    {
        /// <summary>
        /// Gets or sets the User which is currently logged in TopData.
        /// </summary>
        private string UserName { get; set; }

        /// <summary>
        /// Gets or sets the id from the user which is currently logged in TopData.
        /// </summary>
        private int UserId { get; set; }

        /// <summary>
        /// Gets or sets the pc login name.
        /// </summary>
        private string EnvironmentUserName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdDefaultQueryComment"/> class.
        /// Set the current user data.
        /// </summary>
        /// <param name="inlogGebruiker">The current user.</param>
        /// <param name="userId">The current user id.</param>
        public TdDefaultQueryComment(string inlogGebruiker, int userId)
        {
            this.UserName = inlogGebruiker;
            this.UserId = userId;
            this.EnvironmentUserName = Environment.UserName;
        }

        #region Default query text

        /// <summary>
        /// Delete the default query comment text.
        /// </summary>
        public void DeleteDefaultQueryStartText()
        {
            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    using SQLiteCommand command = new(string.Format("delete from {0} ", TdTableName.SETTINGS_APP) +
                                                                     "where  USER_ID = @USER_ID " +
                                                                     "and USER_NAME = @USER_NAME " +
                                                                     "and LOGGED_IN_USER = @LOGGED_IN_USER " +
                                                                     "and ITEM = @ITEM", this.DbConnection);
                    command.Prepare();

                    command.Parameters.Add(new SQLiteParameter("@USER_ID", this.UserId));
                    command.Parameters.Add(new SQLiteParameter("@USER_NAME", this.UserName));
                    command.Parameters.Add(new SQLiteParameter("@LOGGED_IN_USER", this.EnvironmentUserName));
                    command.Parameters.Add(new SQLiteParameter("@ITEM", "QueryDefaultText"));

                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het verwijderen van de default query tekt is mislukt.");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();

                    MessageBox.Show(
                        "Het verwijderen van de default query tekt is mislukt." + Environment.NewLine +
                        Environment.NewLine +
                        "Controleer het logbestand.",
                        MB_Title.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                tr.Commit();
            }

            this.DbConnection.Close();
        }

        /// <summary>
        /// Save the default query comment text.
        /// </summary>
        /// <param name="commentText">The new query comment text.</param>
        public void SaveDefaultQueryStartText(string commentText)
        {
            string insertSql;

            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    insertSql = string.Format("insert into {0} (GUID, USER_ID, USER_NAME, ITEM_DATA, LOGGED_IN_USER, ITEM ) ", TdTableName.SETTINGS_APP);
                    insertSql += "values (@GUID, @USER_ID, @USER_NAME, @ITEM_DATA, @LOGGED_IN_USER, @ITEM )";

                    using SQLiteCommand command = new(insertSql, this.DbConnection);
                    command.Prepare();

                    command.Parameters.Add(new SQLiteParameter("@GUID", System.Guid.NewGuid().ToString()));
                    command.Parameters.Add(new SQLiteParameter("@USER_ID", this.UserId));
                    command.Parameters.Add(new SQLiteParameter("@USER_NAME", this.UserName));
                    command.Parameters.Add(new SQLiteParameter("@ITEM_DATA", commentText));
                    command.Parameters.Add(new SQLiteParameter("@LOGGED_IN_USER", this.EnvironmentUserName));
                    command.Parameters.Add(new SQLiteParameter("@ITEM", "QueryDefaultText"));

                    command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het opslaan van de standaard query commentaar tekst is mislukt.");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();

                    MessageBox.Show(
                        "Het opslaan van de default query tekst is mislukt." + Environment.NewLine +
                        Environment.NewLine +
                        "Controleer het logbestand.",
                        MB_Title.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                tr.Commit();
            }

            this.DbConnection.Close();
        }

        /// <summary>
        /// Load the default Query comment text.
        /// </summary>
        /// <returns>The default query comment text.</returns>
        public string LoadDefaultQueryStartText()
        {
            string commentText = string.Empty;
            try
            {
                this.DbConnection.Open();

                using (SQLiteCommand command = new(string.Format("select ITEM_DATA from {0} ", TdTableName.SETTINGS_APP) +
                                                                "where USER_ID = @USER_ID " +
                                                                "and USER_NAME = @USER_NAME " +
                                                                "and LOGGED_IN_USER = @LOGGED_IN_USER " +
                                                                "and ITEM = @ITEM", this.DbConnection))
                {
                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@USER_ID", this.UserId));
                    command.Parameters.Add(new SQLiteParameter("@USER_NAME", this.UserName));
                    command.Parameters.Add(new SQLiteParameter("@LOGGED_IN_USER", this.EnvironmentUserName));
                    command.Parameters.Add(new SQLiteParameter("@ITEM", "QueryDefaultText"));

                    using SQLiteDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        if (!dr.IsDBNull(0))
                        {
                            commentText = dr.GetString(0);
                        }
                        else
                        {
                            TdLogging.WriteToLogError("Het ophalen van de standaard query tekst is mislukt.");
                        }
                    }
                }

                return commentText;
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Het ophalen van de standaard query tekst is mislukt.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                return commentText;
            }
            finally
            {
                this.DbConnection.Close();
            }
        }
        #endregion Default query text

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
                this.DatabaseFileName = string.Empty;
                this.EnvironmentUserName = string.Empty;

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
