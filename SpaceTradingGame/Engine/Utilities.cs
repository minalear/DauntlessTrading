using System;
using OpenTK.Graphics;

namespace SpaceTradingGame.Engine
{
    public static class TextUtilities
    {
        public static string WordWrap(string unformattedString, int width)
        {
            string returnString = "";
            unformattedString = ApplyFormatting(unformattedString);

            string[] groups = unformattedString.Split('\n');

            for (int group = 0; group < groups.Length; group++)
            {
                string[] words = groups[group].Split(' ');

                string line = "";
                for (int word = 0; word < words.Length; word++)
                {
                    if (line.Length + words[word].Length <= width)
                    {
                        returnString += words[word] + " ";
                        line += words[word] + " ";
                    }
                    else
                    {
                        returnString += "\n" + words[word] + " ";
                        line = words[word] + " ";
                    }
                }

                returnString += "\n";
            }

            return returnString;
        }

        public static string StripFormatting(string text)
        {
            text = text.Replace("\n", string.Empty);

            return text;
        }

        public static string CenterTextPadding(string unformattedString, int width, char padToken)
        {
            if (unformattedString.Length < width)
            {
                int padAmount = (width - unformattedString.Length) / 2;

                string format = "";
                for (int i = 0; i < padAmount; i++)
                    format += padToken;
                format += unformattedString;
                for (int i = 0; i < padAmount; i++)
                    format += padToken;

                return format;
            }

            return unformattedString;
        }

        public static string ApplyFormatting(string text)
        {
            //Strip C# Formatting
            text = text.Replace("\n", "<br>");
            text = text.Replace("\r", string.Empty);

            //Apply HTML-F Formatting (F is for Fuck)
            text = text.Replace("<br>", "\n");
            text = text.Replace("<tb>", "     ");

            return text;
        }

        public static Color4 GetColor(string colorName)
        {
            colorName = colorName.ToLower();

            if (colorName == "red")
                return Color4.Red;
            if (colorName == "blue")
                return new Color4(80, 109, 255, 255);
            if (colorName == "yellow")
                return Color4.Yellow;
            if (colorName == "green")
                return Color4.Green;
            if (colorName == "orange")
                return Color4.Orange;
            if (colorName == "purple")
                return Color4.Purple;
            if (colorName == "cyan")
                return Color4.Cyan;
            if (colorName == "pink")
                return Color4.Pink;
            if (colorName == "gray")
                return Color4.Gray;
            if (colorName == "darkgray")
                return Color4.DarkGray;
            if (colorName == "transparent" || colorName == "clear")
                return Color4.Transparent;

            return Color4.White;
        }
    }
    public static class Extensions
    {
        public static float Truncate(this float f, int digits)
        {
            double mult = System.Math.Pow(10.0, digits);
            double result = System.Math.Truncate(mult * f) / mult;
            return (float)result;
        }

        public static double Truncate(this double f, int digits)
        {
            double mult = System.Math.Pow(10.0, digits);
            double result = System.Math.Truncate(mult * f) / mult;
            return result;
        }
    }

    /// <summary>
    /// Static Class providing random number functionality.
    /// </summary>
    public static class RNG
    {
        private static Random randomGenerator = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Returns a random integer number.
        /// </summary>
        public static int Next()
        {
            return randomGenerator.Next();
        }

        /// <summary>
        /// Returns a random integer number with an upper limit.
        /// </summary>
        public static int Next(int max)
        {
            return randomGenerator.Next(max);
        }

        /// <summary>
        /// Returns a random integer number between two values.
        /// </summary>
        public static int Next(int min, int max)
        {
            return randomGenerator.Next(min, max);
        }

        /// <summary>
        /// Returns a random double number.
        /// </summary>
        public static double NextDouble()
        {
            return randomGenerator.NextDouble();
        }

        /// <summary>
        /// Returns a random double number between two values.
        /// </summary>
        public static double NextDouble(double min, double max)
        {
            return NextDouble() * (max - min) + min;
        }
    }
}
