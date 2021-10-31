namespace TopData
{
    using System.Security;

    /// <summary>
    /// Hold the data of a user.
    /// </summary>
    public static class TdUserData
    {
        /// <summary>
        /// Gets or sets the users password.
        /// </summary>
        public static SecureString? UserPassword { get; set; }

        /// <summary>
        /// Gets or sets users role.
        /// </summary>
        public static string? UserRole { get; set; }

        /// <summary>
        /// Gets or sets the users name.
        /// </summary>
        public static string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the users id.
        /// </summary>
        public static string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        public static string? InlogSalt { get; set; }

        /// <summary>
        /// Gets or sets the users environment name.
        /// </summary>
        public static string? EnvironmentUserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether change user is activated.
        /// // Used in the log in form.
        /// </summary>
        public static bool ChangeUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user may have access.
        /// Used in the log in form. When change user is canceled, the current user is still logged in.
        /// </summary>
        public static bool AccesIsTrue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the first log in.
        /// // Set to yes on the initial login.
        /// </summary>
        public static bool FirstLogIn { get; set; }

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public static string? ConnectionName { get; set; }

        /// <summary>
        /// Gets or sets the connecetion id.
        /// </summary>
        public static int ConnectionId { get; set; } = -1;  // Oracle connection id, no connection then the id = -1; TODO rename to OraConnectionId
    }
}
