using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class GameManager
    {
        private List<System> systems;

        public GameManager()
        {
            systems = new List<System>();

            //Sol System
            System solSystem = new System("Sol");

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
        }

        public List<System> Systems { get { return systems; } }
    }
}
