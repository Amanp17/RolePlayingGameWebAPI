using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RolePlayingGameWebAPI.Data;
using RolePlayingGameWebAPI.Dtos;
using RolePlayingGameWebAPI.Dtos.Weapon;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Services.Weapon
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public WeaponService(DataContext context,IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        private int GetUserId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<GetCharacter>> AddWeapon(AddWeapon newWeapon)
        {
            var response = new ServiceResponse<GetCharacter>();
            try
            {
                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User.ID == GetUserId);
                if(character == null)
                {
                    response.Success = false;
                    response.Message = "Character Not Found";
                    return response;
                }

                var weapon = new Models.Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };

                _context.Weapons.Add(weapon);
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
