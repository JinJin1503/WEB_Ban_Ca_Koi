using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface IFeedbackRepository
	{
		Task<List<Feedback>> GetFeedbacks();
		Task<Feedback> GetFeedbackById(int feedbackId);
		Task AddFeedback(Feedback feedback);
		Task UpdateFeedback(Feedback feedback);
		Task DeleteFeedback(int feedbackId);
	}
}
