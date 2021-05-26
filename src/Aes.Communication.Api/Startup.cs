using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Attributes;
using Aes.Communication.Api.Auth;
using Aes.Communication.Api.Logging;
using Aes.Communication.Application;
using Aes.Communication.Application.Conversations.CreateConversation;
using Aes.Communication.Application.Conversations.Repositories;
using Aes.Communication.Application.Events;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Infrastructure.Conversations;
using Aes.Communication.Infrastructure.CosmosDataAccess.Repositories;
using Aes.Communication.Infrastructure.Events;
using Aes.Communication.Infrastructure.Messages;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace Aes.Communication.Api
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateConversationValidator>());

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            //--IDENTITY SERVER-->
            //todo: real cert
            //var cert = new X509Certificate2("cert_file", "your_cert_password");
            services.AddIdentityServer(options =>
                {
                    options.Authentication.CookieLifetime = new TimeSpan(0, 60, 0);
                    options.Authentication.CookieSlidingExpiration = true;
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(AuthConfig.IdentityResources)
                .AddInMemoryClients(AuthConfig.Clients)
                .AddInMemoryApiResources(AuthConfig.Apis);
            //.AddTestUsers(AuthConfig.Users);
            //.AddProfileService<ProfileService>();

            //IdentityServer dependency injection
            //services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>()
            //    .AddTransient<IProfileService, ProfileService>()
            //    .AddTransient<IUserStore<>, UserStore>();

            //configure authentication for API
            //prevents mapping of standard claim types to Microsoft proprietary ones
            var authConfigKey = "Auth_" + Configuration["Environment"];
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication()
                .AddJwtBearer(jwt =>
                {
                    jwt.Authority = Configuration[$"{authConfigKey}:Authority"];
                    jwt.RequireHttpsMetadata = Convert.ToBoolean(Configuration[$"{authConfigKey}:RequireHttpsMetadata"]);
                    jwt.Audience = Configuration[$"{authConfigKey}:ApiName"];
                });
            //--IDENTITY SERVER END-->

            //MediatR
            services.AddMediatR();
            services.AddMediatR(typeof(IRequestHandler<,>).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(u => ClaimsService.CreateAppUser(new HttpContextAccessor().HttpContext?.User?.Claims));

            //DATA ACCESS
            var connString = Configuration["ConnectionString"];
            if (Configuration["DataSource"] == "cosmos")
            {
                //cosmos
                var dbName = Configuration["CosmosSettings:Database"];
                var convCollection = Configuration["CosmosSettings:ConversationsContainer"];
                var logCollection = Configuration["CosmosSettings:LogsContainer"];

                services.AddTransient<IConversationRepository>(r => new ConversationRepository(connString, dbName, convCollection));
                services.AddTransient<IConversationReadOnlyRepository>(r => new ConversationReadOnlyRepository(connString, dbName, convCollection));
                services.AddTransient<ILogRepository>(r => new CosmosLogRepository(connString, dbName, logCollection));
            }
            else if (Configuration["DataSource"] == "json")
            {
                //json
                services.AddTransient<IConversationRepository>(r => new ConversationJsonRepository($"{connString}Conversation.json"));
                services.AddTransient<IMessageRepository>(r => new ConversationMessageJsonRepository(new ConversationJsonRepository($"{connString}Conversation.json")));
                services.AddTransient<IConversationReadOnlyRepository>(r => new ConversationReadOnlyJsonRepository($"{connString}Conversation.json",
                    ClaimsService.CreateAppUser(new HttpContextAccessor().HttpContext?.User?.Claims)));
            }

            //logging
            //services.AddTransient<ILogger>(l => new TestLogger());
            services.AddTransient<ILogger,Logger>();
            services.AddScoped<LogActionAttribute>();


            //EVENTS
            services.AddTransient<IEventRepository>(r => new EventJsonRepository($"{connString}Event.json"));
            services.AddTransient<EventDispatcher>();

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicies.Dev,
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(2520))
                        .Build());
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            //******swagger*********
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Aquilon Communications API", Version = "v1" });
                // JWT-token authentication by password
                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Flow = "password",
                    TokenUrl = Configuration[$"{authConfigKey}:TokenUrl"],
                    Scopes = new Dictionary<string, string> {
                        {"aes_communications","AES Communications API" }
                    }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();

                var filePath = Path.Combine(AppContext.BaseDirectory, "Aes.Communication.Api.xml");
                c.IncludeXmlComments(filePath);
            });
            //******swagger*********
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();

            //******swagger*********
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aquilon Communications API V1");
                c.OAuthClientId("communications_api_swagger");
                c.OAuthAppName("Communications API - Swagger");
                c.OAuthClientSecret("secret");
            });
            //******swagger*********

            //******IS route*********
            app.Map("/auth", builder =>
            {
                builder.UseIdentityServer();
                builder.UseMvcWithDefaultRoute();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }

    //Swagger helper class
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var hasAuthorize = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] { "aes_communications" } }}
                };
            }
        }
    }
}
