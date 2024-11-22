using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using KoiFarmShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordHasher _passwordHasher;

		public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
		{
			_userRepository = userRepository;
			_passwordHasher = passwordHasher;
		}

		public async Task<bool> RegisterUserAsync(User user, string password)
		{
			if (await _userRepository.IsUserNameExistAsync(user.UserName))
			{
				return false; // Tên đăng nhập hoặc email đã tồn tại
			}

			user.PasswordHasher = _passwordHasher.HashPassword(password);
			await _userRepository.AddUserAsync(user);
			return true;
		}

		public async Task<User> LoginAsync(string userName, string password)
		{
			var user = await _userRepository.GetUserByUserNameAsync(userName);
			if (user != null && _passwordHasher.VerifyHashedPassword(user.PasswordHasher, password))
			{
				return user;
			}
			return null;
		}

		public async Task<bool> UpdateUserAsync(User user)
		{
			await _userRepository.UpdateUserAsync(user);
			return true;
		}

		public async Task<User> GetUserByIdAsync(int userId)
		{
			return await _userRepository.GetUserByIdAsync(userId);
		}

		public async Task<bool> DeleteUserAsync(int userId)
		{
			await _userRepository.DeleteUserAsync(userId);
			return true;
		}

		public async Task<bool> IsUserNameValidAsync(string userName)
		{
			return !await _userRepository.IsUserNameExistAsync(userName);
		}
		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			return await _userRepository.GetAllUsersAsync();
		}
		// Phương thức băm lại mật khẩu cho tất cả người dùng
		public async Task UpdatePasswordsForAllUsersAsync()
		{
			var users = await _userRepository.GetAllUsersAsync();
			foreach (var user in users)
			{
				if (!string.IsNullOrEmpty(user.PasswordHasher)) // Kiểm tra nếu mật khẩu chưa được băm
				{
					// Băm mật khẩu gốc và cập nhật vào cơ sở dữ liệu
					user.PasswordHasher = _passwordHasher.HashPassword(user.PasswordHasher);
					await _userRepository.UpdateUserAsync(user); // Cập nhật lại thông tin người dùng
				}
			}
		}
	}

}