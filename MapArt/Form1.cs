using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Topology;
using DotSpatial.Data;
using DotSpatial.Projections;
using System.IO;
using DotSpatial.Controls.Header;
using DotSpatial;
using System.ComponentModel.Composition;

namespace MapArt
{
    public partial class Form1 : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl Shell;
        //drawing line
        bool digitizingbool = false;
        List<Coordinate> digitizedpoints = new List<Coordinate>();
        List<Coordinate> extractedpoints = new List<Coordinate>();
        public static List<System.Drawing.Point> newline = new List<System.Drawing.Point>();
        System.Drawing.Point newpoint_line;
        //type
        string digitizingtype = "";
        //shader
        bool shaderbool = true;
        //dem
        IMapRasterLayer dem = null;
        //mesure
        Mesure mesureform = new Mesure();
        Coordinate c1 = new Coordinate();
        


        public Form1()
        {
            InitializeComponent();
            Shell = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            appManager1.LoadExtensions();
            StreamReader r = File.OpenText("projection\\defaut.prj");
            global.pend.TryParseEsriString(r.ReadLine());
            r.Close();
        }

        private void toolStripButtonligne_Click(object sender, EventArgs e)
        {
            
        }

        private void map1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((digitizingbool)&global.digitizingbool)
            {
                Coordinate c = new Coordinate();
                System.Drawing.Point p = new System.Drawing.Point();
                p.X = e.X;
                p.Y = e.Y;
                c = map1.PixelToProj(p);
                digitizedpoints.Add(c);
                digitizingbool = false;

                Feature f=null;
                if (digitizingtype == "ligne")
                {
                    f = new Feature(FeatureType.Line, digitizedpoints);
                }
                if (digitizingtype == "points")
                {
                    f = new Feature(FeatureType.MultiPoint, digitizedpoints);
                }
                if (digitizingtype == "polygone")
                {
                    f = new Feature(FeatureType.Polygon, digitizedpoints);
                }

                FeatureSet fs = new FeatureSet();
                fs.AddFeature(f);

                fs.Projection = map1.Projection;
                fs.Name = global.layername;
                map1.Layers.Add(fs);
                
            }
        }

        private void map1_MouseClick(object sender, MouseEventArgs e)
        {
            if ((digitizingbool)&global.digitizingbool)
            {
                Coordinate c = new Coordinate();
                System.Drawing.Point p = new System.Drawing.Point();
                p.X = e.X;
                p.Y = e.Y;
                c = map1.PixelToProj(p);
                digitizedpoints.Add(c);
                newline.Add(p);
                map1.Invalidate();
            }
            if (global.mesurebool)
            {
                global.mesurebool2 = true;
                System.Drawing.Point p = new System.Drawing.Point();
                p.X = e.X;
                p.Y = e.Y;
                c1 = map1.PixelToProj(p);
                newline.Add(p);
            }
            
        }
        /// <summary>
        /// ////////////////////////////////////////////
        /// //fonctions
        /// </summary>
        private void tracer_ligne()
        {
            newline = new List<System.Drawing.Point>();
            newpoint_line = new System.Drawing.Point();
            global.digitizingbool = false;
            digitizingtype = "ligne";
            global.layername = "ligne";
            Layername ln = new Layername();
            ln.Show();
            digitizingbool = true;
            digitizedpoints = new List<Coordinate>();
            map1.FunctionMode = FunctionMode.None;
        }
        private void tracer_points()
        {
            newline = new List<System.Drawing.Point>();
            newpoint_line = new System.Drawing.Point();
            global.digitizingbool = false;
            digitizingtype = "points";
            global.layername = "points";
            Layername ln = new Layername();
            ln.Show();
            digitizingbool = true;
            digitizedpoints = new List<Coordinate>();
            map1.FunctionMode = FunctionMode.None;
        }
        private void tracer_polygone()
        {
            newline = new List<System.Drawing.Point>();
            newpoint_line = new System.Drawing.Point();
            global.digitizingbool = false;
            digitizingtype = "polygone";
            global.layername = "polygone";
            Layername ln = new Layername();
            ln.Show();
            digitizingbool = true;
            digitizedpoints = new List<Coordinate>();
            map1.FunctionMode = FunctionMode.None;
        }
        private void line_analyzer()
        {
            bool a = false;
            global.demlyr = null;
            foreach (IMapLayer demlyr in map1.Layers)
            {
                if (demlyr.IsSelected)
                {
                    a = true;
                }
            }
            if (a)
            {
                try
                {
                    global.linelyr = (MapLineLayer)map1.Layers.SelectedLayer;
                    foreach (IMapLayer demlyr in map1.Layers)
                    {
                        try
                        {
                            global.demlyr = (MapRasterLayer)demlyr;
                        }
                        catch
                        {
                            //do nothing
                        }
                    }
                    if (global.demlyr == null)
                        MessageBox.Show("chargez un DEM pour appliquer l'analyse");
                    else
                    {
                        Line_analyzer la = new Line_analyzer();
                        la.Show();
                    }
                }
                catch
                {
                    MessageBox.Show("veuillez choisir une ligne dans la legende pour appliquer l'analyse");
                }
            }
        }
        private void shader_effect()
        {
            foreach (IMapLayer lyr in map1.Layers)
            {
                try
                {
                    dem = (MapRasterLayer)lyr;
                }
                catch
                {
                    //
                }
            }
                if (dem != null)
                {
                    if (shaderbool)
                    {
                        dem.Symbolizer.ShadedRelief.ElevationFactor = 1;
                        dem.Symbolizer.ShadedRelief.IsUsed = true;
                        dem.WriteBitmap();
                        shaderbool = false;
                    }
                    else
                    {
                        dem.Symbolizer.ShadedRelief.ElevationFactor = 0;
                        dem.Symbolizer.ShadedRelief.IsUsed = false;
                        dem.WriteBitmap();
                        shaderbool = true;
                    }
                }
            
        }
        private void save_shape()
        {
            bool b=false;
            foreach (IMapLayer a in map1.Layers)
            {
                if (a.IsSelected)
                    b = true;
            }
            if (b)
            {
            try
            {
                FeatureSet fs = (FeatureSet)map1.Layers.SelectedLayer.DataSet;
                saveFileDialogshape.ShowDialog();
            }
            catch
            {
                MessageBox.Show("selectionnez un shape!");
            }
            }
            else
                MessageBox.Show("selectionnez un shape!");
        }
        private void ouvrir_ficher()
        {
            try
            {
                map1.AddLayer();
            }
            catch
            {
                //do nothing
            }
            if (dem == null)
            {
                foreach (IMapLayer lyr in map1.Layers)
                {
                    try
                    {
                        dem = (IMapRasterLayer)lyr;
                    }
                    catch
                    {
                        //
                    }
                }
            }
        }
        private void save_as_project()
        {
            saveFileDialog1.Filter = appManager1.SerializationManager.SaveDialogFilterText;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                appManager1.SerializationManager.OpenProject(saveFileDialog1.FileName);
            }
        }
        private void option_projection()
        {
            Projector p = new Projector();
            p.Show();
        }
        private void ouvrir_projet()
        {
            openFileDialog1.Filter = appManager1.SerializationManager.SaveDialogFilterText;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                appManager1.SerializationManager.OpenProject(openFileDialog1.FileName);
                if (dem == null)
                {
                    foreach (IMapLayer lyr in map1.Layers)
                    {
                        try
                        {
                            dem = (IMapRasterLayer)lyr;
                        }
                        catch
                        {
                            //
                        }
                    }
                }
            }

        }
        private void open_convertion()
        {
            Converter c = new Converter();
            c.Show();
        }
        private void mesurer()
        {
            
            if (!global.mesurebool)
            {
                global.mesurebool = true;
                mesureform.Show();
            }
            
        }
        private void mode_normal()
        {
            if (global.mesurebool2)
            {
                global.mesurebool = false;
                global.mesurebool2 = false;
                newline = new List<System.Drawing.Point>();
                newpoint_line = new System.Drawing.Point();
                map1.Invalidate();
            }
            map1.FunctionMode = FunctionMode.None;
        }
        /// <summary>
        /// ///////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// //fonctions
        

        private void ouvrirUnFichierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ouvrir_ficher();
        }

        private void ligneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tracer_ligne();
        }

        private void pointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tracer_points();
        }

        private void polygoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tracer_polygone();
        }

        private void analyseurDeLigneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            line_analyzer();
        }


        private void map1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((digitizingbool) & global.digitizingbool & (newline != null) )
            {
                newpoint_line = e.Location;
                map1.Invalidate();
            }
            if (global.mesurebool2)
            {
                newpoint_line = e.Location;
                map1.Invalidate();
                System.Drawing.Point p2 = new System.Drawing.Point();
                p2 = e.Location;
                Coordinate c2 = map1.PixelToProj(p2);
                double dx = (c2.X - c1.X) * 111000 * Math.Cos(c1.Y*3.14/180);
                double dy = (c2.Y - c1.Y) * 111000;
                double distance = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                double gisement;
                if (distance == 0)
                    gisement = 0;
                else if (dx == 0)
                    gisement = 3200;
                else
                    gisement = 2 * 3200 / 3.14 * Math.Atan(dx / (distance + dy));
                mesureform.textBox1.Text = distance.ToString();
                mesureform.textBox2.Text = gisement.ToString();
            }
            
            System.Drawing.Point p = new System.Drawing.Point();
            p.X = e.X;
            p.Y = e.Y;
            Coordinate c = map1.PixelToProj(p);
            ProjectionInfo pstar = KnownCoordinateSystems.Geographic.World.WGS1984;
            double[] xy = new double[2];
            double[] z = new double[1];
            xy[0] = c.X;
            xy[1] = c.Y;
            z[0] = 0;
            if (dem != null)
                z[0] = dem.DataSet.GetNearestValue(c);
            Reproject.ReprojectPoints(xy, z, pstar, global.pend, 0, 1);
            Label1.Text = "  Lon=  " + c.X.ToString() + "  Lat=  " +c.Y.ToString()+"    X="+xy[0].ToString()+"  Y="+xy[1].ToString()+"  Z="+z[0].ToString();
        }

        private void shaderEffectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shader_effect();
        }

        private void enregisterSousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_as_project();
        }

        private void projectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            option_projection();
        }

        private void ouvrirProjetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ouvrir_projet();
        }

        private void zOOMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            map1.ZoomIn();
        }

        private void zOOMToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            map1.ZoomOut();
        }

        private void avantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map1.ZoomToPrevious();
        }

        private void suivantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map1.ZoomToNext();
        }

        private void mAXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map1.ZoomToMaxExtent();
        }

        private void saveFileDialogshape_FileOk(object sender, CancelEventArgs e)
        {
            FeatureSet fs = (FeatureSet)map1.Layers.SelectedLayer.DataSet;
            fs.SaveAs(saveFileDialogshape.FileName, true);
        }

        private void enregitrerShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_shape();
        }

        private void convertisseurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_convertion();
        }

        private void map1_Paint(object sender, PaintEventArgs e)
        {
            if ((digitizingbool) & global.digitizingbool & ((digitizingtype == "polygone") || (digitizingtype == "ligne")))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                if (newline != null)
                {
                    if (newline.Count > 1)
                    {
                        e.Graphics.DrawLines(Pens.Blue, newline.ToArray());
                    }
                    if (newline.Count > 0)
                    {
                        using (Pen dashed_pen = new Pen(Color.Blue))
                        {
                            dashed_pen.DashPattern = new float[] { 3, 3 };
                            e.Graphics.DrawLine(dashed_pen, newline[newline.Count - 1], newpoint_line);
                        }
                    }
                }
            }
            if (global.mesurebool2)
            {
                using (Pen dashed_pen = new Pen(Color.Blue))
                {
                    dashed_pen.DashPattern = new float[] { 3, 3 };
                    e.Graphics.DrawLine(dashed_pen, newline[newline.Count - 1], newpoint_line);
                }
            }
            
        }

        private void mesureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mesurer();
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode_normal();
        }

        private void toolStripButtonouvrirprojet_Click(object sender, EventArgs e)
        {
            ouvrir_projet();
        }

        private void toolStripButtonsaveproject_Click(object sender, EventArgs e)
        {
            save_as_project();
        }

        private void toolStripButtonajouter_Click(object sender, EventArgs e)
        {
            ouvrir_ficher();
        }

        private void toolStripButtonzoomin_Click(object sender, EventArgs e)
        {
            map1.ZoomIn();
        }

        private void toolStripButtonzoomout_Click(object sender, EventArgs e)
        {
            map1.ZoomOut();
        }

        private void toolStripButtonhome_Click(object sender, EventArgs e)
        {
            map1.ZoomToMaxExtent();
        }

        private void toolStripButtonzoomavant_Click(object sender, EventArgs e)
        {
            map1.ZoomToPrevious();
        }

        private void toolStripButtonzoomsuivant_Click(object sender, EventArgs e)
        {
            map1.ZoomToNext();
        }

        private void toolStripButtonzoommode_Click(object sender, EventArgs e)
        {
            map1.FunctionMode = FunctionMode.ZoomIn;
        }

        private void toolStripButtonzoommodem_Click(object sender, EventArgs e)
        {
            map1.FunctionMode = FunctionMode.ZoomOut;
        }

        private void toolStripButtonmain_Click(object sender, EventArgs e)
        {
            map1.FunctionMode = FunctionMode.Pan;
        }

        private void toolStripButtonnormal_Click(object sender, EventArgs e)
        {
            mode_normal();
        }

        private void toolStripButtoninfomode_Click(object sender, EventArgs e)
        {
            map1.FunctionMode = FunctionMode.Info;
        }

        private void toolStripButtonligneanalyseur_Click(object sender, EventArgs e)
        {
            line_analyzer();
        }

        private void toolStripButtonconvertisseur_Click(object sender, EventArgs e)
        {
            open_convertion();
        }

        private void toolStripButtonmesure_Click(object sender, EventArgs e)
        {
            mesurer();
        }

        private void toolStripButtonshader_Click(object sender, EventArgs e)
        {
            shader_effect();
        }

        private void toolStripButtontracerligne_Click(object sender, EventArgs e)
        {
            tracer_ligne();
        }

        private void toolStripButtontracerpoints_Click(object sender, EventArgs e)
        {
            tracer_points();
        }

        private void toolStripButtontracerpolygone_Click(object sender, EventArgs e)
        {
            tracer_polygone();
        }
    }
}
