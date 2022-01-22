namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Form maintain Query groups.
    /// </summary>
    public partial class FormMaintainQueryGroups : Form
    {
        #region properties

        /// <summary>
        /// Gets or sets the appluication settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        private readonly TdMaintainQueryGroups qGroup;

        private readonly BindingSource bindingSourceDgvTables = new ();

        #endregion properties

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMaintainQueryGroups"/> class.
        /// </summary>
        public FormMaintainQueryGroups()
        {
            this.InitializeComponent();
            this.LoadSettings();
            this.bindingSourceDgvTables = new BindingSource();      // Create an empty new bindingsource for the datagridview.datatable
            this.DataGridViewQgroups.AllowUserToAddRows = false;    // Avoid adding empty new row automatically.

            this.qGroup = new(this.bindingSourceDgvTables); // Create 1 instance
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new ();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }

        #endregion constructor

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Form close
        private void FormMaintainQueryGroups_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.CheckIfClosingIsAllowd(e); // Check for pendng changes. Stop closing when changes not saved.

            TdLogging.WriteToLogInformation("Sluiten Querygroep scherm.");
            this.SaveFormPosition();
            this.SaveSettings();
        }

        private void SaveFormPosition()
        {
            using TdFormPosition frmPos = new(this);
            frmPos.SaveQueryGroupFormPosition();
        }

        private void SaveSettings()
        {
            TdSettingsManager.SaveSettings(this.JsonObjSettings);
        }
        #endregion form close

        #region Form load
        private void FormMaintainQueryGroups_Load(object sender, EventArgs e)
        {
            TdSwitchLanguage sl = new(this, TdCulture.Cul);
            sl.SetLanguageMaintainQueryGroups();

            this.Text = "Query groepen";
            this.LoadFormPosition();

            this.GetAllQueryGroups();
        }

        private void LoadFormPosition()
        {
            using TdFormPosition frmPos = new(this);
            frmPos.LoadQueryGroupFormPosition();
        }
        #endregion Form load

        private void GetAllQueryGroups()
        {
            this.qGroup.GetAllQueryGroups(this.DataGridViewQgroups);
            this.DataGridViewQgroups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;  // After loading the use must size the column width. Otherwise afer evey modification the column resizes.
        }

        #region New QueryGroup

        private void ButtonNewQueryGroup_Click(object sender, EventArgs e)
        {
            this.CreateNewRow();
        }

        private void CreateNewRow()
        {
            // Create an empty new row, keep the data table in MaintainQueryGroups so other methods use the same Datatable.
            TdMaintainQueryGroups tblQueryGoupListMaintenance = new(this.bindingSourceDgvTables)
            {
                Dt = (DataTable)this.DataGridViewQgroups.DataSource,
            };

            int nRowIndex = this.DataGridViewQgroups.Rows.Count - 1;

            if (tblQueryGoupListMaintenance.Dt != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                DataRow newRow = tblQueryGoupListMaintenance.Dt.NewRow();  // Datagrid has a table binding. so add a new row to the table.
                tblQueryGoupListMaintenance.Dt.Rows.Add(newRow);

                int rowCount = tblQueryGoupListMaintenance.Dt.Rows.Count - 1;  // Get the last row

                tblQueryGoupListMaintenance.Dt.Rows[rowCount].SetField("CONNECTION_ID", TdUserData.ConnectionId);
                tblQueryGoupListMaintenance.Dt.Rows[rowCount].SetField("Datum_aamgemaakt", DateTime.Now);
                tblQueryGoupListMaintenance.Dt.Rows[rowCount].SetField("Aangemaakt_door", Environment.UserName);

                Cursor.Current = Cursors.Default;
            }

            // Goto the new row and set the first cell in edit mode
            this.DataGridViewQgroups.ClearSelection();
            this.DataGridViewQgroups.Rows[nRowIndex + 1].Cells[2].Selected = true;
            this.DataGridViewQgroups.FirstDisplayedScrollingRowIndex = nRowIndex + 1;  // In case if you want to scroll down as well.

            // Set the cell in edit mode
            DataGridViewCell cell = this.DataGridViewQgroups.Rows[nRowIndex + 1].Cells[2];
            this.DataGridViewQgroups.CurrentCell = cell;
            this.DataGridViewQgroups.BeginEdit(true);
        }

        /// <summary>
        /// When CTRL+n is pressed in the datagridview a new row will be created.
        /// </summary>
        /// <param name="msg">Windows message.</param>
        /// <param name="keyData">The pressed keys.</param>
        /// <returns>True if CTRL+n else false.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.ActiveControl != null && this.ActiveControl is DataGridView)
            {
                if (keyData == (Keys.Control | Keys.N))
                {
                    this.CreateNewRow();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
        #endregion New QueryGroup

        private void DataGridViewQgroups_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            this.CheckForCellValueLength(e); // check if the value of the cell is not to large

            bool canSave;
            canSave = this.CheckForDuplicateNames();

            // When the querygroup name is not unique disable saving.
            if (canSave)
            {
                this.ButtonSave.Enabled = true;
            }
            else
            {
                this.ButtonSave.Enabled = false;
            }
        }

        private bool CheckForDuplicateNames()
        {
            List<string> querygroupName = new ();
            bool canSave = true;

            foreach (DataGridViewRow row in this.DataGridViewQgroups.Rows)
            {
                querygroupName.Add(row.Cells[2].Value.ToString());
            }

            // Find duplicate values is the list:
            var query = querygroupName.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            // Color the duplcatge values ins the datagridview.
            // First make all values black. And then change the color for the double values.
            foreach (DataGridViewRow row in this.DataGridViewQgroups.Rows)
            {
                this.DataGridViewQgroups[2, row.Index].Style.ForeColor = Color.Black;
                this.DataGridViewQgroups[2, row.Index].Style.Font = new Font(this.DataGridViewQgroups.Font, FontStyle.Regular);
            }

            if (query.Count > 0)
            {
                canSave = false;

                try
                {
                    foreach (string name in query)
                    {
                        foreach (DataGridViewRow row in this.DataGridViewQgroups.Rows)
                        {
                            if (row.Cells[2].Value.ToString().Equals(name))
                            {
                                this.DataGridViewQgroups[2, row.Index].Style.ForeColor = Color.Red;
                                this.DataGridViewQgroups[2, row.Index].Style.Font = new Font(this.DataGridViewQgroups.Font, FontStyle.Bold);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return canSave;
        }

        private void CheckForCellValueLength(DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = this.DataGridViewQgroups.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string celValue = this.DataGridViewQgroups[e.ColumnIndex, e.RowIndex].Value.ToString();

            if (e.ColumnIndex == 2)
            {
                // Check length of Name
                if (this.ValidateStringLength(e, 100))
                {
                    cell.Value = celValue;
                }
                else
                {
                    MessageBox.Show("De naam mag maximaal 100 tekens bevatten.", "Waarschuwing.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cell.Value = celValue.Substring(0, 100);
                }
            }

            // Check length export file path
            if (e.ColumnIndex == 3)
            {
                if (this.ValidateStringLength(e, 1000))
                {
                    cell.Value = celValue;
                }
                else
                {
                    MessageBox.Show("De map naam mag maximaal 1000 tekens bevatten.", "Waarschuwing.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cell.Value = celValue.Substring(0, 1000);
                }
            }

            // Check for file extension
            if (e.ColumnIndex == 4)
            {
                celValue = CheckExtension(this.DataGridViewQgroups[e.ColumnIndex, e.RowIndex].Value.ToString()); // Check for .xlsx

                if (this.ValidateStringLength(e, 50))
                {
                    cell.Value = celValue;
                }
                else
                {
                    MessageBox.Show("De bestandsnaam mag maximaal 50 tekens bevatten.", "Waarschuwing.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cell.Value = celValue.Substring(0, 45) + ".xlsx";
                }
            }
        }

        private bool ValidateStringLength(DataGridViewCellEventArgs e, int maxLength)
        {
            string name = this.DataGridViewQgroups[e.ColumnIndex, e.RowIndex].Value.ToString();
            if (name.Length <= maxLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string CheckExtension(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    // is the same as: if (str.Substring(str.Length - 5).ToUpper() != ".XLSX")
                    if (str[^5..].ToUpper() != ".XLSX")
                    {
                        return str + ".xlsx";
                    }
                }
                catch
                {
                    return str + ".xlsx";  // If string.length <= 4 chars always add .xls to the string
                }
            }

            return str;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            this.qGroup.SaveQueryGroups(this.DataGridViewQgroups);
            this.DataGridViewQgroups.ClearSelection();
        }

        private void DataGridViewQgroups_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 3)
                {
                    using var fbd = new FolderBrowserDialog();
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        DataGridViewCell cell = this.DataGridViewQgroups.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        cell.Value = fbd.SelectedPath;
                    }
                }
            }
        }

        private void CheckIfClosingIsAllowd(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                return;  // In case windows is trying to shut down, don't hold the process up.
            }

            TdMaintainQueryGroups mQg = new ();
            if (mQg.CheckForChanges(this.DataGridViewQgroups))
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Niet alle wijzigingen zijn opgeslagen." + Environment.NewLine + Environment.NewLine +
                    "Doorgaan?",
                    "Let op.",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void DataGridViewQgroups_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null && e.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Opslaan wijzigingen is mislukt.");
                TdLogging.WriteToLogError("Opslaan wijzigingen in de query groepen is mislukt.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(e.Exception.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(e.Exception.ToString());
                }
            }
        }
    }
}
