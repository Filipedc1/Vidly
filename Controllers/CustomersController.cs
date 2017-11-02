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
    public class CustomersController : Controller
    {
        //application db context which is needed to access the database
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customers
        public ActionResult Index()
        {
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();

            return View(customers);
        }

        //used for our Form
        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new NewCustomerViewModel()
            {
                MembershipTypes = membershipTypes
            };

            return View(viewModel);
        }


        //HttpPost attribute is used to make sure this action can only be called using HttpPost and not HttpGet
        //If your actions modify data, they should never be accessible by HttpGet.

        //Model Binding: Because the model behind our view 'New.cshtml' is of type 'NewCustomerViewModel',
        //we can use this type here as a parameter and MVC framework will automatically map request data to this object
        //So MVC framework binds this model to the request data
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            //add this customer to our database
            //adding this customer to the _context only adds it into memory
            _context.Customers.Add(customer);

            //to save the changes
            _context.SaveChanges();

            //redirect user back to list of customers 
            return RedirectToAction("Index", "Customers");
        }

        public ActionResult Details(int? id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }
    }
}