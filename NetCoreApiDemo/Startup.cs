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
    /// �����࣬��û������ ASPNETCORE_ENVIRONMENT û������ʱ���Ҳ���������ʱ�Ż�����
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //����AppConfig
            AppConfig.Configure(Configuration);

        }

        /// <summary>
        /// ������
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ������������
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
                    //��ֹȥ��ActionAsync��׺
                    options.SuppressAsyncSuffixInActionNames = false;
                })
                .AddNewtonsoftJson(options =>
                {
                    //����ѭ������
                    //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //ʹ���շ� ����ĸСд(Ĭ�Ͼ���Сд��ͷ)
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd hh:mm:ss";
                });


            #region ע��Session

            //ע�Ỻ��
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(20);//����ʱ��
                options.Cookie.HttpOnly = true;//�޷�ͨ��js��ȡcookie��Ϣ
                options.Cookie.IsEssential = true;//Cookie �Ǳ����
            });

            #endregion

            #region ע��Swagger

            //�Զ����м��
            services.AddAuthorizationService();
            services.AddSwaggerGen(c =>
            {
                //��ȡע���ĵ�·��  bin\Debug\net5.0\NetCoreApiDemo.xml
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //��ʾ����ע��
                c.IncludeXmlComments(xmlPath, true);
                //c.OrderActionsBy(o => o.RelativePath);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreApiDemo", Version = "v1" });

                #region SwaggerUI ����ͷ������JWT Token��Ϣ

                //���Jwt��֤����,�������ͷ��Ϣ
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

                //��Ȩ��¼��ť
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Value Bearer {token}",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });

                #endregion
            });

            #endregion

            #region ע�����

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            #endregion

            #region ע��HttpContext

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion

            #region ע�������ļ���ȡ������

            //services.AddSingleton(new AppConfig(Configuration));

            #endregion

            #region ���������

            services.AddSingleton<ITestSingleton, TestSingleton>();
            services.AddScoped<ITestScoped, TestScoped>();
            services.AddTransient<ITestTransient, TestTransient>();

            #endregion

            #region ���Ӳ���

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
            Console.WriteLine(env.ContentRootPath);//C:\Ѷ��WorkSpace\MyCode\NetCoreApiDemo\NetCoreApiDemo
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
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); //�۵�Api
                    c.DefaultModelsExpandDepth(-1); //ȥ��Model ��ʾ
                });

                #endregion
            }

            app.UseStaticFiles();//��̬�ļ�

            app.UseRouting();//·��

            app.UseCors(DefaultCorsPolicyName);//����

            app.UseAuthentication();//�����֤

            app.UseAuthorization();//�����Ȩ

            app.UseSession();//Session,UseRouting֮��UseEndpoints֮ǰ

            #region �Զ���������

            //����HttpContext������
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

        //    app.UseStaticFiles();//��̬�ļ�

        //    app.UseRouting();//·��

        //    app.UseAuthorization();//��Ȩ

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
