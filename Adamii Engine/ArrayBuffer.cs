using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Adamii_Engine
{
    class ArrayBuffer : IGLBuffer
    {
        public int ID { get; set; }

        public ArrayBuffer()
        {
            ID = GL.GenBuffer();
        }

        public void Dispose()
        {
            GL.DeleteBuffer(ID);
        }

        public void BufferData<T>(T[] array, int size)
            where T : struct
        {
            Bind();
            GL.BufferData<T>(BufferTarget.ArrayBuffer, size, array, BufferUsageHint.StaticDraw);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
