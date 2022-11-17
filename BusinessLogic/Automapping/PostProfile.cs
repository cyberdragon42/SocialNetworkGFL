using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Dto;
using Domain.Models;

namespace BusinessLogic.Automapping
{
    public class PostProfile: Profile
    {
        public PostProfile()
        {
            CreateMap<Post, ExtendedPostDto>();
            CreateMap<CreatePostDto, Post>();
        }
    }
}
