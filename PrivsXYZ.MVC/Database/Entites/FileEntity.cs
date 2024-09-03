using System.ComponentModel.DataAnnotations;

namespace PrivsXYZ.MVC.Database.Entites
{
    public class FileEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UploaderIPAddress { get; set; }
        public string? UploaderHostname { get; set; }
        public byte[]? FileBytes { get; set; }
        public string? Salt { get; set; }
        public string? ViewerIPAddress { get; set; }
        public string? ViewerHostname { get; set; }
        public DateTime? OpenDate { get; set; }
    }
}
