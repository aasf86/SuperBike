using Microsoft.Extensions.Logging;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Dtos.Motorcycle.Request;
using SuperBike.Business.Dtos.Motorcycle.Response;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using Entity = SuperBike.Domain.Entities;

namespace SuperBike.Business.UseCases.Motorcycle
{
    public class MotorcycleUseCase : UseCaseBase, IMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private IMotorcycleRepository MotorcycleRepository => _motorcycleRepository;
        public MotorcycleUseCase(ILogger<MotorcycleUseCase> logger, IMotorcycleRepository motorcycleRepository) : base(logger) 
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<MotorcycleInsertResponse> Insert(MotorcycleInsertRequest motorcycleInsertRequest)
        {
            try
            {
                "Inciando [insert] de motocicleta: {Plate}".LogInf(motorcycleInsertRequest.Data.Plate);

                var motocycle = motorcycleInsertRequest.Data;
                var motorcycleInsertResponse = new MotorcycleInsertResponse(motocycle);
                var result = Validate(motocycle);

                if (!result.IsSuccess)
                {
                    motorcycleInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", motorcycleInsertResponse.Errors.ToArray());
                    $"Motocicleta inválida '{{Plate}}': {errors} ".LogWrn(motocycle.Plate);                    
                    return motorcycleInsertResponse;
                }

                //aasf86 verificar se já existe placa
                //'MotorcycleRepository.GetAll' ou 'MotorcycleRepository.Get'

                var item = await MotorcycleRepository.GetById(5);
                
                var newItem = new Entity.Motorcycle(item.Year, item.Model + "*", item.Plate);
                newItem.Id = item.Id;

                await MotorcycleRepository.Update(newItem);


                await MotorcycleRepository.Insert(new Entity.Motorcycle(motocycle.Year, motocycle.Model, motocycle.Plate));

                return motorcycleInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [inserir] motocicleta: {Plate}".LogErr(motorcycleInsertRequest.Data.Plate);
                exc.Message.LogErr(exc);

                var motorcycleInsertResponse = new MotorcycleInsertResponse(motorcycleInsertRequest.Data);
                motorcycleInsertResponse.Errors.Add(exc.Message);
                return motorcycleInsertResponse;
            }            
        }
    }
}
