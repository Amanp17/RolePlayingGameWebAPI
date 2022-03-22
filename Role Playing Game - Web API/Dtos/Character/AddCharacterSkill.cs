using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Dtos.Character
{
    public class AddCharacterSkill
    {
        public int CharacterId { get; set; }
        public int SkillID { get; set; }
    }
}
