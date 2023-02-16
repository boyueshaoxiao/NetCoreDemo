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
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetCoreApiDemo.Extensions;
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
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static string GenerateJwt(IEnumerable<string> userRoles, JwtTokenPayload payload)
        {
            var claims = new List<Claim>
            {
                new Claim("userId",payload.UserId),
                new Claim("userName",payload.UserName)
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
        /// 解析
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public static Tuple<bool, JwtTokenPayload> ValidateJwt(string jwtToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(jwtToken);
            var payLoad = new JwtTokenPayload();

            try
            {
                var userId = token.Payload.GetValueOrDefault("userId").ToString();
                var userName = token.Payload.GetValueOrDefault("userName").ToString();
                var roles = token.Payload.GetValueOrDefault(ClaimTypes.Role).ToString();
                payLoad.UserId = userId;
                payLoad.UserName = userName;
                var a = JsonConvert.DeserializeObject<JArray>(roles.ToString());
                //var a = (object[])roles;

                foreach (var obj in a)
                {
                    var deviceJson = obj.Value<string>();
                }

                switch (roles)
                {
                    case string _:
                        payLoad.UserRoles = new List<string> { roles.ToString() };
                        break;
                    default:
                        //payLoad.UserRoles = (IEnumerable<string>)JsonConvert.DeserializeObject(roles.ToString());
                        payLoad.UserRoles = (IEnumerable<string>)roles;
                        break;
                }

                //payLoad.UserRoles = roles switch
                //{
                //    string _ => new List<string> { roles.ToString() },
                //    _ => (IEnumerable<string>)JsonConvert.DeserializeObject(roles.ToString())
                //};

                return new Tuple<bool, JwtTokenPayload>(false, payLoad);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Tuple<bool, JwtTokenPayload>(false, payLoad);
            }
        }
    }
}
