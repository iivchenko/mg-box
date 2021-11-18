using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Content;
using Microsoft.Extensions.Options;
using System;
using XMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;
using XMediaState = Microsoft.Xna.Framework.Media.MediaState;
using XSong = Microsoft.Xna.Framework.Media.Song;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayDirector : IUpdatable
    {
        private readonly IOptionsMonitor<AudioSettings> _settings;
        private readonly Random _random;

        private IContentProvider _content;

        private Chapter _current;

        public GamePlayDirector(
            IOptionsMonitor<AudioSettings> settings,
            IContentProvider content)
        {
            _settings = settings;
            _content = content;

            _random = new Random();

            _current = new SimpleChapter(this);
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

        private sealed class SimpleChapter : Chapter
        {
            public SimpleChapter(GamePlayDirector director)
                : base(director)
            {
                // Move to rules!
                Director._content.Load<XSong>("Music/game1.song");
                XMediaPlayer.Volume = Director._settings.CurrentValue.MusicVolume;
                XMediaPlayer.Play(Director._content.Load<XSong>("Music/game1.song"));
                XMediaPlayer.MediaStateChanged += NextSong;
            }

            public override void Free()
            {
                XMediaPlayer.MediaStateChanged -= NextSong;
            }

            public override void Update(float time)
            {                
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
                var next = Director._random.Next(4) + 1;
                return Director._content.Load<XSong>($"Music/game{next}.song");
            }
        }
    }
}
