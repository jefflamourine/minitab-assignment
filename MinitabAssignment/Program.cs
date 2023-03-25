using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Minitab_Assignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IAddressValidator, UspsAddressValidator>();
            builder.Services.AddSingleton<ICrmRepository, LoggerCrmRepository>();
            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}