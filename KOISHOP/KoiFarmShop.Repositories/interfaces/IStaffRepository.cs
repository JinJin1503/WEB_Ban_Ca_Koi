using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface IStaffRepository
	{
		Task<List<Staff>> GetStaffs();
		Task<Staff> GetStaffById(int staffId);
		Task AddStaff(Staff staff);
		Task UpdateStaff(Staff staff);
		Task DeleteStaff(int staffId);
	}
}
