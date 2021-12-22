/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/22 14:14:42
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          UserModel
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;

namespace NetCoreApiDemo.Models
{
    /// <summary>
    /// 用户Model
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string UserRole { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime UserBirthDay { get; set; }
    }
}
