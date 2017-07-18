using System;
using System.Collections.Generic;
using OpenTK;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class Pilot
    {
        public GameManager GameManager { get; private set; }

        public string Name { get; private set; }
        public Faction Faction { get; private set; }
        public Ship Ship { get; private set; }
        public bool IsPlayer { get; private set; }
        public bool IsTraveling { get; private set; }

        public Pilot(GameManager manager, string name, Faction faction, Ship ship, bool isPlayer = false)
        {
            GameManager = manager;

            Name = name;
            Faction = faction;
            Ship = ship;
            IsPlayer = isPlayer;
            IsTraveling = false;
        }

        public void Update(double days)
        {
            if (!IsTraveling) return;

            while (days >= 1.0)
            {
                updateMovement(1.0);
                days -= 1.0;
            }

            updateMovement(days);
        }
        public void MoveTo(StarSystem system)
        {
            MoveAlongPath(GameManager.Pathfinder.FindPath(Ship.CurrentSystem, system, Ship));
        }
        public void MoveAlongPath(List<StarSystem> path)
        {
            flightPath = path;
            IsTraveling = true;
            timer = 0.0;

            currentNode = 0;
            nextNode = 1;

            updateVectors();
        }
        public StarSystem[] GetTravelPath()
        {
            return flightPath.ToArray();
        }

        private void updateVectors()
        {
            travelVector = flightPath[nextNode].Coordinates - flightPath[currentNode].Coordinates;
            if (travelVector.Length > 0) travelVector.Normalize();

            float dist = flightPath[currentNode].Coordinates.Distance(flightPath[nextNode].Coordinates);
            timeToNextNode = dist / Ship.MoveSpeed;
        }
        private void updateMovement(double mod)
        {
            Ship.WorldPosition += Vector2.Multiply(travelVector, Ship.MoveSpeed * (float)mod);

            timer += mod;
            if (timer >= timeToNextNode)
            {
                Ship.SetCurrentSystem(flightPath[nextNode]);

                //Travel finished
                if (currentNode == nextNode)
                {
                    IsTraveling = false;
                    Finished?.Invoke(this, new PilotFinishedTravelingEventArgs(Ship, this, Ship.CurrentSystem));
                }
                else
                {
                    timer = 0.0;
                    currentNode++;
                    nextNode = (nextNode + 1 != flightPath.Count) ? nextNode + 1 : nextNode;

                    updateVectors();
                }
            }
        }

        public delegate void FinishedTravelingEvent(object sender, PilotFinishedTravelingEventArgs e);
        public event FinishedTravelingEvent Finished;

        private List<StarSystem> flightPath;
        private int currentNode = 0, nextNode = 1;
        private Vector2 travelVector;
        private float timeToNextNode;
        private double timer = 0.0;
    }

    public class PilotFinishedTravelingEventArgs : EventArgs
    {
        public Ship Ship { get; set; }
        public Pilot Pilot { get; set; }
        public StarSystem Destination { get; set; }

        public PilotFinishedTravelingEventArgs(Ship ship, Pilot pilot, StarSystem system)
        {
            Ship = ship;
            Pilot = pilot;
            Destination = system;
        }
    }
}
