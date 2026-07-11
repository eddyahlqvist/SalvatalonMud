using System.Collections.Generic;

namespace SalvatalonMud;

internal class WorldBuilder
{
    public static World Build()
    {       
        Room tyrikaSquare = new(
            "Tyrika Square",
            "You are standing in the bustling town square of Tyrika."
        );

        Room northRoad = new(
            "North Road",
            "A road leading north from Tyrika."
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