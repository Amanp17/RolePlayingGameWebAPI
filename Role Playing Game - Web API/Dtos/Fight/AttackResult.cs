using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Dtos.Fight
{
    public class AttackResult
    {
        public string Attacker { get; set; }
        public string Opponent { get; set; }
        public int AttackerHP { get; set; }
        public int OpponentHP { get; set; }
        public int Damage { get; set; }
    }
}
