using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using static MyHappyCAt.Timer;
using static SplashKitSDK.SplashKit;

namespace MyHappyCAt
{
    public abstract class Timer
    {
        internal int _duration;
        internal double _elapsedTime;
        internal double _startTime;
        internal double _remainingTime;
        internal double _pausedTime;
        internal float _centerX;
        internal float _centerY;
        internal float _radius;
        private Color backgroundColor = Color.RGBColor(252, 237, 244);
        private Color middleColor = Color.RGBColor(243, 204, 222); // Light pink background color
        private Font _mainFont;
        private SoundEffect _timerSound;


        public enum TimerState
        {
            NotStarted,
            Running,
            Paused,
            Finished
        }
        public TimerState _timerState;

        public Timer(int duration, float centerX, float centerY, float radius, Font mainFont)  // duration in minutes
        {
            _duration = duration;     // in seconds         
            _remainingTime = duration;
            _centerX = centerX;
            _centerY = centerY;
            _radius = radius;
            _timerState = TimerState.NotStarted;
            _mainFont = mainFont;
            _timerSound = SplashKit.LoadSoundEffect("timer_sound", "timer_sound.mp3");
        }

        public void DrawBackground()
        {
           
            SplashKit.DrawCircle(middleColor, _centerX - 10, _centerY, _radius);
                       
        }

        public void DrawTimer(Color circleColor, Color textColor)
        {
            
            double percentage = _remainingTime / _duration;
            float sweepAngle = (float)(percentage * 360); // Total sweep angle
            float startAngle = 270; // Top of the circle (12 o'clock position)
            float endAngle = startAngle + sweepAngle; // End of the sweep

            Point2D centerPoint = SplashKit.PointAt(_centerX, _centerY);

            for (float i = startAngle; i < endAngle; i += 0.7f)
            {
                double angleRad1 = i * Math.PI / 180.0; // convert to radian
                double angleRad2 = (i + 0.7f) * Math.PI / 180.0;


                Point2D point1 = SplashKit.PointAt(
                    _centerY + (float)(_radius * Math.Cos(angleRad1)),
                    _centerY + (float)(_radius * Math.Sin(angleRad1))
                );


                Point2D point2 = SplashKit.PointAt(
                    _centerY + (float)(_radius * Math.Cos(angleRad2)),
                    _centerY + (float)(_radius * Math.Sin(angleRad2))
                );


                SplashKit.FillTriangle(circleColor, centerPoint.X, centerPoint.Y, point1.X, point1.Y, point2.X, point2.Y);
            }

            SplashKit.FillCircle(backgroundColor, _centerX - 9, _centerY + 1, _radius - 26);
            SplashKit.DrawCircle(middleColor, _centerX - 9, _centerY + 1, _radius - 26);

            int hours = (int)(_remainingTime / 3600);
            int minutes = (int)(_remainingTime / 60);
            int seconds = (int)(_remainingTime % 60);


            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

            //Set Color and fontsize
            int fontSize = 16;


            if (_remainingTime <= 0)
            {
                timeString = "Done";
                textColor = SplashKit.ColorRed();
                fontSize = 16;
                
            }
            float textX = _centerX - _radius/2 + 10;
            float textY = _centerY +_radius - 45; // Center the text vertically

            SplashKit.DrawText(timeString, textColor, _mainFont, fontSize, textX, textY);
            
        }

        public void Update()
        {
            if (_timerState == TimerState.Running)
            {
                _elapsedTime = (SplashKit.CurrentTicks() - _startTime) / 1000.0; // Convert milliseconds to seconds
                _remainingTime = _duration - _elapsedTime; // Calculate remaining time
                if (_remainingTime < 0)
                {
                    _remainingTime = 0;
                    _timerState = TimerState.Finished; // Stop the timer when time is up
                    _timerSound.Play();
                    Reset(); // Reset the timer after it finishes
                }
            }

        }

        public virtual void Start() 
        { 
            if (_timerState == TimerState.NotStarted || _timerState == TimerState.Finished)
            {
                _startTime = SplashKit.CurrentTicks();
                _remainingTime = _duration;
                _timerState = TimerState.Running;

            }

        }

        public virtual void Resume()
        {
            if (_timerState == TimerState.Paused)
            {
                _timerState = TimerState.Running;
                _startTime = SplashKit.CurrentTicks() - (_duration - _pausedTime)* 1000.0; // Adjust start time based on remaining time
            }
        }

        public virtual void Pause() 
        { 
            if (_timerState == TimerState.Running)
            {
                _pausedTime = _remainingTime;
                _timerState = TimerState.Paused;

            }
        }        

        public virtual void Reset()
        {
            _remainingTime = _duration;
            _startTime = SplashKit.CurrentTicks();
            _timerState = TimerState.NotStarted;
        }

    }
}



