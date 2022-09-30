using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPConaoleChatClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 1097;
            TcpClient tcpClient = new TcpClient();

            tcpClient.Connect(ip, port);
            Console.WriteLine("Client connet server");
            NetworkStream ns = tcpClient.GetStream();
            Thread thread = new Thread(ReceiveData);
            thread.Start(tcpClient);

            string s;
            while(!string.IsNullOrEmpty((s=Console.ReadLine())))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(s);
                ns.Write(buffer, 0, buffer.Length);
            }
            tcpClient.Client.Shutdown(SocketShutdown.Both);
            thread.Join();
            ns.Close();
            tcpClient.Close();
            Console.WriteLine("Відєдналися від сервера");
            Console.ReadKey();

        }

        static void ReceiveData(object o)
        {
            TcpClient client = (TcpClient)o;
            NetworkStream ns = client.GetStream();
            byte[] recivedBytes = new byte[1024];
            int byte_count;
            while((byte_count = ns.Read(recivedBytes, 0, 1024)) > 0)
            {
                Console.WriteLine(Encoding.UTF8.GetString(recivedBytes, 0, byte_count));
            }
        }
    }
}