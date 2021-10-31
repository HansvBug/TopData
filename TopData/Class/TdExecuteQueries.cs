namespace TopData
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;
    using Oracle.ManagedDataAccess.Client;

    // using Oracle.DataAccess.Client;

    /// <summary>
    /// Execute a selected query or a group of selected queries.
    /// </summary>
    public class TdExecuteQueries : IDisposable
    {
        #region properties
        private OracleConnection oraConn;

        /// <summary>
        /// Gets or sets the reference to the parent form.
        /// </summary>
        public FormMain Parent { get; set; }

        /// <summary>
        /// Gets or sets a reference to a datagridview.
        /// </summary>
        public DataGridView Dgv { get; set; }

        /// <summary>
        /// Gets or sets a reference to a treeview node. The query which will be tested.
        /// </summary>
        public TreeNode TrvNode { get; set; }

        /// <summary>
        /// Gets or sets the parameter value in a query text.
        /// </summary>
        public string ParameterValue { get; set; }

        /// <summary>
        /// Gets or sets whether a querys is succesfull executed. When not then it holds the warning.
        /// </summary>
        public string QueryIsExecuted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the SDO geometry column is vissble when 1 query is executed.
        /// </summary>
        public bool SdoGeometryColumnVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the SDO geometry column is vissble when mutiple queries are executed.
        /// </summary>
        public bool SdoGeometryColumnVisibleMultipleQueries { get; set; }

        /// <summary>
        /// Gets or sets the index if the SDO geometry column in the datatabe/dataset.
        /// </summary>
        public int IndexSdoGeometryColumn { get; set; }

        #endregion properties

        #region constrctor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdExecuteQueries"/> class.
        /// </summary>
        /// <param name="oraConn">The oracle connection. (Active conncetion).</param>
        public TdExecuteQueries(OracleConnection oraConn)
        {
            this.oraConn = oraConn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdExecuteQueries"/> class.
        /// </summary>
        public TdExecuteQueries()
        {
            // Default.
        }

        #endregion constrctor

        #region test a query

        /// <summary>
        /// Test a query.
        /// </summary>
        public void TestaQuery()
        {
            TdLogging.WriteToLogInformation("Een Query test wordt uitgevoerd...");
            this.ExecuteQuery();
        }
        #endregion test a query

        /// <summary>
        /// Execute the selected query or queries.
        /// </summary>
        public void StartExecuteQuery()
        {
            Cursor.Current = Cursors.WaitCursor;

            this.ExecuteQuery();

            this.Parent.IndexSdoGeometryColumn = this.IndexSdoGeometryColumn;
            this.Parent.SdoGeometryColumnVisible = this.SdoGeometryColumnVisible;
            this.Parent.SdoGeometryColumnVisibleMultipleQueries = this.SdoGeometryColumnVisibleMultipleQueries;

            Cursor.Current = Cursors.Default;
        }

        private void ExecuteQuery()
        {
            if (this.TrvNode == null)
            {
                return;
            }

            // The last nodes are the query's. Not 100% good, if it is an empty folder then there is no warning
            if (this.TrvNode.LastNode == null || this.TrvNode.Tag is TdQuery)
            {
                if (this.TrvNode.Tag is TdFolder || this.TrvNode.Tag is null)
                {
                    return;
                }

                this.Parent.DataGridViewQueries.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;  // Just to be sure. When the selection mode is headerselect an exception occurs.
                string queryOrg = string.Empty;  // Preserve the original query.
                TdQuery tdQ = (TdQuery)this.TrvNode.Tag;

                try
                {
                    // this.Parent.bn.BindingSource = null;
                    this.Dgv.DataSource = null;
                    this.Parent.ToolStripStatusLabel1.Text = @"De query """ + this.TrvNode.Text + @""" wordt uitgevoerd...";
                    this.Parent.ToolStripStatusLabel2.Text = string.Empty;
                    this.Parent.Refresh();

                    TdLogging.WriteToLogInformation(@"De query """ + this.TrvNode.Text + @""" wordt gestart.");

                    Stopwatch stopWatch = new ();
                    stopWatch.Start();

                    queryOrg = tdQ.Query;  // Preserve the original query so it can be restored

                    // If there is no Query then stop
                    if (!string.IsNullOrEmpty(tdQ.Query))
                    {
                        tdQ.Query = RemoveComments(tdQ.Query); // remove comments. A query can have a parameter
                        tdQ.Query = this.QueryParameter(tdQ.Query); // A query can have a parameter.

                        var sqlqueries = tdQ.Query.Split(new[] { "^" }, StringSplitOptions.RemoveEmptyEntries);  // Split the text

                        OracleCommand oraComm = new ();
                        OracleDataReader dr = null;

                        if (this.oraConn == null)
                        {
                            this.oraConn = this.Parent.OraConn;  // Multiple queries.
                        }

                        oraComm.Connection = this.oraConn;
                        oraComm.CommandType = CommandType.Text;

                        // Remove the comment lines and execute each query.
                        foreach (var query in sqlqueries)
                        {
                            string aQuery = RemoveCommentLinesAttheTop(query);  // Comment or empty lines in front of the query will be removed. (query must start with "select"

                            // Check if query is an SELECT query, if not than assume it's executable.
                            if (aQuery.StartsWith("select", true, System.Globalization.CultureInfo.CurrentCulture))
                            {
                                oraComm.CommandText = aQuery;
                                dr = oraComm.ExecuteReader();
                                dr.Read();
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(aQuery))
                                {
                                    oraComm.CommandText = aQuery;
                                    oraComm.ExecuteNonQuery();
                                }
                            }

                            // TODO; make a method to log the query text in a debugmode state
                        }

                        if (dr != null)
                        {
                            OracleDataAdapter da = new(oraComm);
                            using DataSet ds = new ();
                            da.Fill(ds);
                            da.Dispose();

                            // Create binding
                            BindingSource bs = new ()
                            {
                                DataSource = ds.Tables[0].DefaultView,
                            };

                            // this.Parent.bn.BindingSource = bs;
                            this.Dgv.DataSource = bs;

                            this.Parent.DatatabelExport = ds.Tables[0];  // Required to export the data from the datagridview.
                        }

                        tdQ.Query = queryOrg;  // Put the preserved query back. (need this for the parameters to changed back to #...#)

                        this.CheckForSDOdatatype();  // Check if there is a sdo_geometry field in the query dataset.

                        TimeSpan ts = stopWatch.Elapsed;
                        string elapsedTime = string.Format(
                            CultureInfo.InvariantCulture,
                            "{0:00}:{1:00}:{2:00}.{3:00} (uur:min:sec.msec).",
                            ts.Hours,
                            ts.Minutes,
                            ts.Seconds,
                            ts.Milliseconds / 10);

                        TdLogging.WriteToLogInformation(@"De query """ + this.TrvNode.Text + @""" is gereed.");
                        TdLogging.WriteToLogInformation(@"De query """ + this.TrvNode.Text + @""" duurde : " + elapsedTime);
                        TdLogging.WriteToLogInformation(string.Empty);

                        this.Parent.ToolStripStatusLabel1.Text = "Uitvoeren query is gereed.";
                        this.Parent.Refresh();

                        oraComm.Dispose();
                        if (dr != null)
                        {
                            dr.Dispose();
                        }

                        this.QueryIsExecuted = "Succes"; // Used with "test a Query" in the FormmaintainQueries.
                    }
                    else
                    {
                        MessageBox.Show("De geselecteerde query bevat geen query tekst.", "Waarschuwing.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (OracleException ex)
                {
                    tdQ.Query = queryOrg;
                    this.Dgv.DataSource = null;
                    TdLogging.WriteToLogError("Query kan niet worden uitgevoerd. Betreft query " + this.TrvNode.Text);
                    TdLogging.WriteToLogError("Melding :");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogError(ex.ToString());
                    }

                    this.Parent.ToolStripStatusLabel2.Text = ex.Message;
                    this.Parent.Refresh();
                    this.QueryIsExecuted = ex.Message;  // Show the error in TextBoxOraWarning.text
                }
                catch (InvalidOperationException ex)
                {
                    tdQ.Query = queryOrg;
                    this.Dgv.DataSource = null;
                    TdLogging.WriteToLogError("Er is een onverwachte fout opgetreden bij het uitvoeren van een query. (Query:  " + this.TrvNode.Text + ")");
                    TdLogging.WriteToLogError("Melding :");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogError(ex.ToString());
                    }

                    this.Parent.ToolStripStatusLabel2.Text = ex.Message;
                    this.Parent.Refresh();
                    this.QueryIsExecuted = ex.Message;  // Show the error in TextBoxOraWarning.text
                }
            }
            else
            {
                // A folder is clicked (not an query)
                MessageBox.Show("Selecteer eerst een Query.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Remove the comments from the query text.
        /// </summary>
        /// <param name="query">he query text.</param>
        /// <returns>Query strings without comments.</returns>
        private static string RemoveComments(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return string.Empty;
            }

            int counter = 0;
            string queryString = query;

            while (queryString.IndexOf("/*", StringComparison.InvariantCulture) >= 0)
            {
                int pFrom = queryString.IndexOf("/*", StringComparison.InvariantCulture) - 2;  // If the query starts with /* comment */  then pFrom = -2
                int pTo = queryString.IndexOf("*/", StringComparison.InvariantCulture) + 2;

                if (pTo > pFrom)
                {
                    if (pFrom == -2)
                    {
                        queryString = queryString.Remove(0, pTo - 0);
                    }
                    else
                    {
                        queryString = queryString.Remove(pFrom, pTo - pFrom);
                    }
                }

                counter++;

                if (counter == 100)
                {
                    MessageBox.Show("In de query zit een fout, er is een */  of /* te veel aanwezig.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
            }

            return queryString;
        }

        /// <summary>
        /// Check if the query has a parameter string.
        /// The text #some text# will be replaced with a value from the formQueryparameter.
        /// Replace #...# in the query with a value.
        /// </summary>
        /// <param name="query">The Query text.</param>
        /// <returns>A query string (with paramater values).</returns>
        private string QueryParameter(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return string.Empty;
            }

            try
            {
                // Check for parameters in the query
                string pattern = @"#(.*)#";
                Regex r = new(pattern, RegexOptions.IgnoreCase);  // Instantiate the regular expression object.

                Match m = r.Match(query);                         // Match the regular expression pattern against a text string.
                int counter = 1;

                while (m.Success)
                {
                    FormQueryParameter paramInTake = new ()
                    {
                        Parent = this,
                    };

                    Regex regex = new (@"#(.*)#");

                    var v = regex.Match(query);  // If 2 parameters have the same name then v is the second time empty
                    string s = v.Groups[counter].ToString();
                    paramInTake.Text = "Parameter";
                    paramInTake.ParameterName = s.Replace("_", " ");

                    // Check if the parameter is filled. (In the parameter form).
                    if (!string.IsNullOrEmpty(paramInTake.ParameterName))
                    {
                        paramInTake.ShowDialog();
                        string replacement = "#" + s + "#";         // Query = Regex.Replace(query, replacement, ParameterValue);
                        if (string.IsNullOrEmpty(this.ParameterValue))
                        {
                            query = string.Empty;
                        }
                        else
                        {
                            query = query.Replace(replacement, this.ParameterValue);
                        }
                    }

                    m = m.NextMatch();
                    paramInTake.Dispose();
                }

                return query;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Het opnemen van de parameterwaarde in de query is fout gegaan.", "Fout.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TdLogging.WriteToLogError("Het opnemen van de paramaterwaarde in de query is fout gegaan.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogError(ex.ToString());
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Remove the comment lines in the query text.
        /// </summary>
        /// <param name="query">The query text.</param>
        /// <returns>a query without comments in the text.</returns>
        private static string RemoveCommentLinesAttheTop(string query)
        {
            // A query can't have comment lines at the top because there is a check if the first word =  "select".
            query = Regex.Replace(query, @"^\s+$[\t\r\n]*", string.Empty, RegexOptions.Multiline);  // Remove empty lines
            string aQuery = string.Empty;

            // Remove the comment lines starting with --  (The first line MUST start with select or create).
            string[] array = query.Split("\n"[0]);
            for (int i = 0; i < array.Length; i++)
            {
                if (!array[i].StartsWith("--", true, System.Globalization.CultureInfo.CurrentCulture))
                {
                    aQuery += array[i].ToString(CultureInfo.InvariantCulture) + Environment.NewLine;
                    aQuery = Regex.Replace(aQuery, @"^\s+$[\t\r\n]*", string.Empty, RegexOptions.Multiline);
                }
            }

            return aQuery;
        }

        /// <summary>
        /// Check if the query result contains a sdo_geometry column. This is necessary when the user sets the column to visible or not.
        /// </summary>
        private void CheckForSDOdatatype()
        {
            if (this.Parent.DatatabelExport != null && this.Parent.DatatabelExport.Rows.Count > 0)
            {
                this.IndexSdoGeometryColumn = -1;

                for (int j = 0; j < this.Parent.DatatabelExport.Columns.Count; j++)
                {
                    if (Convert.ToString(this.Parent.DatatabelExport.Columns[j].DataType.Name, CultureInfo.InvariantCulture).ToUpperInvariant() == "SDOGEOMETRY")
                    {
                        // If show geometry is true then pass it to the next function
                        if (this.Parent.SdoGeometryColumnVisible)
                        {
                            this.SdoGeometryColumnVisible = true;
                            this.IndexSdoGeometryColumn = j;
                            this.Parent.IndexSdoGeometryColumn = this.IndexSdoGeometryColumn;
                        }
                        else
                        {
                            this.SdoGeometryColumnVisible = false;
                            this.IndexSdoGeometryColumn = j;
                            this.Parent.IndexSdoGeometryColumn = this.IndexSdoGeometryColumn;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if someone else started the query.
        /// </summary>
        /// <param name="queryId">The id of the started query.</param>
        /// <param name="queryGroup">The query group of the started query.</param>
        /// <param name="queryGuid">The guid of the started query.</param>
        /// <returns>False if the query is not started elsewhere.</returns>
        public static bool IsQueryActive(int queryId, int queryGroup, string queryGuid)
        {
            SQLLiteTopData qActive = new ();
            bool isActive = qActive.IsQueryActive(queryId, queryGroup);
            if (!isActive)
            {
                SQLLiteTopData aQuery = new ();
                aQuery.InsertQueryIsActiveData(queryId, queryGroup, queryGuid);   // Insert the current query_id in query_is_active table.
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Remove query from the query active table.
        /// </summary>
        /// <param name="queryId">Id of the query.</param>
        /// <param name="queryGroup">The query group of the query.</param>
        /// <param name="queryGuid">The guid of the query.</param>
        public void RemoveQueryIsActive(int queryId, int queryGroup, string queryGuid)
        {
            SQLLiteTopData aQuery = new ();
            aQuery.RemoveQueryIsActive(queryId, queryGroup, queryGuid);
        }

        #region Dispose

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

                // Free your own state (unmanaged objects).
                // Set large fields to null.
                this.oraConn = null;

                this.disposed = true;
            }
        }
        #endregion Dispose

    }
}
