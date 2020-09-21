using System;

namespace DiscordPugBot.Entities
{
    public class Registration : IDisposable
    {
        private bool disposedValue;

        public Guid ID { get; set; }
        public Guid Event_ID { get; set; }
        public Guid Player_ID { get; set; }
        public DateTime Created_On { get; set; }
        public bool Player_Cancelled { get; set; }

        public Registration ()
        {

        }

        public Registration(LimitBreakPugsDataSet.RegistrationsRow reg)
        {
            this.ID = reg.ID;
            this.Event_ID = reg.Event_ID;
            this.Player_ID = reg.Player_ID;
            this.Player_Cancelled = reg.Player_Cancelled;
            this.Created_On = reg.Created_On;
        }

        public void Update()
        {
            Data.registrationsTableAdapter.UpdateQuery(this.ID, this.Player_Cancelled);
            Fill();
        }

        public void Update(Guid _ID, bool _Player_Cancelled)
        {
            Data.registrationsTableAdapter.UpdateQuery(_ID, _Player_Cancelled);
            Fill();
        }

        public void Insert()
        {
            Data.registrationsTableAdapter.InsertQuery(this.Event_ID, this.Player_ID);
            Fill();
        }

        public void Insert(LimitBreakPugsDataSet.RegistrationsRow reg)
        {
            Data.registrationsTableAdapter.InsertQuery(reg.Event_ID, reg.Player_ID);
            Fill();
        }

        public void Insert(Guid _Event_ID, Guid _Player_ID)
        {
            Data.registrationsTableAdapter.InsertQuery(_Event_ID, _Player_ID);
            Fill();
        }

        private void Fill()
        {
            Data.registrationsTableAdapter.Fill(Data.registrations);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Registration()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
