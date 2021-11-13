namespace TopData
{
    using System.Globalization;
    using System.Resources;

    /// <summary>
    /// Log resource manager.
    /// </summary>
    public static class TdLogging_Resources
    {
        private static ResourceManager rmLog;   // Declare Resource manager to access to specific cultureinfo
        private static CultureInfo cul;          // Declare culture info

        /// <summary>
        /// Gets or sets the ResourceManager.
        /// </summary>
        public static ResourceManager RmLog
        {
            get { return rmLog; }
            set { rmLog = value; }
        }

        /// <summary>
        /// Gets or sets Culture info.
        /// </summary>
        public static CultureInfo Cul
        {
            get { return cul; }
            set { cul = value; }
        }

        /// <summary>
        /// Gets string Debug_Logging_On.
        /// </summary>
        public static string Debug_Logging_On
        {
            get { return RmLog.GetString("Debug_Logging_On", Cul); }
        }

        /// <summary>
        /// Gets string AppDbase_Present.
        /// </summary>
        public static string AppDbase_Present
        {
            get { return RmLog.GetString("AppDbase_Present", Cul); }
        }

        /// <summary>
        /// Gets string TheAppDbWillBeCreated.
        /// </summary>
        public static string TheAppDbWillBeCreated
        {
            get { return RmLog.GetString("TheAppDbWillBeCreated", Cul); }
        }

        /// <summary>
        /// Gets string SchemaNameDeleted.
        /// </summary>
        public static string SchemaNameDeleted
        {
            get { return RmLog.GetString("SchemaNameDeleted", Cul); }
        }

        /// <summary>
        /// Gets string DatabaseNameDeleted.
        /// </summary>
        public static string DatabaseNameDeleted
        {
            get { return RmLog.GetString("DatabaseNameDeleted", Cul); }
        }

        /// <summary>
        /// Gets string LookInLogFile.
        /// </summary>
        public static string LookInLogFile
        {
            get { return RmLog.GetString("LookInLogFile", Cul); }
        }

        /// <summary>
        /// Gets string DeleteDbOrSChemaName01.
        /// </summary>
        public static string DeleteDbOrSChemaName01
        {
            get { return RmLog.GetString("DeleteDbOrSChemaName01", Cul); }
        }

        /// <summary>
        /// Gets string DeleteDbOrSChemaName02.
        /// </summary>
        public static string DeleteDbOrSChemaName02
        {
            get { return RmLog.GetString("DeleteDbOrSChemaName02", Cul); }
        }

        /// <summary>
        /// Gets string Notification.
        /// </summary>
        public static string Notification
        {
            get { return RmLog.GetString("Notification", Cul); }
        }

        public static string CreNewUser
        {
            get { return RmLog.GetString("CreNewUser", Cul); }
        }

        public static string UserFullDetailsAreChanged
        {
            get { return RmLog.GetString("UserFullDetailsAreChanged", Cul); }
        }

        public static string UserFullDetailsAreChangedExceptPwd
        {
            get { return RmLog.GetString("UserFullDetailsAreChangedExceptPwd", Cul); }
        }

        public static string UserDetailsNotChanged
        {
            get { return RmLog.GetString("UserDetailsNotChanged", Cul); }
        }

        public static string PwdNotEqualRepeatPwd
        {
            get { return RmLog.GetString("PwdNotEqualRepeatPwd", Cul); }
        }

        public static string DoYouWantToChangeThePwd
        {
            get { return RmLog.GetString("DoYouWantToChangeThePwd", Cul); }
        }

        public static string UserFullDetailsAreNotChangedExceptPwd
        {
            get { return RmLog.GetString("UserFullDetailsAreNotChangedExceptPwd", Cul); }
        }

        public static string PwsOrRepeaaPwdIsMissing
        {
            get { return RmLog.GetString("PwsOrRepeaaPwdIsMissing", Cul); }
        }
        
    }
}
