using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SplashKitSDK.SplashKit;



namespace MyHappyCAt
{
    public class GamePage : Page, PageItems
    {
        public GameCenter _gameCenter;
        private Bitmap _startBackground;
        private Bitmap _gameBackground;
        public GamePage(string title, Font mainFont) : base("Game", mainFont)      
        {
            // Initialize the game page with necessary components
            _gameCenter = new GameCenter();
            _startBackground = SplashKit.LoadBitmap("start_background", "start_background.png");
            _gameBackground = SplashKit.LoadBitmap("game_background", "game_background.png");
        }

        public override void Draw()
        {
            if (_gameCenter._currentGameState == GameCenter.GameState.NotStarted)
            {
                // Draw the start background if the game has not started
                SplashKit.DrawBitmap(_startBackground, 120, 0);
                if (Program.CurrentUser != null)
                {
                    SplashKit.DrawText("Welcome back, " + Program.CurrentUser.CatName + "!", mainTextColor, Mainfont, 25, 120, 150);
                }
                else
                {
                    SplashKit.DrawText("Welcome back, Guest!", mainTextColor, Mainfont, 25, 100, 150);

                }
                SplashKit.DrawText("Press Space to Start", Color.Black, 100, 200);
            }
            else if (_gameCenter._currentGameState == GameCenter.GameState.Playing)
            {
                _gameBackground.Draw(0, 0); // Draw the game background

                SplashKit.DrawText("playing", mainTextColor, 100, 100);
                _gameCenter.DrawPlaying();
            }
            else if (_gameCenter._currentGameState == GameCenter.GameState.EndGame)
            {
                // Draw the end game message
                SplashKit.DrawText("Game Over! Press R to restart.", Color.Red, 100, 100);
            }
            else if (_gameCenter._currentGameState == GameCenter.GameState.Pause)
            {
                _gameBackground.Draw(50, 0);
                SplashKit.DrawText("Game Paused. Press R to resume.", Color.Red, 100, 100);
            }
        }

        public override void Update()
        {
            _gameCenter.Update();
            if (SplashKit.KeyTyped(KeyCode.SpaceKey) && _gameCenter._currentGameState == GameCenter.GameState.NotStarted)
            {
                // Start the game when the space key is pressed
                _gameCenter.StartGame();
            }
            else if (SplashKit.KeyTyped(KeyCode.RKey) && _gameCenter._currentGameState == GameCenter.GameState.EndGame)
            {
                // Restart the game when the R key is pressed
                _gameCenter = new GameCenter();
            }
        }

    }
}
