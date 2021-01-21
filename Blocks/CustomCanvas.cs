using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Blocks
{
    class CustomCanvas
    {
        private Canvas GameArea;
        private List<UIElement> shapes = new List<UIElement>();
       

        public CustomCanvas(Canvas area)
        {
            GameArea = area;
        }
   
        public void Add(UIElement rect)
        {
            shapes.Add(rect);
            GameArea.Children.Add(rect);
        }
        public void Remove(UIElement rect)
        {
            GameArea.Children.Remove(rect);
            shapes.Remove(rect);
        }
        public void Add(Quad q)
        {
            if (Contains(q))
                return;
            shapes.Add(q.GetRectangle());
            GameArea.Children.Add(q.GetRectangle());

        }
        public bool Contains(Quad q)
        {
            return GameArea.Children.Contains(q.GetRectangle());
        }
        public void Remove(Quad q)
        {
            GameArea.Children.Remove(q.GetRectangle());
            shapes.Remove(q.GetRectangle());
        }

        public void resetCanvas()
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                GameArea.Children.Remove(shapes.ElementAt(i));
            }
            shapes.Clear();
        }
        public void setPosition(Quad q)
        {
            Canvas.SetLeft(q.GetRectangle(), q.getPosition().X);
            Canvas.SetTop(q.GetRectangle(), q.getPosition().Y);
        }

        public void setPosition(UIElement rect, double x, double y)
        {
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }
    }
}
