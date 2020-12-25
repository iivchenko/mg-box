using Bricks.Desktop.Engine;
using Bricks.Desktop.Entities;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Bricks.Desktop.GamePlay
{
    public sealed class GamePlayContext
    {
        // Services
        public ContentManager Content { get; set; }
        public GraphicsDevice Graphics { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        // Content
        public Texture2D Piexel { get; set; }
        public Texture2D BrickSprite { get; set; }
        public Texture2D PaddleSprite { get; set; }
        public Texture2D BallSprite { get; set; }

        public SoundEffect StartSound { get; set; }
        public SoundEffect BrickSound { get; set; }
        public SoundEffect PaddleBounceSound { get; set; }
        public SoundEffect WallBounceSound { get; set; }
        public SoundEffect MissSound { get; set; } // TODO: USE IT!

        public SpriteFont LabelFont { get; set; }

        // Game Entities
        public GameBorder GameBorder { get; set; }
        public Ball Ball { get; set; }
        public Ball StaticBall { get; set; }
        public Paddle Paddle { get; set; }
        public List<Brick> Wall { get; set; }

        public List<IEntity> Entities { get; set; }

        // Game State
        public int ScreenWidth { get; set; } = 0;
        public int ScreenHeight { get; set; } = 0;
        public MouseState OldMouseState { get; set; }
        public KeyboardState OldKeyboardState { get; set; }
        public int BallsRemaining { get; set; }
    }
}
