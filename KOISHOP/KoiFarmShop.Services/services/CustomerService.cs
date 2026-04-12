using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using KoiFarmShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KoiFarmShop.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetCustomers();
        }

        // Lấy khách hàng theo mã CustomerId
        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _customerRepository.GetCustomerById(customerId);
        }

        // Lấy khách hàng theo mã UserId (tài khoản đăng nhập)
        public async Task<Customer> GetCustomerByUserIdAsync(int userId)
        {
            var customers = await _customerRepository.GetCustomers();
            return customers.FirstOrDefault(c => c.UserId == userId);
        }

        // Thêm khách hàng mới
        public async Task AddCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null");
            }
            customer.RegistrationDate = DateTime.Now;

            await _customerRepository.AddCustomer(customer);
        }

        // Cập nhật thông tin khách hàng
        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null");
            }

            // 1. Dùng Repository tìm khách hàng cũ trong Database
            var existingCustomer = await _customerRepository.GetCustomerById(customer.CustomerId);

            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customer.CustomerId} not found.");
            }

            // 2. Cập nhật các trường thông tin
            existingCustomer.CustomerName = customer.CustomerName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Address = customer.Address;

            // 3. Gọi Repository để lưu thay đổi
            await _customerRepository.UpdateCustomer(existingCustomer);
        }

        // Xóa khách hàng theo ID
        public async Task DeleteCustomerAsync(int customerId)
        {
            // Repository của bạn đã tự xử lý logic tìm và xóa rồi, chỉ cần gọi nó
            await _customerRepository.DeleteCustomer(customerId);
        }

        // Áp dụng điểm thưởng cho khách hàng
        public async Task ApplyLoyaltyPointsAsync(int customerId, int points)
        {
            // 1. Tìm khách hàng
            var customer = await _customerRepository.GetCustomerById(customerId);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }

            // 2. Cộng điểm
            customer.Points += points;

            // 3. Gọi Update của Repository để lưu lại
            await _customerRepository.UpdateCustomer(customer);
        }
    }
}