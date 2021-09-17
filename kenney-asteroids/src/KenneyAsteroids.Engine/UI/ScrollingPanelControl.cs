//-----------------------------------------------------------------------------
// ScrollingPanelControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using KenneyAsteroids.Engine.Screens;

using XGameTime = Microsoft.Xna.Framework.GameTime;

namespace KenneyAsteroids.Engine.UI
{
    public class ScrollingPanelControl : PanelControl
    {
        private ScrollTracker scrollTracker = new ScrollTracker();

        public override void Update(XGameTime gametime)
        {
            var size = ComputeSize();
            scrollTracker.CanvasRect = new Rectangle(scrollTracker.CanvasRect.X, scrollTracker.CanvasRect.Y, (int)size.X, (int)size.Y);
            scrollTracker.Update(gametime);

            base.Update(gametime);
        }

        public override void HandleInput(InputState input)
        {
            scrollTracker.HandleInput(input);
            base.HandleInput(input);
        }

        public override void Draw(DrawContext context)
        {
            // To render the scrolled panel, we just adjust our offset before rendering our child controls as
            // a normal PanelControl
            context.DrawOffset.Y = -scrollTracker.ViewRect.Y;
            base.Draw(context);
        }
    }
}
