using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infra.Configs;
using eAgenda.Infra.Orm;
using eAgenda.Infra.Orm.ModuloTarefa;
using eAgenda.Webapi.Config.AutoMapperConfig;
using eAgenda.Webapi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace eAgenda.Webapi
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
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.SuppressModelStateInvalidFilter = true;
            });

            services.AddAutoMapper(config =>
            {
                config.AddProfile<TarefaProfile>();
            });

            services.AddSingleton((x) => new ConfiguracaoAplicacaoeAgenda().ConnectionStrings);

            services.AddScoped<IContextoPersistencia, eAgendaDbContext>();
            services.AddScoped<IRepositorioTarefa, RepositorioTarefaOrm>();

            services.AddTransient<ServicoTarefa>();

            services.AddControllers(config =>
            {
                config.Filters.Add(new ValidarViewModelActionFilter());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eAgenda.Webapi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "eAgenda.Webapi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
