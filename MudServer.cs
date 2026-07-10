using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SalvatalonMud;

internal class MudServer
{
    private const int Port = 4000;

    public async Task RunAsync()
    {
        TcpListener listener = new(IPAddress.Loopback, Port);

        listener.Start();

        Console.WriteLine($"Salvatalon is listening on port {Port}...");
        Console.WriteLine("Waiting for travelers...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();

            Console.WriteLine("A client connected.");

            ClientSession session = new(client);

            _ = session.RunAsync();
        }
    }
}