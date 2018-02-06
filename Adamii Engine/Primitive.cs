using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Adamii_Engine
{
    class Primitive : IDisposable
    {
        public static Primitive Square1x1 = null;
        public static void Init()
        {
            Square1x1 = new Primitive();
            Square1x1.Begin();
            Square1x1.AddVertex(new Vector3(0f, 0f, 0f), new Vector2(0f, 0f), new Color4(1f, 1f, 1f, 1f));
            Square1x1.AddVertex(new Vector3(0f, 1f, 0f), new Vector2(0f, 1f), new Color4(1f, 1f, 1f, 1f));
            Square1x1.AddVertex(new Vector3(1f, 1f, 0f), new Vector2(1f, 1f), new Color4(1f, 1f, 1f, 1f));

            Square1x1.AddVertex(new Vector3(1f, 1f, 0f), new Vector2(1f, 1f), new Color4(1f, 1f, 1f, 1f));
            Square1x1.AddVertex(new Vector3(1f, 0f, 0f), new Vector2(1f, 0f), new Color4(1f, 1f, 1f, 1f));
            Square1x1.AddVertex(new Vector3(0f, 0f, 0f), new Vector2(0f, 0f), new Color4(1f, 1f, 1f, 1f));
            Square1x1.End();
        }

        VertexBuffer VBO;
        VertexArray VAO;

        List<Vertex> Vertices;

        public bool Ready { get; protected set; }
        public bool Creating { get; protected set; }

        public Primitive()
        {
            Vertices = new List<Vertex>();
        }

        public void Dispose()
        {
            VAO?.Dispose();
            VBO?.Dispose();
        }
        
        public void Begin()
        {
            if (Creating)
                return;
            Ready = false;
            Creating = true;
            Vertices.Clear();
        }

        public void AddVertex(Vertex v)
        {
            if (!Creating)
                return;
            Vertices.Add(v);
        }
        public void AddVertex(Vector3 pos, Vector2 uv, Color4 color)
        {
            AddVertex(new Vertex(pos, uv, color));
        }
        public void End()
        {
            if (!Creating)
                return;
            Creating = false;
            Ready = true;

            VBO?.Dispose();
            VAO?.Dispose();
            VBO = new VertexBuffer();
            VBO.BufferData(Vertices.ToArray());
            VAO = new VertexArray(VBO);

            Ready = true;
        }

        public void DrawOne()
        {
            if (!Ready)
                return;

            VAO.Bind();
            VBO.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, Vertices.Count);
            GLDebug.CheckErrors();
            VAO.Unbind();
            VBO.Unbind();
        }

        public void DrawInstanced(int count)
        {
            if (!Ready)
                return;

            VAO.Bind();
            VBO.Bind();
            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, Vertices.Count, count);
            GLDebug.CheckErrors();
            VAO.Unbind();
            VBO.Unbind();
        }
    }
}
