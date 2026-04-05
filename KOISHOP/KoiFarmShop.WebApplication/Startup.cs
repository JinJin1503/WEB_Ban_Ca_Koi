using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KoiFarmShop.Repositories.Interfaces;  
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.Services.Services;
using Microsoft.AspNetCore.Identity;
using KoiFarmShop.Services.Implementations;
using KoiFarmShop.Services;
using Microsoft.AspNetCore.Mvc;



using Microsoft.AspNetCore.Authentication.Cookies;

namespace KoiFarmShop.WebApplication
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Cấu hình DbContext
			services.AddDbContext<KoiFarmDbContext>(options =>
				options.UseSqlServer(Configuration["ConnectionStrings:ConnectedDb"],
					builder =>
					{
						// Giữ nguyên cấu hình thư mục Migration của bạn
						builder.MigrationsAssembly("KoiFarmShop.Repositories");


                        // Thêm "áo giáp" chống sập: Bắt ứng dụng thử kết nối lại tối đa 5 lần, mỗi lần chờ 30s
                        builder.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }));

						// Đăng ký repository
						services.AddScoped<IKoiFishRepository, KoiFishRepository>();
						services.AddScoped<IKoiCategoryRepository, KoiCategoryRepository>();
						// Đăng ký service
						services.AddScoped<IKoiFishService, KoiFishService>();  // Đăng ký dịch vụ IKoiFishService
						services.AddScoped<IConsignmentRequestService, ConsignmentRequestService>();
						services.AddScoped<ICareServiceService, CareServiceService>();
						services.AddScoped<IUserRepository, UserRepository>();
						services.AddScoped<IUserService, UserService>();
						services.AddScoped<ICustomerService, CustomerService>();
						services.AddScoped<IPasswordHasher, PasswordHasher>();
						services.AddScoped<ICartService, CartService>();
						services.AddScoped<IOrderService, OrderService>();
						services.AddScoped<IStaffService, StaffService>();
						services.AddScoped<IKoiCategoryService, KoiCategoryService>();

						// Đăng ký PasswordHasher
						services.AddScoped(typeof(IPasswordHasher), typeof(PasswordHasher));


						// Cấu hình session
						services.AddDistributedMemoryCache();
						services.AddSession(options =>
						{
							options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của session
							options.Cookie.HttpOnly = true;// Bảo mật cookie session
							options.Cookie.IsEssential = true; // Đảm bảo session hoạt động
						});

						// Cấu hình Authentication (Xác thực người dùng bằng Cookie)
						services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
							.AddCookie(options =>
							{

								options.LoginPath = "/Login/Login";
								options.AccessDeniedPath = "/AccessDenied"; // Đường dẫn khi không đủ quyền truy cập
							});

						// Các dịch vụ khác
						services.AddRazorPages();

					}

			// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
			public void Configure(IApplicationBuilder app, IWebHostEnvironment env, KoiFarmDbContext context)
			{
				if (env.IsDevelopment())
				{
					app.UseDeveloperExceptionPage();
				}
				else
				{
					app.UseExceptionHandler("/Error");
					// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
					app.UseHsts();
				}

				app.UseHttpsRedirection();
				app.UseStaticFiles();

				// Thêm đoạn này để chặn trình duyệt lưu Cache các trang bảo mật
				app.Use(async (context, next) =>
				{
					context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
					context.Response.Headers["Pragma"] = "no-cache";
					context.Response.Headers["Expires"] = "-1";
					await next();
				});

				app.UseSession();
				app.UseRouting();

				app.UseAuthentication();
				app.UseAuthorization();

				// Tự động chạy migration và chèn dữ liệu mẫu
				using (var scope = app.ApplicationServices.CreateScope())
				{
					var dbContext = scope.ServiceProvider.GetRequiredService<KoiFarmDbContext>();
					dbContext.Database.Migrate(); // Áp dụng migration mới nhất
				}

				app.UseEndpoints(endpoints =>
				{
					endpoints.MapRazorPages();


				});
			}
		}
	}
