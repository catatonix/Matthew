using Microsoft.Bot.Builder.FormFlow;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Matthew.Models
{
    /// <summary>
    /// Object to store a geolocation with metadata
    /// 
    /// - potential to add location validation here
    /// - currently based around google maps api json form
    /// 
    /// all attributes need updating when some core values are modified
    /// </summary>
    [Serializable]
    public class Location
    {
        public JObject json;

        public float latitude { get; set; }
        public float longitude { get; set; }
        public string iconURL { get; set; }
        public Boolean verified = false;       // whether or not this location has been confirmed with API // todo decide if this is required anymore..

        string name;

        public string address { get; set; }

        public string formatted_address;


  
        public Location(string name)
        {
            this.name = name;     // todo tidy (maybe use this)
        }

        public void addIconUrl(string url)  // TODO take care of this in the setter methods within this class
        {
            iconURL = url;
        }

        public void addLatLng(float lat, float lng)
        {
            latitude = lat;
            longitude = lng;
            formatted_address = MapsApi.getFormattedAddress(this);
        }

        public override string ToString()   // to allow displaying in buttons
        {
            return formatted_address;
        }


    }
}