/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/29 16:23:29
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          TestMiddleware
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetCoreApiDemo.Helpers;

namespace NetCoreApiDemo.MiddlewareExtensions
{
    /// <summary>
    /// JWT授权中间件
    /// </summary>
    public static class AuthorizationMiddleware
    {
        /// <summary>
        /// 注册授权服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationService(this IServiceCollection services)
        {
            // 开启Bearer认证
            services.AddAuthentication(options =>
                {
                    // 设置默认使用jwt验证方式
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                // 添加JwtBearer服务
                .AddJwtBearer(o =>
                {
                    // token验证参数
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 验证秘钥
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.SecretKey)),
                        // 验证颁发者
                        ValidateIssuer = true,
                        ValidIssuer = AppConfig.Issuer,
                        // 验证订阅者
                        ValidateAudience = true,
                        ValidAudience = AppConfig.Audience,
                        // 验证过期时间必须设置该属性
                        ClockSkew = TimeSpan.Zero
                    };

                    // 添加验证事件监听
                    o.Events = new JwtBearerEvents
                    {
                        // token验证失败时将失败信息返回到响应头中
                        OnAuthenticationFailed = context =>
                        {
                            string tokenErrMsg;
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                // 过期
                                tokenErrMsg = "Expired";
                            }
                            else if (context.Exception.GetType() == typeof(SecurityTokenNoExpirationException))
                            {
                                // 未设置过期时间
                                tokenErrMsg = "Expiration time is not set";
                            }
                            else if (context.Exception.GetType() == typeof(SecurityTokenException))
                            {
                                // token无效
                                tokenErrMsg = "signature invalid";
                            }
                            else
                            {
                                // 解析失败
                                tokenErrMsg = "Identify failed";
                            }

                            context.Response.Headers.Add("Token-Error", tokenErrMsg);
                            return Task.CompletedTask;
                        }
                    };
                });

            // 如果需要角色控制到Action则需要配置Policy
            // 如果没有配置AddPolicy，接口直接使用[Authorize]特性即可
            // 如果接口只允许Admin或System角色的Token访问，则需要添加了[Authorize("SystemOrAdmin")]特性
            // 详细请测试LoginController的ParseToken的方法
            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireRole("User"));
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
            });
        }
    }
}
