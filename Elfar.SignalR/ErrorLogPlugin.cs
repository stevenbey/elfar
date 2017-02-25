namespace Elfar.SignalR
{
    using Microsoft.AspNet.SignalR;

    public class ErrorLogPlugin : IErrorLogPlugin
    {
        public void Execute(ErrorLog errorLog)
        {
            GlobalHost.ConnectionManager.GetHubContext<ErrorLogHub>().Clients.All.notify();
        }
    }
}