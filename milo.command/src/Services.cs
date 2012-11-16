using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace milo.command
{
    public static class Services
    {

        public enum AssemblyInfo
        {
            Version,
            Title,
            Company
        }

        public static string GetName(int port)
        {
            string portName = ConfigurationManager.AppSettings["Port" + port];

            if (portName.Length == 0)
                {
                    portName = "";
                }
                return portName;
        }

        public static string GetVersion(AssemblyInfo info)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            try
            {
                switch (info)
                {
                    case AssemblyInfo.Title:
                        return fvi.ProductName;
                    case AssemblyInfo.Version:

                        int major = fvi.FileMajorPart;
                        int minor = fvi.FileMinorPart;
                        int rev = fvi.FileBuildPart;

                        string version = 
                            major.ToString(CultureInfo.InvariantCulture) + "." +
                            minor.ToString(CultureInfo.InvariantCulture) + "." + 
                            rev.ToString(CultureInfo.InvariantCulture);
                        return version;

                    case AssemblyInfo.Company:
                        return fvi.CompanyName;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

    }
}
