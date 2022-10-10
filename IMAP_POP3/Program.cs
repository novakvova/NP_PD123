using MailKit;
using MailKit.Net.Imap;
using System;

namespace IMAP_POP3
{
    class Progam
    {
        public static void Main(string[] args)
        {
            //From = "super.novakvova@ukr.net",
            //        SmtpServer = "smtp.ukr.net",
            //        Port = 2525,
            string userName = "super.novakvova@ukr.net";
            string password = "cnNfnQDgjVmmEttB";


            /*
             * Сервер вхідних листів (IMAP): imap.ukr.net.
                Порт - 993.
                З'єднання - захищене SSL.
                Безпечна перевірка пароля (SPA) вимкнена.
             */

            using (var client = new ImapClient())
            {
                client.Connect("imap.ukr.net", 993, true);

                client.Authenticate("super.novakvova@ukr.net", "cnNfnQDgjVmmEttB");

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                Console.WriteLine("Total messages: {0}", inbox.Count);
                Console.WriteLine("Recent messages: {0}", inbox.Recent);

                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);
                    Console.WriteLine("Subject: {0}", message.Subject);
                    Console.WriteLine("HTML BODY: {0}", message.HtmlBody);
                }

                client.Disconnect(true);
            }
        }
    }
}
