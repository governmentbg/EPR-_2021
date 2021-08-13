using EPRO.Core.Contracts;
using EPRO.Core.Services;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data;
using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.Models.Identity;
using EPRO.Infrastructure.Data.Models.UserContext;
using EPRO.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace EPRO.Extensions
{
    /// <summary>
    /// Описва услугите и контекстите на приложението
    /// </summary>
    public static class IOWebAppServiceCollectionExtension
    {
        /// <summary>
        /// Регистрира услугите на приложението в  IoC контейнера
        /// </summary>
        /// <param name="services">Регистрирани услуги</param>
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsPrincipalFactory>();
            services.AddScoped<IDismissalService, DismissalService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICdnService, CdnService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<INomenclatureService, NomenclatureService>();
        }

        /// <summary>
        /// Регистрира контекстите на приложението в IoC контейнера
        /// </summary>
        /// <param name="services">Регистрирани услуги</param>
        /// <param name="Configuration">Настройки на приложението</param>
        public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                    m => m.MigrationsAssembly("EPRO.Infrastructure"))
                .UseSnakeCaseNamingConvention());

            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IRepository, Repository>();

            services.AddSingleton<IMongoClient>(s => new MongoClient(Configuration.GetConnectionString("MongoDbConnection")));
        }

        /// <summary>
        /// Регистрира Identity provider в IoC контейнера
        /// </summary>
        /// <param name="services">Регистрирани услуги</param>
        public static void AddApplicationIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedAccount = true;

            })
            .AddUserStore<ApplicationUserStore>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddDefaultTokenProviders();
        }
    }
}
