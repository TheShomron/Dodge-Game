using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace FinalDodge
{
    internal class Player : Pawns
    {
        public static BitmapImage CharmanderPic = new BitmapImage(new Uri(@"ms-appx:///Assets/Charmander.gif"));
        public static BitmapImage SquirtlePic = new BitmapImage(new Uri(@"ms-appx:///Assets/Squirtle.gif"));
        public static BitmapImage BulbasaurPic = new BitmapImage(new Uri(@"ms-appx:///Assets/Bulbasaur.gif"));
        

        public Player(double x, double y ) : base(x, y)
        {

            Pic.Height = 60;
            Pic.Width = 50;
            Pic.Source=CharmanderPic;
        }
        public void movePlayer(double x, double y)
        {
            MoveObject(x, y);
        }

    }
}
