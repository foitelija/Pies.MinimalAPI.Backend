namespace Bakery.MinimalAPI.Backend.Auth
{
    public interface IUserService
    {
        UserDto GetUser(User userModel);
    }
}
