﻿using HYPERMAGE.Helpers;
using HYPERMAGE.Models;

namespace HYPERMAGE.Particles
{
    // from https://github.com/LubiiiCZ/DevQuickie/tree/master/Quickie003-ParticleSystem
    public struct ParticleData
    {
        private static Texture2D defaultTexture;
        public Texture2D texture = defaultTexture ??= Globals.Content.Load<Texture2D>("particle");
        public Animation anim = null;
        public float lifespan = 2f;
        public Color colorStart = Color.White;
        public Color colorEnd = Color.White;
        public float opacityStart = 1f;
        public float opacityEnd = 1f;
        public float sizeStart = 1;
        public float sizeEnd = 1;
        public Vector2 velocity = Vector2.Zero;
        public float resistance = 10f;
        public float rotation = 0f;
        public float rotationSpeed = 0f;
        public bool flashing = false;
        public bool fastScale = false;
        public bool friendly = true;

        public bool spawnIndicator = false;
        public bool manaDrop = false;
        public ParticleData()
        {
        }
    }
}