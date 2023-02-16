namespace NetCoreApiDemo.Enumeration
{
    /// <summary>
    /// Cookie过期类型枚举
    /// </summary>
    public enum ExpireType
    {
        /// <summary>
        /// 年
        /// </summary>
        Year,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 日
        /// </summary>
        Day,
        /// <summary>
        /// 时
        /// </summary>
        Hour,
        /// <summary>
        /// 分
        /// </summary>
        Minute,
        /// <summary>
        /// 秒
        /// </summary>
        Second
    }

    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DbType
    {
        /// <summary>
        /// MsSqlServer
        /// </summary>
        MsSqlServer = 0,

        /// <summary>
        /// MySql
        /// </summary>
        MySql = 1,

        /// <summary>
        /// Oracle
        /// </summary>
        Oracle = 2,

        /// <summary>
        /// SqLite
        /// </summary>
        SqLite = 3,

        /// <summary>
        /// PostGreSQL
        /// </summary>
        PostGreSql = 4
    }

    /// <summary>
    /// Jwt Claim Key
    /// </summary>
    public class JwtClaimKey
    {
        /// <summary>
        /// userId
        /// </summary>
        public static string UserId = "userId";

        /// <summary>
        /// userName
        /// </summary>
        public static string UserName = "userName";
    }
}
