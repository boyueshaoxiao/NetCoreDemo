/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/24 14:56:37
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          ITestSingleton
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;

namespace NetCoreApiDemo.Services
{
    /// <summary>
    /// 全局单例
    /// </summary>
    public interface ITestSingleton : IDisposable
    {
        /// <summary>
        /// 打印
        /// </summary>
        void Print();
    }

    /// <summary>
    /// 全局单例
    /// </summary>
    public class TestSingleton : ITestSingleton
    {
        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public void Print()
        {
            Console.WriteLine("=====TestSingleton=====");
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("TestSingleton Dispose");
        }

    }
}
