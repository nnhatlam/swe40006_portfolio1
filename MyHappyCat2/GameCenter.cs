using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static SplashKitSDK.SplashKit;

namespace MyHappyCAt
{
    public class GameCenter
    {
        public enum GameState
        {
            NotStarted,
            Playing,
            Pause,
            EndGame
        }

        private Player _player;
        private Map _map;
        public GameState _currentGameState;
        private SplashKitSDK.Timer _gameTimer;
        // Center the player on the screen

        public GameCenter()
        {
           _player = new Player();

            _map = new Map(60, "map.txt");
            _currentGameState = GameState.NotStarted;
            _gameTimer = SplashKit.CreateTimer("GameTimer"); 
        }

        public void StartGame()
        {
            _currentGameState = GameState.Playing;
            _gameTimer.Start();            
        }

        public void Update()
        {
            if (_currentGameState == GameState.Playing)
            {
                  // move player
                _player.HandleInput(_map);           
                _player.Update(_map);

                if (!_player.IsAlive)
                {
                    _currentGameState = GameState.EndGame;
                    _gameTimer.Reset();
                }
                if (KeyDown(KeyCode.PKey))
                {
                    _currentGameState = GameState.Pause;
                }
            }
            
        }
        public void DrawPlaying()
        {
            double offsetX = Program.ScreenWidth / 2 - _player.Sprite.X;
            double offsetY = Program.ScreenHeight / 2 - _player.Sprite.Y;

            _map.Draw();
            _player.Draw(offsetX, offsetY);
           
            if (_currentGameState == GameState.Playing)
            {
                SplashKit.DrawText($"Time: {Math.Round(_gameTimer.Ticks / 1000.0, 2)}s", Color.White, 10, 10);
            }
        }

        public void DrawEndGame()
        {
            SplashKit.DrawText("Game Over! Press R to restart.", Color.Red, 100, 100);
            SplashKit.DrawText($"Total Time: {Math.Round(_gameTimer.Ticks / 1000.0, 2)}s", Color.Black, 100, 150);
        }

        public void DrawPause()
        {
            SplashKit.DrawText("Game Paused! Press P to resume.", Color.Yellow, 100, 100);
            SplashKit.DrawText($"Time: {Math.Round(_gameTimer.Ticks / 1000.0, 2)}s", Color.Black, 10, 10);
        }
        
    }
}
