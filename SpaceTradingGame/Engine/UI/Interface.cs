using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Input;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI
{
    public class Interface
    {
        private InterfaceManager interfaceManager;
        private List<Control> controls;

        public InterfaceManager InterfaceManager { get { return interfaceManager; } }

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
            }
        }
        public void Game_MouseEnter(object sender, EventArgs e)
        {
            foreach (Control control in controls)
            {
                if (!control.Contains(interfaceManager.PreviousCursorPosition) &&
                    control.Contains(interfaceManager.CurrentCursorPosition))
                {
                    control.MouseEnter();
                }
            }
        }
        public void Game_MouseLeave(object sender, EventArgs e)
        {
            foreach (Control control in controls)
            {
                if (control.Contains(interfaceManager.CurrentCursorPosition) &&
                    !control.Contains(interfaceManager.PreviousCursorPosition))
                {
                    control.MouseEnter();
                }
            }
        }
        public void Game_MouseMove(object sender, MouseMoveEventArgs e)
        {
            foreach (Control control in controls)
            {
                if (control.Contains(InterfaceManager.CurrentCursorPosition))
                    control.MouseMove();
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
    }
}
