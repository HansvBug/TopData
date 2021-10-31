namespace TopData
{
    using System.Data.SQLite;
    using System.IO;

    /// <summary>
    /// SQliteDatabaseConnection is a base class which creates and holds the SQLite database connection.
    /// </summary>
    public class TdSQliteDatabaseConnection
    {
        /// <summary>
        /// Gets or sets the SQlite connection.
        /// </summary>
        protected SQLiteConnection DbConnection { get; set; }

        /// <summary>
        /// Gets or sets the name of the database file.
        /// </summary>
        protected string DatabaseFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the database file location.
        /// </summary>
        protected string DbLocation { get; set; }

        /// <summary>
        /// Gets or sets the appliction settings.
        /// </summary>
        private dynamic JsonObjSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQliteDatabaseConnection"/> class.
        /// Create the database connection.
        /// </summary>
        public TdSQliteDatabaseConnection()
        {
            this.LoadSettings();
            try
            {
                if (string.IsNullOrEmpty(this.DbLocation))
                {
                    this.DbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
                    this.DatabaseFileName = Path.Combine(this.DbLocation, TdSettingsDefault.SqlLiteDatabaseName);
                    this.DbConnection = new SQLiteConnection("Data Source=" + this.DatabaseFileName);
                }
            }
            catch
            {
                // empty. gets here when the app is started for the first time and there is no Settings file.
            }
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new ();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }
    }
}
