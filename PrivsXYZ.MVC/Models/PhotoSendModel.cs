namespace PrivsXYZ.MVC.Models
{
    public class PhotoSendModel
    {
        public string? SenderIPv4Address { get; set; }
        public string? SenderHostname { get; set; }
        public byte[]? Photo { get; set; }
        public string? KeytoDecrypt { get; set; }
        public string? Salt { get; set; }
    }
}