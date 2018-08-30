using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ConsoleGame
{
    class Game
    {
        //todo: 不同种类的角色
        //todo: 不同种类的怪物
        //todo: 随机的关卡
        //todo: 关底结算，奖励
        //todo: 增幅类卡牌
        //todo: 角色状态（*）
        //todo: 加入BOSS（*）



        public Battle battle;

        public Display gameDisplay;
        public int curPlayerAction = 0;
        public Dictionary<int, Character> charaAxis = new Dictionary<int, Character>();
        public List<string> gameLog = new List<string>();
        public Dictionary<string, Action> actionArchive = new Dictionary<string, Action>();
        public string gameState = "Normal";
        void Init()
        {
            Console.CursorVisible = false;


            for (int i = 0; i <= 10; i++)
            {
                charaAxis.Add(i, null);
            }
            //Game game = Game.GetInstance();

            gameDisplay = new Display();
            gameDisplay.Init();


            GameOpenning();
            
        }

        public void Run()
        {


            while (!"GameOver".Equals(gameState))
            {
                if ("Win".Equals(gameState)) break;
                battle.Init();
                CheckGame();
                gameDisplay.DrawGame();
                battle.RunBattle();

            }
            if ("GameOver".Equals(gameState))
            {
                gameLog.Add("你输了                  ");
                gameDisplay.DrawHint($"你一共进行了{battle.battle}场战斗", "你的冒险结束了......");
            }
            if ("Win".Equals(gameState))
            {
                gameLog.Add("你赢了                  ");
                gameDisplay.DrawHint($"费劲千辛万苦你终于打倒了Boss",
                    "你的冒险结束了......",
                    "可这个世界真的会被拯救吗？");
            }
        }

        Game()
        {
            Init();
        }

        void GameOpenning()
        {
            battle = new Battle();
            gameDisplay.DrawHint("欢迎来到CARD FIGHT",
                "你的战斗场地是一个长度为11的横轴",
                "你需要利用各种卡牌，闪转腾挪，战胜对手",
                "注意，战斗的胜利条件有两个",
                "把对手推下场，或者将对手hp归零",
                "建议使用默认窗口大小游玩",
                "推荐使用点阵字体8x12(中窗体)和12x16(大窗体)");
            gameDisplay.DrawHint("下面介绍卡牌的能力:",
                "耗: 代表使用需要消耗的AP",
                "伤: 代表卡牌会进行攻击，目标为角色的前方第一个目标",
                "移: 向前方移动一段距离，注意，移动的路上如有东西则",
                "会被立即挡住",
                "推: 将前方第一个目标前推一段距离",
                "转: 角色转向",
                "治: 治疗自己一定HP",
                "跳: 向前跳跃一段距离，跳跃的过程不会被阻挡",
                "抽: 抽一张牌",
                "+:  增幅你的一些能力",
                "其他: 也存在一些具有特殊能力的卡牌");
            gameDisplay.DrawHint("游戏操作方法:",
                "方向选择卡牌，回车使用，ESC结束回合",
                "按L键查看已有的牌",
                "按D键查看弃牌");
            gameDisplay.DrawGameState("←选择职业→");

            switch (ChoosePlayer()){
                case 1: battle.Init(1); ReadCardList(1); break;
                case 2: battle.Init(2); ReadCardList(2); break;
                case 3: battle.Init(3); ReadCardList(3); break;
            }


        }


        public int ChoosePlayer()
        {
            int choose = 1;
            again1:
            switch (choose)
            {
                case 1:
                    Point pos1 = new Point(18 + (5 * 4), gameDisplay.fieldHeight / 2);
                    var img1 = Character.fighterImg;
                    for (int i = 0; i < img1.GetLength(0); i++)
                    {
                        for (int j = 0; j < img1.GetLength(1); j++)
                        {
                            Console.SetCursorPosition(pos1.X + j, pos1.Y + i);
                            Console.Write(img1[i, j]);
                        }
                    }
                    gameDisplay.DrawHintPlayer("战士:","",
                "刚健朴实，穿盔甲，具有战士该有的一切技能",
                "初始HP:10  初始AP:5",
                "有各种攻击牌");
                    break;
                case 2:
                    Point pos2 = new Point(18 + (5 * 4), gameDisplay.fieldHeight / 2);
                    var img2 = Character.coachImg;
                    for (int i = 0; i < img2.GetLength(0); i++)
                    {
                        for (int j = 0; j < img2.GetLength(1); j++)
                        {
                            Console.SetCursorPosition(pos2.X + j, pos2.Y + i);
                            Console.Write(img2[i, j]);
                        }
                    }
                    gameDisplay.DrawHintPlayer("健身教练:", "",
                "体术一流，但是职业信条是不造成伤害",
                "初始HP:8   初始AP:6",
                "没有攻击牌，可以推人");
                    break;
                case 3:
                    Point pos3 = new Point(18 + (5 * 4), gameDisplay.fieldHeight / 2);
                    var img3 = Character.programmerImg;
                    for (int i = 0; i < img3.GetLength(0); i++)
                    {
                        for (int j = 0; j < img3.GetLength(1); j++)
                        {
                            Console.SetCursorPosition(pos3.X + j, pos3.Y + i);
                            Console.Write(img3[i, j]);
                        }
                    }
                    gameDisplay.DrawHintPlayer("黑客:", "",
                "很聪明，当了黑客后视力变差了",
                "初始HP:6   初始AP:8",
                "只有很弱的进攻牌但是可以通过编程来强化自己");
                    break;
            }
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.RightArrow:
                    choose += 1;
                    if (choose > 3) choose = 1;
                    break;
                case ConsoleKey.LeftArrow:
                    choose -= 1;
                    if (choose < 1) choose = 3;
                    break;
                case ConsoleKey.Enter:
                    gameDisplay.ClearRect(new Point(1, gameDisplay.fieldHeight + 2), 52, 14, ConsoleColor.Black);
                    return choose;
            }

            goto again1;
        }

        static Game instance;

        public static Game GetInstance()
        {
            if (instance == null)
            {
                instance = new Game();
                return instance;
            }
            else
            {
                return instance;
            }
        }


        public Character getItemInAxisAt(int index)
        {
            return charaAxis[index];
        }


        public Character GetPlayer()
        {
            foreach (var ch in charaAxis)
            {
                if (ch.Value != null)
                {
                    if (ch.Value.type == CharacterType.DefaultPlayer)
                    {
                        return ch.Value;
                    }
                }
            }
            return null;
        }
        public Character GetMonster()
        {
            foreach (var ch in charaAxis)
            {
                if (ch.Value != null)
                {
                    if (ch.Value.type == CharacterType.Monster)
                    {
                        return ch.Value;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 读取卡片文件的信息
        /// </summary>
        void ReadCardList(int index)
        {
            List<string> actionData = new List<string>();
            string temp_ad1 = File.ReadAllText($@"cardinfo{index}.txt");
            foreach (var v in temp_ad1.Split('%'))
            {
                actionData.Add(v);
            }

            Action temp_a = new Action();
            int count = 0;
            foreach (var a in actionData)
            {
                temp_a = Action.ActionMaker(a);
                temp_a.id = count;
                actionArchive.Add(temp_a.name, temp_a);
                //gameLog.Add($"已添加{temp_a.name}              \n");
                count++;
            }
        }

        public void PutCharacter(Character chara, int posInAxis)
        {
            charaAxis[posInAxis] = chara;
            chara.posInAxis = posInAxis;
        }

        void CheckGame()
        {
            if (GetPlayer() == null || GetPlayer().curHp <= 0)
            {
                gameState = "GameOver";

            }
            if (GetMonster() == null || GetMonster().curHp <= 0)
            {
                gameState = "Next";
                gameLog.Add("你赢了                  ");
            }
            if ((GetMonster() == null || GetMonster().curHp <= 0) && battle.battle == 5)
            {
                gameState = "Win";
                gameLog.Add("你赢了Boss              ");
            }
        }
    }
}
