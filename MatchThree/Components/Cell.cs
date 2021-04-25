using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MatchThree.Components
{
    public class Cell : GameComponent
    {
        public Cell() { }
        public Cell(SpriteBatch spriteBatch, Texture2D texture) : base(spriteBatch, texture) { }

        public int Width => Texture.Width;
        public int Height => Texture.Height;
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(Texture, Rectangle, Color.White);
            _spriteBatch.End();
        }
        public override void Update(GameTime gameTime) { }
    }
}
