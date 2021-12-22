using Microsoft.AspNetCore.Http;
using NetCoreApiDemo.Extensions;

namespace NetCoreApiDemo.Helpers
{
    /// <summary>
    /// Session 帮助类
    /// </summary>
    public static class SessionHelper
    {
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return HttpContextHelper.Current.Session.GetString(key);
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            return HttpContextHelper.Current.Session.GetObject<T>(key);
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void Set(string key, string value)
        {
            HttpContextHelper.Current.Session.SetString(key, value);
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void Set<T>(string key, T value) where T : class
        {
            HttpContextHelper.Current.Session.SetObject(key, value);
        }

        /// <summary>
        /// 移除Session
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            HttpContextHelper.Current.Session.Remove(key);
        }
    }
}
