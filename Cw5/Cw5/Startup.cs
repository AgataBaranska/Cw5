
using Cw5.DAL;
using Cw5.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cw5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        //wstrzykiwanie klas przydatnych w wielu miejscach w kodzie(np. logowanie, komunikacja z bd)
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDbService, MssqlDBService>();
            services.AddTransient<IStudentsDbService, SqlServerDbService>();

            services.AddControllers();//zarejestrowanie kontrolerów z widokami i stronami
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //definiuje tzw. middlewary
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            //dodaje now¹ instancjê klasy EndpointRoutingMiddleware(rutowanie zadañ u¿ytkowników na poststawie adresu url, metody http)
            app.UseRouting();


            //mój middleware: doklejanie do odpowiedzi  nag³owek http
            app.Use(async (context, c) =>
            {
                context.Response.Headers.Add("Secret", "1234");
                await c.Invoke();//przepuszczenie rz¹dania do kolejnego middleware w kolejce
            });


            app.UseMiddleware<CustomMiddleware>();


            //dodaje middleware, który sprawdza czy ktoœ ma dostêp do czegoœ
            app.UseAuthorization();


            //definiuje endpointy
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
