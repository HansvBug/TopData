namespace TopData
{
    using System;
    using System.Security;

    /// <summary>
    /// Holds the oracle connection parameters.
    /// </summary>
    public class TdOracleConnection
    {
        #region properties

        /// <summary>
        /// Gets or sets the TopDataOracleConnection id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the schema name.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the password name.
        /// </summary>
        public SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the Datasource name.
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// Gets or sets the connction create date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the user name who created the connection.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets or set the date when the connection is altered.
        /// </summary>
        public DateTime Altered { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who alterd the connection.
        /// </summary>
        public string Alteredby { get; set; }
        #endregion properties

        /// <summary>
        /// Initializes a new instance of the <see cref="TdOracleConnection"/> class.
        /// </summary>
        /// <param name="id">The id of the connection.</param>
        public TdOracleConnection(int id)
        {
            this.Id = id;
        }
    }
}
