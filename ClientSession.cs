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
    private readonly MudServer _server;
    internal Player? Player { get; private set; }
    private StreamWriter? _writer;

    public ClientSession(TcpClient client, World world, MudServer mudServer)
    {
        _client = client;
        _world = world;
        _server = mudServer;
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

            _writer = writer;

            // welcome client and prepare for character creation
            await writer.WriteLineAsync($"Welcome to {_world.Name}!");
            await writer.WriteAsync("What is your name? ");

            string? name = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            name = name.Trim();

            // set race (temporary fixed solution)
            Race human = new(
                name: "Human",
                startingStrength: 13,
                startingDexterity: 13,
                startingConstitution: 13,
                startingIntelligence: 13,
                startingWisdom: 13);

            // create a player
            PlayerBuilder playerBuilder = new();
            Player = playerBuilder.Build(
                name: name,
                startingRoom: _world.StartingRoom,
                race: human
                );

            Player.CurrentRoom.Players.Add(Player);

            await writer.WriteLineAsync();

            await writer.WriteLineAsync(
                $"Hello, {Player.Name}. Welcome to {_world.Name}.\n");

            await writer.WriteLineAsync(Player.CurrentRoom.GetDisplayText(includeDescription: true, Player)); // display login room            

            await writer.WriteAsync("> ");

            while (true)
            {

                // read and normalize player input
                string? command = await reader.ReadLineAsync();

                if (command is null)
                {
                    break;
                }

                command = command.Trim();
                string rawCommand = command;
                command = command.ToLowerInvariant();

                if (string.IsNullOrEmpty(command))
                {
                    await writer.WriteAsync("> ");
                    continue;
                }

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

                    if (verb == "say")
                    {
                        argument = rawCommand[(firstSpace + 1)..].Trim();
                    }
                }

                // check if input is about player movement
                CommandResult result;

                if (_commandHandler.TryGetDirection(
                    verb,
                    out Direction direction))
                {
                    result = _commandHandler.HandleDirection(
                        direction,
                        Player);
                }
                else
                {
                    result = _commandHandler.HandleCommand(
                        verb,
                        argument,
                        Player
                        );
                }

                await writer.WriteLineAsync(result.Message);

                if (result.RoomMessage != null)
                {
                    await _server.SendToRoomAsync(Player, result.RoomMessage);
                }

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
            Console.WriteLine($"Connection error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected session error: {ex}");
        }

        // end session and show session time in console
        finally
        {
            stopwatch.Stop();

            Console.WriteLine(
                $"[{DateTime.Now:HH:mm:ss}] Session lasted {stopwatch.Elapsed:mm\\:ss}.");

            if (Player != null)
            {
                Player.CurrentRoom.Players.Remove(Player);
            }

            _client.Dispose();

            Console.WriteLine("A client disconnected.");
        }
    }

    public async Task SendMessageAsync(string message)
    {
        if (_writer == null)
        {
            return;
        }

        await _writer.WriteLineAsync(message);
    }
}