using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using KoiFarmShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFarmShop.Services
{
	public class StaffService : IStaffService
	{
		private readonly KoiFarmDbContext _context;

		// Constructor nhận vào KoiFarmContext
		public StaffService(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy nhân viên theo UserId
		public async Task<Staff> GetStaffByUserIdAsync(int userId)
		{
			return await _context.Staffs.FirstOrDefaultAsync(s => s.UserId == userId);
		}

		// Lấy tất cả nhân viên
		public async Task<List<Staff>> GetAllStaffAsync()
		{
			return await _context.Staffs.ToListAsync();
		}

		// Lấy nhân viên theo ID
		public async Task<Staff> GetStaffByIdAsync(int staffId)
		{
			return await _context.Staffs.FirstOrDefaultAsync(s => s.StaffId == staffId);
		}

		// Thêm một nhân viên mới
		public async Task AddStaffAsync(Staff staff)
		{
			await _context.Staffs.AddAsync(staff);
			await _context.SaveChangesAsync();
		}

		// Cập nhật nhân viên
		public async Task UpdateStaffAsync(Staff staff)
		{
			_context.Staffs.Update(staff);
			await _context.SaveChangesAsync();
		}

		// Xóa nhân viên theo ID
		public async Task DeleteStaffAsync(int staffId)
		{
			var staff = await _context.Staffs.FindAsync(staffId);
			if (staff != null)
			{
				_context.Staffs.Remove(staff);
				await _context.SaveChangesAsync();
			}
		}

		// Lấy nhân viên theo vai trò
		public async Task<List<Staff>> GetStaffByRoleAsync(string role)
		{
			return await _context.Staffs
				.Where(s => s.Role.Equals(role, StringComparison.OrdinalIgnoreCase))
				.ToListAsync();
		}
	}
}
