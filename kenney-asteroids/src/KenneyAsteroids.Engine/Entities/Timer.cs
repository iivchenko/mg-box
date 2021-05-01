using System;

namespace KenneyAsteroids.Engine.Entities
{
    public sealed class Timer : IEntity, IUpdatable
    {
        private enum TimerState
        {
            Idle,
            Run
        }

        private readonly TimeSpan _timeout;
        private readonly Action<float> _action;
        private readonly bool _loop;

        private TimerState _state;
        private float _remain;

        public Timer(TimeSpan timeout, Action<float> action, bool loop)
        {
            _timeout = timeout;
            _action = action;
            _loop = loop;

            _state = TimerState.Idle;
        }

        void IUpdatable.Update(float time)
        {
            switch (_state)
            {
                case TimerState.Idle:
                    break;
                
                case TimerState.Run:
                    _remain -= time;

                    if (_remain <= 0.0f)
                    {
                        _remain = 0.0f;
                        _action(time);

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

            if (_remain <= 0.0f)
            {
                _remain = (float)_timeout.TotalSeconds;
            }
        }

        public void Pause()
        {
            _state = TimerState.Idle;
        }

        public void Reset()
        {
            _state = TimerState.Idle;
            _remain = (float)_timeout.TotalSeconds;
        }
    }
}
