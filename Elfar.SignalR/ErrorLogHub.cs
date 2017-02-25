namespace Elfar.SignalR
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("errorLogHub")]
    public class ErrorLogHub : Hub {}
}