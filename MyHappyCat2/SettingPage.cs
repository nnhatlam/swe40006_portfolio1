using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SplashKitSDK.SplashKit;
using System.Text.Json;
using System.IO;
using System.Reflection.Metadata.Ecma335;


namespace MyHappyCAt
{
    public class SettingPage: Page, PageItems
    {
        private Bitmap _settingsBackground;
        private Bitmap _popupWindow;
        private Button _inputBox;
        private Button _changePassword;
        private Button _changeUsername;
        private Button _changeCatname;
        private Button _yes_button;
        private Button _no_button;
        private Color semiBlack;
        private string _newUsername = "";
        private string _input_username = "";
        private bool _popupPassword = false;
        private bool _popupUsername = false;
        private bool _popupCatName = false;
        private bool _typeUsername = false;
        private int _cursorBlinkCounter = 0;
        private const int CursorBlinkRate = 30;

        public SettingPage(string title, Font mainFont) : base("Settings", mainFont)
        {
            _settingsBackground = SplashKit.LoadBitmap("settings_background", "settings_background.png");
            _popupWindow = SplashKit.LoadBitmap("popup_window", "change_popup.png");
            _inputBox = new Button((Program.ScreenWidth - 200) / 2.0 + 40, (Program.ScreenHeight - 50) / 2.0 - 30, 159, 25, "input_box.png");
            _changePassword = new Button(60, 130, 137, 44, "change_button.png");
            _changeUsername = new Button(200, 130, 137, 44, "change_button.png");
            _changeCatname = new Button(350, 130, 137, 44, "change_button.png");
            semiBlack = Program.SemiBlack;
            _yes_button = new Button(130 + (Program.ScreenWidth - 200) / 2.0, 50 + (Program.ScreenHeight - 77) / 2.0, 25, 25, "yes_button.png");
            _no_button = new Button(50 + (Program.ScreenWidth - 200) / 2.0, 50 + (Program.ScreenHeight - 77) / 2.0, 25, 25, "no_button.png");
        }

        public override void Update()
        {
            if (_changeUsername.IsClicked() && !_popupUsername)
            {
                _popupUsername = true;
                SplashKit.OpenPopup("Change Username");
            }
            if (_changePassword.IsClicked() && !_popupPassword)
            {
                _popupPassword = true;
                SplashKit.OpenPopup("Change Password");
            }
            if (_changeCatname.IsClicked() && !_popupCatName)
            {
                _popupCatName = true;
                SplashKit.OpenPopup("Change Cat Name");
            }
            if (_popupUsername)
            {
                if (SplashKit.MouseClicked(MouseButton.LeftButton))
                {
                    if (_inputBox.IsMouseOver())
                    {
                        if (!_typeUsername)
                        {
                            _typeUsername = true;
                            _input_username = "";
                            SplashKit.StartReadingText(SplashKit.RectangleFrom(_inputBox.X, _inputBox.Y, 159, 25));
                        }
                    }
                    else
                    {
                        if (_typeUsername)
                        {
                            _typeUsername = false;
                            SplashKit.EndReadingText();
                        }
                    }
                }

                if (_yes_button.IsClicked())
                {
                    if (UpdateUsername(Program.CurrentUser.Username, _newUsername) && _newUsername != "")
                    {
                        Program.CurrentUser.Username = _newUsername; // Update the current user object
                        _popupUsername = false;
                        _input_username = "";
                        Console.WriteLine("Username updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Username already exists or is invalid.");
                    }
                }
                if (_no_button.IsClicked())
                {
                    _popupUsername = false;
                    _input_username = "";
                    Console.WriteLine("Username change cancelled.");
                    SplashKit.EndPopup("Change Username");
                }

                if (_typeUsername)
                {
                    HandleChangeUsernameInput();
                }
            }
        }

        public void HandleChangeUsernameInput()
        {
            if (_input_username.Length < 25)
            {
                _input_username = SplashKit.TextInput();
                _newUsername = _input_username;
            }
            if (SplashKit.KeyTyped(KeyCode.ReturnKey))
            {
                _typeUsername = false;
                SplashKit.EndReadingText();
                Console.WriteLine("New Username entered: " + _newUsername);
            }
        }


        public override void Draw()
        { 
            _settingsBackground.Draw(230, 20); // Draw the settings background
            SplashKit.DrawText("Your Cat's Name: "  + Program.CurrentUser.CatName, mainTextColor, Mainfont, 14, 100, 70);
            SplashKit.DrawText("User name: " + Program.CurrentUser.Username, mainTextColor, Mainfont, 14, 100, 100);
            SplashKit.DrawText("Fish: " + Program.CurrentUser.Fish, mainTextColor, Mainfont, 14, 100, 130);
            _changePassword.Draw();
            SplashKit.DrawText("Change Password", Color.White, Mainfont, 12, _changePassword.X + 15,_changePassword.Y + 15);
            _changeUsername.Draw();
            SplashKit.DrawText("Change Username", Color.White, Mainfont, 12, _changeUsername.X + 15, _changeUsername.Y + 15);
            _changeCatname.Draw();
            SplashKit.DrawText("Change Cat Name", Color.White, Mainfont, 12, _changeCatname.X + 15, _changeCatname.Y + 15);
            
            if (_popupUsername)
            {
                SplashKit.FillRectangle(semiBlack, 0, 0, Program.ScreenWidth, Program.ScreenHeight);
                SplashKit.DrawBitmap(_popupWindow, (Program.ScreenWidth - _popupWindow.Width) / 2 + 25, (Program.ScreenHeight - _popupWindow.Height) / 2);
                SplashKit.DrawText("Enter New Username:", mainTextColor, Mainfont, 12, (Program.ScreenWidth - SplashKit.TextWidth("Enter New Username:", Mainfont, 12)) / 2 + 20, 90);
                _inputBox.Draw();
                SplashKit.DrawText(_input_username, Color.Black, Mainfont, 12, _inputBox.X + 7, _inputBox.Y + 3);
                _cursorBlinkCounter++;
                if (_typeUsername && (_cursorBlinkCounter / CursorBlinkRate) % 2 == 0)
                {
                    SplashKit.DrawText("|", Color.Black, Mainfont, 14, _inputBox.X + 7 + SplashKit.TextWidth(_input_username, Mainfont, 12), _inputBox.Y + 3);
                }
                _yes_button.Draw();
                _no_button.Draw();
            }
        }


        // Write the new information to the Json file

        public bool UpdateUsername(string currentUsername, string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
                return false;

            string json = File.ReadAllText("users.json");
            var users = JsonSerializer.Deserialize<List<UserCredential>>(json);

            // Check for unique username (excluding current user)
            if (users.Any(u => u.Username.Equals(newUsername, StringComparison.OrdinalIgnoreCase) && !u.Username.Equals(currentUsername, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            var user = users.FirstOrDefault(u => u.Username.Equals(currentUsername, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                user.Username = newUsername;
                File.WriteAllText("users.json", JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
                return true;
            }
            return false;
        }


        public bool UpdateCatName(string newCatName)
        {
            string json = File.ReadAllText("users.json");
            var users = JsonSerializer.Deserialize<List<UserCredential>>(json);
            var user = users.FirstOrDefault(u => u.Username.Equals(Program.CurrentUser.Username, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                user.CatName = newCatName;
                File.WriteAllText("users.json", JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
                Program.CurrentUser.CatName = newCatName; // Update the current user object
                return true;
            }
            return false;
        }

        public bool ChangePassword(string newPassword)
        {
            string json = File.ReadAllText("users.json");
            var users = JsonSerializer.Deserialize<List<UserCredential>>(json);
            var user = users.FirstOrDefault(u => u.Username.Equals(Program.CurrentUser.Username, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                user.Password = newPassword;
                File.WriteAllText("users.json", JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
                return true;
            }
            return false;
        }
    }
}
