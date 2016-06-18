using FoodMaze.Scripts.Game.Core;
using FoodMaze.Scripts.Game.Objects;
using FoodMaze.Scripts.Game.Objects.Touches;
using FoodMaze.Scripts.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FoodMaze.Scripts.Screens
{
    class GameScreen : BaseScreen
    {
        public enum State {
            Menu,
            Playing
        };

        private GamePlay gamePlay;
        private State state = State.Menu;
        private List<UIButton> buttons;
        
        public GameScreen(IDrawer drawer)
        {
            this.drawer = drawer;
            gamePlay = GamePlay.Instance;
            InitButtons();
        }

        private void InitButtons()
        {
            buttons = new List<UIButton>();
            UIButton btnPlay = new UIButton(ImageId.BTN_PLAY,
                                    200, 50, (Context.Instance.ScreenWidth - 200)/2, Context.Instance.ScreenHeight/2);
            btnPlay.OnTouchUpHandler += OnPlayBtnUp;
            btnPlay.OnTouchCanceledHandler += OnPlayBtnUp;
            buttons.Add(btnPlay);
        }

        private void OnPlayBtnUp(object sender, Touch touch)
        {
            SetState(State.Playing);
        }

        public void SetState(State newState)
        {
            if (state != newState)
            {
                switch (state)
                {
                    case State.Menu:
                        foreach (UIButton button in buttons) button.Unregister(drawer);
                        break;
                    case State.Playing:
                        gamePlay.HideUI();
                        break;
                }

                state = newState;

                switch (state)
                {
                    case State.Menu:
                        foreach (UIButton button in buttons) button.RegisterTo(drawer);
                        break;
                    case State.Playing:
                        gamePlay.Init();
                        break;
                }

                Show();
            }
        }

        public override void Hide()
        {
            gamePlay.HideUI();
            foreach (UIButton button in buttons) button.Unregister(drawer);
        }

        public override void Show()
        {
            System.Diagnostics.Debug.WriteLine("State:" + state);
            switch (state)
            {
                case State.Menu:
                    foreach (UIButton button in buttons) button.RegisterTo(drawer);
                    break;
                case State.Playing:
                    gamePlay.ShowUI();
                    break;
            }
        }

        public override void Init()
        {
        }
        
        public override void Update(float elapsedSeconds)
        {
            switch (state)
            {
                case State.Menu:
                    break;
                case State.Playing:
                    gamePlay.Update(elapsedSeconds);
                    break;
            }
        }

        public override void UpdateTouch(Touch[] touches, int numTouches)
        {
            switch (state)
            {
                case State.Menu:
                    foreach (UIButton button in buttons) button.UpdateTouch(touches, numTouches);
                    break;
                case State.Playing:
                    gamePlay.UpdateTouch(touches, numTouches);
                    break;
            }
        }
    }
}
