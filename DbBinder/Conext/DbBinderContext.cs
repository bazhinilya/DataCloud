using DbBinder.Models;
using Microsoft.EntityFrameworkCore;

namespace DbBinder.Conext
{
    public class DbBinderContext : DbContext
    {
        public DbSet<FileData> FileDatas { get; set; }
        public DbSet<Extension> Extensions { get; set; }
        public DbBinderContext(DbContextOptions<DbBinderContext> options) : base(options)
        {
            Database.EnsureCreated();
            Database.SetCommandTimeout(50);
        }
    }
}