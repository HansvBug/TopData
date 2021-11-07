namespace TopData
{
    using System.Globalization;
    using System.Resources;

    /// <summary>
    /// Messagebox resources.
    /// </summary>
    public static class MB_Title
    {
        /// <summary>
        /// Declare Resource manager to access to specific cultureinfo.
        /// </summary>
        private static ResourceManager rmMb;

        /// <summary>
        ///  Declare culture info.
        /// </summary>
        private static CultureInfo cul;      // Declare culture info

        /// <summary>
        /// Gets or sets the ResourceManager.
        /// </summary>
        public static ResourceManager RmMb
        {
            get { return rmMb; }
            set { rmMb = value; }
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
        /// Gets the Error string.
        /// </summary>
        public static string Error
        {
            get { return RmMb.GetString("Title_Error", Cul); }
        }

        /// <summary>
        /// Gets the Information string.
        /// </summary>
        public static string Information
        {
            get { return RmMb.GetString("Title_Information", Cul); }
        }

        /// <summary>
        /// Gets the Warning string.
        /// </summary>
        public static string Warning
        {
            get { return RmMb.GetString("Title_Warning", Cul); }
        }

        /// <summary>
        /// Gets the Attention string.
        /// </summary>
        public static string Attention
        {
            get { return RmMb.GetString("Title_Attention", Cul); }
        }

        /// <summary>
        /// Gets the Continue string.
        /// </summary>
        public static string Continue
        {
            get { return RmMb.GetString("Title_Continue", Cul); }
        }

        /// <summary>
        /// Gets the File_Copy string.
        /// </summary>
        public static string File_Copy
        {
            get { return RmMb.GetString("Titlte_File_Copy", Cul); }
        }

        /// <summary>
        /// Gets the Create_New_User string.
        /// </summary>
        public static string Create_New_User
        {
            get { return RmMb.GetString("Tilte_Create_New_User", Cul); }
        }

        /// <summary>
        /// Gets the Save string.
        /// </summary>
        public static string Save
        {
            get { return RmMb.GetString("Title_Save", Cul); }
        }

        /// <summary>
        /// Gets the Drag_Query string.
        /// </summary>
        public static string Drag_Query
        {
            get { return RmMb.GetString("Title_Drag_Query", Cul); }
        }

        /// <summary>
        /// Gets the Cre_New_DB_Connection string.
        /// </summary>
        public static string Cre_New_DB_Connection
        {
            get { return RmMb.GetString("Title_Cre_New_DB_Connection", Cul); }
        }

        /// <summary>
        /// Gets the Change_data string.
        /// </summary>
        public static string Change_data
        {
            get { return RmMb.GetString("Title_Change_data", Cul); }
        }

        /// <summary>
        /// Gets the Ready string.
        /// </summary>
        public static string Ready
        {
            get { return RmMb.GetString("Title_Ready", Cul); }
        }

        /// <summary>
        /// Gets the Remove string.
        /// </summary>
        public static string Remove
        {
            get { return RmMb.GetString("Title_Remove", Cul); }
        }
    }

    public static class MB_Text
    {
        /// <summary>
        /// Declare Resource manager to access to specific cultureinfo.
        /// </summary>
        private static ResourceManager rmMb;

        /// <summary>
        ///  Declare culture info.
        /// </summary>
        private static CultureInfo cul;      // Declare culture info

        /// <summary>
        /// Gets or sets the ResourceManager.
        /// </summary>
        public static ResourceManager RmMb
        {
            get { return rmMb; }
            set { rmMb = value; }
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
        /// Gets the App_Database_Created string.
        /// </summary>
        public static string App_Database_Created => RmMb.GetString("Text_App_Database_Created", Cul);

        /// <summary>
        /// Gets the App_Database_Copy_Failed string.
        /// </summary>
        public static string App_Database_Copy_Failed => RmMb.GetString("Text_App_Database_Copy_Failed", Cul);

        /// <summary>
        /// Gets the App_Database_Create_Failed string.
        /// </summary>
        public static string App_Database_Create_Failed => RmMb.GetString("Text_App_Database_Create_Failed", Cul);

        /// <summary>
        /// Gets the App_Database_Changed string.
        /// </summary>
        public static string App_Database_Changed => RmMb.GetString("Text_App_Database_Changed", Cul);

        /// <summary>
        /// Gets the App_Database_Changed_No_Location string.
        /// </summary>
        public static string App_Database_Changed_No_Location => RmMb.GetString("Text_App_Database_Changed_No_Location", Cul);

        /// <summary>
        /// Gets the Cre_new_Ora_Conn string.
        /// </summary>
        public static string Cre_new_Ora_Conn => RmMb.GetString("Text_Cre_new_Ora_Conn", Cul);

        /// <summary>
        /// Gets the File_Exist_Overwrite string.
        /// </summary>
        public static string File_Exist_Overwrite => RmMb.GetString("Text_File_Exist_Overwrite", Cul);

        /// <summary>
        /// Gets the User_Role_Not_In_Sync string.
        /// </summary>
        public static string User_Role_Not_In_Sync => RmMb.GetString("Text_User_Role_Not_In_Sync", Cul);

        /// <summary>
        /// Gets the Column_Is_Empty string.
        /// </summary>
        public static string Column_Is_Empty => RmMb.GetString("Text_Column_Is_Empty", Cul);

        /// <summary>
        /// Gets the User_Data_Does_not_Exist string.
        /// </summary>
        public static string User_Data_Does_not_Exist => RmMb.GetString("Text_User_Data_Does_not_Exist", Cul);

        /// <summary>
        /// Gets the Max_50 string.
        /// </summary>
        public static string Max_50 => RmMb.GetString("Text_Max_50", Cul);

        /// <summary>
        /// Gets the Max_100 string.
        /// </summary>
        public static string Max_100
        {
            get { return RmMb.GetString("Text_Max_100", Cul); }
        }

        /// <summary>
        /// Gets the Max_200 string.
        /// </summary>
        public static string Max_200
        {
            get { return RmMb.GetString("Text_Max_200", Cul); }
        }

        /// <summary>
        /// Gets the Max_255 string.
        /// </summary>
        public static string Max_255
        {
            get { return RmMb.GetString("Text_Max_255", Cul); }
        }

        /// <summary>
        /// Gets the Max_10000 string.
        /// </summary>
        public static string Max_10000
        {
            get { return RmMb.GetString("Text_Max_10000", Cul); }
        }

        /// <summary>
        /// Gets the Max_900000 string.
        /// </summary>
        public static string Max_900000
        {
            get { return RmMb.GetString("Text_Max_900000", Cul); }
        }

        /// <summary>
        /// Gets the New_User string.
        /// </summary>
        public static string New_User
        {
            get { return RmMb.GetString("Text_New_User", Cul); }
        }

        /// <summary>
        /// Gets the Selected__User_Not_Deleted string.
        /// </summary>
        public static string Selected__User_Not_Deleted
        {
            get { return RmMb.GetString("Text_Selected__User_Not_Deleted", Cul); }
        }

        /// <summary>
        /// Gets the Data_User_Not_Changed string.
        /// </summary>
        public static string Data_User_Not_Changed
        {
            get { return RmMb.GetString("Text_Data_User_Not_Changed", Cul); }
        }

        /// <summary>
        /// Gets the User_Data_Exist string.
        /// </summary>
        public static string User_Data_Exist
        {
            get { return RmMb.GetString("Text_User_Data_Exist", Cul); }
        }

        /// <summary>
        /// Gets the User_Is_Deleted string.
        /// </summary>
        public static string User_Is_Deleted
        {
            get { return RmMb.GetString("Text_User_Is_Deleted", Cul); }
        }

        /// <summary>
        /// Gets the User_Gets_deleted string.
        /// </summary>
        public static string User_Gets_deleted
        {
            get { return RmMb.GetString("Text_User_Gets_deleted", Cul); }
        }

        /// <summary>
        /// Gets the Not_All_Fields_Filled string.
        /// </summary>
        public static string Not_All_Fields_Filled
        {
            get { return RmMb.GetString("Text_Not_All_Fields_Filled", Cul); }
        }

        /// <summary>
        /// Gets the Authentication_Name string.
        /// </summary>
        public static string Authentication_Name
        {
            get { return RmMb.GetString("Text_Authentication_Name", Cul); }
        }

        /// <summary>
        /// Gets the Password_Repeat string.
        /// </summary>
        public static string Password_Repeat
        {
            get { return RmMb.GetString("Text_Password_Repeat", Cul); }
        }

        /// <summary>
        /// Gets the Not_Allowed_Deleted_User string.
        /// </summary>
        public static string Not_Allowed_Deleted_User
        {
            get { return RmMb.GetString("Text_Not_Allowed_Deleted_User", Cul); }
        }

        /// <summary>
        /// Gets the Not_Allowed_Deleted_Any_User string.
        /// </summary>
        public static string Not_Allowed_Deleted_Any_User
        {
            get { return RmMb.GetString("Text_Not_Allowed_Deleted_Any_User", Cul); }
        }

        /// <summary>
        /// Gets the Not_Allowd_Change_User string.
        /// </summary>
        public static string Not_Allowd_Change_User
        {
            get { return RmMb.GetString("Text_Not_Allowd_Change_User", Cul); }
        }

        /// <summary>
        /// Gets the Not_Allowd_Change_Other_User string.
        /// </summary>
        public static string Not_Allowd_Change_Other_User
        {
            get { return RmMb.GetString("Text_Not_Allowd_Change_Other_User", Cul); }
        }

        /// <summary>
        /// Gets the Not_Allowd_Change_Other_User_With_Role string.
        /// </summary>
        public static string Not_Allowd_Change_Other_User_With_Role
        {
            get { return RmMb.GetString("Text_Not_Allowd_Change_Other_User_With_Role", Cul); }
        }

        /// <summary>
        /// Gets the Not_Allowd_Change_Other_User_With_Role_1 string.
        /// </summary>
        public static string Not_Allowd_Change_Other_User_With_Role_1
        {
            get { return RmMb.GetString("Text_Not_Allowd_Change_Other_User_With_Role_1", Cul); }
        }

        /// <summary>
        /// Gets the Enter_Authentiction_Name string.
        /// </summary>
        public static string Enter_Authentiction_Name
        {
            get { return RmMb.GetString("Text_Enter_Authentiction_Name", Cul); }
        }

        /// <summary>
        /// Gets the Duplicate_Databae_Conn string.
        /// </summary>
        public static string Duplicate_Databae_Conn
        {
            get { return RmMb.GetString("Text_Duplicate_Databae_Conn", Cul); }
        }

        /// <summary>
        /// Gets the Ready_Saving_Connection string.
        /// </summary>
        public static string Ready_Saving_Connection
        {
            get { return RmMb.GetString("Text_Ready_Saving_Connection", Cul); }
        }

        /// <summary>
        /// Gets the No_Write_Access string.
        /// </summary>
        public static string No_Write_Access
        {
            get { return RmMb.GetString("Text_No_Write_Access", Cul); }
        }

        /// <summary>
        /// Gets the Max_31_Chars_Excel string.
        /// </summary>
        public static string Max_31_Chars_Excel
        {
            get { return RmMb.GetString("Text_Max_31_Chars_Excel", Cul); }
        }

        /// <summary>
        /// Gets the Continue_Without_Saving string.
        /// </summary>
        public static string Continue_Without_Saving
        {
            get { return RmMb.GetString("Text_Continue_Without_Saving", Cul); }
        }

        /// <summary>
        /// Gets the Copy_DatabaseFile_Completed string.
        /// </summary>
        public static string Copy_DatabaseFile_Completed
        {
            get { return RmMb.GetString("Text_Copy_DatabaseFile_Completed", Cul); }
        }

        /// <summary>
        /// Gets the Database_Copy_Failed string.
        /// </summary>
        public static string Database_Copy_Failed
        {
            get { return RmMb.GetString("Text_Database_Copy_Failed", Cul); }
        }

        /// <summary>
        /// Gets the App_Database_Compressed string.
        /// </summary>
        public static string App_Database_Compressed
        {
            get { return RmMb.GetString("Text_App_Database_Compressed", Cul); }
        }

        /// <summary>
        /// Gets the Query_Not_Exported string.
        /// </summary>
        public static string Query_Not_Exported
        {
            get { return RmMb.GetString("Text_Query_Not_Exported", Cul); }
        }

        /// <summary>
        /// Gets the Query_Not_Saved_No_Ora_Conn string.
        /// </summary>
        public static string Query_Not_Saved_No_Ora_Conn
        {
            get { return RmMb.GetString("Text_Query_Not_Saved_No_Ora_Conn", Cul); }
        }

        /// <summary>
        /// Gets the User_Data_Changed string.
        /// </summary>
        public static string User_Data_Changed
        {
            get { return RmMb.GetString("Text_User_Data_Changed", Cul); }
        }

        /// <summary>
        /// Gets the Ora_Account_Expired string.
        /// </summary>
        public static string Ora_Account_Expired
        {
            get { return RmMb.GetString("Text_Ora_Account_Expired", Cul); }
        }

        /// <summary>
        /// Gets the Drop_Query_On_Folder string.
        /// </summary>
        public static string Drop_Query_On_Folder
        {
            get { return RmMb.GetString("Text_Drop_Query_On_Folder", Cul); }
        }

        /// <summary>
        /// Gets the Settings_File_Not_Found string.
        /// </summary>
        public static string Settings_File_Not_Found
        {
            get { return RmMb.GetString("Text_Settings_File_Not_Found", Cul); }
        }

        /// <summary>
        /// Gets the Error_Ora_Conn string.
        /// </summary>
        public static string Error_Ora_Conn
        {
            get { return RmMb.GetString("Text_Error_Ora_Conn", Cul); }
        }

        /// <summary>
        /// Gets the UnderConstruction string.
        /// </summary>
        public static string UnderConstruction
        {
            get { return RmMb.GetString("Under construction", Cul); }
        }

        /// <summary>
        /// Gets the TextPasswordNotEqualName string.
        /// </summary>
        public static string PasswordnotEqualName
        {
            get { return RmMb.GetString("TextPasswordNotEqualName", Cul); }
        }
    }
}
