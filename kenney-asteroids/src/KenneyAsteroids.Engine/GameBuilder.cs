using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KenneyAsteroids.Engine
{
    public sealed class GameBuilder
    {
        private readonly ServiceCollection _container;
        private readonly GameConfiguration _configuration;
        
        private Type _initialScreen;

        private GameBuilder()
        {
            _container = new ServiceCollection();
            _configuration = new GameConfiguration();
        }

        public static GameBuilder CreateBuilder()
        {
            return new GameBuilder();
        }

        public GameBuilder WithServices(Action<ServiceCollection> configure)
        {
            configure(_container);

            return this;
        }

        public GameBuilder WithConfiguration(Action<GameConfiguration> configure)
        {
            configure(_configuration);

            return this;
        }

        public GameBuilder WithInitialScreen<TScreen>()
            where TScreen : GameScreen
        {
            _initialScreen = typeof(TScreen);

            return this;
        }

        public Game Build()
        {
            if (_initialScreen == null)
            {
                throw new InvalidOperationException($"Initial Screen was not set!");
            }

            return new Game(_container, _configuration, _initialScreen);
        }
    }
}
