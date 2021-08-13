using EPRO.Api.Authentication;
using EPRO.Api.Extensions;
using EPRO.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPRO.Api
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
            services.AddApplicationDbContext(Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = BearerTokenAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = BearerTokenAuthenticationOptions.DefaultScheme;
            }).AddBearerTokenSupport();

            services.AddApplicationServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Електронен публичен регистър на отводите",
                    Version = "v1",
                    Description = "Услуги за интеграция с **ЕПРО**. Тези услуги предоставят възможност на деловодните системи на съдилищата да обменят данни с Електронния публичен регистър на отводите",
                    Contact = new OpenApiContact()
                    {
                        Url = new Uri("https://ecase.justice.bg"),
                        Name = "Електронни съдебни дела"
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "Лицензирано при условията на EUPL-1.2",
                        Url = new Uri("https://joinup.ec.europa.eu/collection/eupl/eupl-text-eupl-12")
                    },
                    // TermsOfService = new Uri("https://www.his.bg/bg/dev/")
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Автентикационен хедър от тип Bearer Token. Въведете **'Bearer'** [интервал] и Вашия AppKey[точка]HmacSha256 на съдържанието на заявката в полето по-долу. Пример: **'Bearer f60748be-7c48-4793-ace6-88cbd9719521.ea92fb32bd042f0fa76ee9fbd8c637577d4702aacbc61d1f2daba1132dfbc63a'**",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = BearerTokenAuthenticationOptions.DefaultScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = BearerTokenAuthenticationOptions.DefaultScheme,
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                c.IncludeXmlComments("EPROApi.xml");
                c.DocumentFilter<ApplyTagDescriptions>();
            });

            services.AddControllers();

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EPRO API V1");
                c.RoutePrefix = string.Empty;
                c.DefaultModelsExpandDepth(-1);
                if (env.EnvironmentName == "Development")
                {
                    c.DisplayRequestDuration();
                }
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
