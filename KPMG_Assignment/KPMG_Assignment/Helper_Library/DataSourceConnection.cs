using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPMG_Assignment.Helper_Library
{
    public class DataSourceConnection
    {
        private const string XLS_FILE_EXTENSION = ".xls";
        private const string XLSX_FILE_EXTENSION = ".xlsx";
        public string SourceType { get; set; }
        public string Location { get; set; }

        public string GetConnectionString()
        {
            string connectionString = string.Empty;

            switch(SourceType)
            {
                case XLS_FILE_EXTENSION:
                    connectionString = string.Format(@"{0}{1}{2}",Constants.XLS_PROVIDER,Location,Constants.XLS_EXTENDED_PROPERTIES);
                        break;
                case XLSX_FILE_EXTENSION:
                        connectionString = string.Format(@"{0}{1}{2}", Constants.XLSX_PROVIDER, Location, Constants.XLSX_EXTENDED_PROPERTIES);
                        break;
                default:
                        connectionString = string.Format(@"{0}{1}{2}", Constants.XLS_PROVIDER, Location, Constants.XLS_EXTENDED_PROPERTIES);
                        break;
            }
            return connectionString;
        }
    }
}