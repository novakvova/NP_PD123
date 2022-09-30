
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            string ipAddress = "91.238.103.197";//"127.0.0.1";
            int port = 1076;
            IPAddress ip = IPAddress.Parse(ipAddress);
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            Socket server = new Socket(ip.AddressFamily,
                SocketType.Stream, 
                ProtocolType.Tcp);

            //привязка сервера до кінцевої точки
            server.Bind(endPoint);
            server.Listen(1000); //масимальна чарга сревера.
            
            Console.WriteLine("Server start {0}", endPoint);

            while (true)
            {
                //Сервер очікує запита від клієнта
                Socket client = server.Accept();
                byte[] buffer = new byte[1024];
                int size = client.Receive(buffer); //читмаємо масив байтів від клієнта
                String text = Encoding.UTF8.GetString(buffer); //пететворив байти в текст
                Console.WriteLine($"Client: {client.RemoteEndPoint} \n Message: {text}");
                byte [] clientSendData = Encoding.UTF8.GetBytes($"Лови рашист гранату {DateTime.Now}");
                client.Send(clientSendData);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }
    }
}

