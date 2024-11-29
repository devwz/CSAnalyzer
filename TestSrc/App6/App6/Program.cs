using App6.Infra;
using App6.Infra.Data;
using Microsoft.Extensions.DependencyInjection;

namespace App6
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton(
                new ApplicationDbContext("Server=(LocalDb)\\MSSqlLocalDb;Initial Catalog=App6;Integrated Security=True"));

            services.AddScoped<IProductService, ProductService>();

            services.AddSingleton<App>();

            var provider = services.BuildServiceProvider();

            // Run Application
            provider.GetRequiredService<App>().Run();
        }
    }
}
