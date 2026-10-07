using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SalvatalonMud;

internal class MudServer
{
    private readonly World _world;

    private const int Port = 4000;
    private List<ClientSession> _sessions = new();

    public MudServer(World world)
    {
        _world = world;
    }

    public async Task RunAsync()
    {
        TcpListener listener = new(IPAddress.Loopback, Port);

        listener.Start();
        Console.WriteLine($"Amount of sessions: {_sessions.Count}."); // tmp for testing
        Console.WriteLine($"{_world.Name} is listening on port {Port}...");
        Console.WriteLine("Waiting for travelers...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();

            Console.WriteLine(
                $"[{DateTime.Now:HH:mm:ss}] A client connected.");

            ClientSession session = new(client, _world);
            _ = HandleSession(session);            
        }
    }

    private async Task HandleSession(ClientSession session)
    {
        _sessions.Add(session);
        Console.WriteLine($"Amount of sessions: {_sessions.Count}."); // tmp for testing
        try
        {
            await session.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Session error: {ex.Message}");
        }
        finally 
        { 
            _sessions.Remove(session);
            Console.WriteLine($"Amount of sessions: {_sessions.Count}."); // tmp for testing
        }        
    }
}