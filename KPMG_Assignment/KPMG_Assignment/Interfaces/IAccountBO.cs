using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KPMG_Assignment.Interfaces
{
    /// <summary>
    /// This interface will provide a handle to classes wanting to use AccountBO business object class
    /// </summary>
    public interface IAccountBO
    {
        DataSet FileData { get; set; }
        void ValidateData();
        void WriteData();
    }
}