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
        Matching,
        Refreshing
    }

    public class GameScreen : Screen
    {
        private Vector2 _offset;
        private int _size;

        private Texture2D[] _textures = new Texture2D[Config.COUNT_FIGURES];
        private List<Figure> _cells;
        private List<Figure> _chosenFigures;

        private GameState _currentGameState;
        private AnimationManager _animationManager;

        public GameScreen(ContentManager contentManager, SpriteBatch spriteBatch) : base(contentManager, spriteBatch)
        {
            _textures[(int)FigureType.Circle] = contentManager.Load<Texture2D>("sprites/circle");
            _textures[(int)FigureType.Cube] = contentManager.Load<Texture2D>("sprites/cube");
            _textures[(int)FigureType.Diamond] = contentManager.Load<Texture2D>("sprites/diamond");
            _textures[(int)FigureType.Rectangle] = contentManager.Load<Texture2D>("sprites/rectangle");
            _textures[(int)FigureType.Triangle] = contentManager.Load<Texture2D>("sprites/triangle");

            _offset = new Vector2(10, Config.HEIGHT_SCREEN - _textures[0].Height * Config.COLS - 15);
            _size = Config.ROWS * Config.COLS;
            _cells = new List<Figure>();
            _chosenFigures = new List<Figure>();
            _animationManager = new AnimationManager(_chosenFigures);

            GenerateCells();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(var cell in _cells)
            {
                if (cell != null)
                {
                    cell.Draw(gameTime);
                }

                _animationManager.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime) 
        {
            foreach (var cell in _cells)
            {
                if (cell != null)
                {
                    cell.Update(gameTime);
                }
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
                        if (CanBeSwapped(_chosenFigures[0], _chosenFigures[1]))
                        {
                            _currentGameState = GameState.Placing;
                            SwapFigures(_chosenFigures[0], _chosenFigures[1]);
                            _animationManager.AddSwapAnimation(_chosenFigures[0], _chosenFigures[1]);
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
                    }
                    break;
                case GameState.Placing:
                    if (NoMoving())
                    {
                        Fill();
                        _currentGameState = GameState.Matching;
                    }   
                    break;
                case GameState.Matching:
                    var matches = FindMatches();
                    
                    if (matches.Count == 0)
                    {
                        if (_chosenFigures.Count != 0)
                        {
                            SwapFigures(_chosenFigures[0], _chosenFigures[1]);
                            _animationManager.AddSwapAnimation(_chosenFigures[0], _chosenFigures[1]);
                        }
                        
                        _currentGameState = GameState.Refreshing;
                    }
                    else
                    {
                        foreach (var figure in matches)
                        {
                            int pos = _cells.IndexOf(figure);
                            if (pos > 0)
                            {
                                _cells[pos] = null;
                            }
                           
                        }

                        SetNewRandomFigures();
                        _currentGameState = GameState.Placing;
                        _chosenFigures.Clear();
                    }
                    break;
                case GameState.Refreshing:
                    if (NoMoving())
                    {
                        _currentGameState = GameState.Initial;
                        _chosenFigures.Clear();
                    }

                    break;

            }
        }

        private List<Figure> FindMatches()
        {
            var matches = new List<Figure>();

            if (_chosenFigures.Count == 0)
            {
                for (int i = 0; i < Config.COLS - 1; i += 2)
                {
                    int first = i * Config.ROWS + i;
                    int second = (i + 1) * Config.ROWS + (i + 1);

                    matches.AddRange(Matched(_cells[first], _cells[second]));
                }
            }
            else
            {
                matches.AddRange(Matched(_chosenFigures[0], _chosenFigures[1]));
            }

            return matches;
        }

        private void Fill()
        {
            for (int i = 0; i < _size; i++)
            {
                if (_cells[i] == null)
                {
                    _cells[i] = GetRandomFigure();

                    int width = _cells[i].Width * (i % Config.COLS);
                    int height = _cells[i].Height * (i / Config.ROWS);

                    _cells[i].StartPosition = _offset + new Vector2(width, height);
                }
            }
        }

        private void SetNewRandomFigures()
        {
            int indexFigure = _size;

            for (int j = Config.COLS - 1; j >= 0; j--)
            {
                int bound = j;

                for (int i = indexFigure - 1; i >= bound; i -= Config.ROWS)
                {
                    if (_cells[i] == null)
                    {
                        var count = Fall(i % Config.COLS, i);
                        bound += (count * Config.ROWS);
                    }
                }

                /*for (int col = j; col < bound; col += Config.ROWS)
                {
                    _cells[col] = GetRandomFigure();
                    _cells[col].StartPosition = _offset + new Vector2(_cells[col].Width * (col % Config.COLS), _cells[col].Height * (col / Config.ROWS));
                }*/

                indexFigure--;
            }
        }

        private int Fall(int start, int end)
        {
            int height = 0;

            while (end >= start && _cells[end] == null)
            {
                end -= Config.ROWS;
                height++;
            }

            while (end >= start && _cells[end] != null)
            {
                var direction = _cells[end].StartPosition + new Vector2(0, _cells[end].Height * height);
                _animationManager.AddTranslation(_cells[end], direction);

                _cells[end + height * Config.ROWS] = _cells[end];
                _cells[end] = null;

                end -= Config.ROWS;
            }

            return height;
        }

        private void FallFig(int start, int end)
        {
            int count = 0;
            int offset = 1;

            while (end > 0 && end != start)
            {
                if (_cells[end] == null)
                {
                    count = CountNulls(ref start, ref end);
                    offset = count + 1;
                }
                else if (count != 0)
                {
                    var direction = _cells[end].StartPosition + new Vector2(0, _cells[end].Height * offset);
                    _animationManager.AddTranslation(_cells[end], direction);

                    _cells[end + Config.ROWS] = _cells[end];
                    _cells[end] = null;
                }

                end -= Config.ROWS;
            }
        }

        private int CountNulls(ref int start, ref int end)
        {
            int count = 0;

            while (end != start && end > 0 && _cells[end] == null)
            {
                count++;
                end -= Config.ROWS;
            }

            return count;
        }

        private void FallFigures(int start, int end)
        {
            var countCells = Config.ROWS * Config.COLS;
            var current = _cells[end]; // null
            var prev = _cells[end];

            if (end + Config.ROWS < countCells)
            {
                prev = _cells[end + Config.ROWS];
            }
            else
            {
                end -= Config.ROWS;
            }

            var countNewFigures = 1;
            int y_offset = 1;

            for (int i = end; i >= start; i -= Config.ROWS)
            {
                current = _cells[i];

                if (current == null)
                {
                    y_offset++;
                    countNewFigures++;

                    if (prev != null)
                    {
                        y_offset = 1;
                    }
                }

                if (current != null && prev == null)
                {
                    var destination = new Vector2(_cells[i].StartPosition.X, _cells[i].StartPosition.Y + _cells[i].Height * y_offset);

                    //var pos = _cells[i].StartPosition;
                    //var destination = new Vector2(pos.X, pos.Y + 10);

                    _animationManager.AddTranslation(_cells[i], destination);

                    var replacedCell = i + Config.ROWS * (y_offset - 1);
                    _cells[replacedCell] = _cells[i];
                    _cells[i] = null;
                }

                prev = current;
            }

            /*for (int i = start, k = 0; countNewFigures > 0 && i < countCells; i += Config.ROWS, countNewFigures--, k++)
            {
                var figure = GetRandomFigure();
                var position = _offset + new Vector2(start * figure.Texture.Width, k * figure.Texture.Height);

                _cells[i] = figure;
                _cells[i].StartPosition = position;
            }*/
        }

        private bool NoMoving()
        {
            return _cells.All(x =>
            {
                if (x != null)
                    return x.IsMoving == false;
                return true;
            }
            );
        }

        private void SwapFigures(Figure first, Figure second)
        {
            var indexFirst = _cells.IndexOf(first);
            var indexSecond = _cells.IndexOf(second);

            var tmp = _cells[indexFirst];
            _cells[indexFirst] = _cells[indexSecond];
            _cells[indexSecond] = tmp;
        }

        private Figure GetRandomFigure()
        {
            var rand = new Random();

            var indexTexture = rand.Next(0, _textures.Length);
            var cell = new Figure(_spriteBatch, _textures[indexTexture], (FigureType)indexTexture);
            cell.Clicked += FigureOnClick;

            return cell;
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

        private List<Figure> GenerateCells(int count)
        {
            List<Figure> result = new List<Figure>();

            if (count < 0)
                return result;

            var rand = new Random();

            while (count-- != 0)
            {
                var indexTexture = rand.Next(0, _textures.Length);
                var cell = new Figure(_spriteBatch, _textures[indexTexture], (FigureType)indexTexture);
                cell.Clicked += FigureOnClick;

                result.Add(cell);
            }

            return result;
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
            
            figures.AddRange(FindMatches(GetRow(first)));
            figures.AddRange(FindMatches(GetRow(second)));
            figures.AddRange(FindMatches(GetCol(first)));
            figures.AddRange(FindMatches(GetCol(second)));
            
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
                    {
                        matches.AddRange(tmp);
                    }

                    tmp.Clear();
                    tmp.Add(figures[i + 1]);
                }
            }

            if (tmp.Count >= 3)
            {
                matches.AddRange(tmp);
            }

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

            if (figure != null && NoMoving())
            {
                _chosenFigures.Add(figure);
                figure.Color = Color.Black;

                _currentGameState = GameState.Choosing;
            }
        }

        private bool CanBeSwapped(Figure first, Figure second)
        {
            var indexFirst = _cells.IndexOf(first);
            var indexSecond = _cells.IndexOf(second);

            return Math.Abs(indexSecond - indexFirst) == 1 || 
                   Math.Abs(indexSecond - indexFirst) == Config.ROWS;
        }
    }
}
