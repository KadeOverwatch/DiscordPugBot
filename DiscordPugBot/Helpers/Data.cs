using System.Globalization;

namespace DiscordPugBot
{
    public static class Data
    {
        public static TextInfo t = new CultureInfo("en-US", false).TextInfo;

        public static LimitBreakPugsDataSet limitBreakPugsDataSet = new LimitBreakPugsDataSet();

        public static LimitBreakPugsDataSetTableAdapters.PlayersTableAdapter playersTableAdapter = new LimitBreakPugsDataSetTableAdapters.PlayersTableAdapter();
        public static LimitBreakPugsDataSetTableAdapters.EventsTableAdapter eventsTableAdapter = new LimitBreakPugsDataSetTableAdapters.EventsTableAdapter();
        public static LimitBreakPugsDataSetTableAdapters.RegistrationsTableAdapter registrationsTableAdapter = new LimitBreakPugsDataSetTableAdapters.RegistrationsTableAdapter();
        public static LimitBreakPugsDataSetTableAdapters.OverwatchMapsTableAdapter overwatchMapsTableAdapter = new LimitBreakPugsDataSetTableAdapters.OverwatchMapsTableAdapter();

        public static LimitBreakPugsDataSet.EventsDataTable events = new LimitBreakPugsDataSet.EventsDataTable();
        public static LimitBreakPugsDataSet.PlayersDataTable players = new LimitBreakPugsDataSet.PlayersDataTable();
        public static LimitBreakPugsDataSet.RegistrationsDataTable registrations = new LimitBreakPugsDataSet.RegistrationsDataTable();
        public static LimitBreakPugsDataSet.OverwatchMapsDataTable overwatchMaps = new LimitBreakPugsDataSet.OverwatchMapsDataTable();

        public static void LoadTables()
        {
            playersTableAdapter.Fill(players);
            eventsTableAdapter.Fill(events);
            registrationsTableAdapter.Fill(registrations);
            overwatchMapsTableAdapter.Fill(overwatchMaps);
        }

        public static string ProperCase(string str)
        {
            return t.ToTitleCase(str.ToLower());
        }
    }
}
