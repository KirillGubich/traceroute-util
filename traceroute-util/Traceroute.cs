using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace traceroute_util
{
    public class Traceroute
    {
        private const int MAXIMUM_JUMPS = 30;
        private const int PACKET_COUNT = 3;
        private const int RECEIVE_TIMEOUT = 3000;

        public void Run(string host, bool needToViewDns)
        {   
            IPHostEntry ipHost;
            try
            {
               ipHost = Dns.GetHostEntry(host); 
            }
            catch (SocketException)
            {
                Console.WriteLine("Не удается разрешить системное имя узла " + host);
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Длина разрешаемого имени узла или IP-адреса превышает 255 символов.");
                return;
            }

            IPEndPoint ipEndPoint = new IPEndPoint(ipHost.AddressList[0], 0);
            EndPoint endPoint = ipEndPoint;
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);

            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, RECEIVE_TIMEOUT);
            bool isEndPointReached = false;
            byte[] data = IcmpUtils.GetEchoIcmpPackage();
            byte[] receivedData;
            int ttl = 1;

            Console.WriteLine("Трассировка маршрута к " + host);
            Console.WriteLine("с максимальным количеством прыжков " + MAXIMUM_JUMPS + ":");
            Console.WriteLine();
            for (int i = 0; i < MAXIMUM_JUMPS; i++)
            {
                int errorCount = 0;
                Console.Write("{0, 2}", i + 1);
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive, ttl++);
                Stopwatch track = new Stopwatch();
                receivedData = new byte[512];
                for (int j = 0; j < PACKET_COUNT; j++)
                {                                   
                    try
                    {        
                        track.Reset();
                        track.Start();
                        socket.SendTo(data, data.Length, SocketFlags.None, ipEndPoint);
                        socket.ReceiveFrom(receivedData, ref endPoint);
                        track.Stop();
                        long responseTime = track.ElapsedMilliseconds;
                        isEndPointReached = DestinationReached(receivedData, responseTime);
                    }
                    catch (SocketException)
                    {
                        Console.Write("{0, 10}", "*");
                        errorCount++;
                    }
                }
                if (errorCount == PACKET_COUNT)
                {
                    Console.Write("  Превышен интервал ожидания для запроса.\n");
                }
                else if (needToViewDns)
                {
                    ViewDns(endPoint);
                }
                else 
                { 
                    Console.Write($"  {endPoint}\n");
                }

                if (isEndPointReached)
                {
                    Console.WriteLine("\nТрассировка завершена.");
                    break;
                }
            }
        }

        private bool DestinationReached(byte[] receivedMessage, long responseTime)
        {
            int responceType = IcmpUtils.GetIcmpType(receivedMessage);          
            if (responceType == 0)
            {
                Console.Write("{0, 10}", responseTime + " мс");
                return true;
            }
            if (responceType == 11)
            {
                Console.Write("{0, 10}", responseTime + " мс");
            }
            return false;
        }

        private void ViewDns(EndPoint endPoint)
        {
            try
            {
                Console.Write($"  {ExtractDns(endPoint)} [{endPoint}]\n");
            }
            catch (SocketException)
            {
                Console.Write($"  {endPoint}\n");
            }
        }

        private string ExtractDns(EndPoint endPoint)
        {
            return Dns.GetHostEntry(IPAddress.Parse(endPoint.ToString().Split(':')[0])).HostName;
        }
    }
}
