/*
    Written by Tobias Lenz
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helper.Primitive
{
    /// <summary>
    /// Simple Circle implementation
    /// </summary>
    public class Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        /// <summary>
        /// Circle Constructor
        /// </summary>
        /// <param name="center">Center Point</param>
        /// <param name="radius">Circle Radius</param>
        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Checks for overlapping with the provided rectangle
        /// </summary>
        /// <param name="other">Rectangle to check against</param>
        /// <returns>true if they overlap</returns>
        public bool Overlaps(Rect other)
        {
            Vector2 circleDistance = Vector2.zero;
            circleDistance.x = Math.Abs(Center.x - other.x);
            circleDistance.y = Math.Abs(Center.y - other.y);

            if (circleDistance.x > (other.width / 2 + Radius)) { return false; }
            if (circleDistance.y > (other.height / 2 + Radius)) { return false; }

            if (circleDistance.x <= (other.width / 2)) { return true; }
            if (circleDistance.y <= (other.height / 2)) { return true; }

            double cornerDistance_sq = Math.Pow((circleDistance.x - other.width / 2), 2) +
                                 Math.Pow((circleDistance.y - other.height / 2), 2);

            return (cornerDistance_sq <= Math.Pow(Radius, 2));
        }

        /// <summary>
        /// Checks for overlapping with the provided circle
        /// </summary>
        /// <param name="other">Circle to check against</param>
        /// <returns>true if they overlap</returns>
        public bool Overlaps(Circle other)
        {
            // Find the distance between the centers.
            float dx = Center.x - other.Center.x;
            float dy = Center.y - other.Center.y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            
            // See how many solutions there are.
            if (dist > Radius + other.Radius)
            {
                // the circles are too far apart.
                return false;
            }
            else if (dist < Math.Abs(Radius - other.Radius))
            {
                // one circle contains the other.
                return true;
            }
            else if ((dist == 0) && (Radius == other.Radius))
            {
                // the circles coincide.
                return true;
            }
            else
            {
                // the circles intersect
                return true;
            }
        }
    }
}
