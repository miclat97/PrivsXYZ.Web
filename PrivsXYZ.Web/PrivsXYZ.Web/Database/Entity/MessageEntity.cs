using System.ComponentModel.DataAnnotations;

namespace PrivsXYZ.Web.Database.Entity
{
    public class MessageEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Message { get; set; }
        public byte[]? Salt { get; set; }
        public DateTime CreateDate { get; set; }
        public string? UploaderIPAddress { get; set; }
        public string? UploaderHostname { get; set; }
        public string? ViewerIPAddress { get; set; }
        public string? ViewerHostname { get; set; }
        public DateTime OpenDate { get; set; }
    }
}
