namespace PrivsXYZ.MVC.Models
{
    public class MessageSendModel
    {
        public MessageSendModel(string? message, string? senderIPv4Address, string? senderHostname)
        {
            Message = message;
            SenderIPv4Address = senderIPv4Address;
            SenderHostname = senderHostname;
        }

        public string? Message { get; set; }
        public string? SenderIPv4Address { get; set; }
        public string? SenderHostname { get; set; }
    }
}
