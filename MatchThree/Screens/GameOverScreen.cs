using MatchThree.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MatchThree.Components;


namespace MatchThree.Screens
{
    public class GameOverScreen : Screen
    {
        private SpriteFont _font;
        private Cell _background;
        private Button _okButton;

        public GameOverScreen(ContentManager contentManager, SpriteBatch spriteBatch) : base(contentManager, spriteBatch)
        {
            _font = contentManager.Load<SpriteFont>("fonts/font");
            var textureGameOver = contentManager.Load<Texture2D>("sprites/game_over");
            var textureButton = _contentManager.Load<Texture2D>("sprites/button");

            _background = new Cell(_spriteBatch, textureGameOver)
            {
                StartPosition = new Vector2((Config.WIDTH_SCREEN - textureGameOver.Width) / 2,
                                            (Config.HEIGHT_SCREEN - textureGameOver.Height) / 2)
            };

            _okButton = new Button(spriteBatch, textureButton, _font, "OK")
            {
                StartPosition = new Vector2((Config.WIDTH_SCREEN - textureButton.Width) / 2,
                                            (Config.HEIGHT_SCREEN - textureButton.Height) / 2),
                MainColor = Color.Yellow,
                HoveredColor = Color.Gray
            };

            _okButton.OnClick += RestartGame;
        }

        public event EventHandler OnGameRestarted;

        public override void Draw(GameTime gameTime)
        {
            _background.Draw(gameTime);
            _okButton.Draw(gameTime);
        }

        public override void Update(GameTime gameTime) 
        {
            _okButton.Update(gameTime);
        }

        private void RestartGame(object obj, EventArgs args)
        {
            OnGameRestarted?.Invoke(this, null);
        }
    }
}
