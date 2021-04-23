using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatchThree.Components
{
    public abstract class GameComponent
    {
        protected SpriteBatch _spriteBatch;
        protected Texture2D _texture;

        protected Rectangle Rectangle => 
            new Rectangle((int)StartPosition.X, (int)StartPosition.Y,
                               _texture.Width, _texture.Height);

        public GameComponent(SpriteBatch spriteBatch, Texture2D texture)
        {
            _spriteBatch = spriteBatch;
            _texture = texture;
        }

        public virtual Vector2 StartPosition { get; set; }

        public virtual void Hovering() { } 
    }
}
