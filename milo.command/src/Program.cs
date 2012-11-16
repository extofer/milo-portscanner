using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading;


namespace milo.command
{
    class Program
    {
        static string Host { get; set; }
        static int FromPort { get; set; }
        static int ToPort { get; set; }

        //public event PortOpenEventHandler PortOpen;
        public delegate void PortOpenEventHandler(string host, int port);
        ///ublic event PortClosedEventHandler PortClosed;
        public delegate void PortClosedEventHandler(string host, int port);
        //public event ErrorOccurredEventHandler ErrorOccurred;
        public delegate void ErrorOccurredEventHandler(Exception ex);

        //public event Scanner.ScanCompleteEventHandler ScanComplete;

        public enum PortState
        {
            Open,
            Closed
        }


        static void Main(string[] args)
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


                //Scanner scanner = new Scanner(Host, FromPort, ToPort);

                
                Console.WriteLine("Scanning .........");


                do
                {
                    Thread t = new Thread(new ParameterizedThreadStart(ScanHostPorts));
                    t.Start(FromPort);
                    FromPort++;
                } while (FromPort != ToPort);
               

            }
            catch (Exception ex)
            {
                
                Console.Write(ex);
            }

            //Console.Write("Do you exit? [y/n]: ");
            //string x = Console.ReadLine();

            Thread.Sleep(1000);

            Console.WriteLine(" ");
            Console.WriteLine("Scan is complete. Press Enter.");
            Console.ReadLine();


        }




        private static void TestThread()
        {
            Console.WriteLine("This is a test");
        }


        private static void ScanHostPorts(object state)
        {
            //int state;
            //string host;

            PortState returnValue = default(PortState);
            returnValue = PortState.Closed;

            IPHostEntry hostInfo = Dns.GetHostEntry(Host);

            //IPAddress hostAddress = hostInfo.AddressList[0];
            //IPAddress hostAddress = Dns.GetHostEntry(_host).AddressList[0];
            //IPAddress hostAddress = Dns.Resolve(_host).AddressList[0];


            foreach (IPAddress hostAddress in hostInfo.AddressList)
            {


                IPEndPoint ePhost = new IPEndPoint(hostAddress, Convert.ToInt32(state));
                Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
