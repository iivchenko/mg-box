using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Leaderboards;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Messaging;
using KenneyAsteroids.Engine.Screens;
using System;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayShipAsteroidCollideEventHandler :
        IMessageHandler<GamePlayEntitiesCollideEvent<Ship, Asteroid>>
    {
        private readonly GamePlayHud _hud;
        private readonly IViewport _viewport;
        private readonly IEntitySystem _entities;
        private readonly LeaderboardsManager _leaderBoard;

        public GamePlayShipAsteroidCollideEventHandler(
            GamePlayHud hud,
            IViewport viewport,
            IEntitySystem entities,
            LeaderboardsManager leaderBoard)
        {
            _hud = hud;
            _viewport = viewport;
            _entities = entities;
            _leaderBoard = leaderBoard;
        }

        public void Handle(GamePlayEntitiesCollideEvent<Ship, Asteroid> message)
        {
            var ship = message.Body1;
            var ateroid = message.Body2;

            if (_hud.Lifes > 0)
            {
                _hud.Lifes--;
                ship.Position = new Vector2(_viewport.Width / 2, _viewport.Height / 2);
            }
            else
            {
                _entities.Remove(ship);

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

            msg.Accepted += (_, __) => LoadingScreen.Load(GameRoot.ScreenManager, false, null, new GamePlayScreen());
            msg.Cancelled += (_, __) => LoadingScreen.Load(GameRoot.ScreenManager, false, null, new MainMenuScreen());

            GameRoot.ScreenManager.AddScreen(msg, null);
        }
    }
}
