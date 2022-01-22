namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text.Json;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// load and save the appliction settings.
    /// </summary>
    public class TdSettingsManager : IDisposable
    {
        /// <summary>
        /// Gets or sets a reference of the setingsmanager.
        /// </summary>
        public static TdSettingsManager TDSettings { get; set; }

        /// <summary>
        /// Gets or sets a reference of the application settings.
        /// </summary>
        public AppSettingsMeta JsonObjSettings { get; set; }

        private string SettingsFile { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        public TdSettingsManager()
        {
            this.SettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName, TdSettingsDefault.SettingsFolder, TdSettingsDefault.ConfigFile);    // ...\appdata\roaming\<application>\settings\...
        }

        /// <summary>
        /// Allpication setttings meta class.
        /// </summary>
        public class AppSettingsMeta
        {
            /// <summary>
            /// Gets or sets a list with application settings.
            /// </summary>
            public List<AppParams> AppParam { get; set; }

            /// <summary>
            /// Gets or sets the form main settings.
            /// </summary>
            public List<FormMainParams> FormMain { get; set; }

            /// <summary>
            /// Gets or sets the configure form settings.
            /// </summary>
            public List<FormConfigureParams> FormConfig { get; set; }

            /// <summary>
            /// Gets or sets the form maintain Oracle connection settings.
            /// </summary>
            public List<FormMaintainOraDbConnParams> FormMaintainOraDbConn { get; set; }

            /// <summary>
            /// Gets or sets the form maintain user settings.
            /// </summary>
            public List<FormMaintainUsersParams> FormMaintainUsers { get; set; }

            /// <summary>
            /// Gets or sets form maintain query settings.
            /// </summary>
            public List<FormMaintainQuerysParams> FormMaintainQuerysParam { get; set; }

            /// <summary>
            /// Gets or sets the form query group settings.
            /// </summary>
            public List<FormQueryGroupNamesParams> FormQueryGroupNamesParam { get; set; }

            /// <summary>
            /// Gets or sets the export parameter.
            /// </summary>
            public List<ExportParams> ExportParam { get; set; }

            /// <summary>
            /// Gets or sets the specific filter form settings.
            /// </summary>
            public List<FormSpecificFilterParams> FormSpecificFilterParam { get; set; }

            /// <summary>
            /// Gets or sets the Query parameter form settings.
            /// </summary>
            public List<FormQueryparameterParams> FormQueryparameterParam { get; set; }
        }

        /// <summary>
        /// The application parameters/settings.
        /// </summary>
        public class AppParams
        {
            /// <summary>
            /// Gets or sets a value indicating whether logging will be activated.
            /// </summary>
            public bool ActivateLogging { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the log file will be appended.
            /// </summary>
            public bool AppendLogFile { get; set; }

            /// <summary>
            /// Gets or sets the application name.
            /// </summary>
            public string ApplicationName { get; set; }

            /// <summary>
            /// Gets or sets the version.
            /// </summary>
            public string ApplicationVersion { get; set; }

            /// <summary>
            /// Gets or sets the application build date.
            /// </summary>
            public string ApplicationBuildDate { get; set; }

            /// <summary>
            /// Gets or sets the settings file location.
            /// </summary>
            public string SettingsFileLocation { get; set; }

            /// <summary>
            /// Gets or sets log file location.
            /// </summary>
            public string LogFileLocation { get; set; }

            /// <summary>
            /// Gets or sets application database file location.
            /// </summary>
            public string DatabaseLocation { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether Password is equal to schema name.
            /// </summary>
            public bool PasswordIsSchemaName { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the connection name will be build from schema+databasename.
            /// </summary>
            public bool ConstructConnectionName { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the text and comboboxes highlight blue when active.
            /// </summary>
            public bool HighlightTextAndComboBox { get; set; }

            /// <summary>
            /// Gets or sets the nummber of app start ups before copying the appplication database.
            /// </summary>
            public int CopyAppDataBaseAfterEveryXStartups { get; set; }

            /// <summary>
            /// Gets or sets the counter app start ups.
            /// </summary>
            public int CopyAppDataBaseAfterEveryXStartupsCounter { get; set; }

            /// <summary>
            /// Gets or sets Search color.
            /// </summary>
            public int TrvFoundQuerySearchColor { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the user will be warned when a query gets deleted.
            /// </summary>
            public bool WarningOnDeleteQuery { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the query guidelines will shown in a new query.
            /// </summary>
            public bool ShowQueryGuideLines { get; set; }

            /// <summary>
            /// Gets or sets the language.
            /// </summary>
            public string Language { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether comments will been shown. Not used yet.
            /// </summary>
            public bool ShowComments { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the geometry field will show in the datagridview.
            /// </summary>
            public bool ShowGeometryField { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether a datagrid row is highlighted when the mouse over it.
            /// </summary>
            public bool HighLightDataGridOnMouseOver { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the alternate color for a datagridview.
            /// </summary>
            public bool DataGridAlternateRowColor { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the row color of a datagridview.
            /// </summary>
            public bool DataGridNoRowColor { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the highlight color of a datagridview when the mouse moves over a row.
            /// </summary>
            public int HighLightDataGridColor { get; set; }
        }

        /// <summary>
        /// Form main form position parameters.
        /// </summary>
        public class FormMainParams
        {
            /// <summary>
            /// Gets or sets the form X position.
            /// system.drawing.rectangle = 10; 10; 700; 500 ==> x, y, width, height.
            /// </summary>
            public int FrmX { get; set; }

            /// <summary>
            /// Gets or sets the form Y position.
            /// </summary>
            public int FrmY { get; set; }

            /// <summary>
            /// Gets or sets the form height.
            /// </summary>
            public int FrmHeight { get; set; }

            /// <summary>
            /// Gets or sets the form width.
            /// </summary>
            public int FrmWidth { get; set; }

            /// <summary>
            /// Gets or sets the form windowstate.
            /// </summary>
            public FormWindowState FrmWindowState { get; set; }

            /// <summary>
            /// Gets or sets splitter main distance.
            /// </summary>
            public int FrmMainSplitterMainDistance { get; set; }

            /// <summary>
            /// Gets or sets splitter querytree distance.
            /// </summary>
            public int FrmMainSplitterQueryTreeDistance { get; set; }
        }

        /// <summary>
        /// Form configure form position parameters.
        /// </summary>
        public class FormConfigureParams
        {
            /// <summary>
            /// Gets or sets the form X position.
            /// </summary>
            public int FrmX { get; set; } = 20;

            /// <summary>
            /// Gets or sets the form Y position.
            /// </summary>
            public int FrmY { get; set; } = 20;

            /// <summary>
            /// Gets or sets the form height.
            /// </summary>
            public int FrmHeight { get; set; }

            /// <summary>
            /// Gets or sets the form width.
            /// </summary>
            public int FrmWidth { get; set; }

            /// <summary>
            /// Gets or sets the form windowstate.
            /// </summary>
            public FormWindowState FrmWindowState { get; set; }
        }

        /// <summary>
        /// Form configure form maitain Oracle connection parameters.
        /// </summary>
        public class FormMaintainOraDbConnParams
        {
            /// <summary>
            /// Gets or sets the form X position.
            /// </summary>
            public int FrmX { get; set; } = 20;

            /// <summary>
            /// Gets or sets the form Y position.
            /// </summary>
            public int FrmY { get; set; } = 20;

            /// <summary>
            /// Gets or sets the form height.
            /// </summary>
            public int FrmHeight { get; set; }

            /// <summary>
            /// Gets or sets the form width.
            /// </summary>
            public int FrmWidth { get; set; }

            /// <summary>
            /// Gets or sets the form windowstate.
            /// </summary>
            public FormWindowState FrmWindowState { get; set; }
        }

        /// <summary>
        /// Form configure form maintain user parameters.
        /// </summary>
        public class FormMaintainUsersParams
        {
            /// <summary>
            /// Gets or sets the form X position.
            /// </summary>
            public int FrmX { get; set; }

            /// <summary>
            /// Gets or sets the form Y position.
            /// </summary>
            public int FrmY { get; set; }

            /// <summary>
            /// Gets or sets the form height.
            /// </summary>
            public int FrmHeight { get; set; }

            /// <summary>
            /// Gets or sets the form width.
            /// </summary>
            public int FrmWidth { get; set; }

            /// <summary>
            /// Gets or sets the form windowstate.
            /// </summary>
            public FormWindowState FrmWindowState { get; set; }
        }

        /// <summary>
        /// Form configure form maintain query parameters.
        /// </summary>
        public class FormMaintainQuerysParams
        {
            /// <summary>
            /// Gets or sets the form X position.
            /// </summary>
            public int FrmX { get; set; }

            /// <summary>
            /// Gets or sets the form Y position.
            /// </summary>
            public int FrmY { get; set; }

            /// <summary>
            /// Gets or sets the form height.
            /// </summary>
            public int FrmHeight { get; set; }

            /// <summary>
            /// Gets or sets the form width.
            /// </summary>
            public int FrmWidth { get; set; }

            /// <summary>
            /// Gets or sets the form windowstate.
            /// </summary>
            public FormWindowState FrmWindowState { get; set; }

            /// <summary>
            /// Gets or sets the splitter 1 distance.
            /// </summary>
            public int FrmMaintainQueriesSplitContainer1Distance { get; set; }

            /// <summary>
            /// Gets or sets the splitter 2 distance.
            /// </summary>
            public int FrmMaintainQueriesSplitContainer2Distance { get; set; }
        }

        /// <summary>
        /// Form configure form querygroup parameters.
        /// </summary>
        public class FormQueryGroupNamesParams
        {
            /// <summary>
            /// Gets or sets the form X position.
            /// </summary>
            public int FrmX { get; set; }

            /// <summary>
            /// Gets or sets the form Y position.
            /// </summary>
            public int FrmY { get; set; }

            /// <summary>
            /// Gets or sets the form height.
            /// </summary>
            public int FrmHeight { get; set; }

            /// <summary>
            /// Gets or sets the form width.
            /// </summary>
            public int FrmWidth { get; set; }

            /// <summary>
            /// Gets or sets the form windowstate.
            /// </summary>
            public FormWindowState FrmWindowState { get; set; }
        }

        /// <summary>
        /// Form specific filter parameters.
        /// </summary>
        public class FormSpecificFilterParams
        {
            /// <summary>
            /// Gets or sets the form X position.
            /// </summary>
            public int FrmX { get; set; }

            /// <summary>
            /// Gets or sets the form Y position.
            /// </summary>
            public int FrmY { get; set; }

            /// <summary>
            /// Gets or sets the form height.
            /// </summary>
            public int FrmHeight { get; set; }

            /// <summary>
            /// Gets or sets the form width.
            /// </summary>
            public int FrmWidth { get; set; }

            /// <summary>
            /// Gets or sets the form windowstate.
            /// </summary>
            public FormWindowState FrmWindowState { get; set; }
        }

        /// <summary>
        /// Form specific filter parameters.
        /// </summary>
        public class FormQueryparameterParams
        {
            /// <summary>
            /// Gets or sets the form X position.
            /// </summary>
            public int FrmX { get; set; }

            /// <summary>
            /// Gets or sets the form Y position.
            /// </summary>
            public int FrmY { get; set; }

            /// <summary>
            /// Gets or sets the form height.
            /// </summary>
            public int FrmHeight { get; set; }

            /// <summary>
            /// Gets or sets the form width.
            /// </summary>
            public int FrmWidth { get; set; }

            /// <summary>
            /// Gets or sets the form windowstate.
            /// </summary>
            public FormWindowState FrmWindowState { get; set; }
        }

        /// <summary>
        /// Export parameters.
        /// </summary>
        public class ExportParams
        {
            /// <summary>
            /// Gets or sets the CSV seperator.
            /// </summary>
            public string CSVExportChar { get; set; }

            /// <summary>
            /// Gets or sets the default export parameter.
            /// </summary>
            public string DefaultExportFileExtension { get; set; }

            /// <summary>
            /// Gets or sets the text seperator.
            /// </summary>
            public string TextExportChar { get; set; }
        }

        /// <summary>
        /// Load the application settings.
        /// The fist time (else) the is no settings file. Default values will be used.
        /// </summary>
        public void LoadSettings()
        {
            if (File.Exists(this.SettingsFile))
            {
                if (TdLogging_Resources.Cul != null && TdLogging_Resources.RmLog != null)
                {
                    TdLogging.WriteToLogInformation("Ophalen settings.");
                }

                string json = File.ReadAllText(this.SettingsFile);
                this.JsonObjSettings = JsonSerializer.Deserialize<AppSettingsMeta>(json);

                TdVisual.ActivateHighlightEntryBoxes = this.JsonObjSettings.AppParam[0].HighlightTextAndComboBox;
            }
            else
            {
                if (this.JsonObjSettings != null)
                {
                    this.JsonObjSettings.AppParam[0].ActivateLogging = false;
                    this.JsonObjSettings.AppParam[0].AppendLogFile = false;
                    this.JsonObjSettings.AppParam[0].SettingsFileLocation = this.SettingsFile;
                    this.JsonObjSettings.AppParam[0].LogFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName, TdSettingsDefault.SettingsFolder);
                }
            }
        }

        /// <summary>
        /// Save the sttings to the settings file.
        /// </summary>
        /// <param name="jsonObjSettings">Object with all the current settings.</param>
        public static void SaveSettings(dynamic jsonObjSettings)
        {
            if (jsonObjSettings != null)
            {
                // Get settings location
                string fileLocation = jsonObjSettings.AppParam[0].SettingsFileLocation;

                if (string.IsNullOrEmpty(fileLocation))
                {
                    // Defaul location
                    fileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName, TdSettingsDefault.SettingsFolder, TdSettingsDefault.ConfigFile);
                }

                try
                {
                    TdLogging.WriteToLogInformation("Opslaan settings.");
                    var saveOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    };

                    string jsonString = JsonSerializer.Serialize(jsonObjSettings, saveOptions);

                    if (!string.IsNullOrEmpty(fileLocation) && !string.IsNullOrEmpty(jsonString))
                    {
                        File.WriteAllText(fileLocation, jsonString);
                    }
                }
                catch (AccessViolationException ex)
                {
                    TdLogging.WriteToLogError("Fout bij het opslaan van de settings.");
                    TdLogging.WriteToLogError(ex.Message);
                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Create the app. settings file.
        /// </summary>
        public static void CreateSettingsFile()
        {
            var getOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName, TdSettingsDefault.SettingsFolder, TdSettingsDefault.ConfigFile);

            if (!File.Exists(settingsFile))
            {
                TdLogging.WriteToLogInformation("Aanmaken settings bestand.");
                TdLogging.WriteToLogInformation("Locatie settings bestand : " + settingsFile);

                var appSettings = new AppSettingsMeta()
                {
                    AppParam = new List<AppParams>()
                    {
                        new AppParams()
                        {
                            ApplicationName = TdSettingsDefault.ApplicationName,
                            ApplicationVersion = TdSettingsDefault.ApplicationVersion,
                            ApplicationBuildDate = TdSettingsDefault.ApplicationBuildDate,
                            ActivateLogging = true,
                            AppendLogFile = true,
                            SettingsFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName, TdSettingsDefault.SettingsFolder, TdSettingsDefault.ConfigFile),
                            LogFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TdSettingsDefault.ApplicationName, TdSettingsDefault.SettingsFolder),

                            // TODO; make it run un usb: DatabaseLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SettingsDefault.ApplicationName, SettingsDefault.DatabaseFolder),
                            DatabaseLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TdSettingsDefault.DatabaseFolder),  // Database will be stored next to de Program. Not in the roaming folder because everyone uses the same database
                            PasswordIsSchemaName = false,
                            ConstructConnectionName = true,
                            HighlightTextAndComboBox = true,
                            CopyAppDataBaseAfterEveryXStartups = 25,
                            CopyAppDataBaseAfterEveryXStartupsCounter = 0,
                            TrvFoundQuerySearchColor = -10768897,
                            WarningOnDeleteQuery = true,
                            ShowQueryGuideLines = true,
                            Language = "en-US",
                            ShowComments = true, // Not used.
                            ShowGeometryField = false,
                            HighLightDataGridOnMouseOver = false,
                            DataGridAlternateRowColor = false,
                            DataGridNoRowColor = true,
                            HighLightDataGridColor = -8323073,
                        },
                    },
                    FormMain = new List<FormMainParams>()
                    {
                        new FormMainParams()
                        {
                            FrmX = 200,
                            FrmY = 100,
                            FrmHeight = 500,
                            FrmWidth = 900,
                            FrmWindowState = FormWindowState.Normal,
                            FrmMainSplitterMainDistance = 200,
                            FrmMainSplitterQueryTreeDistance = 315,
                        },
                    },
                    FormConfig = new List<FormConfigureParams>()
                    {
                        new FormConfigureParams()
                        {
                            FrmX = 20,
                            FrmY = 20,
                            FrmHeight = 485,
                            FrmWidth = 800,
                            FrmWindowState = FormWindowState.Normal,
                        },
                    },
                    FormMaintainOraDbConn = new List<FormMaintainOraDbConnParams>()
                    {
                        new FormMaintainOraDbConnParams()
                        {
                            FrmX = 20,
                            FrmY = 20,
                            FrmHeight = 270,
                            FrmWidth = 435,
                            FrmWindowState = FormWindowState.Normal,
                        },
                    },
                    FormMaintainUsers = new List<FormMaintainUsersParams>()
                    {
                        new FormMaintainUsersParams()
                        {
                            FrmX = 20,
                            FrmY = 20,
                            FrmHeight = 550,
                            FrmWidth = 834,
                            FrmWindowState = FormWindowState.Normal,
                        },
                    },
                    FormMaintainQuerysParam = new List<FormMaintainQuerysParams>
                    {
                        new FormMaintainQuerysParams()
                        {
                            FrmX = 50,
                            FrmY = 50,
                            FrmHeight = 535,
                            FrmWidth = 1115,
                            FrmMaintainQueriesSplitContainer1Distance = 200,
                            FrmMaintainQueriesSplitContainer2Distance = 300,
                        },
                    },
                    FormQueryGroupNamesParam = new List<FormQueryGroupNamesParams>
                    {
                        new FormQueryGroupNamesParams()
                        {
                            FrmX = 50,
                            FrmY = 50,
                            FrmHeight = 275,
                            FrmWidth = 560,
                            FrmWindowState = FormWindowState.Normal,
                        },
                    },
                    ExportParam = new List<ExportParams>()
                    {
                        new ExportParams()
                        {
                            CSVExportChar = "||",
                            DefaultExportFileExtension = "*.csv",
                            TextExportChar = "|",
                        },
                    },
                    FormSpecificFilterParam = new List<FormSpecificFilterParams>()
                    {
                        new FormSpecificFilterParams()
                        {
                            FrmX = 50,
                            FrmY = 50,
                            FrmHeight = 118,
                            FrmWidth = 650,
                            FrmWindowState = FormWindowState.Normal,
                        },
                    },
                    FormQueryparameterParam = new List<FormQueryparameterParams>()
                    {
                        new FormQueryparameterParams()
                        {
                            FrmX = 50,
                            FrmY = 50,
                            FrmHeight = 125,
                            FrmWidth = 375,
                            FrmWindowState = FormWindowState.Normal,
                        },
                    },
                };

                string jsonString;
                jsonString = JsonSerializer.Serialize(appSettings, getOptions);

                File.WriteAllText(settingsFile, jsonString); // Encoding.Unicode
            }
        }

        #region Dispose

        // Instantiate a SafeHandle instance.
        private readonly SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Flag: Has Dispose already been called?
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
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.safeHandle?.Dispose();

                // Free any other managed objects here.
            }

            this.disposed = true;
        }
        #endregion Dispose
    }
}
