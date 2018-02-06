using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adamii_Engine
{
    interface IBindableGLObject : IGLObject
    {
        void Bind();
        void Unbind();
    }
}
