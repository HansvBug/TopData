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
    /// Form specific filter. Adds filter options to the standaard filter.
    /// </summary>
    public partial class FormSpecificFilter : Form
    {
        /// <summary>
        /// Gets or sets the application settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        private readonly FormFilter formFilter;

        /// <summary>
        /// Gets or sets a reference of the main form.
        /// </summary>
        public new FormMain Parent { get; set; }

        /// <summary>
        /// Gets or sets the filter type. Filter type like "begint met, is gelijk aan etc.
        /// </summary>
        public string FilterType { get; set; }

        /// <summary>
        /// A list of items which will be filtered.
        /// </summary>
        private List<string> itemsToFilter = new ();

        /// <summary>
        /// Gets or sets a list of items which will be filtered.
        /// </summary>
        public List<string> ItemsToFilter
        {
            get { return this.itemsToFilter; }
            set { this.itemsToFilter = value; }
        }

        /// <summary>
        /// Gets or sets the column name which is selected.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// gets or sets the filter name.
        /// </summary>
        public string FilterName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSpecificFilter"/> class.
        /// </summary>
        /// <param name="formFilter">The filter form.</param>
        public FormSpecificFilter(FormFilter formFilter)
        {
            this.InitializeComponent();

            this.LoadSettings();

            if (formFilter != null)
            {
                this.formFilter = formFilter;
                this.Parent = formFilter.Parent;
            }
        }

        private void FormSpecificFilter_Load(object sender, EventArgs e)
        {
            this.Text = "Aangepast filter";

            this.LoadWindowPostion();
            this.LoadComboBoxFilterType();
            this.LoadComboBoxFilterItems();
            this.ComboBoxItemsToFilter.Sorted = true;
            this.ActiveControl = this.ComboBoxItemsToFilter;
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new ();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }

        private void LoadWindowPostion()
        {
            using TdFormPosition frmPosition = new(this);
            frmPosition.LoadSpecificFilterFormPosition();
        }

        private void LoadComboBoxFilterItems()
        {
            this.ComboBoxFilterType.Text = this.FilterType;
            foreach (string item in this.itemsToFilter)
            {
                this.ComboBoxItemsToFilter.Items.Add(item);
            }
        }

        private void LoadComboBoxFilterType()
        {
            if (this.FilterName == "Tekstfilters")
            {
                this.ComboBoxFilterType.Items.Add("Is gelijk aan");
                this.ComboBoxFilterType.Items.Add("Is niet gelijk aan");
                this.ComboBoxFilterType.Items.Add("Begint met");
                this.ComboBoxFilterType.Items.Add("Begint niet met");
                this.ComboBoxFilterType.Items.Add("Eindigt met");
                this.ComboBoxFilterType.Items.Add("Eindigt niet met");
                this.ComboBoxFilterType.Items.Add("Bevat");
                this.ComboBoxFilterType.Items.Add("Bevat niet");
            }
            else
            {
                // Getalsfilter
                this.ComboBoxFilterType.Items.Add("Is gelijk aan");
                this.ComboBoxFilterType.Items.Add("Is niet gelijk aan");
                this.ComboBoxFilterType.Items.Add("Groter dan");
                this.ComboBoxFilterType.Items.Add("Is Groter dan of gelijk aan");
                this.ComboBoxFilterType.Items.Add("Kleiner dan");
                this.ComboBoxFilterType.Items.Add("Is Kleiner dan of gelijk aan");

                // comboBoxFilterType.Items.Add("Tussen");  TODO
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.formFilter.IsCanceled = true;
            this.Close();
        }

        private void ComboBoxItemsToFilter_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.ComboBoxItemsToFilter.Text) || !string.IsNullOrEmpty(this.ComboBoxItemsToFilter.Text))
            {
                this.ButtonFilter.Enabled = true;
            }
            else
            {
                this.ButtonFilter.Enabled = false;
            }
        }

        private void ButtonFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ComboBoxFilterType.Text) && !string.IsNullOrEmpty(this.ComboBoxItemsToFilter.Text))
            {
                this.formFilter.FilterItem = this.ComboBoxItemsToFilter.Text;
                this.formFilter.FilterName = this.FilterName; // number or text filter type
                this.formFilter.FilterType = this.ComboBoxFilterType.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Een van de invoervelden is leeg.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (string.IsNullOrEmpty(this.ComboBoxFilterType.Text))
                {
                    this.ActiveControl = this.ComboBoxFilterType;
                }
                else if (string.IsNullOrEmpty(this.ComboBoxItemsToFilter.Text))
                {
                    this.ActiveControl = this.ComboBoxItemsToFilter;
                }
            }
        }

        private void FormSpecificFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveWindowPosition();
            this.SaveSettings();
        }

        private void SaveWindowPosition()
        {
            using TdFormPosition frmPosition = new(this);
            frmPosition.SaveSpecificFilterFormPosition();
        }

        private void SaveSettings()
        {
            TdSettingsManager.SaveSettings(this.JsonObjSettings);
        }
    }
}
