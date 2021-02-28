using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsCosmosDBApp.Models;
using ContactsCosmosDBApp.Models.Abstract;
using ContactsCosmosDBApp.Models.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContactsCosmosDBApp
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<CosmosUtility>(cfg =>
      {
        cfg.CosmosEndpoint = _configuration["CosmosConnectionString:CosmosEndpoint"];
        cfg.CosmosKey = _configuration["CosmosConnectionString:CosmosKey"];
      });

      services.AddScoped<IContactRepository, CosmosContactRepository>();
      #region Swagger
      services.AddSwaggerGen(cfg =>
      {
        cfg.SwaggerDoc(name: "V1", info: new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Contacts API", Version = "V1" });
      });
      #endregion

      services.AddMvc();//.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseSwagger();
      app.UseSwaggerUI(cfg =>
      {
        cfg.SwaggerEndpoint(url: "/swagger/V1/swagger.json", name: "Contacts Api");
        cfg.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
      });
      app.UseRouting();

      app.UseEndpoints(ConfigureEndpoints);
    }
    private void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
      endpoints.MapControllers();

      //endpoints.MapGet("/", async context =>
      //{
      //  await context.Response.WriteAsync("Hello World!");
      //});

    }
  }
}
