using System.IO;
using System.Threading.Tasks;

namespace SalvatalonMud
{
    internal class CommandHandler
    {
        public bool TryGetDirection(string input, out Direction direction)
        {
            switch (input)
            {
                case "north":
                case "n":
                    direction = Direction.North;
                    return true;

                case "south":
                case "s":
                    direction = Direction.South;
                    return true;

                case "east":
                case "e":
                    direction = Direction.East;
                    return true;

                case "west":
                case "w":
                    direction = Direction.West;
                    return true;

                default:
                    direction = default;
                    return false;
            }
        }

        public string HandleDirection(Direction direction, Player player)
        {
            Room? nextRoom = direction switch
            {
                Direction.North => player.CurrentRoom.North,
                Direction.South => player.CurrentRoom.South,
                Direction.East => player.CurrentRoom.East,
                Direction.West => player.CurrentRoom.West,
                _ => null
            };

            if (nextRoom is null)
            {
                return "You can't go that way.";
            }

            player.CurrentRoom = nextRoom;

            return $"{player.CurrentRoom.Name}\n" +
                   $"{player.CurrentRoom.Description}";
        }

        public async Task<bool> HandleCommandAsync(
            string verb,
            string argument,
            Player player,
            StreamWriter writer)
        {
            switch (verb)
            {
                case "look":
                    await LookCommandAsync(argument, player, writer);
                    return true;

                case "score":
                    await ScoreCommandAsync(player, writer);
                    return true;

                case "pigeon":
                    await PigeonSoulAsync(writer);
                    return true;

                case "quit":
                    await QuitCommandAsync(writer);
                    return false;

                default:
                    await writer.WriteLineAsync("Unknown command.");
                    return true;
            }
        }

        private async Task LookCommandAsync(
            string argument,
            Player player,
            StreamWriter writer)
        {
            if(string.IsNullOrWhiteSpace(argument))
            {
                await writer.WriteLineAsync(
                $"You are standing in {player.CurrentRoom.Name}.");
            }

            else if (argument == "me")
            {
                await writer.WriteLineAsync($"You take a moment to look yourself over. " +
                    $"You are {player.Name}. " +
                    $"Everything appears to be where you left it.");
            }

            else
            {
                await writer.WriteLineAsync($"You can't seem to find {argument}.");
            }
        }

        // info commands
        private async Task ScoreCommandAsync(Player player, StreamWriter writer)
        {
            await writer.WriteLineAsync($"HP: {player.HealthPoints}");
        }

        // system commands
        private async Task QuitCommandAsync(StreamWriter writer)
        {
            await writer.WriteLineAsync("Goodbye!");
        }

        // souls
        private async Task PigeonSoulAsync(StreamWriter writer)
        {
            await writer.WriteLineAsync("The suspicious pigeon is not impressed.");
        }
    }
}
