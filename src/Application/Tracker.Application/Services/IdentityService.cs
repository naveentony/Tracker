using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Tracker.Domain.Dtos;

namespace Tracker.Application.Services
{
    public class IdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly byte[] _key;

        public IdentityService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
            _key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
        }

        public JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

        public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
        {
            var tokenDescriptor = GetTokenDescriptor(identity);

            return TokenHandler.CreateToken(tokenDescriptor);
        }

        public string WriteToken(SecurityToken token)
        {
            return TokenHandler.WriteToken(token);
        }
        public async Task SaveToken(UsersDto identityUser, IMongoCollection<UsersDto> collection, List<Token> list)
        {
            var filter = Builders<UsersDto>.Filter.Eq(u => u.Id, identityUser.Id);
            var update = Builders<UsersDto>.Update.Set(u => u.Tokens, list);
            await collection.UpdateOneAsync(filter, update);
        }
        public Token GetJwtString(UsersDto userProfile)
        {
            var result = new Token();
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userProfile.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userProfile.Email),
            new Claim("IdentityId", userProfile.Id.ToString()),

            });
            var token = CreateSecurityToken(claimsIdentity);
            result.LoginProvider = token.Issuer;
            result.Name = token.Id;
            result.Value = WriteToken(token);
            return result;
        }

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
        {
            return new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(2),
                Audience = _jwtSettings.Audiences[0],
                Issuer = _jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
        }
       

    }
}
