﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KPMG_Assignment.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class kpmgEntities : DbContext
    {
        public kpmgEntities()
            : base("name=kpmgEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<KPMG_Accounts> KPMG_Accounts { get; set; }
        public virtual DbSet<KPMG_Invalid_Accounts> KPMG_Invalid_Accounts { get; set; }
    }
}