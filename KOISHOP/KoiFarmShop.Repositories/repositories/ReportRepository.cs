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
	public class ReportRepository : IReportRepository
	{
		private readonly KoiFarmDbContext _context;

		public ReportRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách tất cả các báo cáo
		public async Task<List<Report>> GetReports()
		{
			try
			{
				return await _context.Reports.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving reports.", ex);
			}
		}

		// Lấy báo cáo theo ID
		public async Task<Report> GetReportById(int reportId)
		{
			try
			{
				return await _context.Reports
					.FirstOrDefaultAsync(r => r.ReportId == reportId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving report with ID {reportId}.", ex);
			}
		}

		// Thêm một báo cáo mới
		public async Task AddReport(Report report)
		{
			try
			{
				await _context.Reports.AddAsync(report);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the report.", ex);
			}
		}

		// Cập nhật một báo cáo
		public async Task UpdateReport(Report report)
		{
			try
			{
				_context.Reports.Update(report);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating report with ID {report.ReportId}.", ex);
			}
		}

		// Xóa báo cáo theo ID
		public async Task DeleteReport(int reportId)
		{
			try
			{
				var report = await _context.Reports
					.FirstOrDefaultAsync(r => r.ReportId == reportId);

				if (report != null)
				{
					_context.Reports.Remove(report);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting report with ID {reportId}.", ex);
			}
		}
	}
}
