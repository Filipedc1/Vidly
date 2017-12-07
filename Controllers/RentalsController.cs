using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class RentalsController : Controller
    {
        private ApplicationDbContext _context;

        public RentalsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Rentals
        public ActionResult New()
        {
            return View();
        }

        public ActionResult Return(Rental rental)
        {
            var rentalInDb = _context.Rentals.SingleOrDefault(r => r.Id == rental.Id);

            if (rentalInDb == null)
            {
                return HttpNotFound("This rented movie was not located in the database.");
            }

            //increments number avilable for movie returned
            var movieRented = (from r in _context.Rentals
                               where r.Id == rental.Id
                               let movID = r.Movie.Id
                               let movi = _context.Movies.FirstOrDefault(m => m.Id == movID)
                               select movi).ToList();

            movieRented[0].NumberAvailable++;

            _context.Rentals.Remove(rentalInDb);
            _context.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}