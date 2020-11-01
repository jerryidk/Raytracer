using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConsoleApp1
{
    /// <summary>
    /// Vector3 math class
    /// </summary>
    public class Vec3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public Vec3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3() : this(0, 0, 0) { }

        //Vector addition v1 + v2 
        public static Vec3 Add(Vec3 v1, Vec3 v2) => new Vec3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        //Operator + overloading 
        public static Vec3 operator +(Vec3 v1, Vec3 v2) => Add(v1, v2);
        //Vector substraction v1 - v2
        public static Vec3 Substract(Vec3 v1, Vec3 v2) => new Vec3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        //Operator - overloading
        public static Vec3 operator -(Vec3 v1, Vec3 v2) => Substract(v1, v2);
        //Operator - overloading negation
        public static Vec3 operator -(Vec3 v) => Substract(new Vec3(), v);
        //Scala multiplication c * v1
        public static Vec3 Multiply(double c, Vec3 v1) => new Vec3(v1.X * c, v1.Y * c, v1.Z * c);
        //Operator overloading
        public static Vec3 operator *(double c, Vec3 v2) => Multiply(c, v2);
        //Operator overloading 
        public static Vec3 operator *( Vec3 v2,double c) => Multiply(c, v2);
        //Normalize v1 
        public static Vec3 Normalize(Vec3 v1) => new Vec3(v1.X / v1.Length, v1.Y / v1.Length, v1.Z / v1.Length);
        //Dot product of two vector 
        public static double Dot(Vec3 v1, Vec3 v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        //Cross product of two vector 
        public static Vec3 Cross(Vec3 v1, Vec3 v2) => new Vec3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        //Round all component input vector to a double integer 
        public static Vec3 Round(Vec3 v) => new Vec3(Math.Round(v.X), Math.Round(v.Y), Math.Round(v.Y));
  
    }
}
