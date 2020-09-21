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

            // Commands.RegisterCommands<TestCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }

        private async Task OnReactionAdded(MessageReactionAddEventArgs e)
        {
            if (e.Channel.Id.ToString() == pugAnnouncementsChannel_ID)
            {
                /*
                string formattedDTag = e.User.Username + "#" + e.User.Discriminator;
                Player player = new Player();
                PugEvent eve = new PugEvent();
                LimitBreakPugsDataSet.PlayersDataTable dt = Data.playersTableAdapter.FindByDiscordID(e.User.Username + "#" + e.User.Discriminator);
                */

                LimitBreakPugsDataSet.PlayersRow  p_row = (LimitBreakPugsDataSet.PlayersRow)Data.limitBreakPugsDataSet.Tables["Players"].Select($"Discord_Tag = '{e.User.Username}#{e.User.Discriminator}'").FirstOrDefault();
                LimitBreakPugsDataSet.EventsRow e_row = (LimitBreakPugsDataSet.EventsRow)Data.limitBreakPugsDataSet.Tables["Events"].Select($"Discord_Message_ID = '{e.Message.Id}'").FirstOrDefault();

                /*
                foreach(LimitBreakPugsDataSet.PlayersRow row in dt)
                {
                    if (row.Discord_Tag == formattedDTag)
                    {
                        player = new Player(row);
                    }
                }

                LimitBreakPugsDataSet.EventsDataTable et = Data.eventsTableAdapter.FindByDiscordMsgID(e.Message.Id.ToString());
                foreach(LimitBreakPugsDataSet.EventsRow row in et)
                {
                    if (row.Discord_Message_ID == e.Message.Id.ToString())
                    {
                        eve = new PugEvent(row);
                    }
                }
                */

                Data.registrationsTableAdapter.InsertQuery(e_row.ID, p_row.ID);
                await e.Guild.GetMemberAsync(e.User.Id).Result
                    .SendMessageAsync($"Success?");


            }
            await Task.CompletedTask;
        }

        private async Task OnReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            
            await Task.CompletedTask;
        }
    }
}
