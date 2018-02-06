using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Adamii_Engine
{
    class GameObject
    {
        public bool Deleted { get; private set; }
        Vector3 _Position;
        public virtual Vector3 Position { get; set; }
        GameObjectLayer _Layer;
        public GameObjectLayer Layer
        {
            get
            {
                return _Layer;
            }
            set
            {
                Layer?.Remove(this);
                value?.Add(this);
                _Layer = value;
            }
        }
        public virtual void Update()
        {
            
        }
        public virtual void Delete()
        {
            _Layer = null;
            Deleted = true;
        }
    }
}
