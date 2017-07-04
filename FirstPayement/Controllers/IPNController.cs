using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FirstPayement.Controllers
{
    public class IPNController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public HttpStatusCodeResult Receive()
        {
        
            //Fire and forget verification task
            Task.Run(() => VerifyTask(Request));

            //Reply back a 200 code
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private Dictionary<string,string> makedict(string reply)
        {
            string[] v = reply.Split('&');
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var line in v)
            {



                string[] entry = line.Split('=');
                if (entry.Length == 2)
                {
                    dict.Add(entry[0], entry[1]);

                }
            }

            return dict;
        }
        private void VerifyTask(HttpRequestBase ipnRequest)
        {
            var verificationResponse = string.Empty;
            string reply="";
            try
            {
                var verificationRequest = (HttpWebRequest)WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");
                //Set values for the verification request
                verificationRequest.Method = "POST";
                verificationRequest.ContentType = "application/x-www-form-urlencoded";
                var param = Request.BinaryRead(ipnRequest.ContentLength);
                var strRequest = Encoding.ASCII.GetString(param);
                reply = strRequest;
                //Add cmd=_notify-validate to the payload
                strRequest = "cmd=_notify-validate&" + strRequest;
                verificationRequest.ContentLength = strRequest.Length;
                //Attach payload to the verification request
                var streamOut = new StreamWriter(verificationRequest.GetRequestStream(), Encoding.ASCII);
                streamOut.Write(strRequest);
                streamOut.Close();
                
                //Send the request to PayPal and get the response
                var streamIn = new StreamReader(verificationRequest.GetResponse().GetResponseStream());
                verificationResponse = streamIn.ReadToEnd();
                streamIn.Close();
                
                    
            }
            catch (Exception e)
            {
                    Console.WriteLine(e.Message);

            }

            ProcessVerificationResponse(verificationResponse,reply);

        }


        private void LogRequest(HttpRequestBase request)
        {
            }

        private async Task<int> ProcessVerificationResponse(string verificationResponse, string reply)

        {
            if (verificationResponse.Equals("VERIFIED"))
            {
                await chatHandler.PayementCompleted(makedict(reply));    
            }
            else if (verificationResponse.Equals("INVALID"))
            {
                //Log for manual investigation
                await chatHandler.PayementCancelled(makedict(reply)["custom"]);

            }
            else
            {
                //Log error
            }
            return 0;
        }
    }
}