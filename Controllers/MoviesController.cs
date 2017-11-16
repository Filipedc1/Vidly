using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Movies
        public ActionResult Index()
        {
            //'User' property of controller gives us access to current user
            if (User.IsInRole(RoleName.CanManageMovies))
            {
                return View("List");
            }

            return View("ReadOnlyList");
        }

        public ActionResult Details(int? id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(mov => mov.Id == id);

            if (movie == null)
            {
                return HttpNotFound("'movie' variable in the Details Action method returned null");
            }

            return View(movie);
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieFormViewModel(movie)
            {
                Genres = _context.Genres.ToList(),
            };

            return View("MovieForm", viewModel);
        }

        //add new movie 
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            var viewModel = new MovieFormViewModel()
            {
                Genres = _context.Genres.ToList()
            };

            return View("MovieForm", viewModel);
        }

        //Save form information
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };

                return View("MovieForm", viewModel);
            }


            //if true, then it's a new customer
            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;

                _context.Movies.Add(movie);
            }
            else
            {
                var movieInDatabase = _context.Movies.SingleOrDefault(m => m.Id == movie.Id);

                movieInDatabase.Name = movie.Name;
                movieInDatabase.ReleaseDate = movie.ReleaseDate;
                movieInDatabase.GenreId = movie.GenreId;
                movieInDatabase.NumberInStock = movie.NumberInStock;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }
    }
}