using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MatchThree.Components;
using MatchThree.Configs;


namespace MatchThree.Screens
{
    class MenuScreen : Screen
    {
        private Button _menuButton;

        public MenuScreen(ContentManager content, SpriteBatch spriteBatch) : base(content, spriteBatch)
        {
            var texture = _contentManager.Load<Texture2D>("sprites/button");
            var font = _contentManager.Load<SpriteFont>("fonts/font");

            _menuButton = new Button(spriteBatch, texture, font, "Start Game!")
            {
                StartPosition = new Vector2((Config.WIDTH_SCREEN - texture.Width) / 2, (Config.HEIGHT_SCREEN - texture.Height) / 2),
                MainColor = Color.Yellow,
                HoveredColor = Color.Gray
            };

            _menuButton.OnClick += (o, e) => CurrentScreenState = ScreenState.Deleted;
        }

        public bool MenuButtonClicked { get; set; }

        public override void Draw(GameTime gameTime)
        {
            _menuButton.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            _menuButton.Update(gameTime);
        }
    }
}
