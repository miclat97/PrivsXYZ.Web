using Microsoft.EntityFrameworkCore;
using PrivsXYZ.Web.Database.Entity;

namespace PrivsXYZ.Web.Database
{
    public class PrivsXYZDbContext : DbContext
    {
        public DbSet<MessageEntity> Message { get; set; }
    }
}
