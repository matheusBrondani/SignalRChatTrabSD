using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatsLikeFinal.Models
{
    public class Messages    
    {
        public int IdMensagem { get; set; }
        public int IdSender { get; set; }
        public string  UserName { get; set; }
        public int IdConversa { get; set; }
        public int Entregue { get; set; }
        public int Lida { get; set; }
        public string Mensagem { get; set; }
    }
}
