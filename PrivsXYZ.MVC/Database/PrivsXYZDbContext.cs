using Microsoft.EntityFrameworkCore;
using PrivsXYZ.MVC.Database.Entites;

namespace PrivsXYZ.MVC.Database
{
    public class PrivsXYZDbContext : DbContext
    {
        public PrivsXYZDbContext(DbContextOptions<PrivsXYZDbContext> options) : base(options)
        {
        }

        public DbSet<MessageEntity> Message { get; set; }
    }
}
