using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Models;
        
namespace Library.Controllers
{
    public class AuthorBookController : Controller
    {
        private LibraryEntities db = new LibraryEntities();

        [Route("AuthorBook")]
        public ActionResult BookAuthor()
        {
            var query = db.authors_books.Select(x => new BookAuthorViewModel
            {
                Publisher = x.book.publisher,
                PublicationDate = x.book.pub_date,
                Name = x.author.name + " " + x.author.surname,
                Title = x.book.title
            }).ToList();
            return View("BookAuthor", query);
        }

        public ActionResult BookAuthorConnect()
        {
            BookAuthorConnector model = new BookAuthorConnector();

            model.Books = db.books.Select(x => new SelectListItem()
            {
                Text = x.title,
                Value = x.id.ToString()
            });

            model.Authors = db.authors.Select(x => new SelectListItem()
            {
                Text = x.name,
                Value = x.id.ToString()
            });

            return View("BookAuthorConnect", model);
        }
        [HttpGet]
        public BookAuthorConnector Getbooks(int id)
        {
            BookAuthorConnector model = new BookAuthorConnector();

            model.Books = db.books.Select(x => new SelectListItem()
            {
                Text = x.title,
                Value = x.id.ToString()
            });

            model.Authors = db.authors_books.Where(x => x.book_id == id && x.author.id != x.author_id).Select(x => new SelectListItem()
            {
                Text = x.author.name,
                Value = x.author.id.ToString()
            });

            return model;
        }
    }
}