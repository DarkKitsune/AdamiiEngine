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
    struct Vertex
    {
        public const int PositionSize = 3 * 4;
        public const int UVSize = 2 * 4;
        public const int ColorSize = 4 * 4;
        public const int Size = PositionSize + UVSize + ColorSize;

        public Vector3 Position;
        public Vector2 uV;
        public Color4 Color;

        public Vertex(Vector3 pos, Vector2 uv, Color4 color)
        {
            Position = pos;
            uV = uv;
            Color = color;
        }
    }
}
