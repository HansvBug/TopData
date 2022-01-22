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
        private static CultureInfo cul;         // Declare culture info

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
        /// Gets the string: Debug logging is on.
        /// </summary>
        public static string Debug_Logging_On
        {
            get { return RmLog.GetString("Debug_Logging_On", Cul); }
        }

        /// <summary>
        /// Gets the string: The application database is present.
        /// </summary>
        public static string AppDbase_Present
        {
            get { return RmLog.GetString("AppDbase_Present", Cul); }
        }

        /// <summary>
        /// Gets the string: The application database is missing and will be created.
        /// </summary>
        public static string TheAppDbWillBeCreated
        {
            get { return RmLog.GetString("TheAppDbWillBeCreated", Cul); }
        }

        /// <summary>
        /// Gets the string: Schema name has been successfully deleted.
        /// </summary>
        public static string SchemaNameDeleted
        {
            get { return RmLog.GetString("SchemaNameDeleted", Cul); }
        }

        /// <summary>
        /// Gets the string: Database name has been successfully deleted.
        /// </summary>
        public static string DatabaseNameDeleted
        {
            get { return RmLog.GetString("DatabaseNameDeleted", Cul); }
        }

        /// <summary>
        /// Gets the string: Look in the log file for information.
        /// </summary>
        public static string LookInLogFile
        {
            get { return RmLog.GetString("LookInLogFile", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to enter schema names in table {0}.
        /// </summary>
        public static string DeleteDbOrSChemaName
        {
            get { return RmLog.GetString("DeleteDbOrSChemaName", Cul); }
        }

        /// <summary>
        /// Gets the string: Notification : .
        /// </summary>
        public static string Notification
        {
            get { return RmLog.GetString("Notification", Cul); }
        }

        /// <summary>
        /// Gets the string: Create new user.
        /// </summary>
        public static string CreNewUser
        {
            get { return RmLog.GetString("CreNewUser", Cul); }
        }

        /// <summary>
        /// Gets the string: The full details of user '{0}' have been changed.
        /// </summary>
        public static string UserFullDetailsAreChanged
        {
            get { return RmLog.GetString("UserFullDetailsAreChanged", Cul); }
        }

        /// <summary>
        /// Gets the string: The details of user '{0}' have been changed (except the password).
        /// </summary>
        public static string UserFullDetailsAreChangedExceptPwd
        {
            get { return RmLog.GetString("UserFullDetailsAreChangedExceptPwd", Cul); }
        }

        /// <summary>
        /// Gets the string: The details of user '{0}' have not changed.
        /// </summary>
        public static string UserDetailsNotChanged
        {
            get { return RmLog.GetString("UserDetailsNotChanged", Cul); }
        }

        /// <summary>
        /// Gets the string: The 'Password' is not the same as 'Repeat password'.
        /// </summary>
        public static string PwdNotEqualRepeatPwd
        {
            get { return RmLog.GetString("PwdNotEqualRepeatPwd", Cul); }
        }

        /// <summary>
        /// Gets the string: Do you want to change the data except the password?.
        /// </summary>
        public static string DoYouWantToChangeThePwd
        {
            get { return RmLog.GetString("DoYouWantToChangeThePwd", Cul); }
        }

        /// <summary>
        /// Gets the string: The details of user '{0}' have not changed. (Password not the same as repeat password).
        /// </summary>
        public static string UserFullDetailsAreNotChangedExceptPwd
        {
            get { return RmLog.GetString("UserFullDetailsAreNotChangedExceptPwd", Cul); }
        }

        /// <summary>
        /// Gets the string: The 'Password' or 'Repeat password' has not been entered.
        /// </summary>
        public static string PwdOrRepeatPwdIsMissing
        {
            get { return RmLog.GetString("PwdOrRepeatPwdIsMissing", Cul); }
        }

        /// <summary>
        /// Gets the string: Information.
        /// </summary>
        public static string WriteInformation
        {
            get { return RmLog.GetString("WriteInformation", Cul); }
        }

        /// <summary>
        /// Gets the string: Check for version of the query database.
        /// </summary>
        public static string CheckForCurVersionAppDb
        {
            get { return RmLog.GetString("CheckForCurVersionAppDb", Cul); }
        }

        /// <summary>
        /// Gets the string: Requesting meta version failed. (Version .
        /// </summary>
        public static string RequestMetaVersionFailed
        {
            get { return RmLog.GetString("RequestMetaVersionFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: The table {0} has been changed. (Version {1}).
        /// </summary>
        public static string TableIsChanged
        {
            get { return RmLog.GetString("TableIsChanged", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to enter database version number in table {0}. (Version .
        /// </summary>
        public static string FailedToInsertDbVersionNumber
        {
            get { return RmLog.GetString("FailedToInsertDbVersionNumber", Cul); }
        }

        /// <summary>
        /// Gets the string: Changing the version in table {0} failed. (Version {1}).
        /// </summary>
        public static string ChangeingMetaVersionFailed
        {
            get { return RmLog.GetString("ChangeingMetaVersionFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: The SQlite user_version has been changed. (Version {0}).
        /// </summary>
        public static string SQLiteUserVersionChanged
        {
            get { return RmLog.GetString("SQLiteUserVersionChanged", Cul); }
        }

        /// <summary>
        /// Gets the string: Changing the SQlite user_version failed. (Version {0}).
        /// </summary>
        public static string ChangeSQLiteUserVersionFailed
        {
            get { return RmLog.GetString("ChangeSQLiteUserVersionFailed", Cul); }
        }

        /// <summary>
        /// gets the string: The database '{0}' has been created.
        /// </summary>
        public static string TheAppDbIsCreated
        {
            get { return RmLog.GetString("TheAppDbIsCreated", Cul); }
        }

        /// <summary>
        /// Gets the string: The database file is present, no new empty database file has been created.
        /// </summary>
        public static string NoNewAppDbCreated
        {
            get { return RmLog.GetString("NoNewAppDbCreated", Cul); }
        }

        /// <summary>
        /// Gets the string: The database '{0}' has not been created.
        /// </summary>
        public static string ErrorCreatingAppDbase
        {
            get { return RmLog.GetString("ErrorCreatingAppDbase", Cul); }
        }

        /// <summary>
        /// Gets the string: The SQlite database was not created because no location or database name was specified.
        /// </summary>
        public static string ErrorCreatingAppDbaseNoName
        {
            get { return RmLog.GetString("ErrorCreatingAppDbaseNoName", Cul); }
        }

        /// <summary>
        /// Gets the string: The table {0} has been created. (Version {1}).
        /// </summary>
        public static string TableIsCreated
        {
            get { return RmLog.GetString("TableIsCreated", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to create table {0}. (Version {1}).
        /// </summary>
        public static string FailedToCreateTable
        {
            get { return RmLog.GetString("FailedToCreateTable", Cul); }
        }

        /// <summary>
        /// Gets the string: The creation of the table {0} has not been performed.
        /// </summary>
        public static string TableIsNotCreated
        {
            get { return RmLog.GetString("TableIsNotCreated", Cul); }
        }

        /// <summary>
        /// Gets the string: The table {0} has been updated. (Version {1}).
        /// </summary>
        public static string TableIsUpdated
        {
            get { return RmLog.GetString("TableIsUpdated", Cul); }
        }

        /// <summary>
        /// Gets the string: Error entering a user in table {0}. (Version {1}).
        /// </summary>
        public static string ErrorEnteringUserInTable
        {
            get { return RmLog.GetString("ErrorEnteringUserInTable", Cul); }
        }

        /// <summary>
        /// Gets the string: The default user {0} was not found.
        /// </summary>
        public static string DefaultUserNotFound
        {
            get { return RmLog.GetString("DefaultUserNotFound", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to determine whether user {0} is unique.
        /// </summary>
        public static string FailedToSeeIfUserIsUnique
        {
            get { return RmLog.GetString("FailedToSeeIfUserIsUnique", Cul); }
        }

        /// <summary>
        /// Gets the string: Deleting database name from table {0} was unsuccessful.
        /// </summary>
        public static string ErrorDeletingDbName
        {
            get { return RmLog.GetString("ErrorDeletingDbName", Cul); }
        }

        /// <summary>
        /// Gets the string: Querying the database names in the table {0} failed.
        /// </summary>
        public static string QueryGetDbnamesFailed
        {
            get { return RmLog.GetString("QueryGetDbnamesFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Retrieving the schema names in the table {0} failed.
        /// </summary>
        public static string QuerGetSchemaNamesFailed
        {
            get { return RmLog.GetString("QuerGetSchemaNamesFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Retrieving the connection names in the table {0} was not successful.
        /// </summary>
        public static string QuerGetConnNamesFailed
        {
            get { return RmLog.GetString("QuerGetConnNamesFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Get the Sqlite database file version.
        /// </summary>
        public static string GetSqlDbVersion
        {
            get { return RmLog.GetString("GetSqlDbVersion", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to query Sqlite database file version.
        /// </summary>
        public static string GetSqlDbVersionFailed
        {
            get { return RmLog.GetString("GetSqlDbVersionFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Get SQLite version.
        /// </summary>
        public static string GetSQLiteVersion
        {
            get { return RmLog.GetString("GetSQLiteVersion", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to query Sqlite version.
        /// </summary>
        public static string GetSQLiteVersionFailed
        {
            get { return RmLog.GetString("GetSQLiteVersionFailed", Cul); }
        }

        /// <summary>
        /// Getes the string: Failed to get query group names.
        /// </summary>
        public static string GetQueryGroupNamesFailed
        {
            get { return RmLog.GetString("GetQueryGroupNamesFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Compressing the application database failed.
        /// </summary>
        public static string CompressAppDbFailed
        {
            get { return RmLog.GetString("CompressAppDbFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Resetting the sequences in the application database failed.
        /// </summary>
        public static string ResetAppDbSequenceFailed
        {
            get { return RmLog.GetString("ResetAppDbSequenceFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Resetting the sequences of table {0} failed.
        /// </summary>
        public static string ResetTableSequenceFailed
        {
            get { return RmLog.GetString("ResetTableSequenceFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: The copy of the file '{0}' is ready.
        /// </summary>
        public static string CopyFileReady
        {
            get { return RmLog.GetString("CopyFileReady", Cul); }
        }

        /// <summary>
        /// Gets the string: The copying of the file '{0}' has been aborted.
        /// </summary>
        public static string CopyFileAborted
        {
            get { return RmLog.GetString("CopyFileAborted", Cul); }
        }

        /// <summary>
        /// Gets the string: The file already exists.
        /// </summary>
        public static string FileExists
        {
            get { return RmLog.GetString("FileExists", Cul); }
        }

        /// <summary>
        /// Gets the string: The file '{0}' to be copied is not present.
        /// </summary>
        public static string FileNotPresent
        {
            get { return RmLog.GetString("FileNotPresent", Cul); }
        }

        /// <summary>
        /// Gets the string: The folder '{0}' is not present.
        /// </summary>
        public static string FolderNotPresent
        {
            get { return RmLog.GetString("FolderNotPresent", Cul); }
        }

        /// <summary>
        /// Gets the string: Changing table {0} failed. (Version {1}).
        /// </summary>
        public static string AlterTableFailed
        {
            get { return RmLog.GetString("AlterTableFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Changing table {0} was not performed.
        /// </summary>
        public static string ChangingTableFailed
        {
            get { return RmLog.GetString("ChangingTableFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to update table {0}. (Version: {1}).
        /// </summary>
        public static string UpdateTableFailed
        {
            get { return RmLog.GetString("UpdateTableFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to get application path.
        /// </summary>
        public static string GetApplicationpathFailed
        {
            get { return RmLog.GetString("GetApplicationpathFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to retrieve user name.
        /// </summary>
        public static string GetUserNameFailed
        {
            get { return RmLog.GetString("GetUserNameFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Machine name retrieval failed.
        /// </summary>
        public static string GetMachineNameFailed
        {
            get { return RmLog.GetString("GetMachineNameFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to get Windows version.
        /// </summary>
        public static string GetWinVersionFailed
        {
            get { return RmLog.GetString("GetWinVersionFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to retrieve the processor ID.
        /// </summary>
        public static string GetProcessorIdFailed
        {
            get { return RmLog.GetString("GetProcessorIdFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: No Bios ID found.
        /// </summary>
        public static string GetBiosIdFailed
        {
            get { return RmLog.GetString("GetBiosIdFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Retrieving the Bios ID failed.
        /// </summary>
        public static string GetBiosIdFailed1
        {
            get { return RmLog.GetString("GetBiosIdFailed1", Cul); }
        }

        /// <summary>
        /// Gets the string: No Total Ram count found.
        /// </summary>
        public static string CountRamTotalFailed
        {
            get { return RmLog.GetString("CountRamTotalFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to get total amount of RAM.
        /// </summary>
        public static string GetTotalRamFailed
        {
            get { return RmLog.GetString("GetTotalRamFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to determine monitor width.
        /// </summary>
        public static string GetMonitorWidthFailed
        {
            get { return RmLog.GetString("GetMonitorWidthFailed", Cul); }
        }

        /// <summary>
        /// Gets the string: Failed to determine the number of monitors.
        /// </summary>
        public static string GetMonitorCountFailed
        {
            get { return RmLog.GetString("GetMonitorCountFailed", Cul); }
        }


        








    }
}
