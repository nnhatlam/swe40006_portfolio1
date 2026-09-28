using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using static MyHappyCAt.Timer;
using static SplashKitSDK.SplashKit;



namespace MyHappyCAt
{
    public class FocusPage : Page, PageItems
    {
        Timer _focusTimer;
        private Color circleColor = Color.RGBColor(236, 127, 176); // Dark pink color for the circle
        private Color textColor = Color.RGBColor(179, 51, 110); // Main text color
        private Color semiBlack = Program.SemiBlack;
        private string start_img = "start_timer.png";
        private string pause_img = "pause_timer.png";
        private string button_img;
        private Button start_stop;
        private Bitmap focusCat;
        private string studyCatImg = "study_cat.png";
        private string sadCatImg = "sad_cat.png";
        private Note _note_list;
        private string reset_on = "reset_timer_on.png";
        private string reset_off = "reset_timer_off.png";
        private Button _reset_button;
        private Button _yes_button;
        private Button _no_button;
        private Button _long_session_button;
        private Button _short_session_button;
        private int _short_time = 2; // NEED TO CHANGE TO 25 MINS IN ACTUAL APPLICATION
        private int _long_time = 4;  // NEED TO CHANGE TO 45 MINS IN ACTUAL APPLICATION
        private int _focus_duration;
        private Bitmap _popup_window;
        private bool _isPopupOpen = false;

        public FocusPage(string title, Font mainFont) : base("Focus", mainFont)
        {
            _focus_duration = _short_time;
            _focusTimer = new FocusTimer(_focus_duration * 60, 150, 140, 75, Mainfont);
            button_img = start_img;
            start_stop = new Button(80, 230, 35, 35, button_img);
            focusCat = SplashKit.LoadBitmap("image" + System.IO.Path.GetFileNameWithoutExtension(studyCatImg), studyCatImg);
            _note_list = new Note(260, 50, Mainfont, textColor);
            _reset_button = new Button(150, 230, 35, 35, "reset_timer_off.png");
            _yes_button = new Button(130 + (Program.ScreenWidth - 200) / 2.0, 50 + (Program.ScreenHeight - 77) / 2.0, 25, 25, "yes_button.png");
            _no_button = new Button(50 + (Program.ScreenWidth - 200) / 2.0, 50 + (Program.ScreenHeight - 77) / 2.0, 25, 25, "no_button.png");
            _popup_window = SplashKit.LoadBitmap("image popup_window", "popup_window.png");
            _long_session_button = new Button(90, 30, 50, 20, "long_session.png");
            _short_session_button = new Button(140, 30, 50, 20, "short_session.png");
        }

        public override void Draw()
        {
            _focusTimer.DrawBackground();
            
            _focusTimer.DrawTimer(circleColor, textColor);
            SplashKit.DrawBitmap(focusCat, 90, 85);
            start_stop.Draw();
            _note_list.Draw();
            _reset_button.Draw();
            _long_session_button.Draw();
            SplashKit.DrawText("Long", textColor, Mainfont, 14, 100, 32);
            _short_session_button.Draw();
            SplashKit.DrawText("Short", textColor, Mainfont, 14, 145, 32);

            //if (_focusTimer._timerState == Timer.TimerState.Running)
            //{
            //    SplashKit.DrawText("Focus Mode: ON", textColor, Mainfont, 16, 80, 40);
            //}
            //else
            //{
            //    SplashKit.DrawText("Focus Mode: OFF", textColor, Mainfont, 16, 80, 40);
            //}

            if (_isPopupOpen && SplashKit.StartPopup("Reset Timer"))
            { 
                SplashKit.FillRectangle(semiBlack, 0, 0, Program.ScreenWidth, Program.ScreenHeight);    // overlay semi-black background
                SplashKit.DrawBitmap(_popup_window, (Program.ScreenWidth - 230) / 2, (Program.ScreenHeight - 80)/2);                 
                SplashKit.DrawText("Are you sure you want to reset", textColor, Mainfont, 11, 15 + (Program.ScreenWidth - 200) / 2, 15 + (Program.ScreenHeight - 77) / 2);
                SplashKit.DrawText("the timer? (MINUS FISH)", textColor, Mainfont, 11, 15 + (Program.ScreenWidth - 200) / 2, 30 + (Program.ScreenHeight - 77) / 2);
                _yes_button.Draw();
                _no_button.Draw();

                SplashKit.EndPopup("Reset Timer");
            }
        }
        public override void Update()
        {
            if (_focusTimer._timerState == TimerState.Finished)
            {
                _reset_button.ImageSrc = reset_off;
                start_stop.ImageSrc = start_img;
            }
        
            if (_focusTimer._timerState == TimerState.NotStarted)
            {
                if (_long_session_button.IsClicked())
                {
                    _focus_duration = _long_time;
                    _focusTimer._duration = _focus_duration * 60; // update timer duration in seconds
                    _focusTimer.Reset(); // reset timer to apply new duration
                }
                else if (_short_session_button.IsClicked())
                {
                    _focus_duration = _short_time;
                    _focusTimer._duration = _focus_duration * 60; // update timer duration in seconds
                    _focusTimer.Reset(); // reset timer to apply new duration
                }
            }
            _focusTimer.Update();
            _note_list.Update();

            if (start_stop.IsClicked())
            {
                switch (_focusTimer._timerState)
                {
                    case TimerState.NotStarted:
                        _focusTimer.Start();
                        start_stop.ImageSrc = pause_img;
                        _reset_button.ImageSrc = reset_on;
                        focusCat = SplashKit.LoadBitmap("image" + System.IO.Path.GetFileNameWithoutExtension(studyCatImg), studyCatImg);
                        break;
                    case TimerState.Running:
                        _focusTimer.Pause();
                        start_stop.ImageSrc = start_img;
                        focusCat = SplashKit.LoadBitmap("image" + System.IO.Path.GetFileNameWithoutExtension(sadCatImg), sadCatImg);
                        break;
                    case TimerState.Paused:
                        _focusTimer.Resume();
                        start_stop.ImageSrc = pause_img;
                        focusCat = SplashKit.LoadBitmap("image" + System.IO.Path.GetFileNameWithoutExtension(studyCatImg), studyCatImg);
                        break;
                    case TimerState.Finished:
                        break;
                }

            }

            if (_reset_button.IsClicked() && !_isPopupOpen && (_focusTimer._timerState == TimerState.Running))
            {
                _isPopupOpen = true;
                SplashKit.OpenPopup("Reset Timer");
            }

            if (_isPopupOpen)
            {
                if (_yes_button.IsClicked())
                {
                    _focusTimer.Reset();
                    _isPopupOpen = false;
                    start_stop.ImageSrc = start_img;
                    focusCat = SplashKit.LoadBitmap("image" + System.IO.Path.GetFileNameWithoutExtension(studyCatImg), studyCatImg);
                    _reset_button.ImageSrc = reset_off;

                }
                else if (_no_button.IsClicked())
                {
                    _isPopupOpen = false;
                }
            }


        }
    }
}
