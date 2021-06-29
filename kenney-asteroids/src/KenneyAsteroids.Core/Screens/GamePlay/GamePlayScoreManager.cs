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
                Asteroid _ => 10,
                _ => throw new InvalidOperationException($"Can't calculate scores for {entity}")
            };
    }
}
