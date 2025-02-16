using System.Collections.Generic;
using HYPERMAGE.Helpers;

namespace HYPERMAGE.Models;

public class Animation
{
    private readonly Texture2D texture;
    private readonly List<Rectangle> sourceRectangles = [];
    private readonly int frames;
    private int frame;
    private readonly float frameTime;
    private float frameTimeLeft;
    private bool active = true;

    public int frameWidth;
    public int frameHeight;

    public Animation(Texture2D texture, int framesX, int framesY, float frameTime, int row = 1)
    {
        this.texture = texture;
        this.frameTime = frameTime;
        frameTimeLeft = frameTime;
        frames = framesX;
        frameWidth = texture.Width / framesX;
        frameHeight = texture.Height / framesY;

        for (int i = 0; i < frames; i++)
        {
            sourceRectangles.Add(new(i * frameWidth, (row - 1) * frameHeight, frameWidth, frameHeight));
        }
    }

    public void Stop()
    {
        active = false;
    }

    public void Start()
    {
        active = true;
    }

    public void Reset()
    {
        frame = 0;
        frameTimeLeft = frameTime;
    }

    public void Update()
    {
        if (!active) return;

        frameTimeLeft -= Globals.TotalSeconds;

        if (frameTimeLeft <= 0)
        {
            frameTimeLeft += frameTime;
            frame = (frame + 1) % frames;
        }
    }

    public void Draw(Vector2 pos, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffect, float layerDepth)
    {
        Globals.SpriteBatch.Draw(texture, pos, sourceRectangles[frame], color, rotation, origin, scale, spriteEffect, layerDepth);
    }
}