using System;
using System.Collections.Generic;
using OpenTK.Input;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI
{
    public class Interface
    {
        private InterfaceManager interfaceManager;
        private List<Control> controls;

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
            controls.Add(control);
        }

        /* INPUT FUNCTIONS */
        public void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        public void Game_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
        public void Game_MouseEnter(object sender, EventArgs e)
        {

        }
        public void Game_MouseLeave(object sender, EventArgs e)
        {

        }
        public void Game_MouseMove(object sender, MouseMoveEventArgs e)
        {

        }
        public void Game_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }
    }
}
