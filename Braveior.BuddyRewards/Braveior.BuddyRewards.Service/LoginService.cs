using AutoMapper;
using Braveior.BuddyRewards.DTO;
using Braveior.BuddyRewards.Service.Interfaces;
using Braveior.BuddyRewards.Service.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Braveior.BuddyRewards.Service
{
    /// <summary>
    /// Login service for Authentication and Authorization
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly IMapper _mapper;
        public LoginService(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// Generate the Access Token for the member ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string GenerateAccessToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("thisisasecretkeyanddontsharewithanyone");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.SerialNumber, userId)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        /// <summary>
        /// Validate the Access Token and returns the member Id
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private string ValidateAccessToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("thisisasecretkeyanddontsharewithanyone");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            SecurityToken securityToken;
            var principle = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                var memberId = principle.FindFirst(ClaimTypes.SerialNumber)?.Value;
                if (!String.IsNullOrEmpty(memberId))
                {
                    return memberId;
                }
                else
                    throw new Exception("Invalid access Token");
            }
            throw new Exception("Invalid access Token");
        }

        /// <summary>
        /// Method to Authenticate the user credentials - Email and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public MemberDTO Login(LoginDTO user)
        {

            var memberEntity = DB.Find<Member>().ManyAsync(m => m.Email == user.Email && m.Password == user.Password).Result.FirstOrDefault();

            if (memberEntity == null)
            {
                throw new Exception("Member not found");
            }
            else
            {
                var memberWithToken = _mapper.Map<MemberDTO>(memberEntity);
                memberWithToken.AccessToken = GenerateAccessToken(memberEntity.ID);
                return memberWithToken;
            }

        }
        /// <summary>
        /// Get the User ( Member) mapped to the accessToken
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<MemberDTO> GetUserFromAccessToken(string accessToken)
        {
            var memberId = ValidateAccessToken(accessToken);
            if (!String.IsNullOrEmpty(memberId))
            {
                var memberEntity = await DB.Find<Member>().OneAsync(memberId);
                return _mapper.Map<MemberDTO>(memberEntity);
            }

            throw new Exception("Invalid access Token");
        }

    }

}
