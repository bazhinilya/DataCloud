using DbLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DbLayer.Conext
{
    public class DbLayerContext : DbContext
    {
        public DbSet<FileData> FileDatas { get; set; }
        public DbSet<Extension> Extensions { get; set; }
        public DbLayerContext(DbContextOptions<DbLayerContext> options) : base(options)
        {
            Database.EnsureCreated();
            Database.SetCommandTimeout(int.MaxValue);
        }
    }
}