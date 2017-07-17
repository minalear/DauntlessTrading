using System;
using System.Collections.Generic;
using OpenTK;
using SpaceTradingGame.Engine;

namespace SpaceTradingGame.Game
{
    public class Pathfinder
    {
        public GameManager GameManager { get; private set; }

        public Pathfinder(GameManager manager)
        {
            GameManager = manager;
        }

        public List<StarSystem> FindPath(StarSystem start, StarSystem finish, Ship ship)
        {
            List<StarSystem> path = new List<StarSystem>();
            path.Add(start);

            //If our destination is within jump distance, return the path with just start/finish
            if (start.Coordinates.Distance(finish.Coordinates) <= ship.JumpRadius)
            {
                path.Add(finish);
                return path;
            }

            bool pathFinished = false;
            while (!pathFinished)
            {
                List<StarSystem> weightedList = new List<StarSystem>();

                foreach (StarSystem testSystem in GameManager.Systems)
                {
                    StarSystem currentSystem = path[path.Count - 1];
                    if (currentSystem.ID == testSystem.ID) continue;
                    if (pathContains(path, testSystem)) continue;

                    double value = 0;

                    double distToFinish = currentSystem.Coordinates.Distance(finish.Coordinates);
                    double testToFinish = testSystem.Coordinates.Distance(finish.Coordinates);
                    double distToTest = currentSystem.Coordinates.Distance(testSystem.Coordinates);

                    //The test system is the destination
                    if (testSystem.ID == finish.ID)
                    {
                        if (currentSystem.Coordinates.Distance(testSystem.Coordinates) < ship.JumpRadius)
                        {
                            path.Add(testSystem);
                            return path;
                        }
                        value += 100.0;
                    }
                    else
                    {
                        //If the system gets us closer to our destination
                        Vector2 firstDirection = finish.Coordinates - currentSystem.Coordinates;
                        Vector2 secondDirection = testSystem.Coordinates - currentSystem.Coordinates;

                        firstDirection.Normalize();
                        secondDirection.Normalize();

                        float dot = Vector2.Dot(firstDirection, secondDirection);
                        value += (int)(dot * 100.0);
                    }
                    
                    value += (distToTest <= ship.JumpRadius) ? 25.0 : -40.0;
                    value += (distToTest < distToFinish) ? 20.0 : 0.0;
                    
                    //Modify value based on distance
                    double dist = currentSystem.Coordinates.Distance(testSystem.Coordinates);
                    value += 1 - dist / 30.0;
                    testSystem.WeightedValue = value;
                    
                    weightedList.Add(testSystem);
                }

                weightedList.Sort();
                StarSystem chosenSystem = weightedList[weightedList.Count - 1];
                path.Add(chosenSystem);

                if (chosenSystem.ID == finish.ID)
                    return path;
            }

            throw new NotImplementedException();
        }

        private bool pathContains(List<StarSystem> list, StarSystem testSystem)
        {
            foreach (StarSystem system in list)
            {
                if (system.ID == testSystem.ID)
                    return true;
            }

            return false;
        }
    }
}
