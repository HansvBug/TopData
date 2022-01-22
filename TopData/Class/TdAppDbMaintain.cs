namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Maintain the application database.
    /// </summary>
    public class TdAppDbMaintain : TdSQliteDatabaseConnection, IDisposable
    {
        private List<string> schemaNames = new ();
        private List<string> dataBaseNames = new ();

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdAppDbMaintain"/> class.
        /// </summary>
        public TdAppDbMaintain()
        {
            this.DbConnection = new SQLiteConnection("Data Source=" + this.DatabaseFileName);
        }
        #endregion Constructor

        /// <summary>
        /// Add a new item tot the Schema names list.
        /// </summary>
        /// <param name="newListItem">New list item to add.</param>
        public void AddToSchemaNameList(string newListItem)
        {
            this.schemaNames.Add(newListItem);
        }

        /// <summary>
        /// Add a new item tot the Database names list.
        /// </summary>
        /// <param name="newListItem">New list item to add.</param>
        public void AddToDatabaseaNameList(string newListItem)
        {
            this.dataBaseNames.Add(newListItem);
        }

        #region Maintain ComboboxItems

        /// <summary>
        /// Delete all records from the table ORA_SCHEMA_DATABASE_NAME with a certain schemaname.
        /// </summary>
        /// <param name="name">The schema name which will deleted.</param>
        public void DeleteSchemaName(string name)
        {
            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                using SQLiteConnection dbConnection = new ("Data Source=" + this.DatabaseFileName);
                dbConnection.Open();

                string sqlDelete = string.Format("DELETE FROM {0} where SCHEMA_NAME = @SCHEMANAME", TdTableName.ORA_SCHEMA_DATABASE_NAME);

                SQLiteCommand command = new (sqlDelete, dbConnection);

                command.Parameters.Add(new SQLiteParameter("@SCHEMANAME", name));
                command.Prepare();

                try
                {
                    command.ExecuteNonQuery();
                    TdLogging.WriteToLogInformation(TdLogging_Resources.SchemaNameDeleted);   // Schema name has been successfully deleted.
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(
                        string.Format(TdLogging_Resources.DeleteDbOrSChemaName, TdTableName.ORA_SCHEMA_DATABASE_NAME) + Environment.NewLine +
                        Environment.NewLine + TdLogging_Resources.LookInLogFile,
                        MB_Title.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    TdLogging.WriteToLogError(string.Format(TdLogging_Resources.DeleteDbOrSChemaName, TdTableName.ORA_SCHEMA_DATABASE_NAME));
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
        }

        /// <summary>
        /// Delete all records from the table ORA_SCHEMA_DATABASE_NAME with a certain schemaname.
        /// </summary>
        /// <param name="name">The schema name which will deleted.</param>
        public void DeleteDatabaseName(string name)
        {
            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                using SQLiteConnection dbConnection = new("Data Source=" + this.DatabaseFileName);
                dbConnection.Open();

                string sqlDelete = string.Format("DELETE FROM {0} where DATABASE_NAME = @DATABASENAME", TdTableName.ORA_SCHEMA_DATABASE_NAME);

                SQLiteCommand command = new(sqlDelete, dbConnection);

                command.Parameters.Add(new SQLiteParameter("@DATABASENAME", name));
                command.Prepare();

                try
                {
                    command.ExecuteNonQuery();
                    TdLogging.WriteToLogInformation(TdLogging_Resources.DatabaseNameDeleted);   // Database name has been successfully deleted.
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(
                        string.Format(TdLogging_Resources.ErrorDeletingDbName, TdTableName.ORA_SCHEMA_DATABASE_NAME) + Environment.NewLine +
                        Environment.NewLine + TdLogging_Resources.LookInLogFile,
                        MB_Title.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    TdLogging.WriteToLogError(string.Format(TdLogging_Resources.ErrorDeletingDbName, TdTableName.ORA_SCHEMA_DATABASE_NAME));
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
        }

        /// <summary>
        /// Write the items from a combobox to the table: ORA_SCHEMA_DATABASE_NAME.
        /// Use DeleteSchemaName() before WriteAllComboBoxItems().
        /// </summary>
        /// <param name="comboBoxName">The name of the combobox which items will be saved to the table ORA_SCHEMA_DATABASE_NAME.</param>
        public void WriteAllComboBoxItems(string comboBoxName)
        {
            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                using SQLiteConnection dbConnection = new("Data Source=" + this.DatabaseFileName);
                string insertSQL = string.Empty;

                if (comboBoxName == "SchemaName" || comboBoxName == "AlterSchemaName" || comboBoxName == "DelSchemaName")
                {
                    foreach (var item in this.schemaNames)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            insertSQL = string.Empty;
                            insertSQL += "insert into " + TdTableName.ORA_SCHEMA_DATABASE_NAME + "(GUID, SCHEMA_NAME) ";
                            insertSQL += "select " + @"""" + System.Guid.NewGuid().ToString() + @"""" + "," + @"""" + item + @"""";
                            insertSQL += " where not exists (select SCHEMA_NAME from " + TdTableName.ORA_SCHEMA_DATABASE_NAME + " where SCHEMA_NAME = " + @"""" + item + @"""" + "); ";

                            dbConnection.Open();
                            SQLiteCommand command = new(insertSQL, dbConnection);

                            command.Prepare();

                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (SQLiteException ex)
                            {
                                TdLogging.WriteToLogError(string.Format(TdLogging_Resources.DeleteDbOrSChemaName, TdTableName.ORA_SCHEMA_DATABASE_NAME));
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
                    }
                }

                if (comboBoxName == "DatabaseName" || comboBoxName == "AlterOraDatabase" || comboBoxName == "DelDatabaseName")
                {
                    insertSQL = string.Empty;

                    foreach (var item in this.dataBaseNames)
                    {
                        if(!string.IsNullOrEmpty(item))
                        {
                            insertSQL = string.Empty;
                            insertSQL += "insert into " + TdTableName.ORA_SCHEMA_DATABASE_NAME + "(GUID, DATABASE_NAME) ";
                            insertSQL += "select " + @"""" + System.Guid.NewGuid().ToString() + @"""" + "," + @"""" + item + @"""";
                            insertSQL += " where not exists (select DATABASE_NAME from " + TdTableName.ORA_SCHEMA_DATABASE_NAME + " where DATABASE_NAME = " + @"""" + item + @"""" + "); ";
                        }

                        dbConnection.Open();
                        SQLiteCommand command1 = new(insertSQL, dbConnection);
                        command1.Prepare();

                        try
                        {
                            command1.ExecuteNonQuery();
                        }
                        catch (SQLiteException ex)
                        {
                            TdLogging.WriteToLogError(string.Format(TdLogging_Resources.DeleteDbOrSChemaName, TdTableName.ORA_SCHEMA_DATABASE_NAME));
                            TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                            TdLogging.WriteToLogError(ex.Message);
                            if (TdDebugMode.DebugMode)
                            {
                                TdLogging.WriteToLogDebug(ex.ToString());
                            }
                        }
                        finally
                        {
                            command1.Dispose();
                            dbConnection.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Read the Database names or schema names or connection names from the table ORA_SCHEMA_DATABASE_NAME and return a list of names.
        /// </summary>
        /// <param name="schemaOrDatabaseName">The name which is used in the search query.</param>
        /// <returns>A list with names.</returns>
        public List<string> ReadDataBaseNames(string schemaOrDatabaseName)
        {
            List<string> names = new ();

            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                string selectSql = string.Empty;

                if (schemaOrDatabaseName == "DatabaseName")
                {
                    selectSql = string.Format("select distinct DATABASE_NAME from {0}", TdTableName.ORA_SCHEMA_DATABASE_NAME);
                }

                if (schemaOrDatabaseName == "SchemaName")
                {
                    selectSql = string.Format("select distinct SCHEMA_NAME from {0}", TdTableName.ORA_SCHEMA_DATABASE_NAME);
                }

                if (schemaOrDatabaseName == "ConnectionName")
                {
                    selectSql = string.Format("select distinct CONNECTNAME from {0}", TdTableName.CONN_ORACLE);
                }

                this.DbConnection.Open();

                try
                {
                    using SQLiteCommand command = new (selectSql, this.DbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();

                    DataTable dt = new ();
                    dt.Load(reader);

                    if (schemaOrDatabaseName == "DatabaseName")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            names.Add(row["DATABASE_NAME"].ToString());
                        }
                    }

                    if (schemaOrDatabaseName == "SchemaName")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            names.Add(row["SCHEMA_NAME"].ToString());
                        }
                    }

                    if (schemaOrDatabaseName == "ConnectionName")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            names.Add(row["CONNECTNAME"].ToString());
                        }
                    }

                    List<string> distinctNames = names.Distinct().ToList();
                    reader.Close();
                    dt.Dispose();

                    return distinctNames;
                }
                catch (SQLiteException ex)
                {
                    if (schemaOrDatabaseName == "DatabaseName")
                    {
                        TdLogging.WriteToLogError(string.Format(TdLogging_Resources.QueryGetDbnamesFailed, TdTableName.ORA_SCHEMA_DATABASE_NAME));
                    }
                    else if (schemaOrDatabaseName == "SchemaName")
                    {
                        TdLogging.WriteToLogError(string.Format(TdLogging_Resources.QuerGetSchemaNamesFailed, TdTableName.ORA_SCHEMA_DATABASE_NAME));
                    }
                    else if (schemaOrDatabaseName == "ConnectionName")
                    {
                        TdLogging.WriteToLogError(string.Format(TdLogging_Resources.QuerGetConnNamesFailed, TdTableName.CONN_ORACLE));
                    }

                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
                finally
                {
                    this.DbConnection.Close();
                }
            }

            return names;
        }
        #endregion Maintain ComboboxItems

        #region SQLite (file) information

        /// <summary>
        /// Get the SQlite application database version.
        /// </summary>
        /// <returns>The version which is strored in de SQLite database file.</returns>
        public string GetPramaUserVersion()
        {
            string userVersion = string.Empty;
            string selectSql = "PRAGMA user_version";

            this.DbConnection.Open();
            if (TdDebugMode.DebugMode)
            {
                TdLogging.WriteToLogDebug(TdLogging_Resources.GetSqlDbVersion);
            }

            SQLiteCommand command = new(selectSql, this.DbConnection);
            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    userVersion = dr[0].ToString();
                }

                dr.Close();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(TdLogging_Resources.GetSqlDbVersionFailed);
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                userVersion = string.Empty;
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }

            return userVersion;
        }

        /// <summary>
        /// Get the SQlite (dll) version.
        /// </summary>
        /// <returns>The version of the used SQlite dll file.</returns>
        public string SQLiteVersion()
        {
            string sqliteDllVersion = string.Empty;
            string selectSql = "SELECT sqlite_version() as VERSION";

            this.DbConnection.Open();

            if (TdDebugMode.DebugMode)
            {
                TdLogging.WriteToLogDebug(TdLogging_Resources.GetSQLiteVersion);
            }

            SQLiteCommand command = new(selectSql, this.DbConnection);
            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    sqliteDllVersion = dr[0].ToString();
                }

                dr.Close();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(TdLogging_Resources.GetSQLiteVersionFailed);
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                sqliteDllVersion = string.Empty;
            }
            finally
            {
                command.Dispose();
                this.DbConnection.Close();
            }

            return sqliteDllVersion;
        }

        #endregion SQLite (file) information

        #region QueryGroups

        /// <summary>
        /// Get the querygroup names.
        /// </summary>
        /// <returns>datatable with ony the querygroup names.</returns>
        public DataTable LoadColorsReferenceDataTable()
        {
            string selectSql = string.Format("select id, exportfile_path from {0} order by 1", TdTableName.QUERY_GROUP_LIST);
            try
            {
                this.DbConnection.Open();
                using SQLiteCommand command = new(selectSql, this.DbConnection);

                DataTable dt = new ();
                dt.Load(command.ExecuteReader());
                return dt;
            }
            catch (Exception ex)
            {
                TdLogging.WriteToLogError(TdLogging_Resources.GetQueryGroupNamesFailed);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                return null;
            }
        }
        #endregion QueryGroups

        /// <summary>
        /// Compres the app. database.
        /// </summary>
        public void CompressDatabase()
        {
            this.DbConnection.Open();
            SQLiteCommand command = new(this.DbConnection);
            command.Prepare();
            command.CommandText = "vacuum;";
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(TdLogging_Resources.CompressAppDbFailed);
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

        /// <summary>
        /// Reset all sequences in the appliction databae.
        /// </summary>
        public void ResetAllAutoIncrementFields()
        {
            this.DbConnection.Open();
            SQLiteCommand command = new(this.DbConnection);
            command.Prepare();
            command.CommandText = "DELETE FROM sqlite_sequence;";
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(TdLogging_Resources.ResetAppDbSequenceFailed);
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

        /// <summary>
        /// Reset the sequence of one table.
        /// </summary>
        /// <param name="tableName">The table name of which the sequence will be reset.</param>
        /// </summary>
        public void ResetAutoIncrementFields(string tableName)
        {
            this.DbConnection.Open();
            SQLiteCommand command = new(this.DbConnection);
            command.Prepare();
            command.CommandText = "DELETE FROM sqlite_sequence WHERE name= @TABLE_NAME";
            try
            {
                command.Parameters.Add(new SQLiteParameter("@TABLE_NAME", tableName));
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(string.Format(TdLogging_Resources.ResetTableSequenceFailed, tableName));
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

        /// <summary>
        /// Make a copy of the application database file.
        /// </summary>
        /// <param name="type">Copy on start up: type="StartUp" or just copy.</param>
        /// <returns>true when copy succeeded.</returns>
        public bool CopyDatabaseFile(string type)
        {
            string fileToCopy = this.DatabaseFileName;

            DateTime dateTime = DateTime.UtcNow.Date;
            string currentDate = dateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

            string newLocation = Path.Combine(this.DbLocation, TdSettingsDefault.BackUpFolder) + currentDate + "_" + TdSettingsDefault.SqlLiteDatabaseName;

            bool result = false;

            if (Directory.Exists(Path.Combine(this.DbLocation, TdSettingsDefault.BackUpFolder)))
            {
                if (File.Exists(fileToCopy))
                {
                    if (type == "StartUp")
                    {
                        File.Copy(fileToCopy, newLocation, true);  // Overwrite file = true
                        TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.CopyFileReady, TdSettingsDefault.SqlLiteDatabaseName));
                        result = true;
                    }
                    else
                    {
                        if (File.Exists(newLocation))
                        {
                            DialogResult dialogResult = MessageBox.Show(MB_Text.File_Exist_Overwrite, MB_Title.File_Copy, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                File.Copy(fileToCopy, newLocation, true);  // Overwrite file = true
                                TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.CopyFileReady, TdSettingsDefault.SqlLiteDatabaseName));
                                result = true;
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.CopyFileAborted, TdSettingsDefault.SqlLiteDatabaseName));
                                TdLogging.WriteToLogInformation(TdLogging_Resources.FileExists);
                                result = false;
                            }
                        }
                        else
                        {
                            File.Copy(fileToCopy, newLocation, false);  // Overwrite file = false
                            TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.CopyFileReady, TdSettingsDefault.SqlLiteDatabaseName));
                            result = true;
                        }
                    }
                }
                else
                {
                    TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.FileNotPresent, TdSettingsDefault.SqlLiteDatabaseName));
                    MessageBox.Show(
                        string.Format(TdLogging_Resources.FileNotPresent, TdSettingsDefault.SqlLiteDatabaseName),
                        MB_Title.File_Copy,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    result = false;
                }
            }
            else
            {
                MessageBox.Show(
                    string.Format(TdLogging_Resources.FolderNotPresent, TdSettingsDefault.BackUpFolder),
                    MB_Title.File_Copy,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Make a copy of the app. database file without compressing the databaase first.
        /// </summary>
        /// <param name="folder">The location of the file.</param>
        /// <param name="overWrite">Yes is overwrite the existing file.</param>
        /// <returns>True if succes.</returns>
        public bool CopyDatabaseFileWithoutCompress(string folder, bool overWrite)
        {
            string fileToCopy = this.DatabaseFileName;
            string newLocation = folder;
            bool result;

            if (File.Exists(fileToCopy))
            {
                File.Copy(fileToCopy, newLocation, overWrite);
                TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.CopyFileReady, TdSettingsDefault.SqlLiteDatabaseName));
                result = true;
            }
            else
            {
                TdLogging.WriteToLogInformation(string.Format(TdLogging_Resources.FileNotPresent, TdSettingsDefault.SqlLiteDatabaseName));
                MessageBox.Show(
                    string.Format(TdLogging_Resources.FileNotPresent, TdSettingsDefault.SqlLiteDatabaseName),
                    MB_Title.File_Copy,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                result = false;
            }

            return result;
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
