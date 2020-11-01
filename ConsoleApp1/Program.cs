using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;

//TODO: Multiple objects -> Create abstract class and World to capsulate Geometries
//TODO: Antialiasing
//TODO: Multiple Material
//TODO: GUI with WindowsForm
namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            //File path
            string filepath = @"C:\Users\jerry\Desktop\Image.txt";
            //Image Specs
            double ratio = 16.0 / 9.0;
            int width = 400;
            int height = (int)(width / ratio);
            //Camera Specs
            Vec3 origin = new Vec3(0, 0, 0);
            Vec3 lookAt = new Vec3(0, 0, -3);
            double vpWidth = 2;
            double vpHeight = vpWidth / ratio;
            Camera cam = new Camera(origin, lookAt, vpHeight, vpWidth);
            //Sphere Specs
            Sphere s1 = new Sphere(new Vec3(0, 0, -5), 1.0);
            Sphere s2 = new Sphere(new Vec3(0, -3, -2), 2.5);

            List<Geometry> geometries = new List<Geometry>();
            geometries.Add(s1);
            geometries.Add(s2);
            //World
            World world = new World(geometries, cam);

            CreateImage(filepath, width, height, world);
        }

        public static void CreateImage(string filepath, int imgWidth, int imgHeight, World world)
        {
            Camera cam = world.cam;
            List<Geometry> geometries = world.geometries;
            using (StreamWriter outputFile = new StreamWriter(filepath))
            {
                outputFile.WriteLine("P3");
                outputFile.WriteLine(imgWidth + " " + imgHeight);
                outputFile.WriteLine("255");
                for (int i = 0; i < imgHeight; i++)
                    for (int j = 0; j < imgWidth; j++)
                    {
                        double v = 1.0 * i / (imgHeight - 1.0);
                        double u = 1.0 * j / (imgWidth - 1.0);
                        Vec3 point = cam.Upper_Left_Corner + u * cam.Horizontal - v * cam.Vertical;//Translating raster space into world space
                        Ray r = cam.ShootRay(point);//Generating rays 
                        Vec3 color = new Vec3(255, 255, 255);//Color White as background 
                        Render(ref color, r, geometries);
                        outputFile.Write(color.X + " " + color.Y + " " + color.Z + " \n");
                    }
            }
        }

        private static void Render(ref Vec3 color, Ray r, List<Geometry> geometries)
        {
            //double closest_point = 100000000; 
            double shortest_time = 10000000;

            foreach (Geometry g in geometries)
            {
                if (g is Sphere)
                {
                    Sphere s = (Sphere)g; 
                    if (s.HasHit(r, out double t, out bool inside))
                    {
                        Vec3 pointOnSphere = r.At(t);
                        //Vec3 coord_to_cam = pointOnSphere - r.Origin;
                        //double distance_to_cam = coord_to_cam.Length;
                        Vec3 surfaceNormal = Vec3.Normalize(pointOnSphere - s.Center);
                        if (t < shortest_time)
                        {
                            shortest_time = t; 
                            if (inside) // ray hits the inside of the Sphere
                            {
                                color = new Vec3(255, 0, 0); //Shading
                            }
                            else
                            {
                                color = Vec3.Round(new Vec3(0.1, 0.1, 0.1) * Math.Abs(Vec3.Dot(surfaceNormal, -r.Dir)) * 255); //Shading
                            }
                        }

                    }
                }
            }
        }
    }


    class World
    {
        // List of geometries
        public List<Geometry> geometries { get; set; }
        // Camera
        public Camera cam { get; set;}

        public World(List<Geometry> geometries, Camera cam)
        {
            this.geometries = geometries;
            this.cam = cam;
        }
    }

    public class Ray
    {
        // Origin should be location of the Camera 
        public Vec3 Origin { get; set; }
        // Dir should be the LookAt of the Camera
        public Vec3 Dir { get; set; }
        public Ray(Vec3 origin, Vec3 direction)
        {
            Origin = origin; 
            Dir = direction;
        }

        /// <summary>
        ///  return a point where this Ray hit the Geometry
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vec3 At(double t)
        {
            return Origin + t * Dir;
        }

    }

    public class Camera
    {
        //Viewport height
        private double VPheight;
        //Viewport width 
        private double VPwidth;
        //Camera location, where ray's origin 
        public Vec3 Location { get; private set; }
        //Middle point of the ViewPort
        public Vec3 LookAt { get; private set; }
        //ViewPort width in vector 
        public Vec3 Horizontal { get { return new Vec3(VPwidth, 0, 0); } }
        //ViewPort height in vector 
        public Vec3 Vertical { get { return new Vec3(0, VPheight, 0); } }
        //Lower left corner of the viewport relative to origin of the world
        public Vec3 Upper_Left_Corner
        {
            get
            {
                return LookAt - Horizontal * (1.0 / 2.0) + Vertical * (1.0 / 2.0) + Location;
            }
        }

        public Camera(Vec3 location, Vec3 lookat, double height, double width)
        {
            Location = location;
            LookAt = lookat;
            VPheight = height;
            VPwidth = width;
        }

        /// <summary>
        /// Return a ray object that from Location to ViewPort, the Ray should have origin of cam's location
        /// Ray should have a direction that from cam's location to a point on ViewPort
        /// </summary>
        public Ray ShootRay(Vec3 point)
        {
            return new Ray(Location, Vec3.Normalize(point - Location));
        }

    }
}
