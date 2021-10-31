namespace TopData
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Form receiving a parameter for a query.
    /// </summary>
    public partial class FormQueryParameter : Form
    {
        #region Properties
        public new TdExecuteQueries Parent { get; set; }

        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the parameter value which wil will be inserted in the query.
        /// </summary>
        public string ParameterValue { get; set; }

        /// <summary>
        /// Gets or sets the application settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="FormQueryParameter"/> class.
        /// Form in which you specify a parameter value.
        /// </summary>
        public FormQueryParameter()
        {
            this.InitializeComponent();

            this.LoadSettings();
            this.LoadFormPosition();
        }

        private void FormQueryParameter_Load(object sender, EventArgs e)
        {
            this.Text = "Parameter...";
            this.LabelParameter.Text = this.ParameterName;
            this.ActiveControl = this.TextBoxParameter;
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }

        private void LoadFormPosition()
        {
            using TdFormPosition formPosition = new(this);
            formPosition.LoadQueryparameterFormPosition();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormQueryParameter_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Parent.ParameterValue = this.ParameterValue;
            this.SaveFormPosition();
            this.SaveSettings();
        }

        private void SaveFormPosition()
        {
            using TdFormPosition formPosition = new(this);
            formPosition.SaveQueryparameterFormPosition();
        }

        private void SaveSettings()
        {
            TdSettingsManager.SaveSettings(this.JsonObjSettings);
        }

        private void TextBoxParameter_TextChanged(object sender, EventArgs e)
        {
            this.ParameterValue = this.TextBoxParameter.Text;
        }
    }
}
