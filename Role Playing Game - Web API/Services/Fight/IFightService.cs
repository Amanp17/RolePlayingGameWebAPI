using RolePlayingGameWebAPI.Dtos.Fight;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Services.Fight
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResult>> WeaponAttack(WeaponAttack request);
        Task<ServiceResponse<AttackResult>> SkillAttack(SkillAttack request);
        Task<ServiceResponse<FightResult>> Fight(FightRequest request);
        Task<ServiceResponse<List<HighScore>>> GetHighScore();
    }
}
