using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Dtos.Fight
{
    public class FightRequest
    {
        public List<int> CharacterIds { get; set; }
    }
}
