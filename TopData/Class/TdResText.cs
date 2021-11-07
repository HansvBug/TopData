namespace TopData
{
    using System.Globalization;
    using System.Resources;

    /// <summary>
    /// Text resources.
    /// </summary>
    public static class TdResText
    {
        /// <summary>
        /// Declare Resource manager to access to specific cultureinfo.
        /// </summary>
        private static ResourceManager rmText;

        /// <summary>
        ///  Declare culture info.
        /// </summary>
        private static CultureInfo cul;      // Declare culture info

        /// <summary>
        /// Gets or sets the ResourceManager.
        /// </summary>
        public static ResourceManager RmText
        {
            get { return rmText; }
            set { rmText = value; }
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
        /// Gets the Yes string.
        /// </summary>
        public static string Yes
        {
            get { return RmText.GetString("Yes", cul); }
        }

        /// <summary>
        /// Gets the No string.
        /// </summary>
        public static string No
        {
            get { return RmText.GetString("No", cul); }
        }

        public static string Owner
        {
            get { return RmText.GetString("UserRoleOwner", cul); }
        }

        /// <summary>
        /// Gets the System string.
        /// </summary>
        public static string System
        {
            get { return RmText.GetString("UserRoleSystem", cul); }
        }

        /// <summary>
        /// Gets the Administrator string.
        /// </summary>
        public static string Administrator
        {
            get { return RmText.GetString("UserRoleAdministrator", cul); }
        }

        /// <summary>
        /// Gets the Editor string.
        /// </summary>
        public static string Editor
        {
            get { return RmText.GetString("UserRoleEditor", cul); }
        }

        /// <summary>
        /// Gets the Viewer string.
        /// </summary>
        public static string Viewer
        {
            get { return RmText.GetString("UserRoleViewer", cul); }
        }
    }
}
