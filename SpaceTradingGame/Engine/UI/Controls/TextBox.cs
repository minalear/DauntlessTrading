﻿using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Input;
using OpenTK.Graphics;

namespace SpaceTradingGame.Engine.UI.Controls
{
    public class TextBox : Control
    {
        public TextBox(Control parent, int x, int y, int width, int height)
            : base(parent)
        {
            this.Position = new Point(x, y);
            this.Size = new Point(width, height);

            this.setText(" ");
        }

        public override void DrawStep()
        {
            GraphicConsole.SetColor(this.textColor, this.fillColor);
            GraphicConsole.Draw.Rect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);

            if (!string.IsNullOrEmpty(this.text))
            {
                if (this.scroll)
                {
                    //Scroll Bar Rail
                    GraphicConsole.SetColor(this.scrollRailColor, this.fillColor);
                    for (int h = this.Position.Y; h < this.Size.Y + this.Position.Y; h++)
                    {
                        GraphicConsole.SetCursor(this.Position.X + this.Size.X, h);
                        GraphicConsole.Write(this.scrollRail);
                    }

                    //Scroll Bar
                    GraphicConsole.SetColor(this.scrollBarColor, this.fillColor);
                    GraphicConsole.SetCursor(this.Position.X + this.Size.X, (int)(this.scrollValue / 100f * this.Size.Y) + this.Position.Y);
                    GraphicConsole.Write(this.scrollBar);

                    string[] lines = this.text.Split('\n');
                    this.lineCount = lines.Length;

                    int line = (int)(this.scrollValue / 100f * (lines.Length - this.Size.Y + 1));
                    if (line < 0)
                        line = 0;

                    GraphicConsole.SetColor(this.textColor, this.fillColor);
                    for (int y = 0; y < this.Size.Y && y < lines.Length; y++)
                    {
                        if (line < lines.Length)
                        {
                            this.writeLine(lines[line], this.Position.X, this.Position.Y + y);
                        }

                        line++;
                    }
                }
                else
                {
                    string[] lines = this.text.Split('\n');
                    for (int line = 0; line < lines.Length && line < this.Size.Y; line++)
                    {
                        this.writeLine(lines[line], this.Position.X, this.Position.Y + line);
                    }
                }

                base.DrawStep();
            }
        }
        public override void MouseWheel(MouseWheelEventArgs e)
        {
            int differenceValue = e.Delta;

            if (differenceValue != 0)
            {
                this.scrollValue -= differenceValue/* / (this.lineCount / 2)*/;

                if (this.scrollValue < 0f)
                    this.scrollValue = 0f;
                else if (this.scrollValue >= 100f)
                    this.scrollValue = 99f;

                InterfaceManager.DrawStep();
            }

            base.MouseWheel(e);
        }

        private void setText(string t)
        {
            this.text = TextUtilities.WordWrap(t, this.Size.X - 1);

            string[] lines = this.text.Split('\n');

            if (lines.Length > this.Size.Y)
                this.scroll = true;
            else
                this.scroll = false;
        }
        private void writeLine(string line, int x, int y)
        {
            GraphicConsole.SetCursor(x, y);
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '<')
                {
                    int k = i;
                    string formatTag = "";
                    while (k < line.Length && line[k] != '>')
                    {
                        formatTag += line[k];
                        k++;
                    }
                    formatTag += ">";

                    line = line.Remove(i, formatTag.Length);

                    if (formatTag.Contains("<color ")) //Start Custom Color
                    {
                        //Get the color specified
                        formatTag = formatTag.Remove(0, 7);
                        formatTag = formatTag.Remove(formatTag.Length - 1);

                        //Retrieve the color and apply it
                        GraphicConsole.SetColor(TextUtilities.GetColor(formatTag), this.fillColor);
                    }
                    else if (formatTag == "<color>") //End Custom Color
                    {
                        //Reset colors back to normal
                        GraphicConsole.SetColor(this.textColor, this.fillColor);
                    }
                }
                GraphicConsole.Write(line[i]);
            }

            GraphicConsole.Write('\n');
        }

        private string text;
        private bool scroll;

        private float scrollValue = 0f;
        private int lineCount = 0;

        private char scrollRail = '║';
        private char scrollBar = '▓';

        private Color4 textColor = Color4.White;
        private Color4 fillColor = Color4.Black;
        private Color4 scrollRailColor = Color4.Gray;
        private Color4 scrollBarColor = Color4.LightGray;

        #region Properties
        public string Text { get { return this.text; } set { this.setText(value); } }
        public float ScrollValue { get { return this.scrollValue; } set { this.scrollValue = value; } }
        public Color4 TextColor { get { return this.textColor; } set { this.textColor = value; } }
        public Color4 FillColor { get { return this.fillColor; } set { this.fillColor = value; } }
        public Color4 ScrollRailColor { get { return this.scrollRailColor; } set { this.scrollRailColor = value; } }
        public Color4 ScrollBarColor { get { return this.scrollBarColor; } set { this.scrollBarColor = value; } }
        public char ScrollRail { get { return this.scrollRail; } set { this.scrollRail = value; } }
        public char ScrollBar { get { return this.scrollBar; } set { this.scrollBar = value; } } 
        #endregion
    }
}
