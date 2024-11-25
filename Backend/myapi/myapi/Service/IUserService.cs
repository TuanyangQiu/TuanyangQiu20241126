using myapi.Model;

namespace myapi.Service
{
	public interface IUserService
	{
		public Task<string> SaveUser(User  user);
	}
}
