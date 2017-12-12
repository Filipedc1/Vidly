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
        #region Fields and Attributes

        private ApplicationDbContext _context;

        #endregion

        #region Constructor

        public NewRentalsController()
        {
            _context = new ApplicationDbContext();
        }

        #endregion

        #region Public Members

        //HttpPost used because we will be creating new objects and this complies with RESTful conventions
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            if (newRental == null || newRental.MovieIds.Count == 0)
            {
                return BadRequest("Either newRental object is null OR no Movie Ids have been given.");
            }
            else if (newRental.MovieIds.Count > 3)
            {
                return BadRequest("A maximum of 3 movies can be rented.");
            }

            var customer = _context.Customers.Single(c => c.Id == newRental.CustomerId);

            var movies = _context.Movies.Where(m => newRental.MovieIds.Contains(m.Id)).ToList();

            if (movies.Count != newRental.MovieIds.Count)
            {
                if (ContainsRepeatedMovies(newRental, customer) == true)
                {
                    return BadRequest("You cannot rent the same movie twice.");
                }

                return BadRequest("One or more movie ids are invalid.");
            }

            if (ContainsRepeatedMovies(newRental, customer) == true)
            {
                return BadRequest("You have already rented one of these movies.");
            }

            //Checks to make sure a customer does not have more than 3 movies rented
            var (moviesCurrentlyRentedCount, totalMovies) = NumberOfMoviesRented(customer, newRental);

            if (totalMovies > 3)
            {
                return BadRequest($"A maximum of 3 movies can be rented.\nYou currently have {moviesCurrentlyRentedCount} rented");
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

        #endregion

        #region Private Members
    
        private (int moviesCurrentlyRentedCount, int totalMovies) NumberOfMoviesRented(Customer customer, NewRentalDto newRental)
        {
            int moviesCurrentlyRentedCount = _context.Rentals.Where(m => m.Customer.Id == customer.Id).ToList().Count;

            return (moviesCurrentlyRentedCount, moviesCurrentlyRentedCount + newRental.MovieIds.Count);
        }

        private bool ContainsRepeatedMovies(NewRentalDto newRental, Customer customer)
        {
            HashSet<int> uniqueMovieIDS = new HashSet<int>();

            //checking if any movie was selected twice
            //only perform this check if there is more than 1 movie in newRental.MovieIds 
            if (newRental.MovieIds.Count > 1)
            {
                foreach (int id in newRental.MovieIds)
                {
                    if (uniqueMovieIDS.Contains(id))
                    {
                        return true;
                    }

                    uniqueMovieIDS.Add(id);
                }
            }

            var moviesCurrentlyRented = _context.Rentals
                                        .Where(m => m.Customer.Id == customer.Id)
                                        .Select(m => new NewRentalDto
                                        {
                                               CustomerId = m.Customer.Id,
                                               MovieIds = _context.Movies.Where(movi => movi.Id == m.Movie.Id).Select(movi => movi.Id).ToList()
                                        })
                                        .ToList();

            if ( moviesCurrentlyRented.Any(rental => rental.MovieIds.Any() == newRental.MovieIds.Any()) )
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
