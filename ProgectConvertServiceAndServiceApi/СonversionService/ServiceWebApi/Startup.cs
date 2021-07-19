using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicePersistence;
using System;
using System.IO;
using System.Reflection;
using LibraryApplication;

namespace ServiceWebApi
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddPersistence(configuration);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                  {
                      policy.AllowAnyHeader();
                      policy.AllowAnyMethod();
                      policy.AllowAnyOrigin();
                  });
            });



            services.AddControllers();
            services.AddDbContext<MyDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(configuration.GetConnectionString("main")));
            services.AddSwaggerGen(config =>
            {
                var xmlFile = $"{ Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.RoutePrefix = string.Empty;
                config.SwaggerEndpoint("swagger/v1/swagger.json", "Notes API");
            });

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors(builder => builder.AllowAnyOrigin());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
