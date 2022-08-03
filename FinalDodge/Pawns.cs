using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FinalDodge
{
    class Pawns
    {

        public double X { get; protected set; }
        public double Y { get; protected set; }
        public Image Pic { get; protected set; }


        public Pawns(double x, double y)
        {
            X=x;
            Y=y;

            Pic=new Image();
            Pic.Stretch = Stretch.Fill;

        }

        public void MoveObject(double x, double y)
        {
            X= x; Y=y;
            Canvas.SetLeft(Pic, x);
            Canvas.SetTop(Pic, y);

        }
    }

}
