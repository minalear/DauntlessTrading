using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace SpaceTradingGame.Engine.Console
{
    public class DrawingUtilities
    {
        private GraphicConsole console;

        public DrawingUtilities(GraphicConsole console)
        {
            this.console = console;
            this.PaintMode = PaintModes.Default;
        }

        public void Rect(int x0, int y0, int width, int height, char token, bool solid)
        {
            if (solid)
            {
                for (int y = y0; y < y0 + height; y++)
                {
                    for (int x = x0; x < x0 + width; x++)
                        Draw(x, y, token);
                }
            }
            else
            {
                for (int y = y0; y < y0 + height; y++)
                {
                    Draw(x0, y, token);
                    Draw(x0 + width - 1, y, token);
                }
                for (int x = x0; x < x0 + width; x++)
                {
                    Draw(x, y0, token);
                    Draw(x, y0 + height - 1, token);
                }
            }
        }
        public void Rect(Rectangle rect, char token, bool solid)
        {
            Rect(rect.X, rect.Y, rect.Width, rect.Height, token, solid);
        }

        public void Line(int x0, int y0, int x1, int y1, char token)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
            if (x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
            int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

            for (int x = x0; x <= x1; ++x)
            {
                /*if (!(steep ? plot(y, x) : plot(x, y))) return;*/
                if (steep)
                    console.Put(token, y, x);
                else
                    console.Put(token, x, y);

                err = err - dY;
                if (err < 0) { y += ystep; err += dX; }
            }
        }
        public void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

        public void Circle(int xp, int yp, int r, char token)
        {
            int x = r, y = 0;
            int radiusError = 1 - x;

            while (x >= y)
            {
                Draw(x + xp, y + yp, token);
                Draw(y + xp, x + yp, token);
                Draw(-x + xp, y + yp, token);
                Draw(-y + xp, x + yp, token);
                Draw(-x + xp, -y + yp, token);
                Draw(-y + xp, -x + yp, token);
                Draw(x + xp, -y + yp, token);
                Draw(y + xp, -x + yp, token);

                y++;
                if (radiusError < 0)
                {
                    radiusError += 2 * y + 1;
                }
                else
                {
                    x--;
                    radiusError += 2 * (y - x + 1);
                }
            }
        }
        public void FillCircle(int xp, int yp, int r, char token)
        {
            int y0 = yp - r;
            int y1 = yp + r;
            int x0 = xp - r;
            int x1 = xp + r;

            OpenTK.Vector2 center = new OpenTK.Vector2(xp, yp);
            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    if (center.DistanceSqr(new OpenTK.Vector2(x, y)) < r * r)
                        Draw(x, y, token);
                }
            }
        }

        public Color4 BlendColor(Color4 one, Color4 two)
        {
            float r = MathHelper.Clamp((one.R + two.R) / 2, 0f, 1f);
            float g = MathHelper.Clamp((one.G + two.G) / 2, 0f, 1f);
            float b = MathHelper.Clamp((one.B + two.B) / 2, 0f, 1f);
            float a = MathHelper.Clamp((one.A + two.A) / 2, 0f, 1f);

            return new Color4(r, g, b, a);
        }
        public void Draw(int x, int y, char token)
        {
            if (PaintMode == PaintModes.Default)
            {
                console.Put(token, x, y);
                console.SetColor(x, y);
            }
            else if (PaintMode == PaintModes.Add)
            {
                CharToken info = console.GetCharacterInformation(x, y);
                Color4 blendedForegroundColor = BlendColor(info.ForegroundColor, console.ForegroundColor);
                Color4 blendedBackgroundColor = BlendColor(info.BackgroundColor, console.BackgroundColor);

                console.SetColor(blendedForegroundColor, blendedBackgroundColor, x, y);
            }
            else
            {
                console.Put(token, x, y);
            }
        }

        public PaintModes PaintMode { get; set; }
    }

    public enum PaintModes { Default, Fill, Add }
}
