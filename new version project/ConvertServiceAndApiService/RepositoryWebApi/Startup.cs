using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepositoryApplication.Common.Mappings;
using System.Reflection;
using RepositoryApplication.Interfases;
using RepositoryApplication;
using RepositoryPersistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RepositoryWebApi.Middlewere;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RepositoryWebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(IRepositoryDbContext).Assembly));
            });
            services.AddApplication();
            services.AddPersistence(Configuration);
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration["Jwt:Issuer"],
                       ValidAudience = Configuration["Jwt:Issuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))

                   };
               });
            services.AddMvc();

            services.AddVersionedApiExplorer(options =>
            options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
                ConfigureSwaggerOptions>();

            services.AddSwaggerGen();
            services.AddApiVersioning();


        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    config.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                    config.RoutePrefix = string.Empty;  
                }
            });

            app.UseCastomExeptionHandler();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiVersioning();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
