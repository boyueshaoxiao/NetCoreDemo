using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCoreApiDemo.Enumeration;
using NetCoreApiDemo.Extensions;
using NetCoreApiDemo.Helpers;
using NetCoreApiDemo.HttpResponse;
using NetCoreApiDemo.Models;
using NetCoreApiDemo.Services;

namespace NetCoreApiDemo.Controllers
{
    /// <summary>
    /// 测试API
    /// </summary>
    [Route("api/test/[action]")]
    public class TestController : BaseController
    {
        private readonly ILogger<TestController> _logger;
        private readonly OperationModel _options;
        private readonly OperationModel _snapshot;
        private readonly OperationModel _monitor;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        /// <param name="snapshot"></param>
        /// <param name="monitor"></param>
        public TestController(ILogger<TestController> logger,
            IOptions<OperationModel> options,
            IOptionsSnapshot<OperationModel> snapshot,
            IOptionsMonitor<OperationModel> monitor)
        {
            _logger = logger;
            _options = options.Value;
            _snapshot = snapshot.Value;
            _monitor = monitor.CurrentValue;
        }

        /// <summary>
        /// 日志测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult WriteLog()
        {
            Console.WriteLine("IOptions：" + _options.Name);
            Console.WriteLine("IOptionsSnapshot：" + _snapshot.Name);
            Console.WriteLine("IOptionsMonitor：" + _monitor.Name);
            try
            {
                SessionHelper.Get("Dennis");
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.ErrorMessage());
            }

            //_logger.LogInformation("LogInformation");
            //_logger.LogWarning("LogWarning");
            //_logger.LogError("LogError");
            return Ok();
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            AppConfig.SetAppValue(new[] { "Model", "User", "Id" }, "123456");
            AppConfig.SetAppValue(new[] { "Model", "User", "Name" }, "NewDennis");
            var ip = "null";
            if (HttpContext.Connection.RemoteIpAddress != null)
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return new[] { ip, DateTime.Now.ToFormatString() };
        }

        /// <summary>
        /// 获取传入参数id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IHttpResponseResult> Get(int id)
        {
            _logger.LogInformation("Get Id");
            var result = await Task.Run(() => new[] { id.ToString(), DateTime.Now.ToFormatString() });
            return HttpResponseResult.Success(result);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <returns></returns>
        [HttpGet("{key}")]
        [Authorize]
        public async Task<IHttpResponseResult> GetCookie(string key = "Dennis")
        {
            _logger.LogInformation("GetCookie");
            //return $"Session:{SessionHelper.Get<List<UserModel>>(key)[0].UserBirthDay},Cookie:{CookieHelper.Get(key)}";
            var session = await Task.Run(() => SessionHelper.Get<List<RoleModel>>(key));
            return HttpResponseResult.Success(session);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <returns></returns>
        [HttpPost("key")]
        [Authorize]
        public ActionResult SetCookie(string key = "Dennis")
        {
            var model = new List<UserModel>
            {
                new UserModel
                {
                    UserId = Guid.NewGuid().ToString(),
                    UserName = "Dennis"
                },
                new UserModel
                {
                    UserId = Guid.NewGuid().ToString(),
                    UserName = "Roger"
                }
            };

            SessionHelper.Set(key, model);
            CookieHelper.Set(key, "Cookie Value is Dennis", 3, ExpireType.Minute);
            CookieHelper.Set("Test", "Cookie Value is Dennis");
            return Ok();
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <returns></returns>
        [HttpDelete("{key}")]
        public ActionResult RemoveCookie(string key = "Dennis")
        {
            SessionHelper.Remove(key);
            CookieHelper.Remove(key);
            return Ok();
        }

        /// <summary>
        /// 获取数据库配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetDataBase()
        {
            return $"DbType:{AppConfig.DbType},SqlConnStr:{AppConfig.SqlConnStr}";
        }


        /// <summary>
        /// 依赖注入作用域
        /// </summary>
        /// <param name="testSingleton"></param>
        /// <param name="testSingleton1"></param>
        /// <param name="testScoped"></param>
        /// <param name="testScoped1"></param>
        /// <param name="testTransient"></param>
        /// <param name="testTransient1"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetScope([FromServices] ITestSingleton testSingleton, [FromServices] ITestSingleton testSingleton1,
                  [FromServices] ITestScoped testScoped, [FromServices] ITestScoped testScoped1,
                  [FromServices] ITestTransient testTransient, [FromServices] ITestTransient testTransient1)
        {
            //获取请求作用域(请求容器)
            var requestServices = HttpContext.RequestServices;

            //在请求作用域下创建子作用域
            using var scope = requestServices.CreateScope();

            //在子作用域中通过其容器获取注入的不同生命周期对象
            var testSingleton11 = scope.ServiceProvider.GetService<ITestSingleton>();
            var testScoped11 = scope.ServiceProvider.GetService<ITestScoped>();
            var testTransient11 = scope.ServiceProvider.GetService<ITestTransient>();

            var testSingleton12 = scope.ServiceProvider.GetService<ITestSingleton>();
            var testScoped12 = scope.ServiceProvider.GetService<ITestScoped>();
            var testTransient12 = scope.ServiceProvider.GetService<ITestTransient>();

            Console.WriteLine("================Singleton=============");
            Console.WriteLine($"请求作用域的ITestSingleton对象:{testSingleton.GetHashCode()}");
            Console.WriteLine($"请求作用域的ITestSingleton1对象:{testSingleton1.GetHashCode()}");
            Console.WriteLine($"请求作用域下子作用域的ITestSingleton11对象:{testSingleton11.GetHashCode()}");
            Console.WriteLine($"请求作用域下子作用域的ITestSingleton12对象:{testSingleton12.GetHashCode()}");

            Console.WriteLine("================Scoped=============");
            Console.WriteLine($"请求作用域的ITestScoped对象:{testScoped.GetHashCode()}");
            Console.WriteLine($"请求作用域的ITestScoped1对象:{testScoped1.GetHashCode()}");
            Console.WriteLine($"请求作用域下子作用域的ITestScoped11对象:{testScoped11.GetHashCode()}");
            Console.WriteLine($"请求作用域下子作用域的ITestScoped12对象:{testScoped12.GetHashCode()}");

            Console.WriteLine("================Transient=============");
            Console.WriteLine($"请求作用域的ITestTransient对象:{testTransient.GetHashCode()}");
            Console.WriteLine($"请求作用域的ITestTransient1对象:{testTransient1.GetHashCode()}");
            Console.WriteLine($"请求作用域下子作用域的ITestTransient11对象:{testTransient11.GetHashCode()}");
            Console.WriteLine($"请求作用域下子作用域的ITestTransient12对象:{testTransient12.GetHashCode()}");

            return "TestServiceScope";
        }



        /// <summary>
        /// POST
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public RoleModel Post([FromBody] RoleModel value)
        {
            return value;
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] string value)
        {
            return value;
        }
    }
}
