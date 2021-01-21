using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Blocks
{
    class Quad
    {
        public Point position;
        public Rectangle rect;
        public double rotate = 0;
        public Quad(Point pos, int size, Brush color)
        {
            position = pos;
            


            this.rect = new Rectangle
            {
                Width = size,
                Height = size,
                Fill = color
            };
        }
        public Quad(Point pos, int xsize, int ysize, Brush color)
        {
            position = pos;
           

          
            this.rect = new Rectangle
            {
                Width = xsize,
                Height = ysize,
                Fill = color
            };
        }
        public Point GetPoint()
        {
            return position;
        }

        public Rectangle GetRectangle()
        {
            return rect;
        }
        public void setColor(SolidColorBrush color)
        {
            rect.Fill = color;
        }
        public Point getPosition()
        {
            return position;
        }
        public void setOpacity(double opacity)
        {
            rect.Opacity = opacity;
        }

        public double getWidth()
        {
            return rect.Width;
        }
        public double getHeight()
        {
            return rect.Height;
        }
        public void setRotate(double f)
        {
            rect.RenderTransform = new RotateTransform(f, getWidth() / 2, getHeight() / 2);
            rotate = f;
        }
        public double getRotate()
        {
            return rotate;
        }
    }
    

    class PlayerGun : Quad
    {
        public int lifecount = 100;
        public PlayerGun(Point pos, int xsize) : base(pos, xsize, Brushes.Red)
        {
            BitmapImage theImage = new BitmapImage(new Uri("pack://application:,,,/Images/pixelart.png", UriKind.Absolute));
            ImageBrush myImageBrush = new ImageBrush(theImage);
            rect.Fill = myImageBrush;
            rect.RenderTransform = new RotateTransform(-90, getWidth() / 2, getHeight() / 2);
        }       

        public int decrementLife(int dec)
        {
            return lifecount -= dec;
        }
        public void incrementLife(int inc)
        {
            lifecount += inc;
        }
    }



    class StagedImagePanel : Quad
    {
        List<BitmapImage> stages;
        int current_stage;
        public StagedImagePanel(Point pos, int xsize, int ysize, List<BitmapImage> stages) : base(pos, xsize, ysize, Brushes.Red)
        {
            this.stages = stages;
            ImageBrush myImageBrush = new ImageBrush(stages.ElementAt(0));
            rect.Fill = myImageBrush;
        }

        public void setStage(int stage_s)
        {
            if (current_stage == stage_s)
                return;
            current_stage = stage_s;
            ImageBrush myImageBrush = new ImageBrush(stages.ElementAt(current_stage));
            rect.Fill = myImageBrush;
        }


        public void setNextStage()
        {
            if (current_stage < stages.Count)
                current_stage += 1;
            ImageBrush myImageBrush = new ImageBrush(stages.ElementAt(current_stage));
            rect.Fill = myImageBrush;
        }

        public void setPreviosStage()
        {
            
            if (current_stage >= 0)
                current_stage -= 1;
            ImageBrush myImageBrush = new ImageBrush(stages.ElementAt(current_stage));
            rect.Fill = myImageBrush;
        }
        public int getStageCount()
        {
            return stages.Count;
        }
    }

    class Bluster : Quad
    {
        public int damage = 1;
        public int speed = 10;
        public Bluster(Point pos) : base(pos, 2, 10, Brushes.Green)
        {

        }
    }

    class BlueBluster : Quad
    {
        public int damage = 2;
        public int speed = 20;
        public BlueBluster(Point pos) : base(pos, 2, 10, Brushes.Blue)
        {

        }
    }
}
