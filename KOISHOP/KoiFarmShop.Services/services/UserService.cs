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

        private const int MAX_FAILED_ATTEMPTS = 5;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> RegisterUserAsync(User user, string password)
        {
            if (await _userRepository.IsUserNameExistAsync(user.UserName))
            {
                return false;
            }

            user.PasswordHasher = _passwordHasher.HashPassword(password);
            await _userRepository.AddUserAsync(user);
            return true;
        }

        public async Task<(User user, string errorMessage)> LoginAsync(string userName, string password)
        {
            var user = await _userRepository.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                return (null, "Tên đăng nhập không tồn tại.");
            }

            // 1. Kiểm tra tài khoản có bị khóa cứng không
            if (user.IsLocked)
            {
                return (null, "Tài khoản của bạn đã bị khóa do nhập sai mật khẩu nhiều lần. Vui lòng liên hệ quản trị viên.");
            }

            // 2. Kiểm tra mật khẩu
            if (_passwordHasher.VerifyHashedPassword(user.PasswordHasher, password))
            {
                // Đăng nhập thành công -> Reset lại số lần đếm sai về 0
                if (user.FailedAttemptCount > 0)
                {
                    user.FailedAttemptCount = 0;
                    await _userRepository.UpdateUserAsync(user);
                }
                return (user, string.Empty);
            }
            else
            {
                // Đăng nhập sai -> Tăng số đếm
                user.FailedAttemptCount++;

                // Nếu số lần sai đạt ngưỡng -> Khóa tài khoản
                if (user.FailedAttemptCount >= MAX_FAILED_ATTEMPTS)
                {
                    user.IsLocked = true;
                    await _userRepository.UpdateUserAsync(user);
                    return (null, "Tài khoản của bạn đã bị khóa do nhập sai mật khẩu quá 5 lần.");
                }

                // Cập nhật số lần sai vào database
                await _userRepository.UpdateUserAsync(user);
                return (null, $"Mật khẩu không chính xác. Bạn còn {MAX_FAILED_ATTEMPTS - user.FailedAttemptCount} lần thử.");
            }
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