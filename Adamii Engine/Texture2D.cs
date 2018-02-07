using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Adamii_Engine
{
    class Texture2D : IBindableGLObject, IContent
    {
        public enum FilterType : int
        {
            Linear = TextureMinFilter.Linear,
            Point = TextureMinFilter.Nearest
        }

        public int ID { get; set; }
        public Size Size { get; private set; }
        public FilterType MinFilter
        {
            set
            {
                Bind();

                GL.TexParameter(
                    TextureTarget.Texture2D,
                    TextureParameterName.TextureMinFilter, (int)value
                );
            }
        }
        public FilterType MagFilter
        {
            set
            {
                Bind();

                GL.TexParameter(
                    TextureTarget.Texture2D,
                    TextureParameterName.TextureMagFilter, (int)value
                );
            }
        }

        public Texture2D(Bitmap bitmap)
        {
            ID = GL.GenTexture();

            SetData(bitmap);
            MinFilter = FilterType.Linear;
            MagFilter = FilterType.Linear;
        }

        public void SetData(Bitmap bitmap)
        {
            Bind();

            Size = bitmap.Size;

            var data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                bitmap.Width,
                bitmap.Height,
                0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0
                );
            GLDebug.CheckErrors();

            bitmap.UnlockBits(data);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Dispose()
        {
            GL.DeleteTexture(ID);
        }

        public static Texture2D FromBitmap(Bitmap bitmap)
        {
            return new Texture2D(bitmap);
        }
        public static Texture2D FromStream(Stream stream)
        {
            using (var bm = new Bitmap(stream))
                return FromBitmap(bm);
        }
        public static Texture2D FromFile(string path)
        {
            using (var stream = File.OpenRead(path))
                return FromStream(stream);
        }
    }
}
