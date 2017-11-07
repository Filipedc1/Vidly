﻿using System;
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
            var movies = _context.Movies.Include(m => m.Genre).ToList();

            if (movies == null)
            {
                return HttpNotFound();
            }

            return View(movies);
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

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieFormViewModel()
            {
                Movie = movie,
                Genres = _context.Genres.ToList(),
            };

            return View("MovieForm", viewModel);
        }

        //add new movie 
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
        public ActionResult Save(Movie movie)
        {
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