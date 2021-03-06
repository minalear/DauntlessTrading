﻿using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Input;
using SpaceTradingGame.Engine.Console;

namespace SpaceTradingGame.Engine.UI.Controls
{
    public class Control
    {
        protected Control parent;
        protected Point position, size;
        protected bool isAbsolute;
        protected List<Control> children;

        public Control Parent { get { return this.parent; } }
        public Interface Interface { get; set; }
        public Point Position { get { return this.getPosition(); } set { this.setPosition(value); } }
        public Point Size { get { return this.size; } set { this.size = value; } }
        public List<Control> Children { get { return this.children; } set { this.children = value; } }
        public bool IsVisible { get; set; }

        protected GraphicConsole GraphicConsole
        {
            get
            {
                return InterfaceManager.Console;
            }
        }
        protected InterfaceManager InterfaceManager
        {
            get
            {
                return (isAbsolute) ? Interface.InterfaceManager : parent.Interface.InterfaceManager;
            }
        }

        public Control()
        {
            this.isAbsolute = true;
            this.IsVisible = true;

            this.children = new List<Control>();
        }
        public Control(Control parent)
        {
            this.children = new List<Control>();
            this.IsVisible = true;

            if (parent != null)
            {
                this.parent = parent;
                this.parent.Children.Add(this);

                this.isAbsolute = false;
            }
            else
            {
                this.isAbsolute = true;
            }
        }

        public virtual void DrawStep()
        {
            GraphicConsole.ClearColor();

            for (int i = 0; i < this.children.Count; i++)
            {
                if (this.children[i].IsVisible)
                    this.children[i].DrawStep();
            }
        }
        public virtual void UpdateFrame(GameTime gameTime)
        {
            for (int i = 0; i < this.children.Count; i++)
            {
                if (this.children[i].IsVisible)
                    this.children[i].UpdateFrame(gameTime);
            }
        }
        public virtual void UpdateStep()
        {
            for (int i = 0; i < this.children.Count; i++)
            {
                if (this.children[i].IsVisible)
                    this.children[i].UpdateStep();
            }
        }

        public virtual bool Contains(Point point)
        {
            Point relativePosition = this.Position;
            return (point.X >= relativePosition.X && point.Y >= relativePosition.Y && 
                    point.X < relativePosition.X + size.X && point.Y < relativePosition.Y + size.Y);
        }

        public virtual void MouseDown(MouseButtonEventArgs e)
        {
            foreach (Control control in children)
            {
                if (control.Contains(InterfaceManager.CurrentCursorPosition))
                    control.MouseDown(e);
            }
        }
        public virtual void MouseUp(MouseButtonEventArgs e)
        {
            foreach (Control control in children)
            {
                if (control.Contains(InterfaceManager.CurrentCursorPosition))
                    control.MouseUp(e);
                else
                    control.MouseUpAway(e);
            }
        }
        /// <summary>
        /// Called when the Mouse Button is released, but is not currently over the control
        /// </summary>
        public virtual void MouseUpAway(MouseEventArgs e) { }
        public virtual void MouseEnter() { }
        public virtual void MouseLeave() { }
        public virtual void MouseMove()
        {
            foreach (Control control in children)
            {
                if (control.Contains(InterfaceManager.CurrentCursorPosition))
                {
                    if (!control.Contains(InterfaceManager.PreviousCursorPosition))
                        control.MouseEnter();
                    control.MouseMove();
                }
                else if (control.Contains(InterfaceManager.PreviousCursorPosition))
                    control.MouseLeave();
            }
        }
        public virtual void MouseWheel(MouseWheelEventArgs e)
        {
            foreach (Control control in children)
            {
                if (control.Contains(InterfaceManager.CurrentCursorPosition))
                    control.MouseWheel(e);
            }
        }
        public virtual void KeyPress(OpenTK.KeyPressEventArgs e) { }
        public virtual void KeyUp(KeyboardKeyEventArgs e) { }
        public virtual void KeyDown(KeyboardKeyEventArgs e) { }

        public void SetParent(Control parent)
        {
            this.parent = parent;
            this.isAbsolute = false;
        }

        protected virtual void clearArea()
        {
            GraphicConsole.ClearColor();
            GraphicConsole.Draw.Rect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y, ' ', true);
        }
        protected virtual Point getPosition()
        {
            if (this.isAbsolute)
                return this.position;
            else
            {
                Point parentPos = this.Parent.Position;
                return new Point(this.position.X + parentPos.X, this.position.Y + parentPos.Y);
            }
        }
        protected virtual void setPosition(Point point)
        {
            if (this.isAbsolute)
                this.position = point;
            else
            {
                Point parentPos = this.Parent.Position;

                this.position.X = point.X - parentPos.X;
                this.position.Y = point.Y - parentPos.Y;
            }
        }
    }
}
