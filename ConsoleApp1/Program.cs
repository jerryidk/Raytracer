using System;
using System.Drawing;
using System.IO;
namespace ConsoleApp1
{
    class Program
    {
        const double ratio = 16.0 / 9.0;
        static void Main(string[] args)
        {
            //file path
            string filepath = @"C:\Users\jerry\Desktop\Image.txt";
            //Image Specs
            int width = 400;
            int height = (int)(width / ratio);
            //Camera Specs
            Vec3 origin = new Vec3(0,0,0);
            Vec3 lookAt = new Vec3(0, 0, -1);
            double focal = -1;
            double vpHeight = 2;
            double vpWidth = vpHeight * ratio;
            Camera cam = new Camera(origin,lookAt,focal, vpHeight, vpWidth);
            //Sphere Specs
            Vec3 sLoc = new Vec3(0, 0, -2);
            double radius = 1.0;
            Sphere s = new Sphere(sLoc, radius);

            CreateImage(filepath, s, cam, width, height);
        }

        /// <summary>
        /// AKA viewport, This image should have location of cam's LookAt
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="s"></param>
        /// <param name="cam"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void CreateImage(string filepath, Sphere s, Camera cam, int imgWidth, int imgHeight )
        {
            using (StreamWriter outputFile = new StreamWriter(filepath))
            {
                Vec3 horizontal = new Vec3(cam.VPwidth, 0, 0);
                Vec3 vertical = new Vec3(0, cam.VPheight, 0);
                Vec3 lower_left_corner = cam.Location - horizontal * (1.0 / 2.0)  - vertical * (1.0 / 2.0) - new Vec3(0, 0, cam.FocalLength);
                outputFile.WriteLine("P3");
                outputFile.WriteLine(imgWidth + " " + imgHeight);
                outputFile.WriteLine("255");
                for (int i = 0; i < imgHeight; i++)
                    for (int j = 0; j < imgWidth; j++)
                    {
                        double v = 1.0 * i / (imgHeight - 1.0);
                        double u = 1.0 * j / (imgWidth - 1.0);
                        Vec3 point = lower_left_corner + u * horizontal + v * vertical - cam.Location;// Translating raster space into world space
                        Ray r = cam.ShootRay(point);//generating rays 
                        Vec3 color = new Vec3(0,0,0);//Color Black as background 
                        if(HasHit( s,  r))
                        {
                            color = new Vec3(212,25,25);
                        }
                        outputFile.Write( color.X + " " + color.Y + " " + color.Z + " \n" ); 
                    }
             }
            Console.WriteLine("Done");
            Console.Read();

        }

        private static bool HasHit(Sphere sphere, Ray ray )
        {
            Vec3 extra = ray.Origin - sphere.Center;
            double a = Vec3.Dot(ray.Dir, ray.Dir);
            double b = 2 * Vec3.Dot(ray.Dir, extra);
            double c = Vec3.Dot(extra, extra) - sphere.Radius * sphere.Radius ;

            return (b * b - 4 * a * c) > 0; // Check determinant > 0
        }
    }

    class Ray
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

        
    }

    class Sphere
    {
        public Vec3 Center { get; set; }
        public double Radius { get; set; }
        public Sphere(Vec3 center, double radius)
        {
            Center = center;
            Radius = radius;
        }
    }

    class Camera
    {
        public Vec3 Location { get; set; }
        // Unit Vector that gives the direction of the Camera should LookAt from the Location
        public Vec3 LookAt { get; set; }
        public double FocalLength { get; set;}
        
        public double VPheight { get; set; }
        public double VPwidth { get; set; }
        public Camera(Vec3 location, Vec3 lookat, double focalLength, double height, double width)
        {
            Location = location;
            LookAt = lookat;
            FocalLength = focalLength;
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
