using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Projections;
using System.IO;

namespace MapArt
{
    public partial class Converter : Form
    {
        ProjectionInfo p1, p2;
        public Converter()
        {
            InitializeComponent();
        }

        private void Converter_Load(object sender, EventArgs e)
        {
            p1 = KnownCoordinateSystems.Geographic.World.WGS1984;
            p2 = global.pend;

            label3.Text = "Nom   :" + p1.Name
                + "\nPhi0   :" + p1.Phi0.ToString()
                + "\nPhi1   :" + p1.Phi1.ToString()
                + "\nPhi2   :" + p1.Phi2.ToString()
                + "\nFacteur échelle   :" + p1.ScaleFactor.ToString()
                + "\n1er parallèle standard   :" + p1.StandardParallel1.ToString()
                + "\n2ème parallèle standard   :" + p1.StandardParallel2.ToString()
                + "\nMéridien origine   :" + p1.CentralMeridian.ToString()
                + "\nConstante en X   :" + p1.FalseEasting.ToString()
                    + "\nConstante en Y   :" + p1.FalseNorthing.ToString()
                + "\nZone   :" + p1.Zone.ToString();

            label6.Text = "Nom   :" + p2.Name
                + "\nPhi0   :" + p2.Phi0.ToString()
                + "\nPhi1   :" + p2.Phi1.ToString()
                + "\nPhi2   :" + p2.Phi2.ToString()
                + "\nFacteur échelle   :" + p2.ScaleFactor.ToString()
                + "\n1er parallèle standard   :" + p2.StandardParallel1.ToString()
                + "\n2ème parallèle standard   :" + p2.StandardParallel2.ToString()
                + "\nMéridien origine   :" + p2.CentralMeridian.ToString()
                + "\nConstante en X   :" + p2.FalseEasting.ToString()
                    + "\nConstante en Y   :" + p2.FalseNorthing.ToString()
                + "\nZone   :" + p2.Zone.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader r = File.OpenText(openFileDialog1.FileName);
                string s2 = r.ReadLine();
                p1.TryParseEsriString(s2);
                r.Close();

                label3.Text = "Nom   :" + p1.Name
                + "\nPhi0   :" + p1.Phi0.ToString()
                + "\nPhi1   :" + p1.Phi1.ToString()
                + "\nPhi2   :" + p1.Phi2.ToString()
                + "\nFacteur échelle   :" + p1.ScaleFactor.ToString()
                + "\n1er parallèle standard   :" + p1.StandardParallel1.ToString()
                + "\n2ème parallèle standard   :" + p1.StandardParallel2.ToString()
                + "\nMéridien origine   :" + p1.CentralMeridian.ToString()
                + "\nConstante en X   :" + p1.FalseEasting.ToString()
                    + "\nConstante en Y   :" + p1.FalseNorthing.ToString()
                + "\nZone   :" + p1.Zone.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader r = File.OpenText(openFileDialog1.FileName);
                string s2 = r.ReadLine();
                p2.TryParseEsriString(s2);
                r.Close();

                label6.Text = "Nom   :" + p2.Name
                + "\nPhi0   :" + p2.Phi0.ToString()
                + "\nPhi1   :" + p2.Phi1.ToString()
                + "\nPhi2   :" + p2.Phi2.ToString()
                + "\nFacteur échelle   :" + p2.ScaleFactor.ToString()
                + "\n1er parallèle standard   :" + p2.StandardParallel1.ToString()
                + "\n2ème parallèle standard   :" + p2.StandardParallel2.ToString()
                + "\nMéridien origine   :" + p2.CentralMeridian.ToString()
                + "\nConstante en X   :" + p2.FalseEasting.ToString()
                    + "\nConstante en Y   :" + p2.FalseNorthing.ToString()
                + "\nZone   :" + p2.Zone.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double[] xy = new double[2];
                double[] z = new double[1];
                xy[0] = Convert.ToDouble(textBox1.Text);
                xy[1] = Convert.ToDouble(textBox2.Text);
                z[0] = 0;
                Reproject.ReprojectPoints(xy, z, p1, p2, 0, 1);
                textBox4.Text = xy[0].ToString();
                textBox3.Text = xy[1].ToString();
            }
            catch
            {
                MessageBox.Show("veuillez entrer les coordonnées du point");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double[] xy = new double[2];
                double[] z = new double[1];
                xy[0] = Convert.ToDouble(textBox4.Text);
                xy[1] = Convert.ToDouble(textBox3.Text);
                z[0] = 0;
                Reproject.ReprojectPoints(xy, z, p2, p1, 0, 1);
                textBox1.Text = xy[0].ToString();
                textBox2.Text = xy[1].ToString();
            }
            catch
            {
                MessageBox.Show("veuillez entrer les coordonnées du point");
            }
        }
    }
}
