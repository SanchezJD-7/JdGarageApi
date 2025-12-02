namespace JdGarageApi.Models.DTOs
{
    public class UserLoginResponseDto
    {
        public UserDataDto User { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
    }
}
