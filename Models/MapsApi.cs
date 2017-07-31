using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Matthew.Models
{
    public static class MapsApi
    {

        // Google Maps API Key
        public static readonly string _Key = "AIzaSyCArlB5Ri6tgsmfZvofuwMeeVQ2Yq0QiYs";

        private static int _MaxSearchResults = 3;

        /// <summary>
        /// Queries google place search api and returns up to the top three results in an Enum of Locations
        /// 
        /// - todo implement postcodes
        /// </summary>
        /// <param name="query">Place to search (ie "33 glengarnock avenue")</param>
        /// <returns></returns>
        public static List<Location> getSearchResults(string query)
        {
            query = query.Replace(" ", "+");    // todo replace with more generatic API friendly parser
            string queryString = "https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + query + "&key=" + _Key; // todo add location parameter to increase search accuracy
            using (var client = new WebClient())
            {
                JObject json = JObject.Parse(client.DownloadString(queryString)); // Async google API query, todo could use try / catch
                results[] locations = json.SelectToken("results").ToObject<results[]>();
                List<Location> results = new List<Location>();
                int numResults = 0;
                foreach (results result in locations)
                {
                    numResults++;
                    Location toAdd = new Location(result.name);
                    toAdd.addLatLng(float.Parse(result.geometry.location.lat), float.Parse(result.geometry.location.lng));
                    string icon = getStaticImageURL(toAdd);   // todo use zoom and size field as a variable / setting
                    toAdd.addIconUrl(icon);
                    results.Add(toAdd);
                    if (numResults == _MaxSearchResults)
                    {
                        break;
                    }
                }
                return results;
            }
        }
        public static string getFormattedAddress(Location location)
        {
            string queryString = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={location.latitude},{location.longitude}&key={_Key}";
            using (var client = new WebClient())
            {
                JObject json = JObject.Parse(client.DownloadString(queryString));
                results[] locations = json.SelectToken("results").ToObject<results[]>();
                return locations[0].formatted_address;
            }

        }

        public static string getStaticImageURL(Location location)
        {
            return $"https://maps.googleapis.com/maps/api/staticmap?zoom=18&size=512x512&center={location.latitude},{location.longitude}&key={_Key}";  // todo use zoom and size field as a variable / setting
        }
    }

    /// <summary>
    /// JSON classes
    /// </summary>
    public class results
    {
        public string name { get; set; }
        public geometry geometry { get; set; }
        public string formatted_address { get; set; }
    }
    public class geometry
    {
        public location location { get; set; }
    }
    public class location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

}