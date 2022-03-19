﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RolePlayingGameWebAPI.Data;
using RolePlayingGameWebAPI.Dtos;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper,DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponse<List<GetCharacter>>> GetAllCharacter()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacter>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacter>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacter>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacter>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(x => x.Id == id);
            var character = _mapper.Map<GetCharacter>(dbCharacter);
            serviceResponse.Data = character;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacter>>> AddCharacter(AddCharacter newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacter>>();                             
            Character character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Characters.Select(c=> _mapper.Map<GetCharacter>(c)).ToList();
            return serviceResponse;
        }
        public async Task<ServiceResponse<GetCharacter>> UpdateCharacter(UpdateCharacter updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacter>();
            Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
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
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacter>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacter>>();
            Character character = await _context.Characters.FirstAsync(c => c.Id == id);
            if (character != null)
            {
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _context.Characters.Select(c => _mapper.Map<GetCharacter>(c)).ToList();
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Unable to Delete";
            }
            return serviceResponse;
        }
    }
}
