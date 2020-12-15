using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebTask.Common
{
	public static class ConfigurePipeline
	{
		private static IConfiguration Configuration { get; set; }

		public static IApplicationBuilder ConfigurePipelines(this IApplicationBuilder app, IConfiguration configuration,
			IWebHostEnvironment env)
		{
			Configuration = configuration;
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
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
			return app;
		}

    }
}
