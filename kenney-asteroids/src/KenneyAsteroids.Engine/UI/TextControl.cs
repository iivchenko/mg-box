//-----------------------------------------------------------------------------
// TextControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Engine.UI
{
    /// <summary>
    /// TextControl is a control that displays a single string of text. By default, the
    /// size is computed from the given text and spritefont.
    /// </summary>
    public class TextControl : Control
    {
        private SpriteFont font;
        private string text;

        public Color Color;

        // Actual text to draw
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    InvalidateAutoSize();
                }
            }
        }

        // Font to use
        public SpriteFont Font
        {
            get { return Font; }
            set
            {
                if (font != value)
                {
                    font = value;
                    InvalidateAutoSize();
                }
            }
        }

        public TextControl()
            : this(string.Empty, null, Colors.White, Vector2.Zero)
        {
        }

        public TextControl(string text, SpriteFont font)
            : this(text, font, Colors.White, Vector2.Zero)
        {
        }

        public TextControl(string text, SpriteFont font, Color color)
            : this(text, font, color, Vector2.Zero)
        {
        }

        public TextControl(string text, SpriteFont font, Color color, Vector2 position)
        {
            this.text = text;
            this.font = font;
            this.Position = position;
            this.Color = color;
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);

            context.Painter.DrawString(font, Text, context.DrawOffset.ToVector(), Color);
        }

        override public Vector2 ComputeSize()
        {
            return font.MeasureString(Text);
        }
    }
}
