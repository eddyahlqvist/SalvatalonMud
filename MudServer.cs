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
        Console.WriteLine($"{_world.Name} is listening on port {Port}...");
        Console.WriteLine("Waiting for travelers...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();

            Console.WriteLine(
                $"[{DateTime.Now:HH:mm:ss}] A client connected.");

            ClientSession session = new(client, _world, this);

            await SendToActivePlayersAsync("A new traveler approaches the gates."); // temporary            

            _ = HandleSession(session);
        }
    }

    internal async Task SendToRoomAsync(Player speaker, string message)
    {
        foreach (Player player in speaker.CurrentRoom.Players)
        {
            if (player != speaker)
            {
                await SendToPlayerAsync(player, message);
            }
        }
    }

    private async Task SendToPlayerAsync(Player targetPlayer, string message)
    {
        foreach (ClientSession active in _sessions)
        {
            if (active.Player == targetPlayer)
            {
                await active.SendMessageAsync(message);
            }
        }
    }

    private async Task SendToActivePlayersAsync(string message)
    {
        foreach (ClientSession active in _sessions)
        {
            if (active.Player != null)
            {
                await active.SendMessageAsync(message);
            }
        }
    }

    private async Task HandleSession(ClientSession session)
    {
        _sessions.Add(session);

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
        }
    }
}