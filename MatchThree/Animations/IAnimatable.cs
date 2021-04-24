using System;
using System.Collections.Generic;
using System.Text;
using MatchThree.Components;
using Microsoft.Xna.Framework;

namespace MatchThree.Animations
{
    public interface IAnimatable
    {
        public void Animate(GameTime gameTime);
    }
}
