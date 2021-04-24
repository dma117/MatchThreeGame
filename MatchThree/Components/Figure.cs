using MatchThree.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MatchThree.Components
{
    public class Figure : Cell
    {
        private ButtonState _currentMouseState;
        private ButtonState _previousMouseState;

        public Figure(SpriteBatch spriteBatch, Texture2D texture, FigureType type) : base(spriteBatch, texture) 
        {
            Type = type;
            Color = Color.White;
        }
        public FigureType Type { get; set; }

        public Color Color { get; set; } 

        public event EventHandler Clicked;

        public bool Match(Figure other)
        {
            return this.Type == other.Type;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(Texture, Rectangle, Color);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            var mouseRectangle = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState().LeftButton;

            if (_currentMouseState == ButtonState.Released && _previousMouseState == ButtonState.Pressed)
            {
                if (mouseRectangle.Intersects(Rectangle))
                {
                    Clicked?.Invoke(this, null);
                }
            }
        }
    }
}
