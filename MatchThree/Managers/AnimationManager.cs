using MatchThree.Animations;
using MatchThree.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatchThree.Managers
{
    public class AnimationManager : IDrawUpdate
    {
        private List<Animation> _animations;
        private List<Figure> _figures;

        public AnimationManager(List<Figure> figures)
        {
            _animations = new List<Animation>();
            _figures = figures;
        }

        public void AddTranslateAnimation()
        {
            var posFirst = _figures[0].StartPosition;
            var posSecond = _figures[1].StartPosition;

            var animationFirst = new TranslateAnimation(_figures[0], 100, posSecond);
            animationFirst.OnAnimationEnded += (o, e) => _animations.Remove(animationFirst);

            var animationSecond = new TranslateAnimation(_figures[1], 100, posFirst);
            animationSecond.OnAnimationEnded += (o, e) => _animations.Remove(animationSecond);

            _animations.AddRange(new List<Animation>() { animationFirst, animationSecond });
        }

        public void Draw(GameTime gameTime)
        {
            /*while (_animations.Count != 0)
            {
                _animations[0].Animate(gameTime);
            }*/
            for (int i = 0; i < _animations.Count; i++)
            {
                _animations[i].Animate(gameTime);
            }
        }

        public void Update(GameTime gameTime) { }
    }
}
