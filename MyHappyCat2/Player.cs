using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHappyCAt
{
    public class Player
    {
        private Bitmap _CatSpriteSheet;
        private Sprite _CatSprite;
        private string _spriteRight = "cat_spritesheet.png";

        private double _x;
        private double _y;
        private double _speed;
        private double _velocityY;
        private string _direction;
        private int _health;
        private Animation _currentAnimation;
        private AnimationScript _cat_animation;

        private Animation _idleAnimation;
        private Animation _walkAnimation;

        private const double Gravity = 0.5; // Gravity constant for vertical movement
        private const double JumpForce = - 10.0; // negative because it is upward movement

        private bool _isOnGround;

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public bool IsAlive => Health > 0;

        public Player()
        {
            _x = Program.ScreenWidth / 2;
            _y = Program.ScreenHeight / 2;
            _speed = 2.0;
            _velocityY = 0;
            _health = 100;
      

            _CatSpriteSheet = SplashKit.LoadBitmap("CatSpriteSheet", _spriteRight);
            SplashKit.BitmapSetCellDetails(_CatSpriteSheet, 56, 42, 2, 3, 6);  // width, height, rows, columns total frames

            _cat_animation = SplashKit.LoadAnimationScript("cat_sprite", "cat_sprite.txt");

            _idleAnimation = SplashKit.CreateAnimation(_cat_animation, "cat_idle");
            _walkAnimation = SplashKit.CreateAnimation(_cat_animation, "cat_walk");

            _CatSprite = SplashKit.CreateSprite(_CatSpriteSheet, _cat_animation);

            _CatSprite.X = _x;
            _CatSprite.Y = _y;
            _CatSprite.StartAnimation("cat_idle");
            _isOnGround = true;
        }

        public void HandleInput(Map map)
        {
            double dx = 0;

            if (SplashKit.KeyDown(KeyCode.UpKey) && _isOnGround)
            {
                _velocityY = JumpForce;
                _isOnGround = false;
            }
            if (SplashKit.KeyDown(KeyCode.LeftKey))
            {
                dx -= _speed;
            }
            if (SplashKit.KeyDown(KeyCode.RightKey))
            {
                dx += _speed;
            }

            // Horizontal collision check
            if (dx != 0)
            {
                double newX = _x + dx;
                double minX = 0;
                double maxX = map.WidthTiles() * map.TileSize - _CatSprite.Width;
                newX = Math.Max(minX, Math.Min(newX, maxX));

                Point2D leftFoot = new Point2D() { X = newX, Y = _y + _CatSprite.Height - 1 };
                Point2D rightFoot = new Point2D() { X = newX + _CatSprite.Width - 1, Y = _y + _CatSprite.Height - 1 };

                if (!map.IsSolid(leftFoot) && !map.IsSolid(rightFoot))
                {
                    _x = newX;
                }
            }
        }

        public void Update(Map map)
        {
            // Apply gravity
            _velocityY += Gravity;
            double newY = _y + _velocityY;

            // limit map borders
            double minY = 0;
            double maxY = map.HeightTiles() * map.TileSize - _CatSprite.Height;
            newY = Math.Max(minY, Math.Min(newY, maxY));

            // Vertical collision check (falling or jumping)
            Point2D leftFoot = new Point2D() { X = _x, Y = newY + _CatSprite.Height - 1 };
            Point2D rightFoot = new Point2D() { X = _x + _CatSprite.Width - 1, Y = newY + _CatSprite.Height - 1 };

            if (_velocityY > 0) // Falling
            {
                if (map.IsSolid(leftFoot) || map.IsSolid(rightFoot))
                {
                    int tileSize = map.TileSize;
                    _y = Math.Floor((leftFoot.Y) / tileSize) * tileSize - _CatSprite.Height;
                    _velocityY = 0;
                    _isOnGround = true;
                }
                else
                {
                    _y = newY;
                    _isOnGround = false;
                }
            }
            else // Jumping up
            {
                Point2D headLeft = new Point2D() { X = _x, Y = newY };
                Point2D headRight = new Point2D() { X = _x + _CatSprite.Width - 2, Y = newY };
                if (map.IsSolid(headLeft) || map.IsSolid(headRight))
                {
                    _velocityY = 0;
                }
                else
                {
                    _y = newY;
                }
            }

            // Clamp _y again after collision logic
            _y = Math.Max(0, Math.Min(_y, map.HeightTiles() * map.TileSize - _CatSprite.Height));

            _CatSprite.X = _x;
            _CatSprite.Y = _y;

            _CatSprite.Update();
        }

        public void Draw(double offsetX, double offsetY)
        {
            _CatSprite.X = _x;
            _CatSprite.Y = _y;
            _CatSprite.Draw(offsetX, offsetY);
            SplashKit.DrawText($"Health: {Health}", Color.Black, 10, 10);
        }

        public Sprite Sprite => _CatSprite;
    }
}
