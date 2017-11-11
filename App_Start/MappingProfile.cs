using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using Vidly.Models;
using Vidly.DTOs;

namespace Vidly.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to Dto

            //when createmap method is called, automapper uses reflection to scan these types, find their properties, 
            //and maps them based on their name
            Mapper.CreateMap<Customer, CustomerDto>();
            Mapper.CreateMap<Movie, MovieDto>();
            Mapper.CreateMap<MembershipType, MembershipTypeDto>();

            Mapper.CreateMap<CustomerDto, Customer>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            Mapper.CreateMap<MovieDto, Movie>()
                .ForMember(m => m.Id, opt => opt.Ignore());
        }
    }
}