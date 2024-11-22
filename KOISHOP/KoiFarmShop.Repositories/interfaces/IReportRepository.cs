using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface IReportRepository
	{
		Task<List<Report>> GetReports();
		Task<Report> GetReportById(int reportId);
		Task AddReport(Report report);
		Task UpdateReport(Report report);
		Task DeleteReport(int reportId);
	}
}
