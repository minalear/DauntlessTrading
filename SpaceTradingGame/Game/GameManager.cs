using System;
using System.Collections.Generic;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class GameManager
    {
        private List<StarSystem> systems;

        public GameManager()
        {
            systems = new List<StarSystem>();

            //Sol System
            StarSystem solSystem = new StarSystem("Sol") { Coordinates = OpenTK.Vector2.Zero };
            solSystem.StarColor = OpenTK.Graphics.Color4.Yellow;

            //Terra
            Planetoid terra = new Planetoid(solSystem, "Terra");
            Planetoid luna = new Planetoid(solSystem, "Luna", terra);

            //Mars
            Planetoid mars = new Planetoid(solSystem, "Mars");
            Planetoid phobos = new Planetoid(solSystem, "Phobos", mars);
            Planetoid deimos = new Planetoid(solSystem, "Deimos", mars);

            //Jupiter
            Planetoid jupiter = new Planetoid(solSystem, "Jupiter");
            Planetoid io = new Planetoid(solSystem, "Io", jupiter);
            Planetoid europa = new Planetoid(solSystem, "Europa", jupiter);
            Planetoid ganymede = new Planetoid(solSystem, "Ganymede", jupiter);
            Planetoid callisto = new Planetoid(solSystem, "Callisto", jupiter);

            solSystem.Planetoids.Add(terra);
            solSystem.Planetoids.Add(mars);
            solSystem.Planetoids.Add(jupiter);

            systems.Add(solSystem);
            generateRandomSystems();
        }
        private void generateRandomSystems()
        {
            int numSystems = RNG.Next(250, 500);
            for (int i = 0; i < numSystems; i++)
            {
                StarSystem system = new Game.StarSystem("Test");
                system.Coordinates = new OpenTK.Vector2(
                    RNG.NextFloat(-10000.0f, 10000.0f),
                    RNG.NextFloat(-10000.0f, 10000.0f)
                );

                systems.Add(system);
            }
        }

        public List<StarSystem> Systems { get { return systems; } }
    }
}
