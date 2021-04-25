using Microsoft.Xna.Framework;
using System;


namespace MatchThree.Managers
{
    public class Timer
    {
        private TimeSpan _timer = new TimeSpan();
        private bool _isActive;

        public TimeSpan Value { get => _timer; private set => _timer = value; }

        public EventHandler Finished;

        public void Update(GameTime gameTime)
        {
            if (_isActive)
            {
                _timer -= gameTime.ElapsedGameTime;

                if (_timer.Seconds <= 0)
                {
                    Stop();
                    Finished?.Invoke(this, null);
                }
            }
        }

        public void Stop()
        {
            _isActive = false;
        }

        public void Start()
        {
            _isActive = true;
        }

        public void SetTimer(TimeSpan time)
        {
            _timer = time;
        }
    }
}
