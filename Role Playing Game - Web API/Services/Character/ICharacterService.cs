using RolePlayingGameWebAPI.Dtos;
using RolePlayingGameWebAPI.Dtos.Character;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Services
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacter>>> GetAllCharacter();
        Task<ServiceResponse<GetCharacter>> GetCharacterById(int id);
        Task<ServiceResponse<List<GetCharacter>>> AddCharacter(AddCharacter newCharacter);
        Task<ServiceResponse<GetCharacter>> UpdateCharacter(UpdateCharacter UpdateCharacter);
        Task<ServiceResponse<List<GetCharacter>>> DeleteCharacter(int id);
        Task<ServiceResponse<GetCharacter>> AddCharacterSkill(AddCharacterSkill newCharacterSkill);
    }
}
