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

    private static Texture2D blankTexture;
    public static Vector2 DirectionTo(this Vector2 origin, Vector2 target) => Vector2.Normalize(target - origin);

    public static float ToRotation(this Vector2 vector2) => (float)Math.Atan2(vector2.Y, vector2.X);
    public static float Distance(Vector2 pos1, Vector2 pos2)
    {
        return (float)Math.Sqrt(Math.Pow(Math.Abs(pos1.X - pos2.X), 2) + Math.Pow(Math.Abs(pos1.Y - pos2.Y), 2));
    }
    public static int GetWeightedRandomInt(List<Vector2> intsProbs)
    {
        List<float> probabilites = [];
        List<int> ints = [];

        float totalProb = 0;
        float rand = RandomFloat(0, 100);

        for (int i = 0; i < intsProbs.Count; i++)
        {
            probabilites.Add(intsProbs[i].Y);
            ints.Add((int)intsProbs[i].X);
        }

        if (probabilites.Sum() != 100)
        {
            return 0;
        }

        for (int i = 0; i < probabilites.Count; i++)
        {
            if (rand < probabilites[i] + totalProb && rand > totalProb)
            {
                Debug.WriteLine(ints[i]);
                return ints[i];
            }

            totalProb += probabilites[i];
        }

        return 0;
    }

    public static SpriteFont GetPixelFont()
    {
        return Content.Load<SpriteFont>("font");
    }

    public static void Update(GameTime gt)
    {
        TotalSeconds = (float)gt.ElapsedGameTime.TotalSeconds;
    }
    public static Texture2D GetBlankTexture()
    {
        if (blankTexture == null)
        {
            blankTexture = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1);
            blankTexture.SetData(new[] { Color.White });
        }

        return blankTexture;
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

        if (hitbox.IntersectsWith(PolygonFactory.CreateRectangle((int)(0 + sizeX), (int)(sizeY), (int)(320 - sizeX * 2), (int)(180 - sizeY * 2), 0, Vector2.Zero)))
        {
            return true;
        }

        return false;
    }
    public static Polygon GetBounds(Polygon hitbox)
    {

        float sizeX = hitbox.GetVerticesX().Max<float>() - hitbox.GetVerticesX().Min<float>();
        float sizeY = hitbox.GetVerticesY().Max<float>() - hitbox.GetVerticesY().Min<float>();

        return PolygonFactory.CreateRectangle((int)(0 + sizeX), (int)(sizeY), (int)(320 - sizeX * 2), (int)(180 - sizeY * 2), 0, Vector2.Zero);
    }
    public static float NonLerp(float value1, float value2, float amount)
    {
        return (float)(value1 + (value2 - value1) * Math.Pow(amount, 2));
    }

    public static List<string> LineBreakText(string text, SpriteFont font, float maxWidth, bool breakOnWord = true)
    {
        List<string> lines = new List<string>();
        string[] words = text.Split(' ');
        string currentLine = "";

        foreach (string word in words)
        {
            string testLine = currentLine.Length > 0 ? currentLine + " " + word : word;
            Vector2 size = font.MeasureString(testLine);

            if (size.X <= maxWidth)
            {
                currentLine = testLine;
            }

            else
            {
                if (breakOnWord)
                {
                    if (currentLine.Length > 0)
                        lines.Add(currentLine);
                    currentLine = word;
                }

                else
                {
                    while (currentLine.Length > 0 && font.MeasureString(currentLine + "-").X > maxWidth)
                    {
                        int lastChar = currentLine.Length - 1;
                        lines.Add(currentLine[..lastChar] + "-");
                        currentLine = currentLine[lastChar..];
                    }

                    currentLine += " ";
                    currentLine += word;
                }
            }
        }

        if (currentLine.Length > 0)
            lines.Add(currentLine);

        return lines;
    }
}

