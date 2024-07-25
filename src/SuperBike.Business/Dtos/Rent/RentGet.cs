using SuperBike.Domain.Entities.ValueObjects.Rent;

namespace SuperBike.Business.Dtos.Rent
{
    public class RentGet
    {
        public int Id { get; set; }
        public int RentalplanId { get; set; }
        public int MotorcycleId { get; set; }
        public int RenterId { get; set; }

        /// <summary>
        /// Quantidade de dias informado pelo usuário.
        /// </summary>
        public int RentalDays { get; set; }

        /// <summary>
        /// Dia inicial do aluguel.
        /// </summary>
        public DateTime InitialDate { get; set; }

        /// <summary>
        /// Data final do plano selecionado.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Previsão final do aluguel fornecido pelo usuário.
        /// </summary>
        public DateTime EndPredictionDate { get; set; }

        /// <summary>
        /// Total de dias já passados desde o inicio do aluguel.
        /// </summary>
        public int TotalDaysOfRent { get; set; }

        /// <summary>
        /// Valor total do aluguel até a data atual.
        /// </summary>
        public decimal TotalRentalValue { get; set; }
        public string? UserId { get; set; } = "";
    }
}
