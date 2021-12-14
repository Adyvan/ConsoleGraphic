using System;

namespace ConsoleGraphic
{
    static class Program
    {
        const float SymbolAspectRatio = 11f / 24f;
        const int Width = 80;
        const int Height = 24;
        static byte[] screen = new byte[Width * Height];
        static char[] gradient = " .:-=+*#%@".ToCharArray();

        static void Main(string[] args)
        {
            ConsoleKeyInfo key = default;

            var thread = new System.Threading.Thread(()=> {
                while (key.Key != ConsoleKey.Escape)
                {
                    key = Console.ReadKey(false);
                }
            });

            thread.Start();
            var aspect = SymbolAspectRatio * Width / Height;
            Vec2 uv = default;
            while (key.Key != ConsoleKey.Escape)
            {
                for (int l = 0; l < Height ; l++)
                {
                    for (int c = 0; c < Width; c++)
                    {
                        uv = new Vec2(l, c) / new Vec2(Height, Width) * 2f - new Vec2(1);
                        float x = l * 1f / Height * 2f - 1f;
                        float y = c * 1f / Width * 2f - 1f;
                        uv.Y *= aspect;

                        screen[l * Width + c] = (1f - uv.SqrLenght).ToByte();
                    }
                }
                PrintScreen();
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

        private static byte ToByte(this float f)
        {
            return (byte) Math.Clamp(f * byte.MaxValue, byte.MinValue, byte.MaxValue);
        }
    }
}
