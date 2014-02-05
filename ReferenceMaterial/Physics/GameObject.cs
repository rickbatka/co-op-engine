using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ReferenceMaterial.Physics
{
    /// <summary>
    /// stubbed
    /// </summary>
    class GameObject
    {
        public PhysicsBase PhysicsComponent;
    }

    class PhysicsBase
    {
        public Vector2 Position;
        public Rectangle BoundryBox;
    }

    class CollidablePhysics : PhysicsBase
    {
        public ElasticQuadTree currentQuad;
    }
}
