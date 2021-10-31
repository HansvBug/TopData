namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// Maintain the Query groups.
    /// </summary>
    public class TdMaintainQueryGroups : TdSQliteDatabaseConnection
    {
        #region Fields

        private readonly BindingSource bndSource; // Used with the dataadapter. It is just an empty bindingsource but seems to be needed ?!?!?!

        /// <summary>
        /// A datatabel holding the Query groups.
        /// Used in FormMaintainQueryGroups. (So it has to be public).
        /// </summary>
        private DataTable dt;

        private DataTable changeTableModified;
        private DataTable changeTableDeleted;
        private DataTable changeTableAdded;

        private SQLiteDataAdapter da = new ();
        #endregion Fields

        /// <summary>
        /// Gets or sets the datatable which holds the query groups.
        /// </summary>
        public DataTable Dt
        {
            get { return this.dt; }
            set { this.dt = value; }
        }

        /// <summary>
        /// Gets or sets the Folder name where the query group will save the export file.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the filename name of the query groupexport file.
        /// </summary>
        public string FileName { get; set; }

        private Dictionary<int, string> querygroups = new ();

        /// <summary>
        /// Gets or sets a dictionary with querygroup id and name.
        /// </summary>
        public Dictionary<int, string> Querygroups
        {
            get { return this.querygroups; }
            set { this.querygroups = value; }
        }

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdMaintainQueryGroups"/> class.
        /// </summary>
        public TdMaintainQueryGroups()
        {
            // Default constructor.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdMaintainQueryGroups"/> class.
        /// </summary>
        /// <param name="bndSource">The bindingsource needed for the dataadaptor. (Apply updates).</param>
        public TdMaintainQueryGroups(BindingSource bndSource)
        {
            this.bndSource = bndSource;
        }
        #endregion constructor

        /// <summary>
        /// Get all the querygroup names and other attributes from table QUERY_GROUP_LIST.
        /// </summary>
        /// <param name="aDatagridview">The datagridview in which the querynames are loaded.</param>
        public void GetAllQueryGroups(DataGridView aDatagridview)
        {
            string selectSql = string.Format("select ID, GUID, NAME as Naam, EXPORTFILE_PATH as Export_locatie, EXPORTFILE_NAME as Bestandsnaam, CONNECTION_ID, CREATED_BY as Aangemaakt_door, ifnull(NULL,DATE_CREATED) as Datum_aamgemaakt, ALTERED_BY as Gewijzigd_door, ifnull(NULL,DATE_ALTERED) as Datum_Gewijzigd from {0} order by name", TdTableName.QUERY_GROUP_LIST);

            try
            {
                this.DbConnection.Open();

                this.da = new SQLiteDataAdapter(selectSql, this.DbConnection);
                SQLiteCommandBuilder commandBuilder = new(this.da);

                this.Dt = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture,
                };
                this.da.Fill(this.Dt);

                this.bndSource.DataSource = this.Dt;
                aDatagridview.DataSource = this.Dt;

                if (this.da != null)
                {
                    if (aDatagridview != null)
                    {
                        aDatagridview.DataSource = this.Dt;
                        aDatagridview.Columns[0].Visible = false;  // ID
                        aDatagridview.Columns[1].Visible = false;  // GUID
                        aDatagridview.Columns[5].Visible = false;  // CONNECTION_ID

                        // Disable editing
                        aDatagridview.Columns[6].ReadOnly = true;   // Created_by
                        aDatagridview.Columns[7].ReadOnly = true;   // Date created
                        aDatagridview.Columns[8].ReadOnly = true;   // Alterd by
                        aDatagridview.Columns[9].ReadOnly = true;   // Date altered

                        // Change color
                        aDatagridview.Columns[6].DefaultCellStyle.BackColor = Color.LightGray;
                        aDatagridview.Columns[7].DefaultCellStyle.BackColor = Color.LightGray;
                        aDatagridview.Columns[8].DefaultCellStyle.BackColor = Color.LightGray;
                        aDatagridview.Columns[9].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                    else
                    {
                        TdLogging.WriteToLogError("Fout bij het ophalen van alle Querygroepen.");
                    }
                }
                else
                {
                    TdLogging.WriteToLogError("Fout bij het ophalen van alle Querygroepen.");
                }
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Het ophalen van alle query groep namen is mislukt.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }
            }
            catch (FormatException ex)
            {
                TdLogging.WriteToLogError("Het ophalen van de query groep namen is mislukt.");
                TdLogging.WriteToLogError(TdLogging_Resources.Notification);
                TdLogging.WriteToLogError(ex.Message);
                TdLogging.WriteToLogError(string.Empty);
                TdLogging.WriteToLogError("Controleer het datum formaat van de datum velden.");
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
        /// Save any pending changes in the querygroups.
        /// </summary>
        /// <param name="aDatagridview">The datagridview querygropus.</param>
        public void SaveQueryGroups(DataGridView aDatagridview)
        {
            this.Dt = (DataTable)aDatagridview.DataSource;

            this.changeTableModified = this.Dt.GetChanges(DataRowState.Modified);
            this.changeTableDeleted = this.Dt.GetChanges(DataRowState.Deleted);
            this.changeTableAdded = this.Dt.GetChanges(DataRowState.Added);

            if (this.changeTableModified != null)
            {
                this.da.Update(this.Dt.Select(null, null, DataViewRowState.ModifiedCurrent));
            }

            if (this.changeTableDeleted != null)
            {
                this.da.Update(this.Dt.Select(null, null, DataViewRowState.Deleted));
            }

            if (this.changeTableAdded != null)
            {
                this.da.Update(this.Dt.Select(null, null, DataViewRowState.Added));
            }

            this.LogTableUpdate();
            this.ClearChangeDataTables();
        }

        /// <summary>
        /// See if there are any pending changes in the query groups. Used when the form gets closed.
        /// </summary>
        /// <param name="aDatagridview">The datagridview querygroups.</param>
        /// <returns>true if there are pending changes.</returns>
        public bool CheckForChanges(DataGridView aDatagridview)
        {
            this.Dt = (DataTable)aDatagridview.DataSource;

            DataTable changes = this.Dt.GetChanges();
            if (changes != null && changes.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        private void LogTableUpdate()
        {
            if (this.changeTableDeleted != null)
            {
                if (this.changeTableDeleted.Rows.Count > 0)
                {
                    TdLogging.WriteToLogInformation(string.Format("Er zijn {0} records verwijderd uit de tabel {1}).", this.changeTableDeleted.Rows.Count.ToString(), TdTableName.QUERY_GROUP_LIST));
                }
            }

            if (this.changeTableAdded != null)
            {
                if (this.changeTableAdded.Rows.Count > 0)
                {
                    TdLogging.WriteToLogInformation(string.Format("Er zijn {0} records toegevoegd aan de tabel {1}.", this.changeTableAdded.Rows.Count.ToString(), TdTableName.QUERY_GROUP_LIST));
                }
            }

            if (this.changeTableModified != null)
            {
                if (this.changeTableModified.Rows.Count > 0)
                {
                    TdLogging.WriteToLogInformation(string.Format("Er zijn {0} mutaties doorgevoegd in de tabel {1}.", this.changeTableModified.Rows.Count.ToString(), TdTableName.QUERY_GROUP_LIST));
                }
            }
        }

        private void ClearChangeDataTables()
        {
            if (this.changeTableModified != null)
            {
                this.changeTableModified = null;
            }

            if (this.changeTableDeleted != null)
            {
                this.changeTableDeleted = null;
            }

            if (this.changeTableAdded != null)
            {
                this.changeTableAdded = null;
            }
        }

        /// <summary>
        /// Delete a record from the table: QUERY_GROUP.
        /// </summary>
        /// <param name="oraConId">The Oracle connection id wich is realted to the query group.</param>
        public void DeleteQueryGroupOraCon(int oraConId)
        {
            if (!string.IsNullOrEmpty(this.DatabaseFileName) && !string.IsNullOrEmpty(oraConId.ToString(CultureInfo.InvariantCulture)))
            {
                using SQLiteConnection dbConnection = new("Data Source=" + this.DatabaseFileName);
                dbConnection.Open();

                using var tr = dbConnection.BeginTransaction();
                try
                {
                    string deleteSql = string.Format("DELETE FROM {0} WHERE CONNECTION_ID = @CONNECTION_ID", TdTableName.QUERY_GROUP);

                    using SQLiteCommand command = new(deleteSql, dbConnection);
                    command.Prepare();
                    command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", oraConId));

                    command.ExecuteNonQuery();

                    TdLogging.WriteToLogInformation("De Query groep is Verwijderd uit de QUERY_GROUP_LIST tabel. (CONNECTION_ID : " + oraConId.ToString(CultureInfo.InvariantCulture) + ").");
                }
                catch (SQLiteException ex)
                {
                    TdLogging.WriteToLogError("Het verwijderen van een query groep is mislukt.");
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
        }

        /// <summary>
        /// Get all Querygroup id's and names and put them in the dictionary "Querygroups".
        /// </summary>
        public void GetAllQueryGroupNames()
        {
            string selectSql = string.Format("select id, name from {0} where connection_id = {1} order by name", TdTableName.QUERY_GROUP_LIST, TdUserData.ConnectionId);
            try
            {
                this.DbConnection.Open();
                using SQLiteCommand command = new (selectSql, this.DbConnection);
                using SQLiteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        this.Querygroups.Add(dr.GetInt32(0), dr.GetString(1));
                    }
                    else
                    {
                        TdLogging.WriteToLogError("Geen query groep gevonden.");
                    }
                }
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Het ophalen van alle query groep namen is mislukt.");
                TdLogging.WriteToLogError("Melding :");
                TdLogging.WriteToLogError(ex.Message);
            }
        }

        /// <summary>
        /// Get the filename the query group will save to.
        /// </summary>
        /// <param name="qGroupName">Query group name.</param>
        /// <returns>True if succes.</returns>
        public string GetQueryGroupFileName(string qGroupName)
        {
            string selectSql = string.Format("select EXPORTFILE_PATH, EXPORTFILE_NAME from {0} where NAME = @NAME and CONNECTION_ID = @CONNECTION_ID", TdTableName.QUERY_GROUP_LIST);
            try
            {
                this.DbConnection.Open();
                using SQLiteCommand command = new (selectSql, this.DbConnection);
                command.Parameters.Add(new SQLiteParameter("@NAME", qGroupName));
                command.Parameters.Add(new SQLiteParameter("@CONNECTION_ID", TdUserData.ConnectionId));

                using (SQLiteDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (!dr.IsDBNull(0))
                        {
                            this.Folder = dr.GetString(0);
                            this.FileName = dr.GetString(1);
                        }
                        else
                        {
                            TdLogging.WriteToLogError("Geen query groep export bestandsnaam gevonden. Betreft query groep: " + qGroupName + ".");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this.Folder) && !string.IsNullOrEmpty(this.FileName))
                {
                    return this.Folder + "\\" + this.FileName;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (SQLiteException ex)
            {
                TdLogging.WriteToLogError("Het ophalen van het pad en bestandsnaam van de query group '" + qGroupName + "' is mislukt.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);

                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                return string.Empty;
            }
            finally
            {
                this.DbConnection.Close();
            }
        }
    }
}
