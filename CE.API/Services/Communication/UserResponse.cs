using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services.Communication
{
    public class UserResponse : BaseResponse
    {
        public Entities.Usuario User { get; private set; }

        private UserResponse(bool success, string message, Entities.Usuario user) : base(success, message)
        {
            User = user;
        }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="user">Saved user.</param>
        /// <returns>Response.</returns>
        public UserResponse(Entities.Usuario user) : this(true, string.Empty, user)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public UserResponse(string message) : this(false, message, null)
        { }
    }
}
