using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KPMG_Assignment.Interfaces;
using KPMG_Assignment.Models;
using KPMG_Assignment.Helper_Library;

namespace KPMG_Assignment.BusinessObjects
{
    public class AccountBO : ISQLCopy,IAccountBO
    {
        private string _connectionString = string.Empty;
        private Dictionary<int, string> _sourceColumns;
        private Dictionary<int, string> _tableColumns;
        private string _tableName = string.Empty;
        private List<KPMGAccount> _accounts;
        private DataSet _fileData;
        private ISQLCopy objSqlCopy;

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }

        public Dictionary<int, string> SourceColumns
        {
            get
            {
                return _sourceColumns;
            }
            set
            {
                _sourceColumns = value;
            }
        }

        public Dictionary<int, string> TableColumns
        {
            get
            {
                return _tableColumns;
            }
            set
            {
                _tableColumns = value;
            }
        }

        public List<KPMGAccount> Accounts
        {
            get
            {
                return _accounts;
            }
            set
            {
                _accounts = value;
            }
        }

        public void InsertData()
        {
            objSqlCopy.InsertData();
        }
       
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

        private IAccountBO _localObjAccountBO;
        private List<KPMGAccount> lstInvalidAcct = new List<KPMGAccount>();
        private List<KPMGAccount> lstValidAcct = new List<KPMGAccount>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objAccountBO"></param>
        public  AccountBO(IAccountBO objAccountBO)
        {
            _localObjAccountBO = objAccountBO;
        }

        /// <summary>
        /// This method validates the excel data and separates valid and invalid accounts
        /// </summary>
        public void ValidateData()
        { 
            KPMGAccount acct;
            for (int i = 0; i < _localObjAccountBO.FileData.Tables[0].Rows.Count; i++)
            {
                acct = new KPMGAccount();
                decimal amt;

                acct.Account = _localObjAccountBO.FileData.Tables[0].Rows[i][0].ToString();

                acct.Description = _localObjAccountBO.FileData.Tables[0].Rows[i][1].ToString();
                acct.CurrencyCode = _localObjAccountBO.FileData.Tables[0].Rows[i][2].ToString();
                if (decimal.TryParse(_localObjAccountBO.FileData.Tables[0].Rows[i][3].ToString(), out amt))
                {
                    acct.Amount = amt;
                }

                if (ValidateAccountInfo(acct, lstInvalidAcct))
                { lstValidAcct.Add(acct); }
            }
        }

        /// <summary>
        /// This method will call the insert function of SQLCopy class which will inturn save the data to table
        /// </summary>
        public void WriteData()
        {
            TableName = Constants.TABLE_NAME_ACCOUNTS;
            PopulateAccountBOObject();
            Accounts = lstValidAcct;
            objSqlCopy = new SQLCopy(this);
            this.InsertData();

            TableName = Constants.TABLE_NAME_INVALID_ACCTS;
            PopulateAccountBOObject();
            Accounts = lstInvalidAcct;
            objSqlCopy = new SQLCopy(this);
            this.InsertData();
        }

        /// <summary>
        /// This method populates the proties of the the AccountBO object which will act as parameter to SQLCopy constructor
        /// </summary>
        private void PopulateAccountBOObject()
        {
            ConnectionString = Constants.CONNECTION_STRING;

            SourceColumns = new Dictionary<int, string>();

            SourceColumns.Add(1, Constants.COLUMN_NAME_ACCOUNT);
            SourceColumns.Add(2, Constants.COLUMN_NAME_DESCRIPTION);
            SourceColumns.Add(3, Constants.COLUMN_NAME_CURRENCY_CODE);
            SourceColumns.Add(4, Constants.COLUMN_NAME_AMOUNT);

            TableColumns = new Dictionary<int, string>();

            TableColumns.Add(1, Constants.COLUMN_NAME_ACCOUNT);
            TableColumns.Add(2, Constants.COLUMN_NAME_DESCRIPTION);
            TableColumns.Add(3, Constants.COLUMN_NAME_CURRENCY_CODE);
            TableColumns.Add(4, Constants.COLUMN_NAME_AMOUNT);

            if (TableName == Constants.TABLE_NAME_INVALID_ACCTS)
            {
                SourceColumns.Add(5, Constants.COLUMN_NAME_REASON);
                TableColumns.Add(5, Constants.COLUMN_NAME_REASON);
            }
        }

        private bool ValidateAccount(string param)
        {
            if (String.IsNullOrEmpty(param))
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
        private bool ValidateAccountInfo(KPMGAccount acct, List<KPMGAccount> invalidAccts)
        {
            bool IsValid = true;
            string reason = string.Empty;
            KPMGAccount invalidAcct = new KPMGAccount();

            if (!ValidateAccount(acct.Account))
            {
                reason = Constants.VAL_MSG_NO_ACCOUNT;
                IsValid = false;
                invalidAcct.Account = null;
            }
            else { invalidAcct.Account = acct.Account; }

            if (!ValidateAccount(acct.Description))
            {
                reason = reason + Constants.VAL_MSG_NO_DESC;
                IsValid = false;
                invalidAcct.Description = null;
            }
            else { invalidAcct.Description = acct.Description; }

            if (!ValidateAmount(acct.Amount))
            {
                reason = reason + Constants.VAL_MSG_NO_AMOUNT;
                IsValid = false;
                invalidAcct.Amount = null;
            }
            else { invalidAcct.Amount = acct.Amount; }

            if (!MvcApplication.lstCurrencyCodes.Exists(x => x.code == acct.CurrencyCode))
            {
                reason = reason + Constants.VAL_MSG_NO_AMOUNT;
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


        private bool ValidateAmount(decimal? amount)
        {
            if (amount > 0)
            { return true; }
            else { return false; }
        }
    }
}