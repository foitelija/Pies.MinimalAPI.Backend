namespace Bakery.MinimalAPI.Backend.Auth
{
    public class TokenService : ITokenService
    { 
        //10 min
        private TimeSpan ExpiryDuration = new TimeSpan(0, 10, 0);

        public string BuildToke(string key, string issuer, UserDto user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.usernmae),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            };

            var seckey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credkey = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescr = new JwtSecurityToken(issuer, issuer, claims, expires:
                DateTime.Now.Add(ExpiryDuration), signingCredentials: credkey);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescr);
        }
    }
}
