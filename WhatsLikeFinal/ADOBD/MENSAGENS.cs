//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WhatsLikeFinal.ADOBD
{
    using System;
    using System.Collections.Generic;
    
    public partial class MENSAGENS
    {
        public int id_mensagens { get; set; }
        public Nullable<int> id_sender { get; set; }
        public Nullable<int> id_conversa { get; set; }
        public Nullable<decimal> entregue { get; set; }
        public Nullable<decimal> lida { get; set; }
        public string mensagem { get; set; }
    
        public virtual CONVERSAS CONVERSAS { get; set; }
        public virtual USUARIO USUARIO { get; set; }
    }
}