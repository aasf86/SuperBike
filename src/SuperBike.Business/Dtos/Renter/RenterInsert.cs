using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SuperBike.Domain.Entities.Renter;

namespace SuperBike.Business.Dtos.Renter
{
    public class RenterInsert : IValidatableObject
    {
        [Required(ErrorMessage = RenterMsgDialog.RequiredName)]
        [MinLength(RenterRole.NameMinimalLenth, ErrorMessage = RenterMsgDialog.InvalidName)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = RenterMsgDialog.RequiredCnpjCpf)]
        [MinLength(RenterRole.CnpjCpfMinimalLenth, ErrorMessage = RenterMsgDialog.InvalidCnpjCpf)]
        [MaxLength(RenterRole.CnpjCpfMaxLenth, ErrorMessage = RenterMsgDialog.InvalidCnpjCpf)]
        public string CnpjCpf { get; set; } = "";

        [Required(ErrorMessage = RenterMsgDialog.InvalidDateOfBirth)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = RenterMsgDialog.RequiredCNH)]
        [MinLength(RenterRole.CNHMinimalLenth, ErrorMessage = RenterMsgDialog.InvalidCNH)]
        [MaxLength(RenterRole.CNHMaxLenth, ErrorMessage = RenterMsgDialog.InvalidCNH)]
        public string CNH { get; set; } = "";

        [Required(ErrorMessage = RenterMsgDialog.RequiredCNHType)]        
        public string CNHType { get; set; } = "";

        public string? CNHImg { get; set; }

        [NotMapped]
        public string UserId { get; set; } = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            long result = 0;

            if (!(CnpjCpf.Length == RenterRole.CnpjCpfMaxLenth || CnpjCpf.Length == RenterRole.CnpjCpfMinimalLenth))
                yield return new ValidationResult(RenterMsgDialog.InvalidCnpjCpf, [nameof(CnpjCpf)]);

            if (!long.TryParse(CnpjCpf, out result))
                yield return new ValidationResult(RenterMsgDialog.InvalidCnpjCpf, [nameof(CnpjCpf)]);

            if(DateOfBirth > DateTime.Now)
                yield return new ValidationResult(RenterMsgDialog.InvalidDateOfBirth, [nameof(DateOfBirth)]);

            if (!RenterRole.TypesCNHAllowed.Contains(CNHType))
                yield return new ValidationResult(RenterMsgDialog.InvalidCNHType, [nameof(CNHType)]);

            if (!long.TryParse(CNH, out result))
                yield return new ValidationResult(RenterMsgDialog.InvalidCNH, [nameof(CNH)]);            
        }
    }
}
