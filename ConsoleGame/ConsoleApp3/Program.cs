using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            SoundPlayer s = new SoundPlayer(@"media.wav");
            s.PlayLooping();
            Game.GetInstance().Run();
            //Console.ReadKey();//要是有什么不对，把上面的注释掉，用这个来调整吧（😂）
        }
    }
}
