using IO.LogOperation.Service;
using EPRO.Core.Contracts;
using EPRO.Core.Services;
using EPRO.Infrastructure.Data;
using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Models.UserContext;
using EPRO.Infrastructure.Services;
using MongoDB.Driver;
using EPRO.Api.Authentication;

namespace EPRO.Api.Extensions
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

            services.AddScoped<IGetBearerTokenQuery, GetBearerTokenQuery>();
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<ICdnService, CdnService>();
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
    }
}
