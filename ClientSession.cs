using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SalvatalonMud;

internal class ClientSession
{
    private readonly TcpClient _client;
    private readonly CommandHandler _commandHandler = new();
    private readonly World _world;
    private Player? _player;

    public ClientSession(TcpClient client, World world)
    {
        _client = client;
        _world = world;
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

            await writer.WriteLineAsync($"Welcome to {_world.Name}!");
            await writer.WriteAsync("What is your name? ");

            string? name = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            name = name.Trim();

            // create a player
            PlayerBuilder playerBuilder = new();
            _player = playerBuilder.Build(name, _world.StartingRoom);

            await writer.WriteLineAsync();
            await writer.WriteLineAsync(
                $"Hello, {_player.Name}. You have {_player.HealthPoints} health points.");

            await writer.WriteAsync("> ");

            while (true)
            {
                string? command = await reader.ReadLineAsync();

                if (command is null)
                {
                    break;
                }

                command = command.Trim().ToLowerInvariant();

                if (_commandHandler.TryGetDirection(command, out Direction direction))
                {
                    string result = _commandHandler.HandleDirection(direction, _player);

                    await writer.WriteLineAsync(result);
                }
                else
                {
                    switch (command)
                    {
                        case "look":
                            await writer.WriteLineAsync($"You are standing in {_player.CurrentRoom.Name}.");
                            await writer.WriteLineAsync("A suspicious pigeon watches you.");
                            break;

                        case "quit":
                            await writer.WriteLineAsync("Goodbye!");
                            return;

                        default:
                            await writer.WriteLineAsync($"You typed: {command}");
                            break;
                    }
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