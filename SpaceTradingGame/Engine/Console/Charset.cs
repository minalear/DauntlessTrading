using System.Collections.Generic;
using OpenTK;

namespace SpaceTradingGame.Engine.Console
{
    public class Charset
    {
        public int TextureID { get; private set; }
        public int CharWidth { get; private set; }
        public int CharHeight { get; private set; }

        Dictionary<char, int> characterIndex;
        const string CHARSET_STRING =
            " ☺☻♥♦♣♠•◘○◙♂♀♪♫☼" +
            "►◄↕‼¶§▬↨↑↓→←∟↔▲▼" +
            "\n!\"#$%&'()*+,-./" +
            "0123456789:;<=>?" +
            "@ABCDEFGHIJKLMNO" +
            "PQRSTUVWXYZ[\\]^_" +
            "`abcdefghijklmno" +
            "pqrstuvwxyz{|}~⌂" +
            "ÇüéâäàåçêëèïîìÄÅ" +
            "ÉæÆôöòûùÿÖÜ¢£¥₧ƒ" +
            "áíóúñÑªº¿⌐¬½¼¡«»" +
            "░▒▓│┤╡╢╖╕╣║╗╝╜╛┐" +
            "└┴┬├─┼╞╟╚╔╩╦╠═╬╧" +
            "╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀" +
            "αßΓπΣσµτΦΘΩδ∞φε∩" +
            "≡±≥≤⌠⌡÷≈°∙·√ⁿ²■╠";

        public Charset(ContentManager contentManager, int charWidth, int charHeight)
        {
            this.TextureID = contentManager.LoadTexture("Content/charset.png");

            this.CharWidth = charWidth;
            this.CharHeight = charHeight;

            this.characterIndex = new Dictionary<char, int>();

            for (int i = 0; i < CHARSET_STRING.Length; i++)
            {
                char ch = CHARSET_STRING[i];
                characterIndex.Add(ch, i);
            }

            //Escape Characters
            //characterIndex.Add('\n', 0);
            characterIndex.Add('\r', 0);
            characterIndex.Add('\t', 0);
        }

        public int GetID(char ch)
        {
            return characterIndex[ch];
        }
        public Vector2 CalculateTextureCoords(int id)
        {
            return new Vector2((id % 16) * this.CharWidth / 128f, (id / 16) * this.CharHeight / 192f);
        }
    }
}
