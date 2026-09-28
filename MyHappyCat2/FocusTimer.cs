using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using static SplashKitSDK.SplashKit;

namespace MyHappyCAt
{
    public class FocusTimer : Timer
    {

        public FocusTimer(int duration, float centerX, float centerY, float radius, Font mainFont) : base(duration, centerX, centerY, radius, mainFont)
        {
        }

        
        public override void Resume()
        {
            base.Resume();
            _startTime = SplashKit.CurrentTicks() - (_duration - _remainingTime) * 1000.0;
        }

        public override void Pause()
        {
            base.Pause();
            _startTime = SplashKit.CurrentTicks() - (_duration - _remainingTime) * 1000.0;
        }
        
    }
}
