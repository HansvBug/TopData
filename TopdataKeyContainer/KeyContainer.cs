namespace TopdataKeyContainer
{
    using System.Security.Cryptography;

    /// <summary>
    /// Manage a key container.
    /// </summary>
    public class KeyContainer
    {
        /// <summary>
        /// Gets or sets the name of the key container.
        /// </summary>
        public string KeyContainerName { get; set; }

        /// <summary>
        /// Gets or sets the default file name when the key container is saved to xml.
        /// </summary>
        public string DefaultFileName { get; set; } // "TopDataKeyContainer.xml";

        /// <summary>
        /// Gets or sets the keycontainer export file name.
        /// </summary>
        public string SavedFileName { get; set; }

        private RSACryptoServiceProvider? keyContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyContainer"/> class.
        /// </summary>
        /// <param name="keyContainerName">The key container name.</param>
        public KeyContainer(string keyContainerName)
        {
            this.KeyContainerName = keyContainerName;
            this.DefaultFileName = string.Empty;
            this.SavedFileName = string.Empty;
        }

        /// <summary>
        /// Get an existing Container, or create a new Container if none exists.
        /// </summary>
        /// <returns> True if key container is created or key container is found.</returns>
        public bool GetRSAContainer()
        {
            try
            {
                // Retrieve the key container
                CspParameters cp = new CspParameters();
                cp.KeyContainerName = this.KeyContainerName;
                cp.Flags |= CspProviderFlags.UseMachineKeyStore;

                this.keyContainer = new RSACryptoServiceProvider(cp);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Key container not created or not found." + "\n" +
                    string.Format("Exception: {}0.", ex.Message),
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// Get the key from stored in the key container.
        /// </summary>
        /// <returns>The key in xml format.</returns>
        public string GetKeyFromContainer()
        {
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = this.KeyContainerName;
            cp.Flags |= CspProviderFlags.UseMachineKeyStore;

            this.keyContainer = new RSACryptoServiceProvider(cp);
            return this.keyContainer.ToXmlString(true);
        }

        /// <summary>
        /// Get the key inside the key container.
        /// </summary>
        /// <returns>The file name of the created xml file with the key container data.</returns>
        public string SaveKeyContainerToFile()
        {
            if (this.GetRSAContainer())
            {
                if (this.keyContainer != null)
                {
                    this.WriteDataToFile(this.keyContainer.ToXmlString(true), this.DefaultFileName); // Save key container to a file
                    if (this.SavedFileName != string.Empty)
                    {
                        MessageBox.Show("Saved key container to xml file.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return this.SavedFileName;
                    }
                    else
                    {
                        return "The xml key container file is not saved.";
                    }
                }
                else
                {
                    return "The xml key container file is not saved. The key container was not found. (Null value).";
                }
            }
            else
            {
                return "The xml key container file is not saved. The key container was not found.";
            }
        }

        /// <summary>
        /// Save key container as a xml file.
        /// </summary>
        /// <param name="data">The key container data.</param>
        /// <param name="fileName">File name of the saved key container data (xml file).</param>
        private void WriteDataToFile(string data, string fileName)
        {
            SaveFileDialog dlg = new();
            dlg.FileName = fileName;
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            dlg.ShowDialog();

            if (dlg.FileName != string.Empty)
            {
                this.SavedFileName = dlg.FileName;
                using StringReader sr = new(data);
                using TextWriter tw = new StreamWriter(dlg.FileName);
                var s = string.Empty;
                while (true)
                {
                    s = sr.ReadLine();
                    if (s != null)
                    {
                        tw.WriteLine(s);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                this.SavedFileName = string.Empty;
            }
        }

        /// <summary>
        /// Store the key container xml to the current machine.
        /// </summary>
        public void CreateKeyContainerFromXmlFile()
        {
            if (this.GetRSAContainer())
            {
                // read key container XML from file and save it to the container
                string key = this.ReadDataFromFile(this.DefaultFileName);
                if (key != string.Empty)
                {
                    if (this.keyContainer != null)
                    {
                        this.keyContainer.FromXmlString(key);
                        MessageBox.Show("Xml is stored in key container.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Xml is not stored in key container.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Xml is not stored in key container.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string ReadDataFromFile(string fileName)
        {
            OpenFileDialog dlg = new();
            dlg.FileName = fileName;
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            dlg.ShowDialog();

            string xml = string.Empty;
            if (dlg.FileName != string.Empty)
            {
                this.SavedFileName = dlg.FileName;
                using StreamReader file = new(dlg.FileName);
                xml = file.ReadToEnd();
            }
            else
            {
                this.SavedFileName = string.Empty;
            }

            return xml;
        }
    }
}
