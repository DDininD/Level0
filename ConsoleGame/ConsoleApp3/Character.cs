using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{


    public enum CharacterType
    {
        DefaultPlayer,
        Monster,
        Block
    }

    public enum Direction
    {
        Left,
        Right
    }
    class Character
    {
        public static char[,] fighterImg = new char[,]
        {
            {' ','Y',' '},
            {'/','#','\\'},
            {' ','V',' ' },
            {'/',' ','\\'}
        };

        public static char[,] coachImg = new char[,]
        {
            {' ','0',' '},
            {'/','0','\\'},
            {' ','0',' ' },
            {'/',' ','\\'}
        };

        public static char[,] programmerImg = new char[,]
        {
            {'-','o','o'},
            {'/','|','\\'},
            {' ','|',' ' },
            {'/',' ','\\'}
        };

        public static char[,] bossImg = new char[,]
        {
            {'o','/','\\','o'},
            {'|','[',']','|'},
            {' ','p','q' ,' '},
            {' ','/','\\',' '}
        };


        public static char[,] monsterImg = new char[,]
        {
            {' ',' ',' ',' '},
            {' ','0','0',' '},
            {' ','/','\\',' '},
            {'/','_','_','\\'}
        };

        public static char[,] monsterImg1 = new char[,]
        {
            {'\\','/','\\','/'},
            {'-','0','0','-'},
            {'-','0','0','-'},
            {'/','o','o','\\'}
        };
        public static char[,] blockImg = new char[,]
        {
            {' ','_',' '},
            {'|',' ','|'},
            {'|','+','|' },
            {'|','_','|'}
        };

        public CharacterType type = CharacterType.DefaultPlayer;
        public Random random;
        public string name;

        public Direction faceTo;
        public int posInAxis;
        public char[,] img;

        public int maxHp,
                   curHp;
        public int maxAp,
                   curAp;
        public MonsterAction maction;


        public List<State> states = new List<State>();
        public List<Action> cards = new List<Action>();
        public List<Action> handCards = new List<Action>();

        public Character()
        {
            random = new Random();
        }
        public static Character CreateProgrammerPlayer()
        {
            Character player = new Character();
            player.img = programmerImg;
            player.name = "黑客";
            player.maxHp = 5;
            player.curHp = 5;
            player.maxAp = 8;
            player.curAp = 8;
            player.faceTo = Direction.Right;
            player.AddCard("转向");
            player.AddCard("移动2");
            player.AddCard("攻击1");
            player.AddCard("攻击1");
            player.AddCard("推挤1");
            player.AddCard("入侵系统");
            player.AddCard("编程(攻)");
            player.AddCard("编程(推)");
            //player.AddCard("黑客滑板");
            //player.AddCard("重启世界");
            //player.AddCard("虚拟现实");
            return player;

        }

        public static Character CreateCoachPlayer()
        {
            Character player = new Character();
            player.img = coachImg;
            player.name = "教练";
            player.maxHp = 8;
            player.curHp = 8;
            player.maxAp = 6;
            player.curAp = 6;
            player.faceTo = Direction.Right;
            player.AddCard("转向");
            player.AddCard("移动2");
            player.AddCard("铁山靠");
            player.AddCard("推挤2");
            player.AddCard("推挤3");
            player.AddCard("撞击");
            player.AddCard("健身1");
            player.AddCard("肌肉饮料");
            //player.AddCard("健康饮料");
            //player.AddCard("超级移动");
            return player;
        }


        public static Character CreateFighterPlayer()
        {
            Character player = new Character();
            player.img = fighterImg;
            player.name = "战士";
            player.maxHp = 10;
            player.curHp = 10;
            player.maxAp = 5;
            player.curAp = 5;
            player.faceTo = Direction.Right;
            player.AddCard("转向");
            player.AddCard("移动1");
            player.AddCard("移动2");
            player.AddCard("攻击1");
            player.AddCard("翻滚");
            player.AddCard("跳斩");
            player.AddCard("原素瓶");
            player.AddCard("背刺");
            //player.AddCard("燃烧自我");
            //player.AddCard("附魔斩");
            return player;
        }

        public static Character CreateMonster1()
        {
            Character monster = new Character();
            monster.name = "史莱姆";
            monster.curHp = 5;
            monster.maxHp = 5;
            monster.img = monsterImg;
            monster.faceTo = Direction.Left;
            monster.type = CharacterType.Monster;
            monster.maction = Game.GetInstance().battle.MonsterAction1;
            Game.GetInstance().gameLog.Add($"史莱姆出现了！           ");
            Game.GetInstance().gameLog.Add($"-----------------------");
            Game.GetInstance().gameLog.Add($"史莱姆是会跳来跳去的怪物 ");
            Game.GetInstance().gameLog.Add($"但是绝对不会跳出边缘     ");
            Game.GetInstance().gameLog.Add($"掌握好节奏，就可以抓住它 ");
            Game.GetInstance().gameLog.Add($"-----------------------");


            return monster;
        }
        public static Character CreateMonster2()
        {
            Character monster = new Character();
            monster.name = "蜘蛛";
            monster.curHp = 8;
            monster.maxHp = 8;
            monster.img = monsterImg1;
            monster.faceTo = Direction.Left;
            monster.type = CharacterType.Monster;
            monster.maction = Game.GetInstance().battle.MonsterAction2;
            Game.GetInstance().gameLog.Add($"蜘蛛出现了！             ");
            Game.GetInstance().gameLog.Add($"-----------------------");
            Game.GetInstance().gameLog.Add($"蜘蛛移动的速度不快      ");
            Game.GetInstance().gameLog.Add($"但是攻击高，血量多       ");
            Game.GetInstance().gameLog.Add($"万一它靠近了你          ");
            Game.GetInstance().gameLog.Add($"千万一定要小心谨慎！     ");
            Game.GetInstance().gameLog.Add($"-----------------------");


            return monster;
        }

        public static Character CreateBoss()
        {
            Character monster = new Character();
            monster.name = "鬼怪";
            monster.curHp = 14;
            monster.maxHp = 14;
            monster.img = bossImg;
            monster.faceTo = Direction.Left;
            monster.type = CharacterType.Monster;
            monster.maction = Game.GetInstance().battle.BossAction;

            Game.GetInstance().gameLog.Add($"一个奇怪的生物出现了...  ");
            Game.GetInstance().gameLog.Add($"-----------------------");
            Game.GetInstance().gameLog.Add($"它似乎就是幕后黑手了     ");
            Game.GetInstance().gameLog.Add($"但是我们知道它的信息不多 ");
            Game.GetInstance().gameLog.Add($"不管怎样，干掉它！       ");
            Game.GetInstance().gameLog.Add($"-----------------------");
            return monster;
        }

        public bool BeHit(int damage)
        {
            curHp -= damage;
            return true;
        }

        public bool BeHeal(int increment)
        {
            increment += +modifier["治"];

            curHp += increment;

            if (curHp > maxHp) curHp = maxHp;

            Game.GetInstance().gameLog.Add($"{name}治疗了自己{increment}点生命值  ");
            return true;
        }

        public bool Attack(Character target, int damage)
        {
            damage += modifier["攻"];
            if (target == null)
            {
                Game.GetInstance().gameLog.Add($"{name}前方没有目标       ");
                return false;
            }
            target.BeHit(damage);
            Game.GetInstance().gameLog.Add($"{name}对{target.name}造成{damage}点伤害");
            return true;
        }

        public void Jump(int jump)
        {
            jump += modifier["跳"];
            if (faceTo == Direction.Left)
            {
                int targetpos = posInAxis - jump;
                if (targetpos < 0 || targetpos > 10)
                {
                    Game.GetInstance().gameLog.Add($"{name}跳出了界限        ");
                    Game.GetInstance().charaAxis[posInAxis] = null;
                    return;
                }
                start1:
                if (Game.GetInstance().getItemInAxisAt(targetpos) == null)
                {
                    Game.GetInstance().charaAxis[posInAxis] = null;
                    Game.GetInstance().PutCharacter(this, targetpos);
                    Game.GetInstance().gameLog.Add($"{name}向前跳了{jump}格        ");
                }
                else
                {
                    targetpos++;
                    goto start1;
                }

            }
            else
            {
                int targetpos = posInAxis + jump;
                if (targetpos < 0 || targetpos > 10)
                {
                    Game.GetInstance().gameLog.Add($"{name}跳出了界限        ");
                    Game.GetInstance().charaAxis[posInAxis] = null;

                    return;
                }
                start2:
                if (Game.GetInstance().getItemInAxisAt(targetpos) == null)
                {
                    Game.GetInstance().charaAxis[posInAxis] = null;
                    Game.GetInstance().PutCharacter(this, targetpos);
                    Game.GetInstance().gameLog.Add($"{name}向前跳了{jump}格       ");

                }
                else
                {
                    targetpos--;
                    goto start2;
                }
            }
        }
        public void Push(int thrust)
        {
            thrust += modifier["推"];
            Character target = GetForward();
            if (target == null) { Game.GetInstance().gameLog.Add("你前方没有目标          "); return; }
            if (target.faceTo == faceTo)
            {
                target.Move(thrust);
            }
            else
            {
                target.Turn();
                target.Move(thrust);
                target.Turn();
            }
            Game.GetInstance().gameLog.Add($"{name}将{target.name}推了{thrust}格     ");
        }


        public void Move(int movement)
        {
            movement += modifier["移"];
            var axis = Game.GetInstance().charaAxis;
            if (faceTo == Direction.Left)
            {

                axis[posInAxis] = null;
                for (int i = posInAxis - 1; i >= posInAxis - movement; i--)
                {
                    if (i < 0 || i > 10) break;
                    if (axis[i] != null)
                    {
                        Game.GetInstance().gameLog.Add($"{name}移动了{posInAxis - i + 1}格          ");
                        Game.GetInstance().PutCharacter(this, i + 1);
                        return;
                    }
                }
                if (posInAxis - movement > 10 || posInAxis - movement < 0)
                {
                    Game.GetInstance().gameLog.Add($"{name}移出了界限        ");
                    return;
                }
                Game.GetInstance().PutCharacter(this, posInAxis - movement);
                Game.GetInstance().gameLog.Add($"{name}移动了{movement}格          ");
            }
            else
            {

                axis[posInAxis] = null;
                for (int i = posInAxis + 1; i <= posInAxis + movement; i++)
                {
                    if (i < 0 || i > 10) break;
                    if (axis[i] != null)
                    {
                        Game.GetInstance().gameLog.Add($"{name}移动了{i - posInAxis - 1}格          ");
                        Game.GetInstance().PutCharacter(this, i - 1);
                        return;
                    }
                }
                if (posInAxis + movement > 10 || posInAxis + movement < 0)
                {
                    Game.GetInstance().gameLog.Add($"{name}移出了界限        ");
                    return;
                }
                Game.GetInstance().PutCharacter(this, posInAxis + movement);
                Game.GetInstance().gameLog.Add($"{name}移动了{movement}格          ");

            }
        }
        public Action GetCard(int deckId)
        {
            foreach (var a in cards)
            {
                if (a.deckId == deckId) return a;
            }
            return null;
        }

        public Character GetForward()
        {
            Game game = Game.GetInstance();
            if (faceTo == Direction.Left)
            {
                if (posInAxis - 1 > 10 || posInAxis - 1 < 0) return null;
                return game.getItemInAxisAt(posInAxis - 1);
            }
            else
            {
                if (posInAxis + 1 > 10 || posInAxis + 1 < 0) return null;
                return game.getItemInAxisAt(posInAxis + 1);
            }
        }

        public List<Action> GetHandCards()
        {
            var temp_l = new List<Action>();
            foreach (var c in cards)
            {
                if (c.inHand) temp_l.Add(c);
            }
            return temp_l;
        }

        public static Character CreateBlock()
        {
            Character block = new Character();
            block.name = "障碍物";
            block.img = blockImg;
            block.faceTo = Direction.Left;
            block.type = CharacterType.Block;
            return block;
        }

        public void AddCard(string name)
        {
            Action action = new Action();
            action = Game.GetInstance().actionArchive[name].Copy();
            action.deckId = cards.Count;
            cards.Add(action);
        }

        public void UseCard(int i)
        {
            //if (GetHandCards().Count > 0)
            //{
            //    int count = 0;
            //    foreach(var a in handCards)
            //    {
            //        if (i == count && a.inHand)
            //        {
            //            a.doer = Game.GetInstance().GetPlayer();

            //            if (!a.TakeAcion(a.doer))
            //            {
            //                Game.GetInstance().gameLog.Add("AP不够                 ");
            //                return;
            //            }

            //            a.inHand = false;
            //            a.inDiscard = true;
            //            handCards.Remove(a);
            //            break;
            //        }
            //        else if (a.inHand)
            //        {
            //            count++;
            //        }

            //    }

            //}
            if (handCards.Count != 0)
            {
                handCards[i].doer = Game.GetInstance().GetPlayer();
                if (!handCards[i].TakeAcion(handCards[i].doer))
                {
                    Game.GetInstance().gameLog.Add("AP不够                 ");
                    return;
                }
                else
                {
                    handCards[i].inHand = false;
                    handCards[i].inDiscard = true;
                    handCards.Remove(handCards[i]);
                }
            }
            Game.GetInstance().curPlayerAction = 0;

        }

        public void CardDraw_Once(bool isCost)
        {
            Game.GetInstance().gameLog.Add($"{name}尝试着抽了一张牌    ");
            if (!isCost)
            {
                if (CountDeck() == 0)
                {
                    Game.GetInstance().gameLog.Add("牌库已空                 ");
                    return;
                }
                if (CountHand() == 4)
                {
                    Game.GetInstance().gameLog.Add("手牌已满                 ");
                    return;
                }

            }
            if (CountDeck() == 0)
            {
                Game.GetInstance().gameLog.Add("牌库已空                 ");
                return;
            }
            foreach (var c in cards)
            {

                if (c.inHand == false && c.inDiscard == false)
                {
                    c.inHand = true;
                    handCards.Add(c);
                    return;
                }
            }
        }

        public void Turn()
        {
            faceTo = faceTo == Direction.Left ? Direction.Right : Direction.Left;
            Game.GetInstance().gameLog.Add($"{name}转向了          ");
        }

        public void CardDraw_All()
        {

        }

        public void Shuffle(bool isEnd)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                var rand = random.Next(i, cards.Count);
                var temp = cards[rand];
                cards[rand] = cards[i];
                cards[i] = temp;
            }
            if (isEnd)
            {
                foreach (var c in cards)
                {
                    c.inDiscard = false;
                    c.inHand = false;
                }
            }
            else
            {
                //不放回手牌
                //foreach (var c in cards)
                //{
                //    if (c.inHand) continue;
                //    c.inDiscard = false;
                //}

                //放回手牌
                handCards.RemoveRange(0, handCards.Count);
                foreach (var c in cards)
                {
                    c.inHand = false;
                    c.inDiscard = false;
                }


            }
        }

        public int CountHand()
        {
            int count = 0;
            foreach (var a in cards)
            {
                if (a.inHand)
                {
                    count++;
                }
            }
            return count;
        }

        public int CountDeck()
        {
            int count = 0;
            foreach (var a in cards)
            {
                if (!a.inDiscard && !a.inHand)
                {
                    count++;
                }
            }
            return count;
        }

        public int CountDiscard()
        {
            int count = 0;
            foreach (var a in cards)
            {
                if (a.inDiscard)
                {
                    count++;
                }
            }
            return count;
        }

        public Dictionary<string, int> modifier = new Dictionary<string, int>()
        {
            {"推",0},
            {"移",0},
            {"攻",0},
            {"跳",0},
            {"治",0},
        };

        public void CreateModifier(string hint)
        {
            modifier[hint[0].ToString()] = int.Parse(hint.Substring(1));
        }





    }

    delegate void MonsterAction();

    class Block : Character
    {

    }
}
