using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Services.Interfaces
{
	public interface ICustomerService
	{
		Task<List<Customer>> GetAllCustomersAsync();
		Task<Customer> GetCustomerByIdAsync(int customerId);
		Task AddCustomerAsync(Customer customer);
		Task UpdateCustomerAsync(Customer customer);
		Task DeleteCustomerAsync(int customerId);
		Task ApplyLoyaltyPointsAsync(int customerId, int points); // Áp dụng điểm thưởng
		Task<Customer> GetCustomerByUserIdAsync(int userId);
    }
}
