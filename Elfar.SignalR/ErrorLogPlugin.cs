using System.ComponentModel.Composition;
using SignalR;

namespace Elfar.SignalR
{
    [Export("Plugin", typeof(IErrorLogPlugin))]
    public class ErrorLogPlugin : IErrorLogPlugin
    {
        public void Execute(ErrorLog errorLog)
        {
            GlobalHost.ConnectionManager.GetHubContext<ErrorLogHub>().Clients.notify();
        }
    }
}