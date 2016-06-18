using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodMaze.Scripts.Game.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using FoodMaze.Scripts.Game.Core.PathFinding;
using FoodMaze.Scripts.Game.UI;
using FoodMaze.Scripts.Game.Objects.Characters;
using FoodMaze.Scripts.Game.Objects.Touches;
using FoodMaze.Scripts.Game.Objects.Coordinates;

namespace FoodMaze.Scripts.Game.Objects
{
    class GamePlay
    {
        private const int H_OFFSET = 100;
        private const int V_OFFSET = 100;
        private static GamePlay instance;

        public static GamePlay Instance {
            get {
                if (instance == null)
                {
                    instance = new GamePlay();
                }
                return instance;
            }


            set { }
        }

        private Maze maze;
        private IFinder pathFinder;
        private IWorld world;
        private IDrawer drawer;
        private GameState state;
        private GameSubstate substate;
        private Player player;
        private UIPlayer uiPlayer;
        private AIPlayer aiPlayer;
        private UIAIPlayer uiAIPlayer;
        private Food food;
        private UIFood uiFood;
        private List<UIWall> uiWalls;
        private List<UIButton> buttons;
        private UIButton btnPause;
        private UIButton btnReplay;
        private UIButton btnContinue;
        private UIText resultText;
        private float totalElapsedSeconds;
        private bool isPlayerWin;
        private double angle;
        private double targetAngle;
        private Position centerPoint;
        private MoveDirection currentMazeDirection;

        private GamePlay()
        {
            uiWalls = new List<UIWall>();
            buttons = new List<UIButton>();
        }

        public void Init(bool isFirstGame = true)
        {
            SetState(GameState.Ready);
            System.Diagnostics.Debug.WriteLine("Start");
            totalElapsedSeconds = 0;
            drawer = Context.Instance.Drawer;

            IMazeGenerator generator = new RecursiveGenerator();
            maze = generator.Generate(Context.Instance.TileWidth, Context.Instance.TileHeight);
            
            world = new World();
            world.Init(maze, Context.Instance.ScreenWidth - H_OFFSET, Context.Instance.ScreenHeight - V_OFFSET, 10, H_OFFSET / 2);
            centerPoint = world.FindCenterPoint();

            pathFinder = new AStarFinder();
            pathFinder.Init(maze.Tiles, Context.Instance.TileWidth);

            InitWalls();
            InitFood();
            InitCharacters();

            InitDirectionButtons();
            InitFunctionalButtons();
            InitText();   
        }

        private void RotateAll()
        {
            Position playerTilePosition = world.FindTilePosition(player.CurrentTile);
            Position playerOffset = new Position(player.Position.X - playerTilePosition.X, player.Position.Y - playerTilePosition.Y);
            Position aiTilePosition = world.FindTilePosition(aiPlayer.CurrentTile);
            Position aiPlayerOffset = new Position(aiPlayer.Position.X - aiTilePosition.X, aiPlayer.Position.Y - aiTilePosition.Y);

            maze = maze.Rotate();
            world.Init(maze, Context.Instance.ScreenWidth - H_OFFSET, Context.Instance.ScreenHeight - V_OFFSET, 10, H_OFFSET / 2);

            DisposeWalls();
            InitWalls();
            RotateFood();
            RotateCharacters(playerOffset, aiPlayerOffset);
            ShowUI();
        }

        private void RotateFood()
        {
            food.InitPositionWithTile(world.GetTileAt(Context.Instance.TileHeight - 1 - food.CurrentTile.Y, food.CurrentTile.X));
            food.Notify();
        }

        private void RotateCharacters(Position playerOffset, Position aiPlayerOffset)
        {
            player.RotateDirection(uiPlayer.Width, playerOffset);
            aiPlayer.RotateDirection(uiAIPlayer.Width, aiPlayerOffset);
        }

        private void InitFood()
        {
            Random rand = new Random();
            food = new Food(world);
            uiFood = new UIFood(food, world);
            food.Width = uiFood.Width / 2;
            food.InitPositionWithTile(world.GetTileAt(rand.Next(0, Context.Instance.TileWidth), rand.Next(0, Context.Instance.TileHeight)));
            food.Notify();
        }

        private void InitCharacters()
        {
            Tile foodTile = food.CurrentTile;
            Random rand = new Random();
            int tileX = rand.Next(0, Context.Instance.TileWidth);
            int tileY = rand.Next(0, Context.Instance.TileHeight);
            while (Math.Abs(tileX - foodTile.X) < (Context.Instance.TileWidth - 1) / 2 || Math.Abs(tileY - foodTile.Y) < (Context.Instance.TileHeight - 1) / 2)
            {
                tileX = rand.Next(0, Context.Instance.TileWidth);
                tileY = rand.Next(0, Context.Instance.TileHeight);
            }
            player = new Player(world);
            uiPlayer = new UIPlayer(player, world);
            player.Width = uiPlayer.Width;
            player.InitPositionWithTile(world.GetTileAt(tileX, tileY));
            player.Notify();
            player.Direction = MoveDirection.NIL;
            player.ShouldMove = true;

            tileX = rand.Next(0, Context.Instance.TileWidth);
            tileY = rand.Next(0, Context.Instance.TileHeight);
            while ((Math.Abs(tileX - foodTile.X) < (Context.Instance.TileWidth - 1) / 2 || Math.Abs(tileY - foodTile.Y) < (Context.Instance.TileHeight - 1) / 2) || (tileX == player.CurrentTile.X && tileY == player.CurrentTile.Y))
            {
                tileX = rand.Next(0, Context.Instance.TileWidth);
                tileY = rand.Next(0, Context.Instance.TileHeight);
            }

            aiPlayer = new AIPlayer(world);
            uiAIPlayer = new UIAIPlayer(aiPlayer, world);
            aiPlayer.Width = uiAIPlayer.Width;
            aiPlayer.InitPositionWithTile(world.GetTileAt(tileX, tileY));
            aiPlayer.Notify();
            aiPlayer.Direction = MoveDirection.NIL;
            aiPlayer.Path = pathFinder.Find(food.CurrentTile, aiPlayer.CurrentTile);
            aiPlayer.Go();

            player.Speed = 2 * aiPlayer.Speed;
        }

        private void InitFunctionalButtons()
        {
            btnPause = new UIButton(ImageId.BTN_PAUSE, 50, 50, 0, 20);
            btnPause.OnTouchUpHandler += OnBtnPauseDown;

            btnReplay = new UIButton(ImageId.BTN_REPLAY, 200, 60, (Context.Instance.ScreenWidth - 200) / 2, (Context.Instance.ScreenHeight - 100) / 2 + 60);
            btnReplay.OnTouchUpHandler += OnBtnPlayAgainDown;

            btnContinue = new UIButton(ImageId.BTN_CONTINUE, 200, 60, (Context.Instance.ScreenWidth - 200) / 2, (Context.Instance.ScreenHeight - 100) / 2);
            btnContinue.OnTouchUpHandler += OnBtnContinueDown;
        }

        private void InitDirectionButtons()
        {
            int buttonWidth = Math.Min(world.TileWidth() * 5 / 4, V_OFFSET / 2);
            int leftX = (Context.Instance.ScreenWidth - 3 * buttonWidth) / 2;
            int leftY = world.FindTilePosition(new Tile(0, Context.Instance.TileHeight)).Y + world.WallWidth() + 20;
            buttons.Clear();
            UIButton btnLeft = new UIButton(ImageId.BTN_LEFT, buttonWidth, buttonWidth, leftX, leftY);
            btnLeft.OnTouchDownHandler += OnLeftBtnDown;
            btnLeft.OnTouchUpHandler += OnMoveBtnUp;
            btnLeft.OnTouchCanceledHandler += OnMoveBtnUp;

            UIButton btnRight = new UIButton(ImageId.BTN_RIGHT, buttonWidth, buttonWidth, leftX + 2 * buttonWidth, leftY);
            btnRight.OnTouchDownHandler += OnRightBtnDown;
            btnRight.OnTouchUpHandler += OnMoveBtnUp;
            btnRight.OnTouchCanceledHandler += OnMoveBtnUp;

            UIButton btnUp = new UIButton(ImageId.BTN_UP, buttonWidth, buttonWidth, leftX + buttonWidth, leftY);
            btnUp.OnTouchDownHandler += OnUpBtnDown;
            btnUp.OnTouchUpHandler += OnMoveBtnUp;
            btnUp.OnTouchCanceledHandler += OnMoveBtnUp;

            UIButton btnDown = new UIButton(ImageId.BTN_DOWN, buttonWidth, buttonWidth, leftX + buttonWidth, leftY + buttonWidth);
            btnDown.OnTouchDownHandler += OnDownBtnDown;
            btnDown.OnTouchUpHandler += OnMoveBtnUp;
            btnDown.OnTouchCanceledHandler += OnMoveBtnUp;

            buttons.Add(btnLeft);
            buttons.Add(btnRight);
            buttons.Add(btnUp);
            buttons.Add(btnDown);

            currentMazeDirection = MoveDirection.UP;
        }

        private void InitText()
        {
            resultText = new UIText("", (Context.Instance.ScreenWidth - 100) / 2, (Context.Instance.ScreenHeight - 200) / 2 + 60);
        }

        public void ShowUI()
        {
            foreach (UIButton button in buttons) button.RegisterTo(drawer);
            foreach (UIWall uiWall in uiWalls) uiWall.RegisterTo(drawer);
            if (food != null) uiFood.RegisterTo(drawer);
            if (uiPlayer != null) uiPlayer.RegisterTo(drawer);
            if (uiAIPlayer != null) uiAIPlayer.RegisterTo(drawer);
            if (btnPause != null) btnPause.RegisterTo(drawer);
        }

        public void HideUI()
        {
            foreach (UIButton button in buttons) button.Unregister(drawer);
            foreach (UIWall uiWall in uiWalls) uiWall.Unregister(drawer);
            if (uiPlayer != null) uiPlayer.Unregister(drawer);
            if (uiAIPlayer != null) uiAIPlayer.Unregister(drawer);
            if (food != null) uiFood.Unregister(drawer);
            if (btnPause != null) btnPause.Unregister(drawer);
            if (btnReplay != null) btnReplay.Unregister(drawer);
            if (btnContinue != null) btnContinue.Unregister(drawer);
        }

        public void Update(float elapsedSeconds)
        {
            switch (state)
            {
                case GameState.Ready:
                    totalElapsedSeconds += elapsedSeconds;
                    if (totalElapsedSeconds > 2)
                    {
                        SetState(GameState.Playing);
                    }
                    break;
                case GameState.Playing:
                    switch (substate)
                    {
                        case GameSubstate.Normal:
                            totalElapsedSeconds += elapsedSeconds;
                            if (player != null) player.Update(elapsedSeconds);
                            if (aiPlayer != null) aiPlayer.Update(elapsedSeconds);
                            if (food != null) food.Update(elapsedSeconds);

                            if (player.IsCollideWith(food))
                            {
                                isPlayerWin = true;
                                SetState(GameState.Finished);
                            }
                            else if (aiPlayer != null && aiPlayer.IsCollideWith(food))
                            {
                                isPlayerWin = false;
                                SetState(GameState.Finished);
                            }

                            if (totalElapsedSeconds > 15.0f)
                            {
                                Random rand = new Random();
                                if (rand.Next(0, 100) % 7 == 0)
                                {
                                    Rotate();
                                    totalElapsedSeconds = 0;
                                }
                            }
                            break;
                        case GameSubstate.Rotating:
                            angle += 5;
                            if (angle >= targetAngle)
                            {
                                angle = targetAngle;
                                substate = GameSubstate.Normal;
                            }
                            double rad = angle * Math.PI / 180;
                            foreach (UIWall wall in uiWalls) wall.Rotate(centerPoint, rad, targetAngle, angle >= targetAngle);
                            uiPlayer.Rotate(centerPoint, rad, targetAngle, angle >= targetAngle);
                            uiAIPlayer.Rotate(centerPoint, rad, targetAngle, angle >= targetAngle);
                            uiFood.Rotate(centerPoint, rad, targetAngle, angle >= targetAngle);
                            if (substate == GameSubstate.Normal) RotateAll();
                            break;
                    }
                    break;
                case GameState.Paused:
                    break;
                case GameState.Finished:
                    totalElapsedSeconds += elapsedSeconds;
                    break;
            }   
        }

        void Rotate()
        {
            angle = 0;
            targetAngle = targetAngle % 360;
            targetAngle = angle + (new Random()).Next(1, 1) * 90;
            substate = GameSubstate.Rotating;
            uiPlayer.StartRotate();
            uiAIPlayer.StartRotate();
            uiFood.StartRotate();
            currentMazeDirection = (MoveDirection)(((int)currentMazeDirection + 1) % (int)MoveDirection.NIL);
        }

        public void SetState(GameState newState)
        {
            if (state != newState)
            {
                totalElapsedSeconds = 0;
                switch (state)
                {
                    case GameState.Paused:
                        btnContinue.Unregister(drawer);
                        btnReplay.Unregister(drawer);
                        break;
                    case GameState.Finished:
                        btnReplay.Unregister(drawer);
                        resultText.Unregister(drawer);
                        break;
                }
                state = newState;
                switch (state)
                {
                    case GameState.Paused:
                        btnContinue.RegisterTo(drawer);
                        btnReplay.RegisterTo(drawer);
                        break;
                    case GameState.Finished:
                        btnReplay.RegisterTo(drawer);
                        resultText.SetText(isPlayerWin ? "You Win!" : "AI Wins");
                        resultText.RegisterTo(drawer);
                        break;
                }
            }
        }

        public void UpdateTouch(Touch[] touches, int numTouches)
        {
            switch (state)
            {
                case GameState.Playing:
                    foreach (UIButton button in buttons)
                        button.UpdateTouch(touches, numTouches);
                    break;
                case GameState.Finished:
                    btnReplay.UpdateTouch(touches, numTouches);
                    break;
                case GameState.Paused:
                    btnContinue.UpdateTouch(touches, numTouches);
                    btnReplay.UpdateTouch(touches, numTouches);
                    break;
            }
            
            btnPause.UpdateTouch(touches, numTouches);
            
        }

        private void InitWalls()
        {
            uiWalls.Clear();
            foreach (Wall wall in maze.HWalls)
            {
                UIWall uiWall = UIWallPool.Instance.Pop();
                uiWall.Init(wall, world);
                uiWalls.Add(uiWall);
            }

            foreach (Wall wall in maze.VWalls)
            {
                UIWall uiWall = UIWallPool.Instance.Pop();
                uiWall.Init(wall, world);
                uiWalls.Add(uiWall);
            }
        }

        private void OnBtnPauseDown(object sender, Touch e) {
            SetState(GameState.Paused);
        }

        private void OnBtnContinueDown(object sender, Touch e)
        {
            SetState(GameState.Playing);
        }

        private void OnBtnPlayAgainDown(object sender, Touch e)
        {
            HideUI();
            Dispose();
            Init(false);
            ShowUI();
        }

        private void Dispose()
        {
            foreach (UIButton button in buttons) button.Dispose();
            buttons.Clear();
            DisposeWalls();
            if (uiPlayer != null) uiPlayer.Dispose();
            if (uiAIPlayer != null) uiAIPlayer.Dispose();
            if (food != null) uiFood.Dispose();
            if (btnPause != null) btnPause.Dispose();
            if (btnReplay != null) btnReplay.Dispose();
            if (btnContinue != null) btnContinue.Dispose();
        }

        private void DisposeWalls()
        {
            foreach (UIWall uiWall in uiWalls)
            {
                uiWall.Unregister(drawer);
                uiWall.Dispose();
                UIWallPool.Instance.Push(uiWall);
            }
            uiWalls.Clear();
        }

        private void OnLeftBtnDown(object sender, Touch touch)
        {
            if (player != null)
                player.Direction = (MoveDirection)(((int)MoveDirection.LEFT + (int)currentMazeDirection) % (int)MoveDirection.NIL);
        }

        private void OnMoveBtnUp(object sender, Touch touch)
        {
            if (player != null) player.Direction = MoveDirection.NIL;
        }

        private void OnRightBtnDown(object sender, Touch touch)
        {
            if (player != null) player.Direction = (MoveDirection)(((int)MoveDirection.RIGHT + (int)currentMazeDirection) % (int)MoveDirection.NIL); ;
        }

        private void OnDownBtnDown(object sender, Touch touch)
        {
            if (player != null) player.Direction = (MoveDirection)(((int)MoveDirection.DOWN + (int)currentMazeDirection) % (int)MoveDirection.NIL); ;
        }

        private void OnUpBtnDown(object sender, Touch touch)
        {
            if (player != null) player.Direction = (MoveDirection)(((int)MoveDirection.UP + (int)currentMazeDirection) % (int)MoveDirection.NIL); ;
        }
    }
}
