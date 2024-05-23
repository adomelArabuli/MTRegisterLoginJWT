using MTRegisterLoginJWT.Data;
using MTRegisterLoginJWT.Data.DTO;

namespace MTRegisterLoginJWT.Interfaces
{
	public interface IAuthService
	{
		Task<ServiceResponse<int>> Register(UserRegisterDTO model);
		Task<ServiceResponse<string>> Login(UserLoginDTO model);
	}
}
