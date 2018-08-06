using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SnakeConsole
{
    //四个方向
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    //代表游戏中地图元素的点
    class Point
    {
        int x, y;

        public bool isEmpty = true;
        public bool isFood = false;
        public bool isWall = false;
        public bool isSnakeBody = false;
        public bool isSnakeHead = false;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point()
        {

        }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public static bool Compare(Point p1, Point p2)
        {
            return (p1.X == p2.X && p1.Y == p2.Y) ? true : false;
        }
    }

    #region class Game 游戏类，初始化游戏内容以及控制游戏进程
    class Game
    {
        int INITIAL_INTERVAL_TIME = 500; //初始化间隔时间（ms）
        int INITIAL_DUE_TIME = 1000;
        enum GameState
        {
            DEAUFLT,
            OVER
        }
        GameState state = GameState.DEAUFLT;
        int scoreCount = 0;

        Map gameMap;
        Timer gameTimer;
        Snake snake;

        public void InitGame()
        {
            gameMap = new Map();
            snake = new Snake();
        }
        void Run(Object o)
        {
            gameMap.InitMap();
            gameMap.InitWall();
            SetSnake();
            gameMap.InitFood();
            Draw();
            GameCheck();
            snake.Move();
        }

        public void Start()
        {
            snake.InitSnake();
            gameTimer = new Timer(Run, null, INITIAL_DUE_TIME, INITIAL_INTERVAL_TIME);
            
            while(state!=GameState.OVER)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if(snake.CurDir != Direction.Down) snake.CurDir = Direction.Up;
                        break;
                    case ConsoleKey.RightArrow:
                        if (snake.CurDir != Direction.Left) snake.CurDir = Direction.Right;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (snake.CurDir != Direction.Right) snake.CurDir = Direction.Left;
                        break;
                    case ConsoleKey.DownArrow:
                        if (snake.CurDir != Direction.Up) snake.CurDir = Direction.Down;
                        break;
                }
            }
        }

        void Draw()
        {
            Console.Clear();
            gameMap.MapBuffer.Clear();
            gameMap.GenerateMapBuffer();
            Console.WriteLine("score:{0}", scoreCount);
            Console.Write(gameMap.MapBuffer.ToString());
        }
        void GameCheck()
        {
            for(int i = 0; i < gameMap.mapInfo.Count; i++)
            {
                if (Point.Compare(gameMap.mapInfo[i], snake.bodies[0]))
                {
                    if (gameMap.mapInfo[i].isWall) GameOver();
                    if (gameMap.mapInfo[i].isFood)
                    {
                        scoreCount++;
                        gameMap.mapInfo[i].isFood = false;
                        gameMap.mapInfo[i].isEmpty = true;
                        gameMap.foodPoint = null;
                        snake.eatFood();
                    }
                }
                if(snake.growth != null)
                {
                    snake.bodies.Add(snake.growth);
                    snake.growth = null;
                }
                
            }
            for(int i = 1; i < snake.bodies.Count; i++)
            {
                if (Point.Compare(snake.bodies[i], snake.bodies[0]))
                {
                    GameOver();
                }
            }
        }

        void GameOver()
        {
            Console.Clear();
            Console.WriteLine("Game Over");
            gameTimer.Change(Timeout.Infinite, 0);
            state = GameState.OVER;
        }
        void SetSnake()
        {
            for (int i = 0; i < gameMap.mapInfo.Count; i++)
            {
                for (int p = 0; p < snake.bodies.Count; p++)
                {
                    if (Point.Compare(gameMap.mapInfo[i], snake.bodies[p]))
                    {
                        gameMap.mapInfo[i].isEmpty = false;

                        if (p == 0)
                        {
                            gameMap.mapInfo[i].isSnakeHead = true;
                        }
                        else
                        {
                            gameMap.mapInfo[i].isSnakeBody = true;
                        }


                    }

                }
            }

        }

    }
    #endregion
    #region class Map 地图类，初始化更新和加载地图上所有元素
    class Map
    {
        Random random = new Random();

        string MAP_EMPTY = "  ";
        string MAP_SNAKE = "* ";
        string MAP_FOOD = "@ ";
        string MAP_WALL = "# ";

        public int width, height;
        int INITIAL_WIDTH = 30;
        int INITIAL_HEIGHT = 20;

        public Map()
        {

            width = INITIAL_WIDTH;
            height = INITIAL_HEIGHT;

            for (int y = 1; y <= height; y++)
            {
                for (int x = 1; x <= width; x++)
                {
                    mapInfo.Add(new Point(x, y));
                }
            }
        }

        public Point foodPoint = null;

        public void InitMap()
        {
            for (int i = 0;i < mapInfo.Count; i ++){
                mapInfo[i].isEmpty = true;
            }
            

        }

        public void InitFood()
        {
            ProvideFood();
            for (int i = 0; i < mapInfo.Count; i++)
            {
                if (Point.Compare(mapInfo[i], foodPoint))
                {
                    mapInfo[i].isEmpty = false;
                    mapInfo[i].isFood = true;
                }
            }
        }



        public void InitWall()
        {
            for (int i = 0; i < mapInfo.Count; i++)
            {
                if (mapInfo[i].X == 1 || mapInfo[i].Y == 1 || mapInfo[i].X == width || mapInfo[i].Y == height)
                {
                    mapInfo[i].isEmpty = false;
                    mapInfo[i].isWall = true;
                }
            }
        }

        public void GenerateMapBuffer()
        {
            if (mapBuffer.Length != 0 || mapInfo.Count > height*width) return;
            foreach (Point p in mapInfo)
            {
                if (p.isEmpty)
                {
                    MapBuffer.Append(MAP_EMPTY);
                }
                else if (p.isSnakeBody||p.isSnakeHead)
                {
                    MapBuffer.Append(MAP_SNAKE);
                }
                else if (p.isFood)
                {
                    MapBuffer.Append(MAP_FOOD);
                }
                else if (p.isWall)
                {
                    MapBuffer.Append(MAP_WALL);
                }

                if (p.X == width) MapBuffer.Append("\n");

            }
        }


        public void ProvideFood()
        {
            if (foodPoint == null)
            {
                randomAgain:
                int randVal = random.Next(0, mapInfo.Count);
                if (mapInfo[randVal].isEmpty)
                {
                    foodPoint = mapInfo[randVal];
                }
                else
                {
                    goto randomAgain;
                }
            }

        }




        public List<Point> mapInfo = new List<Point>();
        private StringBuilder mapBuffer = new StringBuilder();

        public StringBuilder MapBuffer { get => mapBuffer; set => mapBuffer = value; }
    }
#endregion
}

