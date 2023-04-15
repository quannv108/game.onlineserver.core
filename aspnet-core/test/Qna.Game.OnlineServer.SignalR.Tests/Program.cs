// See https://aka.ms/new-console-template for more information

using Autofac;
using Microsoft.AspNetCore.SignalR.Client;
using Qna.Game.OnlineServer.SignalR.Client.Client;
using Qna.Game.OnlineServer.SignalR.Tests;

Console.WriteLine("Hello, World!");

var builder = new ContainerBuilder();

// begin register implementation
builder.RegisterType<MessageCallbackHandler>().As<IMessageCallbackHandler>();
builder.RegisterType<DummyClient>().As<DummyClient>();
// end register implementation

var container = builder.Build();

await using var scope = container.BeginLifetimeScope();

// create connection with server
Console.WriteLine("start dummy client");
await using var client = scope.Resolve<DummyClient>();
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