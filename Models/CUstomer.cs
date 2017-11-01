using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)] //this is overriding default conventions. This is known as Data annotations
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        public MembershipType MembershipType { get; set; }

        [Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; } //entity framework recognizes this convention and treats this prop as a foreign key

        [Display(Name = "Date of Birth")] //allows the 'New' form on the UI to display a speicific name instead of the name of the actual variable
        public DateTime? Birthdate { get; set; }
    }
}