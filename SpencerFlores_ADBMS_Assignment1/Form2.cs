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
    public partial class Form2 : Form
    {
        Label lbl = new Label();
        TextBox txt = new TextBox();
        MySQLDBUtilities dbutil = new MySQLDBUtilities();
        HelperMethods hm = new HelperMethods();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            int locationX = 10;
            string query = "SELECT * FROM tblCriteria;";
            DataTable dts = dbutil.SelectTable(query);
            
            int i = 1;
            foreach (DataRow rs in dts.Rows)
            {
                lbl = new Label();
                lbl.Name = "LabelName" + i;
                lbl.Text = rs["CriteriaName"].ToString();
                lbl.Location = new System.Drawing.Point(10, 50 * i);
                lbl.AutoSize = true;
                lbl.Font = new Font("Segoe UI", 12);

                Controls.Add(lbl);

                txt = new TextBox();
                txt.Name = "txtName" + i;
                txt.Location = new System.Drawing.Point(70, 49 * i);
                txt.AutoSize = true;
                txt.Font = new Font("Segoe UI", 12);

                Controls.Add(txt);
                i++;
            }
        }
    }
}