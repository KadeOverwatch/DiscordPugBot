using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using DSharpPlus.EventArgs;
using DiscordPugBot.Entities;
using System.Data;
using DSharpPlus.Entities;
using DiscordPugBot.Commands;

namespace DiscordPugBot
{
    public class Bot
    {
        public string pugAnnouncementsChannel_ID = "757595493269504061";
        public string pugBotLogsChannel_ID = "757694234085163028";

        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task Start()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.MessageReactionAdded += OnReactionAdded;
            Client.MessageReactionRemoved += OnReactionRemoved;


            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                DmHelp = true,
                EnableDefaultHelp = true,
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<ChatCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }

        private async Task OnReactionAdded(MessageReactionAddEventArgs e)
        {
            if (e.User.IsBot || e.Channel.Id.ToString() != pugAnnouncementsChannel_ID) return;
            if (e.Emoji.Name == "YEA")
            {
                LimitBreakPugsDataSet.PlayersDataTable pdt = Data.playersTableAdapter.GetData();
                LimitBreakPugsDataSet.EventsDataTable edt = Data.eventsTableAdapter.GetData();

                if (pdt.Count > 0)
                {
                    LimitBreakPugsDataSet.PlayersRow p_row = (LimitBreakPugsDataSet.PlayersRow)pdt.Select($"Discord_Tag = '{e.User.Username}#{e.User.Discriminator}'").First();
                    LimitBreakPugsDataSet.EventsRow e_row = (LimitBreakPugsDataSet.EventsRow)edt.Select($"Discord_Message_ID={edt.Discord_Message_IDColumn}").First();

                    Data.registrationsTableAdapter.InsertQuery(e_row.ID, p_row.ID);

                    await LogChannel($"Discord user {p_row.Discord_Tag} just registered for the pugs being held on {e_row.Scheduled_Date.ToShortDateString()} - {e_row.Scheduled_Date.ToShortTimeString()}");
                    await e.Guild.GetMemberAsync(e.User.Id).Result
                        .SendMessageAsync($"You have registered yourself for the pug occuring on {e_row.Scheduled_Date.ToShortDateString()} at {e_row.Scheduled_Date.ToShortTimeString()}. See you there!");
                }
                else
                {
                    await e.Message.DeleteReactionAsync(e.Emoji, e.User);
                    await e.Guild.GetMemberAsync(e.User.Id).Result.SendMessageAsync($"You need to share some information before I can sign you up for pugs! Please use the command '?help register' in the bot commands channel for more information.");
                }
            }

            if (e.Emoji.Name == "NAY")
            {
                await e.Message.DeleteReactionAsync(DiscordEmoji.FromName(e.Client, ":YEA:"), e.User).ConfigureAwait(false);
                await e.Message.DeleteReactionAsync(DiscordEmoji.FromName(e.Client, ":DamageLogo:"), e.User).ConfigureAwait(false);
                await e.Message.DeleteReactionAsync(DiscordEmoji.FromName(e.Client, ":TankLogo:"), e.User).ConfigureAwait(false);
                await e.Message.DeleteReactionAsync(DiscordEmoji.FromName(e.Client, ":SupportLogo:"), e.User).ConfigureAwait(false);
                await e.Message.DeleteReactionAsync(DiscordEmoji.FromName(e.Client, ":ZzZz:"), e.User).ConfigureAwait(false);
            }

            await Task.CompletedTask;
        }

        private async Task OnReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            if (e.Channel.Id.ToString() == pugAnnouncementsChannel_ID && e.Emoji.Name == "YEA" && e.User.IsBot == false)
            {
                LimitBreakPugsDataSet.PlayersDataTable pdt = Data.playersTableAdapter.GetData();
                LimitBreakPugsDataSet.EventsDataTable edt = Data.eventsTableAdapter.GetData();

                if (pdt.Count > 0)
                {
                    LimitBreakPugsDataSet.PlayersRow p_row = (LimitBreakPugsDataSet.PlayersRow)pdt.Select($"Discord_Tag = '{e.User.Username}#{e.User.Discriminator}'").First();
                    LimitBreakPugsDataSet.EventsRow e_row = (LimitBreakPugsDataSet.EventsRow)edt.Select($"Discord_Message_ID={edt.Discord_Message_IDColumn}").First();


                    Data.registrationsTableAdapter.UpdateByEventByPlayer(e_row.ID, p_row.ID, true);

                    await LogChannel($"Discord user {p_row.Discord_Tag} just **unregistered** for the pugs being held on {e_row.Scheduled_Date.ToShortDateString()} - {e_row.Scheduled_Date.ToShortTimeString()}");
                    await e.Guild.GetMemberAsync(e.User.Id).Result
                        .SendMessageAsync($"You have removed yourself from the pug occuring on {e_row.Scheduled_Date.ToShortDateString()} at {e_row.Scheduled_Date.ToShortTimeString()}. See you next time :(.");
                }
            }

            await Task.CompletedTask;
        }

        private async Task LogChannel(string msg)
        {
            DSharpPlus.Entities.DiscordChannel channel = await Client.GetChannelAsync(UInt64.Parse(pugBotLogsChannel_ID));
            DiscordMessage message = await channel.SendMessageAsync(msg);
        }
    }
}
