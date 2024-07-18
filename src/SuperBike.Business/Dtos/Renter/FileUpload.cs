using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SuperBike.Domain.Entities.Renter;

namespace SuperBike.Business.Dtos.Renter
{
    public class FileUpload : IValidatableObject
    {
        public IFormFile File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!RenterRole.ImagesAllowedContentType.Contains(File.ContentType))
                yield return new ValidationResult(RenterMsgDialog.InvalidContentType, ["ContentType"]);
        }
    }
}
