﻿
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CutMp3.WebApp.Server.Hubs;
public class DownloadHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}
