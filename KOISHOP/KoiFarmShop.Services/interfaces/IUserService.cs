using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
    public interface IUserService
    {
        // Đăng ký người dùng mới
        Task<bool> RegisterUserAsync(User user, string password);

        // Đăng nhập người dùng
        Task<User> LoginAsync(string userName, string password);

        // Cập nhật thông tin người dùng
        Task<bool> UpdateUserAsync(User user);

        // Lấy thông tin người dùng
        Task<User> GetUserByIdAsync(int userId);

        // Xóa người dùng
        Task<bool> DeleteUserAsync(int userId);

        // Kiểm tra tên đăng nhập hợp lệ
        Task<bool> IsUserNameValidAsync(string userName);

        // Lấy danh sách tất cả người dùng
        Task<IEnumerable<User>> GetAllUsersAsync();
    }

}
