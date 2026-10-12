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

        // connect rooms
        tyrikaSquare.North = northRoad;
        northRoad.South = tyrikaSquare;

        List<Room> rooms = new()
        {
            tyrikaSquare,
            northRoad
        };

        // create and add items to specific rooms
        for (int i = 0; i < 3; i++)
        {
            Item sword = new(
            name: "Rusty Sword",
            displayNamePlural: "Rusty Swords",
            description: "A sword that has seen better days.",
            weight: 30,
            keywords: new string[] { "sword", "weapon" });

            if (i < 2)
            {
                northRoad.Items.Add(sword);
            }
            else
            {
                tyrikaSquare.Items.Add(sword);
            }
        }
        
        // create and add NPCs to specific rooms
        AddPigeons(
            amount: 3,
            room: tyrikaSquare
            );

        AddTownGuards(
            amount: 1,
            room: tyrikaSquare
            );


        return new World(
            name: "Salvatalon",
            startingRoom: tyrikaSquare,
            rooms: rooms);
    }
    private static void AddPigeons(int amount, Room room)
    {        
        for (int i = 0; i < amount; i++)
        {
            Stats pigeonStats = new Stats(
                startingStrength: 3,
                startingDexterity: 4,
                startingConstitution: 3,
                startingIntelligence: 6,
                startingWisdom: 16
            );

            Npc pigeon = new(
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
                currentRoom: room,                
                stats: pigeonStats);

            room.Npcs.Add(pigeon);
        }
    }

    private static void AddTownGuards(int amount, Room room)
    {        
        for (int i = 0; i < amount; i++)
        {
            Stats townGuardStats = new Stats(
                startingStrength: 14,
                startingDexterity: 10,
                startingConstitution: 10,
                startingIntelligence: 10,
                startingWisdom: 8
            );

            Npc townGuard = new(
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
                    "He clutches a splintered wooden truncheon, radiating the distinct, resigned " +
                    "energy of a man whose primary goal is surviving his shift without seeing " +
                    "anything that requires paperwork.",
                currentRoom: room,
                stats: townGuardStats);

            for (int j = 0; j < 2; j++)
            {
                Item sword = new(
                name: "Rusty Sword",
                displayNamePlural: "Rusty Swords",
                description: "A sword that has seen better days.",
                weight: 30,
                keywords: new string[] { "sword", "weapon" });

                townGuard.Inventory.Add(sword);
            }
                                
            room.Npcs.Add(townGuard);            
        }
    }
}