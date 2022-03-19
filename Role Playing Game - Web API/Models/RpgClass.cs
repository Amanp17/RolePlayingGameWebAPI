using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RolePlayingGameWebAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    //JsonConverter Specifies that Converter is Used , StringEnumConverter Converts End Number to string
    public enum RpgClass
    {
        //Represent the Current Class of the Character
        knight,
        Mage,
        Cleric
    }
}
