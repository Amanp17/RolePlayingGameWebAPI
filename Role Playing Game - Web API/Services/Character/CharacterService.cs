using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RolePlayingGameWebAPI.Data;
using RolePlayingGameWebAPI.Dtos;
using RolePlayingGameWebAPI.Dtos.Character;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper,DataContext context,IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        //Gets the Current LoggedIn User ID via Claims
        private int GetUserId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        //Get ALL Characters
        public async Task<ServiceResponse<List<GetCharacter>>> GetAllCharacter()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacter>>();
            var dbCharacters = await _context.Characters
                .Include(c=>c.Skills)
                .Include(c=>c.Weapon)
                .Where(c=>c.User.ID == GetUserId)
                .ToListAsync();
            serviceResponse.Data = dbCharacters
                .Select(c => _mapper.Map<GetCharacter>(c))
                .ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacter>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacter>();
            var dbCharacter = await _context.Characters
                .Include(c=>c.Weapon)
                .Include(c=>c.Skills)
                .FirstOrDefaultAsync(x => x.Id == id && x.User.ID==GetUserId);
            var character = _mapper.Map<GetCharacter>(dbCharacter);
            serviceResponse.Data = character;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacter>>> AddCharacter(AddCharacter newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacter>>();                             
            Character character = _mapper.Map<Character>(newCharacter);
            //LoggedIn User will be assigned with the Character Added
            character.User = await _context.Users.FirstOrDefaultAsync(c => c.ID == GetUserId);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Characters
                .Where(c=>c.User.ID==GetUserId)
                .Select(c=> _mapper.Map<GetCharacter>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacter>> UpdateCharacter(UpdateCharacter updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacter>();
            try
            {
                Character character = await _context.Characters
                    .Include(c=>c.User)
                    .FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);

                if (character != null)
                {
                    character.Name = updateCharacter.Name;
                    character.HitPoint = updateCharacter.HitPoint;
                    character.Intelligence = updateCharacter.Intelligence;
                    character.Strength = updateCharacter.Strength;
                    character.Defense = updateCharacter.Defense;
                    character.Class = updateCharacter.Class;

                    await _context.SaveChangesAsync();
                    serviceResponse.Data = _mapper.Map<GetCharacter>(character);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Data Not Found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacter>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacter>>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.ID == GetUserId);
                if (character != null)
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = _context.Characters
                        .Where(c=>c.User.ID==GetUserId)
                        .Select(c => _mapper.Map<GetCharacter>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character Not Found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacter>> AddCharacterSkill(AddCharacterSkill newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacter>();
            try
            {
                var character = await _context.Characters
                    .Include(c=>c.Weapon)
                    .Include(c=>c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.ID == GetUserId);

                if (character == null)
                {
                    response.Success = false;
                    response.Message = "Character Not Found";
                }

                var skill = await _context.Skill.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillID);
                if (skill == null)
                {
                    response.Success = false;
                    response.Message = "Skill Not Found";
                    return response;
                }
                character.Skills.Add(skill);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacter>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
