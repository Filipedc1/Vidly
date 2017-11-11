using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Vidly.Models;
using System.ComponentModel.DataAnnotations;

namespace Vidly.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)] 
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        public byte MembershipTypeId { get; set; }

        public MembershipTypeDto MembershipType { get; set; }

        //[Min18YearsIfAMember] 
        public DateTime? Birthdate { get; set; }
    }
}