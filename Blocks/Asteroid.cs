using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Blocks
{
    class Asteroid : Quad
    {
        public int lifecount = 1, damage = 1, speed = 7;
        BitmapImage stage1 = new BitmapImage(new Uri("pack://application:,,,/Images/stage1.png", UriKind.Absolute));
        public Asteroid(Point pos, int xsize) : base(pos, xsize, Brushes.Red)
        {

            ImageBrush myImageBrush = new ImageBrush(stage1);
            rect.Fill = myImageBrush;
            rect.RenderTransform = new RotateTransform(-90, getWidth() / 2, getHeight() / 2);
        }

        public int getLifeCount()
        {
            return lifecount;
        }
        public virtual int decrementLifeCount(int dec)
        {
            return lifecount -= dec;
        }
        public int getDamage()
        {
            return damage;
        }
    }

    class SmallAsteroid : Asteroid
    {
        BitmapImage stage3 = new BitmapImage(new Uri("pack://application:,,,/Images/stage3.png", UriKind.Absolute));
        public SmallAsteroid(Point pos) : base(pos, 40)
        {
            damage = 3;
            lifecount = 2;
            speed = 6;
        }
        public override int decrementLifeCount(int dec)
        {
            lifecount -= dec;
            if (lifecount == 1)
            {
                ImageBrush myImageBrush = new ImageBrush(stage3);
                rect.Fill = myImageBrush;
            }
            return lifecount;
        }

    }

    class BigAsteroid : Asteroid
    {
        BitmapImage stage2 = new BitmapImage(new Uri("pack://application:,,,/Images/stage2.png", UriKind.Absolute));
        BitmapImage stage3 = new BitmapImage(new Uri("pack://application:,,,/Images/stage3.png", UriKind.Absolute));
        public BigAsteroid(Point pos) : base(pos, 60)
        {
            lifecount = 3;
            damage = 10;
            speed = 5;
        }

        public override int decrementLifeCount(int dec)
        {
            lifecount -= dec;
            if (lifecount == 2)
            {
                ImageBrush myImageBrush = new ImageBrush(stage2);
                rect.Fill = myImageBrush;
            }
            else if (lifecount == 1)
            {
                ImageBrush myImageBrush = new ImageBrush(stage3);
                rect.Fill = myImageBrush;
            }
            return lifecount;
        }
    }
}
