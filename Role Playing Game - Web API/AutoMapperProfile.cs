using AutoMapper;
using RolePlayingGameWebAPI.Dtos;
using RolePlayingGameWebAPI.Dtos.Fight;
using RolePlayingGameWebAPI.Dtos.Skill;
using RolePlayingGameWebAPI.Dtos.Weapon;
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
            CreateMap<GetCharacter, Character>().ReverseMap();
            CreateMap<Character, AddCharacter>().ReverseMap();
            CreateMap<Weapon, GetWeapon>().ReverseMap();
            CreateMap<Skill, GetSkill>().ReverseMap();
            CreateMap<Character, HighScore>().ReverseMap();
        }
    }
}
