using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Installers {
  public class DbInstaller : IInstaller {
    public void InstallServices(IServiceCollection services,
                                IConfiguration configuration) {
      services.AddDbContext<ApplicationDbContext>(
          options => options.UseSqlServer(
              configuration.GetConnectionString("DefaultConnection")));
      services.AddDefaultIdentity<IdentityUser>()
          .AddEntityFrameworkStores<ApplicationDbContext>();
    }
  }
}
