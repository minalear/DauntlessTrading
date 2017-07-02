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

            this.MinimumTickRate = 0.5;
            this.MaximumTickRate = 3.0;
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

                    this.TimerTick?.Invoke(this, EventArgs.Empty);
                    this.TimerLapse?.Invoke(this, EventArgs.Empty);
                }
                else if (timer >= subTick * fill)
                {
                    fill++;

                    this.TimerTick?.Invoke(this, EventArgs.Empty);
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
        private void UpdateTickRate(double rate)
        {
            this.tickRate = OpenTK.MathHelper.Clamp(rate, MinimumTickRate, MaximumTickRate);
            this.subTick = this.tickRate / Size.X;
        }

        public Color4 FillColor { get; set; }
        public Color4 ForeColor { get; set; }
        public bool Paused { get; set; }

        public double MinimumTickRate { get; set; }
        public double MaximumTickRate { get; set; }

        public double TickRate { get { return tickRate; } set { UpdateTickRate(value); } }
        public double SubTickRate { get { return subTick; } }

        public delegate void TimerLapseEvent(object sender, EventArgs e);
        public delegate void TimerTickEvent(object sender, EventArgs e);

        public event TimerLapseEvent TimerLapse;
        public event TimerTickEvent TimerTick;
    }
}
