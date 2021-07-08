using CalculadoraRest.Business;
using CalculadoraRest.Business.Implementations;
using CalculadoraRest.Configurations;
using CalculadoraRest.Hypermedia.Enricher;
using CalculadoraRest.Hypermedia.Filters;
using CalculadoraRest.Model.Context;
using CalculadoraRest.Repository;
using CalculadoraRest.Repository.Generic;
using CalculadoraRest.Services;
using CalculadoraRest.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestWithASPNETUdemy.Business.Implementations;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculadoraRest
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Popula a classe TokenConfiguratir da sessão TokenConfiguration do app.settings
            var tokenConfigurations = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(Configuration.GetSection("TokenConfigurations")).Configure(tokenConfigurations);

            //AddSingleton pois só havera uma instancia de tokenConfiguration será global
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
            });

            services.AddCors(_ => _.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.AddControllers();

            var connection = Configuration["MySQLConnection:MySQLConnectionString"];
            services.AddDbContext<MySqlContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

            if (Environment.IsDevelopment())
            {
                MigrateDatabase(connection);
            }

            services.AddMvc(options =>
            {
                //Aceita o tipo solicitado no header da requisição
                options.RespectBrowserAcceptHeader = true;

                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
            }).AddXmlSerializerFormatters();


            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ContentRespondeEnricherList.Add(new PersonEnricher());
            filterOptions.ContentRespondeEnricherList.Add(new BookEnricher());

            //Singleton = o Objeto é reutilizado durante a execução da aplicação
            services.AddSingleton(filterOptions);

            services.AddApiVersioning();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "REST API's",
                        Version = "v1",
                        Description = "API RESTful",
                        Contact = new OpenApiContact
                        {
                            Name = "Edenilson Bila",
                            Url = new Uri("https://github.com/edenilsonbila")
                        }
                    });
            });

            //Escoped o Objeto é por Requisição, cada requisição é criado um e finalizado o fim da requisição
            services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
            services.AddScoped<IBookBusiness, BookBusinessImplementation>();
            services.AddScoped<ILoginBusiness, LoginBussinessImplementation>();

            //É sempre um objeto novo o inverso de singleton
            services.AddTransient<ITokenService, TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        }

        private void MigrateDatabase(string connection)
        {
            try
            {
                var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);
                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset" },
                    IsEraseDisabled = true
                };
                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("Database migration failed", ex);
                throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            //Cria o JSON com a documentação
            app.UseSwagger();
            //Gera a Pagina HTML do swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API'S V1");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
            });
        }
    }
}

