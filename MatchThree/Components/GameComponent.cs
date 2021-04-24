using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace MatchThree.Components
{
    public abstract class GameComponent : IDrawUpdate
    {
        protected SpriteBatch _spriteBatch;

        protected Rectangle Rectangle => 
            new Rectangle((int)StartPosition.X, (int)StartPosition.Y,
                               Texture.Width, Texture.Height);

        public GameComponent(SpriteBatch spriteBatch, Texture2D texture)
        {
            _spriteBatch = spriteBatch;
            Texture = texture;
        }

        public virtual Vector2 StartPosition { get; set; }
        public Texture2D Texture { get; set; }

        public abstract void Draw(GameTime gameTime);

        public abstract void Update(GameTime gameTime);
    }
}
