using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HYPERMAGE.Helpers;

public static class Globals
{
    public static float TotalSeconds { get; set; }
    public static ContentManager Content { get; set; }
    public static SpriteBatch SpriteBatch { get; set; }
    public static Random Random { get; set; } = new();

    private static Texture2D _blankTexture;

    public static void Update(GameTime gt)
    {
        TotalSeconds = (float)gt.ElapsedGameTime.TotalSeconds;
    }
    public static Texture2D GetBlankTexture()
    {
        if (_blankTexture == null)
        {
            _blankTexture = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1);
            _blankTexture.SetData(new[] { Color.White });
        }
        return _blankTexture;
    }
    public static float RandomFloat(float min, float max)
    {
        return (float)(Random.NextDouble() * (max - min)) + min;
    }

    public static Vector2 RotatedBy(this Vector2 spinningpoint, double radians, Vector2 center = default)
    {
        float num = (float)Math.Cos(radians);
        float num2 = (float)Math.Sin(radians);
        Vector2 vector = spinningpoint - center;
        Vector2 result = center;
        result.X += vector.X * num - vector.Y * num2;
        result.Y += vector.X * num2 + vector.Y * num;
        return result;
    }

    public static bool InBounds(Polygon hitbox)
    {

        float sizeX = hitbox.GetVerticesX().Max<float>() - hitbox.GetVerticesX().Min<float>();
        float sizeY = hitbox.GetVerticesY().Max<float>() - hitbox.GetVerticesY().Min<float>();

        Debug.WriteLine(sizeX);
        Debug.WriteLine(sizeY);

        if (hitbox.IntersectsWith(PolygonFactory.CreateRectangle((int)(0 + sizeX), (int)(33 + sizeY), (int)(320 - sizeX * 2), (int)(180 - 33 - sizeY * 2), 0, Vector2.Zero)))
        {
            return true;
        }

        return false;
    }

    public static float NonLerp(float value1, float value2, float amount)
    {
        return (float)(value1 + (value2 - value1) * Math.Sin(Math.Pow(amount, 2)));
    }
}

