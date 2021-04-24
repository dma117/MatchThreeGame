using MatchThree.Components;
using Microsoft.Xna.Framework;
using System;

namespace MatchThree.Animations
{
    public class TranslateAnimation : Animation
    {
        private Vector2 _direction;
        public TranslateAnimation(Figure figure, double timePerFrame, Vector2 destination) : base(figure, timePerFrame)
        {
            Destination = destination;
             
            _direction = Destination - _figure.StartPosition;
            _direction.Normalize();
        }

        public Vector2 Destination;
        public event EventHandler OnAnimationEnded;

        public override void Animate(GameTime gameTime)
        {
            if (_figure.StartPosition == Destination)
            {
                OnAnimationEnded?.Invoke(this, null);
            }

            //_figure.StartPosition += _direction;

            _currentTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_currentTime > _timePerFrame)
            {
                _figure.StartPosition += _direction;
                _figure.Draw(gameTime);
                _currentTime = 0;
            }
        }
    }
}
