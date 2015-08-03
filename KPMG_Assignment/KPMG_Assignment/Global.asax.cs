using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KPMG_Assignment
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public class Currency
        {
            public string code {get; set;}
        }
        public static List<Currency> lstCurrencyCodes;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LoadCurrencyCodes();
        }

        

        public bool ValidateCurrency(string code)
        {
            if (lstCurrencyCodes.Exists(x => x.code == code))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        protected void LoadCurrencyCodes()
        {
            lstCurrencyCodes = new List<Currency>();
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "AFN" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "AMD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "AOA" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "AUD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "AZN" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "BBD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "BGN" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "BIF" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "BND" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "BRL" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "BTN" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "BYR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "CAD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "CHF" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "CNY" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "CRC" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "CUP" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "CZK" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "DKK" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "DZD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "ERN" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "EUR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "FKP" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "GEL" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "GHS" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "GMD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "GTQ" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "HKD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "HRK" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "HUF" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "ILS" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "INR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "IRR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "JEP" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "JOD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "KES" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "KHR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "KPW" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "KWD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "KZT" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "LBP" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "LRD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "LYD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "MDL" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "MKD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "MNT" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "MRO" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "MVR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "MXN" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "MZN" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "NGN" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "NOK" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "NZD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "PAB" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "PGK" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "PKR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "PYG" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "RON" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "RUB" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "SAR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "SCR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "SEK" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "SHP" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "SOS" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "SRD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "SVC" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "SZL" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "TJS" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "TND" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "TRY" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "TVD" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "TZS" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "UGX" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "UYU" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "VEF" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "VUV" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "XAF" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "XDR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "XPF" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "ZAR" });
            lstCurrencyCodes.Add(new Currency() { code = "AED" });
            lstCurrencyCodes.Add(new Currency() { code = "ZWD" });

        }
    }
}
