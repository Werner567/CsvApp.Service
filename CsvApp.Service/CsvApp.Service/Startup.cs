
using CsvApp.Service.Models;
using CsvApp.Service.Options;
using CsvApp.Service.Repos;
using CsvApp.Service.Services.CsvDataLoaderWorker;
using CsvApp.Service.Services.CsvLoaderService;
using CsvApp.Service.Services.VehicleService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace CsvApp.Service
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc();
            services.Configure<ServiceOptions>(_configuration.GetSection(nameof(ServiceOptions)));
            services.Configure<SqliteOptions>(_configuration.GetSection(nameof(SqliteOptions)));
            services.Configure<VehicleOptions>(_configuration.GetSection(nameof(VehicleOptions)));
            
            


            services.AddDbContext<AppDbContext>(options =>
            {
                var sqliteOptions = _configuration.GetSection("SqliteOptions").Get<SqliteOptions>();
                options.UseSqlite(sqliteOptions.DefaultConnection);
            });
            services.AddScoped<IDbRepo, DbRepo>();
            services.AddScoped<ICsvLoaderService, CsvLoaderService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddSingleton<IHostedService, CsvDataLoaderWorker>();
            services.AddHostedService<CsvDataLoaderWorker>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CSVApp", Version = "v1" });
            });
            //



        }

       

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CSVApp V1");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Make sure this line is present
            });

        }
    }
}
