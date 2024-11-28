using Microsoft.Extensions.DependencyInjection;

namespace App4
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton<App>();

            var provider = services.BuildServiceProvider();

            // Run Application
            provider.GetRequiredService<App>().Run();
        }
    }
}
