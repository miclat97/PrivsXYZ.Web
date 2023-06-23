namespace PrivsXYZ.MVC.Database.Entites
{
    public class PhotoEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string? UploaderIPAddress { get; set; }
        public string? UploaderHostname { get; set; }
        public byte[]? Photo { get; set; }
        public string? Salt { get; set; }
    }
}