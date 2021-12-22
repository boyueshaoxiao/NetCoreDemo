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
        [HttpGet]
        public IActionResult Login(string role)
        {
            string token;
            var result = false;
            if (role.NotNull())
            {
                token = JWTHelper.IssueJwt(new UserModel
                {
                    UserId = Guid.NewGuid().ToString(), 
                    UserRole = role, 
                    UserName = role,
                    UserBirthDay = DateTime.Now
                });
                result = true;
            }
            else
            {
                token = " Login Fail.";
            }

            return Ok(new { Status = result, Token = token });
        }

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //该接口限制只有System 或 Admin 角色的Token可以访问
        [Authorize("SystemOrAdmin")]
        //如果中间件中没有配置AddPolicy，直接使用Authorize即可
        //[Authorize]
        public IActionResult ParseToken()
        {
            //需要截取Bearer 
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = JWTHelper.SerializeJwt(tokenHeader);
            return Ok(user);

        }
    }
}
