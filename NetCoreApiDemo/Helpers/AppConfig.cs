/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/18 15:54:14
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          AppSettingsHelper
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using NetCoreApiDemo.Enumeration;
using NetCoreApiDemo.Extensions;
using Newtonsoft.Json.Linq;

namespace NetCoreApiDemo.Helpers
{
    /// <summary>
    /// 配置文件读取帮助类
    /// </summary>
    public static class AppConfig
    {
        private static IConfiguration _configuration;
        private static readonly string jsonFile = Directory.GetCurrentDirectory() + "\\appsettings.json";

        ///// <summary>
        ///// 构造方法
        ///// </summary>
        ///// <param name="configuration"></param>
        /////// <param name="optionsMonitor"></param>
        //public AppConfig(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="configuration"></param>
        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region 数据库相关配置

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DbType DbType => Enum.Parse<DbType>(GetAppValue(new[] { "DataBaseSettings", "DbType" }));

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string SqlConnStr => GetAppValue(new[] { "DataBaseSettings", "SqlConnStr" });

        #endregion

        #region JWT相关配置

        /// <summary>
        /// 发行人
        /// </summary>
        public static string Issuer => GetAppValue(new[] { "JwtSetting", "Issuer" });

        /// <summary>
        /// 使用者
        /// </summary>
        public static string Audience => GetAppValue(new[] { "JwtSetting", "Audience" });

        /// <summary>
        /// 密钥
        /// </summary>
        public static string SecretKey => GetAppValue(new[] { "JwtSetting", "SecretKey" });

        /// <summary>
        /// 过期时间
        /// </summary>
        public static int Expire => Convert.ToInt32(GetAppValue(new[] {"JwtSetting", "Expire"}));

        #endregion

        #region 读取AppSettings节点

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string GetAppValue(string[] sections)
        {
            var result = string.Empty;
            try
            {
                if (sections.Any())
                {
                    result = _configuration[string.Join(":", sections)];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置,最大 10 个节点</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetAppValue(string[] sections, string value)
        {
            try
            {
                if (!sections.Any() || sections.Length > 10) return false;

                //_configuration.GetSection(string.Join(":", sections)).Value = value;
                var jsonString = File.ReadAllText(jsonFile, Encoding.UTF8);//读取文件
                var job = JObject.Parse(jsonString);//解析成json

                //根据节点获取索引，更新节点数据
                switch (sections.Length)
                {
                    case 1:
                        job[sections[0]] = value;
                        break;
                    case 2:
                        job[sections[0]][sections[1]] = value;
                        break;
                    case 3:
                        job[sections[0]][sections[1]][sections[2]] = value;
                        break;
                    case 4:
                        job[sections[0]][sections[1]][sections[2]][sections[3]] = value;
                        break;
                    case 5:
                        job[sections[0]][sections[1]][sections[2]][sections[3]][sections[4]] = value;
                        break;
                    case 6:
                        job[sections[0]][sections[1]][sections[2]][sections[3]][sections[4]][sections[5]] = value;
                        break;
                    case 7:
                        job[sections[0]][sections[1]][sections[2]][sections[3]][sections[4]][sections[5]][sections[6]] = value;
                        break;
                    case 8:
                        job[sections[0]][sections[1]][sections[2]][sections[3]][sections[4]][sections[5]][sections[6]][sections[7]] = value;
                        break;
                    case 9:
                        job[sections[0]][sections[1]][sections[2]][sections[3]][sections[4]][sections[5]][sections[6]][sections[7]][sections[8]] = value;
                        break;
                    case 10:
                        job[sections[0]][sections[1]][sections[2]][sections[3]][sections[4]][sections[5]][sections[6]][sections[7]][sections[8]][sections[9]] = value;
                        break;
                    default:
                        job[sections[0]] = value;
                        break;
                }

                var convertString = Convert.ToString(job);//将json装换为string
                File.WriteAllText(jsonFile, convertString);//将内容写进jon文件中

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ErrorMessage());
            }
        }

        #endregion
    }
}
