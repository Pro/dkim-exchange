using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace Exchange.DkimConfiguration.Exchange
{
    public class TransportService : IDisposable
    {
        public event EventHandler StatusChanged;
        
        private Thread thread;
        private Timer transportServiceStatus;

        private Queue<TransportServiceAction> actions;
        private Action<string> errorCallback;
        private ServiceController service;
        private string status;

        /// <summary>
        /// Constructor
        /// </summary>
        public TransportService()
        {
            if(!IsTransportServiceInstalled())
            {
                throw new ExchangeServerException("No service 'MSExchangeTransport' available.");
            }

            actions = new Queue<TransportServiceAction>();
            service = new ServiceController("MSExchangeTransport");
            transportServiceStatus = new Timer(CheckExchangeTransportServiceStatus, null, 0, 1000);
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
            return (GetTransportServiceStatus() == ServiceControllerStatus.Running);
        }

        /// <summary>
        /// Check if Microsoft Exchange Transport service is stopped
        /// </summary>
        /// <returns>bool</returns>
        private bool IsTransportServiceStopped()
        {
            return (GetTransportServiceStatus() == ServiceControllerStatus.Stopped);
        }

        /// <summary>
        /// Check the Microsoft Exchange Transport service status
        /// </summary>
        /// <param name="state"></param>
        private void CheckExchangeTransportServiceStatus(object state)
        {
            try
            {
                string s = GetTransportServiceStatus().ToString();

                if (status != s)
                {
                    status = s;
                    if(StatusChanged != null)
                    {
                        StatusChanged(this, null);
                    }
                }
            }
            catch (ExchangeServerException)
            {
                transportServiceStatus.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Execute a action (start, stop, restart) on Microsoft Exchange Transport service
        /// </summary>
        private void ExecuteAction()
        {
            bool queueIsNotEmpty;

            lock (actions)
            {
                queueIsNotEmpty = actions.Count > 0;
            }

            while (queueIsNotEmpty)
            {
                TransportServiceAction action;

                lock(actions)
                {
                    action = actions.Dequeue();
                }
            
                if(action == TransportServiceAction.Start)
                {
                    if (IsTransportServiceStopped())
                    {
                        try
                        {
                            service.Start();
                            service.WaitForStatus(ServiceControllerStatus.Running);
                        }
                        catch (Exception e)
                        {
                            if (errorCallback != null)
                                errorCallback("Couldn't start 'MSExchangeTransport' service :\n" + e.Message + "\nMake sure you are running the program as an administrator.");
                            else
                                throw new ExchangeServerException("Couldn't start 'MSExchangeTransport' service :\n" + e.Message + "\nMake sure you are running the program as an administrator.", e);
                        }
                    }
                }
                else // action == TransportServiceAction.Stop
                {
                    if(IsTransportServiceRunning())
                    {
                        try
                        {
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped);
                        }
                        catch (Exception e)
                        {
                            if (errorCallback != null)
                                errorCallback("Couldn't stop 'MSExchangeTransport' service :\n" + e.Message + "\nMake sure you are running the program as an administrator.");
                            else
                                throw new ExchangeServerException("Couldn't stop 'MSExchangeTransport' service :\n" + e.Message + "\nMake sure you are running the program as an administrator.", e);
                        }
                    }
                }

                lock (actions)
                {
                    queueIsNotEmpty = actions.Count > 0;
                }
            }
        }

        /// <summary>
        /// Get the current status of Microsoft Exchange Transport service
        /// </summary>
        /// <returns>string</returns>
        public string GetStatus()
        {
            return status;
        }

        /// <summary>
        /// Execute a action (start, stop, restart) on Microsoft Exchange Transport service
        /// </summary>
        /// <param name="action">TransportServiceAction</param>
        public void Do(TransportServiceAction action, Action<string> errorCallback)
        {
            this.errorCallback = errorCallback;
            lock(actions)
            {
                switch (action)
                {
                    case TransportServiceAction.Start:
                    case TransportServiceAction.Stop:
                        actions.Enqueue(action);
                        break;
                    case TransportServiceAction.Restart:
                        actions.Enqueue(TransportServiceAction.Stop);
                        actions.Enqueue(TransportServiceAction.Start);
                        break;
                }
            }

            if(thread == null || thread.ThreadState == ThreadState.Stopped)
            {
                thread = new Thread(ExecuteAction);
                thread.Start();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (thread != null)
            {
                if(thread.ThreadState != ThreadState.Stopped)
                {
                    thread.Join();
                }

                thread = null;
            }

            if (transportServiceStatus != null)
            {
                transportServiceStatus.Change(Timeout.Infinite, Timeout.Infinite);
                transportServiceStatus.Dispose();
                transportServiceStatus = null;
            }

            if (service != null)
            {
                service.Dispose();
                service = null;
            }
        }
    }
}
