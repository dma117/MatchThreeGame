﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MatchThree.Components;
using MatchThree.Configs;


namespace MatchThree.Screens
{
    public class BackgroundScreen : Screen
    {
        private Cell _backgroundCell;
        private Vector2 _offset;
        
        public BackgroundScreen(ContentManager content, SpriteBatch spriteBatch) : base(content, spriteBatch)
        {
            var texture = _contentManager.Load<Texture2D>("sprites/back_rectangle");

            _offset = new Vector2(10, Config.HEIGHT_SCREEN - texture.Height * Config.COLS - 15);

            _backgroundCell = new Cell(spriteBatch, texture)
            {
                StartPosition = _offset
            };
        }

        public override void Draw(GameTime gameTime)
        {
            _backgroundCell.StartPosition = _offset;

            for (int i = 0; i < Config.ROWS; i++)
            {
                for (int j = 0; j < Config.COLS; j++)
                {
                    _backgroundCell.StartPosition = _offset +  new Vector2(_backgroundCell.Width * j, _backgroundCell.Height * i);
                    _backgroundCell.Draw(gameTime);
                }
            }
        }

        public override void Update(GameTime gameTime) { }
    }
}
