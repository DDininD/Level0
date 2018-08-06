using System.Collections.Generic;

namespace SnakeConsole
{
    #region class Snake 蛇类，初始化贪食蛇和蛇的行为，如移动，吃食物，成长
    class Snake
    {

        int INITIAL_LENTH = 3;
        Point INITIAL_POSITION = new Point(10,10);
        Direction INITIAL_DIRECTION = Direction.Down;

        public Direction CurDir;
        public Point head;
        public List<Point> bodies = new List<Point>();

        public Snake()
        {
            CurDir = INITIAL_DIRECTION;
            head = INITIAL_POSITION;
            bodies.Add(head);
        }

        public Point growth = null;

        public void eatFood()
        {
            growth = bodies[bodies.Count - 1];
        }


        public void InitSnake()
        {
            for(int i = 1; i < INITIAL_LENTH; i++)
            {
                switch (CurDir)
                {
                    case Direction.Down:
                        bodies.Add(new Point(head.X, head.Y - i));
                        break;
                    case Direction.Up:
                        bodies.Add(new Point(head.X, head.Y + i));
                        break;
                    case Direction.Left:
                        bodies.Add(new Point(head.X + i, head.Y));
                        break;
                    case Direction.Right:
                        bodies.Add(new Point(head.X - i, head.Y));
                        break;
                    default:
                        break;
                }
            }
        }
        public void Move()
        {
            bodies.RemoveAt(bodies.Count-1);
            Point newHead = null;
            switch (CurDir)
            {
                case Direction.Down:
                    newHead = new Point(head.X, head.Y + 1);
                    break;
                case Direction.Up:
                    newHead = new Point(head.X, head.Y - 1);
                    break;
                case Direction.Left:
                    newHead = new Point(head.X - 1, head.Y);
                    break;
                case Direction.Right:
                    newHead = new Point(head.X + 1, head.Y);
                    break;
            }

            bodies.Insert(0, newHead);
            head = newHead;



        }
    }
    #endregion
}
