using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CircuitsUc.Domain.IRepositories.Base;
using CircuitsUc.InfraStructure.Reposatories.Base;
using CircuitsUc.Domain.IRepositories;
using CircuitsUc.InfraStructure.Reposatories;
using CircuitsUc.Api.ActionFilter;
using CircuitsUc.Api.ActionFilter;
using CircuitsUc.InfraStructure.Presistance;
using CircuitsUc.Api.Middleware;
using CircuitsUc.Application.IServices;
using CircuitsUc.Application.Services;
using CircuitsUc.Application.IService;
using CircuitsUc.Application.Service;
using Microsoft.Extensions.FileProviders;

namespace CircuitsUc.Api
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



            var key = Encoding.ASCII.GetBytes(Configuration["JwtSettings:key"].ToString());
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidAudience = Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });



            services.AddCors(options => options.AddPolicy("DemoCorsPolicy", build =>
            {

                build.WithOrigins(
                                // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                                Configuration["App:CorsOrigins"].ToString()
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                               //.Select(o => o.RemovePostFix("/"))
                               .ToArray()
                       )
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            }));





            services.AddControllers();
            services.AddDbContext<DBContext>(
                  m => m.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CircuitsUc.API", Version = "v1" });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            //services.AddSwaggerGen(options =>
            //{
            //    options.MapType<TimeSpan>(() => new OpenApiSchema
            //    {
            //        Type = "string",
            //        Example = new OpenApiString("00:00:00")
            //    });
            //});
            services.AddSingleton<IFileProvider>(
                    new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            //services.AddAutoMapper(typeof(Startup));
            //services.AddMediatR(typeof(CreateEmployeeHandler).GetTypeInfo().Assembly);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISecurityUserService, SecurityUserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPageTypeService, PageTypeService>();
            services.AddScoped<ISystemParameterServices, SystemParameterService>();
            services.AddScoped<IPageContentService, PageContentService>();
           
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddAutoMapper(typeof(MappingUserProfile));
            #region Localization
            services.AddControllersWithViews();
            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "";
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<BaseActionFilter>();
            services.AddTransient<CustomerBaseFilter>();



            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new List<CultureInfo>
            //        {
            //            new CultureInfo("ar"),
            //            new CultureInfo("en-US"),
            //        };

            //    options.DefaultRequestCulture = new RequestCulture("ar");
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;
            //});

            #endregion

        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            #region Localization Middleware
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            #endregion


            app.UseStaticFiles();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Circuits_Uc.API v1"));


            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using var context = scope.ServiceProvider.GetService<DBContext>();
                context.Database.Migrate();
            }
            /*DBContext dbcontext = app.ApplicationServices.GetRequiredService<DBContext>();
            dbcontext.Database.EnsureCreated();*/
            /*  using (var scope = app.ApplicationServices.CreateScope())
              {
                  var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();
                  // use context
                  dbContext.Database.EnsureCreated(); 
              }*/
            var supportedCultures = new[]
            {
                new CultureInfo("ar"),
                new CultureInfo("en-US"),
            };

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                // you can change the list of providers, if you don't want the default behavior
                // e.g. the following line enables to pick up culture ONLY from cookies
                RequestCultureProviders = new[] { new CookieRequestCultureProvider() }
            };

            app.UseRequestLocalization(localizationOptions);
            app.UseRequestLocalization();
            app.UseHttpsRedirection();

            app.UseRouting();


            #region Globalization and Localization
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            #endregion


            app.UseCors("DemoCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}



