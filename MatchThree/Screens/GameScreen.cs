using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace MatchThree.Screens
{
    public enum CellType
    {
        Circle,
        Cube,
        Diamond,
        Rectangle,
        Triangle
    }

    public class GameScreen : Screen
    {
        public GameScreen(ContentManager contentManager, SpriteBatch spriteBatch) : base(contentManager, spriteBatch)
        {
            
        }
        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        private void LoadTextures()
        {
        }
    }
}
