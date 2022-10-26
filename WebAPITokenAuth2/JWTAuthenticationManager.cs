using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPITokenAuth2
{
    public class JWTAuthenticationManager
    {
        private readonly string key;

        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        {
            { "test", "password"},
            { "test1", "pwd"}
        };

        public JWTAuthenticationManager(string key)
        {
            this.key = key;            
        }

        public string Authenticate(string username, string password)
        {
            if (!users.Any(u => u.Key == username && u.Value == password))
            {
                return null;
            }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),

                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)

            };
            var token = handler.CreateToken(securityTokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
