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
            string userGuess = ShortTermMem.getLastEntityValue("Places.AbsoluteLocation");  // todo add null check here ( "" check in this case) in case no results (throw special exception from ShortTerm Mem?
            Location toAdd = new Location();
            toAdd.setLocation(context, userGuess);
              // todo add labelling stage

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