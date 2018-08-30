using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Point
    {
        int x;
        int y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
    }


    class Display
    {

        char ArrowLeft = '←';
        char ArrowRight = '→';
        public void Init()
        {
            fieldHeight = INIT_HEIGHT;
            fieldWidth = INIT_WIDTH;
            Console.WindowHeight = INIT_HEIGHT + 24;
            Console.WindowWidth = INIT_WIDTH + 2;
            Console.BufferHeight = INIT_HEIGHT + 24;
            Console.BufferWidth = INIT_WIDTH + 2;
            Console.Title = "CARD FIGHT";
        }
        public int fieldHeight;
        public int fieldWidth;
        const int INIT_HEIGHT = 40;
        const int INIT_WIDTH = 80;






        public void DrawGame()
        {

            Console.CursorVisible = false;
            DrawField();
            DrawText(new Point(fieldWidth / 2 - (Game.GetInstance().gameState.Length / 2), 1),
                $"BATTLE {Game.GetInstance().battle.battle}",
                ConsoleColor.Black, ConsoleColor.White);
            DrawGround();
            DrawCharacter();
            DrawLogField();
            DrawPlayerInfo();
            DrawHandCards();
            if (Game.GetInstance().gameState == "GameOver")
            {
                DrawGameState("  Game Over  ");
                Console.ReadKey(true);
                //Environment.Exit(0);
                return;
            }
            if (Game.GetInstance().gameState == "Win")
            {
                DrawGameState("  最终胜利  ");
                Console.ReadKey(true);
                //Environment.Exit(0);
                return;
            }

        }



        void DrawPlayerInfo()
        {
            if (Game.GetInstance().GetPlayer() != null)
            {
                Point pos = new Point(1, fieldHeight + 17);
                DrawField(pos, 52, 6, ConsoleColor.Blue);
                pos.Y -= 5;
                Point pos1 = new Point(3, fieldHeight + 19);
                Point pos2 = new Point(3, fieldHeight + 20);
                DrawText(pos1, "HP:", ConsoleColor.Red, ConsoleColor.Black);
                pos1.X += 3;
                for (int i = 0; i < Game.GetInstance().GetPlayer().maxHp; i++)
                {
                    if (i < Game.GetInstance().GetPlayer().curHp)
                    {
                        DrawAConsoleColor(pos1, " ", ConsoleColor.DarkRed);
                        pos1.X++;
                    }
                    else
                    {
                        DrawAConsoleColor(pos1, " ", ConsoleColor.Red);
                        pos1.X++;
                    }
                }
                DrawText(pos2, "AP:", ConsoleColor.Yellow, ConsoleColor.Black);
                pos2.X += 3;
                for (int i = 0; i < Game.GetInstance().GetPlayer().maxAp; i++)
                {
                    if (i < Game.GetInstance().GetPlayer().curAp)
                    {
                        DrawAConsoleColor(pos2, " ", ConsoleColor.DarkYellow);
                        pos2.X++;
                    }
                    else
                    {
                        DrawAConsoleColor(pos2, " ", ConsoleColor.Yellow);
                        pos2.X++;
                    }
                }

                DrawText(new Point(35, fieldHeight + 21), $"[库：{Game.GetInstance().GetPlayer().CountDeck()} 弃：{Game.GetInstance().GetPlayer().CountDiscard()}]", ConsoleColor.White, ConsoleColor.Black);
                Point pos3 = new Point(3, fieldHeight + 21);
                foreach (var i in Game.GetInstance().GetPlayer().modifier)
                {

                    if (i.Value != 0)
                    {
                        DrawText(pos3,
                            $"{i.Key}:+{i.Value}", ConsoleColor.Red, ConsoleColor.Gray);

                    }
                    else
                    {
                        DrawText(pos3, "      ", ConsoleColor.Black, ConsoleColor.Black);

                    }
                    pos3.X += 6;
                }

            }
            else
            {
                Point pos = new Point(1, fieldHeight + 17);
                DrawField(pos, 52, 6, ConsoleColor.Blue);
                Point pos1 = new Point(3, fieldHeight + 18);
                DrawText(pos1, "YOU OUT", ConsoleColor.Red, ConsoleColor.Black);
            }
        }

        public void DrawGameState(string str)
        {

            DrawText(new Point(fieldWidth / 2 - (str.Length / 2), fieldHeight / 2 - 10), str, ConsoleColor.Black, ConsoleColor.Gray);

        }

        void DrawHandCards()
        {
            if (Game.GetInstance().GetPlayer() != null)
            {
                Character player = Game.GetInstance().GetPlayer();
                int cur = Game.GetInstance().curPlayerAction;
                Point pos = new Point(1, fieldHeight + 2);

                for (int i = 0; i < 4; i++)
                {
                    var c = player.GetHandCards().Count;
                    if (i < c)
                    {
                        if (i == cur)
                        {
                            DrawActionCard(pos, player.handCards[i], ConsoleColor.Cyan);
                        }
                        else
                        {
                            DrawActionCard(pos, player.handCards[i], ConsoleColor.White);
                        }
                    }
                    else
                    {
                        ClearRect(new Point(pos.X, pos.Y), 12, 14, ConsoleColor.Black);
                    }
                    pos.X += 13;
                }
            }
        }

        public string DrawHint(params string[] texts)
        {
            ClearRect(new Point(1, fieldHeight + 2), 52, 14, ConsoleColor.Black);
            Point pos = new Point(1, fieldHeight + 2);
            DrawField(pos, 52, 14, ConsoleColor.White);
            Point pos1 = new Point(2, fieldHeight + 3);
            foreach (var t in texts)
            {
                DrawText(pos1, t, ConsoleColor.Yellow, ConsoleColor.Black);
                pos1.Y++;
            }
            var k = Console.ReadKey(true).KeyChar.ToString();
            ClearRect(new Point(1, fieldHeight + 2), 52, 14, ConsoleColor.Black);
            return k;
        }

        public void DrawHintPlayer(params string[] texts)
        {
            ClearRect(new Point(1, fieldHeight + 2), 52, 14, ConsoleColor.Black);
            Point pos = new Point(1, fieldHeight + 2);
            DrawField(pos, 52, 14, ConsoleColor.White);
            Point pos1 = new Point(2, fieldHeight + 3);
            foreach (var t in texts)
            {
                DrawText(pos1, t, ConsoleColor.Yellow, ConsoleColor.Black);
                pos1.Y++;
            }
        }


        public string DrawHintDeck(string titile, string[] texts)
        {
            ClearRect(new Point(1, fieldHeight + 2), 52, 14, ConsoleColor.Black);
            Point pos = new Point(1, fieldHeight + 2);
            DrawField(pos, 52, 14, ConsoleColor.White);
            Point pos1 = new Point(2, fieldHeight + 3);
            foreach (var t in texts)
            {
                DrawText(pos1, t, ConsoleColor.Yellow, ConsoleColor.Black);
                if (pos.Y >= fieldHeight + 17)
                {
                    pos.Y -= 14;
                    pos.X += 8;
                }
                pos1.Y++;
            }
            DrawText(new Point(1, fieldHeight + 2), titile, ConsoleColor.White, ConsoleColor.Black);
            var k = Console.ReadKey(true).KeyChar.ToString();
            ClearRect(new Point(1, fieldHeight + 2), 52, 14, ConsoleColor.Black);
            return k;
        }

        void DrawLogField()
        {
            Point pos1 = new Point(fieldWidth - 26, fieldHeight + 2);
            Point pos2 = new Point(fieldWidth - 25, fieldHeight + 3);
            DrawField(pos1, 27, 21, ConsoleColor.Yellow);
            DrawGameLog(pos2, 19);
        }







        /// <summary>
        /// 初始化主窗口
        /// </summary>
        void DrawField()
        {

            for (int x = 1; x <= fieldWidth; x++)
            {
                for (int y = 1; y <= fieldHeight; y++)
                {
                    Point pos = new Point(x, y);
                    if (pos.Y == 1 || pos.X == 1 || pos.X == fieldWidth || pos.Y == fieldHeight)
                    {
                        DrawAConsoleColor(pos, " ", ConsoleColor.Gray);
                    }
                }
            }
        }

        #region DrawGround() DrawCharacter() 在主界面中间画出场地和角色
        void DrawGround()
        {
            Point pos = new Point(18, fieldHeight / 2 + 4);

            for (int i = 0; i <= 10; i++)
            {
                DrawAConsoleColor(pos, "    ", ConsoleColor.Green);
                pos.X += 4;
            }

        }
        void DrawCharacter()
        {
            foreach (var chara in Game.GetInstance().charaAxis)
            {

                int inAxis = chara.Key;
                Character character = chara.Value;
                Point pos = new Point(18 + (inAxis * 4), fieldHeight / 2);


                if (chara.Value == null)
                {
                    ClearRect(new Point(pos.X, pos.Y), 4, 4, ConsoleColor.Black);
                    Point pos1 = new Point(18 + (inAxis * 4), fieldHeight / 2 + 5);
                    for (int i = 0; i < 15; i++)
                    {

                        DrawAConsoleColor(pos1, "    ", ConsoleColor.Black);
                        pos1.Y++;
                    }
                    continue;
                }
                else
                {
                    ClearRect(new Point(pos.X, pos.Y), 4, 4, ConsoleColor.Black);
                }


                for (int i = 0; i < character.img.GetLength(0); i++)
                {
                    for (int j = 0; j < character.img.GetLength(1); j++)
                    {
                        Console.SetCursorPosition(pos.X + j, pos.Y + i);
                        Console.Write(character.img[i, j]);
                    }
                }
                if (character.type != CharacterType.Block)
                {
                    DrawAConsoleColor(new Point(18 + (inAxis * 4), fieldHeight / 2 + 4),
                        (character.faceTo == Direction.Left ?
                        ArrowLeft.ToString() + "  "
                        :
                        "  " + ArrowRight.ToString()),
                        (character.type == CharacterType.Monster ? ConsoleColor.Red : ConsoleColor.Blue));
                }
                if (chara.Value.type == CharacterType.Monster)
                {
                    Point pos1 = new Point(18 + (inAxis * 4), fieldHeight / 2 + 5);
                    DrawText(pos1, " HP ", ConsoleColor.Black, ConsoleColor.Gray);
                    pos1.Y++;
                    for (int i = 0; i < chara.Value.maxHp; i++)
                    {
                        if (i < chara.Value.curHp)
                        {
                            DrawAConsoleColor(pos1, "    ", ConsoleColor.DarkRed);
                            pos1.Y++;
                        }
                        else
                        {
                            DrawAConsoleColor(pos1, "    ", ConsoleColor.Black);
                            pos1.Y++;
                        }
                    }
                }
                else
                {
                    Point pos1 = new Point(18 + (inAxis * 4), fieldHeight / 2 + 5);
                    for (int i = 0; i < chara.Value.maxHp + 2; i++)
                    {

                        DrawAConsoleColor(pos1, "    ", ConsoleColor.Black);
                        pos1.Y++;
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// 在指定位置画一张行动卡
        /// </summary>
        /// <param name="point">位置</param>
        /// <param name="action">卡</param>
        /// <param name="color">边框颜色</param>
        public void DrawActionCard(Point point, Action action, ConsoleColor color)
        {
            Point temp_p = new Point(point.X, point.Y);

            ClearRect(new Point(temp_p.X, temp_p.Y), 12, 14, ConsoleColor.Black);
            DrawField(new Point(temp_p.X, temp_p.Y), 12, 14, color);
            temp_p.X += 2;
            temp_p.Y += 2;
            DrawText(new Point(temp_p.X, temp_p.Y), action.name, ConsoleColor.Black, ConsoleColor.Gray);
            temp_p.Y += 3;
            string[] strs = action.describe.Split('\n');
            var p = new Point(temp_p.X, temp_p.Y);
            foreach (var s in strs)
            {
                DrawText(p, s, ConsoleColor.White, ConsoleColor.Black);
                p.Y += 1;
            }

        }
        /// <summary>
        /// 打印游戏信息
        /// </summary>
        /// <param name="point">位置</param>
        /// <param name="maxHeight">最高显示高度</param>
        void DrawGameLog(Point point, int maxHeight)
        {
            var log = Game.GetInstance().gameLog;
            for (int i = (log.Count - maxHeight) < 0 ? 0 : (log.Count - maxHeight); i < log.Count; i++)
            {
                DrawText(point, log[i], ConsoleColor.Cyan, ConsoleColor.Black);
                point.Y++;
            }
        }
        /// <summary>
        /// 画一格控制台颜色里有的颜色到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="str">内容</param>
        /// <param name="color">颜色</param>
        void DrawAConsoleColor(Point pos, string str, ConsoleColor color)
        {
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.BackgroundColor = color;
            Console.Write(str);
            Console.BackgroundColor = ConsoleColor.Black;
        }
        /// <summary>
        /// 画一句话到控制台指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="text">内容</param>
        /// <param name="foreColor">文字颜色</param>
        /// <param name="backColor">背景颜色</param>
        void DrawText(Point pos, String text, ConsoleColor foreColor, ConsoleColor backColor)
        {
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = foreColor;
            foreach (var ch in text)
            {
                Console.Write(ch);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// 以指定颜色清空一段区域
        /// </summary>
        /// <param name="point">位置</param>
        /// <param name="w">区域的宽</param>
        /// <param name="h">区域的高</param>
        /// <param name="color">颜色</param>
        public void ClearRect(Point point, int w, int h, ConsoleColor color)
        {

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    DrawAConsoleColor(point, " ", color);
                    point.X++;
                }
                point.Y++;
                point.X -= w;
            }
        }
        /// <summary>
        /// 以指定颜色画一个窗口
        /// </summary>
        /// <param name="point">位置</param>
        /// <param name="w">窗口的宽</param>
        /// <param name="h">窗口的高</param>
        /// <param name="color">颜色</param>
        void DrawField(Point point, int w, int h, ConsoleColor color)
        {

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (y == 0 || x == 0 || x == w - 1 || y == h - 1)
                    {
                        DrawAConsoleColor(point, " ", color);
                    }
                    point.X++;
                }
                point.Y++;
                point.X -= w;
            }
        }

    }
}
