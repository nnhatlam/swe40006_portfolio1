using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using static SplashKitSDK.SplashKit;

namespace MyHappyCAt
{
    public class Button
    {
        private double _x;
        private double _y;
        private int _width;
        private int _height;
        private string _imageSrc;
        private string _hoverImage;
        private Bitmap buttonBitmap;
        private Bitmap hoverBitmap;
        private SoundEffect _clickSound;


        public Button (double x, double y, int width, int height, string imageSrc)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _imageSrc = imageSrc;
           
            buttonBitmap = SplashKit.LoadBitmap("button" + System.IO.Path.GetFileNameWithoutExtension(_imageSrc), _imageSrc);
            _clickSound = SplashKit.LoadSoundEffect("click_sound", "click_sound.mp3");
        }

        public Button (double x, double y, int width, int height, string imageSrc, string hoverImage)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _imageSrc = imageSrc;
            _hoverImage = hoverImage;

            buttonBitmap = SplashKit.LoadBitmap("button" + System.IO.Path.GetFileNameWithoutExtension(_imageSrc), _imageSrc);
            hoverBitmap = SplashKit.LoadBitmap("button_hover" + System.IO.Path.GetFileNameWithoutExtension(_hoverImage), _hoverImage);

            _clickSound = SplashKit.LoadSoundEffect("click_sound", "click_sound.mp3");

        }

        public bool IsMouseOver()
        {
            Point2D mousePosition = SplashKit.MousePosition();
            
            return mousePosition.X >= _x && mousePosition.X <= _x + _width &&
                   mousePosition.Y >= _y && mousePosition.Y <= _y + _height;
        }     
        
        public void Draw()
        {      
            if (IsMouseOver())
            {
                SplashKit.DrawBitmap("button" + System.IO.Path.GetFileNameWithoutExtension(_imageSrc), _x, _y - 2);
                if (_hoverImage != null)
                {
                    hoverBitmap.Draw(_x, _y - 2);
                }
                else
                {
                    buttonBitmap.Draw(_x, _y -2);
                }
            }
            else
            {
                SplashKit.DrawBitmap("button" + System.IO.Path.GetFileNameWithoutExtension(_imageSrc), _x, _y);
            }

        }

        public string ImageSrc
        {
            get { return _imageSrc; }
            set
            {
                _imageSrc = value;
                buttonBitmap = SplashKit.LoadBitmap("button" + System.IO.Path.GetFileNameWithoutExtension(_imageSrc), _imageSrc);
            }
        }

        public bool IsClicked()
        {
            if (SplashKit.MouseClicked(MouseButton.LeftButton) && IsMouseOver())
            {
                _clickSound.Play();
                return true;
            }
            return false;
        }


        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }
}
