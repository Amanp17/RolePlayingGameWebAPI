using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RolePlayingGameWebAPI.Dtos;
using RolePlayingGameWebAPI.Dtos.Weapon;
using RolePlayingGameWebAPI.Models;
using RolePlayingGameWebAPI.Services.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacter>>> AddWeapon(AddWeapon newWeapon)
        {
            var result = await _weaponService.AddWeapon(newWeapon);
            return Ok(result);
        }

    }
}
