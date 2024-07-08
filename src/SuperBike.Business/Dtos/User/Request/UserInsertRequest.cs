namespace SuperBike.Business.Dtos.User.Request
{
    public class UserInsertRequest : RequestBase<UserInsert>
    {
        public UserInsertRequest(UserInsert userInsert) : base(userInsert) { }
    }
}
