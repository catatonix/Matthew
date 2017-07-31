using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
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
    /// currently bulky API calls, only really need one or two not the near 6 requests I end up making
    /// </summary>
    [Serializable]
    public class Location
    {
        public JObject json;

        public float latitude { get; set; }
        public float longitude { get; set; }
        public string iconURL { get; set; }
        public Boolean verified = false;       // whether or not this location has been confirmed with API // todo decide if this is required anymore..

        public string name { get; set; }

        public string address { get; set; }

        public string formatted_address;

        

        public void addIconUrl(string url)  // TODO take care of this in the setter methods within this class
        {
            iconURL = url;
        }

        public void addLatLng(float lat, float lng)
        {
            latitude = lat;
            longitude = lng;
            formatted_address = MapsApi.getFormattedAddress(this);
            iconURL = MapsApi.getStaticImageURL(this);
        }
        public Location() { }



        /// <summary>
        /// Sets the location a search query and clarifies if necessary
        /// </summary>
        /// <param name="context">Conversation context</param>
        /// <param name="searchQuery">Name of location to add</param>
        /// <returns>returns false if 0 results, true otherwise.</returns>
        public bool setLocation(IDialogContext context, string searchQuery)
        {
            List<Location> searchResults = MapsApi.getSearchResults(searchQuery);
            IMessageActivity validate = context.MakeMessage();
            validate.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            validate.Attachments = new List<Attachment>();
            foreach (Location searchResult in searchResults)
            {
                HeroCard resultCard = new HeroCard();
                CardImage resultImage = new CardImage();
                List<CardImage> cardImages = new List<CardImage>(1);
                resultImage.Url = searchResult.iconURL;
                cardImages.Add(resultImage);
                resultCard.Title = searchResult.formatted_address;
                resultCard.Images = cardImages;
                validate.Attachments.Add(resultCard.ToAttachment());
            }
            context.PostAsync(validate);
            return searchResults.Count > 0;
        }

        public bool setLocationLucky(string searchQuery)
        {
            try
            {
                List<Location> searchResults = MapsApi.getSearchResults(searchQuery);
                this.addLatLng(searchResults[0].latitude, searchResults[0].longitude);
                this.name = searchResults[0].name;
                return true;
            }
            catch
            {
                return false;
            }

        }

        public override string ToString()   // to allow displaying in buttons
        {
            return formatted_address;
        }


    }
}