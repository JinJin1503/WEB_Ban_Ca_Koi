using System;
using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Repositories.Interfaces;
using KoiFarmShop.Repositories.Repositories;
using KoiFarmShop.Services;
using KoiFarmShop.Services.Implementations;
using KoiFarmShop.Services.Interfaces;
using KoiFarmShop.Services.Services;
using KoiFarmShop.WebApplication.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KoiFarmShop.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<KoiFarmDbContext>(options =>
                options.UseSqlServer(
                    Configuration["ConnectionStrings:ConnectedDb"],
                    builder =>
                    {
                        builder.MigrationsAssembly("KoiFarmShop.Repositories");
                        builder.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }));

            services.AddScoped<IKoiFishRepository, KoiFishRepository>();
            services.AddScoped<IKoiCategoryRepository, KoiCategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IKoiFishService, KoiFishService>();
            services.AddScoped<IConsignmentRequestService, ConsignmentRequestService>();
            services.AddScoped<ICareServiceService, CareServiceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IKoiCategoryService, KoiCategoryService>();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Login";
                    options.AccessDeniedPath = "/AccessDenied";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicies.ManagerOnly, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(AppRoles.Manager);
                });

                options.AddPolicy(AppPolicies.StaffOrManager, policy =>
                    policy.RequireRole(AppRoles.Manager, AppRoles.Staff));

                options.AddPolicy(AppPolicies.CustomerOnly, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(AppRoles.Customer);
                    policy.RequireClaim(AppClaimTypes.CustomerId);
                });
            });

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/cart", AppPolicies.CustomerOnly);
                options.Conventions.AuthorizeFolder("/Profile");
                options.Conventions.AuthorizeFolder("/manager", AppPolicies.StaffOrManager);
                options.Conventions.AuthorizePage("/Product/Create", AppPolicies.ManagerOnly);
                options.Conventions.AuthorizePage("/Product/Edit", AppPolicies.ManagerOnly);
                options.Conventions.AuthorizePage("/Product/Delete", AppPolicies.ManagerOnly);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "-1";
                await next();
            });

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<KoiFarmDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
