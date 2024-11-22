using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Repositories
{
	public class StaffRepository : IStaffRepository
	{
		private readonly KoiFarmDbContext _context;

		public StaffRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách tất cả Staffs
		public async Task<List<Staff>> GetStaffs()
		{
			try
			{
				return await _context.Staffs.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving staff members.", ex);
			}
		}

		// Lấy Staff theo ID
		public async Task<Staff> GetStaffById(int staffId)
		{
			try
			{
				return await _context.Staffs
					.FirstOrDefaultAsync(s => s.StaffId == staffId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving staff with ID {staffId}.", ex);
			}
		}

		// Thêm một Staff mới
		public async Task AddStaff(Staff staff)
		{
			try
			{
				await _context.Staffs.AddAsync(staff);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the staff member.", ex);
			}
		}

		// Cập nhật một Staff
		public async Task UpdateStaff(Staff staff)
		{
			try
			{
				_context.Staffs.Update(staff);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating staff with ID {staff.StaffId}.", ex);
			}
		}

		// Xóa Staff theo ID
		public async Task DeleteStaff(int staffId)
		{
			try
			{
				var staff = await _context.Staffs
					.FirstOrDefaultAsync(s => s.StaffId == staffId);

				if (staff != null)
				{
					_context.Staffs.Remove(staff);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting staff with ID {staffId}.", ex);
			}
		}
	}
}
