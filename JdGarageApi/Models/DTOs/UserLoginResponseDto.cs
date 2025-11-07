namespace JdGarageApi.Models.DTOs
{
    public class UserLoginResponseDto
    {
        public UserDataDto User { get; set; }
        public string Role {  get; set; }
        public string Token { get; set; }
    }
}
