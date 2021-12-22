/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/22 10:53:45
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          ExceptionExtension
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;
using System.Text;

namespace NetCoreApiDemo.Extensions
{
    /// <summary>
    /// 异常扩展
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// 扩展方法获取Message和StackTrace
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ErrorMessage(this Exception exception)
        {
            var expSb = new StringBuilder();
            expSb.Append(exception.Message);
            expSb.Append("\n");
            expSb.Append(exception.StackTrace);
            if (exception.InnerException == null) return expSb.ToString();
            expSb.Append("\n");
            expSb.Append(exception.InnerException.Message);
            expSb.Append("\n");
            expSb.Append(exception.InnerException.StackTrace);
            return expSb.ToString();
        }
    }
}
