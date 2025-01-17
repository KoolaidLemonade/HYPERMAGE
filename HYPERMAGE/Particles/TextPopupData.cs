using HYPERMAGE.Helpers;

namespace HYPERMAGE.Particles
{
    public struct TextPopupData
    {
        public string text = " ";
        public float lifespan = 2f;
        public Color colorStart = Color.White;
        public Color colorEnd = Color.White;
        public float opacityStart = 1f;
        public float opacityEnd = 0f;
        public float sizeStart = 5f;
        public float sizeEnd = 2f;
        public Vector2 velocity = Vector2.Zero;
        public float resistance = 1.15f;
        public float rotation = 0f;
        public float rotationSpeed = 0f;
        public float rotationResistance = 1.1f;

        public TextPopupData()
        {
        }
    }
}