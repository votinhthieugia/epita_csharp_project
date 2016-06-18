using FoodMaze.Scripts.Game.Objects;
using FoodMaze.Scripts.Game.Objects.Touches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace FoodMaze.Scripts.Screens
{
    class ScreenManager
    {
        private static ScreenManager instance;
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScreenManager();
                }

                return instance;
            }
        }

        private ScreenId currentScreenId;
        private BaseScreen currentScreen;
        private BaseScreen[] screens;
        private DispatcherTimer timer;
        private DateTimeOffset lastTime;

        private ScreenManager()
        {
            currentScreenId = ScreenId.Nil;
            screens = new BaseScreen[(int)ScreenId.Count];

            lastTime = DateTimeOffset.Now;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(170000);
            timer.Tick += Tick;
            timer.Start();
        }

        private void Tick(object sender, object e)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            TimeSpan span = now - lastTime;
            lastTime = now;
            float elapsedSeconds = Math.Min(span.Milliseconds / 1000f, 0.05f);
            Update(elapsedSeconds);
        }

        private void Update(float elapsedSeconds)
        {
            currentScreen.UpdateTouch(TouchManager.Instance.touches.ToArray<Touch>(), TouchManager.Instance.touches.Count);
            currentScreen.Update(elapsedSeconds);
            TouchManager.Instance.RemoveEndedTouches();
        }

        public void Init()
        {
            Show(ScreenId.Game);
        }

        public void Show(ScreenId screenId)
        {
            if (currentScreen != null) {
                currentScreen.Hide();
            }

            if (currentScreenId != screenId)
            {
                currentScreenId = screenId;
                currentScreen = FindScreen(currentScreenId); ;
                if (currentScreen == null)
                {
                    throw new NotImplementedException();
                }
            }

            currentScreen.Show();
        }

        private BaseScreen FindScreen(ScreenId screenId)
        {
            if (screens[(int)currentScreenId] != null)
            {
                return screens[(int)currentScreenId];
            }

            BaseScreen screen = null;
            switch (screenId)
            {
                case ScreenId.Splash:
                    break;
                case ScreenId.Game:
                    screen = new GameScreen(Context.Instance.Drawer);
                    screen.Init();
                    screens[(int)screenId] = screen;
                    break;       
            }
            return screen;
        }
    }
}
