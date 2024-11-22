using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
    public interface IUserRepository
    {
        // Lấy tất cả người dùng
        Task<IEnumerable<User>> GetAllUsersAsync();

        // Lấy người dùng theo UserId
        Task<User> GetUserByIdAsync(int userId);

        // Lấy người dùng theo tên đăng nhập
        Task<User> GetUserByUserNameAsync(string userName);

        // Thêm người dùng mới
        Task AddUserAsync(User user);

        // Cập nhật thông tin người dùng
        Task UpdateUserAsync(User user);

        // Xóa người dùng
        Task DeleteUserAsync(int userId);

        // Kiểm tra nếu tên người dùng đã tồn tại
        Task<bool> IsUserNameExistAsync(string userName);


    }

}
