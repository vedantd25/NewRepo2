using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
	public class AppIdentityDbContext : IdentityDbContext
	{
		public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
			: base(options)
		{
		}
	}
}
