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

        public void AddSwapAnimation(Figure first, Figure second)
        {
            first.IsMoving = true;
            second.IsMoving = true;

            var posFirst = _figures[0].StartPosition;
            var posSecond = _figures[1].StartPosition;

            var animationFirst = new TranslateAnimation(_figures[0], 100, posSecond);
            animationFirst.OnAnimationEnded += EndAnimation;

            var animationSecond = new TranslateAnimation(_figures[1], 100, posFirst);
            animationSecond.OnAnimationEnded += EndAnimation;

            _animations.AddRange(new List<Animation>() { animationFirst, animationSecond });
        }

        public void AddTranslation(Figure figure, Vector2 direction)
        {
            figure.IsMoving = true;

            var animation = new TranslateAnimation(figure, 100, direction);
            animation.OnAnimationEnded += EndAnimation;

            _animations.Add(animation);
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

        private void EndAnimation(object obj, EventArgs args)
        {
            var animation = obj as Animation;

            if (animation != null)
            {
                _animations.Remove(animation);
            }
        }
    }
}
