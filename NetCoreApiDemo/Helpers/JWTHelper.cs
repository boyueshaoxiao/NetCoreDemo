/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/7/6 14:02:32
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          JWTHelper
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetCoreApiDemo.Enumeration;
using NetCoreApiDemo.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetCoreApiDemo.Helpers
{
    /// <summary>
    /// JWT 帮助类
    /// </summary>
    public static class JwtHelper
    {
        private static readonly string Iss = AppConfig.Issuer;
        private static readonly string Aud = AppConfig.Audience;
        private static readonly string SecretKey = AppConfig.SecretKey;
        private static readonly int Expire = AppConfig.Expire;

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string GenerateJwt(IEnumerable<string> userRoles, JwtTokenPayload payload)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimKey.UserId, payload.UserId),
                new Claim(JwtClaimKey.UserName, payload.UserName)
            };

            // 可以将一个用户的多个角色全部赋予，比如参数System,Admin，那么该token即拥有两个角色
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: Iss,
                audience: Aud,
                claims: claims,
                expires: DateTime.Now.AddHours(Expire),
                signingCredentials: credentials);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public static Tuple<bool, JwtTokenPayload> ValidateJwt(string jwtToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(jwtToken);

            try
            {
                var userId = token.Payload.GetValueOrDefault(JwtClaimKey.UserId).ToString();
                var userName = token.Payload.GetValueOrDefault(JwtClaimKey.UserName).ToString();
                var roles = token.Payload.GetValueOrDefault(ClaimTypes.Role);
                var roleList = new List<string>();

                switch (roles)
                {
                    case string _:
                        roleList = new List<string> { roles.ToString() };
                        break;
                    default:
                        var a = JsonConvert.DeserializeObject<JArray>(roles.ToString());
                        roleList.AddRange(a.Select(obj => obj.Value<string>()));
                        break;
                }

                var payLoad = new JwtTokenPayload
                {
                    UserId = userId,
                    UserName = userName,
                    UserRoles = roleList
                };
                return new Tuple<bool, JwtTokenPayload>(false, payLoad);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Tuple<bool, JwtTokenPayload>(false, null);
            }
        }
    }
}
