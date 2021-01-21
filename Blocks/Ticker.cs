using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocks
{
    class Timer
    {
        private System.Windows.Threading.DispatcherTimer timer;
        public Timer(double time, EventHandler func)
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += func;
            timer.Interval = TimeSpan.FromMilliseconds(time);
        }

        public void start()
        {
            timer.IsEnabled = true;
        }

        public void stop()
        {
            timer.IsEnabled = false;
        }
        public bool isRunning()
        {
            return timer.IsEnabled;
        }
    }
}
