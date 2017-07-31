using AdaptiveCards;
using Matthew.Models;
using Matthew.Models.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace Matthew.Dialogs
{
    // App ID                                           // Programmatic API Key
    [LuisModel("ee87db67-f7c3-4479-98b3-4b6e0ce2a469", "c63b0c2af2db481c9cf0f67801264d8f")]
    [Serializable]
    public class LUISParser : LuisDialog<Object>
    {
        public User currentUser;

        //private ShortTermMem _ShortTermMem;
        //public LUISParser(ShortTermMem mem)
        //{
        //    this._ShortTermMem = mem;
        //}

        [LuisIntent("Greet")]
        public async Task _Greet(IDialogContext context, LuisResult result)
        {
            //Conversation.SendAsync(context.Activity, () => { return Chain.From(() => FormDialog.FromForm(User.BuildUserForm)); });
        }

        /// <summary>
        /// Code to handle adding favourite places with Google API and confirmation
        /// </summary>
        [LuisIntent("Places.AddFavoritePlace")]
        public async Task AddPlace(IDialogContext context, LuisResult result)
        {
            ShortTermMem.updateMemory(result);   // keep the bot's mind paying attention
            string userGuess = ShortTermMem.getLastEntityValue("Places.AbsoluteLocation");  // todo add null check here ( "" check in this case) in case no results
                                                                                            //context.SayAsync($"ShortTermMem: {string.Join(";", ShortTermMem.lastEntities.Select(x => x.Item1 + "=" + x.Item2).ToArray())}"); // sweet debug line, keep it somewhere
            Location toAdd = new Location();
            List<Location> searchResults = MapsApi.getSearchResults(userGuess);
            //context.SayAsync($"Locations Address Parsing Test: {searchResults.FirstOrDefault().address}");   // todo make pretty with buttons
            IMessageActivity validate = context.MakeMessage(); 
            validate.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            validate.Attachments = new List<Attachment>();
            foreach(Location searchResult in searchResults)
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
            await context.PostAsync(validate);  // todo add labelling stage

            // todo end each LuisIntent function with context.wait(CustomClass.CustomFunction);
        }
        // todo add below method with above logic to new file to keep LUISParse org
        private async Task OnLocationSelected(IDialogContext context, IAwaitable<Location> result)
        {
            context.Wait(MessageReceived);
        }

        [LuisIntent("Places.GetRoute")]
        public async Task GetRoute(IDialogContext context, LuisResult result)
        {
            // do route somewhere form
        }

        [LuisIntent("Places.AddFavourite")]
        public async Task addFavourite(IDialogContext context, LuisResult result)
        {
            //context.UserData.SetValue<>;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry to say I don't know what you mean.");
            context.Wait(MessageReceived);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            var choices = await result;
            context.Wait(MessageReceived);
        }
    }

}