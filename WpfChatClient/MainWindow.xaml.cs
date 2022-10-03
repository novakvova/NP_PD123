using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TCPConaoleLib;

namespace WpfChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient client = new TcpClient();
        NetworkStream ns;
        Thread thread;
        ChatMessage _message = new ChatMessage();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 1075;
            _message.UserName = txtUserName.Text;
            _message.UserId=Guid.NewGuid().ToString(); 
            client.Connect(ip, port);
            lbInfo.Items.Add("Підключення до сервера " + ip.ToString() + ":" + port);
            ns=client.GetStream();
            //Отримання даних від сервера
            thread = new Thread(o => ReceiveData((TcpClient)o));
            thread.Start(client);

            //Логінимося на сервак
            _message.MessageType = TypeMessage.Login;
            _message.Text = "Приєднався до чату";
            byte[] bytes = _message.Serialize();
            ns.Write(bytes);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _message.MessageType = TypeMessage.Logout;
            _message.Text = "Покинув чат";
            byte[] buffer = _message.Serialize();
            ns.Write(buffer, 0, buffer.Length);
            client.Client.Shutdown(SocketShutdown.Both);
            thread.Join();
            ns.Close();
            client.Close();
        }

        private void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;
            string data = "";
            while((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        ChatMessage message = ChatMessage.Deserialize(receivedBytes);
                        switch(message.MessageType)
                        {
                            case TypeMessage.Login:
                                {
                                    if (message.UserId != _message.UserId)
                                        lbInfo.Items.Add($"{message.UserName}:{message.Text}");
                                    break;
                                }
                            case TypeMessage.Logout:
                                {
                                    if (message.UserId != _message.UserId)
                                        lbInfo.Items.Add($"{message.UserName}:{message.Text}");
                                    break;
                                }
                            case TypeMessage.Message:
                                {
                                    lbInfo.Items.Add($"{message.UserName}:{message.Text}");
                                    break;
                                }
                        }
                        lbInfo.Items.MoveCurrentToLast();
                        lbInfo.ScrollIntoView(lbInfo.Items.CurrentItem);
                    }
                    catch { }
                }));
            }
        }

        private void bntSend_Click(object sender, RoutedEventArgs e)
        {
            _message.MessageType = TypeMessage.Message;
            _message.Text = txtText.Text;
            byte[] bytes = _message.Serialize();
            ns.Write(bytes, 0, bytes.Length);
        }
    }
}
