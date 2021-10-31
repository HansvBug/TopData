namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;            // Used by Path.
    using System.Management;    // Add reference System.Management
    using System.Reflection;    // Used by Assembly.
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Methods for reading the application environment.
    /// </summary>
    public class TdAppEnvironment : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the application path (location).
        /// </summary>
        public string ApplicationPath { get; set; }

        /// <summary>
        /// Gets or sets the windows logged in user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the machine name.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the windows version.
        /// </summary>
        public string WindowsVersion { get; set; }

        /// <summary>
        /// Gets or sets the nummer of processords.
        /// </summary>
        public string ProcessorCount { get; set; }

        /// <summary>
        /// Gets or sets the processor id.
        /// </summary>
        public string ProcessorId { get; set; }

        /// <summary>
        /// Gets or sets the Bios id.
        /// </summary>
        public string BiosId { get; set; }

        /// <summary>
        /// Gets or sets the total amount of RAM.
        /// </summary>
        public string TotalRam { get; set; }

        /// <summary>
        /// Gets or sets The width of the monitor.
        /// </summary>
        public int MonitorWidth { get; set; }

        /// <summary>
        /// Gets or sets the number of monitors.
        /// </summary>
        public int MonitorCount { get; set; }

        /// <summary>
        /// Gets or sets the .net frame work version.
        /// </summary>
        public List<string> DotNetFrameWorkVersion { get; set; }
        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AppEnvironment"/> class.
        /// Fills the properies.
        /// </summary>
        public TdAppEnvironment()
        {
            this.SetProperties();
        }
        #endregion Constructor

        #region Methods

        /// <summary>
        /// Create a folder at the geiven location.
        /// </summary>
        /// <param name="folderName">The name of folder which needs to be created.</param>
        /// <param name="applicationDataFolder">The %appdata% folder.</param>
        /// <returns>True if success.</returns>
        /// If ApplicationDataFolder = yes then the folder will be created: ...\appdata\roaming\<FolderName>.
        /// If No then the folder will be created in de application directory.
        public bool CreateFolder(string folderName, bool applicationDataFolder)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                return false;
            }

            if (applicationDataFolder)
            {
                string pathString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName);

                if (!Directory.Exists(pathString))
                {
                    try
                    {
                        Directory.CreateDirectory(pathString);
                        return true;
                    }
                    catch (AccessViolationException)
                    {
                        MessageBox.Show("U heeft geen schrijfrechten op de locatie: " + pathString, "Waarschuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                string appDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
                if (!Directory.Exists(appDir))
                {
                    try
                    {
                        Directory.CreateDirectory(appDir);
                        return true;
                    }
                    catch (AccessViolationException)
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        private static string Get_Applicatiepad() // Get the application path
        {
            try
            {
                string appPath;
                appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                appPath += "\\";                                        // add \to the path
                return appPath.Replace("file:\\", string.Empty);  // Remove the text "file:\\" from the path
            }
            catch (ArgumentException aex)
            {
                throw new InvalidOperationException(aex.Message);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Ophalen applicatie pad is mislukt.");
            }
        }

        private static string GetUserName()
        {
            try
            {
                return Environment.UserName;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Ophalen naam gebruiker is mislukt.");
            }
        }

        private static string GetMachineName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Ophalen naam machine is mislukt.");
            }
        }

        private static string GetWindowsVersion(short type)
        {
            try
            {
                string osVersion = string.Empty;

                switch (type)
                {
                    case 1:
                        {
                            osVersion = Environment.OSVersion.ToString();
                            break;
                        }

                    case 2:
                        {
                            osVersion = Convert.ToString(Environment.OSVersion.Version, CultureInfo.InvariantCulture);
                            break;
                        }

                    default:
                        {
                            osVersion = Convert.ToString(Environment.OSVersion.Version, CultureInfo.InvariantCulture);
                            break;
                        }
                }

                return osVersion;
            }
            catch (ArgumentException aex)
            {
                throw new InvalidOperationException(aex.Message);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Ophalen Windows versie is mislukt.");
            }
        }

        private static string GetProcessorCount()
        {
            try
            {
                return Convert.ToString(Environment.ProcessorCount, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Ophalen aantal processors is mislukt.");
            }
        }

        private static string GetProcessorId()
        {
            try
            {
                string result = string.Empty;
                try
                {
                    ManagementObjectSearcher mbs = new("Select ProcessorID From Win32_processor");  // Add reference assemblies: system.management
                    ManagementObjectCollection mbsList = mbs.Get();

                    foreach (ManagementObject mo in mbsList)
                    {
                        result = mo["ProcessorID"].ToString();
                    }

                    mbs.Dispose();

                    return result;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Ophalen id van de processor is mislukt.");
                }
            }
            catch (Exception)
            {
                throw;
            }

            // More (all) options: https://msdn.microsoft.com/en-us/library/aa394373(v=vs.85).aspx
        }

        private static string GetBiosId()
        {
            try
            {
                string bios = string.Empty;
                using (ManagementObjectSearcher searcher = new("SELECT SerialNumber FROM Win32_BIOS"))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        bios = mObject["SerialNumber"].ToString();  // Manufacturer
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(bios))
                {
                    return bios;
                }
                else
                {
                    return "Geen BiosId gevonden.";
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Ophalen id van de Bios is mislukt.");
            }
        }

        private static string GetTotalRam()
        {
            try
            {
                using ManagementClass mc = new("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject item in moc)
                {
                    return Convert.ToString(Math.Round(Convert.ToDouble(item.Properties["TotalPhysicalMemory"].Value, CultureInfo.InvariantCulture) / 1073741824, 2), CultureInfo.InvariantCulture) + " GB";
                }

                return "Geen Totaal telling Ram gevonden.";
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Ophalen totale hoeveelheid RAM is mislukt.");
            }
        }

        private static int GetMonitorWidth()
        {
            try
            {
                var screen = Screen.PrimaryScreen.Bounds;
                return screen.Width;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Bepalen monitorbreedte is mislukt.");
            }
        }

        private static int GetMonitorCount()
        {
            try
            {
                return SystemInformation.MonitorCount;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Bepalen van het aantal monitoren is mislukt.");
            }
        }

        /// <summary>
        /// Get the installed .net version.
        /// </summary>
        /// <returns>A list with all installes .net versions.</returns>
        public static List<string> GetAllDotNetVersions()
        {
            TdGetDotNetVersion netVersion = new ();
            return netVersion.DotNetVersions();
        }

        private void SetProperties()
        {
            this.ApplicationPath = Get_Applicatiepad();
            this.UserName = GetUserName();
            this.MachineName = GetMachineName();
            this.WindowsVersion = GetWindowsVersion(2);
            this.ProcessorCount = GetProcessorCount();
            this.ProcessorId = GetProcessorId();
            this.BiosId = GetBiosId();
            this.TotalRam = GetTotalRam();
            this.MonitorWidth = GetMonitorWidth();
            this.MonitorCount = GetMonitorCount();
            this.DotNetFrameWorkVersion = GetAllDotNetVersions();
        }

        #endregion Methods

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
                this.disposed = true;
            }
        }
        #endregion Dispose
    }
}
