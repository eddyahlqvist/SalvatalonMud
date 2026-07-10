// Salvatalon MUD project started 2026-07-10 by Edius Ahlqvistus

using System.Threading.Tasks;

namespace SalvatalonMud;

internal class Program
{
    private static async Task Main()
    {
        World world = WorldBuilder.Build();

        MudServer server = new(world);

        await server.RunAsync();
    }
}