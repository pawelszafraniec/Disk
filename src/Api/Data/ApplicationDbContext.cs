using Disk.Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Disk.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Directory> Directories { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<File> Files { get; set; } = null!;
    }

}

