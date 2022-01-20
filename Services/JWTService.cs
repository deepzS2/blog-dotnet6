using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Services;

public interface IJWTService
{
  string GenerateJWTToken(int userId);
  int? ValidateJwtToken(string token);
}

public class JWTService : IJWTService
{
  private readonly string _secret;

  public JWTService(string secret)
  {
    _secret = secret;
  }

  /// <summary>
  /// Generate a JsonWebToken with user id
  /// </summary>
  public string GenerateJWTToken(int userId)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_secret);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[] { new Claim("id", userId.ToString()) }),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  /// <summary>
  /// Validate the token provided 
  /// </summary>
  /// <returns>User id or null</returns>
  public int? ValidateJwtToken(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_secret);
    try
    {
      tokenHandler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        ClockSkew = TimeSpan.Zero
      }, out SecurityToken validatedToken);

      var jwtToken = (JwtSecurityToken)validatedToken;
      var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

      // return account id from JWT token if validation successful
      return accountId;
    }
    catch
    {
      // return null if validation fails
      return null;
    }
  }
}