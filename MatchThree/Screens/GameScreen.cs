using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MatchThree.Configs;
using System.Collections.Generic;
using MatchThree.Components;
using System;
using System.Linq;
using MatchThree.Animations;
using MatchThree.Managers;

namespace MatchThree.Screens
{
    public enum GameState
    {
        Initial,
        Choosing,
        Placing,
        Matching
    }

    public class GameScreen : Screen
    {
        private Texture2D[] _textures = new Texture2D[Config.COUNT_FIGURES];
        private List<Figure> _cells;
        private List<Figure> _chosenFigures;
        private GameState _currentGameState;
        private Vector2 _offset;
        private AnimationManager _animationManager;

        public GameScreen(ContentManager contentManager, SpriteBatch spriteBatch) : base(contentManager, spriteBatch)
        {
            _textures[(int)FigureType.Circle] = contentManager.Load<Texture2D>("sprites/circle");
            _textures[(int)FigureType.Cube] = contentManager.Load<Texture2D>("sprites/cube");
            _textures[(int)FigureType.Diamond] = contentManager.Load<Texture2D>("sprites/diamond");
            _textures[(int)FigureType.Rectangle] = contentManager.Load<Texture2D>("sprites/rectangle");
            _textures[(int)FigureType.Triangle] = contentManager.Load<Texture2D>("sprites/triangle");

            _offset = new Vector2(10, Config.HEIGHT_SCREEN - _textures[0].Height * Config.COLS - 15);
            _cells = new List<Figure>();
            _chosenFigures = new List<Figure>();
            _animationManager = new AnimationManager(_chosenFigures);

            GenerateCells();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(var cell in _cells)
            {
                cell.Draw(gameTime);
                _animationManager.Draw(gameTime);
                _animationManager.Draw(gameTime);

            }
        }

        public override void Update(GameTime gameTime) 
        {
            foreach (var cell in _cells)
            {
                cell.Update(gameTime);
            }

            switch(_currentGameState)
            {
                case GameState.Initial:
                    break;
                case GameState.Choosing:
                    foreach(var figure in _chosenFigures)
                    {
                        //animation of choosing
                    }
                    if (_chosenFigures.Count == 2)
                    {
                        _currentGameState = GameState.Placing;
                    }
                    break;
                case GameState.Placing:
                    if (CanBeMatched(_chosenFigures[0], _chosenFigures[1]))
                    {
                        _currentGameState = GameState.Matching;
                    }
                    else
                    {
                        foreach (var figure in _chosenFigures)
                        {
                            figure.Color = Color.White;
                        }

                        _currentGameState = GameState.Initial;
                        _chosenFigures.Clear();
                    }
                    break;
                case GameState.Matching:
                    SwapFigures(_chosenFigures[0], _chosenFigures[1]);
                    //animation place in another position (change coordinates)
                    //for each figure.ChangePosition(vector2 destination)

                    _animationManager.AddTranslateAnimation();

                    var matchedFigures = Matched(_chosenFigures[0], _chosenFigures[1]);
                    
                    if (matchedFigures.Count == 0)
                    {
                        foreach (var figure in _chosenFigures)
                        {
                            figure.Color = Color.Green;
                            //animation of not matching
                        }

                        SwapFigures(_chosenFigures[0], _chosenFigures[1]);

                        var tmp = _chosenFigures[0];
                        _chosenFigures[0] = _chosenFigures[1];
                        _chosenFigures[1] = tmp;
                        //animation place in another position (change coordinats)
                        //for each figure.ChangePosition(vector2 destination)
                        _animationManager.AddTranslateAnimation();
                    }
                    else
                    {
                        foreach(var figure in matchedFigures)
                        {
                            figure.Color = Color.Red;
                            //animation of matching
                            //delete matched figures
                            //random new figures where it needs
                            //don't forget to add them at right index in _cells
                        }
                    }

                    _currentGameState = GameState.Initial;
                    _chosenFigures.Clear();

                    break;
            }
        }

        private void SwapFigures(Figure first, Figure second)
        {
            var indexFirst = _cells.IndexOf(first);
            var indexSecond = _cells.IndexOf(second);

            var tmp = _cells[indexFirst];
            _cells[indexFirst] = _cells[indexSecond];
            _cells[indexSecond] = tmp;
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
                    cell.Clicked += FigureOnClick;

                    _cells.Add(cell);

                    while (Matched(_cells.Count - 1))
                    {
                        cell.Texture = _textures[++indexTexture % _textures.Length];
                        cell.Type = (FigureType)(indexTexture % _textures.Length);
                    }

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

        private List<Figure> Matched(Figure first, Figure second) // поменять элементы перед тем как вызывать метод
        {
            List<Figure> figures = new List<Figure>();
            
            if (CanBeMatched(first, second))
            {
                figures.AddRange(FindMatches(GetRow(first)));
                figures.AddRange(FindMatches(GetRow(second)));
                figures.AddRange(FindMatches(GetCol(first)));
                figures.AddRange(FindMatches(GetCol(second)));
            }

            return figures.Distinct().ToList();
        }

        private List<Figure> FindMatches(List<Figure> figures)
        {
            var matches = new List<Figure>();
            var tmp = new List<Figure>();

            tmp.Add(figures[0]);
            for (int i = 0; i < figures.Count - 1; i++)
            {
                if (figures[i].Match(figures[i + 1]))
                {
                    tmp.Add(figures[i + 1]);
                }
                else
                {
                    if (tmp.Count >= 3)
                        matches.AddRange(tmp);

                    tmp.Clear();
                    tmp.Add(figures[i + 1]);
                }
            }

            if (tmp.Count >= 3)
                matches.AddRange(tmp);

            return matches;
        }

        private List<Figure> GetRow(Figure figure)
        {
            return GetRow(_cells.IndexOf(figure));
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

        private List<Figure> GetCol(Figure figure)
        {
            return GetCol(_cells.IndexOf(figure));
        }

        private List<Figure> GetCol(int index)
        {
            List<Figure> result = new List<Figure>();
            int start = index % Config.ROWS;

            for (int i = start; result.Count < Config.COLS; i += Config.COLS)
            {
                if (_cells.Count - 1 < i)
                {
                    break;
                }

                result.Add(_cells[i]);
            }

            return result;
        }

        private void FigureOnClick(object obj, EventArgs args)
        {
            var figure = obj as Figure;

            if (figure != null)
            {
                _chosenFigures.Add(figure);
                figure.Color = Color.Black;

                _currentGameState = GameState.Choosing;
            }
        }

        private bool CanBeMatched(Figure first, Figure second)
        {
            var indexFirst = _cells.IndexOf(first);
            var indexSecond = _cells.IndexOf(second);

            return Math.Abs(indexSecond - indexFirst) == 1 || 
                   Math.Abs(indexSecond - indexFirst) == Config.ROWS;
        }
    }
}
