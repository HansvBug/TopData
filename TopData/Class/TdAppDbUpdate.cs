namespace TopData
{
    using System;
    using System.Data.SQLite;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Update the appliction database.
    /// </summary>
    public class TdAppDbUpdate : TdAppDb, IDisposable
    {
        /// <summary>
        /// Update the application database tables.
        /// </summary>
        /// <returns>True if update succeeded.</returns>
        public bool Updatetables()
        {
            bool succes = false;
            string version;
            if (this.LatestDbVersion > this.SelectMeta())
            {
                if (this.SelectMeta() < 2)
                {
                    // version = "2";
                    // this.SetDatabaseUserVersion(version);
                    // this.UpdateMeta(version);  // Set the version 2
                }

                if (this.SelectMeta() < 4)
                {
                    version = "4";

                    this.AlterTableQueryList(version);
                    this.UpdateTableQueryList(version);
                    this.AlterUserTable(version);

                    // alter tables..
                    this.SetDatabaseUserVersion(version);
                    this.UpdateMeta(version);  // Set the version 4
                }
            }

            if (!this.Error)
            {
                succes = true;
            }

            return succes;
        }

        private void AlterTableQueryList(string version)
        {
            if (!this.Error)
            {
                this.DbConnection.Open();

                SQLiteCommand command = new("ALTER TABLE " + TdTableName.QUERY_LIST + " add QUERY_AUTORISATION VARCHAR2(50)", this.DbConnection);
                try
                {
                    command.ExecuteNonQuery();
                    TdLogging.WriteToLogInformation("De tabel " + TdTableName.QUERY_LIST + " is gewijzigd. (Versie " + version + ").");
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het wijzigen van de tabel " + TdTableName.QUERY_LIST + " is misukt. (Versie " + Convert.ToString(this.LatestDbVersion, CultureInfo.InvariantCulture) + ").");
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
                TdLogging.WriteToLogError(string.Format("Het wijzigen van de tabel {0} is niet uitgevoerd.", TdTableName.QUERY_LIST));
            }
        }

        private void UpdateTableQueryList(string version)
        {
            if (!this.Error)
            {
                this.DbConnection.Open();

                using var tr = this.DbConnection.BeginTransaction();
                try
                {
                    SQLiteCommand command = new("UPDATE " + TdTableName.QUERY_LIST + " SET QUERY_AUTORISATION = QUERY_GROUP", this.DbConnection);

                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het bijwerken van de tabel " + TdTableName.QUERY_LIST + " is mislukt. (Versie: " + version + ").");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    this.Error = true;
                }

                try
                {
                    SQLiteCommand command = new("UPDATE " + TdTableName.QUERY_LIST + " SET QUERY_GROUP = NULL", this.DbConnection);

                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het bijwerken van de tabel " + TdTableName.QUERY_LIST + " is mislukt.");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    this.Error = true;
                }

                if (this.Error)
                {
                    tr.Rollback();
                    this.DbConnection.Close();
                }
                else
                {
                    tr.Commit();
                    this.DbConnection.Close();
                }
            }
            else
            {
                TdLogging.WriteToLogError(string.Format("Het wijzigen van de tabel {0} is niet uitgevoerd.", TdTableName.QUERY_LIST));
            }
        }

        private void AlterUserTable(string version)
        {
            if (!this.Error)
            {
                this.DbConnection.Open();

                SQLiteCommand command = new("ALTER TABLE " + TdTableName.USER_LIST + " add USERNAME_FULL VARCHAR2(50)", this.DbConnection);
                try
                {
                    command.ExecuteNonQuery();
                    TdLogging.WriteToLogInformation("De tabel " + TdTableName.QUERY_LIST + " is gewijzigd. (Versie " + version + ").");
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het wijzigen van de tabel " + TdTableName.QUERY_LIST + " is misukt. (Versie " + Convert.ToString(this.LatestDbVersion, CultureInfo.InvariantCulture) + ").");
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
                TdLogging.WriteToLogError(string.Format("Het wijzigen van de tabel {0} is niet uitgevoerd.", TdTableName.QUERY_LIST));
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
        #endregion IDisposable
    }
}
