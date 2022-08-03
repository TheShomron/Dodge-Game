using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace FinalDodge
{
    internal class Enemy : Pawns
    {
        public static BitmapImage SnorlaxPic = new BitmapImage(new Uri(@"ms-appx:///Assets/Snorlax.gif"));
        public static BitmapImage GengarPic = new BitmapImage(new Uri(@"ms-appx:///Assets/Ganger.gif"));
        public double speed = 0.5;
       

        public Enemy(double x, double y) : base(x, y)
        {
            Pic.Height = 80;
            Pic.Width = 100;
            Pic.Source=GengarPic;


        }
        public void moveEnemy(double x, double y)
        {
            MoveObject(x, y);
        }
    }
}


