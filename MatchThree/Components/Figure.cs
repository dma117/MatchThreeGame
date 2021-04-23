using Microsoft.Xna.Framework.Graphics;
using System;

namespace MatchThree.Components
{
    public class Figure : Cell
    {
        public Figure(SpriteBatch spriteBatch, Texture2D texture, FigureType type) : base(spriteBatch, texture) 
        {
            Type = type;
        }
        public FigureType Type { get; private set; }

        public event EventHandler Clicked;
    }
}
