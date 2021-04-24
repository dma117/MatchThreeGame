using MatchThree.Components;
using Microsoft.Xna.Framework;
using System;

namespace MatchThree.Animations
{
    public abstract class Animation : IAnimatable
    {
        protected double _currentTime;
        protected double _timePerFrame;
        protected Figure _figure;

        public Animation(Figure figure, double timePerFrame)
        {
            _figure = figure;
            _timePerFrame = timePerFrame;
        }


        public abstract void Animate(GameTime gameTime);
    }
}
