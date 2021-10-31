namespace TopData
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Secure or unsecure a string.
    /// </summary>
    public class TdSecurityExtensions : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TdSecurityExtensions"/> class.
        /// </summary>
        public TdSecurityExtensions()
        {
            // Default constructor
        }

        /// <summary>
        /// Convert a plain string to a secure string.
        /// </summary>
        /// <param name="plainString">The plain string.</param>
        /// <returns>The secure string.</returns>
        public SecureString ConvertToSecureString(string plainString)
        {
            if (plainString == null)
            {
                return null;
            }

            SecureString secureString = new ();
            foreach (char c in plainString.ToCharArray())
            {
                secureString.AppendChar(c);
            }

            return secureString;
        }

        /// <summary>
        /// Convert a secure strng to a plain string.
        /// </summary>
        /// <param name="sString">The secure string.</param>
        /// <returns>The plain string.</returns>
        public string UnSecureString(SecureString sString)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(sString);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }

            // Source: https://gist.github.com/technoscavenger/b8a29314b404934ae23fc5d2ef1bd9e1
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

                // Free your own state (unmanaged objects). Set large fields to null.
                this.disposed = true;
            }
        }
        #endregion Dispose
    }
}
