using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MatchThree.Configs;
using System.Collections.Generic;
using MatchThree.Components;
using System;
using System.Linq;

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
            DetectMatches();

           /* while (DetectMatches().Count > 0)
            {
                GenerateCells();
            }*/
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
                    var indexTexture = rand.Next(0, _textures.Length);
                    var cell = new Figure(_spriteBatch, _textures[indexTexture], (FigureType)indexTexture);
                    _cells.Add(cell);

                    /*while (Matched(_cells.Count - 1))
                    {*/
                       // cell.Texture = _textures[++indexTexture % _textures.Length];
                        //cell.Type = (FigureType)(indexTexture % _textures.Length);
                   // }

                    cell.StartPosition = _offset + new Vector2(cell.Width * j, cell.Height * i);
                }
            }
        }

        private bool Matched(int index)
        {
            if (FindMatches(GetRow(index)).Count >= 3 || FindMatches(GetCol(index)).Count >= 3)
                return true;

            return false;
        }

        private List<Figure> DetectMatches()
        {
            var result = new List<Figure>();

            for (int i = 0; i < 64; i += 8)
            {
                var list = FindMatches(GetRow(i));

                if (list.Count > 0)
                    result.AddRange(list);
            }
            for (int i = 0; i < 8; i++)
            {
                var list = FindMatches(GetCol(i));

                if (list.Count > 0)
                    result.AddRange(list);
            }

            return result;
        }

        private List<Figure> Matched(int first, int second) // поменять элементы перед тем как вызывать метод
        {
            List<Figure> figures = new List<Figure>();

            int indexFirstCol = first % Config.COLS;
            int indexSecondCol = second % Config.COLS;
            int indexFirstRow = first / Config.ROWS;
            int indexSecondRow = second / Config.ROWS;

            if (indexFirstRow == indexSecondRow)
            {
                figures.AddRange(FindMatches(GetRow(indexFirstRow)));
                figures.AddRange(FindMatches(GetCol(indexFirstCol)));
                figures.AddRange(FindMatches(GetCol(indexSecondCol)));
            }
            if (indexFirstCol == indexSecondCol)
            {
                figures.AddRange(FindMatches(GetCol(indexFirstCol)));
                figures.AddRange(FindMatches(GetRow(indexFirstRow)));
                figures.AddRange(FindMatches(GetRow(indexSecondRow)));
            }

            return figures;
        }

        private List<Figure> FindMatches(List<Figure> figures)
        {
            var matches = new List<Figure>();
            var list = new List<Figure>(); // TODO rename on tmp

            list.Add(figures[0]);
            for (int i = 0; i < figures.Count - 1; i++)
            {
                if (figures[i].Match(figures[i + 1]))
                {
                    list.Add(figures[i + 1]);
                }
                else
                {
                    if (list.Count >= 3)
                        matches.AddRange(list);

                    list.Clear();
                    list.Add(figures[i + 1]);
                }
            }
            if (list.Count >= 3)
                matches.AddRange(list);
            matches.ForEach(x => x.Color = Color.White);

            return matches;
        }

        private List<Figure> GetRow(int index)
        {
            List<Figure> result = new List<Figure>();
            int row = index / Config.ROWS;
            int start = row * Config.ROWS;

            for (int i = start; result.Count < Config.ROWS; i++)
            {
                if (_cells.Count - 1 < i)
                {
                    break;
                }
                
                result.Add(_cells[i]);
            }

            return result;
        }

        private List<Figure> GetCol(int index)
        {
            List<Figure> result = new List<Figure>();

            for (int i = index % Config.ROWS; result.Count < Config.COLS; i += Config.COLS)
            {
                if (_cells.Count - 1 < i)
                {
                    break;
                }

                result.Add(_cells[i]);
            }

            return result;
        }
    }
}
