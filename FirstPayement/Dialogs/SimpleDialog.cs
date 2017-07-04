using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace FirstPayement
{
    [Serializable]
    public class SimpleDialog : IDialog
    {
       // private ResumptionCookie resumptionCookie;
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceived);
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text.Contains("Hi"))
            {
                await context.PostAsync("Hi wanna order something.Tell me what would you like to buy");
                context.Done<object>(null);
            }

            if (activity.Text.Contains("Buy") || activity.Text.Contains("buy"))
            {
                await context.PostAsync("we are gonna charge you 100 rupees.Please wait");
                ResumptionCookie cookie = new ResumptionCookie(activity);


                var t = cookie.GetMessage();
                t.Text = "fillertex";
                var encodedcookie = cookie.GZipSerialize();
                var uriBuilder = new UriBuilder(Const.BaseURl+"//Checkout//Checkout");
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["token"] = cookie.ConversationId;
                try
                {

                    ConversationDTO c = new ConversationDTO(encodedcookie, cookie.ConversationId);
                    ConversationalDAC dac = new ConversationalDAC();
                    dac.Set(c);
                    

                //   var path = Path.Combine(Environment.CurrentDirectory, "..\\cookies\\" + cookie.ConversationId + ".txt");
                    //using (System.IO.StreamWriter file =
                    //new System.IO.StreamWriter(Const.SaveAddress+@"cookies/" + cookie.ConversationId + ".txt"))
                    //{
                    //    file.WriteLine(encodedcookie);
                    //    Logger.Log("file created named " + cookie.ConversationId);
                    //}
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                    query["amount"] = "100";
                uriBuilder.Query = query.ToString();
                var checkoutUrl = uriBuilder.Uri.ToString();

              
                var redirecter = context.MakeMessage();
                redirecter.Attachments = new List<Attachment>
                {
                    new HeroCard()
                    {
                        Text ="1 Item costing 100 ",
                        Buttons = new List<CardAction>
                        {
                            new CardAction(ActionTypes.OpenUrl, "Proceed to Checkout", value:checkoutUrl)
                        }
                    }.ToAttachment()
                };
                await context.PostAsync(redirecter);

                context.Wait(AfterPayement);

            }
            else
            {
                await context.PostAsync("Hi wanna order something,Send Buy.");
                context.Done<object>(null);

            }
          

        }

        private async Task AfterPayement(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            
            if (activity.Text.Contains("Completed")&& activity.Text.Contains(activity.Conversation.Id))
            {
                var t = activity.Text.Split('#');
                var id = t[1];
                var reciept = context.MakeMessage();
                reciept.Attachments = activity.Attachments;
                await context.PostAsync(reciept);
                

                context.Done<object>(null);
            }
           else if(activity.Text.Contains("cancel"))
            {
                await context.PostAsync("Payement Canceled");

                context.Done<object>(null);
            }
            else
            {
                await context.PostAsync("Payement failed.Please Complete Payment to proceed or type cancel to reset");

                context.Wait(AfterPayement);

            }

        }
    }
}