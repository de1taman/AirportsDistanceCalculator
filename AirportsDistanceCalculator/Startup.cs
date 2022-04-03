using AirportsDistanceCalculator.ApplicationService;
using AirportsDistanceCalculator.Infrastructure;
using AirportsDistanceCalculator.Web.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace AirportsDistanceCalculator
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AirportsDistanceCalculator", Version = "v1" });
            });
            services.AddScoped<IAirportService, AirportService>();
            services.AddMemoryCache();
            services.AddHttpClient<AirportsServiceClient>(c=> 
            {
                c.BaseAddress = new Uri(Configuration["AirportServiceURL"]);
            });
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<V1GetDistanceRequestValidator>());
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirportsDistanceCalculator v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
