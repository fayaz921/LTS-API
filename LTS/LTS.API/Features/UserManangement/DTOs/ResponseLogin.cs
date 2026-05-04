namespace LTS.API.Features.UserManangement.DTOs
{
    public class ResponseLogin
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
