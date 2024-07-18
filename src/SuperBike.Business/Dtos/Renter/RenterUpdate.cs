using System.ComponentModel.DataAnnotations;
using static SuperBike.Domain.Entities.Rules.Renter;

namespace SuperBike.Business.Dtos.Renter
{
    public class RenterUpdate
    {
        [Required(ErrorMessage = RenterMsgDialog.RequiredCNHImg)]
        public string CNHImg { get; set; }
        
        public string? UserId { get; private set; }

        public void SetUser(string userId) => UserId = userId;
    }
}
