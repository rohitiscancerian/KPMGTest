using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using KPMG_Assignment.Interfaces;
using System.ComponentModel;
using System.Configuration;
using KPMG_Assignment.Models;

namespace KPMG_Assignment.Helper_Library
{
    public class SQLCopy : ISQLCopy
    {
        private string _connectionString = string.Empty;
        private Dictionary<int,string> _sourceColumns;
        private Dictionary<int, string> _tableColumns;
        private string _tableName = string.Empty;
        private List<KPMGAccount> _accounts;
        
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
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlCopy"></param>
        public SQLCopy(ISQLCopy sqlCopy)
        {
            ConnectionString = sqlCopy.ConnectionString;
            SourceColumns = sqlCopy.SourceColumns;
            TableColumns = sqlCopy.TableColumns;
            TableName = sqlCopy.TableName;
            Accounts = sqlCopy.Accounts;
        }

        /// <summary>
        /// This method inserts a list of objects into a sql table 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="TabelName"></param>
        public void InsertData()
        {
            DataTable dt = new DataTable("Accounts");
            if (Accounts != null && Accounts.Count > 0)
            {   dt = ConvertToDataTable(Accounts);}

            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString))
            {
                bulkcopy.BulkCopyTimeout = 660;
                for (int i = 1; i <= _sourceColumns.Count(); i++)
                {
                    bulkcopy.ColumnMappings.Add(_sourceColumns[i], _tableColumns[i]);
                }
                   
                bulkcopy.DestinationTableName = TableName;
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