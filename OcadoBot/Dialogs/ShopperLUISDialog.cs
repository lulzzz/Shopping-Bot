using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ShopperBot.Dialogs
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
            string message = $"Sorry I did not understand. You can say 'Help' if you are not sure what to do.";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            string message = $"Hi {_originActivity.From.Name}. I'm Octo, the Ocado bot, how can I help you today?";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            try
            {
                var message = (Activity)context.MakeMessage();
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
            catch (Exception ex)
            {
                var message = context.MakeMessage();
                message.Type = "message";
                message.TextFormat = "markdown";
                var messageBody = new StringBuilder();
                messageBody.AppendLine(ex.Message);
                message.Text = messageBody.ToString();
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("BookDeliverySlot")]
        public async Task BookDeliverySlot(IDialogContext context, LuisResult result)
        {
            //Initial message
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            var messageBody = new StringBuilder();
            messageBody.AppendLine("Sure, I can book a delivery slot for you. I know you normally prefer Mondays, I have these slots available next Monday 12th September:");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);

            //fake a delay
            Delay(context, 1, true);

            //hero with buttons
            var messageButtons = (Activity)context.MakeMessage();
            messageButtons.Recipient = messageButtons.From;
            messageButtons.Type = "message";
            messageButtons.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            cardButtons.Add(new CardAction(){Value = "9:00",Type = "imBack",Title = "9:00"});
            cardButtons.Add(new CardAction() { Value = "11:45", Type = "imBack", Title = "11:45" });
            cardButtons.Add(new CardAction() { Value = "13:00", Type = "imBack", Title = "13:00" });
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://static.guim.co.uk/sys-images/Guardian/Pix/cartoons/2010/6/7/1275929867641/ocado-van-006.jpg"));
            HeroCard plCard = new HeroCard()
            {
                Title = "Available delivery slots for Monday 12th",
                Subtitle = "Please choose a slot",
                Images = cardImages,
                Buttons = cardButtons
            };
            messageButtons.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(messageButtons);

            //wait for user
            context.Wait(MessageReceived);
        }

        [LuisIntent("NotHappy")]
        public async Task NotHappy(IDialogContext context, LuisResult result)
        {
            //fake a delay
            Delay(context, 1, true);

            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();

            var messageBody = new StringBuilder();
            messageBody.AppendLine("Oh you don't seem happy about that suggestion. Would any other days work for you?");
            message.Text = messageBody.ToString();

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("HowAboutTuesday")]
        public async Task HowAboutTuesday(IDialogContext context, LuisResult result)
        {
            //fake a delay
            Delay(context, 2, true);

            //initial message
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();
            var messageBody = new StringBuilder();
            messageBody.AppendLine("Sure, I have these slots available next Tuesday 13th September:");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);

            //hero with buttons
            var messageButtons = (Activity)context.MakeMessage();
            messageButtons.Recipient = messageButtons.From;
            messageButtons.Type = "message";
            messageButtons.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            cardButtons.Add(new CardAction() { Value = "9:00", Type = "imBack", Title = "9:00" });
            cardButtons.Add(new CardAction() { Value = "10:30", Type = "imBack", Title = "10:30" });
            cardButtons.Add(new CardAction() { Value = "12:00", Type = "imBack", Title = "12:00" });
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://static.guim.co.uk/sys-images/Guardian/Pix/cartoons/2010/6/7/1275929867641/ocado-van-006.jpg"));
            HeroCard plCard = new HeroCard()
            {
                Title = "Available delivery slots for Tuesday 13th",
                Subtitle = "Please choose a slot",
                Images = cardImages,
                Buttons = cardButtons
            };
            messageButtons.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(messageButtons);

            //wait for user
            context.Wait(MessageReceived);
        }

        [LuisIntent("Book1030")]
        public async Task Book1030(IDialogContext context, LuisResult result)
        {
            //initial message
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();
            var messageBody = new StringBuilder();
            messageBody.AppendLine("Thanks, I've booked a delivery for 10:30 on Tuesday 13th September for you");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);

            //were ready message
            var messageReady = context.MakeMessage();
            messageReady.Type = "message";
            messageReady.TextFormat = "markdown";
            messageReady.Attachments = new List<Attachment>();
            var messageReadyBody = new StringBuilder();
            messageReadyBody.AppendLine("We're ready to make an order, here are your saved shopping lists, which one would you like to use?");
            messageReady.Text = messageReadyBody.ToString();
            await context.PostAsync(messageReady);

            //hero with buttons
            var messageButtons = (Activity)context.MakeMessage();
            messageButtons.Recipient = messageButtons.From;
            messageButtons.Type = "message";
            messageButtons.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            cardButtons.Add(new CardAction() { Value = "Weekly Shop", Type = "imBack", Title = "Weekly Shop" });
            cardButtons.Add(new CardAction() { Value = "Big Shop", Type = "imBack", Title = "Big Shop" });
            cardButtons.Add(new CardAction() { Value = "Weekend drinks and nibbles", Type = "imBack", Title = "Weekend drinks and nibbles" });
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://lapalmemagazine.com/wp-content/uploads/2014/11/lots-of-groceries.jpg"));
            HeroCard plCard = new HeroCard()
            {
                Title = "Your stored shopping lists",
                Subtitle = "Please choose a list",
                Images = cardImages,
                Buttons = cardButtons
            };
            messageButtons.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(messageButtons);

            //wait for user
            context.Wait(MessageReceived);
        }

        [LuisIntent("SeeLists")]
        public async Task SeeLists(IDialogContext context, LuisResult result)
        {
            //fake a delay
            Delay(context, 2, true);

            //hero with buttons
            var messageButtons = (Activity)context.MakeMessage();
            messageButtons.Recipient = messageButtons.From;
            messageButtons.Type = "message";
            messageButtons.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            cardButtons.Add(new CardAction() { Value = "Weekly Shop", Type = "imBack", Title = "Weekly Shop" });
            cardButtons.Add(new CardAction() { Value = "Big Shop", Type = "imBack", Title = "Big Shop" });
            cardButtons.Add(new CardAction() { Value = "Weekend drinks and nibbles", Type = "imBack", Title = "Weekend drinks and nibbles" });
            HeroCard plCard = new HeroCard()
            {
                Title = "Your stored shopping lists",
                Subtitle = "Please choose a list",
                Images = null,
                Buttons = cardButtons
            };
            messageButtons.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(messageButtons);

            //wait for user
            context.Wait(MessageReceived);
        }

        [LuisIntent("WeekendDrinksNibbles")]
        public async Task WeekendDrinksNibbles(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();
            var messageBody = new StringBuilder();
            messageBody.AppendLine("Sure, I'll use the Weekend Drinks and Nibbles list. Are you interested in any additional recommended products?");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Beers")]
        public async Task Beers(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();
            var messageBody = new StringBuilder();
            messageBody.AppendLine("OK, we have an 'Any 3 for £5' offer on offer for bottled beers at the moment. You have 4 items that qualify already in your Weekend Drinks and Nibbles list, can I recommend any two of the following to complete the offer?");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);

            //hero with buttons
            var messageButtons = (Activity)context.MakeMessage();
            messageButtons.Recipient = messageButtons.From;
            messageButtons.Type = "message";
            messageButtons.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            cardButtons.Add(new CardAction() { Value = "London Pride £1.79", Type = "imBack", Title = "London Pride £1.79" });
            cardButtons.Add(new CardAction() { Value = "Doom Bar Bitter £1.79", Type = "imBack", Title = "Doom Bar Bitter £1.79" });
            cardButtons.Add(new CardAction() { Value = "Sierra Nevada Pale Ale £1.79", Type = "imBack", Title = "Sierra Nevada Pale Ale £1.79" });
            HeroCard plCard = new HeroCard()
            {
                Title = "Some suggestions",
                Subtitle = "Optionally choose one of these suggestions",
                Images = null,
                Buttons = cardButtons
            };
            messageButtons.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(messageButtons);

            //wait for user
            context.Wait(MessageReceived);
        }

        [LuisIntent("LondonPride")]
        public async Task LondonPride(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();
            var messageBody = new StringBuilder();
            messageBody.AppendLine("OK, I'll add London Pride £1.79 please choose one more to complete the offer?");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);


            //hero with buttons
            var messageButtons = (Activity)context.MakeMessage();
            messageButtons.Recipient = messageButtons.From;
            messageButtons.Type = "message";
            messageButtons.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            cardButtons.Add(new CardAction() { Value = "Purity Gold £1.99", Type = "imBack", Title = "Purity Gold £1.99" });
            cardButtons.Add(new CardAction() { Value = "Doom Bar Bitter £1.79", Type = "imBack", Title = "Doom Bar Bitter £1.79" });
            cardButtons.Add(new CardAction() { Value = "Sierra Nevada Pale Ale £1.79", Type = "imBack", Title = "Sierra Nevada Pale Ale £1.79" });
            HeroCard plCard = new HeroCard()
            {
                Title = "Some suggestions",
                Subtitle = "Optionally choose one of these suggestions",
                Images = null,
                Buttons = cardButtons
            };
            messageButtons.Attachments.Add(plCard.ToAttachment());
            await context.PostAsync(messageButtons);

            //wait for user
            context.Wait(MessageReceived);
        }

        [LuisIntent("DoomBar")]
        public async Task DoomBar(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();
            var messageBody = new StringBuilder();
            messageBody.AppendLine("OK, I'll add **Doom Bar Bitter £1.79**.");
            messageBody.AppendLine(" ");
            messageBody.AppendLine("Is there anything else I can do for you? If you are not sure what I can do, ask for help! :)");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("FinishOrder")]
        public async Task FinishOrder(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();
            var messageBody = new StringBuilder();
            messageBody.AppendLine("OK, thanks, we'll email you an order receipt but [here is a web link for your reference](https://www.ocado.com/webshop/displayAllOrders.do?)");
            messageBody.AppendLine(" ");
            messageBody.AppendLine("You can change the order anytime up to 23:59 the day before (Monday 12th September). Just ask me to change the order if you want to add anything");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);

            //fake a delay
            Delay(context, 1, true);

            var message2 = context.MakeMessage();
            message2.Type = "message";
            message2.TextFormat = "markdown";
            message2.Attachments = new List<Attachment>();
            var message2Body = new StringBuilder();
            message2Body.AppendLine("Our driver Karl will be with you at 10:30 on Tuesday 13th September, I hope you enjoy those new beers.");
            message2.Text = message2Body.ToString();
            await context.PostAsync(message2);

            context.Wait(MessageReceived);
        }

        [LuisIntent("Goodbye")]
        public async Task Goodbye(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();
            var messageBody = new StringBuilder();
            messageBody.AppendLine("Goodbye, have a lovely day. :)");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Logout")]
        public async Task Logout(IDialogContext context, LuisResult result)
        {
            context.PrivateConversationData.Clear();
            await context.PostAsync("...");
            context.Wait(MessageReceived);
        }

        public async Task AnthingElse(IDialogContext context)
        {
            //fake a delay
            Delay(context, 3, true);

            var message = context.MakeMessage();
            message.Type = "message";
            message.TextFormat = "markdown";
            message.Attachments = new List<Attachment>();
            var messageBody = new StringBuilder();
            messageBody.AppendLine("Is there anything else I can do for you? If you are not sure what I can do, ask for help! :)");
            message.Text = messageBody.ToString();
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        public void Delay(IDialogContext context, int seconds, bool sendTypingNotice)
        {
            Thread.Sleep(seconds * 1000);
        }
    }
}