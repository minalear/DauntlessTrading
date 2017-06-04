using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;
using SpaceTradingGame.Engine.UI.Controls.Custom;

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
                starMap.SetCurrentSystem(starMap.SelectedSystem);
            };

            starMap.Selected += (sender, e) =>
            {
                systemTitle.Text = e.Name;

                string desc = "";
                foreach (Game.Planetoid planet in e.Planetoids)
                {
                    desc += planet.Name + "\n";
                }

                systemDesc.Text = desc;
                InterfaceManager.DrawStep();
            };

            RegisterControl(screenTitle);
            RegisterControl(systemTitle);
            RegisterControl(systemDesc);
            RegisterControl(starMap);
            RegisterControl(up);
            RegisterControl(down);
            RegisterControl(left);
            RegisterControl(right);
            RegisterControl(travelButton);
        }

        public override void OnEnable()
        {
            starMap.SetSystemList(GameManager.Systems);
            starMap.SetCurrentSystem(GameManager.Systems[0]); //Sol System

            base.OnEnable();
        }

        public override void DrawStep()
        {
            base.DrawStep();
        }

        private Title screenTitle, systemTitle;
        private TextBox systemDesc;
        private Button up, down, left, right;
        private Button travelButton;
        private StarMap starMap;
    }
}
