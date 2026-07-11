using System.Collections.Generic;

namespace SalvatalonMud;

internal class WorldBuilder
{
    public static World Build()
    {       
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

        return new World(
            "Salvatalon",
            tyrikaSquare,
            rooms);
    }
}