// See https://aka.ms/new-console-template for more information

using Autofac;
using Microsoft.AspNetCore.SignalR.Client;
using Qna.Game.OnlineServer.SignalR.Client.Client;
using Qna.Game.OnlineServer.SignalR.Client.Client.Core;
using Qna.Game.OnlineServer.SignalR.Client.Client.TicTacToe;
using Qna.Game.OnlineServer.SignalR.Tests;
using Qna.Game.OnlineServer.SignalR.Tests2;

Console.WriteLine("Hello, World!");

var builder = new ContainerBuilder();

// begin register implementation
builder.RegisterType<MessageCallbackHandler>().As<ITicTacToeCallbackHandler>();
builder.RegisterType<TicTacToeClient2>().As<TicTacToeClient2>();
// end register implementation

var container = builder.Build();

await using var scope = container.BeginLifetimeScope();

// create connection with server
Console.WriteLine("start dummy client");
await using var client = scope.Resolve<TicTacToeClient2>();
await client.StartAsync();

// try to send a message
client.SayHelloAsync();

// keep it run until connection dropped
while (client.State != HubConnectionState.Disconnected)
{
    Console.WriteLine(client.State.ToString());
    Thread.Sleep(2000);
}
Console.Write("stopped");