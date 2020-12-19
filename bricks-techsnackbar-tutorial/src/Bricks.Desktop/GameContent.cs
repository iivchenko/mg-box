using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop
{
    public sealed class GameContent
    {
        private readonly ContentManager _content;

        public GameContent(ContentManager content)
        {
            _content = content;
        }

        public Texture2D ImgPixel => _content.Load<Texture2D>("Images/Pixel");
        public Texture2D ImgPaddle => _content.Load<Texture2D>("Images/Paddle");
        public Texture2D ImgBrick => _content.Load<Texture2D>("Images/Brick");
        public Texture2D ImgBall => _content.Load<Texture2D>("Images/Ball");
        public SoundEffect StartSound => _content.Load<SoundEffect>("Sounds/Start");
        public SoundEffect BrickSound => _content.Load<SoundEffect>("Sounds/Brick");
        public SoundEffect PaddleBounceSound => _content.Load<SoundEffect>("Sounds/PaddleBounce");
        public SoundEffect WallBounceSound => _content.Load<SoundEffect>("Sounds/WallBounce");
        public SoundEffect MissSound => _content.Load<SoundEffect>("Sounds/Miss");
        public SpriteFont LabelFont => _content.Load<SpriteFont>("Fonts/Arial20");
    }
}
