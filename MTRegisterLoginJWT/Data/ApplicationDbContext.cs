using Microsoft.EntityFrameworkCore;
using MTRegisterLoginJWT.Data.Models;

namespace MTRegisterLoginJWT.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
    }
}
