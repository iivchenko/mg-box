//-----------------------------------------------------------------------------
// DrawContext.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
using KenneyAsteroids.Engine.Graphics;
using System.Numerics;

namespace KenneyAsteroids.Engine.UI
{
    /// <summary>
    /// DrawContext is a collection of rendering data to pass into Control.Draw().
    /// By passing this data into each Draw call, we controls can access necessary
    /// data when they need it, without introducing dependencies on top-level object
    /// like ScreenManager.
    /// </summary>
    public struct DrawContext
    {
        public IViewport Viewport { get; set; }

        public IPainter Painter { get; set; }

        /// <summary>
        /// A single-pixel white texture, useful for drawing boxes and lines within a SpriteBatch.
        /// </summary>
        public Sprite BlankSprite;

        /// <summary>
        /// Positional offset to draw at. Note that this is a simple positional offset rather
        /// than a full transform, so this API doesn't easily support full heirarchical transforms.
        ///
        /// A control's position will already be added to this vector when Control.Draw() is called.
        /// </summary>
        public Vector2 DrawOffset;
    }
}
