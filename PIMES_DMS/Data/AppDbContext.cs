using Microsoft.EntityFrameworkCore;
using PIMES_DMS.Models;

namespace PIMES_DMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<IssueModel> IssueDb { get; set; }

        public DbSet<AccountsModel> AccountsDb { get; set; }
    }
}
