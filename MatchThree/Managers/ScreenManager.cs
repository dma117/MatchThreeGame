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
            for (int i = 0; i < _screens.Count; i++)
            {
                if (_screens[i].CurrentScreenState == ScreenState.Deleted)
                {
                    _screens.Clear();
                    ChangeScreen(++_currentGameState);

                    break;
                }

                _screens[i].Update(gameTime);
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
                    var game = new GameScreen(_contentManager, _spriteBatch);
                    game.OnEndGame += (o, e) => ChangeScreen(++_currentGameState);

                    _screens = new List<Screen>() 
                    {
                        new BackgroundScreen(_contentManager, _spriteBatch),
                        game
                    };
                    break;
                case GameState.End:
                    var gameOver = new GameOverScreen(_contentManager, _spriteBatch);
                    gameOver.OnGameRestarted += (o, e) => ChangeScreen(_currentGameState = GameState.Menu);

                    _screens.Add(gameOver);
                    break;
                default:
                    break;
            }
        }
    }
}
