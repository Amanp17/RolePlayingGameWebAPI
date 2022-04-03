using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Dtos.Fight
{
    public class HighScore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Fight { get; set; }
        public int Victories { get; set; }
        public int Defeat { get; set; }

    }
}
