/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/18 17:14:59
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          TestService
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

namespace NetCoreApiDemo.Services
{
    /// <summary>
    /// 测试类
    /// </summary>
    public class TestService : ITestService
    {
        /// <summary>
        /// 获取Message
        /// </summary>
        /// <returns></returns>
        public string Message(string message)
        {
            return message;
        }
    }


}
