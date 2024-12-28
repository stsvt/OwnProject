using API.Entities;
using Microsoft.EntityFrameworkCore;


// dotnet ef migrations add --project API\API.csproj --startup-project API\API.csproj --context API.Data.DataContext --configuration Debug first_migration --output-dir Data\Migrations
// dotnet ef database update --project API\API.csproj --startup-project API\API.csproj --context API.Data.DataContext --configuration Debug

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> UsersData { get; set; }
    }
}