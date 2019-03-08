using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Models;
using Data.Entity;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        [Route("books")]
        public ActionResult Books()
        {
            var modelquery = db.books.Select(x => new Books
            {
                Title = x.title,
                Publisher = x.publisher,
                PublicationDate = x.pub_date,
                Id = x.id

            }).ToList();
            return View("Books", modelquery);
        }

        public ActionResult AddBooks()
        {
            return View("AddBook");
        }

        [HttpPost]
        public ActionResult AddBooks(book book)
        {
            var any = db.books.SingleOrDefault(c => c.title == book.title && c.pub_date == book.pub_date && c.publisher == book.publisher);
            if (any != null)
            {
                ViewBag.ExistanceError = "This book already exists";
                return View("AddBook");
            }

            db.books.Add(book);
            db.SaveChanges();
            var modelquery = db.books.Select(x => new Books
            {
                Title = x.title,
                Publisher = x.publisher,
                PublicationDate = x.pub_date,
                Id = x.id

            }).ToList();
            return View("~/Views/Book/Books.cshtml", modelquery);
        }

        public ActionResult EditBook(int id)
        {
            var query = db.books.SingleOrDefault(x => x.id == id);

            Books book = new Books()
            {
                Title = query.title,
                PublicationDate = query.pub_date,
                Publisher = query.publisher,
                Id = query.id
            };

            return View("EditBook", book);

        }

        [HttpPost]
        public ActionResult EditBook(book book)
        {
            var query = db.books.SingleOrDefault(x => x.id == book.id);
            query.title = book.title;
            query.pub_date = book.pub_date;
            query.publisher = book.publisher;
            db.SaveChanges();

            var modelquery = db.books.Select(x => new Books
            {
                Title = x.title,
                Publisher = x.publisher,
                PublicationDate = x.pub_date,
                Id = x.id

            }).ToList();
            return View("~/Views/Book/Books.cshtml", modelquery);
        }

    }


}