using FluentValidation;
using VATCalculator.Domain.Queries;
using VATCalculator.Service.Validations;

namespace VATCalculator.Api.IoC
{
    public static class ConfigureValidators
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<GetCalculationRequest>, GetCalculationRequestValidation>();
            return services;
        }
    }
}