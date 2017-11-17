using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Vidly.DTOs;

namespace Vidly.Controllers.API
{
    public class NewRentalsController : ApiController
    {
        //HttpPost used because we will be creating new objects and this complies with RESTful conventions
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            throw new NotImplementedException();
        }
    }
}
