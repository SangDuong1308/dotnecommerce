﻿using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
        }
        public JwtSecurityToken CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.Now.AddSeconds(30),
            //    SigningCredentials = creds,
            //    Issuer = _config["JWT:Issuer"],
            //    Audience = _config["JWT:Audience"]
            //};

            var token = new JwtSecurityToken
            (
                claims : claims,
                expires : DateTime.UtcNow.AddSeconds(30),
                signingCredentials : creds,
                issuer : _config["JWT:Issuer"],
                audience : _config["JWT:Audience"]
            );

            //var tokenHandler = new JwtSecurityTokenHandler();

            //var token = tokenHandler.CreateToken(tokenDescriptor);

            //return tokenHandler.WriteToken(token);

            return token;
        }
    }
}
