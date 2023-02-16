/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/24 11:28:24
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          HomeController
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiDemo.Extensions;
using NetCoreApiDemo.Helpers;
using NetCoreApiDemo.Models;
using Newtonsoft.Json;

namespace NetCoreApiDemo.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [Route("api/login/[action]")]
    public class LoginController : BaseController
    {
        /// <summary>
        /// 登录获取Token
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login([FromBody] RoleModel role)
        {
            string token;
            if (role.UserRoles.Null())
            {
                token = "参数UserRoles为null";
                return BadRequest(new { Error = token });
            }

            var payLoad = new JwtTokenPayload
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = "Dennis"
            };
            token = JwtHelper.GenerateJwt(role.UserRoles, payLoad);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize("SystemOrAdmin")]
        [Authorize]
        public IActionResult ParseToken()
        {
            //需要截取Bearer 
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var tokenRes = JwtHelper.ValidateJwt(tokenHeader);
            return Ok(tokenRes.Item2);

        }
    }
}
