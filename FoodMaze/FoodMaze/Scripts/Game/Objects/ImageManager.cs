using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FoodMaze.Scripts.Game.Objects
{
    public enum ImageId
    {
        WALL,
        BTN_REPLAY,
        BTN_PLAY,
        BTN_CONTINUE,
        BTN_PAUSE,
        BTN_LEFT,
        BTN_RIGHT,
        BTN_DOWN,
        BTN_UP,
        PLAYER,
        AI,
        FOOD
    }

    public static class ImageManager
    {
        public static ImageSource GetImageSource(ImageId imageId)
        {
            switch (imageId)
            {
                case ImageId.PLAYER:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/GameScreen/char01_walk_01.png"));
                case ImageId.AI:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/GameScreen/char02_walk_01.png"));
                case ImageId.WALL:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/GameScreen/wall_bg02.png"));
                case ImageId.BTN_UP:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/GameScreen/btn_up_normal.png"));
                case ImageId.BTN_RIGHT:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/GameScreen/btn_right_normal.png"));
                case ImageId.BTN_DOWN:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/GameScreen/btn_down_normal.png"));
                case ImageId.BTN_LEFT:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/GameScreen/btn_left_normal.png"));
                case ImageId.BTN_PAUSE:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/UI/btn_pause_normal.png"));
                case ImageId.BTN_PLAY:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/UI/btn_single_normal.png"));
                case ImageId.BTN_REPLAY:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/UI/btn_playAgain_normal.png"));
                case ImageId.BTN_CONTINUE:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/UI/btn_resume_normal.png"));
                case ImageId.FOOD:
                    return new BitmapImage(new Uri("ms-appx:///Assets/Images/GameScreen/targetObject_01.png"));
            }

            throw new NotSupportedException();
        }
    }
}
