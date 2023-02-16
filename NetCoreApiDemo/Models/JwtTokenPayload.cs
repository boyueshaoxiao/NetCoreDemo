using System.Collections.Generic;

namespace NetCoreApiDemo.Models
{
    /// <summary>
    /// Jwt Token Payload
    /// </summary>
    public class JwtTokenPayload
    {
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// UserRoles
        /// </summary>
        public IEnumerable<string> UserRoles { get; set; }
    }
}
