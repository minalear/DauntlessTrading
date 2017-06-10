﻿using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
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
            screenTitle = new Title(null, "Star Map", GraphicConsole.BufferWidth / 2, 1, Title.TextAlignModes.Center);
            starMap = new StarMap();
            starMap.Position = new System.Drawing.Point(1, 2);
            starMap.Size = new System.Drawing.Point(74, 33);

            systemTitle = new Title(null, "Current System", 87, 2, Title.TextAlignModes.Center);
            systemDesc = new TextBox(null, 76, 4, 23, 10);

            up = new Button(null, "▲", 87, 30);
            down = new Button(null, "▼", 87, 34);
            left = new Button(null, "◄", 84, 32);
            right = new Button(null, "►", 90, 32);

            travelButton = new Button(null, "Travel", 76, 30);
            detailsButton = new Button(null, "Details", 76, 27);

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
                if (starMap.HasSystemSelected)
                {
                    StartTraveling();
                }
            };
            detailsButton.Click += (sender, e) =>
            {
                InterfaceManager.ChangeInterface("System");
            };

            starMap.Selected += (sender, e) =>
            {
                updateScreenInformation();
                InterfaceManager.DrawStep();
            };

            travelManager = new TravelManager(starMap);

            RegisterControl(screenTitle);
            RegisterControl(systemTitle);
            RegisterControl(systemDesc);
            RegisterControl(starMap);
            RegisterControl(up);
            RegisterControl(down);
            RegisterControl(left);
            RegisterControl(right);
            RegisterControl(travelButton);
            RegisterControl(detailsButton);
        }

        public override void OnEnable()
        {
            starMap.SetSystemList(GameManager.Systems);
            starMap.SetCurrentSystem(GameManager.CurrentSystem);

            updateScreenInformation();

            base.OnEnable();
        }

        public override void DrawStep()
        {
            base.DrawStep();
        }
        public override void UpdateFrame(GameTime gameTime)
        {
            if (travelManager.IsTraveling)
            {
                travelManager.UpdateFrame(gameTime);

                //Used to draw the interface ever 1 second while traveling
                drawTimer += gameTime.ElapsedTime.TotalSeconds;
                if (drawTimer >= 1)
                {
                    drawTimer = 0.0;
                    InterfaceManager.DrawStep();
                }
            }

            base.UpdateFrame(gameTime);
        }
        public void StartTraveling()
        {
            travelManager.SetTravelPath(starMap.GetTravelPath());
        }

        private void updateScreenInformation()
        {
            systemTitle.Text = GameManager.CurrentSystem;

            string desc = "";
            foreach (Game.Planetoid planet in GameManager.CurrentSystem.Planetoids)
            {
                desc += planet.Name + "\n";
            }

            systemDesc.Text = desc;
        }

        private TravelManager travelManager;
        private bool isTraveling = false;
        private double drawTimer = 0.0;

        private Title screenTitle, systemTitle;
        private TextBox systemDesc;
        private Button up, down, left, right;
        private Button travelButton, detailsButton;
        private StarMap starMap;
    }

    public class TravelManager
    {
        private StarMap starMap;
        private StarSystem[] travelPath;

        private Vector2 playerPosition;
        private int currentNode, nextNode;
        private Vector2 travelVector;

        private float timeToNextNode;
        private double timer;

        public float MoveSpeed = 100.0f;
        public bool IsTraveling = false;

        public TravelManager(StarMap map)
        {
            starMap = map;
        }

        public void SetTravelPath(StarSystem[] systemPath)
        {
            travelPath = systemPath;
            playerPosition = systemPath[0].Coordinates;

            currentNode = 0;
            nextNode = 1;

            updateVectors();

            starMap.DrawPlayerPosition = true;
            IsTraveling = true;
        }
        public void UpdateFrame(GameTime gameTime)
        {
            UpdatePlayerPosition((float)gameTime.ElapsedTime.TotalSeconds);

            timer += gameTime.ElapsedTime.TotalSeconds;
            if (timer >= timeToNextNode)
            {
                //Travel finished
                if (currentNode == nextNode)
                {
                    starMap.SetCurrentSystem(travelPath[travelPath.Length - 1]);
                    starMap.DrawPlayerPosition = false;
                    IsTraveling = false;
                }
                else
                {
                    timer = 0.0;
                    currentNode++;
                    nextNode = (nextNode + 1 != travelPath.Length) ? nextNode + 1 : nextNode;

                    updateVectors();
                }
            }
        }
        public void UpdatePlayerPosition(float elapsed)
        {
            playerPosition += Vector2.Multiply(travelVector, MoveSpeed * elapsed);
            starMap.SetPlayerPosition(playerPosition);
        }

        private void updateVectors()
        {
            travelVector = travelPath[nextNode].Coordinates - travelPath[currentNode].Coordinates;
            travelVector.Normalize();

            float distance = travelPath[currentNode].Coordinates.Distance(travelPath[nextNode].Coordinates);
            timeToNextNode = distance / MoveSpeed;
        }
    }
}
