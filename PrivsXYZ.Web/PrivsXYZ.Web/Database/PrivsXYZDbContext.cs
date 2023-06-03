using Microsoft.EntityFrameworkCore;
using PrivsXYZ.Web.Database.Entity;

namespace PrivsXYZ.Web.Database
{
    public class PrivsXYZDbContext : DbContext
    {
        public PrivsXYZDbContext(DbContextOptions<PrivsXYZDbContext> options) : base(options)
        {
        }

        public DbSet<MessageEntity> Message { get; set; }
    }
}
