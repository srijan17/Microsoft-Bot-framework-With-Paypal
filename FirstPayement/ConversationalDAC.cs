using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstPayement
{
    public class ConversationalDAC
    {

        public void Set(ConversationDTO dto)
        {
            using (payementEntities pay = new payementEntities())
            {
                ResumptionTable converse = new ResumptionTable();
                converse.ConvoId = dto.ConvoId;
                converse.ResumptionCookie = dto.ResumptionCookie;
                pay.ResumptionTables.Add(converse);
                pay.SaveChanges();
            }
        }


        public ConversationDTO Get(string conversationId)
        {
            ConversationDTO newconvo = null;
            using (payementEntities pay = new payementEntities())
            {
                ResumptionTable converse = (from g in pay.ResumptionTables where g.ConvoId == conversationId select g).FirstOrDefault();
               
                if(converse!=null)
                {
                    newconvo = new ConversationDTO();
                    newconvo.ConvoId = converse.ConvoId;
                    newconvo.ResumptionCookie = converse.ResumptionCookie;
                }
                if(string.IsNullOrEmpty(newconvo.ResumptionCookie))
                {
                    throw new Exception("No ResumptionCookie Found");
                }
            }
            return newconvo;
        }
    }
}