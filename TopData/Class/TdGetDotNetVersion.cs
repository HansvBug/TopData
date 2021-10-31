namespace TopData
{
    using System.Collections.Generic;
    using Microsoft.Win32;

    /// <summary>
    /// Get the installed .net versions.
    /// </summary>
    public class TdGetDotNetVersion
    {
        // Bron: https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
        private readonly List<string> netVersions = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="TdGetDotNetVersion"/> class.
        /// </summary>
        public TdGetDotNetVersion()
        {
            this.Get45PlusFromRegistry();
            this.GetVersionFromRegistry();
        }

        /// <summary>
        /// get the .nert versions.
        /// </summary>
        /// <returns>A list with installes .net versions.</returns>
        public List<string> DotNetVersions()
        {
            return this.netVersions;
        }

        private void Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    this.netVersions.Add($".NET Framework Version: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}");
                }
                else
                {
                    this.netVersions.Add(".NET Framework Version 4.5 of nieuwer is niet gevonden.");
                }
            }

            // Checking the version using >= enables forward compatibility.
            static string CheckFor45PlusVersion(int releaseKey)
            {
                if (releaseKey >= 528040)
                {
                    return "4.8 of nieuwer";
                }

                if (releaseKey >= 461808)
                {
                    return "4.7.2";
                }

                if (releaseKey >= 461308)
                {
                    return "4.7.1";
                }

                if (releaseKey >= 460798)
                {
                    return "4.7";
                }

                if (releaseKey >= 394802)
                {
                    return "4.6.2";
                }

                if (releaseKey >= 394254)
                {
                    return "4.6.1";
                }

                if (releaseKey >= 393295)
                {
                    return "4.6";
                }

                if (releaseKey >= 379893)
                {
                    return "4.5.2";
                }

                if (releaseKey >= 378675)
                {
                    return "4.5.1";
                }

                if (releaseKey >= 378389)
                {
                    return "4.5";
                }

                // This code should never execute. A non-null release key should mean
                // that 4.5 or later is installed.
                return "Geen versie 4.5 of nieuwere versie aangetroffen.";
            }
        }

        private void GetVersionFromRegistry()
        {
            // Opens the registry key for the .NET Framework entry.
            using RegistryKey ndpKey =
                    RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).
                    OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\");

            foreach (var versionKeyName in ndpKey.GetSubKeyNames())
            {
                // Skip .NET Framework 4.5 version information.
                if (versionKeyName == "v4")
                {
                    continue;
                }

                if (versionKeyName.StartsWith("v"))
                {
                    RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);

                    // Get the .NET Framework version value.
                    var name = (string)versionKey.GetValue("Version", string.Empty);

                    // Get the service pack (SP) number.
                    var sp = versionKey.GetValue("SP", string.Empty).ToString();

                    // Get the installation flag, or an empty string if there is none.
                    var install = versionKey.GetValue("Install", string.Empty).ToString();

                    // No install info; it must be in a child subkey.
                    if (string.IsNullOrEmpty(install))
                    {
                        this.netVersions.Add(".NET Framework Version: " + versionKeyName + " " + name);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sp) && install == "1")
                        {
                            this.netVersions.Add(".NET Framework Version: " + versionKeyName + " " + name + " " + "SP" + sp);
                        }
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    foreach (var subKeyName in versionKey.GetSubKeyNames())
                    {
                        RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                        name = (string)subKey.GetValue("Version", string.Empty);
                        if (!string.IsNullOrEmpty(name))
                        {
                            sp = subKey.GetValue(".NET Framework Version: " + "SP", string.Empty).ToString();
                        }

                        install = subKey.GetValue("Install", string.Empty).ToString();

                        // No install info; it must be later.
                        if (string.IsNullOrEmpty(install))
                        {
                            this.netVersions.Add(".NET Framework Version: " + versionKeyName + " " + name);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(sp) && install == "1")
                            {
                                this.netVersions.Add(".NET Framework Version: " + versionKeyName + " " + name + " " + "SP" + sp);
                            }
                            else if (install == "1")
                            {
                                this.netVersions.Add(subKeyName + " " + name);
                            }
                        }
                    }
                }
            }
        }
    }
}
