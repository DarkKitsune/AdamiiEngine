using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Adamii_Engine
{
    class Sprite
    {
        public Texture2D Texture;
        public Vector2 Size;
        public List<Vector2> Positions;

        public Sprite(Texture2D tex, Vector2 size)
        {
            Texture = tex;
            Size = size;
            Positions = new List<Vector2>();
        }

        public Sprite(Texture2D tex, Vector2 size, params Vector2[] positions)
        {
            Texture = tex;
            Size = size;
            Positions = new List<Vector2>(positions);
        }
    }
}
