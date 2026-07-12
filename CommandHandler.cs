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

        public CommandResult HandleDirection(
            Direction direction,
            Player player)
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
                return new CommandResult(
                    message: "You can't go that way.",
                    shouldContinue: true);
            }

            player.CurrentRoom = nextRoom;

            return new CommandResult(
                message:
                    $"{player.CurrentRoom.Name}\n" +
                    $"{player.CurrentRoom.Description}",
                shouldContinue: true);
        }
       
        public CommandResult HandleCommand(string verb, string argument, Player player)
        {
            switch (verb)
            {
                case "score":
                    return ScoreCommand(player);

                case "look":
                    return LookCommand(argument, player);

                case "pigeon":
                    return PigeonSoulCommand();

                case "quit":
                    return QuitSysCommand();

                default:
                    return new CommandResult(
                        message: "Unknown command.",
                        shouldContinue: true);
            }
        }


        private CommandResult LookCommand(
            string argument,
            Player player)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return new CommandResult(
                    message: $"You are standing in {player.CurrentRoom.Name}.",
                    shouldContinue: true
                );
            }

            else if (argument == "me")
            {
                return new CommandResult(
                    message:
                        $"You take a moment to look yourself over. " +
                        $"You are {player.Name}. " +
                        $"Everything appears to be where you left it.",
                    shouldContinue: true
                );
            }

            else
            {
                return new CommandResult(
                    message: $"You can't seem to find {argument}.",
                    shouldContinue: true
                );
            }
        }

        // info commands
        private CommandResult ScoreCommand(Player player)
        {
            return new CommandResult(
                    message: $"HP: {player.HealthPoints}",
                    shouldContinue: true
                );
        }

        // soul commands
        private CommandResult PigeonSoulCommand()
        {
            return new CommandResult(
                message: "The suspicious pigeon is not impressed.",
                shouldContinue: true);
        }

        // system commands
        private CommandResult QuitSysCommand()
        {
            return new CommandResult(
                message: "Goodbye!",
                shouldContinue: false);
        }
    }
}
