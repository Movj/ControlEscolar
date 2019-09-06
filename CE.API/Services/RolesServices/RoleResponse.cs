using CE.API.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services.RolesServices
{
    public class RoleResponse : BaseResponse
    {
        public Entities.Role Role { get; private set; }

        private RoleResponse(bool success, string message, Entities.Role role) : base(success, message)
        {
            Role = role;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="user">Saved user.</param>
        /// <returns>Response.</returns>
        public RoleResponse(Entities.Role role) : this(true, string.Empty, role)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public RoleResponse(string message) : this(false, message, null)
        { }
    }
}
