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
        private const string FileUploadControlId = "FileUpload";
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

                return Redirect("GetUploadedData");
 
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

            return View("ShowAccounts", objViewModelAccts);
        }

        private int GetFileData(HttpPostedFileBase file)
        {
            
            if (file.ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
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
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnString);
                    List<KPMGInvalidAccount> lstInvalidAcct = new List<KPMGInvalidAccount>();

                    List<KPMGAccount> lstValidAcct = GetValidInvalidAccounts(excelSheets, excelConnection1, lstInvalidAcct);

                    InsertData(lstValidAcct, "KPMG_Accounts");
                    InsertData(lstInvalidAcct, "KPMG_Invalid_Accounts");

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

        private static void InsertData<T>(List<T> list, string TabelName)
        {
            DataTable dt = new DataTable("Accounts");
            dt = ConvertToDataTable(list);
       
            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["kpmgConnection"].ConnectionString))
            {
                bulkcopy.BulkCopyTimeout = 660;
                bulkcopy.ColumnMappings.Add("Account", "Account");
                bulkcopy.ColumnMappings.Add("Description", "Description");
                bulkcopy.ColumnMappings.Add("CurrencyCode", "CurrencyCode");
                bulkcopy.ColumnMappings.Add("Amount", "Amount");
                if (TabelName == "KPMG_Invalid_Accounts")
                { bulkcopy.ColumnMappings.Add("Reason", "Reason"); }
                bulkcopy.DestinationTableName = TabelName;
                bulkcopy.WriteToServer(dt);
            }
        }

        

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