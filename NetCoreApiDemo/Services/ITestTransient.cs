/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/24 15:02:43
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          ITestTransient
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;

namespace NetCoreApiDemo.Services
{
    /// <summary>
    /// 瞬时单例
    /// </summary>
    public interface ITestTransient: IDisposable
    {
        /// <summary>
        /// 打印
        /// </summary>
        void Print();
    }

    /// <summary>
    /// 瞬时单例
    /// </summary>
    public class TestTransient : ITestTransient
    {
        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public void Print()
        {
            Console.WriteLine("=====TestTransient=====");
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("TestTransient Dispose");
        }
    }
}
