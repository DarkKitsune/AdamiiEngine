using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Adamii_Engine
{
    class VertexBuffer : ArrayBuffer
    {
        public int Count { get; private set; }

        public VertexBuffer() : base()
        {
            
        }

        public void BufferData(Vertex[] vertices)
        {
            Count = vertices.Length;
            BufferData(vertices, Vertex.Size * vertices.Length);
            GLDebug.CheckErrors();
        }
    }
}
