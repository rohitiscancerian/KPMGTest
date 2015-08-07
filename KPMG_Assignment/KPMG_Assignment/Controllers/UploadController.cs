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
using KPMG_Assignment.Interfaces;
using KPMG_Assignment.Helper_Library;

namespace KPMG_Assignment.Controllers
{
    public class UploadController : Controller,IFileProcessing
    {
        private HttpPostedFileBase _file;
        public new HttpPostedFileBase File
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

                return Redirect( Constants.ACTION_NAME_GET_UPLOADED_DATA);
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

            return View(Constants.ACTION_NAME_SHOW_ACCOUNTS, objViewModelAccts);
        }

        private void GetFileData(HttpPostedFileBase file)
        {
            IFileProcessing fileProcessing;
            File = file;
            fileProcessing = new FileProcessing(this);
            fileProcessing.ExtractData();
        }

        public void ExtractData()
        {
 	        throw new NotImplementedException();
        }
    }


        
}
