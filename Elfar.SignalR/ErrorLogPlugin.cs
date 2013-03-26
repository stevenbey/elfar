using SignalR;

namespace Elfar.SignalR
{
    public class ErrorLogPlugin : IErrorLogPlugin
    {
        public void Execute(ErrorLog errorLog)
        {
            GlobalHost.ConnectionManager.GetHubContext<ErrorLogHub>().Clients.notify();
        }
    }
}