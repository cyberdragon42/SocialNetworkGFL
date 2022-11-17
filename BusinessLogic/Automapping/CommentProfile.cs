using AutoMapper;
using BusinessLogic.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Automapping
{
    public class CommentProfile: Profile
    {
        public CommentProfile()
        {
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<Comment, UsersCommentDto>();
        }
    }
}
