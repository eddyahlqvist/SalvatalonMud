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

        // create NPCs
        Npc pigeon = new(
            name: "suspicious pigeon",
            keywords: new[]
            {
                "pigeon",
                "bird",
                "animal",
                "monster"
            },
            description: "A suspicious pigeon watches you with open contempt.",
            currentRoom: tyrikaSquare,
            healthPoints: 2);

        // add NPCs to specific rooms
        AddPigeons(
            amount: 3, 
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
}