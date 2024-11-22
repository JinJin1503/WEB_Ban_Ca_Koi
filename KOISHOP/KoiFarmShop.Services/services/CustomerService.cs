using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoiFarmShop.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly KoiFarmDbContext _context;

        public CustomerService(KoiFarmDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả khách hàng
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByUserIdAsync(int userId)
        {
            return await _context.Customers
                                 .FirstOrDefaultAsync(c => c.UserId == userId);
        }


        // Lấy thông tin khách hàng theo ID
        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers
                                 .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        // Thêm khách hàng mới
        public async Task AddCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null");
            }

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin khách hàng
        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null");
            }

            var existingCustomer = await _context.Customers
                                                  .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customer.CustomerId} not found.");
            }

            // Cập nhật thông tin khách hàng
            existingCustomer.CustomerName = customer.CustomerName;
            existingCustomer.Email = customer.Email;

            _context.Customers.Update(existingCustomer);
            await _context.SaveChangesAsync();
        }

        // Xóa khách hàng theo ID
        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Customers
                                         .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        // Áp dụng điểm thưởng cho khách hàng
        public async Task ApplyLoyaltyPointsAsync(int customerId, int points)
        {
            var customer = await _context.Customers
                                         .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }

            // Cập nhật điểm thưởng
            customer.Points += points;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

       
    }
}
