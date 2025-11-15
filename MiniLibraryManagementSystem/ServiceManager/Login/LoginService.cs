using ClassRecord;
using EnumClasses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ServiceManager.Login
{
    public interface ILoginService
    {
        public ReturnRecord AuthenticateUserAsync(LoginUserRecord user);
    }
    public class LoginService : ILoginService
    {
        private readonly IConfiguration configuration;
        public LoginService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public ReturnRecord AuthenticateUserAsync(LoginUserRecord user)
        {
            var _key = configuration["Jwt:Key"];
            var _issuer = configuration["Jwt:Issuer"];
            var _audience = configuration["Jwt:Audience"];
            if (user.Username == "admin" && user.Password == "123456")
            {
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var keyBytes = Encoding.UTF8.GetBytes(_key);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new[]
                    {
                        new System.Security.Claims.Claim("username", user.Username)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new ReturnRecord(new {token = tokenHandler.WriteToken(token).ToString() },"Token Generate Sucessfully",ResultStatus.Success);
            }
            else
            {
                return new ReturnRecord(string.Empty, "Invalid Username or Password", ResultStatus.Failure);
            }
        }
    }
}
