/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/22 17:42:05
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          HttpResponseResult
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

namespace NetCoreApiDemo.HttpResponse
{
    /// <summary>
    /// 响应数据输出
    /// </summary>
    public class HttpResponseResult<T> : IHttpResponseResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        public HttpResponseResult<T> Success(T data, string msg = null)
        {
            Code = 0;
            Data = data;
            Message = msg;

            return this;
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public HttpResponseResult<T> Fail(int code = -1, string msg = null, T data = default)
        {
            Code = code;
            Message = msg;
            Data = data;
            return this;
        }
    }

    /// <summary>
    /// 响应数据静态输出
    /// </summary>
    public static class HttpResponseResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static IHttpResponseResult Success<T>(T data = default, string msg = null)
        {
            return new HttpResponseResult<T>().Success(data, msg);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static IHttpResponseResult Success()
        {
            return Success<string>();
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static IHttpResponseResult Fail<T>(int code = -1, string msg = null, T data = default(T))
        {
            return new HttpResponseResult<T>().Fail(code, msg, data);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static IHttpResponseResult Fail(int code = -1, string msg = null)
        {
            return new HttpResponseResult<string>().Fail(code, msg);
        }

        /// <summary>
        /// 根据布尔值返回结果
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IHttpResponseResult Result<T>(bool success)
        {
            return success ? Success<T>() : Fail<T>();
        }

        /// <summary>
        /// 根据布尔值返回结果
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IHttpResponseResult Result(bool success)
        {
            return success ? Success() : Fail();
        }
    }
}
