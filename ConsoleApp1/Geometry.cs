using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    // All objects 
    public abstract class Geometry
    {
        public abstract bool HasHit(Ray r, out double t, out bool inside);
    }

    public class Sphere : Geometry
    {
        public Vec3 Center { get; set; }
        public double Radius { get; set; }
        public Sphere(Vec3 center, double radius)
        {
            Center = center;
            Radius = radius;
        }
        /// <summary>
        /// Compute if this Sphere get hitted by a Ray and return t as the time
        /// </summary>
        /// <param name="r">Ray</param>
        /// <param name="t">time</param>
        /// <returns></returns>
        public override bool HasHit(Ray r, out double t, out bool inside )
        {
            t = -1;
            inside = false;
            Vec3 extra = r.Origin - Center;
            double a = Vec3.Dot(r.Dir, r.Dir);
            double b = 2 * Vec3.Dot(r.Dir, extra);
            double c = Vec3.Dot(extra, extra) - Radius * Radius;
            double discriminant = b * b - 4 * a * c;
            double t1 = (-b - Math.Sqrt(discriminant)) / (2.0 * a);
            double t2 = (-b + Math.Sqrt(discriminant)) / (2.0 * a);

            if (discriminant < 0) // if determinant is negative, then ray didn't hit object
                return false;

            if (t1 > 0 && t2 > 0)// if both t are positive that means smaller t is hit first
            {
                t = Math.Min(t1, t2);
            }
            else if (t1 < 0 && t2 < 0)// if both t are negative, the object is behind ray
            {
                return false;
            }
            else // one of the t is positive
            {
                inside = true; 
                t = Math.Max(t1, t2);
            }
            return true;
        }
    }
}
