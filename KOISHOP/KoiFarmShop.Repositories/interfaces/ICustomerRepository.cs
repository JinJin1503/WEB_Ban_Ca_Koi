using KoiFarmShop.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFarmShop.Repositories.Interfaces
{
	public interface ICustomerRepository
	{
		Task<List<Customer>> GetCustomers();
		Task<Customer> GetCustomerById(int customerId);
		Task AddCustomer(Customer customer);
		Task UpdateCustomer(Customer customer);
		Task DeleteCustomer(int customerId);
	}                                                                                           
}
