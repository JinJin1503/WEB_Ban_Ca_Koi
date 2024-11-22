using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password); // Băm mật khẩu
        bool VerifyHashedPassword(string hashedPassword, string password); // Kiểm tra mật khẩu có khớp với mật khẩu đã băm
    }

}
