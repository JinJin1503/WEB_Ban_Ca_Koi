using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface IFeedbackService
	{
		Task<List<Feedback>> GetAllFeedbacksAsync();
		Task<Feedback> GetFeedbackByIdAsync(int feedbackId);
		Task<List<Feedback>> GetFeedbacksByKoiIdAsync(int koiId);
		Task AddFeedbackAsync(Feedback feedback);
		Task UpdateFeedbackAsync(Feedback feedback);
		Task DeleteFeedbackAsync(int feedbackId);
		Task<double> GetAverageRatingAsync(int koiId); // Lấy đánh giá trung bình
	}
}
