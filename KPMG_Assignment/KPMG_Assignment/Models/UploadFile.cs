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
        private const string USER_MESSAGE_BROWSE_XL = "Please browse your xls or xlsx file";
        private const string UPLOAD_DISPLAY_NAME = "Upload xls";

        [Required(ErrorMessage = USER_MESSAGE_BROWSE_XL), Display(Name = UPLOAD_DISPLAY_NAME), NotMapped, ValidateFileXls]
        public HttpPostedFileBase File { get; set; }
    }
}