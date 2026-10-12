using System.Collections.Generic;

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

            player.MoveTo(nextRoom);

            return new CommandResult(
                message:
                    $"You move {direction.ToString().ToLowerInvariant()}.\n" +
                    player.CurrentRoom.GetDisplayText(includeDescription: true, player),
                shouldContinue: true);
        }

        public CommandResult HandleCommand(string verb, string argument, Player player)
        {
            switch (verb)
            {
                case "score":
                    return ScoreCommand(argument, player);

                case "look" or "l":
                    return LookCommand(argument, player);

                case "glance":
                    return GlanceCommand(argument, player);

                case "push":
                    return PushCommand(argument, player);

                case "say":
                    return SayCommand(argument, player);

                case "get":
                    return GetCommand(argument, player);

                case "drop":
                    return DropCommand(argument, player);

                case "inventory" or "i":
                    return InventoryCommand(player);

                case "save":
                    return SaveCommand();

                case "quit":
                    return QuitCommand(player);

                case "analyze":
                    return AnalyzeCommand(player, argument);

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

        private CommandResult InventoryCommand(Player player)
        {
            int currentWeight = player.GetInventoryWeight();

            string inventoryText = "Inventory:\n";

            if (player.Inventory.Count == 0)
            {
                inventoryText += "You are not carrying anything.\n";
            }

            foreach (Item item in player.Inventory)
            {
                inventoryText += $"{item.DisplayName}\n";
            }

            inventoryText += $"Weight: {currentWeight} / {player.MaxCarryWeight}.";

            return new CommandResult(
                message: inventoryText,
                shouldContinue: true);
        }

        private CommandResult GetCommand(string argument, Player player)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return new CommandResult(
                    message: "Get what?",
                    shouldContinue: true);
            }

            TargetInfo parsedTarget = ParseTarget(argument);

            Item? itemTarget = FindItem(player.CurrentRoom.Items, parsedTarget);

            if (itemTarget is null)
            {
                return new CommandResult(
                message: $"You can't seem to find {parsedTarget.Name}.",
                shouldContinue: true);
            }

            int currentWeight = player.GetInventoryWeight();

            if (itemTarget.Weight + currentWeight > player.MaxCarryWeight)
            {
                return new CommandResult(
                    message: $"You are too weak to pick up the {itemTarget.Name}. Should not have skipped leg day!", // better message later
                    shouldContinue: true);
            }

            itemTarget.MoveToInventory(player);
            return new CommandResult(
                        message: $"You get {itemTarget.Name} from {player.CurrentRoom.Name}.",
                        shouldContinue: true);
        }

        private CommandResult DropCommand(string argument, Player player)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return new CommandResult(
                    message: "Drop what?",
                    shouldContinue: true);
            }

            TargetInfo parsedTarget = ParseTarget(argument);

            Item? itemTarget = FindItem(player.Inventory, parsedTarget);

            if (itemTarget is null)
            {
                return new CommandResult(
                 message: $"You can't seem to find any {parsedTarget.Name} to drop.",
                 shouldContinue: true);
            }

            itemTarget.MoveToRoom(player);

            return new CommandResult(
                        message: $"You drop a {itemTarget.Name}.",
                        shouldContinue: true);
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
                    message: "Push what in which direction?",
                    shouldContinue: true);
            }

            string targetInput = argument[..lastSpace].Trim();

            string directionInput =
                argument[(lastSpace + 1)..].Trim();

            TargetInfo parsedTarget = ParseTarget(targetInput);

            if (!TryGetDirection(
                directionInput,
                out Direction direction))
            {
                return new CommandResult(
                    message: $"{directionInput} is not a valid direction.",
                    shouldContinue: true);
            }

            Npc? npcTarget = FindNpc(player.CurrentRoom.Npcs, parsedTarget);

            if (npcTarget is null)
            {
                return new CommandResult(
                    message: $"You can't seem to find {parsedTarget.Name}.",
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

            npcTarget.MoveTo(destination);

            return new CommandResult(
                message:
                    $"You push {npcTarget.DisplayName} " +
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
                    message: player.CurrentRoom.GetDisplayText(includeDescription: true, player),
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

            if (TryGetDirection(
                argument,
                out Direction direction))
            {
                Room? nextRoom = player.CurrentRoom.GetExit(direction);

                if (nextRoom is null)
                {
                    return new CommandResult(
                        message: "You see nothing of interest in that direction.",
                        shouldContinue: true);
                }

                return new CommandResult(
                        message: nextRoom.GetDisplayText(includeDescription: true, player),
                        shouldContinue: true);
            }

            TargetInfo parsedTarget = ParseTarget(argument);

            Npc? npcTarget = FindNpc(player.CurrentRoom.Npcs, parsedTarget);

            if (npcTarget is not null)
            {
                return new CommandResult(
                    message: npcTarget.Description,
                    shouldContinue: true);
            }

            Item? itemTarget = FindItem(player.CurrentRoom.Items, parsedTarget);

            if (itemTarget is not null)
            {
                return new CommandResult(
                    message: itemTarget.Description,
                    shouldContinue: true);
            }

            return new CommandResult(
                message: $"You can't seem to find {argument}.",
                shouldContinue: true
                );
        }

        private CommandResult GlanceCommand(
            string argument,
            Player player)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return new CommandResult(
                    message: player.CurrentRoom.GetDisplayText(includeDescription: false, player),
                    shouldContinue: true);
            }

            if (TryGetDirection(
                argument,
                out Direction direction))
            {
                Room? nextRoom = player.CurrentRoom.GetExit(direction);

                if (nextRoom is null)
                {
                    return new CommandResult(
                        message: "You see nothing of interest in that direction.",
                        shouldContinue: true);
                }

                return new CommandResult(
                        message: nextRoom.GetDisplayText(includeDescription: false, player),
                        shouldContinue: true);
            }

            return new CommandResult(
                message: "You glance purposefully at absolutely nothing in particular.",
                shouldContinue: true
                );
        }

        // social commands
        private CommandResult SayCommand(string argument, Player player)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                return new CommandResult(
                message: "Say what? ",
                shouldContinue: true);
            }
            return new CommandResult(
                message: $"You say: {argument}",
                shouldContinue: true,
                roomMessage: $"{player.Name} says: {argument}");
        }

        // info commands
        private CommandResult ScoreCommand(string argument, Player player)
        {
            string scoreMessage = "";

            if (player.IsImmortal)
            {
                scoreMessage = $"<Immortal>\n";
            }

            if (argument == "stats")
            {
                scoreMessage += $"Here are your current stats.\n" +
                    $"Str: {player.Stats.Strength}, " +
                    $"Dex: {player.Stats.Dexterity}, " +
                    $"Con: {player.Stats.Constitution}, " +
                    $"Int: {player.Stats.Intelligence}, " +
                    $"Wis: {player.Stats.Wisdom}";

                return new CommandResult(
                    message: scoreMessage,
                    shouldContinue: true
                );
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                scoreMessage += $"Race: {player.Race.Name}, HP: {player.HealthPoints}"; // race here is just for testing, move to "sheet" later

                return new CommandResult(
                        message: scoreMessage,
                        shouldContinue: true
                    );
            }

            scoreMessage = $"'{argument}' is not a valid score option. Try 'score' or 'score stats'.";

            return new CommandResult(
                        message: scoreMessage,
                        shouldContinue: true
                    );
        }

        // system commands
        private CommandResult QuitCommand(Player player)
        {
            return new CommandResult(
                message: "Goodbye!",
                shouldContinue: false);
        }

        private CommandResult SaveCommand()
        {
            return new CommandResult(
                message: "Saving... [████████████████████] 100% complete! Just kidding. I haven't coded that yet.",
                shouldContinue: true);
        }

        // immortal only commands
        private CommandResult AnalyzeCommand(Player player, string argument)
        {
            if (!player.IsImmortal)
            {
                return new CommandResult(
                message: "Unknown command.",
                shouldContinue: true);
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                return new CommandResult(
                    message: "<Immortal> Analyze what?",
                    shouldContinue: true);
            }

            TargetInfo parsedTarget = ParseTarget(argument);

            Npc? npcTarget = FindNpc(player.CurrentRoom.Npcs, parsedTarget);

            if (npcTarget is not null)
            {
                string npcKeywords = "";
                foreach (string keyword in npcTarget.Keywords)
                {
                    npcKeywords += $"{keyword}\n";
                }

                string npcInventoryMessage = "";
                if (npcTarget.Inventory.Count == 0)
                {
                    npcInventoryMessage = "Empty.\n";
                }
                else
                {
                    foreach (Item item in npcTarget.Inventory)
                    {
                        npcInventoryMessage += $"{item.Name}\n";
                    }
                }
                
                string analyzeNpcMessage = $"Name: {npcTarget.Name}\n" +
                    $"\nDescription: \n{npcTarget.Description}\n" +
                    $"\nInventory: \n{npcInventoryMessage}" +
                    $"\nHealth Points: {npcTarget.HealthPoints}/{npcTarget.MaxHealthPoints}\n" +
                    $"\nKeywords: \n{npcKeywords}\n" +
                    $"Stats: " +
                    $"\nStr: {npcTarget.Stats.Strength}" +
                    $"\nDex: {npcTarget.Stats.Dexterity}" +
                    $"\nCon: {npcTarget.Stats.Constitution}" +
                    $"\nInt: {npcTarget.Stats.Intelligence}" +
                    $"\nWis: {npcTarget.Stats.Wisdom}";

                return new CommandResult(
                    message: analyzeNpcMessage,
                    shouldContinue: true);
            }

            Item? itemTarget = FindItem(player.CurrentRoom.Items, parsedTarget);

            if (itemTarget is not null)
            {
                string itemKeywords = "";
                foreach (string keyword in itemTarget.Keywords)
                {
                    itemKeywords += $"{keyword}\n";
                }

                string analyzeItemMessage = $"Name: {itemTarget.Name}\n" +
                    $"Description: {itemTarget.Description}\n" +
                    $"Weight: {itemTarget.Weight}\n" +
                    $"Keywords: \n{itemKeywords}";

                return new CommandResult(
                    message: analyzeItemMessage,
                    shouldContinue: true);
            }

            return new CommandResult(
                 message: $"<Immortal> You can't seem to find any {parsedTarget.Name} to analyze.",
                 shouldContinue: true);
        }

        private Item? FindItem(List<Item> itemCollection, TargetInfo target)
        {
            int currentItemMatch = 0;

            foreach (Item item in itemCollection)
            {
                if (!item.Matches(target.Name))
                {
                    continue;
                }

                currentItemMatch++;

                if (currentItemMatch == target.Index)
                {
                    return item;
                }
            }
            return null;
        }

        private Npc? FindNpc(List<Npc> npcCollection, TargetInfo target)
        {
            int currentNpcMatch = 0;

            foreach (Npc npc in npcCollection)
            {
                if (!npc.Matches(target.Name))
                {
                    continue;
                }

                currentNpcMatch++;

                if (currentNpcMatch == target.Index)
                {
                    return npc;
                }
            }
            return null;
        }
    }
}
