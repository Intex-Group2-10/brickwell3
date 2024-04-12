using brickwell2.Data;
using brickwell2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential 
            // cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.ConsentCookieValue = "true";
        });
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

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSession();
        app.UseCookiePolicy();
        
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; script-src-attr 'self'; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdnjs.cloudflare.com; font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com; img-src 'self' data: https://www.lego.com https://images.brickset.com https://m.media-amazon.com https://live.staticflickr.com; frame-src 'self'; connect-src 'self' http://localhost:* wss://localhost:* ws://localhost:*; style-src-elem 'self' https://cdnjs.cloudflare.com https://fonts.googleapis.com;");
            await next();
        });
        
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

        //using (var scope = app.Services.CreateScope())
        //{
        //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //    var roles = new[] { "Admin", "Customer" };

        //    foreach (var role in roles)
        //    {
        //        if (!await roleManager.RoleExistsAsync(role))
        //            await roleManager.CreateAsync(new IdentityRole(role));
        //    }
        //}

        //using (var scope = app.Services.CreateScope())
        //{
        //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        //    string email = "admin@section210.com";
        //    string password = "Test12345!";

        //    // Check if the user already exists
        //    var user = await userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        // Create a new user
        //        user = new IdentityUser { UserName = email, Email = email };
        //        var createUserResult = await userManager.CreateAsync(user, password);

        //        // Check if user creation was successful
        //        if (createUserResult.Succeeded)
        //        {
        //            // User created successfully, now add to the "Admin" role
        //            var addToRoleResult = await userManager.AddToRoleAsync(user, "Admin");

        //            // Check if adding to role was successful
        //            if (!addToRoleResult.Succeeded)
        //            {
        //                // User role assignment failed, log the errors
        //                var roleErrors = addToRoleResult.Errors.Select(e => e.Description).ToList();
        //                string roleErrorMessages = string.Join(", ", roleErrors);
        //                throw new InvalidOperationException($"User created but adding user with email '{email}' to role 'Admin' failed. Details: {roleErrorMessages}");
        //            }
        //        }
        //        else
        //        {
        //            // User creation failed, log the errors
        //            var userErrors = createUserResult.Errors.Select(e => e.Description).ToList();
        //            string userErrorMessages = string.Join(", ", userErrors);
        //            throw new InvalidOperationException($"Unexpected error occurred creating user with email '{email}'. Details: {userErrorMessages}");
        //        }
        //    }
        //}


        app.Run();
    }
}