/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/22 17:42:26
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          IHttpResponseResult
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

namespace NetCoreApiDemo.HttpResponse
{
    /// <summary>
    /// 响应数据输出接口
    /// </summary>
    public interface IHttpResponseResult
    {
        /// <summary>
        /// 消息
        /// </summary>
        string Message { get; }
    }

    /// <summary>
    /// 响应数据输出泛型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHttpResponseResult<out T> : IHttpResponseResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        T Data { get; }
    }
}
