using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Adamii_Engine
{
    class VertexArray : IBindableGLObject
    {
        static void SetAttributes()
        {
            var slot = 0;

            //position
            GL.EnableVertexAttribArray(slot);
            GL.VertexAttribPointer(slot, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0);
            GLDebug.CheckErrors();
            slot++;

            //uv
            GL.EnableVertexAttribArray(slot);
            GL.VertexAttribPointer(slot, 2, VertexAttribPointerType.Float, false, Vertex.Size, 12);
            GLDebug.CheckErrors();
            slot++;

            //color
            GL.EnableVertexAttribArray(slot);
            GL.VertexAttribPointer(slot, 4, VertexAttribPointerType.Float, false, Vertex.Size, 20);
            GLDebug.CheckErrors();
            slot++;
        }

        public int ID { get; set; }

        public VertexArray(VertexBuffer vbo)
        {
            ID = GL.GenVertexArray();

            Bind();
            vbo.Bind();

            SetAttributes();

            Unbind();
            vbo.Unbind();
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(ID);
        }

        public void Bind()
        {
            GL.BindVertexArray(ID);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }
    }
}
