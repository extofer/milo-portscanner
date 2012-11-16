using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace milo.command
{
    class Program
    {
        static string Host { get; set; }
        static int FromPort { get; set; }
        static int ToPort { get; set; }
        static int i { get; set; }

        //public delegate void PortOpenEventHandler(string host, int port);
        //public delegate void PortClosedEventHandler(string host, int port);
       // public delegate void ErrorOccurredEventHandler(Exception ex);
        
        public enum PortState
        {
            Open,
            Closed
        }


        static void Main(string[] args)
        {
            if (args[0] == "-v" || args[0] == "-version")
            {
                Console.WriteLine("\t" + Services.GetVersion(Services.AssemblyInfo.Title) + " version: " + Services.GetVersion(Services.AssemblyInfo.Version));
                Console.WriteLine("\t" + Services.GetVersion(Services.AssemblyInfo.Company));
                Console.ReadLine();

            }
            else
            {
                

            try
            {
                Arguments arguments = new Arguments(args);
                Host = arguments["host"];

                if (Host == null)
                {
                    Console.Write("Enter Host: ");
                    Host = Console.ReadLine();
                }

                if (arguments["from"] == null && arguments["to"] == null)
                {
                    Console.Write("Do you want to scan the default ports? [y/n]: ");
                    string yn = Console.ReadLine();

                    if (yn == "y")
                    {
                        FromPort = 0;
                        ToPort = 1023;
                    }
                    else
                    {
                        if (yn != "n")
                        {
                            Console.WriteLine("Command Not Found!");
                        }
                        else
                        {
                            Console.Write("Enter Beginning Port: ");
                            FromPort = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Ending Port: ");
                            ToPort = Convert.ToInt32(Console.ReadLine());
                        }

                    }

                }
                else
                {
                    FromPort = Convert.ToInt32(arguments["from"]);
                    ToPort = Convert.ToInt32(arguments["to"]);
                }

                Console.WriteLine("Scanning .........");

                i = 0;
                do
                {
                    Thread t = new Thread(ScanHostPorts);
                    t.Start(FromPort);
                    FromPort++;

                    i++;
                    if (i == 1023)
                    {
                        i = 0;
                        Thread.Sleep(5000);
                    }


                } while (FromPort != ToPort);
               

            }

            catch (Exception ex)
            {

                Console.Write(ex);
            }

            Thread.Sleep(1000);

            Console.WriteLine(" ");
            Console.WriteLine("Scan is complete. Press Enter.");
            Console.ReadLine();

            }

        }


        private static void ScanHostPorts(object state)
        {
         
            PortState returnValue =  PortState.Closed;
            IPHostEntry hostInfo = Dns.GetHostEntry(Host);

            foreach (IPAddress hostAddress in hostInfo.AddressList)
            {

                IPEndPoint ePhost = new IPEndPoint(hostAddress, Convert.ToInt32(state));

                using (Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                            try
                            {
                                mySocket.Connect(ePhost);
                                if (mySocket.Connected)
                                {
                                    returnValue = PortState.Open;

                                    Console.WriteLine("Host: " + hostAddress + "\t" + " Port: " + state + "\t" + PortState.Open + "\t" + Services.GetName(Convert.ToInt32(state)));

                                    mySocket.Close();
                                }
                                else
                                {
                                    returnValue = PortState.Closed;
                                }
                            }
                            catch (Exception ex)
                            {
                                returnValue = PortState.Closed;
                            }
                            finally
                            {
                                if (returnValue == PortState.Open)
                                {
                                    Console.WriteLine("Host: " + hostAddress + "\t" + " Port: " + state + "\t" + PortState.Open + "\t" + Services.GetName(Convert.ToInt32(state)));
                                }

                            }
                }

            }

        }




        

    }
}
