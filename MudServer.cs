using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SalvatalonMud;

internal class MudServer
{
    private readonly World _world;

    private const int Port = 4000;

    public MudServer(World world)
    {
        _world = world;
    }

    public async Task RunAsync()
    {
        TcpListener listener = new(IPAddress.Loopback, Port);

        listener.Start();

        Console.WriteLine($"{_world.Name} is listening on port {Port}...");
        Console.WriteLine("Waiting for travelers...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();

            Console.WriteLine(
                $"[{DateTime.Now:HH:mm:ss}] A client connected.");

            ClientSession session = new(client, _world);

            _ = session.RunAsync();
        }
    }
}