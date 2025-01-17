using HYPERMAGE.Helpers;

namespace HYPERMAGE.Particles
{
    // from https://github.com/LubiiiCZ/DevQuickie/tree/master/Quickie003-ParticleSystem
    public struct ParticleData
    {
        private static Texture2D defaultTexture;
        public Texture2D texture = defaultTexture ??= Globals.Content.Load<Texture2D>("particle");
        public float lifespan = 2f;
        public Color colorStart = Color.Yellow;
        public Color colorEnd = Color.Red;
        public float opacityStart = 1f;
        public float opacityEnd = 0f;
        public float sizeStart = 32f;
        public float sizeEnd = 4f;
        public Vector2 velocity = Vector2.Zero;
        public float resistance = 1.15f;
        public float rotation = 0f;
        public float rotationSpeed = 0f;

        public ParticleData()
        {
        }
    }
}