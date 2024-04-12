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
<<<<<<< HEAD
            try
            {
                return new InferenceSession("intex_model.onnx");
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"Error loading the ONNX model: {ex.Message}");
                return null;
            }
        }
=======
            options.UseSqlServer(builder.Configuration["ConnectionStrings:LegoConnection"]);
        });
>>>>>>> parent of 4359518 (getting the fraud pred to work)

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

<<<<<<< HEAD
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.ConsentCookieValue = "true";
            });

            services.AddDbContext<LegoDbContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:LegoConnection"]);
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LegoDbContext>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddDbContext<LegoDbContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:LegoConnection"]);
            });
        services.Configure<IdentityOptions>(options =>
        {
            // Default Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 12;
            options.Password.RequiredUniqueChars = 3;
        });
        services.AddDbContext<LegoDbContext>(options =>
        {
            //options.UseSqlite(builder.Configuration["ConnectionStrings:LegoConnection"]);
            options.UseSqlServer(configuration["ConnectionStrings:LegoConnection"]);
        });

		builder.Services.AddDbContext<BrickwellDbContext>(options =>
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

            services.AddScoped<ILegoRepository, EFLegoRepository>();

            services.AddScoped<ILegoSecurityRepository, EFLegoSecurityRepository>();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void Configure(IApplicationBuilder app)
=======
        builder.Services.AddDbContext<LegoSecurityContext>(options =>
>>>>>>> parent of 4359518 (getting the fraud pred to work)
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
        
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; script-src-attr 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdnjs.cloudflare.com; font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com; img-src 'self' data: https://www.lego.com https://images.brickset.com https://m.media-amazon.com https://live.staticflickr.com; frame-src 'self'; connect-src 'self' http://localhost:* wss://localhost:* ws://localhost:*; style-src-elem 'self' https://cdnjs.cloudflare.com https://fonts.googleapis.com 'unsafe-inline';");
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