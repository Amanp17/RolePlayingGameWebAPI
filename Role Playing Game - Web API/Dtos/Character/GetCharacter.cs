using RolePlayingGameWebAPI.Dtos.Skill;
using RolePlayingGameWebAPI.Dtos.Weapon;
using RolePlayingGameWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Dtos
{
    public class GetCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoint { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.knight;
        public GetWeapon Weapon { get; set; }
        public List<GetSkill> Skills { get; set; }
        public int Fight { get; set; }
        public int Victories { get; set; }
        public int Defeat { get; set; }
    }
}
