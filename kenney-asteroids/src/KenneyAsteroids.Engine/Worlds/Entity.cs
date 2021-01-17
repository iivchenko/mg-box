using Microsoft.Xna.Framework;

namespace KenneyAsteroids.Engine.Worlds
{
    /* TODO: Refine Entiy in the future
     * Provide Rotation property
     */
    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
