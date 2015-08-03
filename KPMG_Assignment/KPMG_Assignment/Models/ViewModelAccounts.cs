using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPMG_Assignment.Models
{
    public class ViewModelAccounts
    {
        public IQueryable<KPMG_Accounts> validAccounts { get; set; }
        public IQueryable<KPMG_Invalid_Accounts> invalidAccounts { get; set; }
    }
}