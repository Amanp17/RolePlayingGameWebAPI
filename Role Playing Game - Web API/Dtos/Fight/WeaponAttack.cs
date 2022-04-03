using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Dtos.Fight
{
    public class WeaponAttack
    {
        public int AttackerId { get; set; }
        public int OpponentId { get; set; }
    }
}
