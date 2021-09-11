using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine.Entities;
using System;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayScoreManager
    {
        public int GetScore(IEntity entity)
            => entity switch
            {
                Asteroid asteroid when asteroid.Type == AsteroidType.Tiny => 10,
                Asteroid asteroid when asteroid.Type == AsteroidType.Small => 15,
                Asteroid asteroid when asteroid.Type == AsteroidType.Medium => 20,
                Asteroid asteroid when asteroid.Type == AsteroidType.Big => 25,
                _ => throw new InvalidOperationException($"Can't calculate scores for {entity}")
            };
    }
}
