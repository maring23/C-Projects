using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyClassCollection;

namespace SpencerFlores_ADBMS_Assignment1
{
    public partial class Form1 : Form
    {
        MySQLDBUtilities dbutil = new MySQLDBUtilities();
        HelperMethods hm = new HelperMethods();
        int a = 1;

        public Form1()
        {
            InitializeComponent();
            dbutil.OpenConnection();
            GenerateAll();
        }

        public void GenerateAll()
        {
            Panel panels = new Panel();
            panels.Name = "MainPanel";
            panels.Size = new System.Drawing.Size(865, 756);
            panels.Dock = System.Windows.Forms.DockStyle.Fill;
            panels.TabIndex = 0;
            panels.AutoScroll = true;

            int Locationx = 10;
            int Locationy = 10;


            string query = "SELECT * FROM tblcontestant;";
            DataTable dts = dbutil.SelectTable(query);

            foreach (DataRow rs in dts.Rows)
            {
                int i = 1;

                GroupBox gb = new GroupBox();
                PictureBox pb = new PictureBox();
                Label lbname = new Label();
                Label lbinfo = new Label();
                Button btn = new Button();

                if (Locationx == 10)
                {
                    gb.Name = "GroupBox" + a;
                    gb.Location = new Point(Locationx, Locationy);
                    gb.Size = new Size(406, 348);
                    gb.Font = new Font("Segoe UI", 15);
                    gb.FlatStyle = FlatStyle.Standard;
                    gb.BringToFront();
                    gb.Visible = true;
                    gb.Text="Contestant" + rs["ID"].ToString();
                    
                    string path = rs["ImagePath"].ToString();
                    path.Replace("\\", "\\\\");
                    pb.Name = "PictureBox" + i;
                    pb.Location = new Point(93, 30);
                    pb.Size = new System.Drawing.Size(220, 225);
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Image = hm.GetCopyImage(path);
                    gb.Controls.Add(pb);

                    string change = rs["Name"].ToString();
                    int yy = 258;
                    for (int x = 0; x < 2; x++)
                    {
                        lbname = new Label();
                        lbname.Name = "LabelName" + i;
                        lbname.Text = change;
                        lbname.Location = new System.Drawing.Point(128,yy);
                        lbname.AutoSize = true;
                        lbname.Font = new Font("Segoe UI", 12);
                        gb.Controls.Add(lbname);

                        change = rs["Information"].ToString();
                        yy += 20;
                    }

                    //button
                    btn.Name = "btnScore" + i;
                    btn.Text = "SCORE";
                    btn.Location = new System.Drawing.Point(150, 302);
                    btn.Font = new Font("Segoe UI", 12);
                    btn.AutoSize = true;
                    btn.Click += new System.EventHandler(btn_Click);

                    gb.Controls.Add(btn);

                    panels.Controls.Add(gb);

                    a += 1;

                    Locationx = 420;
                }

                else if (Locationx != 10)
                {
                    gb.Name = "GroupBox" + a;
                    gb.Location = new Point(Locationx, Locationy);
                    gb.Size = new Size(406, 348);
                    gb.Font = new Font("Segoe UI", 15);
                    gb.FlatStyle = FlatStyle.Standard;
                    gb.Visible = true;
                    gb.BringToFront();
                    gb.Text = "Contestant " + rs["ID"].ToString();

                    string path = rs["ImagePath"].ToString();
                    path.Replace("\\", "\\\\");
                    pb.Name = "PictureBox" + i;
                    pb.Location = new Point(93, 30);
                    pb.Size = new System.Drawing.Size(220, 225);
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Image = hm.GetCopyImage(path);
                    gb.Controls.Add(pb);

                    string change = rs["Name"].ToString();
                    int yy = 258;
                    for (int x = 0; x < 2; x++)
                    {
                        lbname = new Label();
                        lbname.Name = "LabelName" + i;
                        lbname.Text = change;
                        lbname.Location = new System.Drawing.Point(128, yy);
                        lbname.AutoSize = true;
                        lbname.Font = new Font("Segoe UI", 12);
                        gb.Controls.Add(lbname);

                        change = rs["Information"].ToString();
                        yy += 20;
                    }

                    btn.Name = "btnScore" + i;
                    btn.Text = "SCORE";
                    btn.Location = new System.Drawing.Point(150, 302);
                    btn.Font = new Font("Segoe UI", 12);
                    btn.AutoSize = true;
                    btn.Click += new System.EventHandler(btn_Click);

                    gb.Controls.Add(btn);

                    panels.Controls.Add(gb);

                    a += 1;

                    Locationx = 10;
                    Locationy += 350;
                }
                i++;
            }

            Button btnSubmit = new Button();
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Text = "SUBMIT";
            btnSubmit.Location = new System.Drawing.Point(750, Locationy);
            btnSubmit.Font = new Font("Segoe UI", 12);
            btnSubmit.AutoSize = true;

            panels.Controls.Add(btnSubmit);
            this.Controls.Add(panels);
        }

        void btn_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            frm2.ShowDialog();
            this.Close();
        }
    }
}
