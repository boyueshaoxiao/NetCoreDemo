using System;
using Microsoft.AspNetCore.Http;
using NetCoreApiDemo.Enumeration;

namespace NetCoreApiDemo.Helpers
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return HttpContextHelper.Current.Request.Cookies.TryGetValue(key, out var result) 
                ? result 
                : string.Empty;
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void Set(string key, string value)
        {
            HttpContextHelper.Current.Response.Cookies.Append(key, value);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire">过期时间</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static void Set(string key, string value, int expire, ExpireType type = ExpireType.Day)
        {
            var nowTime = type switch
            {
                ExpireType.Year => DateTime.Now.AddYears(expire),
                ExpireType.Month => DateTime.Now.AddMonths(expire),
                ExpireType.Hour => DateTime.Now.AddHours(expire),
                ExpireType.Minute => DateTime.Now.AddMinutes(expire),
                ExpireType.Second => DateTime.Now.AddSeconds(expire),
                ExpireType.Day => DateTimeOffset.Now.AddDays(expire),
                _ => DateTimeOffset.Now.AddDays(expire)
            };
            HttpContextHelper.Current.Response.Cookies.Append(key, value, new CookieOptions { Expires = nowTime });
        }

        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            HttpContextHelper.Current.Response.Cookies.Delete(key);
        }

    }
}
