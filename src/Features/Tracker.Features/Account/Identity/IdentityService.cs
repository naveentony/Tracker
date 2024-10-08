﻿using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Tracker.Domain.Dtos;

namespace Tracker.Features.Account.Identity
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
        public async Task AssigneUserToParent(Guid ChildID, IMongoCollection<UsersDto> collection, Guid ParentID)
        {
          var result=  collection.Find(x => x.Id == ParentID).FirstOrDefault();
            result.AssigedUsers.Add(ChildID);
            var filter = Builders<UsersDto>.Filter.Eq(u => u.Id, ParentID);
            var update = Builders<UsersDto>.Update.Set(u => u.AssigedUsers, result.AssigedUsers);
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
            new Claim("RoleId", userProfile.Roles[0].ToString()),
            new Claim("UserType", userProfile.UserType.ToString()),
            new Claim("ParentID", userProfile.ParentId.ToString()),
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
