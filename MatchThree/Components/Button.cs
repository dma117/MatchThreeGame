using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MatchThree.Components
{
	public class Button : GameComponent
	{
		private SpriteFont _font;
		private string _text;
		private bool _isHovering;

		private ButtonState _currentMouseState;
		private ButtonState _previousMouseState;

		public Button(SpriteBatch spriteBatch, Texture2D texture) : base(spriteBatch, texture)
		{
			MainColor = Color.White;
			HoveredColor = Color.Gray;
			_isHovering = false;
		}

		public Button(SpriteBatch spriteBatch, Texture2D texture, SpriteFont font, string text) : this(spriteBatch, texture)
		{
			_font = font;
			_text = text;
		}

		public Color MainColor { get; set; }
		public Color HoveredColor { get; set; }

		public event EventHandler OnClick;

		public void Draw(GameTime gameTime)
		{
			_spriteBatch.Begin();

			if (_isHovering)
			{
				_spriteBatch.Draw(Texture, Rectangle, HoveredColor);
			}
			else
			{
				_spriteBatch.Draw(Texture, Rectangle, MainColor);
			}

			_spriteBatch.DrawString(_font, _text, GetTextPosition(), Color.Black);
			_spriteBatch.End();
		}

		public void Update(GameTime gameTime)
		{
			var mouseRectangle = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);

			if (mouseRectangle.Intersects(Rectangle))
			{
				_isHovering = true;
			}
			else
			{
				_isHovering = false;
			}

			_previousMouseState = _currentMouseState;
			_currentMouseState = Mouse.GetState().LeftButton;

			if (_currentMouseState == ButtonState.Pressed && _previousMouseState == ButtonState.Pressed)
			{
				OnClick?.Invoke(this, null);
			}
		}

		private Vector2 GetTextPosition()
		{
			var x = (StartPosition.X + Rectangle.Width / 2) - _font.MeasureString(_text).X / 2;
			var y = (StartPosition.Y + Rectangle.Height / 2) - _font.MeasureString(_text).Y / 2;

			return new Vector2(x, y);
		}
	}
}
