using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace FirstPayement
{
    public class chatHandler
    {


        public async static Task<int> PayementCompleted(Dictionary<string,string> dict)
        {
            var token = dict["custom"];
            string escapetoken = "";
            try
            {

                ConversationalDAC dac = new ConversationalDAC();
                ConversationDTO dto = dac.Get(token);
              //  using (System.IO.StreamReader file =
              //new System.IO.StreamReader(Const.SaveAddress+@"cookies/" + token + ".txt"))
              //  {
              //      escapetoken = file.ReadLine();
              //      Logger.Log("File read by payement complete");
              //  }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //  var escapetoken = Uri.UnescapeDataString(token);
            var cookie = ResumptionCookie.GZipDeserialize(escapetoken);
            var message = cookie.GetMessage();
            var payementID = Guid.NewGuid();
            message.Text = "Completed#" + dict["txn_id"]+"#"+dict["custom"];
            message.Attachments = new List<Attachment>();
            var reciept = new ReceiptCard()
            {
                Title = Uri.UnescapeDataString(dict["business"]) + " Shop",
                Facts = new List<Fact> {
                    new Fact("Order Id", dict["txn_id"]),
                    new Fact("payment type", dict["payment_type"])
                },
                Items = new List<ReceiptItem>
                {
                    new ReceiptItem(
                        title: Uri.UnescapeDataString(dict["item_name"]),
                        subtitle: "item",
                        price:dict["payment_gross"]
                       ),
                },
                Total = dict["payment_gross"]
            };
            message.Attachments.Add(reciept.ToAttachment());

            try
            {
                await Conversation.ResumeAsync(cookie, message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return 0;
        }

        public async static Task<int> PayementCancelled(string token)
        {
            //var token = dict["custom"];
            string escapetoken = "";
            try
            {

                ConversationalDAC dac = new ConversationalDAC();
                ConversationDTO dto = dac.Get(token);
                    escapetoken = dto.ResumptionCookie;
                //  using (System.IO.StreamReader file =
                //new System.IO.StreamReader(Const.SaveAddress+@"cookies/" + token + ".txt"))
                //  {
                //      escapetoken = file.ReadLine();
                //      Logger.Log("file read in payement cancelled");
                //  }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //  var escapetoken = Uri.UnescapeDataString(token);
            var cookie = ResumptionCookie.GZipDeserialize(escapetoken);
            var message = cookie.GetMessage();
            message.Text = "cancel"+"#"+token;
            try
            {
                await Conversation.ResumeAsync(cookie, message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return 0;
        }
    }
}