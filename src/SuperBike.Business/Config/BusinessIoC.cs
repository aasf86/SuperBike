using Microsoft.Extensions.DependencyInjection;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.UseCases.Motorcycle;
using SuperBike.Business.UseCases.User;

namespace SuperBike.Business.Config
{
    public static class BusinessIoC
    {
        public static IServiceCollection AddBusinessIoC(this IServiceCollection services)
        {
            services.AddScoped<IUserUseCase, UserUseCase>();
            services.AddScoped<IMotorcycleUseCase, MotorcycleUseCase>();

            return services;
        }
    }
}
