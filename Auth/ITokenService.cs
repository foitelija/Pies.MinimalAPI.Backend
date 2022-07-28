namespace Bakery.MinimalAPI.Backend.Auth
{
    public interface ITokenService
    {
        string BuildToke(string key, string issuer, UserDto user);
    }
}
