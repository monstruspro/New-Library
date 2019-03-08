using AutoMapper;
using Data.Entity;
using Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        [System.Web.Mvc.Route("Registration")]
        public ActionResult Registration()
        {
            return View();
        }

        private LibraryEntities db = new LibraryEntities();

        [System.Web.Mvc.Route("Registration")]
        [System.Web.Mvc.HttpPost]
        public ActionResult Registration(Users UserModel)
        {
            users_privileges userprev = new users_privileges();
            user user = new user();
            privilege privilege = new privilege();

            if (Session["username"] == null)
            {
                if (!ModelState.IsValid)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                string extension = Path.GetExtension(UserModel.ImageFile.FileName);
                string fileName = Guid.NewGuid().ToString() + extension;
                var url = System.Configuration.ConfigurationManager.AppSettings["ImageSaveRoute"];
                UserModel.Photo = fileName;
                ////check here
                fileName = Path.Combine(Server.MapPath(url), fileName);

                UserModel.ImageFile.SaveAs(fileName);

                var username = db.users.SingleOrDefault(c => c.username == UserModel.Username);
                var email = db.users.SingleOrDefault(c => c.email == UserModel.Email);
                if (email != null && username != null)
                {
                    ViewBag.UsernameError = "This username exists";
                    ViewBag.EmailError = "This email is exist";
                    return View("Registration");
                }
                else if (username != null)
                {
                    ViewBag.UsernameError = "This username exists";
                    return View("Registration");
                }
                else if (email != null)
                {
                    ViewBag.EmailError = "This email exists";
                    return View("RegisterView");
                }



                //var config = new MapperConfiguration(cfg =>
                //{
                //    cfg.CreateMap<Users, user>()
                //    .ForMember(entity => entity.name, model => model.MapFrom(mod => mod.Name))
                //    .ForMember(entity => entity.surname, model => model.MapFrom(mod => mod.Surname))
                //    .ForMember(entity => entity.password, model => model.MapFrom(mod => mod.Password))
                //    .ForMember(entity => entity.telephone, model => model.MapFrom(mod => mod.Telephone))
                //    .ForMember(entity => entity.email, model => model.MapFrom(mod => mod.Email))
                //    .ForMember(entity => entity.address, model => model.MapFrom(mod => mod.Address))
                //    .ForMember(entity => entity.username, model => model.MapFrom(mod => mod.Username))
                //    .ForMember(entity => entity.photo, model => model.MapFrom(mod => mod.Photo));
                //});
                //var IMapper = config.CreateMapper();
                //var destination = IMapper.Map<Users, user>(UserModel);

                user.name = UserModel.Name;
                user.surname = UserModel.Surname;
                user.password = UserModel.Password;
                user.address = UserModel.Address;
                user.email = UserModel.Email;
                user.telephone = UserModel.Telephone;
                user.username = UserModel.Username;
                user.photo = UserModel.Photo;
                                               
                var priviligie = db.privileges.SingleOrDefault(c => c.privilege1 == "User");
                if (priviligie == null)
                {
                    privilege addprev = new privilege();
                    addprev.privilege1 = "User";
                    db.privileges.Add(addprev);
                    db.SaveChanges();
                    priviligie = db.privileges.SingleOrDefault(c => c.privilege1 == "User");
                }

                db.users.Add(user);
                db.SaveChanges();
                
                //filling many to many table
                userprev.user_id = user.id;
                userprev.privileges_id = priviligie.id;
                db.users_privileges.Add(userprev);
                db.SaveChanges();
                // i connect created user with default privilige

                //Create Session
                Session["Current_User"] = new SessionConstructor()
                {
                    Id = user.id,
                    Privilege = priviligie.privilege1
                };
            }
            else
            {
                ViewBag.UsernameError = "You must log out first";
            }

            return RedirectToAction("BookAuthor", "AuthorBookController");
        }
    }
}