/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/22 11:30:36
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          ObjectExtension
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using Newtonsoft.Json;

namespace NetCoreApiDemo.Extensions
{
    /// <summary>
    /// Object 扩展
    /// </summary>
    public static class ObjectExtension
    {
        #region 非空判断

        /// <summary>
        /// 非NULL或""或WhiteSpace
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool NotNull(this object obj)
        {
            return obj != null && !string.IsNullOrEmpty(obj.ToString()) && !string.IsNullOrWhiteSpace(obj.ToString());
        }

        /// <summary>
        /// NULL、""、WhiteSpace
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Null(this object obj)
        {
            return !NotNull(obj);
        }

        #endregion

        #region Object和Json相互转换

        /// <summary>
        /// Object To Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            //return JSON.Serialize(obj);
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Json To Object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json) where T : class
        {
            //return JSON.Deserialize<T>(json);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion
    }
}
