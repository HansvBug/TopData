namespace TopData
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// Methods for all forms.
    /// </summary>
    public static class TdExtensionMethods
    {
        /// <summary>
        /// Avoid flickering of a datagridview with large amount of data.
        /// </summary>
        /// <param name="dgv">Datagridview.</param>
        /// <param name="setting">True = DoubleBuffered = Yes.</param>
        public static void DoubleBuffered(DataGridView dgv, bool setting)
        {
            if (dgv != null)
            {
                Type dgvType = dgv.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(dgv, setting, null);
            }
        }

        /// <summary>
        /// Avoid flickering of a Treeview.
        /// </summary>
        /// <param name="trv">The TreeView.</param>
        /// <param name="setting">True = DoubleBuffered = Yes.</param>
        public static void DoubleBuffered(TreeView trv, bool setting)
        {
            if (trv != null)
            {
                Type dgvType = trv.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(trv, setting, null);
            }
        }
    }
}
