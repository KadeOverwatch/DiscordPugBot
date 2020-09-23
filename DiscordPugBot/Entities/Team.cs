using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordPugBot.Entities
{
    public class Team
    {
        public Dictionary<Player, string> Members = new Dictionary<Player, string>();

        public int AverageSR()
        {
            if (Members.Count == 0) throw new NotImplementedException();
            return Convert.ToInt32(Members.Average(x => x.Key.Player_Rank));
        }

        public Player AddPlayer(Player player, string role)
        {
            Members.Add(player, role);
            return player;
        }

        public int PlayerCount()
        {
            return Members.Count;
        }

        public bool ValidateStructure()
        {
            if (PlayerCount() != 6) return false;
            if (Members.Count(x => x.Value == ":TankLogo:") != 2) return false;
            if (Members.Count(x => x.Value == ":DpsLogo:") != 2) return false;
            if (Members.Count(x => x.Value == ":SupportLogo:") != 2) return false;
            return true;
        }
    }
}
