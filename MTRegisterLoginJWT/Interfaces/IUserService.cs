using MTRegisterLoginJWT.Data;
using MTRegisterLoginJWT.Data.Models;

namespace MTRegisterLoginJWT.Interfaces
{
	public interface IUserService
	{
		Task<ServiceResponse<ICollection<User>>> GetUsersAsync();
	}
}
