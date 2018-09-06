using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace MapArt
{
    public partial class Line_analyzer : Form
    {
        public Line_analyzer()
        {
            InitializeComponent();
        }
        private void Line_analyzer_Load(object sender, EventArgs e)
        {
            IFeatureSet fs = global.linelyr.DataSet;
            IFeature f=fs.Features[0];
            Coordinate c = new Coordinate();
            double x1=0, x2=0, y1=0, y2=0, newx=0, newy=0,newz=0, min=10000, max=0,dx,dy,distance=0;
            double slope = 0, maxslope = 0;
            float d = 0;
            int n=f.NumPoints;
            x2 = f.Coordinates[n-1].X;
            y2 = f.Coordinates[n-1].Y;
            for (int i = n-2; i >0; i--)
            {
                    x1 = x2;
                    y1 = y2;

                    x2 = f.Coordinates[i].X;
                    y2 = f.Coordinates[i].Y;

                    double dix = (x2 - x1) * 111000 * Math.Cos(y1 * 3.14 / 180);
                    double diy = (y2 - y1) * 111000;

                    distance = Math.Sqrt(Math.Pow(dix, 2) + Math.Pow(diy, 2));
                    double nr = distance / 10;

                    dx = (x2 - x1) / nr;
                    dy = (y2 - y1) / nr;

                    slope = dy / dx;
                    if (slope > maxslope)
                        maxslope = slope;


                    c = new Coordinate();
                    newx = x1;
                    newy = y1;

                    for (int j = 0; j < nr-1; j++)
                    {
                        newx += dx;
                        newy += dy;
                        d += 10;
                        newz = global.demlyr.DataSet.GetNearestValue(newx, newy);
                        chart1.Series["Altitude"].Points.AddXY(d, newz);
                        if (newz < min)
                            min = newz;
                        if (newz > max)
                            max = newz;
                    }
            }
            double elevationchange = max = min;
            String ds = difficultee(distance, elevationchange, maxslope);
            label1.Text = "Distance   :" + d.ToString()
                + "\nAltutude maximale   :" + max.ToString()
                + "\nAltutude minimale   :" + min.ToString()
                +"\nDifficultee   :" + ds;
        }
        private string difficultee(double distance, double elevatch, double maxs)
        {
            double a = 0, b = 0, c = 0;
            string s = "";
            if (distance < 3000)
                a = 1;
            else if (distance < 6000)
                a = 2;
            else if (distance < 9000)
                a = 3;
            else
                a = 4;
            if (elevatch < 250)
                b = 1;
            else if (elevatch < 500)
                b = 2;
            else if (elevatch < 750)
                b = 3;
            else
                b = 4;
            if (maxs < 0.05)
                c = 1;
            else if (maxs < 0.12)
                c = 2;
            else if (maxs < 0.2)
                c = 3;
            else
                c = 4;
            double d = a + b + c;
            if (d >= 8)
                s = "difficile";
            else if (d >= 4)
                s = "moyen";
            else
                s = "facile";
            return s;
        }
        
    }
}
