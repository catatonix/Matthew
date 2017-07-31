using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matthew.Models

    // todo replace with enum in User class, unnessecary extra file
{
    /// <summary>
    /// Enum for storing permission level
    /// </summary>
    public class PermissionsStatus
    {

        public enum Status   /// Permissions level of the Account
        {
            Locked,
            Normal,
            Raised,
            Admin
        }

    }
}