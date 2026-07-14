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
            Room? nextRoom = player.CurrentRoom.GetExit(direction);

            if (nextRoom is null)
            {
                return new CommandResult(
                    message: "You can't go that way.",
                    shouldContinue: true);
            }

            player.CurrentRoom = nextRoom;

            return new CommandResult(
                message:
                    $"You move {direction.ToString().ToLowerInvariant()}.\n" +
                    player.CurrentRoom.GetDisplayText(),
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

                case "push":
                    return PushCommand(argument, player);

                case "quit":
                    return QuitCommand();

                default:
                    return new CommandResult(
                        message: "Unknown command.",
                        shouldContinue: true);
            }
        }

        private TargetInfo ParseTarget(string input)
        {
            input = input.Trim();

            int lastSpace = input.LastIndexOf(' ');

            if (lastSpace == -1)
            {
                return new TargetInfo(
                    name: input,
                    index: 1);
            }

            string possibleIndex =
                input[(lastSpace + 1)..].Trim();

            if (!int.TryParse(possibleIndex, out int index) ||
                index < 1)
            {
                return new TargetInfo(
                    name: input,
                    index: 1);
            }

            string target =
                input[..lastSpace].Trim();

            return new TargetInfo(
                name: target,
                index: index);
        }


        private CommandResult PushCommand(
            string argument,
            Player player)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return new CommandResult(
                    message: "Push what?",
                    shouldContinue: true);
            }

            if (argument == "me")
            {
                return new CommandResult(
                    message: "You fail to push yourself over.",
                    shouldContinue: true);
            }

            int lastSpace = argument.LastIndexOf(' ');

            if (lastSpace == -1)
            {
                return new CommandResult(
                    message: "Push what in which direction?", // fix better version later
                    shouldContinue: true);
            }

            string target = argument[..lastSpace].Trim();
            string directionInput =
                argument[(lastSpace + 1)..].Trim();

            if (!TryGetDirection(
                directionInput,
                out Direction direction))
            {
                return new CommandResult(
                    message: $"{directionInput} is not a valid direction.",
                    shouldContinue: true);
            }

            Npc? targetNpc = null;

            foreach (Npc npc in player.CurrentRoom.Npcs)
            {
                if (npc.Matches(target))
                {
                    targetNpc = npc;
                    break;
                }
            }

            if (targetNpc is null)
            {
                return new CommandResult(
                    message: $"You can't seem to find {target}.",
                    shouldContinue: true);
            }

            Room? destination =
                player.CurrentRoom.GetExit(direction);

            if (destination is null)
            {
                return new CommandResult(
                    message:
                        $"There is no exit {directionInput}.",
                    shouldContinue: true);
            }

            targetNpc.MoveTo(destination);

            return new CommandResult(
                message:
                    $"You push {targetNpc.DisplayName} " +
                    $"{directionInput}.",
                shouldContinue: true);
        }


        private CommandResult LookCommand(
            string argument,
            Player player)
        {            
            if (string.IsNullOrWhiteSpace(argument))
            {
                return new CommandResult(
                    message: player.CurrentRoom.GetDisplayText(),
                    shouldContinue: true);
            }

            if (argument == "me")
            {
                return new CommandResult(
                    message:
                        $"You take a moment to look yourself over. " +
                        $"You are {player.Name}. " +
                        $"Everything appears to be where you left it.",
                    shouldContinue: true
                );
            }

            int currentMatch = 0;

            TargetInfo target = ParseTarget(argument);

            foreach (Npc npc in player.CurrentRoom.Npcs)
            {
                if (!npc.Matches(target.Name))
                {
                    continue;
                }

                currentMatch++;

                if (currentMatch == target.Index)
                {
                    return new CommandResult(
                        message: npc.Description,
                        shouldContinue: true);
                }
            }

            return new CommandResult(
            message: $"You can't seem to find {argument}.",
            shouldContinue: true
            );
        }

        // info commands
        private CommandResult ScoreCommand(Player player)
        {
            return new CommandResult(
                    message: $"HP: {player.HealthPoints}",
                    shouldContinue: true
                );
        }

        // system commands
        private CommandResult QuitCommand()
        {
            return new CommandResult(
                message: "Goodbye!",
                shouldContinue: false);
        }
    }
}
