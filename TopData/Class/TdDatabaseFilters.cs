namespace TopData
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Filter the query result.
    /// </summary>
    public class TdDatabaseFilters
    {
        private readonly List<TdDatabaseFilter> filteritems = new ();

        /// <summary>
        /// Gets a list with all the filterd items.
        /// </summary>
        public List<TdDatabaseFilter> CheckedFilterItem
        {
            get { return this.filteritems; }
        }

        #region Properties etc

        /// <summary>
        /// Gets or sets a reference to the main form.
        /// </summary>
        public FormMain Parent { get; set; }

        /// <summary>
        /// Gets or sets the filter query.
        /// </summary>
        public string FilterQuery { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the filter form is opened.
        /// </summary>
        public bool FormIsOpened { get; set; }

        /// <summary>
        /// Gets or sets the Decimal seperator.
        /// </summary>
        public string DecimalSeperator { get; set; }

        #endregion Properties etc

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdDatabaseFilters"/> class.
        /// </summary>
        public TdDatabaseFilters()
        {
            // Default
        }

        #endregion constructor

        /// <summary>
        /// Create the filter string.
        /// </summary>
        public void FilterSamenstellen()
        {
            if (TdDebugMode.DebugMode)
            {
                TdLogging.WriteToLogDebug("Start Filter samenstellen...");
            }

            string build;
            string buildFilterComplete = string.Empty;
            string buildEmptyValue;

            string filterType;
            string filterTextOrNumber = string.Empty;

            bool emptyValueString = false;

            string emptyValueOperator = string.Empty; // AND / OR

            // 1 or more items checked for filtering
            if (this.CheckedFilterItem.Count >= 1)
            {
                foreach (var getCheckedItem in this.CheckedFilterItem)
                {
                    // Filter type:
                    filterType = FilterOperator(getCheckedItem.FilterType);
                    filterTextOrNumber = getCheckedItem.TextOrNumberFilter;

                    if (getCheckedItem.TextOrNumberFilter == "Tekstfilters")
                    {
                        filterType = FilterOperator(getCheckedItem.FilterType);
                    }
                    else if (getCheckedItem.TextOrNumberFilter == "Getalfilters")
                    {
                        filterType = GetFilterTypeNumber(getCheckedItem.FilterType);

                        // replace decimal seperator if needed
                        for (int i = 0; i < getCheckedItem.CheckedItems.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(getCheckedItem.CheckedItems[i]))
                            {
                                if (this.DecimalSeperator != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                                {
                                    getCheckedItem.CheckedItems[i] = getCheckedItem.CheckedItems[i].Replace(this.DecimalSeperator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                                    if (TdDebugMode.DebugMode)
                                    {
                                        TdLogging.WriteToLogDebug("Getalsfilter ; this.DecimalSeperator                                          : " + this.DecimalSeperator);
                                        TdLogging.WriteToLogDebug("Getalsfilter ; CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator : " + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                                        TdLogging.WriteToLogDebug("getCheckedItem._checkedItems[i]                                               : " + getCheckedItem.CheckedItems[i]);
                                    }
                                }
                            }
                        }
                    }
                    else if (getCheckedItem.TextOrNumberFilter == "Datumfilters")
                    {
                        filterType = GetFilterTypeDate(getCheckedItem.FilterType);
                    }

                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug("FilterType : " + filterType);
                    }

                    emptyValueOperator = getCheckedItem.Operator;  // Used by checked empty values.

                    // Build the filter string
                    build = string.Empty;
                    buildEmptyValue = string.Empty;

                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug("Filter string : " + build);
                    }

                    for (int i = 0; i < getCheckedItem.CheckedItems.Count; i++)
                    {
                        emptyValueString = false;

                        if (getCheckedItem.TextOrNumberFilter == "Tekstfilters")
                        {
                            if (!string.IsNullOrEmpty(getCheckedItem.CheckedItems[i]))
                            {
                                build += this.SurroundWith(getCheckedItem.CheckedItems[i].Replace("'", "''"), "'", getCheckedItem.FilterType);  // if a string contains a quote then it must be replaced with ''
                                if (TdDebugMode.DebugMode)
                                {
                                    TdLogging.WriteToLogDebug("Filter string : " + build);
                                }
                            }
                            else
                            {
                                emptyValueString = true;
                                buildEmptyValue = " (" + getCheckedItem.ColumnName + " IS NULL OR " + getCheckedItem.ColumnName + "= '')";  // make string for checked empty value
                                if (TdDebugMode.DebugMode)
                                {
                                    TdLogging.WriteToLogDebug("Filter string : " + build);
                                }
                            }
                        }
                        else if (getCheckedItem.TextOrNumberFilter == "Getalfilters")
                        {
                            if (!string.IsNullOrEmpty(getCheckedItem.CheckedItems[i]))
                            {
                                build += this.SurroundWith(getCheckedItem.CheckedItems[i].Replace("'", "''"), "'", getCheckedItem.FilterType);

                                if (TdDebugMode.DebugMode)
                                {
                                    TdLogging.WriteToLogDebug("Filter string : " + build);
                                }
                            }
                            else
                            {
                                emptyValueString = true;
                                buildEmptyValue = " (" + getCheckedItem.ColumnName + " IS NULL" + ")";
                                if (TdDebugMode.DebugMode)
                                {
                                    TdLogging.WriteToLogDebug("Filter string : " + build);
                                }
                            }
                        }
                        else if (getCheckedItem.TextOrNumberFilter == "Datumfilters")
                        {
                            if (!string.IsNullOrEmpty(getCheckedItem.CheckedItems[i]))
                            {
                                build += this.SurroundWith(getCheckedItem.CheckedItems[i], "'", getCheckedItem.FilterType);
                                if (TdDebugMode.DebugMode)
                                {
                                    TdLogging.WriteToLogDebug("Filter string : " + build);
                                }
                            }
                            else
                            {
                                emptyValueString = true;
                                buildEmptyValue = " (" + getCheckedItem.ColumnName + " IS NULL" + ")";
                                if (TdDebugMode.DebugMode)
                                {
                                    TdLogging.WriteToLogDebug("Filter string : " + build);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(getCheckedItem.CheckedItems[i]))
                        {
                            build += ",";
                            if (TdDebugMode.DebugMode)
                            {
                                TdLogging.WriteToLogDebug("Filter string : " + build);
                            }
                        }
                    }

                    build = build.TrimEnd(','); // Remove last ","

                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug("Filter string : " + build);
                    }

                    // if checked item has value then "AND kolomnaam('waarde')"
                    if (!emptyValueString)
                    {
                        build = getCheckedItem.Operator + getCheckedItem.ColumnName + filterType + "(" + build + ")";
                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug("Filter string : " + build);
                        }
                    }

                    // Concatenate the filter string...
                    if (!string.IsNullOrEmpty(build) && !string.IsNullOrEmpty(buildEmptyValue))
                    {
                        if (string.IsNullOrEmpty(emptyValueOperator))
                        {
                            emptyValueOperator = " OR ";
                        }

                        buildFilterComplete = "( " + buildFilterComplete + build + emptyValueOperator + buildEmptyValue + " ) ";  // extra "(  )" for the AND and OR
                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug("Filter string : " + buildFilterComplete);
                        }
                    }
                    else if (string.IsNullOrEmpty(build) && !string.IsNullOrEmpty(buildEmptyValue))
                    {
                        buildFilterComplete = "(" + buildFilterComplete + emptyValueOperator + buildEmptyValue + ")";
                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug("Filter string : " + buildFilterComplete);
                        }

                        // Loze AND of OF voor de filter verwijderen. Treedt op als in 2 kolommen in beide alleen een lege regel wordt aangevinkt
                        if (buildFilterComplete.Substring(0, 6) == "( AND " || buildFilterComplete.Substring(0, 6) == "( OR ")
                        {
                            buildFilterComplete = buildFilterComplete.Remove(0, 6);
                            if (TdDebugMode.DebugMode)
                            {
                                TdLogging.WriteToLogDebug("Filter string : " + buildFilterComplete);
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(build) && string.IsNullOrEmpty(buildEmptyValue))
                    {
                        buildFilterComplete = "(" + buildFilterComplete + build + ")";
                        if (TdDebugMode.DebugMode)
                        {
                            TdLogging.WriteToLogDebug("Filter string : " + buildFilterComplete);
                        }
                    }
                }

                this.FilterQuery = buildFilterComplete;

                TdLogging.WriteToLogInformation("Filter (" + filterTextOrNumber + ") : " + buildFilterComplete);

                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug("Filter  string (" + filterTextOrNumber + ") : " + buildFilterComplete);
                }
            }
        }

        private static string FilterOperator(string filterType)
        {
            switch (filterType)
            {
                case " IN ":
                    {
                        return " IN ";
                    }

                case "Is gelijk aan":
                    {
                        return " IN ";
                    }

                case "Is niet gelijk aan":
                    {
                        return " NOT IN ";
                    }

                case "Begint met":
                    {
                        return " LIKE ";
                    }

                case "Begint niet met":
                    {
                        return " NOT LIKE ";
                    }

                case "Eindigt met":
                    {
                        return " LIKE ";
                    }

                case "Eindigt niet met":
                    {
                        return " NOT LIKE ";
                    }

                case "Bevat":
                    {
                        return " LIKE ";
                    }

                case "Bevat niet":
                    {
                        return " NOT LIKE ";
                    }

                case "Groter dan":
                    {
                        return " > ";
                    }

                case "Is Groter dan of gelijk aan":
                    {
                        return " >= ";
                    }

                case "Kleiner dan":
                    {
                        return " < ";
                    }

                case "Is Kleiner dan of gelijk aan":
                    {
                        return " <= ";
                    }

                default:
                    {
                        return string.Empty;
                    }
            }
        }

        private static string GetFilterTypeNumber(string filterType)
        {
            if (filterType == "Is gelijk aan")
            {
                return " = ";
            }
            else if (filterType == "Is niet gelijk aan")
            {
                return " <> ";
            }
            else if (filterType == "Groter dan")
            {
                return " > ";
            }
            else if (filterType == "Is Groter dan of gelijk aan")
            {
                return " >= ";
            }
            else if (filterType == "Kleiner dan")
            {
                return " < ";
            }
            else if (filterType == "Is Kleiner dan of gelijk aan")
            {
                return " <= ";
            }
            else if (filterType == " IN ")
            {
                return " IN ";
            }
            else if (string.IsNullOrEmpty(filterType))
            {
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetFilterTypeDate(string filterType)
        {
            if (filterType == "Is gelijk aan")
            {
                return " = ";
            }
            else if (filterType == "Is niet gelijk aan")
            {
                return " <> ";
            }
            else if (filterType == "Voor")
            {
                return " < ";
            }
            else if (filterType == "Na")
            {
                return " > ";
            }
            else if (filterType == " IN ")
            {
                return " = ";
            }
            else if (string.IsNullOrEmpty(filterType))
            {
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        private string SurroundWith(string text, string ends, string filterSign)
        {
            if (TdDebugMode.DebugMode)
            {
                TdLogging.WriteToLogDebug("SurroundWith start (text, end, FilterSign) : " + text + "  -  " + ends + "  -  " + filterSign);
            }

            if (filterSign == "Begint met")
            {
                return ends + text + "*" + ends;
            }
            else if (filterSign == "Begint niet met")
            {
                return ends + text + "*" + ends;
            }
            else if (filterSign == "Eindigt met")
            {
                return ends + "*" + text + ends;
            }
            else if (filterSign == "Eindigt niet met")
            {
                return ends + "*" + text + ends;
            }
            else if (filterSign == "Bevat")
            {
                return ends + "*" + text + "*" + ends;
            }
            else if (filterSign == "Bevat niet")
            {
                return ends + "*" + text + "*" + ends;
            }
            else
            {
                TdLogging.WriteToLogDebug("SurroundWith return : " + ends + text + ends);
                return ends + text + ends;
            }
        }
    }
}
