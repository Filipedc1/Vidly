using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Vidly.Models;

//create a view model that encapsulates all the data required for a specific view

namespace Vidly.ViewModels
{
    public class CustomerFormViewModel
    {
        //using IEnumberable instead of List<> because we dont need any of the functionality (methods) that List<> provides
        //we just need to be able to iterate through our list
        //if in the future we want to use another collection, we dont need to modify this viewmodel as long as the collection
        //implements IEnumerable so our code is more loosely coupled
        public IEnumerable<MembershipType> MembershipTypes { get; set; }
        public Customer Customer { get; set; }
    }
}