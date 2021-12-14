using System;
namespace ConsoleGraphic
{
    public class Vec2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float SqrLenght => X * X + Y * Y;
        public float Lenght => MathF.Sqrt(X * X + Y * Y);
        public Vec2 Normalize => new Vec2(X, Y) / Lenght;

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vec2(float xy)
        {
            X = xy;
            Y = xy;
        }

        public Vec2() : this(0)
        {
        }

        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
        public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.X * b.X, a.Y * b.Y);
        public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.X / b.X, a.Y / b.Y);
        public static Vec2 operator *(Vec2 a, float f) => new Vec2(a.X * f, a.Y * f);
        public static Vec2 operator /(Vec2 a, float f) => new Vec2(a.X / f, a.Y / f);
    }

    public class Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float SqrLenght => X * X + Y * Y + Z * Z;
        public float Lenght => MathF.Sqrt(X * X + Y * Y + Z * Z);
        public Vec3 Normalize => new Vec3(X,Y,Z) / Lenght;

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3(float xyz)
        {
            X = xyz;
            Y = xyz;
            Z = xyz;
        }

        public Vec3() : this(0)
        {
        }

        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vec3 operator *(Vec3 a, Vec3 b) => new Vec3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Vec3 operator /(Vec3 a, Vec3 b) => new Vec3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        public static Vec3 operator *(Vec3 a, float f) => new Vec3(a.X * f, a.Y * f, a.Z * f);
        public static Vec3 operator /(Vec3 a, float f) => new Vec3(a.X / f, a.Y / f, a.Z / f);
    }
}
