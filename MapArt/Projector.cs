using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MapArt
{
    public partial class Projector : Form
    {
        public Projector()
        {
            InitializeComponent();
        }

        private void Projector_Load(object sender, EventArgs e)
        {
            label1.Text = "Nom   :" + global.pend.Name
                + "\nPhi0   :" + global.pend.Phi0.ToString()
                + "\nPhi1   :" + global.pend.Phi1.ToString()
                + "\nPhi2   :" + global.pend.Phi2.ToString()
                + "\nFacteur échelle   :" + global.pend.ScaleFactor.ToString()
                + "\n1er parallèle standard   :" + global.pend.StandardParallel1.ToString()
                + "\n2ème parallèle standard   :" + global.pend.StandardParallel2.ToString()
                + "\nMéridien origine   :" + global.pend.CentralMeridian.ToString()
                +"\nConstante en X   :"+global.pend.FalseEasting.ToString()
                    + "\nConstante en Y   :" + global.pend.FalseNorthing.ToString()
                + "\nZone   :" + global.pend.Zone.ToString();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                button1.Visible = true;
                button2.Enabled = false;
            }
            else
            {
                button1.Visible = false;
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s="";
            if (radioButton1.Checked)
                s = "Z1";
            if (radioButton2.Checked)
                s = "Z2";
            if (radioButton3.Checked)
                s = "Z3";
            if (radioButton4.Checked)
                s = "Z4";
            StreamReader r = File.OpenText("projection\\" + s + ".prj");
            string s2 = r.ReadLine();
            global.pend.TryParseEsriString(s2);
            StreamWriter w = new StreamWriter("projection\\defaut.prj");
            w.Write(s2);
            r.Close();
            w.Close();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            StreamReader r = File.OpenText(openFileDialog1.FileName);
            string s2 = r.ReadLine();
            global.pend.TryParseEsriString(s2);
            StreamWriter w = new StreamWriter("projection\\defaut.prj");
            w.Write(s2);
            r.Close();
            w.Close();
            Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
    }
}
