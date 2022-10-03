namespace TCPConaoleLib
{
    public enum TypeMessage
    {
        Login,
        Logout,
        Message
    }
    public class ChatMessage
    {
        public TypeMessage MessageType { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }

        public byte[] Serialize()
        {
            using(MemoryStream ms = new MemoryStream())
            {
                using(BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((int)MessageType);
                    bw.Write(UserId);
                    bw.Write(UserName);
                    bw.Write(Text);
                }
                return ms.ToArray();
            }
        }

        public static ChatMessage Deserialize(byte[] data)
        {
            ChatMessage message = new ChatMessage();
            using(MemoryStream m = new MemoryStream(data))
            {
                using(BinaryReader br = new BinaryReader(m))
                {
                    message.MessageType = (TypeMessage)br.ReadInt32();
                    message.UserId = br.ReadString();
                    message.UserName = br.ReadString();
                    message.Text = br.ReadString();
                }
            }
            return message;
        }
    }
}