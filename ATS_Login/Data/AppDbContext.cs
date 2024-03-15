using ATS_Login.Models;
using Microsoft.EntityFrameworkCore;

namespace ATS_Login.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<AccountsModel> Accounts { get; set; }

        public DbSet<SessionModel> Sessions { get; set; }
    }
}
