namespace DiscordPugBot.Entities
{
    public class Match
    {
        public Team team1 { get; set; }
        public Team team2 { get; set; }
        public Match(Team team1, Team team2)
        {
            this.team1 = team1;
            this.team2 = team2;
        }
    }
}
