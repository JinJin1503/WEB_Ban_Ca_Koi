using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface IReportService
	{
		Task<List<Report>> GetReportsAsync();
		Task<Report> GetReportByIdAsync(int reportId);
		Task AddReportAsync(Report report);
		Task UpdateReportAsync(Report report);
		Task DeleteReportAsync(int reportId);
	}
}
