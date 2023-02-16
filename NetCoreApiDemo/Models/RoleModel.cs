/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/22 14:14:42
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          UserModel
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace NetCoreApiDemo.Models
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class RoleModel
    {
        /// <summary>
        /// 角色
        /// </summary>
        public IEnumerable<string> UserRoles { get; set; }
    }
}
