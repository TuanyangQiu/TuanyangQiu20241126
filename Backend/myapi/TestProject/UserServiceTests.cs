using Handler.Model;
using Handler.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
	public class UserServiceTests
	{
		private readonly Mock<IStorage<User>> _mockStorage;
		private readonly IUserService _userService;

		public UserServiceTests()
		{
			_mockStorage = new Mock<IStorage<User>>();
			_userService = new UserService(_mockStorage.Object);
		}

		[Fact]
		public async Task SaveUser_NewUser_ReturnsEmptyStringAndWritesUser()
		{

			var newUser = new User { FirstName = "John", LastName = "Doe" };

			//	Simulate ReadAsync to return that there is no user currently
			_mockStorage.Setup(s => s.ReadAsync()).ReturnsAsync(new List<User>());

			var result = await _userService.SaveUser(newUser);

			Assert.Equal(string.Empty, result);
			_mockStorage.Verify(s => s.WriteAsync(It.Is<User>(
				u => u.FirstName == newUser.FirstName && u.LastName == newUser.LastName
			)), Times.Once);
		}

		[Fact]
		public async Task SaveUser_ExistingUser_ReturnsExistedMessageAndDoesNotWriteUser()
		{
			// Arrange
			var existingUser = new User { FirstName = "Jane", LastName = "Smith" };
			var newUser = new User { FirstName = "Jane", LastName = "Smith" };

			// Simulate ReadAsync to return an existing user
			_mockStorage.Setup(s => s.ReadAsync()).ReturnsAsync(new List<User> { existingUser });

			// Act
			var result = await _userService.SaveUser(newUser);

			// Assert
			Assert.Equal("The user is existed", result);
			_mockStorage.Verify(s => s.WriteAsync(It.IsAny<User>()), Times.Never);
		}


		[Fact]
		public async Task SaveUser_UserWithWhitespace_TrimsNamesBeforeSaving()
		{

			var newUser = new User { FirstName = "  Alice  ", LastName = "  Wonderland  " };
			//Simulate ReadAsync to return that there is no user currently
			_mockStorage.Setup(s => s.ReadAsync()).ReturnsAsync(new List<User>());


			var result = await _userService.SaveUser(newUser);

			Assert.Equal(string.Empty, result);
			_mockStorage.Verify(s => s.WriteAsync(It.Is<User>(
				u => u.FirstName == "Alice" && u.LastName == "Wonderland"
			)), Times.Once);
		}


		[Fact]
		public async Task SaveUser_NullNames_TrimsNamesBeforeSaving()
		{
			var newUser = new User { FirstName = null, LastName = "  Wonderland  " };

			//Simulate ReadAsync to return that there is no user currently
			_mockStorage.Setup(s => s.ReadAsync()).ReturnsAsync(new List<User>());

			var result = await _userService.SaveUser(newUser);

			Assert.Equal(string.Empty, result);
			_mockStorage.Verify(s => s.WriteAsync(It.Is<User>(
				u => u.FirstName == null && u.LastName == "Wonderland"
			)), Times.Once);
		}

		[Fact]
		public async Task SaveUser_StorageReadThrowsException_PropagatesException()
		{
			var newUser = new User { FirstName = "John", LastName = "Doe" };

			//Simulate ReadAsync throwing an exception
			_mockStorage.Setup(s => s.ReadAsync()).ThrowsAsync(new Exception("Read error"));

			var exception = await Assert.ThrowsAsync<Exception>(() => _userService.SaveUser(newUser));
			Assert.Equal("Read error", exception.Message);
			_mockStorage.Verify(s => s.WriteAsync(It.IsAny<User>()), Times.Never);
		}


		[Fact]
		public async Task SaveUser_StorageWriteThrowsException_PropagatesException()
		{
			var newUser = new User { FirstName = "John", LastName = "Doe" };
			//Simulate ReadAsync to return that there is no user currently
			_mockStorage.Setup(s => s.ReadAsync()).ReturnsAsync(new List<User>());
			//Simulate ReadAsync throwing an exception
			_mockStorage.Setup(s => s.WriteAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Write error"));

			var exception = await Assert.ThrowsAsync<Exception>(() => _userService.SaveUser(newUser));
			Assert.Equal("Write error", exception.Message);
			_mockStorage.Verify(s => s.WriteAsync(It.IsAny<User>()), Times.Once);
		}
	}
}
