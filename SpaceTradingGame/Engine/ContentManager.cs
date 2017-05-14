using System;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SpaceTradingGame.Engine.Shaders;

namespace SpaceTradingGame.Engine
{
    public class ContentManager : IDisposable
    {
        private List<int> loadedTextures;

        public ContentManager(GameWindow game)
        {
            this.loadedTextures = new List<int>();
        }

        public void Dispose()
        {
            if (this.loadedTextures.Count > 0)
                this.UnloadTextures();
        }

        public int LoadTexture(string filepath)
        {
            if (String.IsNullOrEmpty(filepath) || !System.IO.File.Exists(filepath))
                throw new ArgumentException(String.Format("Asset file '{0}' is missing!", filepath));

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Bitmap bitmap = new Bitmap(filepath);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            this.loadedTextures.Add(id);

            return id;
        }
        public void UnloadTextures()
        {
            for (int i = 0; i < this.loadedTextures.Count; i++)
                GL.DeleteTexture(this.loadedTextures[i]);
        }

        public string StreamShaderSource(string resourceName)
        {
            //Returns shader source from embedded resource file
            Assembly assembly = Assembly.GetExecutingAssembly();

            string shaderSource = String.Empty;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    shaderSource = reader.ReadToEnd();
                }
            }

            return shaderSource;
        }
        public string LoadShaderSource(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
