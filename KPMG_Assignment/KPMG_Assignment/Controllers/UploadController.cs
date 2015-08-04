using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Data;
using System.Data.Sql;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.ComponentModel;
using KPMG_Assignment.Models;



namespace KPMG_Assignment.Controllers
{
    public class UploadController : Controller
    {
        private const string TABLE_NAME = "TABLE_NAME";
        private const string TABLE_NAME_ACCOUNTS = "KPMG_Accounts";
        private const string TABLE_NAME_INVALID_ACCTS = "KPMG_Invalid_Accounts";
        private const string XLS_FILE_EXTENTION = ".xls";
        private const string XLSX_FILE_EXTENTION = ".xlsx";

        private const string COLUMN_NAME_ACCOUNT = "Account";
        private const string COLUMN_NAME_DESCRIPTION = "Description";
        private const string COLUMN_NAME_CURRENCY_CODE = "CurrencyCode";
        private const string COLUMN_NAME_AMOUNT = "Amount";
        private const string COLUMN_NAME_REASON = "Reason";

        private const string ACTION_NAME_SHOW_ACCOUNTS = "ShowAccounts";
        private const string ACTION_NAME_GET_UPLOADED_DATA = "GetUploadedData";




        // GET: UploadFile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(UploadFile objFile)
        {
            if (ModelState.IsValid)
            {
                kpmgEntities dbContext = new kpmgEntities();
                ViewModelAccounts objViewModelAccts = new ViewModelAccounts();

                HttpPostedFileBase file = objFile.File;
                GetFileData(file);

                return Redirect(ACTION_NAME_GET_UPLOADED_DATA);
            }
            else
            { 
                return View("Index",objFile); 
            }
            
        }

     
        [HttpGet]
        public ActionResult GetUploadedData()
        {
            kpmgEntities dbContext = new kpmgEntities();
            ViewModelAccounts objViewModelAccts = new ViewModelAccounts();
            objViewModelAccts.validAccounts = dbContext.KPMG_Accounts.ToList().AsQueryable();
            objViewModelAccts.invalidAccounts = dbContext.KPMG_Invalid_Accounts.ToList().AsQueryable();

            return View(ACTION_NAME_SHOW_ACCOUNTS, objViewModelAccts);
        }

        private int GetFileData(HttpPostedFileBase file)
        {
            
            if (file.ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(file.FileName);

                if (fileExtension == XLS_FILE_EXTENTION || fileExtension == XLSX_FILE_EXTENTION)
                {

                    string fileLocation = Server.MapPath("~/Content/") + file.FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    file.SaveAs(fileLocation);

                    string excelConnString = GetExcelConnectionString(fileLocation, fileExtension);

                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return 1;   // 
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row[TABLE_NAME].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnString);
                    List<KPMGInvalidAccount> lstInvalidAcct = new List<KPMGInvalidAccount>();

                    List<KPMGAccount> lstValidAcct = GetValidInvalidAccounts(excelSheets, excelConnection1, lstInvalidAcct);

                    InsertData(lstValidAcct, TABLE_NAME_ACCOUNTS);
                    InsertData(lstInvalidAcct, TABLE_NAME_INVALID_ACCTS);

                    return 4;   // Data successfully loaded
                }
                else
                {
                    return 3; // File not supported
                }
            }
            else
            {   
                return 2; // Empty file 
            }

        }

       
        private string GetExcelConnectionString(string fileLocation, string fileExtension)
        {
            string excelConnectionString = string.Empty;
            switch (fileExtension)
            {
                case ".xls": 
                                excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                                break;
                case ".xlsx" :
                                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                break;
                default :
                                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                break;

            }
            return excelConnectionString;
        }
        /// <summary>
        /// This method iterates through all the sheets in the excel file 
        /// and separates valid and invalid accounts
        /// </summary>
        /// <param name="excelSheets"></param>
        /// <param name="excelConnection"></param>
        /// <param name="lstInvalidAccts"></param>
        /// <returns></returns>
        private List<KPMGAccount> GetValidInvalidAccounts(String[] excelSheets, OleDbConnection excelConnection, List<KPMGInvalidAccount> lstInvalidAccts)
        {
            DataSet ds;
            KPMGAccount acct;
            List<KPMGAccount> lstValidAcct = new List<KPMGAccount>();

            for (int count = 0; count < excelSheets.Count(); count++)
            {
                ds = new DataSet();
                string query = string.Format("Select * from [{0}]", excelSheets[count]);
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                {
                    dataAdapter.Fill(ds);
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    acct = new KPMGAccount();
                    decimal amt;

                    acct.Account = ds.Tables[0].Rows[i][0].ToString();

                    acct.Description = ds.Tables[0].Rows[i][1].ToString();
                    acct.CurrencyCode = ds.Tables[0].Rows[i][2].ToString();
                    if (decimal.TryParse(ds.Tables[0].Rows[i][3].ToString(), out amt))
                    {
                        acct.Amount = amt;
                    }

                    if (ValidateAccountInfo(acct, lstInvalidAccts))
                    { lstValidAcct.Add(acct); }
                }
                ds = null;
            }
            return lstValidAcct;
        }

        private bool ValidateAccount(string param)
        {
            if (String.IsNullOrEmpty(param) )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// This method validates the account information
        /// </summary>
        /// <param name="acct"></param>
        /// <param name="invalidAccts"></param>
        /// <returns></returns>
        private bool ValidateAccountInfo(KPMGAccount acct, List<KPMGInvalidAccount> invalidAccts)
        {
            bool IsValid = true;
            string reason = string.Empty;
            KPMGInvalidAccount invalidAcct = new KPMGInvalidAccount();

            if (!ValidateAccount(acct.Account))
            {
                reason = "No Account info";
                IsValid = false;
                invalidAcct.Account = null;
            }
            else { invalidAcct.Account = acct.Account; }

            if (!ValidateAccount(acct.Description))
            {
                reason = reason + " | No Account description";
                IsValid = false;
                invalidAcct.Description = null;
            }
            else { invalidAcct.Description = acct.Description; }

            if (!ValidateAmount(acct.Amount))
            {
                reason = reason + " | No Amount ";
                IsValid = false;
                invalidAcct.Amount = null;
            }
            else { invalidAcct.Amount = acct.Amount; }

            if (!MvcApplication.lstCurrencyCodes.Exists(x => x.code == acct.CurrencyCode))
            {
                reason = reason +  "| Invalid currency code ";
                IsValid = false;
                invalidAcct.CurrencyCode = null;
            }
            else { invalidAcct.CurrencyCode = acct.CurrencyCode; }

            if (!IsValid)
            {  
                invalidAcct.Reason = reason;
                invalidAccts.Add(invalidAcct);
            }

            return IsValid;
        }

             
        private bool ValidateAmount(decimal amount)
        {
            if (amount > 0)
            { return true; }
            else { return false; }
        }

        /// <summary>
        /// This method inserts a list of objects into a sql table 
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="TabelName"></param>
        private static void InsertData<T>(List<T> list, string TabelName)
        {
            DataTable dt = new DataTable("Accounts");
            dt = ConvertToDataTable(list);
       
            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["kpmgConnection"].ConnectionString))
            {
                bulkcopy.BulkCopyTimeout = 660;
                bulkcopy.ColumnMappings.Add(COLUMN_NAME_ACCOUNT, COLUMN_NAME_ACCOUNT);
                bulkcopy.ColumnMappings.Add(COLUMN_NAME_DESCRIPTION, COLUMN_NAME_DESCRIPTION);
                bulkcopy.ColumnMappings.Add(COLUMN_NAME_CURRENCY_CODE, COLUMN_NAME_CURRENCY_CODE);
                bulkcopy.ColumnMappings.Add(COLUMN_NAME_AMOUNT, COLUMN_NAME_AMOUNT);
                if (TabelName == TABLE_NAME_INVALID_ACCTS)
                { bulkcopy.ColumnMappings.Add(COLUMN_NAME_REASON, COLUMN_NAME_REASON); }
                bulkcopy.DestinationTableName = TabelName;
                bulkcopy.WriteToServer(dt);
            }
        }

        
        /// <summary>
        /// This method converts a list of objects into a datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}