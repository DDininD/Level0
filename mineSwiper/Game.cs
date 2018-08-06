using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mineSwiper
{
    class Point
    {
        int x;
        int y;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    class Game
    {

        Point curPoint;
        int MineCount;
        int FlagCount;

        enum CoverType
        {
            invisible = 0,
            flag = 1,
            none = 3,
        }

        enum GameState
        {
            win = 1,
            over = -1,
            defualt = 0
        }


        int[,] map;
        CoverType[,] mapCover;

        public void GameInit()
        {
            //选择难度
            Console.WriteLine("欢迎来到扫雷，请选择难度：[1]简单 [2]中等 [3]困难");
            switch (GetCommand())
            {
                case "1": map = InitMap(1); break;
                case "2": map = InitMap(2); break;
                case "3": map = InitMap(3); break;
            }
            mapCover = new CoverType[map.GetLength(0), map.GetLength(1)];
            InitCursor();
        }

        int[,] InitMap(int difficulty)
        {
            Random rand = new Random();
            switch (difficulty)
            {
                case 1:
                    int[,] map_e = new int[7, 7];
                    for (int i = 0; i < 5; i++)
                    {
                        if (map_e[rand.Next(0, 7), rand.Next(0, 7)] != 9)
                        {
                            map_e[rand.Next(0, 7), rand.Next(0, 7)] = 9;
                        }
                        else
                        {
                            i--;
                            continue;
                        }

                    }
                    MineCount = 5;
                    FlagCount = 5;
                    SetNumber(map_e);
                    return map_e;
                case 2:
                    int[,] map_m = new int[9, 9];
                    for (int i = 0; i < 7; i++)
                    {
                        if (map_m[rand.Next(0, 9), rand.Next(0, 9)] != 9)
                        {
                            map_m[rand.Next(0, 9), rand.Next(0, 9)] = 9;
                        }
                        else
                        {
                            i--;
                            continue;
                        }

                    }
                    MineCount = 7;
                    FlagCount = 7;
                    SetNumber(map_m);
                    return map_m;
                case 3:
                    int[,] map_h = new int[11, 11];
                    for (int i = 0; i < 9; i++)
                    {
                        if (map_h[rand.Next(0, 11), rand.Next(0, 11)] != 9)
                        {
                            map_h[rand.Next(0, 11), rand.Next(0, 11)] = 9;
                        }
                        else
                        {
                            i--;
                            continue;
                        }
                    }
                    MineCount = 9;
                    FlagCount = 9;
                    SetNumber(map_h);
                    return map_h;
                default: break;
            }
            return null;
        }

        void SetNumber(int[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    int count = 0;
                    if (map[i, j] != 9)
                    {
                        for (int n1 = -1; n1 <= 1; n1++)
                        {
                            for (int n2 = -1; n2 <= 1; n2++)
                            {
                                if (i + n1 > (map.GetLength(0) - 1) || j + n2 > (map.GetLength(1) - 1) || i + n1 < 0 || j + n2 < 0) continue;
                                if (map[i + n1, j + n2] == 9)
                                {
                                    count++;
                                }
                            }
                        }
                        map[i, j] = count;
                    }
                }
            }
        }

        void InitCursor()
        {
            curPoint = new Point(1, 1);
        }


        void PrintMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (curPoint.X == i + 1 && curPoint.Y == j + 1)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    if (mapCover[i, j] == CoverType.none)
                    {
                        switch (map[i, j])
                        {
                            case 0: Console.Write("  "); break;
                            case 9: Console.Write("* "); break;
                            default: Console.Write($"{map[i, j]} "); break;
                        }
                    }
                    else
                    {
                        switch (mapCover[i, j])
                        {
                            case CoverType.invisible: Console.Write("? "); break;
                            case CoverType.flag: Console.Write("f "); break;
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        void FindWay(Point point)
        {
            if (mapCover[point.X - 1, point.Y - 1] != CoverType.flag)
            {
                mapCover[point.X - 1, point.Y - 1] = CoverType.none;
            }
            if (map[point.X - 1, point.Y - 1] == 9)
            {
                gameState = GameState.over;
            }
            else if (map[point.X - 1, point.Y - 1] == 0)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {

                        if (point.X - 1 + i >= 0 && point.X - 1 + i < map.GetLength(0) && point.Y - 1 + j >= 0 && point.Y - 1 + j < map.GetLength(1))
                        {
                            if (map[point.X - 1 + i, point.Y - 1 + j] == 0)
                            {
                                if (mapCover[point.X - 1 + i, point.Y - 1 + j] != CoverType.none)
                                {
                                    Point temp_p = new Point(point.X + i, point.Y + j);
                                    FindWay(temp_p);
                                }
                            }
                            else if (map[point.X - 1 + i, point.Y - 1 + j] > 0 && map[point.X - 1 + i, point.Y - 1 + j] < 9)
                            {
                                if (mapCover[point.X - 1 + i, point.Y - 1 + j] != CoverType.none)
                                {
                                    mapCover[point.X - 1 + i, point.Y - 1 + j] = CoverType.none;
                                }
                            }
                        }

                    }
                }
            }
        }
        void SetFlag()
        {
            if (FlagCount < 0) return;
            if (mapCover[curPoint.X - 1, curPoint.Y - 1] == CoverType.none)
            {
                return;
            }
            if (mapCover[curPoint.X - 1, curPoint.Y - 1] != CoverType.flag)
            {
                mapCover[curPoint.X - 1, curPoint.Y - 1] = CoverType.flag;
                if (map[curPoint.X - 1, curPoint.Y - 1] == 9)
                {
                    MineCount--;
                }
                FlagCount--;
            }
            else
            {
                mapCover[curPoint.X - 1, curPoint.Y - 1] = CoverType.invisible;
                if (map[curPoint.X - 1, curPoint.Y - 1] == 9)
                {
                    MineCount++;
                }
                FlagCount++;
            }

        }
        string GetCommand()
        {

            ConsoleKeyInfo keyinfo = Console.ReadKey();
            string commandoo = keyinfo.KeyChar.ToString();

            if (map == null) return commandoo;

            switch (keyinfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (curPoint.X > 1) curPoint.X--;
                    break;
                case ConsoleKey.LeftArrow:
                    if (curPoint.Y > 1) curPoint.Y--;
                    break;
                case ConsoleKey.DownArrow:
                    if (curPoint.X < map.GetLength(0)) curPoint.X++;
                    break;
                case ConsoleKey.RightArrow:
                    if (curPoint.Y < map.GetLength(1)) curPoint.Y++;
                    break;
                case ConsoleKey.F:
                    SetFlag();
                    break;
                case ConsoleKey.M:
                    FindWay(curPoint);
                    break;
                default:
                    break;
            }

            return commandoo;
        }
        GameState gameState;
        public void Run()
        {
            while (true)
            {
                Console.Clear();
                PrintMap();
                switch (gameState)
                {
                    case GameState.over:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine("你踩到雷了，游戏结束！");
                        return;
                    case GameState.win:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine("你赢了，游戏结束！");
                        return;
                    default:
                        if (MineCount == 0)
                        {
                            gameState = GameState.win;
                            for (int i = 0; i < mapCover.GetLength(0); i++)
                            {
                                for (int j = 0; j < mapCover.GetLength(1); j++)
                                {
                                    mapCover[i, j] = CoverType.none;
                                }
                            }
                            continue;
                        }
                        break;
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("输入指令：[↑][↓][←][→]控制光标移动 [f]插旗/取消 [m]扫雷");
                //Console.WriteLine("剩余雷数：{0}", MineCount);
                Console.WriteLine("剩余旗子数：{0}", FlagCount);
                GetCommand();


            }
        }
    }
}
