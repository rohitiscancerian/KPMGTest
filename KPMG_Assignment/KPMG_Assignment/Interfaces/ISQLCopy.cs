using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KPMG_Assignment.Models;

namespace KPMG_Assignment.Interfaces
{
    /// <summary>
    /// This interface will provide a handle to classes wanting to use SQLCopy  Helper library class
    /// </summary>
    public interface ISQLCopy
    {
             string ConnectionString { get; set; }
             string TableName { get; set; }
             Dictionary<int,string> SourceColumns { get; set; }
             Dictionary<int, string> TableColumns { get; set; }
             List<KPMGAccount> Accounts {get;set;}

             void InsertData();
    }
}