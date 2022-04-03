using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RolePlayingGameWebAPI.Data;
using RolePlayingGameWebAPI.Dtos.Fight;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Services.Fight
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<FightResult>> Fight(FightRequest request)
        {
            var response = new ServiceResponse<FightResult>
            {
                Data = new FightResult()
            };
            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => request.CharacterIds.Contains(c.Id)).ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }

                        response.Data.Log
                            .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage > 0 ? damage : 0)} damage.");

                        if (opponent.HitPoint <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeat++;
                            response.Data.Log.Add($"{opponent.Name} has Defeated!!");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoint} HP left");
                            break;
                        }
                    }
                }
                characters.ForEach(c =>
                {
                    c.Fight++;
                    c.HitPoint = 100;
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<ServiceResponse<AttackResult>> SkillAttack(SkillAttack request)
        {
            var response = new ServiceResponse<AttackResult>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if (skill == null)
                {
                    response.Success = false;
                    response.Message = $"{attacker.Name} doesn't know this skill!!";
                    return response;
                }

                int damage = DoSkillAttack(attacker, opponent, skill);
                if (opponent.HitPoint <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated!!";
                }

                await _context.SaveChangesAsync();
                response.Data = new AttackResult()
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.HitPoint,
                    AttackerHP = attacker.HitPoint,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
            {
                opponent.HitPoint -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResult>> WeaponAttack(WeaponAttack request)
        {
            var response = new ServiceResponse<AttackResult>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                int damage = DoWeaponAttack(attacker, opponent);
                if (opponent.HitPoint <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated!!";
                }

                await _context.SaveChangesAsync();
                response.Data = new AttackResult()
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.HitPoint,
                    AttackerHP = attacker.HitPoint,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
            {
                opponent.HitPoint -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<List<HighScore>>> GetHighScore()
        {

            var characters = await _context.Characters
                .Where(c => c.Fight > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeat)
                .ToListAsync();

            var response = new ServiceResponse<List<HighScore>>
            {
                Data = characters.Select(c => _mapper.Map<HighScore>(c)).ToList()
            };

            return response;
        }
    }
}

