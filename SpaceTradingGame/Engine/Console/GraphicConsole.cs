using System;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SpaceTradingGame.Engine.Shaders;

namespace SpaceTradingGame.Engine.Console
{
    public class GraphicConsole
    {
        private TradingGame game;
        private Shader consoleShader;

        private int left, top, cursorLeft, cursorTop;
        private int bufferWidth, bufferHeight;
        private Charset charset;

        private CharToken[,] characterMatrix;
        private float[] vertices;
        private int vao, vbo;

        private Color4 foregroundColor, backgroundColor;

        private DrawingUtilities drawingUtilities;

        public GraphicConsole(TradingGame game) : this(game, 80, 30) { } //Default
        public GraphicConsole(TradingGame game, int bufferWidth, int bufferHeight)
        {
            this.game = game;

            this.left = 0;
            this.top = 0;

            this.bufferWidth = bufferWidth;
            this.bufferHeight = bufferHeight;

            this.foregroundColor = Color.White;
            this.backgroundColor = Color.Black;

            this.drawingUtilities = new DrawingUtilities(this);
            this.charset = new Charset(game.Content, 8, 12);

            this.initShader();

            this.characterMatrix = new CharToken[this.bufferWidth, this.bufferHeight];
            for (int y = 0; y < this.bufferHeight; y++)
            {
                for (int x = 0; x < this.bufferWidth; x++)
                {
                    this.characterMatrix[x, y] = new CharToken()
                    {
                        X = x,
                        Y = y,
                        Token = ' ',
                        TextureCoords = new Vector2(0.0f, 0.0f),
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    };
                }
            }

            this.setVertexBuffer();

            game.MouseMove += Window_MouseMove; //For updating CursorLeft and CursorTop

            //Initialze OpenGL drawing settings
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, charset.TextureID);
        }

        public void Write(string text)
        {
            for (int i = 0; i < text.Length; i++)
                this.Write(text[i]);
        }
        public void Write(char ch)
        {
            if (ch == '\n' || ch == '\r')
            {
                this.Top++;
                this.Left = 0;
            }
            else if (ch == '\t')
            {
                this.Write("    ");
            }
            else
            {
                this.Put(ch, this.left, this.top);
                this.Left++;
            }
        }
        public void Write(Object obj)
        {
            this.Write(obj.ToString());
        }

        public void WriteLine(string text)
        {
            this.Write(text + "\n");
        }
        public void WriteLine(Object obj)
        {
            this.Write(obj.ToString() + "\n");
        }

        public void Put(char token, int x, int y)
        {
            if (x >= 0 && x < bufferWidth && y >= 0 && y < bufferHeight)
                this.put(token, x, y);
        }
        //Doesn't bound check, private use only
        private void put(char token, int x, int y)
        {
            this.characterMatrix[x, y].Token = token;
            this.characterMatrix[x, y].TextureCoords = charset.CalculateTextureCoords(this.charset.GetID(token));
            this.characterMatrix[x, y].ForegroundColor = this.foregroundColor;
            this.characterMatrix[x, y].BackgroundColor = this.backgroundColor;
        }

        public void SetColor(Color4 foreground, Color4 background)
        {
            this.foregroundColor = foreground;
            this.backgroundColor = background;
        }
        public void SetCursor(Point position)
        {
            this.Left = position.X;
            this.Top = position.Y;
        }
        public void SetCursor(int left, int top)
        {
            this.Left = left;
            this.Top = top;
        }
        public void Clear()
        {
            this.ClearColor();

            for (int y = 0; y < this.bufferHeight; y++)
            {
                for (int x = 0; x < this.bufferWidth; x++)
                {
                    this.characterMatrix[x, y].Token = ' ';
                    this.characterMatrix[x, y].TextureCoords = new Vector2(0.0f, 0.0f);
                    this.characterMatrix[x, y].ForegroundColor = Color.White;
                    this.characterMatrix[x, y].BackgroundColor = Color.Black;
                }
            }

            Left = 0;
            Top = 0;
        }
        public void ClearColor()
        {
            this.foregroundColor = Color.White;
            this.backgroundColor = Color.Black;
        }

        public Point GetTilePosition(Point position)
        {
            return new Point(position.X / charset.CharWidth, position.Y / charset.CharHeight);
        }
        public Point GetTilePosition(int x, int y)
        {
            return new Point(x / charset.CharWidth, y / charset.CharHeight);
        }

        public void RenderFrame()
        {
            this.consoleShader.Use();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, charset.TextureID);

            GL.BindVertexArray(this.vao);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4 * bufferWidth * bufferHeight);
            GL.BindVertexArray(0);

            //GL.Flush();
            this.updateVertexBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StreamDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void UpdateFrame(GameTime gameTime) { }

        private void Window_MouseMove(object sender, MouseMoveEventArgs e)
        {
            this.cursorLeft = e.Position.X / this.charset.CharWidth;
            this.cursorTop = e.Position.Y / this.charset.CharHeight;
        }
        private void initShader()
        {
            string vertexSource = game.Content.LoadShaderSource("Content/vertexShader.glsl");
            string fragmentSource = game.Content.LoadShaderSource("Content/fragmentShader.glsl");

            this.consoleShader = new Shader(vertexSource, fragmentSource);
            this.consoleShader.Use();

            Matrix4 orthoProj = Matrix4.CreateOrthographicOffCenter(0.0f, game.Width, game.Height, 0.0f, -1.0f, 1.0f);
            this.consoleShader.SetInteger("font", 0); //Set sampler2D to 0
            this.consoleShader.SetMatrix4("proj", orthoProj);

            Matrix4 model = Matrix4.CreateTranslation(new Vector3(0.0f, 0.0f, 0.0f));
            this.consoleShader.SetMatrix4("model", model);
        }
        private void setVertexBuffer()
        {
            vertices = new float[40 * bufferWidth * bufferHeight];
            System.Console.WriteLine(vertices.Length * sizeof(float));
            this.updateVertexBuffer();

            this.vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StreamDraw);

            this.consoleShader.Use();
            GL.BindVertexArray(this.vao);

            int posAttrib = GL.GetAttribLocation(this.consoleShader.ID, "position");
            GL.EnableVertexAttribArray(posAttrib);
            GL.VertexAttribPointer(posAttrib, 2, VertexAttribPointerType.Float, false, 10 * sizeof(float), 0);

            int texAttrib = GL.GetAttribLocation(this.consoleShader.ID, "texcoords");
            GL.EnableVertexAttribArray(texAttrib);
            GL.VertexAttribPointer(texAttrib, 2, VertexAttribPointerType.Float, false, 10 * sizeof(float), 2 * sizeof(float));

            int foreAttrib = GL.GetAttribLocation(this.consoleShader.ID, "foreColor");
            GL.EnableVertexAttribArray(foreAttrib);
            GL.VertexAttribPointer(foreAttrib, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 4 * sizeof(float));

            int backAttrib = GL.GetAttribLocation(this.consoleShader.ID, "backColor");
            GL.EnableVertexAttribArray(backAttrib);
            GL.VertexAttribPointer(backAttrib, 3, VertexAttribPointerType.Float, false, 10 * sizeof(float), 7 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
        private void updateVertexBuffer()
        {
            float tw = (float)charset.CharWidth / 128.0f;
            float th = (float)charset.CharHeight / 192.0f;

            int i = 0;
            for (int y = 0; y < bufferHeight; y++)
            {
                for (int x = 0; x < bufferWidth; x++, i++)
                {
                    #region Vertex Info
                    //Coord 0
                    vertices[0 + i * 40] = x * charset.CharWidth; //0
                    vertices[1 + i * 40] = y * charset.CharHeight; //0

                    vertices[2 + i * 40] = characterMatrix[x, y].TextureCoords.X;
                    vertices[3 + i * 40] = characterMatrix[x, y].TextureCoords.Y;

                    //Foreground Color
                    vertices[4 + i * 40] = characterMatrix[x, y].ForegroundColor.R;
                    vertices[5 + i * 40] = characterMatrix[x, y].ForegroundColor.G;
                    vertices[6 + i * 40] = characterMatrix[x, y].ForegroundColor.B;

                    //Background Color
                    vertices[7 + i * 40] = characterMatrix[x, y].BackgroundColor.R;
                    vertices[8 + i * 40] = characterMatrix[x, y].BackgroundColor.G;
                    vertices[9 + i * 40] = characterMatrix[x, y].BackgroundColor.B;


                    //Coord 1
                    vertices[10 + i * 40] = x * charset.CharWidth; //0
                    vertices[11 + i * 40] = y * charset.CharHeight + charset.CharHeight; //1

                    vertices[12 + i * 40] = characterMatrix[x, y].TextureCoords.X;
                    vertices[13 + i * 40] = characterMatrix[x, y].TextureCoords.Y + th;

                    //Foreground Color
                    vertices[14 + i * 40] = characterMatrix[x, y].ForegroundColor.R;
                    vertices[15 + i * 40] = characterMatrix[x, y].ForegroundColor.G;
                    vertices[16 + i * 40] = characterMatrix[x, y].ForegroundColor.B;

                    //Background Color
                    vertices[17 + i * 40] = characterMatrix[x, y].BackgroundColor.R;
                    vertices[18 + i * 40] = characterMatrix[x, y].BackgroundColor.G;
                    vertices[19 + i * 40] = characterMatrix[x, y].BackgroundColor.B;


                    //Coord 2
                    vertices[20 + i * 40] = x * charset.CharWidth + charset.CharWidth; //1
                    vertices[21 + i * 40] = y * charset.CharHeight + charset.CharHeight; //1

                    vertices[22 + i * 40] = characterMatrix[x, y].TextureCoords.X + tw; ;
                    vertices[23 + i * 40] = characterMatrix[x, y].TextureCoords.Y + th;

                    //Foreground Color
                    vertices[24 + i * 40] = characterMatrix[x, y].ForegroundColor.R;
                    vertices[25 + i * 40] = characterMatrix[x, y].ForegroundColor.G;
                    vertices[26 + i * 40] = characterMatrix[x, y].ForegroundColor.B;

                    //Background Color
                    vertices[27 + i * 40] = characterMatrix[x, y].BackgroundColor.R;
                    vertices[28 + i * 40] = characterMatrix[x, y].BackgroundColor.G;
                    vertices[29 + i * 40] = characterMatrix[x, y].BackgroundColor.B;


                    //Coord 3
                    vertices[30 + i * 40] = x * charset.CharWidth + charset.CharWidth; //1
                    vertices[31 + i * 40] = y * charset.CharHeight; //0

                    vertices[32 + i * 40] = characterMatrix[x, y].TextureCoords.X + tw;
                    vertices[33 + i * 40] = characterMatrix[x, y].TextureCoords.Y;

                    //Foreground Color
                    vertices[34 + i * 40] = characterMatrix[x, y].ForegroundColor.R;
                    vertices[35 + i * 40] = characterMatrix[x, y].ForegroundColor.G;
                    vertices[36 + i * 40] = characterMatrix[x, y].ForegroundColor.B;

                    //Background Color
                    vertices[37 + i * 40] = characterMatrix[x, y].BackgroundColor.R;
                    vertices[38 + i * 40] = characterMatrix[x, y].BackgroundColor.G;
                    vertices[39 + i * 40] = characterMatrix[x, y].BackgroundColor.B;
                    #endregion
                }
            }
        }

        public int Left
        {
            get { return this.left; }
            set
            {
                this.left = value;

                if (this.left < 0)
                    this.left = 0;
                else if (this.left >= this.bufferWidth)
                {
                    this.left = 0;
                    this.top++;
                }
            }
        }
        public int Top
        {
            get { return this.top; }
            set
            {
                this.top = value;

                if (this.top < 0)
                    this.top = 0;
                else if (this.top >= this.bufferHeight)
                    this.top = this.bufferHeight - 1;
            }
        }
        public int BufferWidth { get { return this.bufferWidth; } }
        public int BufferHeight { get { return this.bufferHeight; } }
        public Color4 ForegroundColor { get { return this.foregroundColor; } }
        public Color4 BackgroundColor { get { return this.backgroundColor; } }

        public DrawingUtilities Draw { get { return this.drawingUtilities; } }
    }

    public struct CharToken
    {
        public char Token;
        public Vector2 TextureCoords;
        public int X, Y;
        public Color4 ForegroundColor;
        public Color4 BackgroundColor;

        public override string ToString()
        {
            return string.Format("{0} - ({1}, {2})", Token, X, Y);
        }
    }
}
