//-----------------------------------------------------------------------------
// TextControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using KenneyAsteroids.Engine.Graphics;
using System.Numerics;

namespace KenneyAsteroids.Engine.UI
{
    /// <summary>
    /// TextControl is a control that displays a single string of text. By default, the
    /// size is computed from the given text and spritefont.
    /// </summary>
    public class TextControl : Control
    {
        private Font font;
        private IFontService _fontService;
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
        public Font Font
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

        public IFontService FontService { get => _fontService; set => _fontService = value; }

        public TextControl()
            : this(string.Empty, null, null, Colors.White, Vector2.Zero)
        {
        }

        public TextControl(string text, Font font, IFontService fontService)
            : this(text, font, fontService, Colors.White, Vector2.Zero)
        {
        }

        public TextControl(string text, Font font, IFontService fontService, Color color)
            : this(text, font, fontService, color, Vector2.Zero)
        {
        }

        public TextControl(string text, Font font, IFontService fontService, Color color, Vector2 position)
        {
            this.text = text;
            this.font = font;
            _fontService = fontService;
            this.Position = position;
            this.Color = color;
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);

            context.Painter.DrawString(font, Text, context.DrawOffset, Color);
        }

        override public Vector2 ComputeSize()
        {
            var size = _fontService.MeasureText(Text, this.font);
            return new Vector2(size.Width, size.Height);
        }
    }
}
