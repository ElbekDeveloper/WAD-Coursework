using Api.Installers;
using Api.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Api {
public class Startup {
  public Startup(IConfiguration configuration) {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to
  // the container.
  public void ConfigureServices(IServiceCollection services) {
    services.InstallServicesInAssembly(Configuration);
  }

  // This method gets called by the runtime. Use this method to configure the
  // HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
    if (env.IsDevelopment()) {
      app.UseDeveloperExceptionPage();
    } else {
      app.UseHsts();
    }

    var swaggerOptions = new SwaggerOptions();
    Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

    app.UseSwagger(options => {
      options.RouteTemplate = swaggerOptions.JsonRoute;
    });

    app.UseSwaggerUI(options => {
      options.SwaggerEndpoint(swaggerOptions.UiEndpoint,
                              swaggerOptions.Description);
    });

    app.UseRouting();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => {
      endpoints.MapGet("/", context => {
        context.Response.Redirect("/swagger/");
        return Task.CompletedTask;
      });
      endpoints.MapControllers();
    });
  }
}
}
