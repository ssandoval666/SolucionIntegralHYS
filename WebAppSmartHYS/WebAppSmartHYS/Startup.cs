using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using WebAppSmartHYS.Data;
using WebAppSmartHYS.Models;

namespace WebAppSmartHYS
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
			
			var oConfig = Configuration.GetSection(SmartSettings.SectionName);
			/*
			using (var httpClient = new HttpClient())
			{
				var cliente = new WebApiDS.WebApi("https://localhost:7245", httpClient);
				var oRta = cliente.SettingAsync("1").Result;

				oConfig["Version"] = oRta.Version;
				oConfig["App"] = oRta.App;
				oConfig["AppName"] = oRta.AppName;
				oConfig["AppFlavor"] = oRta.AppFlavor;
				oConfig["AppFlavorSubscript"] = oRta.AppFlavorSubscript;
			}
			*/
			services.Configure<SmartSettings>(oConfig);

			// Note: This line is for demonstration purposes only, I would not recommend using this as a shorthand approach for accessing settings
			// While having to type '.Value' everywhere is driving me nuts (>_<), using this method means reloaded appSettings.json from disk will not work
			services.AddSingleton(s => s.GetRequiredService<IOptions<SmartSettings>>().Value);

			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
			services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddRoleManager<RoleManager<IdentityRole>>()
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddTransient<IEmailSender, EmailSender>();

			/*
            services
                .AddControllersWithViews();

			*/

			services
				.AddControllersWithViews(options =>
				{
					var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
					options.Filters.Add(new AuthorizeFilter(policy));
				});
			
			services.AddRazorPages();
			services.AddBlazoredLocalStorage();


			services.AddDistributedMemoryCache();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddMemoryCache();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromSeconds(1800);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Identity/Account/Login";
				//options.LogoutPath = "/Identity/Account/Logout";
				options.AccessDeniedPath = "/Identity/Account/AccessDenied";
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseSession();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					"default",
                    "{controller=Main}/{action=Index}");
                endpoints.MapRazorPages();
			});


        }

		
    }
}
