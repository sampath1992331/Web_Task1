using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using Synapsys.AdminPortal.Core.Utility;
using WebTask.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebTask.Common
{
    public static class ConfigureService
    {
        private static IConfiguration Configuration { get; set; }

		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			Configuration = configuration;
            services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Insert(0, new AutoIntConverter());
                    opts.JsonSerializerOptions.Converters.Insert(1, new AutoDecimalConverter());
                    opts.JsonSerializerOptions.Converters.Insert(2, new AutoDoubleConverter());
                    opts.JsonSerializerOptions.Converters.Insert(3, new AutoNullableDateConverter());
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
            services.AddLocalization();
			services.InjectDipendancies();
            return services;
		}
		public static IServiceCollection InjectDipendancies(this IServiceCollection services)
		{
			services.AddSingleton<IUnitOfWork, UnitOfWork>(ctx =>
			{
				IConnectionFactory connectionFactory = new ConnectionFactory(Configuration.GetConnectionString("SqlConnectionString"));
				return new UnitOfWork(connectionFactory);
			});

			services.AddTransient<IMain, Main>();
		
			return services;
		}

    }
}
