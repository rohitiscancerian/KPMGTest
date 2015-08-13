using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.OleDb;
using System.Web.Security;
using KPMG_Assignment.BusinessObjects;
using KPMG_Assignment.Interfaces;

namespace KPMG_Assignment.Helper_Library
{
    public class FileProcessing : IFileProcessing, IAccountBO
    {
        private HttpPostedFileBase _file;
        private IAccountBO objAccountBO;
        public HttpPostedFileBase File
        {
            get
            {
                return _file;
            }
            set
            {
                _file = value;
            }
        }

        private DataSet _fileData;
        public DataSet FileData
        {
            get
            {
                return _fileData;
            }
            set
            {
                _fileData = value;
            }
        }
        private IFileProcessing localFileProcessing;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="senderFileProcessing"></param>
        public FileProcessing(IFileProcessing senderFileProcessing)
        {
            localFileProcessing = senderFileProcessing;
        }

        /// <summary>
        /// This method extracts the data from the uploaded file into dataset and passes on to Account business object
        /// </summary>
        public void ExtractData()
        {
            DataSourceConnection objSourceConn = new DataSourceConnection();
            OleDbConnection excelConnection1;
            string fileExtension;
            string excelConnString;
            DataSet ds;
            DataTable dt = new DataTable();

            if (localFileProcessing.File.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(localFileProcessing.File.FileName);

                if (fileExtension == Constants.XLS_FILE_EXTENTION || fileExtension == Constants.XLSX_FILE_EXTENTION)
                {

                    string fileLocation = HttpContext.Current.Server.MapPath("~/Content/") + localFileProcessing.File.FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    localFileProcessing.File.SaveAs(fileLocation);

                    objSourceConn.Location = fileLocation;
                    objSourceConn.SourceType = fileExtension;

                    excelConnString = objSourceConn.GetConnectionString();


                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnString);
                    excelConnection.Open();
                   

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        throw new Exception("No data in excel");
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row[Constants.TABLE_NAME].ToString();
                        t++;
                    }

                    excelConnection1 = new OleDbConnection(excelConnString);
                    ds = new DataSet();
                    for (int count = 0; count < excelSheets.Count(); count++)
                    {   
                        string query = string.Format("Select * from [{0}]", excelSheets[count]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }
                    FileData = ds;
                    objAccountBO = new AccountBO(this);
                    this.ValidateData();
                    this.WriteData();
                }
                else
                {
                    throw new Exception("File not supported"); // File not supported
                }
            }
            else
            {
                throw new Exception("No data in file");
            }
        }

        public void ValidateData()
        {
            objAccountBO.ValidateData();
        }


        public void WriteData()
        {
            objAccountBO.WriteData();
        }
    }
}