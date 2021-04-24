using MatchThree.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MatchThree.Animations
{
    public abstract class Animation : IAnimatable
    {
        protected double _currentTime;
        protected double _timePerFrame;
        public Figure _figure;

        public Animation(Figure figure, double timePerFrame)
        {
            _figure = figure;
            _timePerFrame = timePerFrame;
        }

        public abstract void Animate(GameTime gameTime);
    }
}
