using System;
using System.Collections.Generic;
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

            up = new Button(null, "▲", 87, 30);
            down = new Button(null, "▼", 87, 34);
            left = new Button(null, "◄", 84, 32);
            right = new Button(null, "►", 90, 32);

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

            RegisterControl(screenTitle);
            RegisterControl(starMap);
            RegisterControl(up);
            RegisterControl(down);
            RegisterControl(left);
            RegisterControl(right);
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

        private Title screenTitle;
        private Button up, down, left, right;
        private StarMap starMap;
    }
}
