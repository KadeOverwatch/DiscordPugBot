using DiscordPugBot.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace DiscordPugBot.Commands
{
    public class ChatCommands : BaseCommandModule
    {
        public string pugAnnouncementsChannel_ID = "757595493269504061";
        public string pugBotLogsChannel_ID = "757694234085163028";

        [Command("CreatePug")]
        public async Task CreatePug(CommandContext ctx, DateTime scheduledDate)
        {
            DiscordMessage msg = await ctx.Channel.SendMessageAsync($"Attempting scheduling...").ConfigureAwait(false);
            await LogChannel(ctx, $"Discord user {ctx.User.Username} created a new Pug Event on {scheduledDate.ToShortDateString()}  at {scheduledDate.ToShortTimeString()}");
            await AnnounceEvent(ctx, scheduledDate, msg);
        }

        [Command("Register")]
        [Description("Example: ?register Kade#12345 1500")]
        public async Task RegisterPlayer(CommandContext ctx, string _Battle_Tag, int _Player_Rank)
        {
            DiscordMessage msg = await ctx.Channel.SendMessageAsync($"Attempting registration...").ConfigureAwait(false);

            Player p = new Player();
            p.Battle_Tag = _Battle_Tag;
            p.Player_Rank = _Player_Rank;
            p.Discord_Tag = ctx.User.Username + "#" + ctx.User.Discriminator;
            p.Player_Team = "Not Implemented";
            p.Insert();

            await LogChannel(ctx, $"Discord user {p.Discord_Tag} registered as a new user with the following bnet account: {_Battle_Tag} - SR: {_Player_Rank}");
            await msg.ModifyAsync("You have successfully registered! You may now sign up for pugs by reacting to the announcement of them.").ConfigureAwait(false);
        }

        public async Task AnnounceEvent(CommandContext ctx, DateTime scheduledDate, DiscordMessage msg)
        {
            DiscordChannel channel = await ctx.Client.GetChannelAsync(UInt64.Parse(pugAnnouncementsChannel_ID));
            DiscordMessage message = await channel.SendMessageAsync($"New Pug Event scheduled on {scheduledDate.ToShortDateString()} at {scheduledDate.ToShortTimeString()}! Please react to sign-up! (Any reaction)");

            PugEvent e = new PugEvent();
            e.Scheduled_Date = scheduledDate;
            e.Discord_Message_ID = message.Id.ToString();

            Data.eventsTableAdapter.InsertQuery(scheduledDate, message.Id.ToString());
            await msg.ModifyAsync("The event has been scheduled!").ConfigureAwait(false);
        }

        private async Task LogChannel(CommandContext ctx, string msg)
        {
            DiscordChannel channel = await ctx.Client.GetChannelAsync(UInt64.Parse(pugBotLogsChannel_ID));
            DiscordMessage message = await channel.SendMessageAsync(msg);
        }
    }
}
