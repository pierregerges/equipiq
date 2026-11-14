using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EquipIQ.Application;

public static class DependencyInjection
{
    /// <summary>Registers everything the Application layer needs.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Finds every validator in this project automatically.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
} 