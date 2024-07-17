using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using SuperBike.Domain.Contracts.Repositories.Renter;
using SuperBike.Domain.Contracts.Services;
using SuperBike.Infrastructure.Repositories.Motorcycle;
using SuperBike.Infrastructure.Repositories.Renter;
using SuperBike.Infrastructure.Services;

namespace SuperBike.Infrastructure.Config
{
    public static class InfrastructureIoC
    {
        public static IServiceCollection AddInfrastructureIoC(this IServiceCollection services, IConfigurationManager config)
        {
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IRenterRepository, RenterRepository>();
            services.AddScoped<IMessageBroker>(serviceProvider =>
            {
                return new MessageBrokerRabbitMq(
                    config.GetSection("RabbitMqHost:Host").Value ?? "",
                    int.Parse(config.GetSection("RabbitMqHost:Port").Value ?? "0"),
                    config.GetSection("RabbitMqHost:UserName").Value ?? "",
                    config.GetSection("RabbitMqHost:Password").Value ?? ""
                );
            });
            return services;
        }
    }
}
