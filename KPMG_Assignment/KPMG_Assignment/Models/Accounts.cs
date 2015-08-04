using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPMG_Assignment.Models
{
    public class KPMGAccount
    {
        public string Account { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }

    public class KPMGInvalidAccount
    {
        public string Account { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? Amount { get; set; }
        public string Reason { get; set; }
    }
}