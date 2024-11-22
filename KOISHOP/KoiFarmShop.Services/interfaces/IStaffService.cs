using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface IStaffService
	{
		Task<List<Staff>> GetAllStaffAsync();
		Task<Staff> GetStaffByIdAsync(int staffId);
		Task AddStaffAsync(Staff staff);
		Task UpdateStaffAsync(Staff staff);
		Task DeleteStaffAsync(int staffId);
		Task<Staff> GetStaffByUserIdAsync(int userId);
		Task<List<Staff>> GetStaffByRoleAsync(string role); // Lấy nhân viên theo vai trò
	}
}
