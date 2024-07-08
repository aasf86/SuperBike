namespace SuperBike.Business.Dtos.User.Response
{
    public class UserInsertResponse : ResponseBase<UserInsert>
    {
        public UserInsertResponse(UserInsert userInsert)
        {
            Data = userInsert;
        }

        public UserInsertResponse(UserInsert userInsert, List<string?> erros)
        {
            Data = userInsert;
            Errors = erros;
        }
    }
}
