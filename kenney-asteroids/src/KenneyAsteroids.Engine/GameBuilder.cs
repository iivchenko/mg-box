using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KenneyAsteroids.Engine
{
    public sealed class GameBuilder
    {
        private readonly ServiceCollection _container;
        private readonly GameConfiguration _configuration;

        private GameBuilder()
        {
            _container = new ServiceCollection();
            _configuration = new GameConfiguration();
        }

        public static GameBuilder CreateBuilder()
        {
            return new GameBuilder();
        }

        public GameBuilder WithServices(Action<IServiceCollection> configure)
        {
            configure(_container);

            return this;
        }

        public GameBuilder WithConfiguration(Action<GameConfiguration> configure)
        {
            configure(_configuration);

            return this;
        }

        public Game Build<TScreen>()
            where TScreen : GameScreen, new()
        {
            return new Game(_container, _configuration, new TScreen());
        }
    }
}
