using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KPMG_Assignment.Validation
{
    public class ValidateFileXlsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            const int maxContent = 1024 * 1024 * 10;
            var allowedExt = new string[] { Constants.XLS_FILE_EXTENTION, Constants.XLSX_FILE_EXTENTION };

            var file = value as HttpPostedFileBase;

            if (file == null)
                return false;

            // check type of file 
            if (!allowedExt.ToList().Contains(file.FileName.Substring(file.FileName.LastIndexOf("."))))
            {
                ErrorMessage = Constants.USE_MESSAGE_UPLOAD_FILE_TYPE + string.Join(", ", allowedExt);
                return false;
            }

            // check the length of the file
            if (file.ContentLength > maxContent)
            {
                ErrorMessage = Constants.USER_MESSAGE_FILE_BIG + (maxContent / 1024) + "MB";
                return false;
            }
            return true;
        }
    }
}