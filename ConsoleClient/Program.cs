
using System.Net;
using System.Net.Sockets;
using System.Text;

string ipAddressServer = "91.238.103.197";//"127.0.0.1";
int portServer = 1076;

IPAddress ipAddress = IPAddress.Parse(ipAddressServer);
IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, portServer);

Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

client.Connect(ipEndPoint);

Console.WriteLine($"Client connet to server {ipEndPoint}");
Console.WriteLine("Вкажіть текст повідомлення: ");
string message = Console.ReadLine();

byte[] bytes = Encoding.UTF8.GetBytes(message);
client.Send(bytes);
byte[] serverResponse = new byte[1024];
client.Receive(serverResponse);
string serverString = Encoding.UTF8.GetString(serverResponse);
Console.WriteLine("Server responce {0}", serverString);
client.Shutdown(SocketShutdown.Both);
client.Close();

