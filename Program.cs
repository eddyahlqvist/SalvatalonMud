using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SalvatalonMud;

internal static class Program
{
    private const int Port = 4000;

    private static async Task Main()
    {
        TcpListener server = new(IPAddress.Loopback, Port);

        server.Start();

        Console.WriteLine($"Salvatalon is listening on port {Port}...");
        Console.WriteLine("Waiting for travelers...");

        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();

            Console.WriteLine("A client connected.");

            _ = HandleClientAsync(client);
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            await using NetworkStream stream = client.GetStream();

            using StreamReader reader = new(
                stream,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                leaveOpen: true);

            await using StreamWriter writer = new(
                stream,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                leaveOpen: true)
            {
                AutoFlush = true
            };

            await writer.WriteLineAsync("Welcome to Salvatalon!");
            await writer.WriteAsync("What is your name? ");

            string? name = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(name))
            {
                await writer.WriteLineAsync();
                await writer.WriteLineAsync("No name was given. The mists claim you...");
                return;
            }

            name = name.Trim();

            await writer.WriteLineAsync();
            await writer.WriteLineAsync(
                $"Hello, {name}. You are standing in Tyrika Square.");
            await writer.WriteAsync("> ");

            while (true)
            {
                string? command = await reader.ReadLineAsync();

                if (command is null)
                {
                    break;
                }

                command = command.Trim().ToLowerInvariant();

                switch (command)
                {
                    case "look":
                        await writer.WriteLineAsync(
                            "You are standing in Tyrika Square.");
                        await writer.WriteLineAsync(
                            "A suspicious pigeon watches you.");
                        break;

                    case "quit":
                        await writer.WriteLineAsync("Goodbye!");
                        return;

                    case "":
                        break;

                    default:
                        await writer.WriteLineAsync(
                            $"You typed: {command}");
                        break;
                }

                await writer.WriteAsync("> ");
            }
        }
        catch (IOException exception)
        {
            Console.WriteLine(
                $"Connection ended unexpectedly: {exception.Message}");
        }
        finally
        {
            client.Dispose();
            Console.WriteLine("A client disconnected.");
        }
    }
}