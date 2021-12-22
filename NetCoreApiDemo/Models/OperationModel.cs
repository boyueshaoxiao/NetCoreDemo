/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/6/24 15:49:24
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          OperationModel
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

namespace NetCoreApiDemo.Models
{
    /// <summary>
    /// Options 操作Model
    /// </summary>
    public class OperationModel
    {
        /// <summary>
        /// Model
        /// </summary>
        public const string Model = "Model";

        /// <summary>
        /// User
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }
}
