using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPMG_Assignment.Interfaces
{
    /// <summary>
    /// This interface will provide a handle to classes wanting to use FileProcessing  Helper library class
    /// </summary>
    public interface IFileProcessing
    {
        HttpPostedFileBase File { get; set; }
        
        void ExtractData() ;
    }
}