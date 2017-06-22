using System;
using OpenTK.Graphics;

namespace SpaceTradingGame.Engine.UI.Controls.Custom
{
    public class Clock : Control
    {
        private double tickRate, subTick, timer;
        private int fill = 0;

        public Clock(Control parent, int width, double tickRate)
            : base(parent)
        {
            this.Size = new System.Drawing.Point(width, 1);
            this.tickRate = tickRate;
            this.subTick = tickRate / width;

            Paused = false;
            ForeColor = Color4.White;
            FillColor = Color4.Black;
        }

        public override void UpdateFrame(GameTime gameTime)
        {
            if (!Paused)
            {
                timer += gameTime.ElapsedTime.TotalSeconds;

                if (timer >= tickRate)
                {
                    timer = 0.0;
                    fill = 0;

                    this.TimerLapse?.Invoke(this, EventArgs.Empty);
                }
                else if (timer >= subTick * fill)
                {
                    fill++;
                    InterfaceManager.DrawStep();
                }
            }

            base.UpdateFrame(gameTime);
        }
        public override void DrawStep()
        {
            GraphicConsole.SetColor(ForeColor, FillColor);

            GraphicConsole.SetCursor(this.Position);
            GraphicConsole.Write(new string('░', Size.X));

            GraphicConsole.SetCursor(this.Position);
            GraphicConsole.Write(new string('█', fill));

            base.DrawStep();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }
        public void Toggle()
        {
            Paused = !Paused;
        }

        public Color4 FillColor { get; set; }
        public Color4 ForeColor { get; set; }
        public bool Paused { get; set; }

        public double TickRate { get { return tickRate; } set { tickRate = value; } }

        public delegate void TimerLapseEvent(object sender, EventArgs e);
        public event TimerLapseEvent TimerLapse;
    }
}
