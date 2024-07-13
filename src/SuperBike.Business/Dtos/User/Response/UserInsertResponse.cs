
namespace SuperBike.Business.Dtos.User.Response
{
    public class UserInsertResponse : ResponseBase<UserInsert>
    {
        public UserInsertResponse(UserInsert userInsert) : base(userInsert) { }
        public UserInsertResponse(UserInsert userInsert, List<string?> erros, Guid? guid) : base(userInsert, erros, guid) { }
    }
}