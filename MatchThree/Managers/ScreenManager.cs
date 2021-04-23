using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MatchThree.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace MatchThree.Managers
{
    public enum GameState
    {
        Menu, 
        InGame,
        End
    }

    public class ScreenManager
    {
        private List<Screen> _screens;

        private GameState _currentGameState;

        private ContentManager _contentManager;
        private SpriteBatch _spriteBatch;

        public ScreenManager(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;

            _screens = new List<Screen>()
            {
               new MenuScreen(contentManager, spriteBatch),
            };
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var screen in _screens)
            {
                screen.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
           
            foreach (var screen in _screens)
            {
                if (screen.CurrentScreenState == ScreenState.Deleted)
                {
                    _screens.Clear();
                    ChangeScreen(++_currentGameState);

                    break;
                }

                screen.Update(gameTime);
            }
        }

        private void ChangeScreen(GameState state)
        {
            switch(state)
            {
                case GameState.Menu:
                    _screens = new List<Screen>() { new MenuScreen(_contentManager, _spriteBatch) };
                    break;
                case GameState.InGame:
                    _screens = new List<Screen>() { new GameScreen(_contentManager, _spriteBatch) };
                    break;
                case GameState.End:
                    //_screens = new List<Screen>() { new GameScreen(_contentManager, _spriteBatch) }; // TODO (add a screen for ending the game)
                    break;
                default:
                    break;
            }
        }
    }
}
