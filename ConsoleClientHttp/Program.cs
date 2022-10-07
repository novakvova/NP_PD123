using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ConsoleClientHttp
{
    class Program
    {
        private static string urlServer = "https://bv012.novakvova.com";
        public static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            string imgUser = "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";

            CreateUserDTO user = new CreateUserDTO
            {
                Email = "girl@gmail.com",
                SecondName = "Маркова",
                FirstName = "Амазонка",
                Phone = "+38(097)76-98-123",
                Photo = imgUser,
                Password = "123456",
                ConfirmPassword = "123456"
            };
            //RegisterUser(user);
            Console.WriteLine("Запуск клієнта!");
            ReadData();

        }
        public static void RegisterUser(CreateUserDTO userDTO)
        {
            string json = JsonConvert.SerializeObject(userDTO);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            WebRequest request = WebRequest.Create($"{urlServer}/api/account/register");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (Stream stream = request.GetRequestStream())
                stream.Write(bytes, 0, bytes.Length);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    string data = stream.ReadToEnd();
                    Console.WriteLine(data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ReadData()
        {
            WebRequest request = WebRequest.Create($"{urlServer}/api/account/users");
            request.Method = "GET";
            request.ContentType = "application/json";
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    string data = stream.ReadToEnd();
                    var users = JsonConvert.DeserializeObject<List<UserItemDTO>>(data);
                    foreach (var item in users)
                    {
                        Console.WriteLine("Email: " + item.Email);
                        Console.WriteLine("Phone: " + item.Phone);
                        Console.WriteLine("Image: " + urlServer + item.Photo);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
