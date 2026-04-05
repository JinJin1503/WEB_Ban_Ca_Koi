using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHasher { get; set; }
        public string Email { get; set; }
        public int FailedAttemptCount { get; set; } = 0; // Đếm số lần nhập sai (mặc định = 0)
        public bool IsLocked { get; set; } = false;      // Đánh dấu tài khoản bị khóa (mặc định = false)
    }
}
