using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI
{
    public class Interface
    {
        private List<Control> controls;
        private InterfaceManager interfaceManager;

        public InterfaceManager InterfaceManager { get { return interfaceManager; } }
        public Engine.Console.GraphicConsole GraphicConsole { get { return interfaceManager.Console; } }
        public Game.GameManager GameManager { get { return interfaceManager.Game.GameManager; } }

        public Interface(InterfaceManager manager)
        {
            this.interfaceManager = manager;
            this.controls = new List<Control>();
        }

        public virtual void DrawFrame(GameTime gameTime) { }
        public virtual void UpdateFrame(GameTime gameTime)
        {
            foreach (Control control in controls)
                control.UpdateFrame(gameTime);
        }
        public virtual void DrawStep()
        {
            foreach (Control control in controls)
                control.DrawStep();
        }
        public virtual void UpdateStep()
        {
            foreach (Control control in controls)
                control.UpdateStep();
        }

        public virtual void OnEnable()
        {
            interfaceManager.Console.Clear();

            DrawStep();
            UpdateStep();
        }
        public virtual void OnDisable()
        {

        }

        public void RegisterControl(Control control)
        {
            control.Interface = this;
            controls.Add(control);
        }

        /* INPUT FUNCTIONS */
        public void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Control control in controls)
            {
                if (control.Contains(InterfaceManager.CurrentCursorPosition))
                    control.MouseDown(e);
            }
        }
        public void Game_MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach (Control control in controls)
            {
                if (control.Contains(InterfaceManager.CurrentCursorPosition))
                    control.MouseUp(e);
                else
                    control.MouseUpAway(e);
            }
        }
        public void Game_MouseEnter(object sender, EventArgs e) { }
        public void Game_MouseLeave(object sender, EventArgs e) { }
        public void Game_MouseMove(object sender, MouseMoveEventArgs e)
        {
            foreach (Control control in controls)
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
        public void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            foreach (Control control in controls)
            {
                if (control.Contains(InterfaceManager.CurrentCursorPosition))
                    control.MouseWheel(e);
            }
        }
        public void Game_KeyPress(object sender, KeyPressEventArgs e)
        {
            foreach (Control control in controls)
            {
                control.KeyPress(e);
            }
        }
        public void Game_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            foreach (Control control in controls)
            {
                control.KeyUp(e);
            }
        }
        public void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            foreach (Control control in controls)
            {
                control.KeyDown(e);
            }
        }
    }
}
