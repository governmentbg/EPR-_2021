using DataTables.AspNet.AspNetCore;
using EPRO.Core.Configuration;
using EPRO.Core.Constants;
using EPRO.Extensions;
using EPRO.Infrastructure.Constants;
using EPRO.ModelBinders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using EPRO.Models;

namespace EPRO
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
            // За добавяне на контексти, използвайте extension метода!!!
            services.AddApplicationDbContext(Configuration);


            services.AddAuthentication(x =>
            {
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie()
            .AddStampIT(options =>
            {
                options.AppId = Configuration.GetValue<string>("Authentication:StampIT:AppId");
                options.AppSecret = Configuration.GetValue<string>("Authentication:StampIT:AppSecret");
                options.Scope.Add("pid");
                options.ClaimActions.DeleteClaim(ClaimTypes.NameIdentifier);
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "pid");
                options.ClaimActions.MapJsonKey(CustomClaimType.IdStampit.CertificateNumber, "certno");
                options.AuthorizationEndpoint = Configuration.GetValue<string>("Authentication:StampIT:AuthorizationEndpoint");
                options.TokenEndpoint = Configuration.GetValue<string>("Authentication:StampIT:TokenEndpoint");
                options.UserInformationEndpoint = Configuration.GetValue<string>("Authentication:StampIT:UserInformationEndpoint");
                options.Events = new OAuthEvents()
                {
                    OnRemoteFailure = context => HandleRemoteFailure(context)
                };
            });

            // За конфигуриране на Identity, използвайте extension метода!!! 
            services.AddApplicationIdentity();

            // За добавяне на услуги, използвайте extension метода!!!
            services.AddApplicationServices();
            services.Configure<RecaptchaOptions>(Configuration.GetSection("RecaptchaOptions"));
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new CultureInfo[]
                                     {
                                         new CultureInfo("bg")
                                     };

                options.DefaultRequestCulture = new RequestCulture("bg");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            int cookieMaxAgeMinutes = Configuration.GetValue<int>("Authentication:CookieMaxAgeMinutes");
            services.ConfigureApplicationCookie(options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(cookieMaxAgeMinutes);
                options.LoginPath = new PathString("/public/Account/Login");
                options.LogoutPath = new PathString("/public/Account/LogOff");
            });


            services.AddDistributedMemoryCache();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddMvcOptions(options =>
                {
                    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider(FormatConstant.NormalDateFormat));
                    options.ModelBinderProviders.Insert(1, new DoubleModelBinderProvider());
                    options.ModelBinderProviders.Insert(2, new DecimalModelBinderProvider());
                }).AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.RegisterDataTables();
            services.AddAutoMapper(typeof(APPKReportsProfile).Assembly);
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/public/public/Error");
            app.UseHsts();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.Use((authContext, next) =>
            {
                authContext.Request.Scheme = "https";
                return next();
            });

            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env, Configuration.GetValue<string>("RotativaLibRelativePath"));

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                     name: "defaultWithArea",
                     pattern: "{area=public}/{controller=Public}/{action=Index}/{id?}");
                
                endpoints.MapHealthChecks("/health");
            });
        }

        private static Task HandleRemoteFailure(RemoteFailureContext context)
        {
            context.Response.Redirect($"/public/account/logincerterror?error={context.Failure}");
            context.HandleResponse();

            return Task.FromResult(0);
        }
    }
}
