﻿using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class ShipScreen : Interface
    {
        public ShipScreen(InterfaceManager manager)
            : base(manager)
        {
            shipDesignationTitle = new Title(null, "USS Ravioli", 1, 1, Title.TextAlignModes.Left);
            shipDesignationTitle.TextColor = Color4.White;
            shipModelTitle = new Title(null, "Disney Gummi mk1*", 1, 2, Title.TextAlignModes.Left);
            shipModelTitle.TextColor = Color4.Gray;

            cockpit = new Button(null, " ", 9, 6, 3, 2);
            cockpit.FillColor = new Color4(50, 50, 50, 255);
            leftWing = new Button(null, " ", 4, 9, 3, 2);
            leftWing.FillColor = new Color4(50, 50, 50, 255);
            cargoBay = new Button(null, " ", 9, 9, 3, 2);
            cargoBay.FillColor = new Color4(50, 50, 50, 255);
            rightWing = new Button(null, " ", 14, 9, 3, 2);
            rightWing.FillColor = new Color4(50, 50, 50, 255);
            drive = new Button(null, " ", 9, 12, 3, 2);
            drive.FillColor = new Color4(50, 50, 50, 255);
            leftEngine = new Button(null, " ", 5, 14, 3, 2);
            leftEngine.FillColor = new Color4(50, 50, 50, 255);
            rightEngine = new Button(null, " ", 13, 14, 3, 2);
            rightEngine.FillColor = new Color4(50, 50, 50, 255);

            scrollingList = new ScrollingList(null, 30, 1, GraphicConsole.BufferWidth - 31, 22);
            scrollingList.FillColor = new Color4(50, 50, 50, 255);
            descriptionBox = new TextBox(null, 30, 24, GraphicConsole.BufferWidth - 31, 12);
            descriptionBox.FillColor = new Color4(50, 50, 50, 255);

            List<ListItem> list = new List<ListItem>() {
                new ListItem("Item 01"),
                new ListItem("Item 02"),
                new ListItem("Item 03"),
                new ListItem("Item 04")
            };
            scrollingList.SetList(list);

            descriptionBox.Text = "This is a very cool and awesome description box.  <color Blue>Hello I'm blue.<color>";

            //Titles
            RegisterControl(shipDesignationTitle);
            RegisterControl(shipModelTitle);

            //Modules
            RegisterControl(cockpit);
            RegisterControl(leftWing);
            RegisterControl(cargoBay);
            RegisterControl(rightWing);
            RegisterControl(drive);
            RegisterControl(leftEngine);
            RegisterControl(rightEngine);

            //Other
            RegisterControl(scrollingList);
            RegisterControl(descriptionBox);
        }

        public override void DrawStep()
        {
            GraphicConsole.SetCursor(5, 6);
            GraphicConsole.Write("   ╔   ╗");
            GraphicConsole.SetCursor(5, 7);
            GraphicConsole.Write("   ║   ║");
            GraphicConsole.SetCursor(5, 8);
            GraphicConsole.Write("╔══╝   ╚══╗");
            GraphicConsole.SetCursor(5, 11);
            GraphicConsole.Write("╚═╗     ╔═╝");
            GraphicConsole.SetCursor(5, 12);
            GraphicConsole.Write("  ║     ║");
            GraphicConsole.SetCursor(5, 13);
            GraphicConsole.Write("╔═╝     ╚═╗");

            base.DrawStep();
        }

        private Title shipDesignationTitle, shipModelTitle;
        private Button cockpit, leftWing, rightWing, cargoBay, drive, leftEngine, rightEngine;
        private ScrollingList scrollingList;
        private TextBox descriptionBox;
    }
}
