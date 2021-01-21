using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Blocks
{
    class Tools
    {
        //public static Bonus generateBonus(int block_size, int w_count, int h_count)
        //{
        //    Random rand = new Random();
        //    double r = rand.NextDouble();
        //    int x = rand.Next(1, w_count - 1);
        //    int y = rand.Next(1, h_count - 1);
        //    Point pos = new Point(block_size * x, block_size * y);
        //    if (r < 0.1)
        //    {
        //        return new NoClipBonus(pos, block_size);
        //    }
        //    else
        //    {
        //        return new DoubleSpeedBonus(pos, block_size);
        //    }

        //}

        //public static bool validateHeadMove(Snake snake)
        //{
        //    Quad head = snake.parts.Last();
        //    for (int i = 0; i < snake.parts.Count - 1; i++)
        //    {
        //        Quad th = snake.parts.ElementAt(i);
        //        if (hasColision(head, th))
        //            return false;
        //    }
        //    return true;
        //}


        public static bool hasColision(Quad a, Quad b)
        {
            if (a.position.X >= b.position.X && a.position.X  < b.position.X + b.getWidth() 
                && a.position.Y < b.position.Y + b.getHeight() 
                && a.position.Y > b.position.Y)
                return true;
            return false;
        }

        //public static bool hasBorderCollision(Snake snake, int block_size, double width, double height)
        //{
        //    Quad q = snake.parts.Last();
        //    Point pos = q.getPosition();
        //    return pos.X < block_size || pos.X > width - 1.5 * block_size || pos.Y < block_size || pos.Y > height - 1.5 * block_size;
        //}


    
        public static T hasBlockColision<T>(Quad q, LinkedList<T> quads) where T : Quad
        {
            foreach (T th in quads)
            {
                if (hasColision(q, th))
                    return th;
            }
            return null;
        }


    }
}
