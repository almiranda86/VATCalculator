using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Service.Managers;

namespace VATCalculator.Api.IoC
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICalculationManager, CalculationManager>();
            services.AddScoped<INetAmmountCalculationManager, NetAmmountCalculationManager>();
            services.AddScoped<IGrossAmmountCalculationManager, GrossAmmountCalculationManager>();
            services.AddScoped<IVatTaxAmmountCalculationManager, VatTaxAmmountCalculationManager>();

            return services;
        }
    }
}
