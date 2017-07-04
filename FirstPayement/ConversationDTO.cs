using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstPayement
{
    public class ConversationDTO
    {
        public string ResumptionCookie { get; set; }
        public string ConvoId { get; set; }

        public ConversationDTO():
            base()
        {
           
        }
        public ConversationDTO(string cookie , string Id) :
            base()
        {
            ResumptionCookie = cookie;
            ConvoId = Id;
        }


    }
}