﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SignalRBDEntities : DbContext
    {
        public SignalRBDEntities()
            : base("name=SignalRBDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CONTATOS> CONTATOS { get; set; }
        public virtual DbSet<CONVERSAS> CONVERSAS { get; set; }
        public virtual DbSet<MENSAGENS> MENSAGENS { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
    }
}