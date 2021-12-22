/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/22 16:50:51
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          DateTimeExtension
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;

namespace NetCoreApiDemo.Extensions
{
    /// <summary>
    /// 时间DateTime扩展类
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 格式化时间格式 yyyy-MM-dd hh:mm:ss
        /// </summary>
        /// <returns></returns>
        public static string ToFormatString(this DateTime date, string format = "yyyy-MM-dd hh:mm:ss")
        {
            return date.ToString(format);
        }
    }
}
