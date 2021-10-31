namespace TopData
{
    using System.Globalization;

    /// <summary>
    /// Holds the culture data.
    /// </summary>
    public static class TdCulture
    {
        /// <summary>
        /// Holds the selected culture.
        /// </summary>
        private static CultureInfo cul;

        /// <summary>
        /// Gets or sets Cul(tureInfo).
        /// </summary>
        public static CultureInfo Cul
        {
            get { return cul; }
            set { cul = value; }
        }
    }
}
