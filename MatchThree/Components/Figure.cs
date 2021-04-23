using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MatchThree.Components
{
    public class Figure : Cell
    {
        public Figure(SpriteBatch spriteBatch, Texture2D texture, FigureType type) : base(spriteBatch, texture) 
        {
            Type = type;
            Color = Color.Red;
        }
        public FigureType Type { get; set; }

        public Color Color { get; set; } 

        public event EventHandler Clicked;

        public bool Match(Figure other)
        {
            return this.Type == other.Type;
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(Texture, Rectangle, Color);
            _spriteBatch.End();
        }
    }
}
