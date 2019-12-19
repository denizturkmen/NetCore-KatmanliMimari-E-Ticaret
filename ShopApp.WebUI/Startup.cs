using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.BusinessLayer.Abstract;
using ShopApp.BusinessLayer.Concrete;
using ShopApp.DataAccessLayer.Abstract;
using ShopApp.DataAccessLayer.Concrete.EntityFrameWork;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Middlewares;

namespace ShopApp.WebUI
{
    public class Startup
    {
        //appsettings dosyasından bilgi almak için
        //appsettings clasik  mvc web.config eş değer
        public IConfiguration Configuration { get; set; }

        //Inject işlemi dışarıdan geleni almnak
        // appsettings.json içindekileri almak için
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Dependecy Injection burda ayarlıyoruz
            // IProduct, EfCoreProductDal
            // IProduct, MySqlProductDal yapıyı değiştirdiğin sadece burası değişcek
            //IproductDal çağrıldığında MemoryProductDal bu class işlemler yapılcak
            //Yapıyı tamamem Interfaceler üzerinden yürütecez
            //services.AddScoped<IProductDAL, MemoryProductDal>();

            //appsettings connection string almak için
            //Hangi DAtabase provider kullancağını söylüyoruz
            // https://docs.microsoft.com/tr-tr/ef/core/providers/?tabs=dotnet-core-cli
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            //token reset-confirm için benzersiz token veriyor
            // AddEntityFrameworkStores vwrileri saklayacağı yer
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            //Kullanıcı ile ilgili kısıtlamalar
            services.Configure<IdentityOptions>(options =>
            {
                // password
                options.Password.RequireDigit = true; //sayısal değer olsun
                options.Password.RequireLowercase = true; //küçük harf içersin
                options.Password.RequiredLength = 6; //minumun uzunluğu
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;

                //Lock
                options.Lockout.MaxFailedAccessAttempts = 5; //5 yanlıştan sonra kilitler
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // ne kadar kilitliyeceği
                options.Lockout.AllowedForNewUsers = true; //yeni kullanıcılar içinde geçerli lock

                // options.User.AllowedUserNameCharacters = ""; // özel karakterler belirleyebilirsin alfabeden
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true; // onay maili gelsin
                options.SignIn.RequireConfirmedPhoneNumber = false; // onay telefona gelsin
            });

            //Cookie ayarlarları(tarayıcda tutulan)
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // tarayıcıda ne kadar süre kalsın
                options.SlidingExpiration = true;

                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie",
                    //Csrf engellemek için ama controller tarafında validate et token ı
                    SameSite = SameSiteMode.Strict 
                };

            });

            //birbiriden bağımsız yapılar Dependecy Injection
            services.AddScoped<IProductDAL, EfCoreProductDAL>();
            services.AddScoped<ICategoryDAL, EfCoreCategoryDAL>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICartDAL, EfCoreCartDAL>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService, OrderManager>();
            services.AddScoped<IOrderDAL, EfCoreOrderDal>();

            //Email send-confirm 
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //Test verileri çalıştırmak için
                SeedDatabase.Seed();
            }

            //DefaultMcx yönlednirme
            //app.UseMvcWithDefaultRoute();

            //static olan dosyaları açmak için erişme node_modules
            //burada değilde class yazıp açtık
            app.UseStaticFiles();
            app.CustomStaticFiles();

            //useMvc önce olmak zorunda 
            app.UseAuthentication();
            
            //Yönlendirme 
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "products",
                    template: "products/{category?}",
                    defaults: new { controller = "Shop", action = "List" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "adminProducts",
                    template: "admin/products",
                    defaults: new { controller = "Admin", action = "ProductList" }
                );

                routes.MapRoute(
                    name: "adminProducts",
                    template: "admin/products/{id?}",
                    defaults: new { controller = "Admin", action = "Editproduct" }
                );

                routes.MapRoute(
                    name: "cart",
                    template: "cart",
                    defaults: new { controller = "Cart", action = "Index" }
                );

                routes.MapRoute(
                    name: "checkout",
                    template: "checkout",
                    defaults: new { controller = "Cart", action = "Checkout" }
                );

                routes.MapRoute(
                    name: "orders",
                    template: "orders",
                    defaults: new { controller = "Cart", action = "GetOrders" }
                );

            });
            //aseknron olduğu için wait() ekle
            SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();





        }
    }
}
