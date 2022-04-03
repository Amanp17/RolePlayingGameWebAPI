using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Dtos.Fight
{
    public class SkillAttack
    {
        public int AttackerId { get; set; }
        public int OpponentId { get; set; }
        public int SkillId { get; set; }

    }
}
