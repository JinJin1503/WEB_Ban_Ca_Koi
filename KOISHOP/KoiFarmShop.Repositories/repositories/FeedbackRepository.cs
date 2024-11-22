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
	public class FeedbackRepository : IFeedbackRepository
	{
		private readonly KoiFarmDbContext _context;

		public FeedbackRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách tất cả Feedbacks
		public async Task<List<Feedback>> GetFeedbacks()
		{
			try
			{
				return await _context.Feedbacks.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving feedbacks.", ex);
			}
		}

		// Lấy Feedback theo ID
		public async Task<Feedback> GetFeedbackById(int feedbackId)
		{
			try
			{
				return await _context.Feedbacks
					.FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving feedback with ID {feedbackId}.", ex);
			}
		}

		// Thêm một Feedback mới
		public async Task AddFeedback(Feedback feedback)
		{
			try
			{
				await _context.Feedbacks.AddAsync(feedback);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the feedback.", ex);
			}
		}

		// Cập nhật một Feedback
		public async Task UpdateFeedback(Feedback feedback)
		{
			try
			{
				_context.Feedbacks.Update(feedback);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating feedback with ID {feedback.FeedbackId}.", ex);
			}
		}

		// Xóa Feedback theo ID
		public async Task DeleteFeedback(int feedbackId)
		{
			try
			{
				var feedback = await _context.Feedbacks
					.FirstOrDefaultAsync(f => f.FeedbackId == feedbackId);

				if (feedback != null)
				{
					_context.Feedbacks.Remove(feedback);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting feedback with ID {feedbackId}.", ex);
			}
		}
	}
}
