using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Battle
    {
        int turn;
        public int battle;
        int playerJob;
        Character player;
        Character monster;
        Random random;


        public void Init(int job)
        {
            random = new Random();
            battle = 1;
            playerJob = job;
            turn = 0;
        }

        public void Init()
        {
            if (Game.GetInstance().gameState == "Next")
            {

                Reset();
                switch (Game.GetInstance().gameDisplay.DrawHint($"你赢了{monster.name}",
                    "选择奖励吧", "", "",
                    "[1]增加最大HP 5点",
                    "[2]增加最大AP 2点",
                    "[3]获得一张卡",
                    "[...]跳过"))
                {
                    case "1":
                        player.maxHp += 5;
                        player.curHp = player.maxHp;
                        break;
                    case "2":
                        player.maxAp += 2;
                        player.curAp = player.maxAp;
                        break;
                    case "3":
                        int rand = random.Next(0, Game.GetInstance().actionArchive.Count - 1);
                        int count = 0;
                        foreach (var kvp in Game.GetInstance().actionArchive)
                        {
                            if (count == rand)
                            {
                                player.AddCard(kvp.Key);
                                Game.GetInstance().gameLog.Add($"添加了卡牌{kvp.Key}       ");
                                break;
                            }
                            count++;
                        }
                        break;
                }


                battle += 1;

            }
            if (turn == 0)
            {
                if (player == null)
                {
                    switch (playerJob)
                    {
                        case 1: player = Character.CreateFighterPlayer(); break;
                        case 2: player = Character.CreateCoachPlayer(); break;
                        case 3: player = Character.CreateProgrammerPlayer(); break;
                    }
                }
                if (battle == 5)
                {
                    monster = Character.CreateBoss();
                }
                else
                {
                    int a = random.Next(1, 10);
                    if (a <= 6) monster = Character.CreateMonster1();
                    if (a > 6) monster = Character.CreateMonster2();

                }

                Game.GetInstance().PutCharacter(player, 0);
                Game.GetInstance().PutCharacter(monster, 10);
                Game.GetInstance().GetPlayer().Shuffle(true);
                Game.GetInstance().GetPlayer().CardDraw_Once(false);
                Game.GetInstance().GetPlayer().CardDraw_Once(false);
                Game.GetInstance().GetPlayer().CardDraw_Once(false);
                Game.GetInstance().GetPlayer().CardDraw_Once(false);
                turn += 1;
            }

        }
        public void Reset()
        {
            turn = 0;
            for (int i = 0; i < Game.GetInstance().charaAxis.Count; i++)
            {
                Game.GetInstance().charaAxis[i] = null;
            }
            Game.GetInstance().gameLog.RemoveRange(0, Game.GetInstance().gameLog.Count);
            Point pos = new Point(Game.GetInstance().gameDisplay.fieldWidth - 25,
                Game.GetInstance().gameDisplay.fieldHeight + 3);
            Game.GetInstance().gameDisplay.ClearRect(pos, 25, 19, ConsoleColor.Black);
            player.curHp = player.maxHp;
            player.curAp = player.maxAp;
            player.faceTo = Direction.Right;
            player.handCards.RemoveRange(0, player.handCards.Count);
            string[] keys = player.modifier.Keys.ToArray();
            foreach (var i in keys)
            {
                player.modifier[i] = 0;
            }
            Game.GetInstance().gameState = "Normal";
        }
        public void RunBattle()
        {
            if (Game.GetInstance().gameState == "GameOver" || Game.GetInstance().gameState == "Win")
            {
                return;
            }
            Game.GetInstance().gameDisplay.DrawGameState($"       回合:{turn}       ");
            if (!CardChooser())
            {
                monster.maction();
                player.Shuffle(false);
                int count = player.handCards.Count;
                for (int i = 0; i < 4 - count; i++)
                {
                    player.CardDraw_Once(false);
                }
                player.curAp = player.maxAp;
                turn++;
            }
        }

        bool CardChooser()
        {
            if (Game.GetInstance().gameState == "Next")
            {
                Console.ReadKey(true);
                return true;
            }

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.LeftArrow:
                    if (Game.GetInstance().curPlayerAction > 0)
                        Game.GetInstance().curPlayerAction--;
                    break;


                case ConsoleKey.RightArrow:
                    if (Game.GetInstance().curPlayerAction < player.CountHand() - 1)
                        Game.GetInstance().curPlayerAction++;
                    break;

                case ConsoleKey.Enter:
                    player.UseCard(Game.GetInstance().curPlayerAction);
                    break;

                case ConsoleKey.Escape:
                    return false;

                case ConsoleKey.D:
                    List<string> strs = new List<string>();
                    foreach (var a in player.cards)
                    {
                        if (a.inDiscard == true)
                        {
                            strs.Add($"{a.name}");
                        }
                    }
                    Game.GetInstance().gameDisplay.DrawHintDeck("弃牌堆", strs.ToArray());
                    break;
                case ConsoleKey.L:
                    List<string> strs2 = new List<string>();
                    for (int count = 0; count < player.cards.Count; count++)
                    {
                        foreach (var a in player.cards)
                        {
                            if (a.deckId == count)
                            {
                                strs2.Add($"[{a.deckId}]{a.name}");
                            }
                        }
                    }

                    int cardInfo = -1;
                    bool flag = int.TryParse(Game.GetInstance().gameDisplay.DrawHintDeck("牌库", strs2.ToArray()), out cardInfo);

                    if (flag && cardInfo >= 0 && cardInfo < player.cards.Count)
                    {
                        Game.GetInstance().gameDisplay.DrawActionCard(
                            new Point(1, Game.GetInstance().gameDisplay.fieldHeight + 2)
                            , player.GetCard(cardInfo), ConsoleColor.Blue);
                        Console.ReadKey(true);
                        Game.GetInstance().gameDisplay.ClearRect(
                            new Point(1, Game.GetInstance().gameDisplay.fieldHeight + 2)
                            , 12, 14, ConsoleColor.Black);
                    }
                    break;
            }
            return true;
        }

        //todo: Multiple Actions
        public void MonsterAction1()
        {
            Game.GetInstance().gameDisplay.DrawGameState(" 怪物的回合 ");
            int jump = random.Next(3, 6);
            System.Threading.Thread.Sleep(500);
            if (monster.posInAxis - jump < 0 && monster.faceTo == Direction.Left)
            {
                monster.Turn();
            }
            if (monster.posInAxis + jump > 10 && monster.faceTo == Direction.Right)
            {
                monster.Turn();
            }
            System.Threading.Thread.Sleep(500);
            monster.Jump(jump);
            System.Threading.Thread.Sleep(500);
            if (monster.GetForward() != null)
            {
                monster.Attack(monster.GetForward(), 3);
            }
            System.Threading.Thread.Sleep(1000);
        }

        public void MonsterAction2()
        {
            Game.GetInstance().gameDisplay.DrawGameState(" 怪物的回合 ");
            int move = random.Next(2, 3);
            if (monster.posInAxis - move < 0 && monster.faceTo == Direction.Left)
            {
                monster.Turn();
            }
            if (monster.posInAxis + move > 10 && monster.faceTo == Direction.Right)
            {
                monster.Turn();
            }
            System.Threading.Thread.Sleep(500);
            monster.Move(move);
            System.Threading.Thread.Sleep(500);
            if (monster.GetForward() != null)
            {
                monster.Attack(monster.GetForward(), 5);
            }
            System.Threading.Thread.Sleep(1000);
        }

        public void BossAction()
        {
            Game.GetInstance().gameDisplay.DrawGameState(" BOSS的回合 ");
            List<int> blockPos = new List<int>();

            foreach (var a in Game.GetInstance().charaAxis)
            {
                if (a.Value != null)
                {
                    if (a.Value.type == CharacterType.Block)
                    {
                        blockPos.Add(a.Key);
                    }
                }
            }

            if (blockPos.Count < 3)
            {
                int pos = 0;
                bool ok = false;
                while (!ok)
                {
                    pos = random.Next(0, 10);
                    if (pos != player.posInAxis && pos != monster.posInAxis && !blockPos.Contains(pos))
                    {
                        ok = true;
                    }
                }
                System.Threading.Thread.Sleep(500);
                Game.GetInstance().PutCharacter(Character.CreateBlock(), pos);
                Game.GetInstance().gameLog.Add("BOSS召唤了障碍物！       ");
                System.Threading.Thread.Sleep(500);
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                if (monster.GetForward() != null)
                {
                    if (monster.GetForward().type == CharacterType.DefaultPlayer)
                    {
                        System.Threading.Thread.Sleep(500);
                        monster.Attack(monster.GetForward(), 10);
                        System.Threading.Thread.Sleep(500);
                        return;
                    }
                    else
                    {
                        monster.Turn();
                        if (monster.GetForward() != null)
                        {
                            if (monster.GetForward().type == CharacterType.DefaultPlayer)
                            {
                                System.Threading.Thread.Sleep(500);
                                monster.Attack(monster.GetForward(), 10);
                                System.Threading.Thread.Sleep(500);
                                return;
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(500);
                Game.GetInstance().charaAxis[monster.posInAxis] = null;
                int pos = 0;
                bool ok = false;
                while (!ok)
                {
                    pos = random.Next(0, 10);
                    if (pos != player.posInAxis && pos != monster.posInAxis && !blockPos.Contains(pos))
                    {
                        ok = true;
                    }
                }
                Game.GetInstance().PutCharacter(monster, pos);
                Game.GetInstance().gameLog.Add("BOSS瞬移了！            ");
                System.Threading.Thread.Sleep(500);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
