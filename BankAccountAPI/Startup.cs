using BankAccountAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BankAccountAPI
{
    public class Startup
    {

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // requires using Microsoft.Extensions.Options
            services.Configure<StatementsDatabaseSettings>(
                Configuration.GetSection(nameof(StatementsDatabaseSettings)));

            services.AddSingleton<IStatementsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<StatementsDatabaseSettings>>().Value);

            services.AddCors(options =>
                {
                    options.AddPolicy(name: MyAllowSpecificOrigins,
                            builder =>
                            {
                                builder.WithOrigins("https://localhost");
                            });
                });
            services.AddControllers().AddNewtonsoftJson();
            services.AddScoped<IStatementService, StatementService>();
            services.AddScoped<IStatementRepository, StatementRepository>();
            services.AddScoped<IStatementDownloadService, StatementDownloadService>();
            services.AddScoped<IFinTsExecutor, FinTsExecutor>();
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
            app.UseCors(options => options.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
