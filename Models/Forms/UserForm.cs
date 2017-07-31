using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matthew.Models.Forms
{
    [Serializable]
    public class UserForm
    {
        [Prompt("What's your name?")]
        public string name;

        [Prompt("Where do you live? (you don't have to tell me!")]
        public string homeAddress;

        public static IForm<UserForm> BuildForm()
        {
            var builder = new FormBuilder<UserForm>();

            return builder
                .Message("Let me get to know you...")
                .Field(nameof(name))
                .Field(nameof(homeAddress))
                .Build();
        }
    }
}