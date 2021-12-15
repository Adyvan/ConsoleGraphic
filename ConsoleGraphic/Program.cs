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
            uint fixedFps = 50;
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


            while (key.Key != ConsoleKey.Escape)
            {
                Vec3 light = new Vec3(MathF.Sin(frame * 0.01f) - 0.5f, -1, MathF.Cos(frame * 0.01f) - 0.5f).Normalize;

                for (int l = 0; l < Height ; l++)
                {
                    for (int c = 0; c < Width; c++)
                    {
                        uv = new Vec2(c, l) / new Vec2(Width, Height) * 2f - new Vec2(1);
                        uv.X *= aspect;

                        var viewPlanePoint = viewPlaneOX * uv.X + viewPlaneOY * uv.Y + viewPlanePosition;
                        var rayForPoint = (viewPlanePoint - camPos).Normalize;

                        Vec2 intersection = Sphere(camPos, rayForPoint, 1);

                        if(intersection.X > 0)
                        {
                            Vec3 itPoint = camPos + rayForPoint * intersection.X;
                            var diff = itPoint.Normalize.Dot(light);
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

        private static Vec2 Sphere(Vec3 cameraPos, Vec3 ray, float radius)
        {
            var b = cameraPos.Dot(ray);
            var c = cameraPos.SqrLenght - radius * radius;
            var h = b * b - c;

            if (h < 0)
            {
                return new Vec2(-1);
            }

            h = MathF.Sqrt(h);
            return new Vec2(-b - h, -b + h);
        }
    }
}
