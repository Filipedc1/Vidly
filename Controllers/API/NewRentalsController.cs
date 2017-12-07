using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Vidly.DTOs;
using Vidly.Models;

namespace Vidly.Controllers.API
{
    public class NewRentalsController : ApiController
    {
        private ApplicationDbContext _context;

        public NewRentalsController()
        {
            _context = new ApplicationDbContext();
        }

        //HttpPost used because we will be creating new objects and this complies with RESTful conventions
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            if (newRental == null || newRental.MovieIds.Count == 0)
            {
                return BadRequest("Either newRental object is null OR no Movie Ids have been given.");
            }

            var customer = _context.Customers.Single(c => c.Id == newRental.CustomerId);

            var movies = _context.Movies.Where(m => newRental.MovieIds.Contains(m.Id)).ToList();

            if (movies.Count != newRental.MovieIds.Count)
            {
                return BadRequest("One or more movie ids are invalid.");
            }

            //Checks to make sure a customer does not have more than 3 movies rented
            if (newRental.MovieIds.Count > 3)
            {
                return BadRequest("A maximum of 3 movies can be rented.");
            }
            else
            {
                int moviesCurrentlyRentedCount = _context.Rentals.Where(m => m.Customer.Id == customer.Id).ToList().Count;
                      
                if (moviesCurrentlyRentedCount + newRental.MovieIds.Count > 3)
                {
                    return BadRequest($"A maximum of 3 movies can be rented.\nYou currently have {moviesCurrentlyRentedCount} rented");
                }
            }
            

            foreach (var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                {
                    return BadRequest($"Movie is not available.\nMovie Id: {movie.Id}\nMovie Name: {movie.Name}");
                }

                movie.NumberAvailable--;

                var rental = new Rental()
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };

                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
