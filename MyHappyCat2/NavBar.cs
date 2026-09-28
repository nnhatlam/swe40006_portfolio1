using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace MyHappyCAt
{
    public class NavBar: PageItems
    {
        private Color middleColor = Color.RGBColor(243, 204, 222); 
        private string _focus_img;
        private string _settings_img;
        private string _game_img;
        private Button _game_btn;
        private Button _settings_btn;
        private Button _focus_btn;
        
        
      
        public NavBar()
        {
            _focus_img = "focus.png";
            _settings_img = "setting.png";
            _game_img = "game.png";
            _focus_btn = new Button(5, 35, 50, 50, _focus_img);
            _settings_btn = new Button(5, 135, 50, 50, _settings_img);
            _game_btn = new Button(5, 245, 50, 50, _game_img);  
            
        }

        public void Draw()
        {
            SplashKit.FillRectangle(middleColor, 0, 0, 50, Program.ScreenHeight);
            _focus_btn.Draw();
            _game_btn.Draw();
            _settings_btn.Draw();
        }

        public void Update()
        {
            if (_focus_btn.IsMouseOver() && SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Program.CurrentPage = Program.Pages.Focus;
            }
            else if (_settings_btn.IsMouseOver() && SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Program.CurrentPage = Program.Pages.Setting;
            }
            else if (_game_btn.IsMouseOver() && SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Program.CurrentPage = Program.Pages.Game;
            }
        }
    }
}
