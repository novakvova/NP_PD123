using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerWPFUI
{
    class Program
    {
        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();

        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 1075;
            TcpListener serverSocket = new TcpListener(ip, port);
            serverSocket.Start();
            int count = 1;
            while(true)
            {
                TcpClient client = serverSocket.AcceptTcpClient();
                lock (_lock) list_clients.Add(count, client);
                Console.WriteLine("Connect Client {0}", client.Client.RemoteEndPoint);
                Thread t = new Thread(handle_client);
                t.Start(count);
                count++;
            }
        }
        public static void handle_client(object o)
        {
            int id = (int)o;
            TcpClient client;
            lock(_lock) client = list_clients[id];
            while(true)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);
                if(byte_count==0)
                    break;
                string data = Encoding.UTF8.GetString(buffer,0,byte_count);
                brodcast(data);
                Console.WriteLine(data);
            }
            lock(_lock) list_clients.Remove(id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
        public static void brodcast(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            lock(_lock)
            {
                foreach(TcpClient c in list_clients.Values)
                {
                    NetworkStream stream = c.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
