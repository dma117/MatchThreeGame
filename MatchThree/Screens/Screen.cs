using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using MatchThree.Components;

namespace MatchThree.Screens
{
    public enum ScreenState
    {
        Nothing,
        Deleted
    }

    public abstract class Screen : IDrawUpdate
    {
        protected ContentManager _contentManager;
        protected readonly SpriteBatch _spriteBatch;
        public ScreenState CurrentScreenState;

        public Screen(ContentManager content, SpriteBatch spriteBatch)
        {
            _contentManager = content;
            _spriteBatch = spriteBatch;
        }

        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}
