using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class Planetoid : Engine.UI.Controls.ListItem
    {
        public StarSystem System { get; private set; }
        public Planetoid Parent { get; private set; }
        public string Name { get; private set; }
        public List<Planetoid> Moons { get; private set; }
        public List<Station> Stations { get; private set; }
        public List<Factory> Factories { get; private set; }

        public List<MaterialDeposit> MaterialDeposits { get; set; }

        public Planetoid(StarSystem system, string name)
        {
            System = system;
            Name = name;

            Moons = new List<Planetoid>();
            Stations = new List<Station>();
            Factories = new List<Factory>();
            MaterialDeposits = new List<MaterialDeposit>();

            ListText = Name;
        }
        public Planetoid(StarSystem system, string name, Planetoid parent)
        {
            System = system;
            Name = name;
            Parent = parent;

            Moons = new List<Planetoid>();
            Stations = new List<Station>();
            Factories = new List<Factory>();
            MaterialDeposits = new List<MaterialDeposit>();

            Parent.Moons.Add(this);

            ListText = Name;
        }

        public void UpdatePlanetoid()
        {
            foreach (Station station in Stations)
            {
                station.UpdateSpaceStation();
            }
            foreach (Factory factory in Factories)
            {
                factory.UpdateFactory();
            }
            foreach (Planetoid moon in Moons)
            {
                moon.UpdatePlanetoid();
            }
        }
        public void AddMaterialDeposit(MaterialDeposit deposit)
        {
            for (int i = 0; i < MaterialDeposits.Count; i++)
            {
                //Combine similar deposits
                if (MaterialDeposits[i].Material.Equals(deposit.Material))
                {
                    MaterialDeposits[i].Density *= deposit.Density;

                    return;
                }
            }

            //Add new deposit
            MaterialDeposits.Add(deposit);
        }
        public void BuildStation(Faction owner)
        {
            Station station = new Station(this, owner, 1);
            Stations.Add(station);
        }
        public void BuildFactory(Faction owner, Product product)
        {
            Factory factory = new Factory(this, owner, product, 1);
            Factories.Add(factory);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class MaterialDeposit
    {
        public double Density;
        public Item Material;

        public override string ToString()
        {
            return string.Format("{0} - {1}", Material.Name, Density);
        }
    }
}
