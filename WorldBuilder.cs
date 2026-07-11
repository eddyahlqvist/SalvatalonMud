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

        List<Room> rooms = new()
        {
            tyrikaSquare
        };

        return new World(
            "Salvatalon",
            tyrikaSquare,
            rooms);
    }
}