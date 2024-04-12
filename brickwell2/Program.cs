using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Threading.Tasks;
using brickwell2.Data;
using brickwell2.Models;

namespace brickwell2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Load the ONNX model
            var session = LoadModel();

            var builder = WebApplication.CreateBuilder(args);

            // Configure the services
            ConfigureServices(builder.Services, builder.Configuration, session);

            var app = builder.Build();

            // Configure the HTTP request pipeline
            Configure(app);

            await app.RunAsync();
        }

        private static InferenceSession LoadModel()
        {
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

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, InferenceSession session)
        {
            services.AddSingleton(session); // Register the session as a singleton
            services.AddControllersWithViews();
            // Register ZooContext with the DI container
            // services.AddDbContext<LegoDbContext>(options =>
            //     options.UseSqlite(configuration.GetConnectionString("IntexDatabase")));

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
                //options.UseSqlite(builder.Configuration["ConnectionStrings:LegoConnection"]);
                options.UseSqlServer(configuration["ConnectionStrings:LegoConnection"]);
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:LegoConnection"]);
            });
            
            services.AddDbContext<LegoSecurityContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:LegoConnection"]);
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            // services.AddDbContext<LegoDbContext>(options =>
            // {
            //     options.UseSqlServer(configuration["ConnectionStrings:LegoConnection"]);
            // });
            
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
        {
            // if (app.Environment.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            //     app.UseMigrationsEndPoint();
            // }
            // else
            // {
            //     app.UseExceptionHandler("/Home/Error");
            //     app.UseHsts();
            // }
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy",
                    "default-src 'self'; script-src 'self' 'unsafe-inline'; script-src-attr 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdnjs.cloudflare.com; font-src 'self' https://fonts.gstatic.com https://cdnjs.cloudflare.com; img-src 'self' data: https://www.lego.com https://images.brickset.com https://m.media-amazon.com https://live.staticflickr.com; frame-src 'self'; connect-src 'self' http://localhost:* wss://localhost:* ws://localhost:*; style-src-elem 'self' https://cdnjs.cloudflare.com https://fonts.googleapis.com 'unsafe-inline';");
                await next();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("pagenumandtype", "{productCategory}/{pageNum}",
                    new { Controller = "Home", action = "Products" });
                endpoints.MapControllerRoute("pagination", "{pageNum}",
                    new { Controller = "Home", action = "Products", pageNum = 1 });
                endpoints.MapControllerRoute("productCategory", "{productCategory}",
                    new { Controller = "Home", action = "Products", pageNum = 1 });

                endpoints.MapRazorPages();
            });
        }
    }
}
