using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MatchThree.Configs;
using System.Collections.Generic;
using MatchThree.Components;
using System;

namespace MatchThree.Screens
{
    public class GameScreen : Screen
    {
        private Texture2D[] _textures = new Texture2D[Config.COUNT_FIGURES];
        private List<Figure> _cells;
        private Vector2 _offset;

        public GameScreen(ContentManager contentManager, SpriteBatch spriteBatch) : base(contentManager, spriteBatch)
        {
            _textures[(int)FigureType.Circle] = contentManager.Load<Texture2D>("sprites/circle");
            _textures[(int)FigureType.Cube] = contentManager.Load<Texture2D>("sprites/cube");
            _textures[(int)FigureType.Diamond] = contentManager.Load<Texture2D>("sprites/diamond");
            _textures[(int)FigureType.Rectangle] = contentManager.Load<Texture2D>("sprites/rectangle");
            _textures[(int)FigureType.Triangle] = contentManager.Load<Texture2D>("sprites/triangle");

            _offset = new Vector2(10, Config.HEIGHT_SCREEN - _textures[0].Height * Config.COLS - 15);
            _cells = new List<Figure>();

            GenerateCells();
        }
        public override void Draw(GameTime gameTime)
        {
            foreach(var cell in _cells)
            {
                cell.Draw();
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        private void LoadTextures()
        {
        }

        private void GenerateCells()
        {
            var rand = new Random();

            for (int i = 0; i < Config.ROWS; i++)
            {
                for (int j = 0; j < Config.COLS; j++)
                {
                    var index = rand.Next(0, _textures.Length);

                    var cell = new Figure(_spriteBatch, _textures[index], (FigureType)index);
                    cell.StartPosition = _offset + new Vector2(cell.Width * i, cell.Height * j);

                    _cells.Add(cell);
                }
            }
        }
    }
}
