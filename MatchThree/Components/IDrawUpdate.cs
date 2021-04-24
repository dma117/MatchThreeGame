using Microsoft.Xna.Framework;

namespace MatchThree.Components
{
    public interface IDrawUpdate
    {
        public void Draw(GameTime gameTime);
        public void Update(GameTime gameTime);
    }
}
