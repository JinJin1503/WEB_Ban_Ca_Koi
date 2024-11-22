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
	public class CustomerRepository : ICustomerRepository
	{
		private readonly KoiFarmDbContext _context;

		public CustomerRepository(KoiFarmDbContext context)
		{
			_context = context;
		}

		// Lấy danh sách tất cả khách hàng
		public async Task<List<Customer>> GetCustomers()
		{
			try
			{
				return await _context.Customers.ToListAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while retrieving customers.", ex);
			}
		}

		// Lấy thông tin khách hàng theo ID
		public async Task<Customer> GetCustomerById(int customerId)
		{
			try
			{
				return await _context.Customers
					.FirstOrDefaultAsync(c => c.CustomerId == customerId);
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while retrieving customer with ID {customerId}.", ex);
			}
		}

		// Thêm khách hàng mới
		public async Task AddCustomer(Customer customer)
		{
			try
			{
				await _context.Customers.AddAsync(customer);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception("An error occurred while adding the new customer.", ex);
			}
		}

		// Cập nhật thông tin khách hàng
		public async Task UpdateCustomer(Customer customer)
		{
			try
			{
				_context.Customers.Update(customer);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while updating customer with ID {customer.CustomerId}.", ex);
			}
		}

		// Xóa khách hàng theo ID
		public async Task DeleteCustomer(int customerId)
		{
			try
			{
				var customer = await _context.Customers
					.FirstOrDefaultAsync(c => c.CustomerId == customerId);

				if (customer != null)
				{
					_context.Customers.Remove(customer);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ nếu cần
				throw new Exception($"An error occurred while deleting customer with ID {customerId}.", ex);
			}
		}
	}
}
