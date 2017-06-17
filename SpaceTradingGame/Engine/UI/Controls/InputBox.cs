using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;

namespace SpaceTradingGame.Engine.UI.Controls
{
    public class InputBox : Control
    {
        public InputBox(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            this.text = string.Empty;

            this.Position = new Point(x, y);
            this.Size = new Point(width, height);

            if (height > 1)
                this.isMultiline = true;
        }
        public InputBox(Control parent, int x, int y, int width)
            : base(parent)
        {
            this.text = string.Empty;

            this.Position = new Point(x, y);
            this.Size = new Point(width, 1);
            this.isMultiline = false;
        }

        public override void DrawStep()
        {
            this.clearArea();

            GraphicConsole.SetColor(Color.Transparent, this.fillColor);
            GraphicConsole.Draw.Rect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

            if (this.text != string.Empty)
            {
                GraphicConsole.SetColor(this.textColor, this.fillColor);
                GraphicConsole.SetCursor(this.Position);
                GraphicConsole.Write(this.text);
            }

            base.DrawStep();
        }
        public override void UpdateFrame(GameTime gameTime)
        {
            if (this.hasFocus)
            {
                this.cursorCounter += gameTime.ElapsedTime.TotalMilliseconds;

                if (this.cursorCounter >= cursorFlickerRate * 2)
                    this.cursorCounter = 0.0;
                if (this.cursorCounter > cursorFlickerRate)
                {
                    GraphicConsole.SetColor(this.textColor, this.fillColor);
                    GraphicConsole.SetCursor(this.Position.X + this.text.Length, this.Position.Y);
                    GraphicConsole.Write(this.cursor);
                }
                else
                {
                    GraphicConsole.SetColor(this.textColor, this.fillColor);
                    GraphicConsole.SetCursor(this.Position.X + this.text.Length, this.Position.Y);
                    GraphicConsole.Write(' ');
                }
            }
        }
        public void ForceSubmit(object sender)
        {
            this.onSubmit(sender);
        }
        public void Clear()
        {
            this.text = "";
        }

        public override void KeyPress(KeyPressEventArgs e)
        {
            if (hasFocus)
            {
                if (this.CharacterLimit == 0 || this.text.Length < this.characterLimit)
                {
                    this.text += e.KeyChar;
                    this.DrawStep();
                }
            }
        }
        public override void KeyUp(KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.BackSpace)
            {
                if (text.Length > 0)
                    text = text.Substring(0, text.Length - 1);
                this.DrawStep();
            }
            else if (e.Key == Key.Enter)
            {
                this.onSubmit(this);
                this.hasFocus = false;
                this.DrawStep();
            }
            else if (e.Key == Key.Escape)
            {
                this.hasFocus = false;
                this.DrawStep();
            }
        }
        public override void MouseUp(MouseButtonEventArgs e)
        {
            this.hasFocus = true;
            this.cursorCounter = 0.0;

            this.DrawStep();
        }

        protected void onSubmit(object sender)
        {
            Submitted?.Invoke(sender);

            InterfaceManager.UpdateStep();
            InterfaceManager.DrawStep();
        }
        private void wrapText()
        {
            if (this.isMultiline)
            {
                this.text = TextUtilities.StripFormatting(text);
                this.text = TextUtilities.WordWrap(text, this.size.X);
            }
        }

        private string text;
        private Color4 textColor = Color4.White;
        private Color4 fillColor = Color4.Black;

        private bool hasFocus = false;
        private bool isMultiline = false;
        private bool showCursor = true;

        private char cursor = '█';
        private double cursorCounter = 0.0;
        private double cursorFlickerRate = 600.0;
        private int characterLimit = 0;
        //private int line = 0; //for multiline cursor placement

        public event InputBoxSubmit Submitted;
        public delegate void InputBoxSubmit(object sender);

        #region Properties
        public string Text { get { return this.text; } set { this.text = value; } }
        public Color4 TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public Color4 FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public bool HasFocus { get { return this.hasFocus; } set { this.hasFocus = value; } }
        //public bool IsMultiline { get { return this.isMultiline; } set { this.isMultiline = value; } }
        public bool ShowCursor { get { return this.showCursor; } set { this.showCursor = value; } }
        public int CharacterLimit { get { return this.characterLimit; } set { this.characterLimit = value; } }
        #endregion
    }
}
