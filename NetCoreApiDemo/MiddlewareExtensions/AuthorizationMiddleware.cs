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
                    // 令牌验证参数
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 设置生成token的秘钥
                        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppConfig.SecretKey)),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.SecretKey)),
                        // 验证秘钥
                        ValidateIssuerSigningKey = true,
                        // 验证发布者
                        ValidateIssuer = true,
                        // 验证Issure
                        ValidIssuer = AppConfig.Issuer,//发行人
                        // 验证接收者
                        ValidateAudience = true,
                        // 读配置Audience
                        ValidAudience = AppConfig.Audience,//订阅人
                        // 验证过期时间
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                        RequireExpirationTime = true
                    };

                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            // 如果过期，则把<是否过期>添加到，返回头信息中
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireRole("User").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));

            });
        }
    }
}
