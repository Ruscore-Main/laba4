using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4Laba
{

    // (1+2)*3 ===== 12+3*
    public partial class Form1 : Form
    {
        bool openedHistory = false;
        PointF[] points;
        string nameA;
        string name;

        List<PointsS> namesA = new List<PointsS>();
        List<Polygon> names = new List<Polygon>();
        Bitmap bitmap;
        Pen[] pens = new Pen[3] { new Pen(Color.Red, 4), new Pen(Color.Green, 4), new Pen(Color.Aqua, 4) };
        public void Clear()
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            pictureBox1.Image = bitmap;
        }
        private bool IsOperation(char item)
        {
            return (item == 'A' || item == 'M' || item == 'P' || item == 'D' || item == 'C');
        }

        private void AddPoints(string name, int n, Stack<string> numbers)
        {
            // A(x,2,100,100,200,200)
            // P(y,x)
            // D(y)
            //MessageBox.Show($"{name} {n} {numbers}");
            points = new PointF[n];
            for (int i = 0; i < n; i++) {
                points[i] = new PointF(Convert.ToInt32(numbers.Pop()), Convert.ToInt32(numbers.Pop()));
            }
            namesA.Add(new PointsS(name, points));
            richTextBox1.Text += $"A({nameA},n,points) - {n} точек было добавлено\n";
        }

        private void CreatePolygon(string name, string nameA)
        {
            foreach (PointsS pointss in namesA)
            {
                if (pointss.name == nameA)
                {
                    Polygon pol = new Polygon(name, pointss.points);
                    names.Add(pol);
                    pol.Draw(bitmap, pictureBox1);
                    richTextBox1.Text += $"P({name},{nameA}) - Многоугольник был отрисован\n";
                }
            }
        }

        private void MoveTo(string name, int dx, int dy)
        {
            bool inList = false;
            Polygon copyPol = null;
            foreach (Polygon pol in names)
            {
                if (pol.name == name)
                {
                    inList = true;
                    copyPol = pol;
                }
            }
            if (inList)
            {
                DeletePolygon(copyPol.name);
                for (int i = 0; i < copyPol.points.Length; i++)
                {
                    copyPol.points[i].X += dx;
                    copyPol.points[i].Y += dy;
                }
                names.Add(copyPol);
                copyPol.Draw(bitmap, pictureBox1);
                richTextBox1.Text += $"M({name},{dx},{dy}) - Многоугольник был передвижен\n";
            }
            
        }

        private void DeletePolygon(string name)
        {
            Polygon delPol = null;
            bool inList = false;
            foreach (Polygon pol in names)
            {
                if (pol.name == name)
                {
                    delPol = pol;
                    inList = true;
                    
                }
            }
            if (inList)
            {
                names.Remove(delPol);
                Clear();
                foreach (Polygon polygon in names)
                {
                    polygon.Draw(bitmap, pictureBox1);
                }
                richTextBox1.Text += $"D({name}) - Многоугольник был удален\n";
            }
            
        }

        private void ChangeColor(string name, int color)
        {

            Pen pen = pens[color];
            bool inList = false;
            Polygon copyPol = null;
            foreach (Polygon pol in names)
            {
                if (pol.name == name)
                {
                    inList = true;
                    copyPol = pol;
                }
            }
            if (inList)
            {
                DeletePolygon(copyPol.name);
                copyPol.pen = pen;
                names.Add(copyPol);
                copyPol.Draw(bitmap, pictureBox1);
                richTextBox1.Text += $"C({name},{color}) - Многоугольник был перекрашен\n";
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.bitmap = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            richTextBox1.Visible = false;
        }



        private void textBoxInputString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                try
                {
                    char type = textBoxInputString.Text[0];
                    if (IsOperation(type))
                    {
                        string inputString = textBoxInputString.Text.Substring(1).Replace(' ', '\0').Replace('(', ' ').Replace(')', ' ').Replace(',', ' ').Trim();
                        //MessageBox.Show(inputString);
                        if (type == 'A')
                        {
                            string[] inputItems = inputString.Split(' ');
                            nameA = inputItems[0];
                            int n = Convert.ToInt32(inputItems[1]);
                            Stack<string> numbers = new Stack<string>();
                            for (int j = inputItems.Length - 1; j >= 2; j--)
                            {
                                numbers.Push(inputItems[j]);
                            }
                            AddPoints(nameA, n, numbers);

                        }
                        else if (type == 'P')
                        {
                            string[] inputItems = inputString.Split(' ');
                            name = inputItems[0];
                            nameA = inputItems[1];
                            //MessageBox.Show($"{name} {nameA}");
                            CreatePolygon(name, nameA);
                        }
                        else if (type == 'D')
                        {
                            name = inputString;
                            //MessageBox.Show($"{name}");
                            DeletePolygon(name);
                        }
                        else if (type == 'M')
                        {
                            string[] inputItems = inputString.Split(' ');
                            name = inputItems[0];
                            int dx = Convert.ToInt32(inputItems[1]), dy = Convert.ToInt32(inputItems[2]);
                            MoveTo(name, dx, dy);
                        }
                        if (type == 'C')
                        {
                            string[] inputItems = inputString.Split(' ');
                            name = inputItems[0];
                            int color = Convert.ToInt32(inputItems[1]);
                            MessageBox.Show(Convert.ToString(color));
                            if (color >= 0 && color < 3)
                            {
                                ChangeColor(name, color);
                            }
                            else
                            {
                                MessageBox.Show("Не существует такого цвета");
                            }

                        }
                        textBoxInputString.Text = "";
                    }
                }
                catch
                {
                    MessageBox.Show("Заданная строка имеент неправильный формат", "Ошибка");
                }
                
                
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (openedHistory)
            {
                openedHistory = false;
                richTextBox1.Visible = false;
            }
            else
            {
                openedHistory = true;
                richTextBox1.Visible = true;
            }
        }
    }
}

