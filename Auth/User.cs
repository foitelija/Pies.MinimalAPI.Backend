namespace Bakery.MinimalAPI.Backend.Auth
{
    public record UserDto(string usernmae, string password);

    public class User
    {
        [Required]
        public string Usernmae { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
