using System;
using System.Collections.Generic;
using System.Threading;

namespace Configuration.DkimSigner.Exchange
{
    public class TransportServiceStatus : IDisposable
    {
        private List<TransportServiceStatusObserver> observers;

        private Timer tiTransportServiceStatus = null;

        private string sStatus = null;

        public TransportServiceStatus()
        {
            this.observers = new List<TransportServiceStatusObserver>();

            this.tiTransportServiceStatus = new Timer(new TimerCallback(this.CheckExchangeTransportServiceStatus), null, 0, 1000);
        }

        /// <summary>
        /// Check the Microsoft Exchange Transport Service Status
        /// </summary>
        /// <param name="state"></param>
        private void CheckExchangeTransportServiceStatus(object state)
        {
            try
            {
                this.sStatus = ExchangeServer.GetTransportServiceStatus().ToString();
                this.Notify();
            }
            catch (ExchangeServerException)
            {
                this.tiTransportServiceStatus.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public string GetStatus()
        {
            return this.sStatus;
        }

        public void Subscribe(TransportServiceStatusObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(TransportServiceStatusObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            observers.ForEach(x => x.UpdateTransportStatus());
        }

        public void Dispose()
        {
            if (this.tiTransportServiceStatus != null)
            {
                this.tiTransportServiceStatus.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }
    }
}
