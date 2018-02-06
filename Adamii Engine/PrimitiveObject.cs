using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Adamii_Engine
{
    class PrimitiveObject : GameObject
    {
        public Primitive Primitive;
        protected Sprite _Sprite;
        public virtual Sprite Sprite
        {
            get
            {
                return _Sprite;
            }
            set
            {
                if (value != _Sprite)
                    Frame = 0;
                _Sprite = value;
            }
        }
        float _Frame;
        public float Frame
        {
            get
            {
                return _Frame;
            }
            set
            {
                _Frame = value % (float)Sprite.Positions.Count;
                if (_Frame < 0f)
                    _Frame += (float)Sprite.Positions.Count;
            }
        }

        public float AnimationFPS = 0f;

        public Vector3 Scale = Vector3.One;
        public Vector3 ScaleMultiplier { get; protected set; } = Vector3.One;

        public PrimitiveObject(Primitive p, Sprite sprite, Vector3 pos)
        {
            Primitive = p;
            Sprite = sprite;
            Position = pos;
        }


        public override void Update()
        {
            if (AnimationFPS != 0f && Sprite.Positions.Count > 1)
            {
                Frame += AnimationFPS / (float)Program.Window.UpdateFrequency;
            }

            base.Update();
        }
    }
}
