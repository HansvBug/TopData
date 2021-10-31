namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public class KeyContainer
    {
        private string keyContainerName = "TopData.KeyContainer.01"; // Your.KeyContainer.Name.
        private string defaultFileName = "TopDataKeyContainer.xml";
        private string savedFileName = string.Empty;
        private RSACryptoServiceProvider tdKeyContainer;

        // call these from one of the two button clicks:
        public void GetKeyContainerSaveToFile()
        {
            if (this.GetRSAContainer())
            {
                // save key container to a file
                this.WriteDataToFile(this.tdKeyContainer.ToXmlString(true), this.defaultFileName);
                if (this.savedFileName != string.Empty)
                {
                    MessageBox.Show("Keycontainer opgelagen: " + this.savedFileName, "opgelagen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Display to UI: "CANCELLED! RSA KeyContainer NOT saved to file";
            }
        }

        public void GetXmlCreateKeyContainer()
        {
            if (this.GetRSAContainer())
            {
                // read key container XML from file and save it to the container
                string key = this.ReadDataFromFile(this.defaultFileName);
                if (key != string.Empty)
                {
                    this.tdKeyContainer.FromXmlString(key);

                    // Display to UI: "RSA KeyContainer info retrieved from file " + this.SavedFileName + " and saved to machine KeyContainer";
                }
                else
                {
                    // Display to UI: "CANCELLED! RSA NOT retreived from file and NOT saved to machine";
                }
            }
        }

        // This will get an existing Container, or create a new Container if none exists
        public bool GetRSAContainer()
        {
            try
            {
                // Retrieve the key container
                CspParameters cp = new ();
                cp.KeyContainerName = this.keyContainerName;
                cp.Flags |= CspProviderFlags.UseMachineKeyStore;
                this.tdKeyContainer = new RSACryptoServiceProvider(cp);
                return true;
            }
            catch (Exception ex)
            {
                // Display the Exception to UI
                TdLogging.WriteToLogError("Fout bij het ophalen van de RSA container.");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogError(ex.ToString());
                }

                return false;
            }
        }

        // Write to an XML file:
        private void WriteDataToFile(string data, string fileName)
        {
            // Dit slaat de Key container op als xml bestand. wellicht tijdelijk versleutelen op de huidige manier
            // Configure file dialog box
            SaveFileDialog dlg = new ()
            {
                FileName = fileName,
                DefaultExt = ".xml", // Default file extension
                Filter = "XML documents (.xml)|*.xml", // Filter files by extension
            };

            // Show save file dialog box
            dlg.ShowDialog();

            if (dlg.FileName != string.Empty)
            {
                this.savedFileName = dlg.FileName;
                using StringReader sr = new(data);
                using TextWriter tw = new StreamWriter(dlg.FileName);
                string s = null;
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
                this.savedFileName = string.Empty;
            }
        }

        // Retrieve from the XML file
        private string ReadDataFromFile(string fileName)
        {
            // Configure file dialog box
            OpenFileDialog dlg = new ();
            dlg.FileName = fileName;
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension

            // Show open file dialog box
            dlg.ShowDialog();

            string xml = string.Empty;
            if (dlg.FileName != string.Empty)
            {
                this.savedFileName = dlg.FileName;
                using StreamReader file = new(dlg.FileName);
                xml = file.ReadToEnd();
            }
            else
            {
                this.savedFileName = string.Empty;
            }

            return xml;
        }
    }
}
