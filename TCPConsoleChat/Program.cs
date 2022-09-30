
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPConsoleChat
{
    public class Program
    {
        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();

        public static void Main(string[] args)
        {
            int count = 1;
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 1097);
            server.Start();
            while(true)
            {
                TcpClient client = server.AcceptTcpClient();
                lock (_lock) list_clients.Add(count, client);
                Thread t = new Thread(handle_client);
                t.Start(count);
                count++;
            }
        }

        private static void handle_client(object count)
        {
            int id = (int)count;
            TcpClient client;
            lock(_lock) client = list_clients[id];
            while(true)
            {
                NetworkStream stream = client.GetStream(); //читаємо дані від клієнта
                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);
                if(byte_count > 0) //Якщо дані є
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, byte_count);
                    brodcast(data);  //Розсилаємо дані усім клієнтам, які є в чаті
                    Console.WriteLine(data);
                }
            }
            lock (_lock) list_clients.Remove(id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
        private static void brodcast(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            lock(_lock)
            {
                foreach(TcpClient c in list_clients.Values) //Перебирає усіх клієнтів, які є
                {
                    NetworkStream stream = c.GetStream(); //Отримує потік самого клієнта
                    stream.Write(buffer, 0, buffer.Length); //Кидає повідомлення клієнту
                }
            }
        }

    }
}

