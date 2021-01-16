using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CatapultWars.Android.Utility
{
    public sealed class Animation
    {
        // Animation variables
        Texture2D animatedCharacter;
        Point sheetSize;
        Point currentFrame;
        public Point FrameSize { get; set; }

        public int FrameCount
        {
            get { return sheetSize.X * sheetSize.Y; }
        }

        public Vector2 Offset { get; set; }

        public int FrameIndex
        {
            get
            {
                return sheetSize.X * currentFrame.Y + currentFrame.X;
            }
            set
            {
                if (value >= sheetSize.X * sheetSize.Y + 1)
                {
                    throw new InvalidOperationException(
                        "Specified frame index exeeds available frames");
                }

                currentFrame.Y = value / sheetSize.X;
                currentFrame.X = value % sheetSize.X;
            }
        }

        public bool IsActive { get; private set; }

        public Animation(Texture2D frameSheet, Point size, Point frameSheetSize)
        {
            animatedCharacter = frameSheet;
            FrameSize = size;
            sheetSize = frameSheetSize;
            Offset = Vector2.Zero;
        }

        public void Update()
        {
            if (IsActive)
            {
                if (FrameIndex >= FrameCount - 1)
                {
                    IsActive = false;
                    FrameIndex = FrameCount - 1; // Stop at last frame 
                }
                else
                {
                    // Remember that updating "currentFrame" will also
                    // update the FrameIndex property.

                    currentFrame.X++;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                        currentFrame.Y++;
                    }
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffect)
        {
            Draw(spriteBatch, position, 1.0f, spriteEffect);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects spriteEffect)
        {
            spriteBatch.Draw(animatedCharacter, position + Offset, new Rectangle(
                    FrameSize.X * currentFrame.X,
                    FrameSize.Y * currentFrame.Y,
                    FrameSize.X,
                    FrameSize.Y),
                    Color.White, 0f, Vector2.Zero, scale, spriteEffect, 0);
        }

        public void PlayFromFrameIndex(int frameIndex)
        {
            FrameIndex = frameIndex;
            IsActive = true;
        }
    }
}