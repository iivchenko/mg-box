using Microsoft.Xna.Framework;
using System;

namespace KenneyAsteroids.Engine
{
    public sealed class Timer : IEntity, IUpdatable
    {
        private enum TimerState
        {
            Idle,
            Run
        }

        private readonly TimeSpan _timeout;
        private readonly Action<GameTime> _action;
        private readonly bool _loop;

        private TimerState _state;
        private TimeSpan _remain;

        public Timer(TimeSpan timeout, Action<GameTime> action, bool loop)
        {
            _timeout = timeout;
            _action = action;
            _loop = loop;

            _state = TimerState.Idle;
        }

        void IUpdatable.Update(GameTime gameTime)
        {
            switch (_state)
            {
                case TimerState.Idle:
                    break;
                
                case TimerState.Run:
                    _remain = _remain.Subtract(gameTime.ElapsedGameTime);

                    if (_remain <= TimeSpan.Zero)
                    {
                        _remain = TimeSpan.Zero;
                        _action(gameTime);

                        if (_loop)
                        {
                            Start();
                        }
                        else
                        {
                            _state = TimerState.Idle;
                        }
                    }

                    break;
            }
        }

        public void Start()
        {
            _state = TimerState.Run;

            if (_remain <= TimeSpan.Zero)
            {
                _remain = _timeout;
            }
        }

        public void Pause()
        {
            _state = TimerState.Idle;
        }

        public void Reset()
        {
            _state = TimerState.Idle;
            _remain = _timeout;
        }
    }
}
