using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Messaging;
using Microsoft.Extensions.Options;
using System;

using XMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;
using XMediaState = Microsoft.Xna.Framework.Media.MediaState;
using XSong = Microsoft.Xna.Framework.Media.Song;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayDirector : IUpdatable
    {
        private readonly IPublisher _publisher;
        private readonly IOptionsMonitor<AudioSettings> _settings;

        private IContentProvider _content;

        private Chapter _current;

        public GamePlayDirector(
            IPublisher publisher,
            IOptionsMonitor<AudioSettings> settings,
            IContentProvider content)
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
            private readonly XSong _song;
            private IUpdatable _timer;
            private int _phase;
            private int _maxAsteroids;

            public ChapterBegining(GamePlayDirector director)
                : base(director)
            {
                _phase = 0;
                _maxAsteroids = 2;

                var timer = new Timer(TimeSpan.FromSeconds(3), IncreaseDifficulty, true);
                _song = Director._content.Load<XSong>("Music/game1.song");

                XMediaPlayer.Volume = Director._settings.CurrentValue.MusicVolume;
                XMediaPlayer.Play(_song);
                XMediaPlayer.MediaStateChanged += ChapterFinished;

                _timer = timer;
                Director._publisher.Publish(new GamePlayCreateAsteroidCommand());

                timer.Start();
            }

            private void ChapterFinished(object sender, EventArgs e)
            {
                if (XMediaPlayer.State == XMediaState.Stopped)
                {
                    XMediaPlayer.MediaStateChanged -= ChapterFinished;
                    Director._current = new ChapterNext(Director);
                }
            }

            private void IncreaseDifficulty(float time)
            {
                Director._publisher.Publish(new GamePlayCreateAsteroidCommand());
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
                    var delta = (XMediaPlayer.PlayPosition - TimeSpan.FromSeconds(14 + 2)).TotalSeconds;

                    if (delta >= 0.5 && delta <= 1)
                    {
                        _phase = 1;
                        for (var i = 0; i < 10; i++) Director._publisher.Publish(new GamePlayCreateAsteroidCommand());
                    }
                }
                else if (_phase == 1)
                {
                    var delta = (XMediaPlayer.PlayPosition - TimeSpan.FromSeconds(59 + 1)).TotalSeconds;

                    if (delta >= 0.5 && delta <= 1)
                    {
                        _phase = 2;
                        for (var i = 0; i < 10; i++) Director._publisher.Publish(new GamePlayCreateAsteroidCommand());
                    }
                }
            }

            public override void Free()
            {
                XMediaPlayer.MediaStateChanged -= ChapterFinished;
            }
        }

        private sealed class ChapterNext : Chapter
        {
            private readonly Random _random;

            public ChapterNext(GamePlayDirector director)
                : base(director)
            {
                _random = new Random();
                NextSong(null, null);
                XMediaPlayer.MediaStateChanged += NextSong;
            }

            public override void Free()
            {
                XMediaPlayer.MediaStateChanged -= NextSong;
            }

            private void NextSong(object sender, EventArgs e)
            {
                if (XMediaPlayer.State == XMediaState.Stopped)
                {
                    var song = Next();
                    while (song == XMediaPlayer.Queue.ActiveSong)
                        song = Next();

                    XMediaPlayer.Play(song);
                }
            }

            private XSong Next()
            {
                var next = _random.Next(4) + 1;
                return Director._content.Load<XSong>($"Music/game{next}.song");
            }

            public override void Update(float time)
            {
            }
        }
    }
}
