//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class KPMG_Invalid_Accounts
    {
        public int id { get; set; }
        public string Account { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Reason { get; set; }
    }
}
