using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NetCoreApiDemo.Helpers;
using NetCoreApiDemo.MiddlewareExtensions;
using NetCoreApiDemo.Models;
using NetCoreApiDemo.Services;
using Newtonsoft.Json.Serialization;

namespace NetCoreApiDemo
{
    /// <summary>
    /// 启动类，当没有配置 ASPNETCORE_ENVIRONMENT 没有设置时或找不到启动类时才会启动
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //配置AppConfig
            AppConfig.Configure(Configuration);

        }

        /// <summary>
        /// 配置项
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 跨域配置名称
        /// </summary>
        private const string DefaultCorsPolicyName = "AllowCross";

        #region DefaultConfigure

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //var build = new ConfigurationBuilder();
            //build.AddJsonFile("appsettings.json", false, true);
            //var configRoot = build.Build();
            ////var token = configRoot.GetReloadToken();
            //ChangeToken.OnChange(() => configRoot.GetReloadToken(), () =>
            //{
            //    Console.WriteLine("====ChangeToken.OnChange===");
            //});


            services.AddControllers(options =>
                {
                    //禁止去除ActionAsync后缀
                    options.SuppressAsyncSuffixInActionNames = false;
                })
                .AddNewtonsoftJson(options =>
                {
                    //忽略循环引用
                    //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //使用驼峰 首字母小写(默认就是小写开头)
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd hh:mm:ss";
                });


            #region 注册Session

            //注册缓存
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(20);//过期时间
                options.Cookie.HttpOnly = true;//无法通过js获取cookie信息
                options.Cookie.IsEssential = true;//Cookie 是必须的
            });

            #endregion

            #region 注册Swagger

            //自定义中间件
            services.AddAuthorizationService();
            services.AddSwaggerGen(c =>
            {
                //获取注释文档路径  bin\Debug\net5.0\NetCoreApiDemo.xml
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //显示方法注释
                c.IncludeXmlComments(xmlPath, true);
                //c.OrderActionsBy(o => o.RelativePath);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreApiDemo", Version = "v1" });

                #region SwaggerUI 请求头中配置JWT Token信息

                //添加Jwt验证设置,添加请求头信息
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                //授权登录按钮
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Value Bearer {token}",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

                #endregion
            });

            #endregion

            #region 注册跨域

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            #endregion

            #region 注册HttpContext

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion

            #region 注册配置文件读取帮助类

            //services.AddSingleton(new AppConfig(Configuration));

            #endregion

            #region 作用域测试

            services.AddSingleton<ITestSingleton, TestSingleton>();
            services.AddScoped<ITestScoped, TestScoped>();
            services.AddTransient<ITestTransient, TestTransient>();

            #endregion

            #region 闲杂测试

            services.Configure<OperationModel>(Configuration.GetSection($"{OperationModel.Model}:{OperationModel.User}"));
            services.AddTransient<ITestService, TestService>();

            #endregion
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="testService"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITestService testService)
        {
            Console.WriteLine(testService.Message("Hello .Net Core"));
            Console.WriteLine(env.WebRootPath);
            Console.WriteLine(env.ContentRootPath);//C:\讯首WorkSpace\MyCode\NetCoreApiDemo\NetCoreApiDemo
            Console.WriteLine(env.ApplicationName);//NetCoreApiDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            Console.WriteLine(env.WebRootFileProvider);//Microsoft.Extensions.FileProviders.NullFileProvider
            Console.WriteLine(env.ContentRootFileProvider);//Microsoft.Extensions.FileProviders.PhysicalFileProvider
            Console.WriteLine($"ASPNETCORE_ENVIRONMENT is {env.EnvironmentName}. Class is DefaultStartup. Function is Configure.");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                #region Swagger

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreApiDemo v1");
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); //折叠Api
                    c.DefaultModelsExpandDepth(-1); //去除Model 显示
                });

                #endregion
            }

            app.UseStaticFiles();//静态文件

            app.UseRouting();//路由

            app.UseCors(DefaultCorsPolicyName);//跨域

            app.UseAuthentication();//身份验证

            app.UseAuthorization();//身份授权

            app.UseSession();//Session,UseRouting之后，UseEndpoints之前

            #region 自定义类配置

            //配置HttpContext上下文
            HttpContextHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    "default",
                //    "{controller=Home}/{action=Index}{id?}");
            });
        }

        #endregion

        #region Development Configure

        //public void ConfigureDevelopmentServices(IServiceCollection services)
        //{
        //    services.AddControllers();
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreApiDemo", Version = "v1" });
        //    });
        //}

        //public void ConfigureDevelopment(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    Console.WriteLine($"ASPNETCORE_ENVIRONMENT Is {env.EnvironmentName}. Class is DefaultStartup. Function is ConfigureDevelopment.");

        //    app.UseDeveloperExceptionPage();

        //    app.UseStaticFiles();//静态文件

        //    app.UseRouting();//路由

        //    app.UseAuthorization();//授权

        //    app.UseSwagger();
        //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreApiDemo v1"));

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllers();
        //    });
        //}

        #endregion
    }
}
