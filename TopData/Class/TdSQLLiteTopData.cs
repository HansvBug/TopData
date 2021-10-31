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

    #region Folders

    /// <summary>
    /// /// Manage the query treeview folders.
    /// </summary>
    public enum FolderTypes
    {
        /// <summary>
        /// Represents a folder (in the treeview).
        /// </summary>
        Folder,

        /// <summary>
        /// Represents a Query (in the treeview).
        /// </summary>
        Query,
    }

    /// <summary>
    /// a List with folders whiche are treeview parent nodes.
    /// </summary>
    public class TdFolders
    {
        private readonly List<TdFolder> items = new ();

        /// <summary>
        /// Gets a list with TopdataFolder objects.
        /// </summary>
        public List<TdFolder> Items
        {
            get { return this.items; }
        } // List with all the folders
    }

    /// <summary>
    /// Maintain the folders (treeview parent nodes).
    /// </summary>
    public class TdFolder : TdSQliteDatabaseConnection
    {
        #region properties

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Foldertype (Foldertype = Map). Folder is a treeview node.
        /// </summary>
        public FolderTypes FolderType { get; set; }

        /// <summary>
        /// Gets or sets the guid of the folder (Folder is a treeview node).
        /// </summary>
        public string FolderGuid { get; set; }

        /// <summary>
        /// Gets or sets the folder name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets  the parent folder (Parent treeview node).
        /// </summary>
        public string ParentFolder { get; set; }

        /// <summary>
        /// Gets or sets the create date of the folder (treeview node).
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date when the folder is modified.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the user name who created the folder.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the user name who modified the folder.
        /// </summary>
        public string AlteredBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets Error.
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets an error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        #endregion properties

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdFolder"/> class.
        /// </summary>
        /// <param name="qId">The id of the selected query.</param>
        public TdFolder(int qId)
        {
            this.Id = qId;
        }
        #endregion constructor

        /// <summary>
        /// Save the folder data. Create a new folder or alter an existing folder.
        /// -1 = new folder. everything else is update a folder.
        /// </summary>
        public void SaveFolder()
        {
            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    this.Error = false;
                    this.ErrorMessage = string.Empty;
                    string insertSql;

                    if (this.Id == -1)
                    {
                        insertSql = string.Format("insert into {0} (GUID, NAME, FOLDERTYPE, PARENT, DATE_CREATED, DATE_ALTERED, CREATED_BY, ALTERED_BY)", TdTableName.FOLDER_LIST);
                        insertSql += "values ( @GUID, @NAME, @FOLDERTYPE, @PARENT, @DATE_CREATED, @DATE_ALTERED, @CREATED_BY, @ALTERED_BY)";
                    }
                    else
                    {
                        insertSql = "UPDATE FOLDER_LIST set NAME = @NAME,";
                        insertSql += "DATE_ALTERED = @DATE_ALTERED, ";
                        insertSql += "ALTERED_BY = @ALTERED_BY ";
                        insertSql += "where GUID = @GUID;";
                    }

                    SQLiteCommand command = new (insertSql, this.DbConnection);

                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@GUID", this.FolderGuid));
                    command.Parameters.Add(new SQLiteParameter("@NAME", this.Name));
                    command.Parameters.Add(new SQLiteParameter("@PARENT", this.ParentFolder));
                    command.Parameters.Add(new SQLiteParameter("@FOLDERTYPE", this.FolderType));

                    if (this.Id == -1)
                    {
                        command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));
                        command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));
                    }

                    command.Parameters.Add(new SQLiteParameter("@DATE_ALTERED", DateTime.Now));
                    command.Parameters.Add(new SQLiteParameter("@ALTERED_BY", Environment.UserName));

                    command.ExecuteNonQuery();
                    command.Dispose();
                    TdLogging.WriteToLogInformation("Nieuwe Folder is opgeslagen. Betreft : " + this.Name + ".");
                }
                catch (SQLiteException ex)
                {
                    if (this.Id == -1)
                    {
                        TdLogging.WriteToLogError(string.Format("Aanmaken van een nieuw record in de table {0} is misukt.", TdTableName.FOLDER_LIST));
                    }
                    else
                    {
                        TdLogging.WriteToLogError(string.Format("Bijwerken van een bestaand record in de tabel {0} is misukt.", TdTableName.FOLDER_LIST));
                    }

                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    this.ErrorMessage = "Opslaan niet gelukt" + Environment.NewLine + ex.Message;
                    this.Error = true;

                    tr.Rollback();
                }
                catch (Exception ex)
                {
                    if (this.Id == -1)
                    {
                        TdLogging.WriteToLogError(string.Format("Aanmaken van een record in de table {0} is misukt. ", TdTableName.FOLDER_LIST));
                    }
                    else
                    {
                        TdLogging.WriteToLogError(string.Format("Bijwerken van een record in de tabel {0} is misukt.", TdTableName.FOLDER_LIST));
                    }

                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    this.ErrorMessage = "Opslaan niet gelukt" + Environment.NewLine + ex.Message;
                    this.Error = true;

                    tr.Rollback();
                }

                tr.Commit();
            }

            this.DbConnection.Close();
        }

        /// <summary>
        /// Delete a folder.
        /// First check if there are queries within the folder, these queries will be deleted before the folder gets deleted.
        /// </summary>
        /// <param name="databaseFileName">the app database file location and name.</param>
        /// <returns>True if success.</returns>
        public bool DeleteFolderComplete(string databaseFileName)
        {
            try
            {
                string selectSql = string.Format("SELECT ID FROM {0} WHERE FOLDER = @GUID", TdTableName.QUERY_LIST);

                this.DbConnection.Open();

                SQLiteCommand command = new (selectSql, this.DbConnection);
                command.Prepare();
                command.Parameters.Add(new SQLiteParameter("@GUID", this.FolderGuid));

                SQLiteDataReader dr = command.ExecuteReader();
                DataTable dt = new ();
                dt.Load(dr);

                dr.Close();
                command.Dispose();
                if (dt.IsInitialized == true)
                {
                    foreach (DataRow dRow in dt.Rows)
                    {
                        int queryId = int.Parse(dRow["ID"].ToString(), CultureInfo.InvariantCulture);
                        TdQuery tdQ = new (queryId);
                        tdQ.DeleteQueryComplete();  // Remove the Query
                    }
                }
                else
                {
                    TdLogging.WriteToLogInformation("Er zijn geen query's gevonden binnen de te verwijderen folder.");
                }

                dt.Dispose();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Verwijderen van de relatie(s) folder-query is mislukt.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.Message);
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                return false;
            }
            finally
            {
                this.DbConnection.Close();
            }

            // And now we can delete the folder
            try
            {
                this.DeleteFolder();
                return true;
            }
            catch (Exception ex)
            {
                TdLogging.WriteToLogError("Het verwijderen van de folder is mislukt.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                return false;
            }
        }

        private void DeleteFolder()
        {
            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    string deleteSql = string.Format("DELETE FROM {0} WHERE GUID = @GUID", TdTableName.FOLDER_LIST);

                    SQLiteCommand command = new (deleteSql, this.DbConnection);
                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@GUID", this.FolderGuid));

                    command.ExecuteNonQuery();
                    command.Dispose();
                    TdLogging.WriteToLogError("De folder " + this.FolderGuid + " is verwijderd uit de tabel FOLDER_LIST.");
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het verwijderen van de folder uit de tabel FOLDER_LIST is misukt. (Guid = " + this.FolderGuid + ").");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();
                }

                tr.Commit();
            }

            this.DbConnection.Close();
        }
    }
    #endregion Folders

    #region Queries

    /// <summary>
    /// Class holding the queries.
    /// </summary>
    public class TdQueries
    {
        /// <summary>
        /// Gets or sets a reference of the main form.
        /// </summary>
        public FormMain Parent { get; set; }

        private readonly List<TdQuery> items = new ();

        /// <summary>
        /// Getst a list of all queries.
        /// </summary>
        public List<TdQuery> Items
        {
            get { return this.items; }
        }
    }

    /// <summary>
    /// Handle the queries.
    /// </summary>
    public class TdQuery : TdSQliteDatabaseConnection
    {
        private int id;

        #region properties

        /// <summary>
        /// List with the Querygroup namens.
        /// </summary>
        public List<string> QueryGroupNames = new ();
        private Dictionary<int, string> queryGroupIds = new ();

        /// <summary>
        /// Dictionary with the realated querygroup id's.
        /// </summary>
        public Dictionary<int, string> RelatedQueryGroupIds = new ();

        /// <summary>
        /// Gets or sets the query id.
        /// </summary>
        public int Id
        {
            get { return this.id; } set { this.id = 0; }
        }

        /// <summary>
        /// Gets or sets the query guid.
        /// </summary>
        public string QueryGuid { get; set; }

        /// <summary>
        /// Gets or sets the query name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the query discription.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the export file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the Excel worksheet name.
        /// </summary>
        public string WorksheetName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a query is locked.
        /// </summary>
        public bool QueryIsLocked { get; set; }

        /// <summary>
        /// Gets or sets the autoristion level of the query.
        /// </summary>
        public string QueryAutorisation { get; set; }

        /// <summary>
        /// Gets or sets the query group of the query.
        /// </summary>
        public string QueryGroup { get; set; }

        /// <summary>
        /// Gets or sets the qurey group id.
        /// </summary>
        public int QueryGroupId { get; set; }

        /// <summary>
        /// Gets or sets the export file of the query group.
        /// </summary>
        public string QueryGroupExportFile { get; set; }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the query create date and time.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date when the query is modified.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the user who made the query.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the user who altered the query.
        /// </summary>
        public string AlteredBy { get; set; }

        /// <summary>
        /// Gets or sets the folder name. The treeview node name.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the name of the output file.
        /// </summary>
        public string FileNameOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets Error.
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets an error message.
        /// </summary>
        public string ErrorMessage { get; set; }
        #endregion properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdQuery"/> class.
        /// </summary>
        /// <param name="qId">The id of the query.</param>
        public TdQuery(int qId)
        {
            this.id = qId;
            if (qId == -1)
            {
                this.QueryIsLocked = false;   // By default the query's are not locked
            }

            this.DbConnection = new SQLiteConnection("Data Source=" + this.DatabaseFileName);
        }
        #endregion Constructor

        /// <summary>
        /// Delate the Querygroup id's from the tabel query_group.
        /// </summary>
        /// <param name="deleteType">Delete all or just a single record.</param>
        public void DeleteQuerGroupIDs(string deleteType)
        {
            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    string deleteSql = string.Empty;
                    if (string.IsNullOrEmpty(deleteType))
                    {
                        deleteSql = string.Format("DELETE FROM {0} WHERE QUERY_ID = @QUERY_ID AND CONNECTION_ID = @CONNECTION_ID", TdTableName.QUERY_GROUP);
                    }
                    else
                    {
                        // When the query is deleted all records in QUERY_GROUP must be deleted.
                        deleteSql = string.Format("DELETE FROM {0} WHERE QUERY_ID = @QUERY_ID", TdTableName.QUERY_GROUP);
                    }

                    SQLiteCommand command = new (deleteSql, this.DbConnection);

                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@QUERY_ID", this.Id));
                    command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", TdUserData.ConnectionId));

                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het verwijderen van de Query-querygroep informatie is mislukt.");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();
                }

                tr.Commit();
            }

            this.DbConnection.Close();
        }

        /// <summary>
        /// Save the querygroup data.
        /// </summary>
        public void SaveQueryGroup()
        {
            // 0: delete existing records
            this.DeleteQuerGroupIDs(string.Empty);

            // 1: Get the queryGoupIds....
            this.GetQuerGroupIDs();

            // Save the query goup - query combi
            if (this.queryGroupIds.Count > 0)
            {
                this.DbConnection.Open();

                using (var tr = this.DbConnection.BeginTransaction())
                {
                    foreach (KeyValuePair<int, string> entry in this.queryGroupIds)
                    {
                        try
                        {
                            string insertSql = string.Format("insert into {0}(GUID, NAME, QUERY_GROUP_LIST_ID, QUERY_ID, CONNECTION_ID, DATE_CREATED, CREATED_BY) ", TdTableName.QUERY_GROUP);
                            insertSql += "values(@GUID, @NAME, @QUERY_GROUP_LIST_ID, @QUERY_ID, @CONNECTION_ID, @DATE_CREATED, @CREATED_BY)";

                            SQLiteCommand command = new (insertSql, this.DbConnection);

                            command.Prepare();
                            command.Parameters.Add(new SQLiteParameter("@GUID", Guid.NewGuid().ToString()));

                            command.Parameters.Add(new SQLiteParameter("NAME", entry.Value));

                            command.Parameters.Add(new SQLiteParameter("QUERY_GROUP_LIST_ID", entry.Key));
                            command.Parameters.Add(new SQLiteParameter("QUERY_ID", this.Id));
                            command.Parameters.Add(new SQLiteParameter("CONNECTION_ID", TdUserData.ConnectionId));
                            command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));
                            command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));

                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                        catch (SQLiteException)
                        {
                            tr.Rollback();
                        }
                    }

                    tr.Commit();
                }

                this.DbConnection.Close();
            }
        }

        private void GetQuerGroupIDs()
        {
            // Fill the quergroupid list --> QueryGroupIds
            try
            {
                if (this.QueryGroupNames.Count > 0)
                {
                    string selectSql = string.Empty;
                    this.queryGroupIds.Clear();

                    foreach (string qGroupName in this.QueryGroupNames)
                    {
                        selectSql = "Select ID, NAME from QUERY_GROUP_LIST where NAME = @NAME";

                        if (this.DbConnection.State != ConnectionState.Open)
                        {
                            this.DbConnection.Open();
                        }

                        SQLiteCommand command = new (selectSql, this.DbConnection);
                        command.Prepare();
                        command.Parameters.Add(new SQLiteParameter("@NAME", qGroupName));

                        SQLiteDataReader dr = command.ExecuteReader();
                        dr.Read();

                        if (dr.HasRows)
                        {
                            if (!this.queryGroupIds.ContainsValue(qGroupName))
                            {
                                this.queryGroupIds.Add(dr.GetInt32(0), dr.GetString(1));
                            }

                            dr.Close();
                            command.Dispose();
                        }
                        else
                        {
                            TdLogging.WriteToLogInformation("Er is geen relatie query-connectie gevonden.");
                            command.Dispose();
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Ophalen query groep id's is mislukt.");
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

        /// <summary>
        /// Select all the queries with a specific querygroup.
        /// </summary>
        /// <param name="queryId">The id of the selected query.</param>
        public void SelectRelatedQueries(int queryId)
        {
            try
            {
                string selectSql = string.Empty;
                this.RelatedQueryGroupIds.Clear();

                selectSql = string.Format("Select QUERY_GROUP_LIST_ID, NAME from {0} where QUERY_ID = @QUERY_ID and CONNECTION_ID = @CONNECTION_ID", TdTableName.QUERY_GROUP);

                this.DbConnection.Open();

                SQLiteCommand command = new (selectSql, this.DbConnection);
                command.Prepare();
                command.Parameters.Add(new SQLiteParameter("@QUERY_ID", queryId));
                command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", TdUserData.ConnectionId));

                using (SQLiteDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (!dr.IsDBNull(0))
                        {
                            this.RelatedQueryGroupIds.Add(dr.GetInt32(0), dr.GetString(1));
                        }
                    }
                }

                command.Dispose();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Ophalen relatie query-query groep is mislukt.");
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

        /// <summary>
        /// Connect the right folder guid to the dragged query.
        /// </summary>
        /// <param name="queryId">The id of the dragged query.</param>
        /// <param name="queryToGuid">The guid of the folder in which the query is dropped.</param>
        public void UpdateQueryFolderWhenDragged(int queryId, string queryToGuid)
        {
            string updateSql;
            {
                updateSql = string.Format("UPDATE {0} set FOLDER = @FOLDER", TdTableName.QUERY_LIST);
                updateSql += ", ";
                updateSql += "DATE_ALTERED = @DATE_ALTERED, ";
                updateSql += "ALTERED_BY = @ALTERED_BY ";
                updateSql += "where ID = @ID;";
            }

            this.DbConnection.Open();
            SQLiteCommand command = new (updateSql, this.DbConnection);

            command.Prepare();
            command.Parameters.Add(new SQLiteParameter("@ID", queryId));
            command.Parameters.Add(new SQLiteParameter("@FOLDER", queryToGuid));
            command.Parameters.Add(new SQLiteParameter("@DATE_ALTERED", DateTime.Now));
            command.Parameters.Add(new SQLiteParameter("@ALTERED_BY", Environment.UserName));

            try
            {
                command.ExecuteNonQuery();
                TdLogging.WriteToLogInformation("Een bestaande Query is verplaatst.");
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(string.Format("Bijwerken van het record in {0} is misukt. (Verplaats Query).", TdTableName.QUERY_LIST));
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
        /// Save the query into the table QUERY_LIST.
        /// </summary>
        public void SaveQuery()
        {
            this.DbConnection.Open();

            using var tr = this.DbConnection.BeginTransaction();
            try
            {
                this.Error = false;
                this.ErrorMessage = string.Empty;
                string sqlString;

                string qGrp = string.Empty;

                // Loop through the querygroup names
                foreach (string item in this.QueryGroupNames)
                {
                    if (string.IsNullOrEmpty(qGrp))
                    {
                        qGrp += item;
                    }
                    else if (!string.IsNullOrEmpty(qGrp))
                    {
                        qGrp += ", " + item;
                    }
                }

                if (this.Id == -1)
                {
                    // Insert the new Query
                    sqlString = string.Format("insert into {0} (GUID, FOLDER, WORKSHEET, QUERYNAME, DESCRIPTION, QUERY, DATE_CREATED, DATE_ALTERED, CREATED_BY, ALTERED_BY, FILENAME_EXP, LOCKED, QUERY_AUTORISATION, QUERY_GROUP)", TdTableName.QUERY_LIST);
                    sqlString += "values ( @GUID, @FOLDER, @WORKSHEET, @QUERYNAME, @DESCRIPTION, @QUERY, @DATE_CREATED, @DATE_ALTERED, @CREATED_BY, @ALTERED_BY, @FILENAME_EXP, @LOCKED, @QUERY_AUTORISATION, @QUERY_GROUP)";
                }
                else
                {
                    sqlString = string.Format("UPDATE {0} set QUERYNAME = @QUERYNAME,", TdTableName.QUERY_LIST);
                    sqlString += "FOLDER = @FOLDER, ";
                    sqlString += "WORKSHEET = @WORKSHEET, ";
                    sqlString += "LOCKED = @LOCKED, ";
                    sqlString += "DESCRIPTION = @DESCRIPTION, ";
                    sqlString += "QUERY = @QUERY, ";
                    sqlString += "DATE_ALTERED = @DATE_ALTERED, ";
                    sqlString += "ALTERED_BY = @ALTERED_BY, ";
                    sqlString += "FILENAME_EXP = @FILENAME_EXP, ";
                    sqlString += "QUERY_AUTORISATION = @QUERY_AUTORISATION, ";
                    sqlString += "QUERY_GROUP = @QUERY_GROUP ";
                    sqlString += "where GUID = @GUID;";
                }

                SQLiteCommand command = new (sqlString, this.DbConnection);

                command.Prepare();

                command.Parameters.Add(new SQLiteParameter("@GUID", this.QueryGuid));
                command.Parameters.Add(new SQLiteParameter("@FOLDER", this.Folder));
                command.Parameters.Add(new SQLiteParameter("@WORKSHEET", this.WorksheetName));
                command.Parameters.Add(new SQLiteParameter("@LOCKED", TdEncryptDecrypt.Encrypt(this.QueryIsLocked + this.QueryGuid, TdSettingsDefault.StringSleutel)));

                command.Parameters.Add(new SQLiteParameter("@QUERYNAME", this.Name));
                command.Parameters.Add(new SQLiteParameter("@DESCRIPTION", this.Description));
                command.Parameters.Add(new SQLiteParameter("@QUERY_AUTORISATION", this.QueryAutorisation));
                command.Parameters.Add(new SQLiteParameter("@QUERY_GROUP", qGrp));

                if (this.Query == null)
                {
                    this.Query = string.Empty;
                }

                command.Parameters.Add(new SQLiteParameter("@QUERY", TdEncryptDecrypt.Encrypt(this.Query, TdSettingsDefault.StringSleutel)));

                if (this.Id == -1)
                {
                    command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));
                    command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));
                }

                command.Parameters.Add(new SQLiteParameter("@DATE_ALTERED", DateTime.Now));
                command.Parameters.Add(new SQLiteParameter("@ALTERED_BY", Environment.UserName));
                if (this.FileNameOutput == null)
                {
                    this.FileNameOutput = string.Empty; // TODO change the creation of the table and remove the null in the create statement
                }

                command.Parameters.Add(new SQLiteParameter("@FILENAME_EXP", this.FileNameOutput));

                command.ExecuteNonQuery();
                command.Dispose();

                if (this.Id == -1)
                {
                    TdLogging.WriteToLogInformation("Nieuwe Query is opgeslagen. (Naam : " + this.Name + " , id : " + Convert.ToString(this.Id, CultureInfo.InvariantCulture) + ")");
                }
                else
                {
                    TdLogging.WriteToLogInformation("Een bestaande Query is gewijzigd. (Naam : " + this.Name + " , id : " + Convert.ToString(this.Id, CultureInfo.InvariantCulture) + ")");
                }
            }
            catch (SQLiteException ex)
            {
                if (this.Id == -1)
                {
                    TdLogging.WriteToLogError("Het aanmaken van een nieuw record in de tabel QUERY_LIST is misukt. (SQLiteException).");
                }
                else
                {
                    TdLogging.WriteToLogError(string.Format("Het bijwerken van een record in de tabel {0} is misukt.", TdTableName.QUERY_LIST));
                }

                TdLogging.WriteToLogError("Melding: ");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                this.ErrorMessage = "Opslaan niet gelukt" + Environment.NewLine + ex.Message;
                this.Error = true;

                tr.Rollback();
            }
            catch (Exception ex)
            {
                if (this.Id == -1)
                {
                    TdLogging.WriteToLogError(string.Format("Het aanmaken van een nieuw record in de tabel {0} is misukt.", TdTableName.QUERY_LIST));
                    TdLogging.WriteToLogError("Melding: ");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
                else
                {
                    TdLogging.WriteToLogError(string.Format("Het bijwerken van een record in de tabel {0} is misukt.", TdTableName.QUERY_LIST));
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
            }

            tr.Commit();
            this.DbConnection.Close();
        }

        /// <summary>
        /// Delete the selecte query and realions to connections complete.
        /// </summary>
        /// <returns>True if query is deleted.</returns>
        public bool DeleteQueryComplete()
        {
            int connectionId;

            // Delete the query-connection
            try
            {
                string selectSql = string.Format("select CONNECTION_ID from {0} where QUERY_ID = @ID", TdTableName.CONNECTION_QUERY);

                this.DbConnection.Open();

                SQLiteCommand command = new (selectSql, this.DbConnection);
                command.Prepare();
                command.Parameters.Add(new SQLiteParameter("@ID", this.Id));

                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    connectionId = dr.GetInt32(0);

                    SQLLiteTopData deleteQueryConnection = new ();

                    dr.Close();
                    command.Dispose();

                    deleteQueryConnection.DeleteRelateAllConnectionsFromQuery(connectionId, this.Id);
                }
                else
                {
                    TdLogging.WriteToLogInformation("Er is geen relatie query-connectie gevonden.");
                    command.Dispose();
                }

                this.DbConnection.Close();
                this.DeleteQuerGroupIDs("All");  // Delete the querygroup relations
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Verwijderen relatie query-connectie is mislukt.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                return false;
            }

            // Delete the query
            try
            {
                this.DeleteQuery(this.Id);
                return true;
            }
            catch (Exception ex)
            {
                TdLogging.WriteToLogError("Het verwijderen van de query is mislukt.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                return false;
            }
        }

        private void DeleteQuery(int queryId)
        {
            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    string deleteSql = "DELETE FROM QUERY_LIST WHERE ID = @QUERY_ID";

                    SQLiteCommand command = new (deleteSql, this.DbConnection);

                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@QUERY_ID", queryId));

                    command.ExecuteNonQuery();
                    command.Dispose();
                    TdLogging.WriteToLogInformation("De Query met id '" + queryId + string.Format("' is verwijderd uit de tabel {0}.", TdTableName.QUERY_LIST));
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het verwijderen van de Query met id = " + Convert.ToString(queryId, CultureInfo.InvariantCulture) + string.Format(" uit de tabel {0} is misukt.", TdTableName.QUERY_LIST));
                    TdLogging.WriteToLogError("Melding:");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();
                }

                tr.Commit();
            }

            this.DbConnection.Close();
        }
    }

    #endregion Queries

    /// <summary>
    /// SQLLiteTopData.
    /// </summary>
    public class SQLLiteTopData : TdSQliteDatabaseConnection
    {
        /// <summary>
        /// Gets or sets the reference to the main form.
        /// </summary>
        public FormMain Parent { get; set; }

        /// <summary>
        /// Gets the id of a new query.
        /// </summary>
        public int Id { get; }

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLLiteTopData"/> class.
        /// </summary>
        public SQLLiteTopData()
        {
            this.DbConnection = new SQLiteConnection("Data Source=" + this.DatabaseFileName);
        }

        #endregion constructor

        #region Oracle connections

        /// <summary>
        /// Get the Oracle connection mames.
        /// </summary>
        /// <returns>A list with connectionames.</returns>
        public TdOracleConnections GetOracleConnectionNames()
        {
            return this.GetOracleConnectionNames(string.Empty);
        }

        /// <summary>
        /// The where clause for getting the Oracle connection names.
        /// </summary>
        /// <param name="whereClause">The Sql where clause.</param>
        /// <returns>A list ith connection names.</returns>
        public TdOracleConnections GetOracleConnectionNames(string whereClause)
        {
            TdOracleConnections oraConnections = new ();
            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                // Create Connections table
                // string SELECTSQL = string.Format("SELECT ID, CONNECTNAME, SCHEMANAME, SCHEMAPASS, CONNECT, DATE_CREATED, CREATED_BY FROM {0}", TableName.CONN_ORACLE);
                string selectSql = string.Format("SELECT ID, CONNECTNAME, SCHEMANAME, SCHEMAPASS, CONNECT FROM {0}", TdTableName.CONN_ORACLE);
                if (!string.IsNullOrEmpty(whereClause))
                {
                    selectSql += " " + whereClause;
                }

                selectSql += " ORDER BY CONNECTNAME";

                this.DbConnection.Open();
                try
                {
                    using SQLiteCommand command = new(selectSql, this.DbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();

                    using (DataTable dt = new ())
                    {
                        dt.Load(reader);
                        using TdSecurityExtensions securityExt = new();

                        foreach (DataRow row in dt.Rows)
                        {
                            int id = int.Parse(row["ID"].ToString(), CultureInfo.InvariantCulture);
                            TdOracleConnection item = new (id)
                            {
                                Name = row["CONNECTNAME"].ToString(),
                                Schema = row["SCHEMANAME"].ToString(),
                                Password = securityExt.ConvertToSecureString(row["SCHEMAPASS"].ToString()),
                                Connection = row["CONNECT"].ToString(),
                            };
                            oraConnections.Items.Add(item);
                        }
                    }

                    reader.Close();
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Onverwachte fout bij het ophalen van de Oracle schema connectie namen.");
                    TdLogging.WriteToLogError("Melding: ");
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

            return oraConnections;
        }

        /// <summary>
        /// Make a relation between query en current connection.
        /// </summary>
        /// <param name="connectionId">The current connection id.</param>
        /// <param name="queryId">the current query id.</param>
        /// <returns>true if success.</returns>
        public bool RelateConnectionToQuery(int connectionId, int queryId)
        {
            // First Delete relation just in case
            this.DeleteRelateConnectionFromQuery(connectionId, queryId);

            bool result;

            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    string insertSql = string.Format("insert into {0} (CONNECTION_ID, QUERY_ID, CREATED_BY)", TdTableName.CONNECTION_QUERY);
                    insertSql += "values ( @CONNECTION_ID, @QUERY_ID, @CREATED_BY)";

                    using SQLiteCommand command = new (insertSql, this.DbConnection);
                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", connectionId));
                    command.Parameters.Add(new SQLiteParameter("@QUERY_ID", queryId));
                    command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));

                    command.ExecuteNonQuery();

                    TdLogging.WriteToLogInformation("Relatie tussen Query id " + Convert.ToString(queryId, CultureInfo.InvariantCulture) + " en Connectie id " +
                                                  Convert.ToString(connectionId, CultureInfo.InvariantCulture) + " is ingevoerd.");
                    result = true;
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het aanmaken van de relatie tussen Query id " + Convert.ToString(queryId, CultureInfo.InvariantCulture) +
                                            " en Connectie id " + Convert.ToString(connectionId, CultureInfo.InvariantCulture) + " is mislukt.");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    result = false;

                    tr.Rollback();
                }

                tr.Commit();
            }

            this.DbConnection.Close();
            return result;
        }

        /// <summary>
        /// Delete record(s) from table: CONNECTION_QUERY.
        /// </summary>
        /// <param name="connectionId">The connection id.</param>
        /// <param name="queryId">The query id.</param>
        /// <returns>True if query is executed.</returns>
        public bool DeleteRelateConnectionFromQuery(int connectionId, int queryId)
        {
            bool result;

            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    string deleteSql = string.Format("DELETE FROM {0} ", TdTableName.CONNECTION_QUERY);
                    deleteSql += "WHERE CONNECTION_ID = @CONNECTION_ID AND QUERY_ID = @QUERY_ID";

                    using SQLiteCommand command = new (deleteSql, this.DbConnection);
                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", connectionId));
                    command.Parameters.Add(new SQLiteParameter("@QUERY_ID", queryId));

                    command.ExecuteNonQuery();

                    TdLogging.WriteToLogInformation(string.Format("Een Query-connectie is verwijderd uit de tabel {0}. (Query_id was : ", TdTableName.CONNECTION_QUERY) +
                                                   Convert.ToString(queryId, CultureInfo.InvariantCulture) + " en connectie_id was : " +
                                                   Convert.ToString(connectionId, CultureInfo.InvariantCulture) + ")");
                    result = true;
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Verwijderen 'Query - Connection' relatie is misukt.");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();
                    result = false;
                }

                tr.Commit();
            }

            this.DbConnection.Close();
            return result;
        }
        #endregion Oracle connections

        #region Get the folders

        /// <summary>
        /// Get all the folders (names).
        /// </summary>
        /// <returns>A list with the foldeers.</returns>
        public TdFolders GetFolders()
        {
            return this.GetFolders(string.Empty);
        }

        /// <summary>
        /// Get a folder (name).
        /// </summary>
        /// <param name="whereClause">The where part of the select sql.</param>
        /// <returns>A list with folders (foldernames).</returns>
        public TdFolders GetFolders(string whereClause)
        {
            TdFolders tdFolders = new ();
            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                string selectSql = string.Format("SELECT ID, GUID, NAME, FOLDERTYPE, PARENT, DATE_CREATED, DATE_ALTERED, CREATED_BY, ALTERED_BY FROM {0}", TdTableName.FOLDER_LIST);
                if (!string.IsNullOrEmpty(whereClause))
                {
                    selectSql += " " + whereClause;
                }

                selectSql += " ORDER BY NAME";

                this.DbConnection.Open();
                try
                {
                    SQLiteCommand command = new (selectSql, this.DbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();

                    DataTable dt = new ();
                    dt.Load(reader);
                    foreach (DataRow row in dt.Rows)
                    {
                        int id = int.Parse(row["ID"].ToString(), CultureInfo.InvariantCulture);

                        TdFolder item = new (id)
                        {
                            FolderGuid = row["GUID"].ToString(),
                            Name = row["NAME"].ToString(),
                            ParentFolder = row["PARENT"].ToString(),
                            CreatedBy = row["CREATED_BY"].ToString(),
                            AlteredBy = row["ALTERED_BY"].ToString(),
                            Created = DateTime.Parse(row["DATE_CREATED"].ToString()),
                            Modified = DateTime.Parse(row["DATE_ALTERED"].ToString()),
                        };
                        tdFolders.Items.Add(item);
                    }

                    reader.Close();
                    command.Dispose();
                    dt.Dispose();
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Onverwachte fout opgetreden bij het ophalen van de folders.");
                    TdLogging.WriteToLogError("Melding: ");
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

            return tdFolders;
        }
        #endregion Get the Folders

        #region Get the Queries

        /// <summary>
        /// A list of multiple queries.
        /// </summary>
        /// <returns>A list with multiple queries.</returns>
        public TdQueries GetQueries()
        {
            return this.GetQueries(string.Empty);
        }

        /// <summary>
        /// get all the queries.
        /// </summary>
        /// <param name="whereClause">Extend the query with a where clause.</param>
        /// <returns>A list with queries.</returns>
        public TdQueries GetQueries(string whereClause)
        {
            TdQueries queries = new ()
            {
                Parent = this.Parent,
            };
            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                using SQLiteConnection dbConnection = new ("Data Source=" + this.DatabaseFileName);

                string selectSql = string.Format("SELECT ID, GUID, FOLDER, QUERYNAME, DESCRIPTION, WORKSHEET, QUERY, DATE_CREATED, DATE_ALTERED, CREATED_BY, ALTERED_BY, FILENAME_EXP, LOCKED, QUERY_AUTORISATION, QUERY_GROUP FROM {0} ", TdTableName.QUERY_LIST);

                if (TdUserData.UserRole == TdRoleTypes.Owner)
                {
                    selectSql += "WHERE (QUERY_AUTORISATION in ('" + TdRoleTypes.Owner + "', '" + TdRoleTypes.System + "', '" + TdRoleTypes.Administrator + "', '" + TdRoleTypes.Muteren + "', '" + TdRoleTypes.Raadplegen + "', '') or QUERY_AUTORISATION is null ) ";
                }
                else if (TdUserData.UserRole == TdRoleTypes.System)
                {
                    selectSql += "WHERE (QUERY_AUTORISATION in ('" + TdRoleTypes.System + "', '" + TdRoleTypes.Administrator + "', '" + TdRoleTypes.Muteren + "', '" + TdRoleTypes.Raadplegen + "', '') or QUERY_AUTORISATION is null ) ";
                }
                else if (TdUserData.UserRole == TdRoleTypes.Administrator)
                {
                    selectSql += "WHERE (QUERY_AUTORISATION in ('" + TdRoleTypes.Administrator + "', '" + TdRoleTypes.Muteren + "', '" + TdRoleTypes.Raadplegen + "', '') or QUERY_AUTORISATION is null ) ";
                }
                else if (TdUserData.UserRole == TdRoleTypes.Muteren)
                {
                    selectSql += "WHERE QUERY_AUTORISATION in ('" + TdRoleTypes.Muteren + "', '" + TdRoleTypes.Raadplegen + "') ";
                }
                else if (TdUserData.UserRole == TdRoleTypes.Raadplegen)
                {
                    selectSql += "WHERE QUERY_AUTORISATION = '" + TdRoleTypes.Raadplegen + "' ";
                }
                else
                {
                    selectSql += "WHERE QUERY_AUTORISATION = '" + TdRoleTypes.Raadplegen + "' ";
                }

                if (!string.IsNullOrEmpty(whereClause))
                {
                    selectSql += " " + whereClause;
                }

                selectSql += " ORDER BY QUERYNAME";

                dbConnection.Open();

                try
                {
                    using SQLiteCommand command = new (selectSql, dbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();

                    using DataTable dt = new ();
                    dt.Load(reader);
                    int id;
                    foreach (DataRow row in dt.Rows)
                    {
                        id = int.Parse(row["ID"].ToString(), CultureInfo.InvariantCulture);

                        TdQuery item = new (id)
                        {
                            QueryGuid = row["GUID"].ToString(),
                            Folder = row["FOLDER"].ToString(),
                            Name = row["QUERYNAME"].ToString(),
                            Description = row["DESCRIPTION"].ToString(),
                            WorksheetName = row["WORKSHEET"].ToString(),
                            QueryAutorisation = row["QUERY_AUTORISATION"].ToString(),
                            QueryGroup = row["QUERY_GROUP"].ToString(),
                        };

                        // Existing queries have an empty locked field
                        if (!string.IsNullOrEmpty(row["LOCKED"].ToString()))
                        {
                            // Queryislocked is the concatenation of querylock and the query guid
                            string temp = TdEncryptDecrypt.Decrypt(row["LOCKED"].ToString(), TdSettingsDefault.StringSleutel);
                            temp = temp.Replace(item.QueryGuid, string.Empty);
                            item.QueryIsLocked = Convert.ToBoolean(temp, CultureInfo.InvariantCulture);
                        }

                        // Assume Query is encrypted. When Query is not encrypted read als plain text.
                        try
                        {
                            item.Query = TdEncryptDecrypt.Decrypt(row["QUERY"].ToString(), TdSettingsDefault.StringSleutel);
                        }
                        catch
                        {
                            item.Query = row["QUERY"].ToString();
                        }

                        // Add the querygroup names to the query object
                        TdQuery tdq = new (this.Id);
                        tdq.SelectRelatedQueries(id);

                        foreach (KeyValuePair<int, string> entry in tdq.RelatedQueryGroupIds)
                        {
                            item.QueryGroupNames.Add(entry.Value);
                        }

                        // Add the querygroup names to the query object
                        reader.Close();

                        item.CreatedBy = row["CREATED_BY"].ToString();
                        item.Created = DateTime.Parse(row["DATE_CREATED"].ToString());
                        item.Modified = DateTime.Parse(row["DATE_ALTERED"].ToString());
                        item.AlteredBy = row["ALTERED_BY"].ToString();
                        item.FileNameOutput = row["FILENAME_EXP"].ToString();

                        queries.Items.Add(item);
                    }
                }
                catch(System.Data.ConstraintException cex)
                {
                    TdLogging.WriteToLogError("Bij het ophalen van de query's is een onverwachte fout opgetreden.");
                    TdLogging.WriteToLogError("Melding:");
                    TdLogging.WriteToLogError(cex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(cex.ToString());
                    }
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Bij het ophalen van de query's is een onverwachte fout opgetreden.");
                    TdLogging.WriteToLogError("Melding:");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
                finally
                {
                    dbConnection.Close();
                }
            }
            else
            {
                MessageBox.Show("Bij het ophalen van de query's is een onverwachte fout opgetreden." + Environment.NewLine + "De applicatie database locatie ontbreekt.", "Fout.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return queries;
        }
        #endregion Get the Queries

        /// <summary>
        /// Select rows from table. All the folders or queries with a certain id.
        /// </summary>
        /// <param name="sql">Sql select string.</param>
        /// <returns>The selected datarows.</returns>
        public DataRow SelectedRowFromQuery(string sql)
        {
            DataRow dRow = null;

            if (!string.IsNullOrEmpty(sql))
            {
                if (!string.IsNullOrEmpty(this.DatabaseFileName))
                {
                    try
                    {
                        this.DbConnection.Open();
                        try
                        {
                            using SQLiteCommand command = new(sql, this.DbConnection);
                            SQLiteDataReader reader = command.ExecuteReader();

                            DataTable dt = new ();
                            dt.Load(reader);
                            dRow = dt.Rows[0];

                            dt.Dispose();
                        }
                        catch (SQLiteException ex)
                        {
                            TdLogging.WriteToLogError("Onverwachte fout opgetreden in de functie SelectedRowFromQuery");
                            TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                            TdLogging.WriteToLogError(ex.Message);
                            if (TdDebugMode.DebugMode)
                            {
                                TdLogging.WriteToLogDebug(ex.ToString());
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            TdLogging.WriteToLogError("Een niet bestaande Folder_guid aangetroffen.");
                            TdLogging.WriteToLogError("query die geen resultaat geeft: " + sql);
                            TdLogging.WriteToLogError("Melding:");
                            TdLogging.WriteToLogError(ex.Message);
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        TdLogging.WriteToLogError("Onverwachte fout opgetreden in de functie SelectedRowFromQuery.");
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

                return dRow;
            }

            return dRow;
        }

        /// <summary>
        /// Select the Oracle connections which have a connection with the selectd query.
        /// Used in the after select querytreeview (maintain query form).
        /// </summary>
        /// <param name="queryId">The id of the selectd query.</param>
        /// <returns>A list with Oracle connections.</returns>
        public TdOracleConnections SelectRelatedConnections(int queryId)
        {
            string where = string.Format("WHERE ID IN (SELECT CONNECTION_ID FROM {0} WHERE QUERY_ID = " + Convert.ToString(queryId, CultureInfo.InvariantCulture) + ")", TdTableName.CONNECTION_QUERY);
            return this.GetOracleConnectionNames(where);
        }

        /// <summary>
        /// Delete all oracle connectionrecords.
        /// </summary>
        /// <param name="connectionId">The selected Oracle connection id.</param>
        /// <param name="queryId">The selected query id.</param>
        /// <returns>True if success.</returns>
        public bool DeleteRelateAllConnectionsFromQuery(int connectionId, int queryId)
        {
            bool result;

            this.DbConnection.Open();

            using (var tr = this.DbConnection.BeginTransaction())
            {
                try
                {
                    string deleteSql = string.Format("DELETE FROM {0} ", TdTableName.CONNECTION_QUERY);
                    deleteSql += "WHERE QUERY_ID = @QUERY_ID AND CONNECTION_ID = @CONNECTION_ID";

                    SQLiteCommand command = new (deleteSql, this.DbConnection);

                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@QUERY_ID", queryId));
                    command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", connectionId));

                    command.ExecuteNonQuery();
                    command.Dispose();
                    TdLogging.WriteToLogInformation("De relatie 'Query - Connectie(s)' is/zijn verwijderd.");
                    result = true;
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Verwijderen van alle 'Query - Connectie(s)' relatie(s) is misukt.");
                    TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();
                    result = false;
                }

                tr.Commit();
            }

            this.DbConnection.Close();
            return result;
        }

        /// <summary>
        /// Select all queries with the selcted Oracle connection id.
        /// </summary>
        /// <param name="connectionId">The id of the Oracle connection.</param>
        /// <returns>A list with queries.</returns>
        public TdQueries SelectRelatedQueries(int connectionId)
        {
            // Get all the query's with a relation tot the current connection
            string where = string.Format("AND ID IN (SELECT QUERY_ID FROM {0} WHERE CONNECTION_ID = " + connectionId + ")", TdTableName.CONNECTION_QUERY); // Convert.ToString(ConnectionId, CultureInfo.InvariantCulture) + ")";
            return this.GetQueries(where);
        }

        #region is Query or query_group active

        /// <summary>
        /// Check if the query exists in the QUERY_IS_ACTIVE table.
        /// </summary>
        /// <param name="queryId">The id of the query.</param>
        /// <param name="queryGroupId">The query group of the query.</param>
        /// <returns>True when the querys is fount in the table QUERY_IS_ACTIVE. </returns>
        public bool IsQueryActive(int queryId, int queryGroupId)
        {
            bool qisActive = false;

            string selectSql = string.Format("SELECT query_id, Query_GROUP_ID FROM {0} WHERE (QUERY_ID = @QUERY_ID AND QUERY_GROUP_ID = @QUERY_GROUP_ID)", TdTableName.QUERY_IS_ACTIVE);

            this.DbConnection.Open();

            using SQLiteCommand command = new(selectSql, this.DbConnection);
            command.Prepare();
            command.Parameters.Add(new SQLiteParameter("@QUERY_ID", queryId));
            command.Parameters.Add(new SQLiteParameter("@QUERY_GROUP_ID", queryGroupId));

            using (SQLiteDataReader dr = command.ExecuteReader())
            {
                while (dr.Read())
                {
                    // if a record is found then the query is all ready started (and may not be started for the scond time).
                    if (!dr.IsDBNull(0))
                    {
                        qisActive = true;
                    }
                    else
                    {
                        qisActive = false;
                    }
                }
            }

            this.DbConnection.Close();
            return qisActive;
        }

        /// <summary>
        /// If the query is not started then insert the query in the table QUERY_IS_ACTIVE.
        /// </summary>
        /// <param name="queryId">the id of the query.</param>
        /// <param name="queryGroupId">the querygroup id.</param>
        /// <param name="queryGuid">The query guid.</param>
        public void InsertQueryIsActiveData(int queryId, int queryGroupId, string queryGuid)
        {
            this.DbConnection.Open();

            using var tr = this.DbConnection.BeginTransaction();
            try
            {
                string insertSql = string.Format("insert into {0} (guid, query_id, QUERY_GUID, Query_GROUP_ID, date_created, created_by) ", TdTableName.QUERY_IS_ACTIVE);
                insertSql += "values (@GUID, @QUERY_ID, @QUERY_GUID, @Query_GROUP_ID, @DATE_CREATED, @CREATED_BY)";

                using SQLiteCommand command = new(insertSql, this.DbConnection);
                command.Prepare();
                command.Parameters.Add(new SQLiteParameter("@GUID", System.Guid.NewGuid().ToString()));
                command.Parameters.Add(new SQLiteParameter("@QUERY_ID", queryId));
                command.Parameters.Add(new SQLiteParameter("@QUERY_GUID", queryGuid));

                command.Parameters.Add(new SQLiteParameter("@Query_GROUP_ID", queryGroupId));
                command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now.ToString(CultureInfo.InvariantCulture)));
                command.Parameters.Add(new SQLiteParameter("@CREATED_BY", Environment.UserName));

                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(string.Format("Het aanmaken van records in de tabel {0} is mislukt.", TdTableName.QUERY_IS_ACTIVE));
                TdLogging.WriteToLogError("Melding :");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                tr.Rollback();
            }
            finally
            {
                tr.Commit();
                this.DbConnection.Close();
            }
        }

        /// <summary>
        /// Remove the query from the query is active table.
        /// </summary>
        /// <param name="queryId">The id of the query.</param>
        /// <param name="queryGroupId">The query group id of the query.</param>
        /// <param name="queryGuid">The guid of the query.</param>
        public void RemoveQueryIsActive(int queryId, int queryGroupId, string queryGuid)
        {
            this.DbConnection.Open();

            using var tr = this.DbConnection.BeginTransaction();
            try
            {
                string deleteQuery = string.Format("DELETE FROM {0} WHERE QUERY_ID = @QUERY_ID AND QUERY_GUID = @QUERY_GUID AND QUERY_GROUP_ID = @QUERY_GROUP_ID", TdTableName.QUERY_IS_ACTIVE);

                SQLiteCommand command = new(deleteQuery, this.DbConnection);
                command.Prepare();
                command.Parameters.Add(new SQLiteParameter("@QUERY_ID", queryId));
                command.Parameters.Add(new SQLiteParameter("@QUERY_GUID", queryGuid));
                command.Parameters.Add(new SQLiteParameter("@QUERY_GROUP_ID", queryGroupId));

                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError(string.Format("Het verwijderen van de query uit de tabel {0} is misukt. (Query_id = ", TdTableName.QUERY_IS_ACTIVE) + queryId.ToString(CultureInfo.InvariantCulture) + ").");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                tr.Rollback();
            }
            finally
            {
                tr.Commit();
                this.DbConnection.Close();
            }
        }
        #endregion is Query or query_group active
    }
}
