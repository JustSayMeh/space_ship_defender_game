using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Blocks
{
    public partial class MainWindow : Window
    {
        const string best_score_variable_name = "ShipScore";
        static int block_size = 20;
        double width = 0;
        double height = 0;
        int h_count, w_count, red_disp_count = -1, green_disp_count = -1, score = 0;
        Quad red_disp, green_disp;
        LinkedList<Quad> powers = new LinkedList<Quad>();
        LinkedList<Asteroid> blocks = new LinkedList<Asteroid>();
        LinkedList<Quad> bonuses = new LinkedList<Quad>();
        CustomCanvas canvas;
        IntSystemVariable bestscore;
        Timer timer, timer2, timer3;
        static int tick_time = 100;
        PlayerGun player;
        Label score_label = new Label();
        Label life_label = new Label();
        StagedImagePanel ship_panel;

        int double_damage_count = 0;
        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer(20, timer_tick1);
            timer2 = new Timer(1300, timer_tick2);
            timer3 = new Timer(20, timer_tick3);
            bestscore = new IntSystemVariable(best_score_variable_name, EnvironmentVariableTarget.User);
            BestScore.Text = bestscore.get().ToString();
            canvas = new CustomCanvas(GameArea);

        }


        private void Game_start()
        {
            
            timer.stop();
            timer2.stop();
            timer3.stop();
            canvas.resetCanvas();
         
            player = new PlayerGun(new Point(0, 0), 3 * block_size);
            canvas.Add(player);
      
            width = GameArea.Width;
            height = GameArea.Height;

            h_count = (int)height / block_size;
            w_count = (int)width / block_size;


            player.position.X = width / 2 - player.getWidth() / 2;
            player.position.Y = block_size * (h_count - 4) + player.getHeight() / 2;

            red_disp = new Quad(new Point(0, 0), (int)width, (int)height + 50, Brushes.Red);
            red_disp.rect.Opacity = 0.5;
            canvas.setPosition(red_disp);


            green_disp = new Quad(new Point(0, 0), (int)width, (int)height + 50, Brushes.Green);
            green_disp.rect.Opacity = 0.5;
            canvas.setPosition(green_disp);



            ship_panel = new StagedImagePanel(new Point(0, block_size * (h_count - 2.5)), (int)width, 60, new List<BitmapImage>
            {
                new BitmapImage(new Uri("pack://application:,,,/Images/panel.png", UriKind.Absolute)),
                new BitmapImage(new Uri("pack://application:,,,/Images/panel2.png", UriKind.Absolute)),
                new BitmapImage(new Uri("pack://application:,,,/Images/panel3.png", UriKind.Absolute)),
                new BitmapImage(new Uri("pack://application:,,,/Images/panel4.png", UriKind.Absolute))
            });

 
            canvas.setPosition(ship_panel);
            canvas.Add(ship_panel);


            Canvas.SetZIndex(player.rect, 998);
            Canvas.SetZIndex(red_disp.rect, 999);

            
            score_label.Content = "Score: 0";
            score_label.FontSize = 18;
            score_label.Foreground = Brushes.Green;
            canvas.Add(score_label);
            canvas.setPosition(score_label, 50, block_size * (h_count - 1.3));



            life_label.Content = "Strength: " + player.lifecount;
            life_label.FontSize = 18;
            life_label.Foreground = Brushes.Green;

            canvas.Add(life_label);
            canvas.setPosition(life_label, 150, block_size * (h_count - 1.3));

            timer.start(); 
            timer2.start();
            timer3.start();
        }
        private void timer_tick1(object sender, EventArgs e)
        {
            life_label.Content = "Strength: " + player.lifecount;
            canvas.setPosition(player);
            LinkedList<Asteroid> blocks_local = new LinkedList<Asteroid>();
            foreach (Asteroid th in blocks)
            {
                th.position.Y = th.position.Y + 5;
                if (th.position.Y > height)
                {
                    canvas.Remove(th);
                    player.decrementLife(th.getDamage());
                    canvas.Add(red_disp);
                    red_disp_count = 1;
                    change_panel_image();
                } 
                else if (th.position.Y > height - 100 && Tools.hasColision(player, th))
                {
                    player.decrementLife(th.getDamage() * 2);
                    canvas.Remove(th);
                    canvas.Add(red_disp);
                    red_disp_count = 1;
                    change_panel_image();
                }
                else
                    blocks_local.AddLast(th);
                canvas.setPosition(th);
            }
            LinkedList<Quad> bonuses_local = new LinkedList<Quad>();
            foreach (Quad th in bonuses)
            {
                th.position.Y = th.position.Y + 5;
                if (th.position.Y > height)
                {
                    canvas.Remove(th);
                    if (typeof(RepairKit).IsInstanceOfType(th))
                    {
                        RepairKit rk = (RepairKit)th;
                        player.incrementLife(rk.hill_count);
                        green_disp_count = 1;
                        canvas.Add(green_disp);
                        change_panel_image();
                    }else if (typeof(DoubleDamage).IsInstanceOfType(th))
                    {
                        double_damage_count += ((DoubleDamage)th).life_time;
                    }
                }
                else
                    bonuses_local.AddLast(th);
                canvas.setPosition(th);
            }

            blocks = blocks_local;
            bonuses = bonuses_local;
            if (player.lifecount < 0)
                Game_end();
        }

        private void timer_tick2(object sender, EventArgs e)
        {
            Asteroid block = generateAsteroid();
            canvas.Add(block);
            canvas.setPosition(block);
            blocks.AddLast(block);
            if (red_disp_count == 1)
            {
                red_disp_count -= 1;
                red_disp_count = -1;
                canvas.Remove(red_disp);
            }

            if (green_disp_count == 1)
            {
                green_disp_count -= 1;
                green_disp_count = -1;
                canvas.Remove(green_disp);
            }

            GenerateBonus();
            if (double_damage_count > 0)
                double_damage_count -= 1;
        }

        private void timer_tick3(object sender, EventArgs e)
        {
            int speed = 0, damage = 0;
            Asteroid rm;
            Quad rqad;
            LinkedList<Quad> power_local = new LinkedList<Quad>();
            score_label.Content = "Score: " + score;
            foreach (Quad th in powers)
            {
                if (typeof(Bluster).IsInstanceOfType(th))
                {
                    speed = ((Bluster)th).speed;
                    damage = ((Bluster)th).damage;
                } else if (typeof(BlueBluster).IsInstanceOfType(th))
                {
                    speed = ((BlueBluster)th).speed;
                    damage = ((BlueBluster)th).damage;
                }
                th.position.Y += Math.Sin((th.getRotate() + 90)* Math.PI / 180) * speed;
                th.position.X += Math.Cos((th.getRotate() + 90)* Math.PI / 180) * speed;
                canvas.setPosition(th);
                if ((rm = Tools.hasBlockColision<Asteroid>(th, blocks)) != null)
                {
                    canvas.Remove(th);
                    
                    if (rm.decrementLifeCount(damage) <= 0)
                    {
                        blocks.Remove(rm);
                        canvas.Remove(rm);
                        switch (rm.getDamage())
                        {
                            case 1:
                                score += 10;
                                break;
                            case 3:
                                score += 20;
                                break;
                            case 10:
                                score += 50;
                                break;
                        }
                    }
                        

                }
                else if ((rqad = Tools.hasBlockColision(th, bonuses)) != null)
                {
                    canvas.Remove(th);
                    bonuses.Remove(rqad);
                    canvas.Remove(rqad);
                }
                else if (th.position.Y + th.getHeight() < -10 || th.position.X < -10 || th.position.X > width || th.position.Y > height + 50)
                    canvas.Remove(th);
                else
                    power_local.AddLast(th);

            }
            powers = power_local;
        }

      


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (!timer.isRunning())
                return;
            switch (e.Key)
            {
                case Key.Escape:
                    Game_end();
                    break;
            }
            //move_snake();
        }


        private void GenerateBonus()
        {
            Random rand = new Random();
            int x = rand.Next(4, w_count - 2);
            int y = 0;
            Point pos = new Point(block_size * x, block_size * y);
            if (rand.NextDouble() > 0.7 && player.lifecount < 50)
            {
                RepairKit rk = new RepairKit(pos, 40);
                canvas.setPosition(rk);
                canvas.Add(rk);
                bonuses.AddLast(rk);
            }else 
            if (rand.NextDouble() > 0.96)
            {
                DoubleDamage dd = new DoubleDamage(pos, 40);
                canvas.setPosition(dd);
                canvas.Add(dd);
                bonuses.AddLast(dd);
                
            }
        }
        private Asteroid generateAsteroid()
        {
            Random rand = new Random();
            int x = rand.Next(4, w_count - 7);
            int y = 0;
            Point pos = new Point(block_size * x, block_size * y);
            if (rand.NextDouble() > 0.6)
                return new Asteroid(pos, 20);
            else if (rand.NextDouble() > 0.3)
                return new SmallAsteroid(pos);
            else
                return new BigAsteroid(pos);
        }





        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            startMenu.Visibility = Visibility.Collapsed;
            Game_start();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            canvas.resetCanvas();
            Guide.Visibility = Visibility.Visible;
            startMenu.Visibility = Visibility.Collapsed;

        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Guide.Visibility = Visibility.Collapsed;
            startMenu.Visibility = Visibility.Visible;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bestscore.set(0);
            BestScore.Text = "0";
        }

        private void RadioButton_Click_2(object sender, RoutedEventArgs e)
        {
            tick_time = 80;
            timer = new Timer(tick_time, timer_tick1);
        }

        private void RadioButton_Click_3(object sender, RoutedEventArgs e)
        {
            tick_time = 60;
            timer = new Timer(tick_time, timer_tick1);
        }

        private void GameArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer.isRunning())
                return;
            double x = Mouse.GetPosition(this).X;
            double y = Mouse.GetPosition(this).Y;

            x = x - (player.getPosition().X + player.getWidth() / 2);
            y = y - (player.getPosition().Y + player.getHeight() / 2);
            
            if (x > 0.00001)
                player.setRotate(Math.Atan(y / x) * 180 / Math.PI);
            else if (x < -0.00001)
                player.setRotate((Math.Atan(y / x) - Math.PI) * 180 / Math.PI);
            else
                player.setRotate(-90);
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            block_size = 20;
        }

        private void GameArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!timer.isRunning())
                return;
            Quad q;
            double x = player.getPosition().X + player.getWidth() / 2 - 1;
            double y = player.getPosition().Y + player.getHeight() / 2;
            if (double_damage_count > 0)
                q = new BlueBluster(new Point(x, y));
            else
                q = new Bluster(new Point(x, y));
            q.setRotate(player.getRotate() - 90);
            powers.AddLast(q); 
            canvas.Add(q);
            canvas.setPosition(q);
        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            block_size = 10;
        }

        public void Game_end()
        {
            timer.stop();
            timer2.stop();
            timer3.stop();
            canvas.resetCanvas();
            blocks.Clear();
            powers.Clear();
            startMenu.Visibility = Visibility.Visible;
            this.Title = "Space Ship Defender";
            
            if (score > bestscore.get())
            {
                bestscore.set(score);
                BestScore.Text = score.ToString();
            }
        }

        public void change_panel_image()
        {
            if (player.lifecount > 75)
                ship_panel.setStage(0);
            else if (player.lifecount > 50)
                ship_panel.setStage(1);
            else if (player.lifecount > 25)
                ship_panel.setStage(2);
            else
                ship_panel.setStage(3);
        }
    }
}
