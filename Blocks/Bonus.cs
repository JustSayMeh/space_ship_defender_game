using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Blocks
{
    class RepairKit : Quad
    {
        public int hill_count = 20;
        BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/Images/repair_kit.png", UriKind.Absolute));
        public RepairKit(Point pos, int xsize) : base(pos, xsize, Brushes.Red)
        {
            ImageBrush myImageBrush = new ImageBrush(image);
            rect.Fill = myImageBrush;
        }
    }

    class DoubleDamage : Quad
    {
        public int life_time = 8;
        BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/Images/bluster_mod.png", UriKind.Absolute));
        public DoubleDamage(Point pos, int xsize) : base(pos, xsize, Brushes.Red)
        {
            ImageBrush myImageBrush = new ImageBrush(image);
            rect.Fill = myImageBrush;
        }
    }
}
