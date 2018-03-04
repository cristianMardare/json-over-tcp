using JsonRpcOverTcp.Channels;
using JsonRpcOverTcp.ServiceModel;
using JsonRpcOverTcp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace WcfServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = null;
            try
            {
                serviceHost = new ServiceHost(typeof(WcfServiceHost.WcfService));
                serviceHost.Open();
                Console.WriteLine("Host opened");

                RunTestBatch(serviceHost);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message}");
                Console.WriteLine("\nPress any key to close the Service");
                Console.ReadKey();
                serviceHost?.Close();
                serviceHost = null;
            }

            if (serviceHost != null)
            {
                Console.WriteLine("\nPress any key to close the Service");
                Console.ReadKey();
                Console.WriteLine("\nService is shutting down...");
                serviceHost.Close();
                serviceHost = null;
            }
        }

        static ServiceHost InitializeProgramatically()
        {
            string baseAddress = "sized.tcp://" + Environment.MachineName + ":8000/";
            ServiceHost host = new ServiceHost(typeof(WcfService), new Uri(baseAddress));
            CustomBinding binding = new CustomBinding(
                new ByteStreamMessageEncodingBindingElement(),
                new SizedTcpTransportBindingElement());
            //host.AddServiceEndpoint(typeof(IUntypedTest), binding, "");
            host.AddServiceEndpoint(typeof(IWcfService), binding, "").Behaviors.Add(new JsonRpcEndpointBehavior());

            return host;
        }

        static void RunTestBatch(ServiceHost host)
        {


            string[] requests = new string[]
            {
                "{\"method\":\"Collect\",\"params\":[1],\"id\":1}"
            };

            foreach (string request in requests)
            {
                Console.WriteLine("Request: {0}", request);
                Socket socket = GetConnectedSocket(8000);
                byte[] data = Encoding.UTF8.GetBytes(request);
                byte[] toSend = new byte[data.Length + 4];
                Formatting.SizeToBytes(data.Length, toSend, 0);
                Array.Copy(data, 0, toSend, 4, data.Length);
                socket.Send(toSend);
                Console.WriteLine("Sent request to the server");
                byte[] recvBuffer = new byte[1000];
                int bytesReceived = socket.Receive(recvBuffer);
                Console.WriteLine("Received {0} bytes", bytesReceived);
                Debugging.PrintBytes(recvBuffer, bytesReceived);
                socket.Close();
            }
        }

        static Socket GetConnectedSocket(int port)
        {
            Socket socket = null;
            IPHostEntry hostEntry = Dns.GetHostEntry(Environment.MachineName);
            for (int i = 0; i < hostEntry.AddressList.Length; i++)
            {
                try
                {
                    IPAddress address = hostEntry.AddressList[i];
                    socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(new IPEndPoint(address, port));
                    break;
                }
                catch (SocketException)
                {
                }
            }

            return socket;
        }
    }
}
