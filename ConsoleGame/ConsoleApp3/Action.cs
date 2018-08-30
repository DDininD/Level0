using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace ConsoleGame
{
    delegate void ActionTempl(Character doer);
    class Action
    {
        public Character doer;
        public Character target;

        public string name;
        public int cost;
        public int damage;
        public int movement;
        public int increment;
        public int thrust;
        public int jump;
        public string modifyHint;

        public int id;
        public int deckId;

        public bool inHand;
        public bool inDiscard;
        public string describe;

        ActionTempl actionTaker;
        public Action()
        {
            describe = "";
            inHand = false;
            inDiscard = false;
        }


        public static Action ActionMaker(string data)
        {
            List<string> strs = new List<string>();
            Action temp_a = new Action();

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '$')
                {
                    string temp = "";
                    for (int j = i + 1; ; j++)
                    {
                        if (j >= data.Length || data[j] == '$') break;
                        else temp += data[j];
                    }
                    if (temp != "") strs.Add(temp);
                }
            }

            //foreach (var s in strs) Game.GetInstance().gameLog.Append(s + "\n");

            foreach (var s in strs)
            {
                string temp_s = "";
                switch (s[0])
                {
                    case 'n':
                        temp_s = s.Substring(1);
                        temp_a.name = temp_s;
                        break;
                    case 'c':
                        temp_s = s.Substring(1);
                        temp_a.cost = int.Parse(temp_s);
                        temp_a.describe += $"耗:{int.Parse(temp_s)}\n\n";
                        break;
                    case 'm':
                        temp_s = s.Substring(1);
                        temp_a.movement = int.Parse(temp_s);
                        temp_a.actionTaker += temp_a.MoveAction;
                        temp_a.describe += $"移:{int.Parse(temp_s)}\n";
                        break;
                    case 'a':
                        temp_s = s.Substring(1);
                        temp_a.damage = int.Parse(temp_s);
                        temp_a.actionTaker += temp_a.AttackAction;
                        temp_a.describe += $"伤:{int.Parse(temp_s)}\n";

                        break;
                    case 'h':
                        temp_s = s.Substring(1);
                        temp_a.increment = int.Parse(temp_s);
                        temp_a.actionTaker += temp_a.HealAction;
                        temp_a.describe += $"治:{int.Parse(temp_s)}\n";
                        break;
                    case 'd':
                        temp_a.actionTaker += temp_a.DrawAction;
                        temp_a.describe += $"抽\n";
                        break;
                    case 't':
                        temp_a.actionTaker += temp_a.TurnAction;
                        temp_a.describe += $"转\n";
                        break;
                    case 'p':
                        temp_s = s.Substring(1);
                        temp_a.thrust = int.Parse(temp_s);
                        temp_a.actionTaker += temp_a.PushAction;
                        temp_a.describe += $"推:{int.Parse(temp_s)}\n";
                        break;
                    case 'j':
                        temp_s = s.Substring(1);
                        temp_a.jump = int.Parse(temp_s);
                        temp_a.actionTaker += temp_a.JumpAction;
                        temp_a.describe += $"跳:{int.Parse(temp_s)}\n";
                        break;
                    case 'M':
                        temp_a.modifyHint = s.Substring(1);
                        temp_a.actionTaker += temp_a.ModAction;
                        temp_a.describe += $"+{temp_a.modifyHint}\n";
                        break;
                    case 'R':
                        temp_a.actionTaker += temp_a.ResetAction;
                        temp_a.describe += $"重启一局\n";
                        temp_a.describe += $"游戏\n";
                        break;

                    case 'S':
                        temp_a.actionTaker += temp_a.SuperMoveAction;
                        temp_a.describe += $"控制人物\n";
                        temp_a.describe += $"移动吧！\n";
                        temp_a.describe += $"按回车确\n";
                        temp_a.describe += $"认\n";
                        break;
                    case 'T':
                        temp_a.actionTaker += temp_a.SuperState;
                        temp_a.describe += $"全属性\n";
                        temp_a.describe += $"+2\n";
                        break;
                }
            }

            return temp_a;

        }

        void AttackAction(Character doer)
        {
            target = doer.GetForward();
            doer.Attack(target, damage);
        }

        void HealAction(Character doer)
        {
            doer.BeHeal(increment);

        }

        void MoveAction(Character doer)
        {
            doer.Move(movement);
        }

        void DrawAction(Character doer)
        {

            doer.CardDraw_Once(true);
        }

        void TurnAction(Character doer)
        {
            doer.Turn();
        }

        void PushAction(Character doer)
        {
            doer.Push(thrust);
        }

        void JumpAction(Character doer)
        {
            doer.Jump(jump);
        }

        void ModAction(Character doer)
        {
            doer.CreateModifier(modifyHint);
        }
        void ResetAction(Character doer)
        {
            Game.GetInstance().battle.Reset();
        }
        void SuperMoveAction(Character doer)
        {
            Game.GetInstance().gameDisplay.DrawGameState(" 超级移动 ");
            choose:
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.LeftArrow:
                    if (doer.faceTo == Direction.Left)
                    {
                        doer.Move(1);
                        Game.GetInstance().gameDisplay.DrawGame();
                        goto choose;
                    }
                    else
                    {
                        doer.Turn();
                        doer.Move(1);
                        Game.GetInstance().gameDisplay.DrawGame();
                        goto choose;
                    }

                case ConsoleKey.RightArrow:
                    if (doer.faceTo == Direction.Right)
                    {
                        doer.Move(1);
                        Game.GetInstance().gameDisplay.DrawGame();
                        goto choose;
                    }
                    else
                    {
                        doer.Turn();
                        doer.Move(1);
                        Game.GetInstance().gameDisplay.DrawGame();
                        goto choose;
                    }
                case ConsoleKey.Enter:
                    return;
            }
        }

        void SuperState(Character doer)
        {
            string[] keys = doer.modifier.Keys.ToArray();
            for(int i = 0; i < keys.Length; i++)
            {
                doer.modifier[keys[i]] = 2;
            }
        }

        public bool TakeAcion(Character doer)
        {
            if (doer.curAp - cost < 0)
            {
                return false;
            }
            doer.curAp -= cost;
            actionTaker(doer);
            return true;
        }
        public Action Copy()
        {
            Action a = new Action();
            a.name = name;
            a.cost = cost;
            a.damage = damage;
            a.movement = movement;
            a.increment = increment;
            a.describe = describe;
            a.actionTaker = actionTaker;
            a.id = id;
            a.deckId = deckId;
            return a;
        }

    }
}
