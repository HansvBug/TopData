namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Get the process arguments when the application starts.
    /// </summary>
    public class TdProcessArguments : IDisposable
    {
        #region Properties
        private List<string> cmdLineArg = new ();

        /// <summary>
        /// Gets or sets a list with arguments.
        /// </summary>
        public List<string> CmdLineArg
        {
            get { return this.cmdLineArg; }
            set { this.cmdLineArg = value; }
        }

        /// <summary>
        /// Gets or sets the Install argument.
        /// </summary>
        public string ArgIntall { get; set; }

        /// <summary>
        /// Gets or sets the Install System argument.
        /// </summary>
        public string ArgInstallSystem { get; set; }

        /// <summary>
        /// Gets or sets the Install Owner argument.
        /// </summary>
        public string ArgInstallOwner { get; set; }

        /// <summary>
        /// Gets or sets the Debug argument.
        /// </summary>
        public string ArgDebug { get; set; }

        #endregion Properties

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessArguments"/> class.
        /// </summary>
        public TdProcessArguments()
        {
            this.GetArguments();
        }
        #endregion constructor

        private void GetArguments()
        {
            string[] args = Environment.GetCommandLineArgs();   // Store command line arguments

            foreach (string arg in args)
            {
                string argument = Convert.ToString(arg, CultureInfo.InvariantCulture);
                this.CmdLineArg.Add(argument);  // 0 = program name
                switch (argument)
                {
                    case "Install":
                        this.ArgIntall = "Install";
                        break;
                    case "DefaultSystem":
                        this.ArgInstallSystem = "DefaultSystem";
                        break;
                    case "DefaultOwner":
                        this.ArgInstallOwner = "DefaultOwner";
                        break;
                    case "DebugMode=On":
                        this.ArgDebug = "DebugMode=On";
                        break;
                }
            }
        }

        #region Dispose

        private bool disposed;  // Flag: Has Dispose already been called?

        // Instantiate a SafeHandle instance.
        private SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);

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
                    this.CmdLineArg = null;

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
