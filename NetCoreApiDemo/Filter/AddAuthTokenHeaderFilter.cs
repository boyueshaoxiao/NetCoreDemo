/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/7/6 17:03:03
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          AddAuthTokenHeaderFilter
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetCoreApiDemo.Filter
{
    /// <summary>
    /// Swagger 中添加Header 授权Token
    /// </summary>
    public class AddAuthTokenHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();
            var attrs = context.ApiDescription.get();
            foreach (var attr in attrs)
            {
                // 如果 Attribute 是我们自定义的验证过滤器
                if (attr.GetType() == typeof(Auth))
                {
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "AuthToken",
                        In = "header",
                        Type = "string",
                        Required = false
                    });
                }
            }
        }

        //public void Apply(OpenApiOperation operation, OperationFilterContext context)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
