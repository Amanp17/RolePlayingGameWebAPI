using RolePlayingGameWebAPI.Dtos;
using RolePlayingGameWebAPI.Dtos.Weapon;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Services.Weapon
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacter>> AddWeapon(AddWeapon newWeapon);
    }
}
