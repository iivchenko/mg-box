using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Rules;
using System;
using System.Linq;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay.Rules
{
    public static class GamePlayRules
    {
        public static class WhenTimeComes
        {
            public sealed class ThenCreateAsteroid : IRule<OnTimerEvent>
            {
                private readonly IEntitySystem _entities;
                private readonly IViewport _viewport;
                private readonly IEntityFactory _entityFactory;
                private readonly Random _random;

                public ThenCreateAsteroid(
                    IEntitySystem entities,
                    IViewport viewport,
                    IEntityFactory entityFactory)
                {
                    _entities = entities;
                    _viewport = viewport;
                    _entityFactory = entityFactory;

                    _random = new Random();
                }

                public bool ExecuteCondition(OnTimerEvent @event) => @event.Timer.Tags.Contains(GameTags.NextAsteroid);

                public void ExecuteAction(OnTimerEvent @event)
                {
                    var x = 0;
                    var y = 0;

                    switch (_random.Next(0, 4))
                    {
                        case 0: // Up -> Down
                            x = _random.Next(0, (int)_viewport.Width);
                            y = 0;
                            break;

                        case 1: // Right -> Left
                            x = (int)_viewport.Width;
                            y = _random.Next(0, (int)_viewport.Height);
                            break;

                        case 2: // Down -> UP
                            x = _random.Next(0, (int)_viewport.Width);
                            y = (int)_viewport.Height;
                            break;

                        case 3: // Left -> Right
                            x = 0;
                            y = _random.Next(0, (int)_viewport.Height);
                            break;
                    }

                    var position = new Vector2(x, y);
                    var direction = _random.Next(0, 360).AsRadians();
                    var type = new[] { AsteroidType.Tiny, AsteroidType.Small, AsteroidType.Medium, AsteroidType.Big }.RandomPick();
                    var asteroid = _entityFactory.CreateAsteroid(type, position, direction);

                    _entities.Add(asteroid);
                }
            }

            public sealed class ThenDecreaseAsteroidTimeout : IRule<OnTimerEvent>
            {
                private readonly IEntitySystem _entities;
                private readonly IEventPublisher _publisher;

                public ThenDecreaseAsteroidTimeout(
                    IEntitySystem entities,
                    IEventPublisher publisher)
                {
                    _entities = entities;
                    _publisher = publisher;
                }

                public bool ExecuteCondition(OnTimerEvent @event) => @event.Timer.Tags.Contains(GameTags.NextAsteroidLimitChange);

                public void ExecuteAction(OnTimerEvent @event)
                {
                    var timer = _entities.First(timer => timer.Tags.Contains(GameTags.NextAsteroid)) as Timer;                    

                    if (timer.Timeout.TotalSeconds > 0.3)
                    {
                        _entities.Remove(timer);
                        var newTimer = new Timer(TimeSpan.FromSeconds(timer.Timeout.TotalSeconds - 0.2), GameTags.NextAsteroid, _publisher);

                        _entities.Add(newTimer);
                    }
                    else
                    {
                        _entities.Remove(@event.Timer);
                    }
                }
            }

            public sealed class ThenCreateHazardousSituation : IRule<OnTimerEvent>
            {
                private readonly IEntitySystem _entities;
                private readonly IViewport _viewport;
                private readonly IEntityFactory _entityFactory;

                public ThenCreateHazardousSituation(
                    IEntitySystem entities,
                    IViewport viewport,
                    IEntityFactory entityFactory)
                {
                    _entities = entities;
                    _viewport = viewport;
                    _entityFactory = entityFactory;
                }

                public bool ExecuteCondition(OnTimerEvent @event) => @event.Timer.Tags.Contains(GameTags.NextHasardSituation);

                public void ExecuteAction(OnTimerEvent @event)
                {
                    var player  = _entities.First(entity => entity is Ship) as Ship;
                    var target = player.Position;
                    _entities.Add(
                        Create(new Vector2(_viewport.Width / 2, 0), target),
                        Create(new Vector2(_viewport.Width, _viewport.Height / 2), target),
                        Create(new Vector2(_viewport.Width / 2, _viewport.Height), target),
                        Create(new Vector2(0, _viewport.Height / 2), target));
                }

                public Asteroid Create(Vector2 position, Vector2 target)
                {
                    var direction = Vector2.Normalize(target - position).ToRotation();
                    return _entityFactory.CreateAsteroid(AsteroidType.Tiny, position, direction);
                }
            }
        }
    }
}
