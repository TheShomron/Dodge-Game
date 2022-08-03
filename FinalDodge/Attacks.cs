using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace FinalDodge
{
    internal class Attacks : Pawns
    {
        public static BitmapImage fireUp = new BitmapImage(new Uri(@"ms-appx:///Assets/fireball-up.gif"));
        public static BitmapImage fireDown = new BitmapImage(new Uri(@"ms-appx:///Assets/fireball-down.gif"));
        public static BitmapImage fireRight = new BitmapImage(new Uri(@"ms-appx:///Assets/fireball-Right.gif"));
        public static BitmapImage fireLeft = new BitmapImage(new Uri(@"ms-appx:///Assets/fireball- left.gif"));

        public static BitmapImage waterUP = new BitmapImage(new Uri(@"ms-appx:///Assets/waterball-up.gif"));
        public static BitmapImage waterDown = new BitmapImage(new Uri(@"ms-appx:///Assets/waterball-down.gif"));
        public static BitmapImage waterRight = new BitmapImage(new Uri(@"ms-appx:///Assets/waterball-right.gif"));
        public static BitmapImage waterLeft = new BitmapImage(new Uri(@"ms-appx:///Assets/waterball-left.gif"));

        private int Dir;
        private double speed;


        public Attacks(double x, double y, int dir, int num)
                 : base(x, y)
        {
            
            



            Dir = dir;
            speed = 5;

            //אתחול תמונה לכיוון
            if (num==1)
            {
                switch (dir)
                {
                    case 2:
                        Pic.Source = fireUp;
                        Dir = 2;

                        Pic.Height = 110;
                        Pic.Width = 110;

                        X-= 40;
                        Y-= 30;
                        break;
                    case 3:
                        Pic.Source = fireDown;
                        Dir = 3;

                        Pic.Height = 110;
                        Pic.Width = 110;

                        X-= 40;
                        break;
                    case 0:
                        Pic.Source = fireRight;
                        Dir = 0;

                        Pic.Height = 110;
                        Pic.Width = 110;

                        Y-= 30;
                        break;
                    case 1:
                        Pic.Source = fireLeft;
                        Dir = 1;

                        Pic.Height = 110;
                        Pic.Width = 110;

                        Y-= 30;
                        X-= 40;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (dir)
                {
                    case 2:
                        Pic.Source = waterUP;
                        Dir = 2;

                        Pic.Height = 70;
                        Pic.Width = 70;

                        X-= 20;
                        Y-=20;
                        break;
                    case 3:
                        Pic.Source = waterDown;
                        Dir = 3;

                        Pic.Height = 70;
                        Pic.Width = 70;

                        X-= 20;

                        break;
                    case 0:
                        Pic.Source = waterRight;
                        Dir = 0;

                        Pic.Height = 70;
                        Pic.Width = 70;

                        Y-= 15;
                        break;
                    case 1:
                        Pic.Source = waterLeft;
                        Dir = 1;

                        Pic.Height = 70;
                        Pic.Width = 70;

                        Y-= 15;
                        X-= 40;
                        break;
                    default:
                        break;
                }
            }
            MoveArrow();
        }

        public void MoveArrow()
        {
            switch (Dir)
            {
                case 2:
                    MoveObject(X, Y - speed);
                    break;
                case 3:
                    MoveObject(X, Y + speed);
                    break;
                case 0:
                    MoveObject(X + speed, Y);
                    break;
                case 1:
                    MoveObject(X - speed, Y);
                    break;
                default:
                    break;
            }
        }
    }
}



