﻿using System;
using System.Drawing;
using OpenTK.Input;
using OpenTK.Graphics;

namespace SpaceTradingGame.Engine.UI.Controls
{
    public class ToggleButton : Control
    {
        public ToggleButton(Control parent, string text, int x, int y)
            : base(parent)
        {
            this.setDefaults();

            this.text = text;
            this.position = new Point(x, y);
            this.size = new Point(text.Length + 2, 3);

            this.setTextPosition();
        }
        public ToggleButton(Control parent, string text, int x, int y, int width, int height)
            : base(parent)
        {
            this.setDefaults();

            this.text = text;
            this.position = new Point(x, y);
            this.size = new Point(width, height);

            this.setTextPosition();
        }
        
        public override void DrawStep()
        {
            this.clearArea();

            if (this.mode == ButtonModes.Hover)
            {
                //Fill Area
                GraphicConsole.SetColor(Color.Transparent, this.fillColorHover);
                GraphicConsole.Draw.Rect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColor(this.textColorHover, this.fillColorHover);
                GraphicConsole.SetCursor(this.textPosition);
                GraphicConsole.Write(this.text);
            }
            else if (!this.enabled)
            {
                //Fill Area
                GraphicConsole.SetColor(Color.Transparent, this.fillColor);
                GraphicConsole.Draw.Rect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColor(this.textColor, this.fillColor);
                GraphicConsole.SetCursor(this.textPosition);
                GraphicConsole.Write(this.text);
            }
            else if (this.enabled)
            {
                //Fill Area
                GraphicConsole.SetColor(Color.Transparent, this.fillColorPressed);
                GraphicConsole.Draw.Rect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

                //Write Text
                GraphicConsole.SetColor(this.textColorPressed, this.fillColorPressed);
                GraphicConsole.SetCursor(this.textPosition);
                GraphicConsole.Write(this.text);
            }

            base.DrawStep();
        }
        public override void UpdateFrame(GameTime gameTime)
        {

        }

        public override void MouseEnter()
        {
            this.mode = ButtonModes.Hover;
            this.onButtonHover();
            this.DrawStep();

            base.MouseEnter();
        }
        public override void MouseLeave()
        {
            if (this.enabled)
                this.mode = ButtonModes.Pressed;
            else
                this.mode = ButtonModes.Normal;

            InterfaceManager.DrawStep();

            base.MouseLeave();
        }
        public override void MouseDown(MouseButtonEventArgs e)
        {
            this.mode = ButtonModes.Pressed;
            this.DrawStep();

            base.MouseDown(e);
        }
        public override void MouseUp(MouseButtonEventArgs e)
        {
            this.onButtonPress(MouseButton.Left);

            InterfaceManager.UpdateStep();
            InterfaceManager.DrawStep();

            base.MouseUp(e);
        }

        //Event Methods
        protected void onButtonPress(MouseButton button)
        {
            this.enabled = !this.enabled;

            this.Click?.Invoke(this);

            if (this.enabled)
                this.mode = ButtonModes.Pressed;
            else
                this.mode = ButtonModes.Normal;
        }
        protected void onButtonHover()
        {
            this.Hover?.Invoke(this);
        }

        private void setDefaults()
        {
            this.textColor = DEFAULT_TEXT_COLOR;
            this.fillColor = DEFAULT_FILL_COLOR;

            this.textColorHover = DEFAULT_TEXT_HOVER_COLOR;
            this.fillColorHover = DEFAULT_FILL_HOVER_COLOR;

            this.textColorPressed = DEFAULT_TEXT_PRESSED_COLOR;
            this.fillColorPressed = DEFAULT_FILL_PRESSED_COLOR;
        }
        private void setTextPosition()
        {
            this.textPosition.X = this.Position.X + (this.Size.X / 2 - this.text.Length / 2);
            this.textPosition.Y = (this.Size.Y / 2) + this.Position.Y;
        }

        private string text;
        private Color4 textColor, fillColor;
        private Color4 textColorHover, fillColorHover;
        private Color4 textColorPressed, fillColorPressed;
        private ButtonModes mode;
        private Point textPosition;
        private bool enabled = false;

        private enum ButtonModes { Normal, Hover, Pressed }

        #region Properties
        public string Text { get { return this.text; } set { this.text = value; this.setTextPosition(); } }
        public Color4 TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public Color4 FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public Color4 TextColorHover { get { return this.textColorHover; } set { this.textColorHover = value; } }
        public Color4 FillColorHover { get { return this.fillColorHover; } set { this.fillColorHover = value; } }
        public Color4 TextColorPressed { get { return this.textColorPressed; } set { this.textColorPressed = value; } }
        public Color4 FillColorPressed { get { return this.fillColorPressed; } set { this.fillColorPressed = value; } }
        public bool Enabled { get { return this.enabled; } set { this.enabled = value; } }
        #endregion
        #region Constants
        private static Color4 DEFAULT_TEXT_COLOR = Color4.White;
        private static Color4 DEFAULT_FILL_COLOR = Color4.Black;

        private static Color4 DEFAULT_TEXT_HOVER_COLOR = Color4.White;
        private static Color4 DEFAULT_FILL_HOVER_COLOR = new Color4(170, 181, 187, 255);

        private static Color4 DEFAULT_TEXT_PRESSED_COLOR = Color4.Black;
        private static Color4 DEFAULT_FILL_PRESSED_COLOR = Color4.White;
        #endregion

        public event ButtonToggled Click;
        public event ButtonHovered Hover;

        public delegate void ButtonToggled(object sender);
        public delegate void ButtonHovered(object sender);
    }
}
