using DataAccess.Services;
using System.Configuration;

namespace LibraryApp
{
    public class Startup
    {

        readonly string CorsPolicy = "_corsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            // debe ir al inicio de todo 
            //services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicy,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                       .AllowAnyMethod()
                                       .AllowAnyHeader();
                                  });
            });

            services.AddControllers();
            // intento de organizar las inyecciones de dependencias a travez de una clase de configuración
            TransientConfig transientConfig = new TransientConfig(services);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors(options =>
            {
                //agregar la IP cliente que hace la petición
                options.WithOrigins("http://localhost:3000");
                options.WithOrigins("http://riskserver01.ddns.net:8090", "http://127.0.0.1", "http://localhost", "http://127.0.0.1:80", "http://localhost:80");
                options.WithOrigins( "http://localhost:3000", "https://localhost:44394");
                options.AllowAnyMethod();
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
            });
            //app.UseCors(CorsPolicy);
            app.UseCors(p => p.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
               
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
