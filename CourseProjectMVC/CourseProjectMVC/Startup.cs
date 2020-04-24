using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Interfaces;
using CourseProjectMVC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using EasyCaching.Core;
using EasyCaching.Core.Configurations;

namespace CourseProjectMVC
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
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseNpgsql("Host = localhost; Database = ProductStores; Username = postgres; Password = postgres");
            });
            services.AddMvc();
            services.AddControllers().AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                }
            );

            services.AddRazorPages();

            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.Password.RequireLowercase = true;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireDigit = false;
                config.Password.RequiredLength = 5;
            })
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            services.ConfigureApplicationCookie(conf =>
            {
                conf.Cookie.Name = "Identity.Cookie";
                conf.LoginPath = "/api/auth/login";
            });

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "My API" });
            });

            services.AddEasyCaching(opt =>
            {
                opt.UseRedis(conf =>
                {
                    conf.DBConfig.Endpoints.Add(new ServerEndPoint("localhost", 6379));
                    conf.DBConfig.AllowAdmin = true;
                },
                  "redis1");
            });

            services.AddScoped<IRedisService,RedisService>();
            services.AddScoped<IÑurrencyService, CurrencyService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IStockService, StockServices>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
