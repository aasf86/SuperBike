using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Rules.Renter;

namespace SuperBike.Business.Dtos.Renter
{
    public class FileUpload : IValidatableObject
    {
        public IFormFile File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!RenterRule.ImagesAllowedContentType.Contains(File.ContentType))
                yield return new ValidationResult(RenterMsgDialog.InvalidContentType, ["ContentType"]);
        }
    }
}
