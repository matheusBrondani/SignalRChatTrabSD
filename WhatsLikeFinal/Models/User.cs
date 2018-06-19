using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatsLikeFinal.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public string UserNick { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<User> ListContatos { get; set; }
        public int OnLine { get; set; }
    }
}