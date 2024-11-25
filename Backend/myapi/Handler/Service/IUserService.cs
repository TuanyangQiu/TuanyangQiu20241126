using Handler.Model;

namespace Handler.Service
{
	public interface IUserService
	{
		public Task<string> SaveUser(User  user);
	}
}
