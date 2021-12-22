/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/24 15:01:35
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          ITestScoped
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;

namespace NetCoreApiDemo.Services
{
    /// <summary>
    /// 范围单例
    /// </summary>
    public interface ITestScoped : IDisposable
    {
        /// <summary>
        /// 打印
        /// </summary>
        void Print();
    }

    /// <summary>
    /// 范围单例
    /// </summary>
    public class TestScoped : ITestScoped
    {
        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public void Print()
        {
            Console.WriteLine("=====TestScoped=====");
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("TestScoped Dispose");
        }

    }
}
