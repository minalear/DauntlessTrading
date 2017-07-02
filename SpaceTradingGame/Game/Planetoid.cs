using System;
using System.Collections.Generic;

namespace SpaceTradingGame.Game
{
    public class Planetoid : Engine.UI.Controls.ListItem
    {
        public StarSystem System { get; set; }
        public Planetoid Parent { get; set; }
        public string Name { get; set; }
        public List<Planetoid> Moons { get; set; }
        public List<Station> Stations { get; set; }
        public Item PrimaryExport { get; set; }

        public List<MaterialDeposit> MaterialDeposits { get; set; }

        public Planetoid(StarSystem system, string name)
        {
            System = system;
            Name = name;

            Moons = new List<Planetoid>();
            Stations = new List<Station>();
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
            MaterialDeposits = new List<MaterialDeposit>();

            Parent.Moons.Add(this);

            ListText = Name;
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
