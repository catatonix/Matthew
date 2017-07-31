using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matthew.Models.Forms
{
    public abstract class Form
    {

        public abstract IForm<Form> BuildForm();
    }
}