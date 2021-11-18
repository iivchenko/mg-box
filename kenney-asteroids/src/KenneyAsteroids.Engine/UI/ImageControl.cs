//-----------------------------------------------------------------------------
// ImageControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using KenneyAsteroids.Engine.Graphics;
using System.Numerics;

namespace KenneyAsteroids.Engine.UI
{
    /// <summary>
    /// ImageControl is a control that displays a single sprite. By default it displays an entire texture.
    ///
    /// If a null texture is given, this control will use DrawContext.BlankTexture. This allows it to be
    /// used to draw solid-colored rectangles.
    /// </summary>
    public class ImageControl : Control
    {
        private Sprite _sprite;

        // Position within the source texture, in texels. Default is (0,0) for the upper-left corner.
        public Vector2 origin;

        // Size in texels of source rectangle. If null (the default), size will be the same as the size of the control.
        // You only need to set this property if you want texels scaled at some other size than 1-to-1; normally
        // you can just set the size of both the source and destination rectangles with the Size property.
        public Vector2? SourceSize;

        // Color to modulate the texture with. The default is white, which displays the original unmodified texture.
        public Color Color;

        // Texture to draw
        public Sprite Texture
        {
            get { return _sprite; }
            set
            {
                if (_sprite != value)
                {
                    _sprite = value;
                    InvalidateAutoSize();
                }
            }
        }

        public ImageControl() : this(null, Vector2.Zero)
        {
        }

        public ImageControl(Sprite sprite, Vector2 position)
        {
            _sprite = sprite;
            Position = position;
            Color = Colors.White;
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);
            var drawSprite = _sprite ?? context.BlankSprite;

            Vector2 actualSourceSize = SourceSize ?? Size;
            Rectangle sourceRectangle = new Rectangle
            (
                (int)origin.X,
                (int)origin.Y,
                (int)actualSourceSize.X,
                (int)actualSourceSize.Y
            );
            Rectangle destRectangle = new Rectangle
            (
                (int)context.DrawOffset.X,
                (int)context.DrawOffset.Y,
                (int)Size.X,
                (int)Size.Y
            );
            context.Painter.Draw(drawSprite, destRectangle, sourceRectangle, Color);
        }

        override public Vector2 ComputeSize()
        {
            if(_sprite!=null)
            {
                return new Vector2(_sprite.Width, _sprite.Height);
            }
            return Vector2.Zero;
        }
    }
}
