using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KPMG_Assignment.Validation;

namespace KPMG_Assignment.Models
{
    public class UploadFile
    {
        [Required(ErrorMessage = "Please browse your xls or xlsx file"), Display(Name = "Upload xls"), NotMapped, ValidateFileXls]
        public HttpPostedFileBase File { get; set; }
    }
}