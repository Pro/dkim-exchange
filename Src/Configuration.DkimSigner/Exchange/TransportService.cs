using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace Configuration.DkimSigner.Exchange
{
    public class TransportService : IDisposable
    {
        public event EventHandler StatusChanged;
        
        private Thread thread = null;
        private Timer transportServiceStatus = null;

        private Queue<TransportServiceAction> actions = null;
        private ServiceController service = null;
        private string status = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public TransportService()
        {
            if(!this.IsTransportServiceInstalled())
            {
                throw new ExchangeServerException("No service 'MSExchangeTransport' available.");
            }

            this.actions = new Queue<TransportServiceAction>();
            this.service = new ServiceController("MSExchangeTransport");
            this.transportServiceStatus = new Timer(new TimerCallback(this.CheckExchangeTransportServiceStatus), null, 0, 1000);
        }

        /// <summary>
        /// Check if Microsoft Exchange Transport service is installed
        /// </summary>
        /// <returns>bool</returns>
        private bool IsTransportServiceInstalled()
        {
            return (ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "MSExchangeTransport") != null);
        }

        /// <summary>
        /// Get Microsoft Exchange Transport service status
        /// </summary>
        /// <returns>ServiceControllerStatus</returns>
        private ServiceControllerStatus GetTransportServiceStatus()
        {
            return service.Status;
        }

        /// <summary>
        /// Check if Microsoft Exchange Transport service is running
        /// </summary>
        /// <returns>bool</returns>
        private bool IsTransportServiceRunning()
        {
            return (this.GetTransportServiceStatus() == ServiceControllerStatus.Running);
        }

        /// <summary>
        /// Check if Microsoft Exchange Transport service is stopped
        /// </summary>
        /// <returns>bool</returns>
        private bool IsTransportServiceStopped()
        {
            return (this.GetTransportServiceStatus() == ServiceControllerStatus.Stopped);
        }

        /// <summary>
        /// Check the Microsoft Exchange Transport service status
        /// </summary>
        /// <param name="state"></param>
        private void CheckExchangeTransportServiceStatus(object state)
        {
            try
            {
                string status = this.GetTransportServiceStatus().ToString();

                if (this.status != status)
                {
                    this.status = status;
                    this.StatusChanged(this, null);
                }
            }
            catch (ExchangeServerException)
            {
                this.transportServiceStatus.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Execute a action (start, stop, restart) on Microsoft Exchange Transport service
        /// </summary>
        private void ExecuteAction()
        {
            while(this.actions.Count > 0)
            {
                TransportServiceAction action;

                lock(this.actions)
                {
                    action = this.actions.Dequeue();
                }
            
                if(action == TransportServiceAction.Start)
                {
                    if (this.IsTransportServiceStopped())
                    {
                        try
                        {
                            this.service.Start();
                            this.service.WaitForStatus(ServiceControllerStatus.Running);
                        }
                        catch (Exception e)
                        {
                            //throw new ExchangeServerException("Couldn't start 'MSExchangeTransport' service :\n" + e.Message, e);
                        }
                    }
                }
                else // action == TransportServiceAction.Stop
                {
                    if(this.IsTransportServiceRunning())
                    {
                        try
                        {
                            this.service.Stop();
                            this.service.WaitForStatus(ServiceControllerStatus.Stopped);
                        }
                        catch (Exception e)
                        {
                            //throw new ExchangeServerException("Couldn't stop 'MSExchangeTransport' service :\n" + e.Message, e);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the current status of Microsoft Exchange Transport service
        /// </summary>
        /// <returns>string</returns>
        public string GetStatus()
        {
            return this.status;
        }

        /// <summary>
        /// Execute a action (start, stop, restart) on Microsoft Exchange Transport service
        /// </summary>
        /// <param name="action">TransportServiceAction</param>
        public void Do(TransportServiceAction action)
        {
            lock(this.actions)
            {
                switch (action)
                {
                    case TransportServiceAction.Start:
                    case TransportServiceAction.Stop:
                        this.actions.Enqueue(action);
                        break;
                    case TransportServiceAction.Restart:
                        this.actions.Enqueue(TransportServiceAction.Stop);
                        this.actions.Enqueue(TransportServiceAction.Start);
                        break;
                }
            }

            if(this.thread == null || this.thread.ThreadState == ThreadState.Stopped)
            {
                this.thread = new Thread(this.ExecuteAction);
                this.thread.Start();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (this.thread != null)
            {
                if(this.thread.ThreadState != ThreadState.Stopped)
                {
                    this.thread.Join();
                }

                this.thread = null;
            }

            if (this.transportServiceStatus != null)
            {
                this.transportServiceStatus.Change(Timeout.Infinite, Timeout.Infinite);
                this.transportServiceStatus.Dispose();
                this.transportServiceStatus = null;
            }

            if (this.service != null)
            {
                this.service.Dispose();
                this.service = null;
            }
        }
    }
}
