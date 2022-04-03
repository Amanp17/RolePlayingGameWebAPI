using Microsoft.AspNetCore.Mvc;
using RolePlayingGameWebAPI.Dtos.Fight;
using RolePlayingGameWebAPI.Models;
using RolePlayingGameWebAPI.Services.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResult>>> WeaponAttack(WeaponAttack request)
        {
            var result = await _fightService.WeaponAttack(request);
            return Ok(result);
        }
        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResult>>> SkillAttack(SkillAttack request)
        {
            var result = await _fightService.SkillAttack(request);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<FightResult>>> Fight(FightRequest request)
        {
            var result = await _fightService.Fight(request);
            return Ok(result);
        }
        [HttpGet("topList")]
        public async Task<ActionResult<ServiceResponse<HighScore>>> GetHighScore()
        {
            var result = await _fightService.GetHighScore();
            return Ok(result);
        }
    }
}
