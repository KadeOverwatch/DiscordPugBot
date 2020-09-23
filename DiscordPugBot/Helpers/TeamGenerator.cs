using DiscordPugBot.Entities;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordPugBot.Helpers
{
    public static class TeamGenerator
    {
        /*
         * Find all possible team combinations
         * Determine average rating for each combination
        */

        public static List<Player> ReactedPlayers = new List<Player>();
        public static List<string> emojis = new List<string>(new string[] { ":ZzZz:", ":TankLogo:", ":DpsLogo:", ":SupportLogo:" });
        public static List<Match> MatchHistory = new List<Match>();

        public static void GetReactions(DiscordClient client, DiscordMessage msg)
        {
            ReactedPlayers.Clear();
            DiscordEmoji afk = DiscordEmoji.FromName(client, ":ZzZz:");

            foreach (string role in emojis)
            {
                foreach(DiscordUser u in msg.GetReactionsAsync(DiscordEmoji.FromName(client, role)).Result)
                {
                    using (Player p = new Player(u))
                    {
                        p.Role = role;
                        Player afk_P = p;
                        afk_P.Role = ":ZzZz:";

                        if (!ReactedPlayers.Contains(afk_P))
                        {
                            ReactedPlayers.Add(p);
                        }
                    }
                }
            }
        }

        public static void GetTeams()
        {
            // Return if there are no reactions on the event.
            if (ReactedPlayers.Count == 0) return;

            // Create 2 teams, and a List or Lists that will contain the players available for each role.
            var random = new Random();
            Team[] teams = { new Team(), new Team() };
            List<List<Player>> players = new List<List<Player>>();

            // Logos are the role emoji names. For each role, get a list of available players and add it to our list of lists (players)
            foreach (string logo in emojis)
            {
                if (logo != ":ZzZz:") players.Add(ReactedPlayers.Where(i => i.Role == logo).ToList());
            }

            // For each of the 2 teams, select 2 random players of each role and assign them to the team.
            foreach(Team team in teams)
            {
                foreach(List<Player> rolePlayers in players)
                {
                    using (Player p = rolePlayers[random.Next(rolePlayers.Count())]) { rolePlayers.Remove(team.AddPlayer(p, p.Role));}
                    using (Player p = rolePlayers[random.Next(rolePlayers.Count())]) { rolePlayers.Remove(team.AddPlayer(p, p.Role)); }
                }
            }

            // Save for match history
            if (teams[0].ValidateStructure() && teams[1].ValidateStructure()) MatchHistory.Add(new Match(teams[0], teams[1]));
        }
    }
}
