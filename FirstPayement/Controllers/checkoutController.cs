using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FirstPayement.Controllers
{
    [RoutePrefix("CheckOut")]
    public class checkoutController : Controller
    {
        // GET: checkout
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            var token = Request.Params["token"];
            ViewBag.token = token; //send converasationid
            var amount = Request.Params["amount"];
            ViewBag.Amount = amount;
            return View();
            //byte[] data = System.Text.Encoding.ASCII.GetBytes($"business=testpineappleshop@shop.com&cmd=_xclick&custom={token}&amount={amount}&shopping_url=http:////www.yourwebsite.com//shoppingpage.html&return=http://www.yourwebsite.com/success.html&cancel_return={Const.BaseURl}/Checkout/PaymentCancelled?token={token}");
            //WebRequest request = WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = data.Length;
            //using (Stream stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            //Uri respuri;

            //using (WebResponse response = request.GetResponse())
            //{
            //     respuri = response.ResponseUri;
            //  }
            
            //return  Redirect(respuri.AbsoluteUri);
        }
        //public async Task<ActionResult> PaymentCompleted()
        //{
        //    var token =Uri.EscapeDataString(Request.Params["token"]);
        //    // await chatHandler.PayementCompleted(token);
        //    await chatHandler.PayementCancelled(token);
        //    return View();
        //}

        public async Task<ActionResult> PaymentCancelled()
        {
            var token = Uri.UnescapeDataString(Request.Params["token"]);
            await chatHandler.PayementCancelled(token);

            return View();
        }






    }
}