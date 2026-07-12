using System;
using System.Diagnostics;
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
        // log session time (start)
        Stopwatch stopwatch = Stopwatch.StartNew();

        // manage the client connection
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

            // welcome client and prepare for character creation
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

            await DisplayExitsAsync(writer, _player.CurrentRoom);

            await writer.WriteLineAsync(
                $"Hello, {_player.Name}. You have {_player.HealthPoints} health points.");

            await writer.WriteAsync("> ");

            while (true)
            {

                // read and normalize player input
                string? command = await reader.ReadLineAsync();

                if (command is null)
                {
                    break;
                }

                command = command.Trim().ToLowerInvariant();

                // split input into verb and arguments
                string verb;
                string argument;
                int firstSpace = command.IndexOf(' ');

                if (firstSpace == -1)
                {
                    verb = command;
                    argument = "";
                }

                else
                {
                    verb = command[..firstSpace];
                    argument = command[(firstSpace + 1)..].Trim();
                }

                // check if input is about player movement
                CommandResult result;

                if (_commandHandler.TryGetDirection(
                    verb,
                    out Direction direction))
                {
                    result = _commandHandler.HandleDirection(
                        direction,
                        _player);

                    await writer.WriteLineAsync(result.Message);                    
                    await DisplayExitsAsync(
                        writer,
                        _player.CurrentRoom);
                }

                // if input is not about player movement handle it here
                else
                {
                    result = _commandHandler.HandleCommand(
                        verb,
                        argument,
                        _player);

                    await writer.WriteLineAsync(result.Message);
                }

                // end the session if requested by the command
                if (!result.ShouldContinue)
                {
                    return;
                }

                await writer.WriteAsync("> ");
            }
        }

        // handle unexpected connection errors
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
        }

        // end session and show session time in console
        finally
        {
            stopwatch.Stop();

            Console.WriteLine(
                $"[{DateTime.Now:HH:mm:ss}] Session lasted {stopwatch.Elapsed:mm\\:ss}.");

            _client.Dispose();

            Console.WriteLine("A client disconnected.");
        }
    }

    private async Task DisplayExitsAsync(
        StreamWriter writer,
        Room currentRoom)
    {
        await writer.WriteLineAsync(currentRoom.GetExitShort());
    }
}