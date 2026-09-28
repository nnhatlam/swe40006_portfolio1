using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using SplashKitSDK;
using static SplashKitSDK.SplashKit;

namespace MyHappyCAt
{
    public class UserCredential
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Fish { get; set; }
        public string CatName { get; set; }
    }

    public class LoginPage : Page, PageItems
    {
        private Bitmap _background;
        private Button _loginButton;
        private Rectangle username_box = SplashKit.RectangleFrom(160, 140, 180, 22);
        private Rectangle password_box = SplashKit.RectangleFrom(160, 190, 180, 22);
        private string _username = "";
        private string _password = "";
        private bool _type_username = false;
        private bool _type_password = false;
        private string _input_username = "";
        private string _input_password = "";
        private LoginState _loginState;

        private enum LoginState
        {
            None,
            Checking,
            Success,
            Failed
        }

        public LoginPage(string name, Font mainFont) : base(name, mainFont)
        {
            _background = SplashKit.LoadBitmap("LoginBackground", "images/login_background.png");
            _loginButton = new Button((Program.ScreenWidth - 100)/2, 230, 100, 44, "login_button.png");
            _loginState = LoginState.None;
        }

        public override void Draw()
        {
            SplashKit.DrawBitmap(_background, 0, 0);
            SplashKit.DrawText("Username", mainTextColor, Mainfont, 12, 170, 125);
            _loginButton.Draw();
            SplashKit.DrawText("Password", mainTextColor, Mainfont, 12, 170, 172);
            SplashKit.DrawText("Login", mainTextColor, Mainfont, 20, (Program.ScreenWidth - SplashKit.TextWidth("Login", Mainfont, 20)) / 2, 243);
            SplashKit.DrawText(_username, mainTextColor, Mainfont, 12, username_box.X + 5, username_box.Y + 3);
            
            string hiddenPassword = new string('*', _password.Length);
            SplashKit.DrawText(hiddenPassword, mainTextColor, Mainfont, 12, password_box.X + 5, password_box.Y + 2);
            if (_type_username)
            {
                SplashKit.DrawText("|", Color.Black, Mainfont, 16, username_box.X + 5 + SplashKit.TextWidth(_username, Mainfont, 12), username_box.Y + 2);
            }
            if (_type_password)
            {
                SplashKit.DrawText("|", Color.Black, Mainfont, 16, password_box.X + 5 + SplashKit.TextWidth(_password, Mainfont, 12), password_box.Y + 2);
            }

            switch(_loginState)
            {
                case LoginState.Checking:
                    SplashKit.DrawText("Please enter your username and password", Color.Red, Mainfont, 11, (Program.ScreenWidth - SplashKit.TextWidth("Please enter your username and password", Mainfont, 11)) / 2, 280);
                    break;
                case LoginState.Success:
                    Console.WriteLine("Login successful!");
                    break;
                case LoginState.Failed:
                    SplashKit.DrawText("Login failed. Please try again.", Color.Red, Mainfont, 12, (Program.ScreenWidth - SplashKit.TextWidth("Login failed. Please try again.", Mainfont, 12)) / 2, 280);
                    break;
                default:
                    break;
            }
        }

        public override void Update()
        {
            // Check login status
            if (_loginButton.IsClicked())
            {

                if (_username != "" && _password != "")
                {
                    if (LogInCheck(_username, _password))
                    {
                        _loginState = LoginState.Success;
                        Program.Login = true;
                    }
                    else
                    {
                        _loginState = LoginState.Failed;
                    }
                }
                else
                {
                    _loginState = LoginState.Checking;
                }
            }

            //navigate through different input sections

           if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
               if (SplashKit.MouseX() >= username_box.X && SplashKit.MouseX() <= username_box.X + username_box.Width &&
                SplashKit.MouseY() >= username_box.Y && SplashKit.MouseY() <= username_box.Y + username_box.Height)
                {
                    if (!_type_username)
                    {
                        _type_username = true;
                        _type_password = false; // Stop typing password if username box is clicked
                        
                        SplashKit.StartReadingText(username_box);
                    }
                }
                else if (SplashKit.MouseX() >= password_box.X && SplashKit.MouseX() <= password_box.X + password_box.Width && SplashKit.MouseY() <= password_box.Y + password_box.Height)
                {
                    if (!_type_password)
                    {
                        _type_password = true;
                        _type_username = false; // Stop typing username if password box is clicked
                        _input_password = ""; // Clear the input buffer for password
                        _password = "";
                        SplashKit.StartReadingText(password_box);
                    }
                }
                else
                {
                    _type_password = false;
                    _type_username = false;
                    SplashKit.EndReadingText();
                }
            }

            if (_type_username)
            {
                HandleUsernameInput();
            }

            if (_type_password)
            {
                HandlePasswordInput();
            }
        }            
        
        public void HandleUsernameInput()
        {
            if (_username.Length < 25)
            {
                _input_username = SplashKit.TextInput();
                _username = _input_username;
            }
            if (SplashKit.KeyTyped(KeyCode.ReturnKey))
            {
                if (_type_username)
                {
                    _type_username = false;
                    _type_password = true; // Start typing password after pressing Enter in username box
                    SplashKit.EndReadingText(); // End reading username
                    _input_password = "";       // Clear password input buffer
                    _password = "";
                    SplashKit.StartReadingText(password_box);
                }                
            }
        }

        public void HandlePasswordInput()
        {
            if (_password.Length < 25)
            {
                _input_password = SplashKit.TextInput();
                _password = _input_password;
            }
            if (SplashKit.KeyTyped(KeyCode.ReturnKey))
            {
                if (_type_password)
                {
                    _type_password = false;
                    SplashKit.EndReadingText();
                    Console.WriteLine("Username:" + _username + "Password entered: " + _password);   // Log the password for debugging purposes
                }

            }
        }

        public bool LogInCheck(string usernameInput, string passwordInput)
        {
            try
            {
                string json = File.ReadAllText("users.json");
                var users = JsonSerializer.Deserialize<List<UserCredential>>(json);
                if (users == null || users.Any(u => u.Username == null || u.Password == null))
                {
                    throw new InvalidOperationException("Invalid data in users.json");   // avoid null password
                }

                foreach (var user in users)
                {
                    if (user.Username.Trim().Equals(usernameInput.Trim(), StringComparison.OrdinalIgnoreCase) &&
                        user.Password.Trim().Equals(passwordInput.Trim(), StringComparison.Ordinal))   //avoid white space issue
                    {
                        Program.CurrentUser = user;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading users.json: " + ex.Message);
                return false;
            }
        }

        

    }
}
