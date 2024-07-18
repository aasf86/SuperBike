using SuperBike.Domain.Entities.ValueObjects.Renter;
using System.ComponentModel.DataAnnotations;

namespace SuperBike.Domain.Entities
{
    public partial class Renter : EntityBase
    {
        public Renter() { }
        public Renter(string name, string cnpjCpf, DateTime dateOfBirth, string cnh, string cnhType, string userId, string? cnhImg = null)
        {
            long result = 0;

            if (string.IsNullOrEmpty(name) 
                || name.Length < RenterRole.NameMinimalLenth
            ) throw new InvalidDataException(RenterMsgDialog.RequiredName);

            if (string.IsNullOrEmpty(cnpjCpf)
            ) throw new InvalidDataException(RenterMsgDialog.RequiredCnpjCpf);

            if (cnpjCpf.Length < RenterRole.CnpjCpfMinimalLenth
                || cnpjCpf.Length > RenterRole.CnpjCpfMaxLenth
            ) throw new InvalidDataException(RenterMsgDialog.InvalidCnpjCpf);

            if (!(cnpjCpf.Length == RenterRole.CnpjCpfMaxLenth 
                || cnpjCpf.Length == RenterRole.CnpjCpfMinimalLenth)
            ) throw new InvalidDataException(RenterMsgDialog.InvalidCnpjCpf);

            if (!long.TryParse(cnpjCpf, out result))
                throw new InvalidDataException(RenterMsgDialog.InvalidCnpjCpf);

            if (dateOfBirth > DateTime.Now) throw new InvalidDataException(RenterMsgDialog.InvalidDateOfBirth);
            if (string.IsNullOrEmpty(cnh)) throw new InvalidDataException(RenterMsgDialog.RequiredCNH);
            if (!long.TryParse(cnh, out result)) throw new InvalidDataException(RenterMsgDialog.InvalidCNH);
            if (string.IsNullOrEmpty(cnhType)) throw new InvalidDataException(RenterMsgDialog.RequiredCNHType);
            if (!RenterRole.TypesCNHAllowed.Contains(cnhType)) throw new InvalidDataException(RenterMsgDialog.InvalidCNHType);
            if (string.IsNullOrEmpty(userId)) throw new InvalidDataException(RenterMsgDialog.RequiredUserId);

            Name = name;
            CnpjCpf = cnpjCpf;
            DateOfBirth = dateOfBirth;
            CNH = cnh;
            CNHImg = cnhImg;
            CNHType = cnhType;
            UserId = userId;
        }
        public string Name { get; private set; } = "";
        public string CnpjCpf { get; private set; } = "";
        public DateTime DateOfBirth { get; private set; }
        public string CNH { get; private set; } = "";
        public string CNHType { get; private set; } = "";
        public string? CNHImg { get; private set; }
        public FileDisk? CNHFile { get; private set; }
        public string UserId { get; private set; } = "";
    }
}
