using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RolePlayingGameWebAPI.Dtos;
using RolePlayingGameWebAPI.Dtos.Character;
using RolePlayingGameWebAPI.Models;
using RolePlayingGameWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }


        //Get api/Character/GetAll
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> GetAll()
        {
            return Ok(await _characterService.GetAllCharacter());
        }
        //Get api/CHaracter/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacter>>> GetById(int id)
        {
            var result = await _characterService.GetCharacterById(id);
            if(result.Data == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        //Post api/Character
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> AddCharacter(AddCharacter newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }
        //Put api/Character
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacter>>> UpdateCharacters(UpdateCharacter updateCharacter)
        {
            var result = await _characterService.UpdateCharacter(updateCharacter);
            if(result.Data == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        //Delete api/Character/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacter>>>> DeleteCharacter([FromRoute] int id)
        {
            var result = await _characterService.DeleteCharacter(id);
            if (result.Data == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        //Post api/CharacterSkill
        [HttpPost("Skills")]
        public async Task<ActionResult<ServiceResponse<GetCharacter>>> AddCharacterSkill(AddCharacterSkill newCharacterSkill)
        {
            var result = await _characterService.AddCharacterSkill(newCharacterSkill);
            return Ok(result);
        }
    }
}
