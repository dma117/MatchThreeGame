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
        private Texture2D _texture;
        private Button _okButton;

        public GameOverScreen(ContentManager contentManager, SpriteBatch spriteBatch) : base(contentManager, spriteBatch)
        {
            _font = contentManager.Load<SpriteFont>("fonts/font");
            _texture = contentManager.Load<Texture2D>("sprites/game_over");

            _okButton = new Button(spriteBatch, _texture, _font, "OK")
            {
                StartPosition = new Vector2(Config.WIDTH_SCREEN / 2 - _texture.Width / 2, Config.HEIGHT_SCREEN / 2 - _texture.Height / 2),
                MainColor = Color.Yellow,
                HoveredColor = Color.Gray
            };

            _okButton.OnClick += RestartGame;
        }

        public event EventHandler OnGameRestarted;

        public override void Draw(GameTime gameTime)
        {
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
