using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SalvatalonMud;

internal class ClientSession
{
    private readonly TcpClient _client;

    public ClientSession(TcpClient client)
    {
        _client = client;
    }

    public async Task RunAsync()
    {
        try
        {
            await using NetworkStream stream = _client.GetStream();

            using StreamReader reader = new(
                stream,
                new UTF8Encoding(false),
                leaveOpen: true);

            await using StreamWriter writer = new(
                stream,
                new UTF8Encoding(false),
                leaveOpen: true)
            {
                AutoFlush = true
            };

            await writer.WriteLineAsync("Welcome to Salvatalon!");
            await writer.WriteAsync("What is your name? ");

            string? name = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(name))
            {
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
                        await writer.WriteLineAsync("You are standing in Tyrika Square.");
                        await writer.WriteLineAsync("A suspicious pigeon watches you.");
                        break;

                    case "quit":
                        await writer.WriteLineAsync("Goodbye!");
                        return;

                    default:
                        await writer.WriteLineAsync($"You typed: {command}");
                        break;
                }

                await writer.WriteAsync("> ");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _client.Dispose();

            Console.WriteLine("A client disconnected.");
        }
    }
}