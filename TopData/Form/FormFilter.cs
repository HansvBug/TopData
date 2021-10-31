namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// The filter form.
    /// </summary>
    public partial class FormFilter : Form
    {
        private readonly List<string> listTreeViewItems = new (); // Create a list from the treeview itmes.

        private readonly List<string> keepSelectedItems = new (); // Create a list with the selected items.

        private readonly TdDatabaseFilter dbFilter = new ();  // Filter reference

        #region  properties

        /// <summary>
        /// Gets or sets a reference to the parent form.
        /// </summary>
        public new FormMain Parent { get; set; }

        /// <summary>
        /// Gets or sets the column name. The column which is selected in de datagridview.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets if the filter form has been openend before.
        /// </summary>
        public int FilterFormHasBeenOpenend { get; set; }

        /// <summary>
        /// Gets or sets the datatype of the in the datagridview selected column.
        /// </summary>
        public string DataTypeSelectedColumn { get; set; }

        /// <summary>
        /// gets or sets the X difference.
        /// </summary>
        public int XDif { get; set; }

        /// <summary>
        /// gets or sets the Y difference.
        /// </summary>
        public int YDif { get; set; }

        /// <summary>
        /// Gets or sets the filter type. Number or text filter.
        /// </summary>
        public string TypeOfFilter { get; set; }

        // Specific filter

        /// <summary>
        /// Gets or sets the filter item.
        /// </summary>
        public string FilterItem { get; set; }

        /// <summary>
        /// gets or sets the filter type. (= , >, =>, etc.).
        /// </summary>
        public string FilterType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the filter is canceled.
        /// </summary>
        public bool IsCanceled { get; set; }

        /// <summary>
        /// gets or sets the filter name. (Number or textfilter).
        /// </summary>
        public string FilterName { get; set; }

        #endregion properties

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormFilter"/> class.
        /// </summary>
        public FormFilter()
        {
            this.InitializeComponent();
        }
        #endregion constructor

        #region form load
        private void FormFilter_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.TreeViewFilter.Sorted = true;
            this.RadioButtonAND.Checked = true;
            this.FilterFormHasBeenOpenend++;

            this.TreeViewFilter.BeginUpdate(); // Suppress repainting the TreeView until all the objects have been created.

            foreach (object item in this.GetItemsDataTable(this.ColumnName))
            {
                TreeNode aNode = new (item.ToString()) { Name = item.ToString() };

                this.TreeViewFilter.Nodes.Add(aNode);

                this.listTreeViewItems.Add(item.ToString());
            }

            this.dbFilter.DataTypeSelectedColumn = this.TypeOfFilter;

            this.TreeViewFilter.EndUpdate();

            if (this.FilterFormHasBeenOpenend == 0)
            {
                this.GroupBoxOperator.Enabled = false;
                this.dbFilter.FilterFormHasBeenOpenend = this.FilterFormHasBeenOpenend;
            }
            else if (this.FilterFormHasBeenOpenend >= 1)
            {
                this.GroupBoxOperator.Enabled = true;
                this.dbFilter.FilterFormHasBeenOpenend = this.FilterFormHasBeenOpenend;
            }
            else
            {
                this.GroupBoxOperator.Enabled = false;
                this.dbFilter.FilterFormHasBeenOpenend = this.FilterFormHasBeenOpenend;
            }

            this.ShowFilterType();
            this.Parent.Filters.DecimalSeperator = this.Parent.DecimalSeperator;  // Set the decimal filter for filtering floats

            TdExtensionMethods.DoubleBuffered(this.TreeViewFilter, true);

            this.FormMinSize();

            Cursor.Current = Cursors.Default;
        }

        private List<string> GetItemsDataTable(string fieldName)
        {
            if (fieldName is null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            var vv = this.Parent.DataGridViewFilterd.Rows.Cast<DataGridViewRow>()
                          .Where(x => !x.IsNewRow)
                          .Select(x => x.Cells[this.ColumnName].Value.ToString())
                          .Distinct()
                          .ToList();

            List<string> distginctFieldItems = vv.ToList();
            return distginctFieldItems;
        }

        private void FormMinSize()
        {
            this.MinimumSize = new Size(195, 280);  // the minimal size of the filter form.
        }

        private void ShowFilterType() // Filter types
        {
            string getalFilter = "Getalfilters";
            string tekstfilter = "Tekstfilters";
            string datumfilter = "Datumfilters";

            switch (this.DataTypeSelectedColumn)
            {
                case "System.String":
                    this.ButtonFiltertype.Text = tekstfilter;
                    this.TypeOfFilter = tekstfilter;
                    this.SetContectMenuItems("Tekst");
                    break;
                case "System.Char":
                    this.ButtonFiltertype.Text = tekstfilter;
                    this.TypeOfFilter = tekstfilter;
                    this.SetContectMenuItems("Tekst");
                    break;
                case "System.Double":
                    this.ButtonFiltertype.Text = getalFilter;
                    this.TypeOfFilter = getalFilter;
                    this.SetContectMenuItems("Getal");
                    break;
                case "System.Decimal":
                    this.ButtonFiltertype.Text = getalFilter;
                    this.TypeOfFilter = getalFilter;
                    this.SetContectMenuItems("Getal");
                    break;
                case "System.Int16":
                    this.ButtonFiltertype.Text = getalFilter;
                    this.TypeOfFilter = getalFilter;
                    this.SetContectMenuItems("Getal");
                    break;
                case "System.Int32":
                    this.ButtonFiltertype.Text = getalFilter;
                    this.TypeOfFilter = getalFilter;
                    this.SetContectMenuItems("Getal");
                    break;
                case "System.Int64":
                    this.ButtonFiltertype.Text = getalFilter;
                    this.TypeOfFilter = getalFilter;
                    this.SetContectMenuItems("Getal");
                    break;
                case "System.UInt16":
                    this.ButtonFiltertype.Text = getalFilter;
                    this.TypeOfFilter = getalFilter;
                    this.SetContectMenuItems("Getal");
                    break;
                case "System.UInt32":
                    this.ButtonFiltertype.Text = getalFilter;
                    this.TypeOfFilter = getalFilter;
                    this.SetContectMenuItems("Getal");
                    break;
                case "System.UInt64":
                    this.ButtonFiltertype.Text = getalFilter;
                    this.TypeOfFilter = getalFilter;
                    this.SetContectMenuItems("Getal");
                    break;

                case "System.DateTime":
                    this.ButtonFiltertype.Text = datumfilter;
                    this.TypeOfFilter = datumfilter;
                    this.SetContectMenuItems("Datum");
                    break;

                default:
                    this.ButtonFiltertype.Visible = false;
                    TdLogging.WriteToLogError("Onbekend datatype aangetroffen.");
                    TdLogging.WriteToLogError("Het datatype is : " + this.DataTypeSelectedColumn);

                    MessageBox.Show(
                        "Onbekend datatype aangetroffen.",
                        "Onbekend datatype.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    this.TypeOfFilter = tekstfilter;
                    this.SetContectMenuItems("Tekst");

                    break;

                    /* Over...
                     Boolean, Byte, SByte, Single, TimeSpan,
                    */
            }
        }

        private void SetContectMenuItems(string typeFilter)
        {
            this.ContextMenuStrip1.Items.Clear();

            if (typeFilter == "Tekst")
            {
                this.ContextMenuStrip1.Items.Add("Is gelijk aan...");
                this.ContextMenuStrip1.Items.Add("Is niet gelijk aan...");
                this.ContextMenuStrip1.Items.Add("-");
                this.ContextMenuStrip1.Items.Add("Begint met...");
                this.ContextMenuStrip1.Items.Add("Eindigt niet met...");
                this.ContextMenuStrip1.Items.Add("-");
                this.ContextMenuStrip1.Items.Add("Bevat...");
                this.ContextMenuStrip1.Items.Add("Bevat niet...");
            }

            if (typeFilter == "Getal")
            {
                this.ContextMenuStrip1.Items.Add("Is gelijk aan...");
                this.ContextMenuStrip1.Items.Add("Is niet gelijk aan...");
                this.ContextMenuStrip1.Items.Add("-");
                this.ContextMenuStrip1.Items.Add("Groter dan...");
                this.ContextMenuStrip1.Items.Add("Is groter dan of gelijk aan...");
                this.ContextMenuStrip1.Items.Add("Kleiner dan...");
                this.ContextMenuStrip1.Items.Add("Is kleiner dan of gelijk aan...");
            }

            if (typeFilter == "Datum")
            {
                this.ContextMenuStrip1.Items.Add("Is gelijk aan...");
                this.ContextMenuStrip1.Items.Add("Is niet gelijk aan...");
                this.ContextMenuStrip1.Items.Add("-");
                this.ContextMenuStrip1.Items.Add("Voor...");
                this.ContextMenuStrip1.Items.Add("Na...");
            }
        }

        #endregion form load

        #region select buttons
        private void ButtonSelectAll_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (TreeNode node in this.TreeViewFilter.Nodes)
            {
                node.Checked = true;
            }

            this.ActivateFilterButton();
            Cursor.Current = Cursors.Default;
        }

        private void ButtonInvertSelection_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.InvertSelection();
            this.StoreSelectedItems();
            this.ActivateFilterButton();
            Cursor.Current = Cursors.Default;
        }

        private void ButtonSelectNone_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (TreeNode node in this.TreeViewFilter.Nodes)
            {
                node.Checked = false;
            }

            this.ActivateFilterButton();
            Cursor.Current = Cursors.Default;
        }

        private void ActivateFilterButton()
        {
            // At least one node must checked to start the filter
            int unCheckedNodes = 0;
            foreach (TreeNode aNode in this.TreeViewFilter.Nodes)
            {
                if (aNode.Checked == false)
                {
                    unCheckedNodes++;
                }
            }

            if (unCheckedNodes == this.TreeViewFilter.Nodes.Count)
            {
                this.ButtonFilter.Enabled = false;
                this.ButtonShowSelected.Enabled = false;
            }
            else
            {
                this.ButtonFilter.Enabled = true;
                this.ButtonShowSelected.Enabled = true;
            }
        }

        private void InvertSelection()
        {
            if (this.TreeViewFilter.Nodes.Count > 0)
            {
                foreach (TreeNode aNode in this.TreeViewFilter.Nodes)
                {
                    if (aNode.Checked == true)
                    {
                        aNode.Checked = false;
                    }
                    else
                    {
                        aNode.Checked = true;
                    }
                }

                this.TreeViewFilter.SelectedNode = this.TreeViewFilter.Nodes[0];   // Force the ListBox to scroll back to the top of the list.
            }
        }

        private void StoreSelectedItems()
        {
            foreach (TreeNode aNode in this.TreeViewFilter.Nodes)
            {
                // At least one node must checked to start the filter.
                if (aNode.Checked == true)
                {
                    this.keepSelectedItems.Add(aNode.Text);
                }
            }
        }

        private void ButtonShowSelected_Click(object sender, EventArgs e)
        {
            // Keep only the selected items in the treeviewfilter
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < this.TreeViewFilter.Nodes.Count; i++)
            {
                this.RemoveUncheckedNodes();
            }

            Cursor.Current = Cursors.Default;
        }

        private void RemoveUncheckedNodes()
        {
            var nodes = new Stack<TreeNode>(this.TreeViewFilter.Nodes.Cast<TreeNode>());
            while (nodes.Count > 0)
            {
                var n = nodes.Pop();
                if (!n.Checked)
                {
                    if (n.Parent != null)
                    {
                        n.Parent.Nodes.Remove(n);
                    }
                    else
                    {
                        this.TreeViewFilter.Nodes.Remove(n);
                    }
                }
                else
                {
                    foreach (TreeNode tn in n.Nodes)
                    {
                        nodes.Push(tn);
                    }
                }
            }
        }

        #endregion select buttons

        #region cancel
        private void ButtonAnnuleer_Click(object sender, EventArgs e)
        {
            if (this.FilterFormHasBeenOpenend >= 1)
            {
                this.FilterFormHasBeenOpenend--;  // If the filter form is openend for the first time and canceld then FilterFormHasBeenOpenend must be 0 to avoid wrong filter building
            }

            if (this.Parent.Filters == null)
            {
                this.Parent.DatatabelExport.DefaultView.RowFilter = string.Empty;
                this.Parent.ButtonRemoveSelection.Visible = false;
                this.Parent.Filters.CheckedFilterItem.Clear();
                this.Parent.FilterFormHasBeenOpenend = 0;
                this.Parent.FilterIsActive = false;
            }

            if (this.FilterFormHasBeenOpenend == 0 && this.Parent.DataTableIsFilterd == false)
            {
                this.Parent.DatatabelExport.DefaultView.RowFilter = string.Empty;
                this.Parent.ButtonRemoveSelection.Visible = false;
                this.Parent.Filters.CheckedFilterItem.Clear();
                this.Parent.FilterFormHasBeenOpenend = 0;
            }

            this.SetParentProperties();

            this.Close();
        }

        private void SetParentProperties()
        {
            if (this.Parent.FilterIsActive)
            {
                this.Parent.FilterFormIsOpenend = false;
                this.Parent.ButtonRemoveSelection.Visible = true;  // Activate remove selection button
                this.Parent.DataTableIsFilterd = true;
            }
            else
            {
                this.Parent.FilterFormIsOpenend = false;
                this.Parent.ButtonRemoveSelection.Visible = false;  // Deactivate remove selection button
                this.Parent.DataTableIsFilterd = false;
            }
        }

        #endregion cancel

        #region Filter
        private void ButtonFilter_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool adObject = false;
            foreach (TreeNode aNode in this.TreeViewFilter.Nodes)
            {
                if (aNode.Checked == true)
                {
                    this.dbFilter.Parent = this.Parent;
                    this.dbFilter.CheckedItems.Add(aNode.Text);

                    this.dbFilter.ColumnName = this.ColumnName;
                    this.dbFilter.FilterFormHasBeenOpenend = this.FilterFormHasBeenOpenend;
                    this.dbFilter.FilterType = " IN ";

                    this.dbFilter.TextOrNumberFilter = this.TypeOfFilter;

                    if (this.FilterFormHasBeenOpenend > 1)
                    {
                        if (this.RadioButtonAND.Checked)
                        {
                            this.dbFilter.Operator = " AND ";
                        }

                        if (this.RadioButtonOR.Checked)
                        {
                            this.dbFilter.Operator = " OR ";
                        }
                    }
                    else
                    {
                        // if Filter form has been opened = 1, the first time then there can't be an operator AND/OR. This can only the second and ... time the form is opend. because then there will be added a column to the filter
                        this.dbFilter.Operator = string.Empty;
                    }

                    adObject = true;
                }
            }

            if (adObject)
            {
                this.Parent.Filters.CheckedFilterItem.Add(this.dbFilter);  // Put the object into the objectlist (column name)
            }

            if (this.Parent.DatatabelExport.Rows.Count > 0)
            {
                if (this.dbFilter.CheckedItems.Count > 0)
                {
                    this.ClearGridSelection();
                    this.Parent.Filters.Parent = this.Parent;
                    this.Parent.Filters.FilterSamenstellen();

                    try
                    {
                        this.Parent.DatatabelExport.DefaultView.RowFilter = this.Parent.Filters.FilterQuery;
                        this.Parent.FilterIsActive = true;
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(
                            "Er is een onverwachte fout opgetreden bij het aanmaken van de filter." + Environment.NewLine +
                            "Kijk in het log bestand voor meer informatie.",
                            "De filter is niet uitgevoerd.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        TdLogging.WriteToLogError("Er is een onverwachte fout opgetreden bij het samenstellen van de filter.");
                        TdLogging.WriteToLogError("Filter : " + this.Parent.Filters.FilterQuery);
                        TdLogging.WriteToLogError("Melding : ");
                        TdLogging.WriteToLogError(ex.Message);

                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug(ex.ToString());
                        }
                    }
                }
                else
                {
                    this.Parent.DatatabelExport.DefaultView.RowFilter = string.Empty;
                }
            }

            this.SetParentProperties();

            Cursor.Current = Cursors.Default;
            this.Close();
        }

        private void ClearGridSelection()
        {
            this.Parent.DatatabelExport.DefaultView.RowFilter = string.Empty;
        }

        #endregion Filter

        private void FormFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.TreeViewFilter.Nodes.Count > 0)
            {
                this.TreeViewFilter.Nodes.Clear();
            }
        }

        private void ContextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Open a form for the filter
            Cursor.Current = Cursors.WaitCursor;
            this.IsCanceled = false;

            FormSpecificFilter filterForm = new (this)
            {
                FilterType = e.ClickedItem.Text.Replace("...", string.Empty),
                FilterName = this.ButtonFiltertype.Text,
            };

            // Fill the combobox with the items of the selected column
            Cursor.Current = Cursors.WaitCursor;

            foreach (object item in this.GetItemsDataTable(this.ColumnName))
            {
                filterForm.ItemsToFilter.Add(item.ToString());
            }

            filterForm.ColumnName = this.ColumnName;  // Pass the clicked column name to the FormSpecificFilter

            Cursor.Current = Cursors.Default;

            this.Visible = false;
            filterForm.ShowDialog(this);   // Show the form

            // Execute the filter
            if (!this.IsCanceled)
            {
                this.ExecuteSpecialFilter();
            }

            filterForm.Dispose();
            this.Visible = true;
        }

        private void ExecuteSpecialFilter()
        {
            this.dbFilter.ColumnName = this.ColumnName;
            this.dbFilter.CheckedItems.Add(this.FilterItem);
            this.dbFilter.FilterType = this.FilterType;

            if (this.FilterName == "Tekstfilters")
            {
                this.dbFilter.TextOrNumberFilter = "Tekstfilters";
            }
            else if (this.FilterName == "Getalfilters")
            {
                this.dbFilter.TextOrNumberFilter = "Getalfilters";
            }
            else if (this.FilterName == "Datumfilters")
            {
                this.dbFilter.TextOrNumberFilter = "Datumfilters";
            }

            if (this.FilterFormHasBeenOpenend > 1)
            {
                if (this.RadioButtonAND.Checked)
                {
                    this.dbFilter.Operator = " AND ";
                }

                if (this.RadioButtonOR.Checked)
                {
                    this.dbFilter.Operator = " OR ";
                }
            }
            else
            {
                this.dbFilter.Operator = string.Empty;
            }

            this.Parent.Filters.CheckedFilterItem.Add(this.dbFilter);  // put the object into the objectlist (column name)

            // build the Filter
            if (this.Parent.DatatabelExport.Rows.Count > 0)
            {
                if (this.dbFilter.CheckedItems.Count > 0)
                {
                    this.ClearGridSelection();

                    this.Parent.Filters.Parent = this.Parent;
                    this.Parent.Filters.FilterSamenstellen();  // Build the actual filter

                    try
                    {
                        // An empty string returns when canceling the aangepaste filter form
                        if (!string.IsNullOrEmpty(this.Parent.Filters.FilterQuery))
                        {
                            this.Parent.DatatabelExport.DefaultView.RowFilter = this.Parent.Filters.FilterQuery;
                            this.Parent.FilterIsActive = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            "Er is een onverwachte fout opgetreden bij het aanmaken van de filter." + Environment.NewLine +
                            "Kijk in het log bestand voor meer informatie.",
                            "Filter is niet uitgevoerd.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        TdLogging.WriteToLogError("Er is een onverwachte fout opgetreden bij het samenstellen van de filter.");
                        TdLogging.WriteToLogError("Filter : " + this.Parent.Filters.FilterQuery);
                        TdLogging.WriteToLogError("Melding : ");
                        TdLogging.WriteToLogError(ex.Message);

                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug(ex.ToString());
                        }
                    }
                }
                else
                {
                    this.Parent.DatatabelExport.DefaultView.RowFilter = string.Empty;
                }
            }

            this.SetParentProperties();

            this.Close();
        }

        private void TreeViewFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.StoreSelectedItems();
            this.ActivateFilterButton(); // Filter button only active when actual items are checked
        }

        private void TreeViewFilter_Click(object sender, EventArgs e)
        {
            this.StoreSelectedItems();
            this.ActivateFilterButton(); // Filter button only active when actual items are checked
        }

        private void TextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            var filteredItems = this.listTreeViewItems.Where(item => item.Contains(this.TextBoxFilter.Text));

            List<string> filterList = filteredItems.ToList();

            this.TreeViewFilter.Nodes.Clear();           // Remove all nodes
            this.TreeViewFilter.BeginUpdate();           // Suppress repainting the TreeView until all the objects have been created.

            // Fill it again with only the filtered items
            foreach (object item in filterList)
            {
                TreeNode aNode = new (item.ToString()) { Name = item.ToString() };
                this.TreeViewFilter.Nodes.Add(aNode);
            }

            this.TreeViewFilter.EndUpdate();

            // Replace the checked items
            List<string> keepSelectedItemsUnique = new ();
            keepSelectedItemsUnique = this.keepSelectedItems.Distinct().ToList();  // Keep only the unique values

            // Keep the items checked when filter starts
            foreach (string keepitem in keepSelectedItemsUnique)
            {
                TreeNode[] arr = this.TreeViewFilter.Nodes.Find(keepitem, true);

                foreach (TreeNode s in arr)
                {
                    this.TreeViewFilter.SelectedNode = s;
                    s.Checked = true;
                }
            }

            // Check if empty then put the original list back
            // park the checked items
            if (string.IsNullOrEmpty(this.TextBoxFilter.Text))
            {
                foreach (string item in this.keepSelectedItems)
                {
                    TreeNode[] arr = this.TreeViewFilter.Nodes.Find(item, true);
                    foreach (TreeNode s in arr)
                    {
                        this.TreeViewFilter.SelectedNode = s;
                        s.Checked = true;
                    }
                }
            }
        }

        private void ButtonFiltertype_Click(object sender, EventArgs e)
        {
            if (this.Parent.DatatabelExport.Rows.Count > 0)
            {
                this.ContextMenuStrip1.Show();
                this.ContextMenuStrip1.Left = this.Left + this.panel1.Location.X + this.ButtonFiltertype.Location.X + this.ButtonFiltertype.Width;
                this.ContextMenuStrip1.Top = this.Top + this.panel1.Location.Y + this.ButtonFiltertype.Location.Y;
            }
            else
            {
                MessageBox.Show("Voer eerst een Query uit.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #region  Make resize borderless form possible

        /// <summary>
        /// Override WndProc.
        /// </summary>
        /// <param name="m">The message.</param>
        protected override void WndProc(ref Message m)
        {
            const uint WM_NCHITTEST = 0x0084;
            const uint WM_MOUSEMOVE = 0x0200;

            const uint HTLEFT = 10;
            const uint HTRIGHT = 11;
            const uint HTBOTTOMRIGHT = 17;
            const uint HTBOTTOM = 15;
            const uint HTBOTTOMLEFT = 16;

            const int RESIZE_HANDLE_SIZE = 10;
            bool handled = false;
            if (m.Msg == WM_NCHITTEST || m.Msg == WM_MOUSEMOVE)
            {
                Size formSize = this.Size;
                Point screenPoint = new (m.LParam.ToInt32());
                Point clientPoint = this.PointToClient(screenPoint);

                Dictionary<uint, Rectangle> boxes = new ()
                {
                    { HTBOTTOMLEFT, new Rectangle(0, formSize.Height - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE) },
                    { HTBOTTOM, new Rectangle(RESIZE_HANDLE_SIZE, formSize.Height - RESIZE_HANDLE_SIZE, formSize.Width - (2 * RESIZE_HANDLE_SIZE), RESIZE_HANDLE_SIZE) },
                    { HTBOTTOMRIGHT, new Rectangle(formSize.Width - RESIZE_HANDLE_SIZE, formSize.Height - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE) },
                    { HTRIGHT, new Rectangle(formSize.Width - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, formSize.Height - (2 * RESIZE_HANDLE_SIZE)) },
                    { HTLEFT, new Rectangle(0, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, formSize.Height - (2 * RESIZE_HANDLE_SIZE)) },
                };

                foreach (KeyValuePair<uint, Rectangle> hitBox in boxes)
                {
                    if (hitBox.Value.Contains(clientPoint))
                    {
                        m.Result = (IntPtr)hitBox.Key;
                        handled = true;
                        break;
                    }
                }
            }

            if (!handled)
            {
                base.WndProc(ref m);
            }
        }

        #endregion Make resize borderless form possible
    }
}
