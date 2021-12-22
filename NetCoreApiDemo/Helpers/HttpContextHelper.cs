using Microsoft.AspNetCore.Http;

namespace NetCoreApiDemo.Helpers
{
    /// <summary>
    /// 静态类中获取HttpContext上下文
    /// </summary>
    public static class HttpContextHelper
    {
        private static IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// 注入IHttpContextAccessor获取HttpContext上下文
        /// </summary>
        /// <param name="contextAccessor"></param>
        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// HttpContext上下文
        /// </summary>
        public static HttpContext Current => _contextAccessor.HttpContext;
    }
}
