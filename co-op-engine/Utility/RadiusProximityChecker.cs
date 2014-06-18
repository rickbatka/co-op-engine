using co_op_engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public class RadiusProximityChecker
    {
        private GameObject Owner;
        private int Radius;

        public RectangleFloat DrawArea { get { return new RectangleFloat(Owner.Position.X - Radius, Owner.Position.Y - Radius, Radius * 2, Radius * 2); } }

        public RadiusProximityChecker(GameObject owner, int radius = 250)
        {
            Owner = owner;
            Radius = radius;
        }

        public List<GameObject> QueryRange()
        {
            var colliders = Owner.CurrentQuad.MasterQuery(DrawArea);
            List<GameObject> results = new List<GameObject>();

            foreach (var collider in colliders)
            {
                if (collider != Owner
                    && IsWithinRadius(collider))
                {
                    results.Add(collider);
                }
            }

            return results;
        }

        private bool IsWithinRadius(GameObject collider)
        {
            return (collider.Position - Owner.Position).Length() <= Radius;
        }
    }
}
