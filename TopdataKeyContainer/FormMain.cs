namespace TopdataKeyContainer
{
    /// <summary>
    /// The main form entry point.
    /// </summary>
    public partial class MainFrm : Form
    {
        // Bron: https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-store-asymmetric-keys-in-a-key-container
        private string KeyContainerName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainFrm"/> class.
        /// </summary>
        public MainFrm()
        {
            this.InitializeComponent();
            this.KeyContainerName = string.Empty;
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonCreateOrRetrieveKeyContainer_Click(object sender, EventArgs e)
        {
            KeyContainer kc = new KeyContainer(this.TextBoxNameKeyContainer.Text);
            if (kc.GetRSAContainer())
            {
                MessageBox.Show("Key container found or created successful.", "Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Key container not found and not created successful.", "Failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         }

        private void ButtonGetKey_Click(object sender, EventArgs e)
        {
            KeyContainer kc = new KeyContainer(this.TextBoxNameKeyContainer.Text);
            this.RichTextBoxKey.Text = kc.GetKeyFromContainer();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.TextBoxNameKeyContainer.Text = "Topdata.KeyContainer"; // Default name key container.
        }

        private void ButtonSaveToXml_Click(object sender, EventArgs e)
        {
            KeyContainer kc = new KeyContainer(this.TextBoxNameKeyContainer.Text);
            this.TextBoxSaved.Text = kc.SaveKeyContainerToFile();
        }

        private void ButtonSaveKeyToCurrentMachine_Click(object sender, EventArgs e)
        {
            KeyContainer kc = new KeyContainer(this.TextBoxNameKeyContainer.Text);
            kc.CreateKeyContainerFromXmlFile();
        }
    }
}