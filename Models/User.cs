using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matthew.Models
{
    /// <summary>
    /// User is the Model for a user the bot commincates with
    /// 
    /// </summary>
    [Serializable]
    public class User
    {


        public Dictionary<string, Location> Locations;    /// List of users stored locations ~ eg {"home":homeLocation}
        public Location currentLocation;   // needs to be set for location search to work

        [Prompt("What should I call you?")]
        public string Name { get; set; }     // Name of the user according to the Bot Framework


        public static IForm<User> BuildUserForm()
        {
            return new FormBuilder<User>()
                .Field(nameof(Name))
                .Build();                  
        }
    }
}