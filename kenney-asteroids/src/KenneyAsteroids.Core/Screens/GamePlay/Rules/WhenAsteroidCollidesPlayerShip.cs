using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Leaderboards;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Messaging;
using KenneyAsteroids.Engine.Screens;
using System;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay.Rules
{
    public static class WhenAsteroidCollidesPlayerShip
    {
        public static class AndPlayerShipHasEnoughLifes
        {
            public sealed class ThenReduceLifes : IMessageHandler<GamePlayEntitiesCollideEvent<Ship, Asteroid>>
            {
                private readonly GamePlayHud _hud;

                public ThenReduceLifes(GamePlayHud hud)
                {
                    _hud = hud;
                }

                public void Execute(GamePlayEntitiesCollideEvent<Ship, Asteroid> message)
                {
                    if (_hud.Lifes > 0) _hud.Lifes--;
                }
            }

            public sealed class ThenResetPlayersShip : IMessageHandler<GamePlayEntitiesCollideEvent<Ship, Asteroid>>
            {
                private readonly GamePlayHud _hud;
                private readonly IViewport _viewport;

                public ThenResetPlayersShip(
                    GamePlayHud hud,
                    IViewport viewport)
                {
                    _hud = hud;
                    _viewport = viewport;
                }

                public void Execute(GamePlayEntitiesCollideEvent<Ship, Asteroid> message)
                {
                    var ship = message.Body1;

                    if (_hud.Lifes > 0)
                    {
                        ship.Position = new Vector2(_viewport.Width / 2, _viewport.Height / 2);
                    }
                }
            }
        }

        public static class AndPlayerShipDoesntHaveEnoughLifes
        {
            public sealed class ThenRemovePlayersShipFromTheGame : IMessageHandler<GamePlayEntitiesCollideEvent<Ship, Asteroid>>
            {
                private readonly GamePlayHud _hud;
                private readonly IEntitySystem _entities;

                public ThenRemovePlayersShipFromTheGame(
                    GamePlayHud hud,
                    IEntitySystem entities)
                {
                    _hud = hud;
                    _entities = entities;
                }

                public void Execute(GamePlayEntitiesCollideEvent<Ship, Asteroid> message)
                {
                    var ship = message.Body1;

                    if (_hud.Lifes <= 0) _entities.Remove(ship);
                }
            }

            public sealed class ThenGameOver : IMessageHandler<GamePlayEntitiesCollideEvent<Ship, Asteroid>>
            {
                private readonly GamePlayHud _hud;
                private readonly LeaderboardsManager _leaderBoard;

                public ThenGameOver(
                    GamePlayHud hud,
                    LeaderboardsManager leaderBoard)
                {
                    _hud = hud;
                    _leaderBoard = leaderBoard;
                }

                public void Execute(GamePlayEntitiesCollideEvent<Ship, Asteroid> message)
                {
                    var ship = message.Body1;

                    if (_hud.Lifes <= 0)
                    {
                        var playedTime = DateTime.Now - _hud.StartTime;

                        if (_leaderBoard.CanAddLeader(_hud.Scores))
                        {
                            var newHigthScorePrompt = new PromptScreen("Congratulations, you made new high score!\nEnter you name:");

                            newHigthScorePrompt.Accepted += (_, __) =>
                            {
                                _leaderBoard.AddLeader(newHigthScorePrompt.Text, _hud.Scores, playedTime);
                                GameOverMessage();
                            };
                            newHigthScorePrompt.Cancelled += (_, __) => GameOverMessage();

                            GameRoot.ScreenManager.AddScreen(newHigthScorePrompt, null);
                        }
                        else
                        {
                            GameOverMessage();
                        }
                    }
                }

                private void GameOverMessage()
                {
                    const string message = "GAME OVER?\nA button, Space, Enter = Restart\nB button, Esc = Exit";
                    var msg = new MessageBoxScreen(message);

                    msg.Accepted += (_, __) => LoadingScreen.Load(GameRoot.ScreenManager, false, null, new StarScreen(), new GamePlayScreen());
                    msg.Cancelled += (_, __) => LoadingScreen.Load(GameRoot.ScreenManager, false, null, new StarScreen(), new MainMenuScreen());

                    GameRoot.ScreenManager.AddScreen(msg, null);
                }
            }
        }
    }
}
