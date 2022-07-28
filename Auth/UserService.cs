namespace Bakery.MinimalAPI.Backend.Auth
{
    public class UserService : IUserService
    {
        private List<UserDto> _users => new()
        {
            new UserDto("test", "123"),
            new UserDto("test2", "123"),
            new UserDto("Juro", "123")
        };

        public UserDto GetUser(User userModel)
        {
            _users.FirstOrDefault(u =>
            string.Equals(u.usernmae, userModel.Usernmae) &&
            string.Equals(u.password, userModel.Password));

            throw new Exception();
        }
    }
}
