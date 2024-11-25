
using myapi.Model;
using System.Text.Json;
using System.Linq;
namespace myapi.Service
{
	public class UserService : IUserService
	{
		private readonly IStorage<User> _storage;
		public UserService(IStorage<User> storage)
		{
			_storage = storage;
		}

		public async Task<string> SaveUser(User user)
		{
			user.FirstName = user.FirstName?.Trim();
			user.LastName = user.LastName?.Trim();

			List<User>? users = await _storage.ReadAsync();
			bool existed = users?.Any(x =>
	string.Equals(x.LastName, user.LastName, StringComparison.OrdinalIgnoreCase) &&
	string.Equals(x.FirstName, user.FirstName, StringComparison.OrdinalIgnoreCase)) == true;
			if (existed)
				return "The user is existed";


			await _storage.WriteAsync(user);
			return "";

		}
	}
}
