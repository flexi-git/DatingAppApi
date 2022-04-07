using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class ApiDataContext : DbContext
    {
        public ApiDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users => Set<AppUser>();
    }
}
