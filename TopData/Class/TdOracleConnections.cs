namespace TopData
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds the TopDataOracleConnection type.
    /// </summary>
    public class TdOracleConnections
    {
        private readonly List<TdOracleConnection> items = new ();

        /// <summary>
        /// Gets items. These are the Oracle connections as saved in the table: CONN_ORACLE.
        /// </summary>
        public List<TdOracleConnection> Items
        {
            get { return this.items; }
        }
    }
}
