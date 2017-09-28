using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {



        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string user, string pass)
        {
            ViewBag.Message = string.Empty;
            if (user == "Admin" && pass == "pass")
                return View("Admin");
            else
            {
                using (var db = new LIBRARYEntities())
                {
                    foreach (var account in db.Accounts)
                        if (user == account.AccountUsername && pass == account.AccountPassword)
                            return View("Reader", account.AccountID);
                    ViewBag.Message = "Wrong username or password";
                    return View();

                }
            }
        }
        public ActionResult Admin()
        {
            return View("Admin");
        }
        public ActionResult Reader()
        {
            return View("Reader");
        }

        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(string name,long phone,string email,string address)
        {
            ViewBag.Message = string.Empty;
            using (var db = new LIBRARYEntities())
            {

                foreach(var item in db.Readers)
                    if(name==item.ReaderName)
                    {
                        ViewBag.Message = "Name already in use";
                        return View();
                    }

                var reader = new Reader
                {
                    ReaderName = name,
                    ReaderPhone = phone,
                    ReaderEmail = email,
                    ReaderAddress = address
                };

                db.Readers.Add(reader);

                var account = new Account
                {
                    AccountUsername = name,
                    AccountPassword = name,
                    ReaderID = reader.ReaderID
                };

                db.Accounts.Add(account);

                db.SaveChanges();

                return View("CreateAccountResult", account);
            }
        }

        public ActionResult ForgotPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPass(string name,long phone,string email,string address,string newPass)
        {
            ViewBag.Message = string.Empty;

            try
            {
                using (var db = new LIBRARYEntities())
                {
                    var reader = db.Readers.Where(x => x.ReaderName == name).Single();

                    if(phone!=reader.ReaderPhone || email!=reader.ReaderEmail || address!=reader.ReaderAddress)
                    {
                        ViewBag.Message = "Not enough information";
                        return View();
                    }

                    var account = db.Accounts.Where(x => x.ReaderID == reader.ReaderID).Single();

                    account.AccountPassword = newPass;

                    db.SaveChanges();

                    ViewBag.Message = "Password changed";
                    return View();
                    
                }
            }
            catch(Exception)
            {
                ViewBag.Message = "No account with the given username";
                return View();
            }
        }

        
    }
}