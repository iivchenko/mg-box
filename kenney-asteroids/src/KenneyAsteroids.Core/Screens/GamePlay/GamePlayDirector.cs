using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Messaging;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayDirector : IUpdatable
    {
        private readonly IPublisher _publisher;
        private readonly IOptionsMonitor<AudioSettings> _settings;

        private ContentManager _content;

        private Chapter _current;

        public GamePlayDirector(
            IPublisher publisher,
            IOptionsMonitor<AudioSettings> settings,
            ContentManager content)
        {
            _publisher = publisher;
            _settings = settings;
            _content = content;

            _current = new ChapterBegining(this);
        }

        public void Update(float time)
        {
            _current.Update(time);
        }

        public void Free()
        {
            _current.Free();
        }

        private abstract class Chapter
        {
            public Chapter(GamePlayDirector director)
            {
                Director = director;
            }

            protected GamePlayDirector Director { get; }

            public abstract void Update(float time);

            public abstract void Free();
        }

        private sealed class ChapterBegining : Chapter
        {
            private readonly Song _song;
            private IUpdatable _timer;
            private int _phase;
            private int _maxAsteroids;

            public ChapterBegining(GamePlayDirector director)
                : base(director)
            {
                _phase = 0;
                _maxAsteroids = 2;

                var timer = new Timer(TimeSpan.FromSeconds(3), IncreaseDifficulty, true);
                _song = Director._content.Load<Song>("Music/game1.song");

                MediaPlayer.Volume = Director._settings.CurrentValue.MusicVolume;
                MediaPlayer.Play(_song);
                MediaPlayer.MediaStateChanged += ChapterFinished;

                _timer = timer;
                Director._publisher.Publish(new CreateAsteroidCommand());

                timer.Start();
            }

            private void ChapterFinished(object sender, EventArgs e)
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.MediaStateChanged -= ChapterFinished;
                    Director._current = new ChapterNext(Director);
                }
            }

            private void IncreaseDifficulty(float time)
            {
                Director._publisher.Publish(new CreateAsteroidCommand());
                _maxAsteroids--;

                if (_maxAsteroids <= 0)
                {
                    _timer = new Timer(TimeSpan.FromSeconds(60), t => { }, true);
                }
            }

            public override void Update(float time)
            {
                _timer.Update(time);
                
                if (_phase == 0)
                {
                    var delta = (MediaPlayer.PlayPosition - TimeSpan.FromSeconds(14 + 2)).TotalSeconds;

                    if (delta >= 0.5 && delta <= 1)
                    {
                        _phase = 1;
                        for (var i = 0; i < 10; i++) Director._publisher.Publish(new CreateAsteroidCommand());
                    }
                }
                else if (_phase == 1)
                {
                    var delta = (MediaPlayer.PlayPosition - TimeSpan.FromSeconds(59 + 1)).TotalSeconds;

                    if (delta >= 0.5 && delta <= 1)
                    {
                        _phase = 2;
                        for (var i = 0; i < 10; i++) Director._publisher.Publish(new CreateAsteroidCommand());
                    }
                }
            }

            public override void Free()
            {
                MediaPlayer.MediaStateChanged -= ChapterFinished;
            }
        }

        private sealed class ChapterNext : Chapter
        {
            private readonly Random _random;

            public ChapterNext(GamePlayDirector director)
                : base(director)
            {
                _random = new Random();
                MediaPlayer.Play(Next());
                MediaPlayer.MediaStateChanged += NextSong;
            }

            public override void Free()
            {
                MediaPlayer.MediaStateChanged -= NextSong;
            }

            private void NextSong(object sender, EventArgs e)
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.Play(Next());
                }
            }

            private Song Next()
            {
                var next = _random.Next(4) + 1;
                return Director._content.Load<Song>($"Music/game{next}.song");
            }

            public override void Update(float time)
            {
            }
        }
    }
}
