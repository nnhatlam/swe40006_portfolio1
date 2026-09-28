using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using static SplashKitSDK.SplashKit;

namespace MyHappyCAt
{
    public abstract class Page : PageItems
    {
        public string Title { get; set; }
        public Color BackgroundColor { get; set; }
        public Color mainTextColor { get; set; } = Color.RGBColor(179, 51, 110);
        public int FontSize { get; set; }
        public Font Mainfont { get; protected set; }
        public Page(string title, Font mainFont)
        {
            Title = title;
            BackgroundColor = Color.RGBColor(252, 237, 244);
            Mainfont = mainFont;
        }

        public abstract void Draw();

        public abstract void Update();
    }
}
