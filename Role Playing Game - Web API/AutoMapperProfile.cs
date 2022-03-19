using AutoMapper;
using RolePlayingGameWebAPI.Dtos;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacter>().ReverseMap();
            CreateMap<Character, AddCharacter>().ReverseMap();
        }
    }
}
