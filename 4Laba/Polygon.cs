using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace _4Laba
{
    class Polygon
    {
        public string name;
        public PointF[] points;
        public Polygon(string name)
        {
            this.name = name;
        }
        public Polygon(string name, PointF[] points)
        {
            this.name = name;

            this.points = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                this.points[i].X = points[i].X;
                this.points[i].Y = points[i].Y;
            }
        }
        public void Draw(Bitmap bitmap, PictureBox pictureBox)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawPolygon(new Pen(Color.Red, 4), points);
            pictureBox.Image = bitmap;
        }
       
    }
}
