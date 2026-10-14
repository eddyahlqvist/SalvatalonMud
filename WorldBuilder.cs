using System;
using System.Collections.Generic;

namespace SalvatalonMud;

internal class WorldBuilder
{
    public static World Build()
    {
        // create rooms
        Room tyrikaSquare = new(
            name: "Tyrika Square",
            description: "You are standing in the bustling town square of Tyrika.",
            x: 0, y: 0, z: 0
        );

        Room northRoad = new(
            name: "North Road",
            description: "A road leading north from Tyrika.",
            x: 0, y: 1, z: 0
        );

        Room eastRoad = new(
            name: "East Road",
            description: "A road leading east from Tyrika.",
            x: 1, y: 0, z: 0
        );

        Room tyrikaBank = new(
            name: "Bank of Tyrika",
            description: "A sturdy building where you can conduct banking business.",
            x: 1, y: -1, z: 0
        );

        // connect rooms
        tyrikaSquare.North = northRoad;       
        northRoad.South = tyrikaSquare;
        tyrikaSquare.East = eastRoad;
        eastRoad.West = tyrikaSquare;
        eastRoad.South = tyrikaBank;
        tyrikaBank.North = eastRoad;

        List<Room> rooms = new()
        {
            tyrikaSquare,
            northRoad,
            eastRoad,
            tyrikaBank
        };

        // create and add items to specific rooms        
        AddItemToRoom(
            amount: 1,
            createItem: CreateRustySword,
            tyrikaSquare);

        AddItemToRoom(
            amount: 2,
            createItem: CreateRustySword,
            northRoad);

        AddItemToRoom(
            amount: 1,
            createItem: CreatePigeonFeather,
            tyrikaSquare);

        // create and add npcs to specific rooms
        AddNpcToRoom(
            amount: 3,
            createNpc: CreateSuspiciousPigeon,
            room: tyrikaSquare);

        AddNpcToRoom(
            amount: 1,
            createNpc: CreateTownGuard,
            room: tyrikaSquare);

        AddNpcToRoom(
            amount: 1,
            createNpc: CreateGripshark,
            room: tyrikaBank);

        return new World(
            name: "Salvatalon",
            startingRoom: tyrikaSquare,
            rooms: rooms);
    }

    private static void AddItemToRoom(int amount, Func<Item> createItem, Room room)
    {
        for (int i = 0; i < amount; i++)
        {
            Item item = createItem();

            room.Items.Add(item);
        }
    }

    private static void AddNpcToRoom(int amount, Func<Npc> createNpc, Room room)
    {
        for (int i = 0; i < amount; i++)
        {
            Npc npc = createNpc();

            npc.CurrentRoom = room;
            room.Npcs.Add(npc);
        }
    }

    // weapons
    private static Item CreateRustySword()
    {
        Item sword = new(
            name: "Rusty Sword",
            displayNamePlural: "Rusty Swords",
            description: "A sword that has seen better days.",
            weight: 30,
            keywords: new string[] { "sword", "weapon" });

        return sword;
    }

    // components, ingredients or junk items, not yet decided
    private static Item CreatePigeonFeather()
    {
        Item feather = new(
            name: "Pigeon Feather",
            displayNamePlural: "Pigeon Feathers",
            description: "A gray pigeon feather. It looks innocent enough, but it was " +
            "attached to a bird that was clearly wearing a fake mustache. It " +
            "feels uncomfortably warm.",
            weight: 2,
            keywords: new string[] { "feather" });

        return feather;
    }

    private static Npc CreateSuspiciousPigeon()
    {
        Stats startingStats = new Stats(
            startingStrength: 3,
            startingDexterity: 4,
            startingConstitution: 3,
            startingIntelligence: 6,
            startingWisdom: 16
        );

        Npc npc = new(
            name: "suspicious pigeon",
            displayNamePlural: "Suspicious Pigeons",
            keywords: new string[]
            {
                "pigeon",
                "bird",
                "animal",
                "monster"
            },
            description:
                "A suspicious pigeon watches you with open contempt.",
            stats: startingStats);

        npc.Inventory.Add(CreatePigeonFeather());

        return npc;
    }

    private static Npc CreateTownGuard()
    {
        Stats startingStats = new Stats(
                startingStrength: 14,
                startingDexterity: 10,
                startingConstitution: 10,
                startingIntelligence: 10,
                startingWisdom: 8
            );

        Npc npc = new(
            name: "town guard",
            displayNamePlural: "Town Guards",
            keywords: new string[]
            {
                "guard",
                "monster"
            },
            description:
                "This world-weary city guard slouches in a rusty breastplate, " +
                "wearing an expression that suggests he would rather be in a pub. " +
                "He clutches a splintered wooden truncheon, radiating the distinct, resigned " +  // fix description later about weapon
                "energy of a man whose primary goal is surviving his shift without seeing " +
                "anything that requires paperwork.",
            stats: startingStats);

        npc.Inventory.Add(CreateRustySword());

        npc.CoinPurse.Brass = 4;
        npc.CoinPurse.Copper = 2;
        npc.CoinPurse.Silver = 3;
        npc.CoinPurse.Gold = 1;

        return npc;
    }

    // unique NPCs
    private static Npc CreateGripshark()
    {
        Stats startingStats = new Stats(
                startingStrength: 8,
                startingDexterity: 10,
                startingConstitution: 12,
                startingIntelligence: 18,
                startingWisdom: 8
            );

        Npc npc = new(
            name: "Gripshark",
            displayNamePlural: "Goblin Bankers", //hmm... should never be Plural though...
            keywords: new string[]
            {
                "gripshark",
                "goblin",
                "banker",
                "monster"
            },
            description:
                "You see a goblin who traded the traditional fear of daylight for an even stronger " +
                "passion for double-entry bookkeeping. He wears a tailored suit clearly meant for a " +
                "much larger dwarf, giving him the distinct silhouette of an angry, crumpled umbrella. " +
                "Gripshark wears a pair of magnifying spectacles so thick they turn his eyes into two " +
                "enormous, predatory glass marbles. Behind them lies an intellect sharp enough to calculate " +
                "compound interest on half a copper piece. A fountain pen dances between his clawed fingers " +
                "like a dagger—and frankly, in his hands, it probably is.",
            stats: startingStats);

        //npc.Inventory.Add(CreateFountainPen()); // maybe add this later for "protection"

        npc.CoinPurse.Gold = 26;

        return npc;
    }
}