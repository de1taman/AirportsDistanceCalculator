using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

#warning неверный namespace; это не критично, просто гигиена
namespace AirportsDistanceCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
