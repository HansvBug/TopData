namespace TopData
{
    using System;
    using System.Data.SQLite;
    using System.Globalization;

    /// <summary>
    /// Application databse clas with some basic functions. It gets inhereted.
    /// </summary>
    public class TdAppDb : TdSQliteDatabaseConnection
    {
        private readonly int latestDbVersion;

        /// <summary>
        /// Gets latestDbVersion.
        /// </summary>
        protected int LatestDbVersion
        {
            get { return this.latestDbVersion; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an Error has occurred.
        /// </summary>
        protected bool Error { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdAppDb"/> class.
        /// </summary>
        public TdAppDb()
        {
            this.latestDbVersion = TdSettingsDefault.UpdateVersion;
            this.Error = false;
        }

        /// <summary>
        /// Select the version of the application databse.
        /// </summary>
        /// <returns>The current version.</returns>
        public int SelectMeta() // Made public so you can check the version on every application start
        {
            int sqlLiteMetaVersion = 0;

            string selectSql = "SELECT VALUE FROM " + TdTableName.SETTINGS_META + " WHERE KEY = 'VERSION'";

            this.DbConnection.Open();
            TdLogging.WriteToLogInformation("Controle op versie van de query database.");

            SQLiteCommand command = new(selectSql, this.DbConnection);
            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    sqlLiteMetaVersion = int.Parse(dr[0].ToString(), CultureInfo.InvariantCulture);
                }

                dr.Close();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Opvragen meta versie is misukt. (Versie " + Convert.ToString(this.latestDbVersion, CultureInfo.InvariantCulture) + ").");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                this.Error = true;
                sqlLiteMetaVersion = 9999;
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }

            return sqlLiteMetaVersion;
        }

        /// <summary>
        /// Insert the new application database version.
        /// </summary>
        /// <param name="version">The new version.</param>
        protected void InsertMeta(string version)
        {
            if (!this.Error)
            {
                this.DbConnection.Open();
                string insertSQL = string.Format("INSERT INTO {0} VALUES('VERSION', @VERSION)", TdTableName.SETTINGS_META);

                SQLiteCommand command = new(insertSQL, this.DbConnection);
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@VERSION", version));

                    command.ExecuteNonQuery();
                    TdLogging.WriteToLogInformation(string.Format("De tabel {0} is gewijzigd. (Versie ", TdTableName.SETTINGS_META) + version + ").");
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError(string.Format("Het invoeren van het database versienummer in de tabel {0} is misukt. (Versie ", TdTableName.SETTINGS_META) + Convert.ToString(this.latestDbVersion, CultureInfo.InvariantCulture) + ").");
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
                TdLogging.WriteToLogError(string.Format("Het invoeren van het database versienummer in de tabel {0} is mislukt.", TdTableName.SETTINGS_META));
            }
        }

        /// <summary>
        /// Change the applicateion database version.
        /// </summary>
        /// <param name="version">The new version.</param>
        protected void UpdateMeta(string version)
        {
            this.DbConnection.Open();

            using var tr = this.DbConnection.BeginTransaction();
            string insertSQL = "UPDATE " + TdTableName.SETTINGS_META + " SET VALUE  = @VERSION WHERE KEY = @KEY";

            SQLiteCommand command = new(insertSQL, this.DbConnection);
            try
            {
                command.Parameters.Add(new SQLiteParameter("@VERSION", version));
                command.Parameters.Add(new SQLiteParameter("@KEY", "VERSION"));

                command.ExecuteNonQuery();
                TdLogging.WriteToLogInformation("De tabel " + TdTableName.SETTINGS_META + " is gewijzigd. (Versie " + version + ").");
                command.Dispose();
                tr.Commit();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Wijzigen tabel " + TdTableName.SETTINGS_META + " versie is misukt. (Versie " + version + ").");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                command.Dispose();
                tr.Rollback();
            }
            finally
            {
                this.DbConnection.Close();
            }
        }

        /// <summary>
        /// Update the user_version from the application database SQlite file (pragma user_version).
        /// </summary>
        /// <param name="version">The new version.</param>
        protected void SetDatabaseUserVersion(string version)
        {
            string updateSql = string.Format("PRAGMA user_version = {0} ", version);

            this.DbConnection.Open();

            SQLiteCommand command = new(updateSql, this.DbConnection);
            try
            {
                command.ExecuteNonQuery();
                TdLogging.WriteToLogInformation("De SQlite user_version is gewijzigd. (Versie " + version + ").");
                command.Dispose();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Het wijzigen van de SQlite user_version is misukt. (Versie " + version + ").");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                command.Dispose();
            }
            finally
            {
                this.DbConnection.Close();
            }
        }
    }
}
