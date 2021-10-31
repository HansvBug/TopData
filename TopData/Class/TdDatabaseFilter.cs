namespace TopData
{
    using System.Collections.Generic;

    /// <summary>
    /// Filter class. Filter the query result.
    /// </summary>
    public class TdDatabaseFilter
    {
        // Row filter syntax: http://www.csharp-examples.net/dataview-rowfilter/
        #region Properties etc

        /// <summary>
        /// gets or sets a reference to the main form.
        /// </summary>
        public FormMain Parent { get; set; }

        /// <summary>
        /// Gets or sets the datatype of the selected column.
        /// </summary>
        public string DataTypeSelectedColumn { get; set; }

        /// <summary>
        /// Gets or sets if the filter form is shown.
        /// </summary>
        public int FilterFormHasBeenOpenend { get; set; }

        /// <summary>
        /// Gets or sets the filter type.
        /// </summary>
        public string FilterType { get; set; }

        /// <summary>
        /// Gets or sets if the filter filters text or numbers.
        /// </summary>
        public string TextOrNumberFilter { get; set; }

        /// <summary>
        /// A list with the checked items. the items to filter.
        /// </summary>
        private List<string> checkedItems = new ();

        /// <summary>
        /// Gets or sets the checked items.
        /// </summary>
        public List<string> CheckedItems
        {
            get { return this.checkedItems; }
            set { this.checkedItems = value; }
        }

        private string columnName;

        /// <summary>
        /// Gets or sets the columnname. (The filtered column).
        /// </summary>
        public string ColumnName
        {
            get
            {
                return this.columnName;
            }

            set
            {
                if (value != null)
                {
                    if (value.Contains("'"))
                    {
                        this.columnName = "[" + value + "]";
                    }
                    else
                    {
                        this.columnName = value;
                    }
                }
            }

            /*
                Column names
                If a column name contains any of these special characters ~ ( ) # \ / = > < + - * % & | ^ ' " [ ], you must enclose the column name within square brackets [ ]. If a column name contains right bracket ] or backslash \, escape it with backslash (] or \).
             */
        }

        /// <summary>
        /// Gets or sets the filter string.
        /// </summary>
        public string Filterstring { get; set; }

        /// <summary>
        /// Gets or sets the filter operator (AND / OR).
        /// </summary>
        public string Operator { get; set; }
        #endregion  Properties etc
    }
}
