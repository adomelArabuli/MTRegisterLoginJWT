using Microsoft.EntityFrameworkCore;
using MTRegisterLoginJWT.Data;
using MTRegisterLoginJWT.Data.Models;
using MTRegisterLoginJWT.Interfaces;

namespace MTRegisterLoginJWT.Services
{
	public class UserService : IUserService
	{
		private readonly ApplicationDbContext _db;

		public UserService(ApplicationDbContext context)
		{
			_db = context;
		}

		public async Task<ServiceResponse<ICollection<User>>> GetUsersAsync()
		{
			var users = await _db.Users.ToListAsync();
			return new() { Data = users};
		}
	}
}
