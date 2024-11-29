using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App6
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                const string connectionString = @"Server=(LocalDb)\MSSqlLocalDb;Initial Catalog=App6-code-first;Integrated Security=True";
                options.UseSqlServer(connectionString);
            });

            services.AddSingleton<App>();

            var provider = services.BuildServiceProvider();

            // Run Application
            provider.GetRequiredService<App>().Run();
        }
    }
}
