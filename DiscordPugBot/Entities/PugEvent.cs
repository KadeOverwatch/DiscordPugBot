using System;

namespace DiscordPugBot.Entities
{
    public class PugEvent : IDisposable
    {
        private bool disposedValue;

        public Guid ID { get; set; }
        public DateTime Scheduled_Date { get; set; }
        public DateTime Created_On { get; set; }
        public string Discord_Message_ID { get; set; }

        public PugEvent()
        {

        }

        public PugEvent(LimitBreakPugsDataSet.EventsRow e)
        {
            this.ID = e.ID;
            this.Scheduled_Date = e.Scheduled_Date;
            this.Created_On = e.Created_On;
            this.Discord_Message_ID = e.Discord_Message_ID;
        }

        public void Insert(DateTime _Scheduled_date, string _Discord_Message_ID)
        {
            Data.eventsTableAdapter.InsertQuery(_Scheduled_date, _Discord_Message_ID);
            Fill();
        }

        public void Insert()
        {
            Data.eventsTableAdapter.InsertQuery(this.Scheduled_Date, this.Discord_Message_ID);
            Fill();
        }

        public void Insert(LimitBreakPugsDataSet.EventsRow e)
        {
            Data.eventsTableAdapter.InsertQuery(e.Scheduled_Date, e.Discord_Message_ID);
            Fill();
        }

        public void Fill()
        {
            Data.eventsTableAdapter.Fill(Data.events);
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
