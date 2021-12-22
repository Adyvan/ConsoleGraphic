using System;
using System.Threading;

namespace ConsoleGraphic
{
    static class Program
    {
        const float SymbolAspectRatio = 11f / 24f;
        const int Width = 80;
        const int Height = 23;
        static byte[] screen = new byte[Width * Height];
        static char[] gradient = " .:-=+*#%@".ToCharArray();

        static void Main(string[] args)
        {
            //for Windows
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(Width, Height);
            }
            //Input
            ConsoleKeyInfo key = default;
            var thread = new Thread(()=> {
                while (key.Key != ConsoleKey.Escape)
                {
                    key = Console.ReadKey(false);
                }
            });
            thread.Start();


            //Render
            uint fixedFps = 10;
            uint msPerFrame = 1000 / fixedFps; 
            uint frame = 0;
            var aspect = SymbolAspectRatio * Width / Height;
            Vec2 uv = default;
            var time = DateTime.Now;
            
            Vec3 camPos = new Vec3(-2, 0, 0);
            Vec3 camDirection = (new Vec3() - camPos).Normalize; // Look at 0,0,0 point
            Vec3 camUp = new Vec3(0, 0, 1);
            var viewPlaneOX = camUp.Cross(camDirection).Normalize;
            var viewPlaneOY = camDirection.Cross(viewPlaneOX).Normalize;
            float camMinDistance = 1f;
            Vec3 viewPlanePosition = camDirection * camMinDistance + camPos;

            var sphareCenter = new Vec3(0);


            while (key.Key != ConsoleKey.Escape)
            {
                Vec3 light = new Vec3(MathF.Sin(frame * 0.01f) - 0.5f, MathF.Cos(frame * 0.01f) + 0.5f, -1).Normalize;

                for (int l = 0; l < Height ; l++)
                {
                    for (int c = 0; c < Width; c++)
                    {
                        uv = new Vec2(c, l) / new Vec2(Width, Height) * 2f - new Vec2(1);
                        uv.X *= aspect;

                        var viewPlanePoint = viewPlaneOX * uv.X + viewPlaneOY * uv.Y + viewPlanePosition;
                        var rayForPoint = (viewPlanePoint - camPos).Normalize;

                        var intersection = MySphare(camPos, rayForPoint, sphareCenter, 1);

                        if(intersection != null)
                        {
                            Vec3 normal = (intersection - sphareCenter).Normalize;
                            Vec3 diffNorm = rayForPoint.Reflect(normal) * -1;
                            var diff = diffNorm.Dot(light);
                            screen[l * Width + c] = diff.ToPixelByte();
                        }
                        else
                        {
                            screen[l * Width + c] = 0;
                        }
                    }
                }
                PrintScreen();
                frame++;
                var delay = DateTime.Now - time;
                Console.Write(delay);
                Console.Write($"planeOX {viewPlaneOX} ");
                Console.Write($"planeOY {viewPlaneOY} ");

                Console.WriteLine();
                if (delay.TotalSeconds < (1f / fixedFps))
                {
                    Thread.Sleep((int)(msPerFrame - delay.TotalMilliseconds));
                }
                time = DateTime.Now;
            }

            thread.Join();
        }

        private static void PrintScreen()
        {
            if (OperatingSystem.IsWindows())
            {
                Console.Clear();
            }
            foreach (var p in screen)
            {
                Console.Write(GetPixelColor(p));
            }
        }
        

        private static char GetPixelColor(byte intensity)
        {
            var index = (int)MathF.Round((gradient.Length - 1f) * intensity / byte.MaxValue);
            return gradient[index];
        }

        private static byte ToPixelByte(this float f)
        {
            return (byte) Math.Clamp(f * byte.MaxValue, byte.MinValue, byte.MaxValue);
        }

        private static Vec3 MySphare(Vec3 camPos, Vec3 ray, Vec3 sphereCenter, float radius)
        {
            var vectorToCenter = camPos - sphereCenter;
            var normal = ray.Cross(vectorToCenter);
            var sqrD = normal.SqrLenght / ray.SqrLenght;
            if(sqrD > radius * radius)
            {
                return null;
            }

            return ray * -MathF.Sqrt(radius * radius - sqrD) + ray * vectorToCenter.Lenght;
        }
    }
}
