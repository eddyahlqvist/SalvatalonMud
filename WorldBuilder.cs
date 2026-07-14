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

        // connect rooms
        tyrikaSquare.North = northRoad;
        northRoad.South = tyrikaSquare;

        List<Room> rooms = new()
        {
            tyrikaSquare,
            northRoad
        };             

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
            "Salvatalon",
            tyrikaSquare,
            rooms);
    }
    private static void AddPigeons(int amount, Room room)
    {
        for (int i = 0; i < amount; i++)
        {
            Npc pigeon = new(
                name: "suspicious pigeon",
                displayNamePlural: "Suspicious Pigeons",
                keywords: new[]
                {
                "pigeon",
                "bird",
                "animal",
                "monster"
                },
                description:
                    "A suspicious pigeon watches you with open contempt.",
                currentRoom: room,
                healthPoints: 2);

            room.Npcs.Add(pigeon);
        }
    }

    private static void AddTownGuards(int amount, Room room)
    {
        for (int i = 0; i < amount; i++)
        {
            Npc townGuard = new(
                name: "town guard",
                displayNamePlural: "Town Guards",
                keywords: new[]
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
                healthPoints: 10);

            room.Npcs.Add(townGuard);
        }
    }
}