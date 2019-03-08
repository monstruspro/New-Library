using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Data.Entity;
using Model.Models;

namespace Library.Controllers
{
    public class LoginController : Controller
    {

        private LibraryEntities db = new LibraryEntities();
        // GET: Login
        [System.Web.Mvc.Route("Login")]
        public ActionResult Login()
        {
            //BookAuthorConnector a = new BookAuthorConnector();
            return View();
        }
        [System.Web.Mvc.Route("Login")]
        [System.Web.Mvc.HttpPost]
        public ActionResult Login(Login user)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var userdata = db.users_privileges.SingleOrDefault(c => c.user.username == user.Username && c.user.password == user.Password);
            if (userdata == null)
            {
                ViewBag.LoginError = "Wrong username/password";
                return View("Login");
            }

            //Create Session            
            Session["Current_User"] = new SessionConstructor()
            {
                Privilege = userdata.privilege.privilege1,
                Id = userdata.user_id
            };

            return RedirectToAction("BookAuthor", "AuthorBook");

        }
    }
}