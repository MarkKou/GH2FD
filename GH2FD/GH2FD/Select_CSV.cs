//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using FlowDesigner;

//namespace GH2FD
//{
//    public partial class Select_CSV : Form
//    {
//        Set_CSV_From scf;

//        public Select_CSV(Set_CSV_From _scf)
//        {
//            InitializeComponent();
//            csvsetting = new CSV_Setting(0);
//            labellist = new List<Label>();

//            int counter = 0;

//            foreach (CSV_Group group in csvsetting.Targets)
//            {
//                Label tempgp = new Label();
//                tempgp.AutoSize = true;
//                tempgp.Location = new System.Drawing.Point(12, counter * 20 + 12);
//                tempgp.Name = group.Name;
//                tempgp.Size = new System.Drawing.Size(35, 13);
//                tempgp.TabIndex = counter;
//                tempgp.Text = group.Name;
//                labellist.Add(tempgp);

//                counter++;

//                foreach (CSV_CB item in group.Items)
//                {
//                    item.AutoSize = true;
//                    item.Location = new System.Drawing.Point(32, counter * 20 + 12);
//                    item.Size = new System.Drawing.Size(35, 13);
//                    item.TabIndex = counter;
//                    Controls.Add(item);
//                    counter++;
//                }
//                counter++;
//            }

//            Label cyclename = new Label();
//            cyclename.AutoSize = true;
//            cyclename.Location = new System.Drawing.Point(12, counter * 20 + 12);
//            cyclename.Name = csvsetting.Cycles.Name;
//            cyclename.Size = new System.Drawing.Size(35, 13);
//            cyclename.TabIndex = counter;
//            cyclename.Text = csvsetting.Cycles.Name;
//            labellist.Add(cyclename);

//            counter++;

//            foreach (CSV_CB item in csvsetting.Cycles.Items)
//            {
//                item.AutoSize = true;
//                item.Location = new System.Drawing.Point(32, counter * 20 + 12);
//                item.Size = new System.Drawing.Size(35, 13);
//                item.TabIndex = counter;
//                Controls.Add(item);
//                counter++;
//            }
//            counter++;

//            foreach (Label lb in labellist)
//            {
//                Controls.Add(lb);
//            }

//            button1.Location = new Point(12, counter * 20 + 12);
//            button2.Location = new Point(100, counter * 20 + 12);

//            this.ClientSize = new System.Drawing.Size(280, counter * 20 + 50);

//            scf = _scf;
//        }

//        List<Label> labellist;
//        CSV_Setting csvsetting;

//        private void button1_Click(object sender, EventArgs e)
//        {
//            csvsetting.Update();

//            foreach (CSV_Group group in csvsetting.Targets)
//            {
//                foreach (CSV_CB item in group.Items)
//                {
//                    richTextBox1.Text += item.Set_string + Environment.NewLine;
//                }
//            }

//            foreach (CSV_Group group in csvsetting.Targets)
//            {
//                foreach (CSV_CB item in group.Items)
//                {
//                    scf.results.Add(item.Checked);
//                }
//            }

//            scf.ExpireSolution(true);
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
//            foreach (CSV_Group group in csvsetting.Targets)
//            {
//                foreach (CSV_CB item in group.Items)
//                {
//                    item.Checked = false;
//                }
//            }
//        }

//        private void Select_CSV_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}
