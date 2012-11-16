using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace milo.command
{
    public static class Services
    {

        public static string GetName(int port)
        {
            string portName = ConfigurationManager.AppSettings["Port" + port];

            if (portName.Length == 0)
                {
                    portName = "";
                }
                return portName;
        }


    }
}
