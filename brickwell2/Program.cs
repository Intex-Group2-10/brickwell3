using brickwell2.Data;
using brickwell2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Services.Configure<CookiePolicyOptions>(options =>
		{
			// This lambda determines whether user consent for non-essential 
			// cookies is needed for a given request.
			options.CheckConsentNeeded = context => true;
			options.MinimumSameSitePolicy = SameSiteMode.None;
			options.ConsentCookieValue = "true";
		});
		var services = builder.Services;
		var configuration = builder.Configuration;

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlServer(builder.Configuration["ConnectionStrings:LegoConnection"]);
		});

		builder.Services.AddDatabaseDeveloperPageExceptionFilter();

		builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();

		builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
		builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


		builder.Services.Configure<IdentityOptions>(options =>
		{
			// Default Password settings.
			options.Password.RequireDigit = true;
			options.Password.RequireLowercase = true;
			options.Password.RequireNonAlphanumeric = true;
			options.Password.RequireUppercase = true;
			options.Password.RequiredLength = 12;
			options.Password.RequiredUniqueChars = 3;
		});
		builder.Services.AddDbContext<LegoDbContext>(options =>
		{
			//options.UseSqlite(builder.Configuration["ConnectionStrings:LegoConnection"]);
			options.UseSqlServer(builder.Configuration["ConnectionStrings:LegoConnection"]);
		});

		builder.Services.AddDbContext<LegoSecurityContext>(options =>
		{
			options.UseSqlServer(builder.Configuration["ConnectionStrings:LegoConnection"]);
			//options.UseSqlite(builder.Configuration["ConnectionStrings:LegoSecurityConnection"]);
		});


		builder.Services.AddDbContext<BrickwellDbContext>(options =>
		{
			options.UseSqlServer(builder.Configuration["ConnectionStrings:LegoConnection"]);
			//options.UseSqlite(builder.Configuration["ConnectionStrings:LegoSecurityConnection"]);
		});


		services.AddAuthentication().AddGoogle(googleOptions =>
		{
			googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
			googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
		});

		builder.Services.AddScoped<ILegoRepository, EFLegoRepository>();

		builder.Services.AddScoped<ILegoSecurityRepository, EFLegoSecurityRepository>();


		builder.Services.AddDistributedMemoryCache();
		builder.Services.AddSession();

		builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
		builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		var app = builder.Build();
		// builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
		// builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		// var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseMigrationsEndPoint();
		}
		else
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}
		app.UseCookiePolicy();
		app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseSession();
		app.UseCookiePolicy();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		app.MapControllerRoute("pagenumandtype", "{productCategory}/{pageNum}",
			new { Controller = "Home", action = "Products" });
		app.MapControllerRoute("pagination", "{pageNum}",
			new { Controller = "Home", action = "Products", pageNum = 1 });
		app.MapControllerRoute("productCategory", "{productCategory}",
			new { Controller = "Home", action = "Products", pageNum = 1 });


		app.MapRazorPages();
		app.Run();
	}
}