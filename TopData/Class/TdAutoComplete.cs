namespace TopData
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Add auto-complete functionality to objects.
    /// </summary>
    public class TdAutoComplete : IDisposable
    {
        /// <summary>
        /// Create a auto complete list from the treeview node names.
        /// </summary>
        /// <param name="trv">The selected streeview.</param>
        /// <returns>List with treeview nodes.</returns>
        public AutoCompleteStringCollection CreAutoCompleteListFromTrv(TreeView trv)
        {
            if (trv != null)
            {
                AutoCompleteStringCollection dataCollection = new ();
                this.AddItems(dataCollection, trv);

                return dataCollection;
            }
            else
            {
                return null;
            }
        }

        private void AddItems(AutoCompleteStringCollection col, TreeView trv)
        {
            TreeNodeCollection nodes = trv.Nodes;
            foreach (TreeNode n in nodes)
            {
                this.GetTrvNodeName(n, col);
            }
        }

        private void GetTrvNodeName(TreeNode treeNode, AutoCompleteStringCollection col)
        {
            col.Add(treeNode.Name);
            foreach (TreeNode tn in treeNode.Nodes)
            {
                this.GetTrvNodeName(tn, col);
            }
        }

        #region Dispose

        private readonly SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        private bool disposed;

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Has Dispose already been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.safeHandle?.Dispose();

                    // Free other state (managed objects).
                }

                // Free your own state (unmanaged objects).
                // Set large fields to null.
                this.disposed = true;
            }
        }
        #endregion Dispose
    }
}
