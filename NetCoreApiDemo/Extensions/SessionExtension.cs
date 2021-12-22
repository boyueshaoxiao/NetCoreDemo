using Microsoft.AspNetCore.Http;

namespace NetCoreApiDemo.Extensions
{
    /// <summary>
    /// Session 扩展类
    /// </summary>
    public static class SessionExtension
    {
        /// <summary>
        /// 设置泛型Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObject<T>(this ISession session, string key, T value) where T : class
        {
            session.SetString(key, value.ToJson());
        }

        /// <summary>
        /// 获取泛型Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            var value = session.GetString(key);
            return value.NotNull() ? value.ToObject<T>() : default;
        }
    }
}
