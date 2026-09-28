using Calculator;
using MyHappyCAt;
using SplashKitSDK;
using System;
using System.ComponentModel.Design;
using Calculator;

namespace MyHappyCAt
{
    public class Program
    {
        
        public static int ScreenWidth { get; } = 500;
        public static int ScreenHeight { get;} = 325;
        private static NavBar _menuBar;
        public static bool Login { get; set; } = false;
        public static UserCredential CurrentUser { get; set; }
        public static Color SemiBlack { get; } = SplashKit.RGBAColor(0, 0, 0, 128);

        public enum Pages
        {
            Login,
            Focus,
            Setting,
            Game
        }
        public static Pages CurrentPage { get; set; } = Pages.Focus;
        public static Font AppMainFont { get; private set; }
        public static string DllStatus { get; private set; } = string.Empty;

        public static void Main(string[] args)
        {
            try
            {
                DllStatus = CatHelper.GetCatMood(5);
                Console.WriteLine($"[DLL Dependency Loaded] Mood status: {DllStatus}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dependency: {ex.Message}");
            }
            Color backgroundColor = Color.RGBColor(252, 237, 244);
            AppMainFont = SplashKit.LoadFont("MainFont", "fonts/perfect_dos.ttf");
            _menuBar = new NavBar();

            SplashKit.OpenWindow("My Happy Cat", ScreenWidth, ScreenHeight);
         

            FocusPage focusPage = new FocusPage("Focus", AppMainFont);
            LoginPage login = new LoginPage("Login", AppMainFont);
            GamePage gamePage = new GamePage("Game", AppMainFont);
            SettingPage settingPage = new SettingPage("Settings", AppMainFont);
            while (!SplashKit.WindowCloseRequested("My Happy Cat"))
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen(backgroundColor); 
                if (!Login)
                {
                    login.Update();
                    login.Draw();
                }
                else
                {
                    _menuBar.Draw();
                    if (!(CurrentPage == Pages.Game && !(gamePage._gameCenter._currentGameState == GameCenter.GameState.Playing)))
                    {
                        _menuBar.Update();
                    }
                    _menuBar.Update();
                    switch (CurrentPage)
                    {
                        case Pages.Focus:
                            focusPage.Update();
                            focusPage.Draw();
                            break;
                        case Pages.Setting:
                            settingPage.Update();
                            settingPage.Draw();
                            break;
                        case Pages.Game:
                            gamePage.Draw();
                            gamePage.Update();
                            break;                    
                    }
                }

                

                SplashKit.RefreshScreen();
                SplashKit.Delay(10); // Delay to control the frame rate
            }

            SplashKit.CloseWindow("My Happy Cat");
        }
    }
}