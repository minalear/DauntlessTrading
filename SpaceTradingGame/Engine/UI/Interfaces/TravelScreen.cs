using System;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Engine.UI.Controls.Custom;
using SpaceTradingGame.Game;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class TravelScreen : Interface
    {
        public TravelScreen(InterfaceManager manager)
            : base(manager)
        {
            screenTitle = new Title(null, "Star Map", GraphicConsole.BufferWidth / 2 - 11, 1, Title.TextAlignModes.Center);
            starMap = new StarMap();
            starMap.Position = new System.Drawing.Point(1, 2);
            starMap.Size = new System.Drawing.Point(74, 33);

            systemTitle = new Title(null, "Current System", 87, 1, Title.TextAlignModes.Center);
            systemDescriptionBox = new TextBox(null, 76, 3, 23, 15);
            systemDescriptionBox.FillColor = new Color4(0.15f, 0.15f, 0.15f, 1f);

            up = new Button(null, "▲", 87, 30);
            down = new Button(null, "▼", 87, 34);
            left = new Button(null, "◄", 84, 32);
            right = new Button(null, "►", 90, 32);

            systemButton = new Button(null, "System", 76, 27);
            travelButton = new Button(null, "Travel", 76, 30);
            cargoButton = new Button(null, "Cargo", GraphicConsole.BufferWidth - 7, 27);
            stockButton = new Button(null, "Stock", GraphicConsole.BufferWidth - 7, 30);

            travelManager = new TravelManager(GameManager, starMap);
            screenTitle.Text = GameManager.GalacticDate.ToShortDateString();

            clock = new Clock(null, 10, 5);
            clock.Position = new System.Drawing.Point(63, 1);

            playButton = new Button(null, "►", 74, 1, 1, 1);
            fasterButton = new Button(null, "+", 61, 1, 1, 1);
            slowerButton = new Button(null, "-", 62, 1, 1, 1);

            //UI Events
            up.Click += (sender, e) =>
            {
                starMap.PanMap(0f, 100f);
                InterfaceManager.DrawStep();
            };
            down.Click += (sender, e) =>
            {
                starMap.PanMap(0f, -100f);
                InterfaceManager.DrawStep();
            };
            left.Click += (sender, e) =>
            {
                starMap.PanMap(100f, 0f);
                InterfaceManager.DrawStep();
            };
            right.Click += (sender, e) =>
            {
                starMap.PanMap(-100f, 0f);
                InterfaceManager.DrawStep();
            };

            travelButton.Click += (sender, e) =>
            {
                if (starMap.HasSystemSelected && !travelManager.IsTraveling)
                {
                    StartTraveling();
                }
            };
            systemButton.Click += (sender, e) =>
            {
                if (!travelManager.IsTraveling)
                    InterfaceManager.ChangeInterface("System");
            };
            cargoButton.Click += (sender, e) =>
            {
                if (!travelManager.IsTraveling)
                    InterfaceManager.ChangeInterface("Ship");
            };
            stockButton.Click += (sender, e) =>
            {
                if (!travelManager.IsTraveling)
                    InterfaceManager.ChangeInterface("Stock");
            };

            starMap.Selected += (sender, e) =>
            {
                starMap.SetPath(GameManager.Pathfinder.FindPath(GameManager.CurrentSystem, e, GameManager.PlayerShip));
                updateScreenInformation();
                InterfaceManager.DrawStep();
            };

            clock.TimerLapse += (sender, e) =>
            {
                //GameManager.SimulateGame(1.0);
                screenTitle.Text = GameManager.GalacticDate.ToShortDateString();
            };
            clock.TimerTick += (sender, e) =>
            {
                GameManager.SimulateGame(0.1);
                if (travelManager.IsTraveling)
                {
                    travelManager.SimulateTravel(0.1);
                }
            };

            playButton.Click += (sender, e) =>
            {
                playButton.Text = (playButton.Text == "■") ? "►" : "■";
                clock.Toggle();

                InterfaceManager.DrawStep();
            };
            fasterButton.Click += (sender, e) =>
            {
                clock.TickRate -= 0.5;
            };
            slowerButton.Click += (sender, e) =>
            {
                clock.TickRate += 0.5;
            };

            #region ControlRegister
            RegisterControl(screenTitle);
            RegisterControl(systemTitle);
            RegisterControl(systemDescriptionBox);
            RegisterControl(starMap);
            RegisterControl(up);
            RegisterControl(down);
            RegisterControl(left);
            RegisterControl(right);
            RegisterControl(playButton);
            RegisterControl(fasterButton);
            RegisterControl(slowerButton);
            RegisterControl(travelButton);
            RegisterControl(systemButton);
            RegisterControl(cargoButton);
            RegisterControl(stockButton);
            RegisterControl(clock);
            #endregion
        }

        public override void OnEnable()
        {
            starMap.SetSystemList(GameManager.Systems);
            starMap.SetCurrentSystem(GameManager.CurrentSystem);

            updateScreenInformation();

            base.OnEnable();
        }
        public override void Game_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Up)
                starMap.PanMap(0f, 100f);
            else if (e.Key == Key.Down)
                starMap.PanMap(0f, -100f);
            else if (e.Key == Key.Left)
                starMap.PanMap(100f, 0f);
            else if (e.Key == Key.Right)
                starMap.PanMap(-100f, 0f);

            InterfaceManager.DrawStep();

            base.Game_KeyUp(sender, e);
        }

        public void StartTraveling()
        {
            travelManager.SetTravelPath(starMap.GetTravelPath());
        }

        private void updateScreenInformation()
        {
            StarSystem infoSystem = (starMap.HasSystemSelected) ? starMap.SelectedSystem : GameManager.CurrentSystem;
            systemTitle.Text = infoSystem.Name;

            StringBuilder systemDescription = new StringBuilder();
            if (infoSystem.Planetoids.Count == 0)
                systemDescription.AppendLine("No planets.");
            else
            {
                foreach (Planetoid planet in infoSystem.Planetoids)
                {
                    systemDescription.AppendLine(planet.Name);
                }
            }

            systemDescription.Append("-\n");

            if (infoSystem.HasMarket)
                systemDescription.AppendFormat("System market owned by {0}.", infoSystem.SystemMarket.Owner.Name);
            else
                systemDescription.Append("No system market.");

            systemDescriptionBox.Text = systemDescription.ToString(); ;
        }

        private TravelManager travelManager;

        private Title screenTitle, systemTitle;
        private TextBox systemDescriptionBox;
        private Button up, down, left, right;
        private Button playButton, fasterButton, slowerButton;
        private Button travelButton, systemButton, cargoButton, stockButton;
        private StarMap starMap;
        private Clock clock;
    }

    public class TravelManager
    {
        private GameManager gameManager;

        private StarMap starMap;
        private StarSystem[] travelPath;
        
        private int currentNode, nextNode;
        private Vector2 travelVector;

        private float timeToNextNode;
        private double timer;

        public float MoveSpeed = 450.0f;
        public bool IsTraveling = false;

        public TravelManager(GameManager manager, StarMap map)
        {
            gameManager = manager;
            starMap = map;
        }

        public void SetTravelPath(StarSystem[] systemPath)
        {
            travelPath = systemPath;
            gameManager.PlayerShip.WorldPosition = systemPath[0].Coordinates;

            currentNode = 0;
            nextNode = 1;

            updateVectors();

            starMap.DrawPlayerPosition = true;
            IsTraveling = true;
        }

        public void SimulateTravel(double days)
        {
            UpdatePlayerPosition((float)days);

            timer += days;
            if (timer >= timeToNextNode)
            {
                //Travel finished
                if (currentNode == nextNode)
                {
                    starMap.SetCurrentSystem(travelPath[travelPath.Length - 1]);
                    starMap.DrawPlayerPosition = false;
                    IsTraveling = false;

                    starMap.Interface.InterfaceManager.DrawStep();
                }
                else
                {
                    timer = 0.0;
                    currentNode++;
                    nextNode = (nextNode + 1 != travelPath.Length) ? nextNode + 1 : nextNode;
                    gameManager.PlayerShip.WorldPosition = travelPath[currentNode].Coordinates;

                    updateVectors();
                }
            }
        }
        public void UpdatePlayerPosition(float elapsed)
        {
            gameManager.PlayerShip.WorldPosition += Vector2.Multiply(travelVector, MoveSpeed * elapsed);
            starMap.SetPlayerPosition(gameManager.PlayerShip.WorldPosition);
        }

        private void updateVectors()
        {
            travelVector = travelPath[nextNode].Coordinates - travelPath[currentNode].Coordinates;
            if (travelVector.Length > 0) travelVector.Normalize();

            float distance = travelPath[currentNode].Coordinates.Distance(travelPath[nextNode].Coordinates);
            timeToNextNode = distance / MoveSpeed;
        }
    }
}
