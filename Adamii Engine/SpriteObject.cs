using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Adamii_Engine
{
    class SpriteObject : PrimitiveObject
    {
        public override Sprite Sprite
        {
            get
            {
                return _Sprite;
            }
            set
            {
                _Sprite = value;
                ScaleMultiplier = new Vector3(value.Size.X, value.Size.Y, 1f);
            }
        }

        public SpriteObject(Sprite sprite, Vector3 pos) : base(Primitive.Square1x1, sprite, pos)
        {
            Primitive = Primitive.Square1x1;
            Sprite = sprite;
        }
    }
}
