using DSharpPlus.Entities;
using System;
using System.Linq;

namespace DiscordPugBot
{
    public class Player : IDisposable
    {
        private bool disposedValue;

        public Guid ID { get; set; }
        public string Battle_Tag { get; set; }
        public string Discord_Tag { get; set; }
        public int Player_Rank { get; set; }
        public string Player_Team { get; set; }
        public DateTime Created_On { get; set; }
        public DateTime Modified_On { get; set; }

        public string Role { get; set; }

        public Player()
        {

        }

        public Player(LimitBreakPugsDataSet.PlayersRow player)
        {
            this.ID = player.ID;
            this.Battle_Tag = player.Battle_Tag;
            this.Discord_Tag = player.Discord_Tag;
            this.Player_Rank = player.Player_Rank;
            this.Player_Team = player.Player_Team;
            this.Created_On = player.Created_On;
            this.Modified_On = player.Modified_On;
        }

        public Player(DiscordUser discordUser)
        {
            LimitBreakPugsDataSet.PlayersRow player = Data.playersTableAdapter.FindByDiscordID(discordUser.Username + "#" + discordUser.Discriminator).First();
            
            this.ID = player.ID;
            this.Battle_Tag = player.Battle_Tag;
            this.Discord_Tag = player.Discord_Tag;
            this.Player_Rank = player.Player_Rank;
            this.Player_Team = player.Player_Team;
            this.Created_On = player.Created_On;
            this.Modified_On = player.Modified_On;
        }

        public void Update()
        {
            Data.playersTableAdapter.UpdateQuery(this.ID, this.Battle_Tag, this.Discord_Tag, this.Player_Rank, this.Player_Team);
            Fill();
        }
        public void Update(Guid _ID, string _Battle_Tag = null, string _Discord_Tag = null, int _Player_Rank = 0, string _Player_Team = null)
        {
            Player p = new Player();
            if (_Battle_Tag == null) _Battle_Tag = this.Battle_Tag;
            if (_Discord_Tag == null) _Discord_Tag = this.Discord_Tag;
            if (_Player_Rank == 0) _Player_Rank = this.Player_Rank;
            if (_Player_Team == null) _Player_Team = this.Player_Team;

            Data.playersTableAdapter.UpdateQuery(ID, _Battle_Tag, _Discord_Tag, _Player_Rank, _Player_Team);
            Fill();
        }

        public void Insert()
        {
            Data.playersTableAdapter.InsertQuery(this.Battle_Tag, this.Discord_Tag, this.Player_Rank, this.Player_Team);
            Fill();
        }

        public void Insert(string _Battle_Tag, string _Discord_Tag, int _Player_Rank, string _Player_Team)
        {
            Data.playersTableAdapter.InsertQuery(_Battle_Tag, _Discord_Tag, _Player_Rank, _Player_Team);
            Fill();
        }

        public void Insert(LimitBreakPugsDataSet.PlayersRow player)
        {
            Data.playersTableAdapter.InsertQuery(player.Battle_Tag, player.Discord_Tag, player.Player_Rank, player.Player_Team);
            Fill();
        }

        public void Insert(Player player)
        {
            Data.playersTableAdapter.InsertQuery(player.Battle_Tag, player.Discord_Tag, player.Player_Rank, player.Player_Team);
            Fill();
        }

        public void Fill()
        {
            Data.playersTableAdapter.Fill(Data.players);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
