using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OcadoBot.Dialogs
{
    [LuisModel("fa0ef930-1b59-4c42-98f2-8f68ad83246c", "d004b0b064694dd1bec537e3629863fb")]
    [Serializable]
    public class ShopperLUISDialog : LuisDialog<object>
    {
        [NonSerialized]
        private IMessageActivity _originActivity;

        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            _originActivity = await item;
            await base.MessageReceived(context, item);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand: {string.Join(", ", result.Intents.Select(i => i.Intent))}. You can say 'Help' if you are not sure.";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            string message = $"Hi {_originActivity.From.Name}, I'm the Ocado bot, how can I help you today?";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            var messageBody = new StringBuilder();
            messageBody.AppendLine("You can ask me things like");
            messageBody.AppendLine("* Book a delivery ");
            messageBody.AppendLine("* Show me recommendations ");
            messageBody.AppendLine("* See my lists ");
            messageBody.AppendLine("* Check an order ");
            messageBody.AppendLine("* Change an order ");
            message.Text = messageBody.ToString();

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
    }
}