using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapArt
{
    public partial class Layername : Form
    {
        public Layername()
        {
            InitializeComponent();
        }

        private void Layername_Load(object sender, EventArgs e)
        {
            textBox1.Text = global.layername;
            if (global.layername == "ligne") 
            {
                label1.Text="nom de la ligne:";
                label2.Text="Entrez le nom de la ligne puis sur OK\ncliquez sur la carte pour tracer et faite un double clic pour terminer";
            }
            if (global.layername == "points")
            {
                label1.Text = "nom des points:";
                label2.Text = "Entrez le nom des points puis sur OK\ncliquez sur la carte pour tracer un point et faite un double clic pour terminer";
            }
            if (global.layername == "polygone")
            {
                label1.Text = "nom du polygone:";
                label2.Text = "Entrez le nom du polygone puis sur OK\ncliquez sur la carte pour tracer un point et faite un double clic pour terminer";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            global.layername = textBox1.Text;
            global.digitizingbool = true;
            Close();
        }
    }
}
