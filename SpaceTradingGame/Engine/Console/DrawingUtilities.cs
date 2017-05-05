using System;
using System.Drawing;

namespace SpaceTradingGame.Engine.Console
{
    public class DrawingUtilities
    {
        private GraphicConsole console;
        public DrawingUtilities(GraphicConsole console)
        {
            this.console = console;
        }

        public void Rect(int x0, int y0, int width, int height, char token, bool solid)
        {
            if (solid)
            {
                for (int y = y0; y < y0 + height; y++)
                {
                    for (int x = x0; x < x0 + width; x++)
                        console.Put(token, x, y);
                }
            }
            else
            {
                for (int y = y0; y < y0 + height; y++)
                {
                    console.Put(token, x0, y);
                    console.Put(token, x0 + width - 1, y);
                }
                for (int x = x0; x < x0 + width; x++)
                {
                    console.Put(token, x, y0);
                    console.Put(token, x, y0 + height - 1);
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
                console.Put(token, x + xp, y + yp);
                console.Put(token, y + xp, x + yp);
                console.Put(token, -x + xp, y + yp);
                console.Put(token, -y + xp, x + yp);
                console.Put(token, -x + xp, -y + yp);
                console.Put(token, -y + xp, -x + yp);
                console.Put(token, x + xp, -y + yp);
                console.Put(token, y + xp, -x + yp);
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

        public Color BlendColor(Color one, Color two)
        {
            return Color.FromArgb((one.A + two.A) / 2, (one.R + two.R) / 2, (one.G + two.G) / 2, (one.B + two.B) / 2);
        }
    }
}
