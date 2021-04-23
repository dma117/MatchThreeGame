using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MatchThree.Components
{
    public class Cell : GameComponent
    {
        public Cell(SpriteBatch spriteBatch, Texture2D texture) : base(spriteBatch, texture) { }

        public int Width => _texture.Width;
        public int Height => _texture.Height;

        public void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, Rectangle, Color.White);
            _spriteBatch.End();
        }

        public void Update()
        {

        }
    }
}
