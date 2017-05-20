using System;
using System.Drawing;
using OpenTK.Input;
using OpenTK.Graphics;

namespace SpaceTradingGame.Engine.UI.Controls
{
    public class CheckBox : Control
    {
        public CheckBox(Control parent, int x, int y) 
            : base(parent)
        {
            this.ForegroundColor = Color.White;
            this.BackgroundColor = Color.Black;

            this.Size = new Point(1, 1);
            this.Position = new Point(x, y);
        }

        public override void DrawStep()
        {
            GraphicConsole.SetCursor(this.Position.X, this.Position.Y);

            if (this.isHover) //Mouse is hovering
                GraphicConsole.SetColor(this.foregroundColorHover, this.backgroundColorHover);
            else if (this.enabled) //Check box is checked
                GraphicConsole.SetColor(this.foregroundColorEnabled, this.backgroundColorEnabled);
            else //Check box isn't checked
                GraphicConsole.SetColor(this.foregroundColorDisabled, this.backgroundColorDisabled);


            if (this.enabled)
                GraphicConsole.Write(this.enabledToken);
            else
                GraphicConsole.Write(this.disabledToken);

            base.DrawStep();
        }
        public override void UpdateFrame(GameTime gameTime)
        {
            
        }

        public override void MouseEnter()
        {
            this.isHover = true;
            this.DrawStep();

            base.MouseEnter();
        }
        public override void MouseLeave()
        {
            this.isHover = false;
            this.DrawStep();

            base.MouseLeave();
        }
        public override void MouseDown(MouseButtonEventArgs e)
        {
            this.isHover = false;
            this.DrawStep();

            base.MouseDown(e);
        }
        public override void MouseUp(MouseButtonEventArgs e)
        {
            this.onToggle();

            InterfaceManager.UpdateStep();
            InterfaceManager.DrawStep();

            this.isHover = false;

            base.MouseUp(e);
        }

        protected void onToggle()
        {
            this.enabled = !this.enabled;

            this.Toggled?.Invoke(this);
        }

        private char enabledToken = '⌂';
        private char disabledToken = '⌂';

        private bool enabled = false;
        private bool isHover = false;

        private Color4 foregroundColorEnabled = Color4.Black;
        private Color4 foregroundColorDisabled = Color4.White;
        private Color4 foregroundColorHover = Color4.White;

        private Color4 backgroundColorEnabled = Color4.White;
        private Color4 backgroundColorDisabled = Color4.Black;
        private Color4 backgroundColorHover = new Color4(170, 181, 187, 255);

        #region Properties
        public Color4 ForegroundColor { get; set; }
        public Color4 BackgroundColor { get; set; }
        public char EnabledToken { get { return this.enabledToken; } set { this.enabledToken = value; } }
        public char DisabledToken { get { return this.disabledToken; } set { this.disabledToken = value; } }
        public bool Enabled { get { return this.enabled; } set { this.enabled = value; } }
        #endregion

        public event CheckBoxPressed Toggled;
        public delegate void CheckBoxPressed(object sender);
    }
}
