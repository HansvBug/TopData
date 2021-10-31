// Source: https://stackoverflow.com/questions/937298/restoring-window-size-position-with-multiple-monitors

namespace TopData
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Load and Save the postion of a form.
    /// </summary>
    public class TdFormPosition : IDisposable
    {
        private readonly FormMain mainForm;
        private readonly FormConfigure configureForm;
        private readonly FormMaintainOraDbConnections maintainOraDbConnForm;
        private readonly FormMaintainUsers maintainUserDataForm;
        private readonly FormMaintainQueries maintainQueriesForm;
        private readonly FormMaintainQueryGroups queryGroupForm;
        private readonly FormSpecificFilter specificFilterForm;
        private readonly FormQueryParameter queryParameterForm;

        /// <summary>
        /// Gets or sets the appliction settings.
        /// </summary>
        private dynamic JsonObjSettings { get; set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdFormPosition"/> class.
        /// </summary>
        /// <param name="mainForm">a reference to the main form.</param>
        public TdFormPosition(FormMain mainForm)
        {
            this.mainForm = mainForm;
            this.JsonObjSettings = mainForm.JsonObjSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdFormPosition"/> class.
        /// </summary>
        /// <param name="configureForm">a reference to the configure form.</param>
        public TdFormPosition(FormConfigure configureForm)
        {
            this.configureForm = configureForm;
            this.JsonObjSettings = configureForm.JsonObjSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdFormPosition"/> class.
        /// </summary>
        /// <param name="maintainOraDbConnForm">a reference to the maintain oracle database conections form.</param>
        public TdFormPosition(FormMaintainOraDbConnections maintainOraDbConnForm)
        {
            this.maintainOraDbConnForm = maintainOraDbConnForm;
            this.JsonObjSettings = maintainOraDbConnForm.JsonObjSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdFormPosition"/> class.
        /// </summary>
        /// <param name="maintainUserDataForm">A reference to the maintain users form.</param>
        public TdFormPosition(FormMaintainUsers maintainUserDataForm)
        {
            this.maintainUserDataForm = maintainUserDataForm;
            this.JsonObjSettings = maintainUserDataForm.JsonObjSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdFormPosition"/> class.
        /// </summary>
        /// <param name="maintainQueriesForm">A reference to the maintain queries form.</param>
        public TdFormPosition(FormMaintainQueries maintainQueriesForm)
        {
            this.maintainQueriesForm = maintainQueriesForm;
            this.JsonObjSettings = maintainQueriesForm.JsonObjSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdFormPosition"/> class.
        /// </summary>
        /// <param name="queryGroupForm">A reference to the query group form.</param>
        public TdFormPosition(FormMaintainQueryGroups queryGroupForm)
        {
            this.queryGroupForm = queryGroupForm;
            this.JsonObjSettings = queryGroupForm.JsonObjSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdFormPosition"/> class.
        /// </summary>
        /// <param name="specificFilterForm">A reference to the specific filter form.</param>
        public TdFormPosition(FormSpecificFilter specificFilterForm)
        {
            this.specificFilterForm = specificFilterForm;
            this.JsonObjSettings = specificFilterForm.JsonObjSettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TdFormPosition"/> class.
        /// </summary>
        /// <param name="queryParameterForm">a reference to the query parameter form.</param>
        public TdFormPosition(FormQueryParameter queryParameterForm)
        {
            this.queryParameterForm = queryParameterForm;
            this.JsonObjSettings = queryParameterForm.JsonObjSettings;
        }
        #endregion Constructor

        #region Helper
        private static bool IsVisibleOnAnyScreen(Rectangle rect)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(rect))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion Helper

        #region FormMain

        /// <summary>
        /// Load the main form position parameters.
        /// </summary>
        public void LoadMainFormPosition()
        {
            TdLogging.WriteToLogInformation("Ophalen scherm positie hoofdscherm.");

            // Default
            this.mainForm.WindowState = FormWindowState.Normal;
            this.mainForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (this.JsonObjSettings != null && this.JsonObjSettings.FormMain != null)
            {
                Rectangle frmRect = new ()
                {
                    X = this.JsonObjSettings.FormMain[0].FrmX,
                    Y = this.JsonObjSettings.FormMain[0].FrmY,
                    Width = this.JsonObjSettings.FormMain[0].FrmWidth,
                    Height = this.JsonObjSettings.FormMain[0].FrmHeight,
                };

                // Check if the saved bounds are nonzero and visible on any screen
                if (frmRect != Rectangle.Empty && IsVisibleOnAnyScreen(frmRect))
                { // First set the bounds
                    this.mainForm.StartPosition = FormStartPosition.Manual;
                    this.mainForm.DesktopBounds = frmRect;

                    // Afterwards set the window state to the saved value (which could be Maximized)
                    this.mainForm.WindowState = this.JsonObjSettings.FormMain[0].FrmWindowState;
                }
                else
                {
                    // This resets the upper left corner of the window to windows standards
                    this.mainForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    // We can still apply the saved size
                    if (frmRect != Rectangle.Empty)
                    {
                        this.mainForm.Size = frmRect.Size;
                    }
                }
            }
        }

        /// <summary>
        /// Save the main form position parameters.
        /// </summary>
        public void SaveMainFormPosition()
        {
            TdLogging.WriteToLogInformation("Opslaan scherm positie hoofdscherm.");
            string settingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(settingsFile))
            {
                if (this.mainForm.WindowState == FormWindowState.Normal)
                {
                    this.JsonObjSettings.FormMain[0].FrmWindowState = FormWindowState.Normal;

                    if (this.mainForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormMain[0].FrmX = this.mainForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMain[0].FrmX = 0;
                    }

                    if (this.mainForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormMain[0].FrmY = this.mainForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMain[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormMain[0].FrmHeight = this.mainForm.Height;
                    this.JsonObjSettings.FormMain[0].FrmWidth = this.mainForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormMain[0].FrmWindowState = this.mainForm.WindowState;
                }
            }
        }

        /// <summary>
        /// Load the main form spliter parameters.
        /// </summary>
        public void LoadMainFormSplitterPosition()
        {
            try
            {
                int frmMainSplitterMainDistance = this.JsonObjSettings.FormMain[0].FrmMainSplitterMainDistance;
                if (frmMainSplitterMainDistance > 0)
                {
                    this.mainForm.SplitContainer1FormMain.SplitterDistance = frmMainSplitterMainDistance;
                }
                else
                {
                    this.mainForm.SplitContainer1FormMain.SplitterDistance = 200;  // Default
                }

                int frmMainSplitterQueryTreeDistance = this.JsonObjSettings.FormMain[0].FrmMainSplitterQueryTreeDistance;
                if (frmMainSplitterQueryTreeDistance > 0)
                {
                    this.mainForm.SplitContainerQueryTree.SplitterDistance = frmMainSplitterQueryTreeDistance;
                }
                else
                {
                    frmMainSplitterQueryTreeDistance = 315;
                }
            }
            catch (Exception ex)
            {
                TdLogging.WriteToLogError("Fout bij bepalen van de splitter afstand voor het hoofdscherm.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                int frmMainSplitterMainDistance = this.JsonObjSettings.FormMain[0].FrmMainSplitterMainDistance;
                TdLogging.WriteToLogError("FrmMainSplitterMainDistance : " + frmMainSplitterMainDistance.ToString(CultureInfo.InvariantCulture));

                int frmMainSplitterQueryTreeDistance = this.JsonObjSettings.FormMain[0].FrmMainSplitterQueryTreeDistance;
                TdLogging.WriteToLogError("FrmMainSplitterQueryTreeDistance : " + frmMainSplitterQueryTreeDistance.ToString(CultureInfo.InvariantCulture));

                TdLogging.WriteToLogError(string.Empty);
            }
        }

        /// <summary>
        /// Save the main form splitter parameters.
        /// </summary>
        public void SaveMainFormSplitterPosition()
        {
           this.JsonObjSettings.FormMain[0].FrmMainSplitterMainDistance = this.mainForm.SplitContainer1FormMain.SplitterDistance;
           this.JsonObjSettings.FormMain[0].FrmMainSplitterQueryTreeDistance = this.mainForm.SplitContainerQueryTree.SplitterDistance;
        }
        #endregion FormMain

        #region Form Configure

        /// <summary>
        /// Load the configure form position parameters.
        /// </summary>
        public void LoadConfigureFormPosition()
        {
            TdLogging.WriteToLogInformation("Ophalen scherm positie configuratie scherm.");

            // This is the default
            this.configureForm.WindowState = FormWindowState.Normal;
            this.configureForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (this.JsonObjSettings != null && this.JsonObjSettings.FormConfig != null)
            {
                Rectangle frmRect = new ()
                {
                    X = this.JsonObjSettings.FormConfig[0].FrmX,
                    Y = this.JsonObjSettings.FormConfig[0].FrmY,
                    Width = this.JsonObjSettings.FormConfig[0].FrmWidth,
                    Height = this.JsonObjSettings.FormConfig[0].FrmHeight,
                };

                if (frmRect != Rectangle.Empty && IsVisibleOnAnyScreen(frmRect))
                {
                    this.configureForm.StartPosition = FormStartPosition.Manual;
                    this.configureForm.DesktopBounds = frmRect;

                    this.configureForm.WindowState = this.JsonObjSettings.FormConfig[0].FrmWindowState;
                }
                else
                {
                    this.configureForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    if (frmRect != Rectangle.Empty)
                    {
                        this.configureForm.Size = frmRect.Size;
                    }
                }
            }
        }

        /// <summary>
        /// Save the configure form position parameters.
        /// </summary>
        public void SaveConfigureFormPosition()
        {
            TdLogging.WriteToLogInformation("Opslaan scherm positie configuratie scherm.");
            string settingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(settingsFile))
            {
                if (this.configureForm.WindowState == FormWindowState.Normal)
                {
                    this.JsonObjSettings.FormConfig[0].FrmWindowState = FormWindowState.Normal;

                    if (this.configureForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormConfig[0].FrmX = this.configureForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormConfig[0].FrmX = 0;
                    }

                    if (this.configureForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormConfig[0].FrmY = this.configureForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormConfig[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormConfig[0].FrmHeight = this.configureForm.Height;
                    this.JsonObjSettings.FormConfig[0].FrmWidth = this.configureForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormConfig[0].FrmWindowState = this.mainForm.WindowState;
                }
            }
        }
        #endregion Form Configure

        #region Form maintain Oracle database connections

        /// <summary>
        /// Load the maintain Oracle connections form position parameters.
        /// </summary>
        public void LoadMaintainOraDbConnFormPosition()
        {
            TdLogging.WriteToLogInformation("Ophalen scherm positie beheer Oracle database connecties scherm.");
            this.maintainOraDbConnForm.WindowState = FormWindowState.Normal;
            this.maintainOraDbConnForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (this.JsonObjSettings != null && this.JsonObjSettings.FormMain != null)
            {
                Rectangle frmRect = new ()
                {
                    X = this.JsonObjSettings.FormMaintainOraDbConn[0].FrmX,
                    Y = this.JsonObjSettings.FormMaintainOraDbConn[0].FrmY,
                    Width = this.JsonObjSettings.FormMaintainOraDbConn[0].FrmWidth,
                    Height = this.JsonObjSettings.FormMaintainOraDbConn[0].FrmHeight,
                };

                if (frmRect != Rectangle.Empty && IsVisibleOnAnyScreen(frmRect))
                {
                    this.maintainOraDbConnForm.StartPosition = FormStartPosition.Manual;
                    this.maintainOraDbConnForm.DesktopBounds = frmRect;

                    this.maintainOraDbConnForm.WindowState = this.JsonObjSettings.FormMaintainOraDbConn[0].FrmWindowState;
                }
                else
                {
                    this.maintainOraDbConnForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    if (frmRect != Rectangle.Empty)
                    {
                        this.maintainOraDbConnForm.Size = frmRect.Size;
                    }
                }
            }
        }

        /// <summary>
        /// Save the maintain Oracle connections form position parameters.
        /// </summary>
        public void SaveMaintainOraDbConnFormPosition()
        {
            TdLogging.WriteToLogInformation("Opslaan scherm positie configuratie scherm.");
            string settingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(settingsFile))
            {
                if (this.maintainOraDbConnForm.WindowState == FormWindowState.Normal)
                {
                    this.JsonObjSettings.FormMaintainOraDbConn[0].FrmWindowState = FormWindowState.Normal;

                    if (this.maintainOraDbConnForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormMaintainOraDbConn[0].FrmX = this.maintainOraDbConnForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMaintainOraDbConn[0].FrmX = 0;
                    }

                    if (this.maintainOraDbConnForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormMaintainOraDbConn[0].FrmY = this.maintainOraDbConnForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMaintainOraDbConn[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormMaintainOraDbConn[0].FrmHeight = this.maintainOraDbConnForm.Height;
                    this.JsonObjSettings.FormMaintainOraDbConn[0].FrmWidth = this.maintainOraDbConnForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormMaintainOraDbConn[0].FrmWindowState = this.mainForm.WindowState;
                }
            }
        }
        #endregion Form maintain Oracle database connections

        #region Form maintain user data

        /// <summary>
        /// Load the maintain users form position parameters.
        /// </summary>
        public void LoadMaintainUserdataFormPosition()
        {
            TdLogging.WriteToLogInformation("Ophalen scherm positie beheer gebruiker data scherm.");
            this.maintainUserDataForm.WindowState = FormWindowState.Normal;
            this.maintainUserDataForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (this.JsonObjSettings != null && this.JsonObjSettings.FormMain != null)
            {
                Rectangle frmRect = new ()
                {
                    X = this.JsonObjSettings.FormMaintainUsers[0].FrmX,
                    Y = this.JsonObjSettings.FormMaintainUsers[0].FrmY,
                    Width = this.JsonObjSettings.FormMaintainUsers[0].FrmWidth,
                    Height = this.JsonObjSettings.FormMaintainUsers[0].FrmHeight,
                };

                if (frmRect != Rectangle.Empty && IsVisibleOnAnyScreen(frmRect))
                {
                    this.maintainUserDataForm.StartPosition = FormStartPosition.Manual;
                    this.maintainUserDataForm.DesktopBounds = frmRect;

                    this.maintainUserDataForm.WindowState = this.JsonObjSettings.FormMaintainUsers[0].FrmWindowState;
                }
                else
                {
                    this.maintainUserDataForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    if (frmRect != Rectangle.Empty)
                    {
                        this.maintainUserDataForm.Size = frmRect.Size;
                    }
                }
            }
        }

        /// <summary>
        /// Save the maintain users form position parameters.
        /// </summary>
        public void SaveMaintainUserdataFormPosition()
        {
            TdLogging.WriteToLogInformation("Opslaan scherm positie beheer gebruiker data scherm.");

            string settingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(settingsFile))
            {
                if (this.maintainUserDataForm.WindowState == FormWindowState.Normal)
                {
                    this.JsonObjSettings.FormMaintainUsers[0].FrmWindowState = FormWindowState.Normal;

                    if (this.maintainUserDataForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormMaintainUsers[0].FrmX = this.maintainUserDataForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMaintainUsers[0].FrmX = 0;
                    }

                    if (this.maintainUserDataForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormMaintainUsers[0].FrmY = this.maintainUserDataForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMaintainUsers[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormMaintainUsers[0].FrmHeight = this.maintainUserDataForm.Height;
                    this.JsonObjSettings.FormMaintainUsers[0].FrmWidth = this.maintainUserDataForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormMaintainUsers[0].FrmWindowState = this.mainForm.WindowState;
                }
            }
        }
        #endregion Form maintain user data

        #region maintain queries

        /// <summary>
        /// Load the maintain queries form position parameters.
        /// </summary>
        public void LoadMaintainQueriesFormPosition()
        {
            TdLogging.WriteToLogInformation("Ophalen scherm positie Beheer query's scherm.");

            // Default
            this.maintainQueriesForm.WindowState = FormWindowState.Normal;
            this.maintainQueriesForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (this.JsonObjSettings != null && this.JsonObjSettings.FormMaintainQuerysParam != null)
            {
                Rectangle frmRect = new ()
                {
                    X = this.JsonObjSettings.FormMaintainQuerysParam[0].FrmX,
                    Y = this.JsonObjSettings.FormMaintainQuerysParam[0].FrmY,
                    Width = this.JsonObjSettings.FormMaintainQuerysParam[0].FrmWidth,
                    Height = this.JsonObjSettings.FormMaintainQuerysParam[0].FrmHeight,
                };

                // Check if the saved bounds are nonzero and visible on any screen
                if (frmRect != Rectangle.Empty && IsVisibleOnAnyScreen(frmRect))
                { // First set the bounds
                    this.maintainQueriesForm.StartPosition = FormStartPosition.Manual;
                    this.maintainQueriesForm.DesktopBounds = frmRect;

                    // Afterwards set the window state to the saved value (which could be Maximized)
                    this.maintainQueriesForm.WindowState = this.JsonObjSettings.FormMaintainQuerysParam[0].FrmWindowState;
                }
                else
                {
                    // This resets the upper left corner of the window to windows standards
                    this.maintainQueriesForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    // We can still apply the saved size
                    if (frmRect != Rectangle.Empty)
                    {
                        this.maintainQueriesForm.Size = frmRect.Size;
                    }
                }
            }
        }

        /// <summary>
        /// Save the maintain queries form position parameters.
        /// </summary>
        public void SaveMaintainQueriesFormPosition()
        {
            TdLogging.WriteToLogInformation("Opslaan positie Beheer query's scherm.");
            string settingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(settingsFile))
            {
                if (this.maintainQueriesForm.WindowState == FormWindowState.Normal)
                {
                    this.JsonObjSettings.FormMaintainQuerysParam[0].FrmWindowState = FormWindowState.Normal;

                    if (this.maintainQueriesForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormMaintainQuerysParam[0].FrmX = this.maintainQueriesForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMaintainQuerysParam[0].FrmX = 0;
                    }

                    if (this.maintainQueriesForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormMaintainQuerysParam[0].FrmY = this.maintainQueriesForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMaintainQuerysParam[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormMaintainQuerysParam[0].FrmHeight = this.maintainQueriesForm.Height;
                    this.JsonObjSettings.FormMaintainQuerysParam[0].FrmWidth = this.maintainQueriesForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormMaintainQuerysParam[0].FrmWindowState = this.maintainQueriesForm.WindowState;
                }
            }
        }

        /// <summary>
        /// Load the maintain queries form splitter position parameters.
        /// </summary>
        public void LoadMaintainQueriesFormSplitterPosition()
        {
            int frmMainSplitterMainDistance = this.JsonObjSettings.FormMaintainQuerysParam[0].FrmMaintainQueriesSplitContainer1Distance;
            this.maintainQueriesForm.SplitContainer1FormQuery.SplitterDistance = frmMainSplitterMainDistance;

            int frmMainSplitterQueryTreeDistance = this.JsonObjSettings.FormMaintainQuerysParam[0].FrmMaintainQueriesSplitContainer2Distance;
            this.maintainQueriesForm.SplitContainer2FormQuery.SplitterDistance = frmMainSplitterQueryTreeDistance;
        }

        /// <summary>
        /// Save the maintain queries form splitter position parameters.
        /// </summary>
        public void SaveMaintainQueriesFormSplitterPosition()
        {
           this.JsonObjSettings.FormMaintainQuerysParam[0].FrmMaintainQueriesSplitContainer1Distance = this.maintainQueriesForm.SplitContainer1FormQuery.SplitterDistance;
           this.JsonObjSettings.FormMaintainQuerysParam[0].FrmMaintainQueriesSplitContainer2Distance = this.maintainQueriesForm.SplitContainer2FormQuery.SplitterDistance;
        }
        #endregion maintain queries

        #region Querygroup form

        /// <summary>
        /// Load the quergroup form position parameters.
        /// </summary>
        public void LoadQueryGroupFormPosition()
        {
            TdLogging.WriteToLogInformation("Ophalen scherm positie querygroep scherm.");

            this.queryGroupForm.WindowState = FormWindowState.Normal;
            this.queryGroupForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (this.JsonObjSettings != null && this.JsonObjSettings.FormQueryGroupNamesParam != null)
            {
                Rectangle frmRect = new ()
                {
                    X = this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmX,
                    Y = this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmY,
                    Width = this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmWidth,
                    Height = this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmHeight,
                };

                // check if the saved bounds are nonzero and visible on any screen
                if (frmRect != Rectangle.Empty && IsVisibleOnAnyScreen(frmRect))
                { // first set the bounds
                    this.queryGroupForm.StartPosition = FormStartPosition.Manual;
                    this.queryGroupForm.DesktopBounds = frmRect;

                    // afterwards set the window state to the saved value (which could be Maximized)
                    this.queryGroupForm.WindowState = this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmWindowState;
                }
                else
                {
                    // this resets the upper left corner of the window to windows standards
                    this.queryGroupForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    // we can still apply the saved size
                    if (frmRect != Rectangle.Empty)
                    {
                        this.queryGroupForm.Size = frmRect.Size;
                    }
                }
            }
        }

        /// <summary>
        /// Save the quergroup form position parameters.
        /// </summary>
        public void SaveQueryGroupFormPosition()
        {
            TdLogging.WriteToLogInformation("Opslaan positie Beheer querygroep scherm.");
            string settingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(settingsFile))
            {
                if (this.queryGroupForm.WindowState == FormWindowState.Normal)
                {
                    this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmWindowState = FormWindowState.Normal;

                    if (this.queryGroupForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmX = this.queryGroupForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmX = 0;
                    }

                    if (this.queryGroupForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmY = this.queryGroupForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmHeight = this.queryGroupForm.Height;
                    this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmWidth = this.queryGroupForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormQueryGroupNamesParam[0].FrmWindowState = this.queryGroupForm.WindowState;
                }
            }
        }

        #endregion Querygroup form

        #region Form specific filter

        /// <summary>
        /// Load the specific filter form position parameters.
        /// </summary>
        public void LoadSpecificFilterFormPosition()
        {
            TdLogging.WriteToLogInformation("Ophalen scherm positie specifiek filter scherm.");

            // This is the default
            this.specificFilterForm.WindowState = FormWindowState.Normal;
            this.specificFilterForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (this.JsonObjSettings != null && this.JsonObjSettings.FormMain != null)
            {
                Rectangle frmRect = new ()
                {
                    X = this.JsonObjSettings.FormSpecificFilterParam[0].FrmX,
                    Y = this.JsonObjSettings.FormSpecificFilterParam[0].FrmY,
                    Width = this.JsonObjSettings.FormSpecificFilterParam[0].FrmWidth,
                    Height = this.JsonObjSettings.FormSpecificFilterParam[0].FrmHeight,
                };

                if (frmRect != Rectangle.Empty && IsVisibleOnAnyScreen(frmRect))
                {
                    this.specificFilterForm.StartPosition = FormStartPosition.Manual;
                    this.specificFilterForm.DesktopBounds = frmRect;

                    this.specificFilterForm.WindowState = this.JsonObjSettings.FormSpecificFilterParam[0].FrmWindowState;
                }
                else
                {
                    this.specificFilterForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    if (frmRect != Rectangle.Empty)
                    {
                        this.specificFilterForm.Size = frmRect.Size;
                    }
                }
            }
        }

        /// <summary>
        /// Save the specific filter form position parameters.
        /// </summary>
        public void SaveSpecificFilterFormPosition()
        {
            TdLogging.WriteToLogInformation("Opslaan scherm positie specifiek filter scherm.");
            string settingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(settingsFile))
            {
                if (this.specificFilterForm.WindowState == FormWindowState.Normal)
                {
                    this.JsonObjSettings.FormSpecificFilterParam[0].FrmWindowState = FormWindowState.Normal;

                    if (this.specificFilterForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormSpecificFilterParam[0].FrmX = this.specificFilterForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormSpecificFilterParam[0].FrmX = 0;
                    }

                    if (this.specificFilterForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormSpecificFilterParam[0].FrmY = this.specificFilterForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormSpecificFilterParam[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormSpecificFilterParam[0].FrmHeight = this.specificFilterForm.Height;
                    this.JsonObjSettings.FormSpecificFilterParam[0].FrmWidth = this.specificFilterForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormSpecificFilterParam[0].FrmWindowState = this.mainForm.WindowState;
                }
            }
        }
        #endregion Form specific filter

        #region Queryparameter

        /// <summary>
        /// Load the Query parameter form position parameters.
        /// </summary>
        public void LoadQueryparameterFormPosition()
        {
            TdLogging.WriteToLogInformation("Ophalen positie query parameter scherm.");

            // This is the default
            this.queryParameterForm.WindowState = FormWindowState.Normal;
            this.queryParameterForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (this.JsonObjSettings != null && this.JsonObjSettings.FormQueryparameterParam != null)
            {
                Rectangle frmRect = new()
                {
                    X = this.JsonObjSettings.FormQueryparameterParam[0].FrmX,
                    Y = this.JsonObjSettings.FormQueryparameterParam[0].FrmY,
                    Width = this.JsonObjSettings.FormQueryparameterParam[0].FrmWidth,
                    Height = this.JsonObjSettings.FormQueryparameterParam[0].FrmHeight,
                };

                if (frmRect != Rectangle.Empty && IsVisibleOnAnyScreen(frmRect))
                {
                    this.queryParameterForm.StartPosition = FormStartPosition.Manual;
                    this.queryParameterForm.DesktopBounds = frmRect;

                    this.queryParameterForm.WindowState = this.JsonObjSettings.FormQueryparameterParam[0].FrmWindowState;
                }
                else
                {
                    this.queryParameterForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    if (frmRect != Rectangle.Empty)
                    {
                        this.queryParameterForm.Size = frmRect.Size;
                    }
                }
            }
        }

        public void SaveQueryparameterFormPosition()
        {
            TdLogging.WriteToLogInformation("Opslaan positie query parameter scherm.");
            string settingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(settingsFile))
            {
                if (this.queryParameterForm.WindowState == FormWindowState.Normal)
                {
                    this.JsonObjSettings.FormQueryparameterParam[0].FrmWindowState = FormWindowState.Normal;

                    if (this.queryParameterForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormQueryparameterParam[0].FrmX = this.queryParameterForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormQueryparameterParam[0].FrmX = 0;
                    }

                    if (this.queryParameterForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormQueryparameterParam[0].FrmY = this.queryParameterForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormQueryparameterParam[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormQueryparameterParam[0].FrmHeight = this.queryParameterForm.Height;
                    this.JsonObjSettings.FormQueryparameterParam[0].FrmWidth = this.queryParameterForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormQueryparameterParam[0].FrmWindowState = this.queryParameterForm.WindowState;
                }
            }
        }
        #endregion Queryparameter

        #region Dispose

        // Flag: Has Dispose already been called?
        private bool disposed;

        // Instantiate a SafeHandle instance.
        private SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);

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
