namespace TopData
{
    using System;
    using System.Globalization;

    /// <summary>
    /// The default settings.
    /// </summary>
    public static class TdSettingsDefault
    {
        /// <summary>
        /// The name of the application.
        /// </summary>
        public const string ApplicationName = "TopData";

        /// <summary>
        /// The current application vesion.
        /// </summary>
        public const string ApplicationVersion = "1.3.0.0";

        /// <summary>
        /// Gets the current application build date.
        /// </summary>
        public static string ApplicationBuildDate
        {
            get { return DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// System menu consatant, current year.
        /// </summary>
        public const string SystemMenu = "2021";        // For the systemmenu line

        /// <summary>
        /// System menu constant Copyricht years.
        /// </summary>
        public const string Copyright = "2019-2021";    // Started february 2019

        /// <summary>
        /// Curent database version.
        /// </summary>
        public const int UpdateVersion = 4;

        /// <summary>
        /// Log file name.
        /// </summary>
        public const string LogFileName = "TopData.log";

        /// <summary>
        /// Config filename.
        /// </summary>
        public const string ConfigFile = "TopData.cfg";

        /// <summary>
        /// Settings folder name.
        /// </summary>
        public const string SettingsFolder = "Settings\\";

        /// <summary>
        /// Database folder name.
        /// </summary>
        public const string DatabaseFolder = "Database\\";

        /// <summary>
        /// back-up folder name.
        /// </summary>
        public const string BackUpFolder = "BackUp\\";

        /// <summary>
        /// temporary export folder name.
        /// </summary>
        public const string ExportTempFolder = "Export_Tmp\\";

        /// <summary>
        /// SQlite database file name.
        /// </summary>
        public const string SqlLiteDatabaseName = "TopData.db";

        // not good...

        /// <summary>
        /// Encryption key 1.
        /// </summary>
        public const string StringSleutel = "$fvs-sdo(fwa|fk(d)as(9dvQ=+<P[gth<}]f" + AStringSleutel;

        /// <summary>
        /// Encryption key 2.
        /// </summary>
        public const string StringSleutel1 = "$fvs-sdo1(fwa|fk(d)as1(9dvQ=+<P[gth<}]f" + AStringSleutel1;

        /// <summary>
        /// Encryption key 3.
        /// </summary>
        public const string AStringSleutel = "gt#vt&wss^cvwsr)0ingsrgKey#tgrj+suci@asd?/vdk";

        /// <summary>
        /// Encryption key 4.
        /// </summary>
        public const string AStringSleutel1 = "gt#v1t&wss^cvwsr)0ingsrgKey#tgrj+1suci@asd?/vdk";
    }
}
