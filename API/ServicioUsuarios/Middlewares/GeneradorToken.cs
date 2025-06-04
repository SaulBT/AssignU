using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ServicioUsuarios.Middlewares.Interfaces;

namespace ServicioUsuarios.Middlewares;

public class GeneradorToken : IGeneradorToken
{
    private readonly IConfiguration _configuration;


    public GeneradorToken(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerarToken(string nombreUsuario, string tipo, int idUsuario)
    {
        var issuer = _configuration["JWTSettings:Issuer"];
        var audience = _configuration["JWTSettings:Audience"];
        var key = _configuration["JWTSettings:Key"];
        var noHoras = Convert.ToInt32(_configuration["JWTSettings:NoHoras"]);
        var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var expiraToken = DateTime.Now.AddHours(noHoras).AddHours(6);

        var identidad = new ClaimsIdentity(new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Iss, issuer),
            new Claim(JwtRegisteredClaimNames.Aud, audience),
            new Claim("idUsuario", idUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
            new Claim(ClaimTypes.Name, nombreUsuario),
            new Claim(ClaimTypes.Role, tipo),
        });
        
        var credenciales = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256);
        var descriptorTokenSeguridad = new SecurityTokenDescriptor
        {
            Subject = identidad,
            Expires = expiraToken,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now,
            SigningCredentials = credenciales
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(descriptorTokenSeguridad);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);
        
        return token;
    }
}